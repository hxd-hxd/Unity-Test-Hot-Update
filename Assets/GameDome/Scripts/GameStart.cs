using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Framework;
using Framework.YooAssetExpress;
using global::YooAsset;

namespace Test.TestYooAsset
{
    public class GameStart : MonoBehaviour
    {
        const float KB = 1024f;
        const float MB = 1048576f;
        const float GB = 1073741824f;
        const float TB = 1099511627776f;

        [Header("资源下载服务器地址")]
        //string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
        //string hostServerIP = "http://127.0.0.1";
        public string hostServerIP = "http://10.0.0.29";
        [Header("资源在服务器上的根路径")]
        public string resPath = "CDN";      // 资源在服务器上的根路径
        [Header("版本号路径")]
        public string gameVersion = "v1.0"; // 要更新的版本号
        // 资源包名，可能会有多个资源包
        [Header("资源包名，可能会有多个资源包")]
        public string packageName = "DefaultPackage";

        /// <summary>
        /// 资源系统模式
        /// </summary>
        [Header("资源系统模式")]
        public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;

        /// <summary>
        /// 最大尝试下载次数
        /// </summary>
        [Header("最大尝试下载次数")]
        [SerializeField] private int maxTryDownloadNum = 3;
        /// <summary>
        /// 尝试下载次数
        /// </summary>
        [Header("尝试下载次数")]
        [SerializeField] private int tryDownloadNum = 0;


        /// <summary>
        /// 包裹的版本信息
        /// </summary>
        public string PackageVersion { set; get; }

        /// <summary>
        /// 下载器
        /// </summary>
        public PatchDownloaderOperation Downloader { set; get; }


        void Start()
        {
            YooAssets.Initialize();
            YooAssets.SetOperationSystemMaxTimeSlice(30);

            StartCoroutine(InitPackage());
        }


        #region 资源热更流程

        // 初始化资源包
        private IEnumerator InitPackage()
        {
            yield return new WaitForSeconds(1);

            EPlayMode _playMode = PlayMode;

            Debug.Log("------------------------------------创建默认的资源包------------------------------------");

            // 创建默认的资源包
            AssetsPackage package = YooAssets.TryGetAssetsPackage(packageName);
            if (package == null)
            {
                // 没有则创建
                package = YooAssets.CreateAssetsPackage(packageName);
                YooAssets.SetDefaultAssetsPackage(package);
            }

            InitializationOperation initializationOperation = null;
            switch (_playMode)
            {
                // 编辑器下的模拟模式
                case EPlayMode.EditorSimulateMode:
                    {
                        var createParameters = new EditorSimulateModeParameters();
                        createParameters.SimulatePatchManifestPath = EditorSimulateModeHelper.SimulateBuild(packageName);
                        initializationOperation = package.InitializeAsync(createParameters);
                    }
                    break;

                // 单机运行模式
                case EPlayMode.OfflinePlayMode:
                    {
                        OfflinePlayModeParameters createParameters = new OfflinePlayModeParameters();
                        createParameters.DecryptionServices = new GameDecryptionServices();
                        initializationOperation = package.InitializeAsync(createParameters);
                    }
                    break;

                // 联机运行模式
                case EPlayMode.HostPlayMode:
                    {
                        HostPlayModeParameters createParameters = new HostPlayModeParameters();
                        createParameters.DecryptionServices = new GameDecryptionServices();
                        createParameters.QueryServices = new GameQueryServices();
                        createParameters.DefaultHostServer = GetHostServerURL();
                        createParameters.FallbackHostServer = GetHostServerURL();
                        initializationOperation = package.InitializeAsync(createParameters);
                    }
                    break;
                default:
                    break;
            }

            yield return initializationOperation;

            // 初始化成功开始更新版本
            if (package.InitializeStatus == EOperationStatus.Succeed)
            {
                Debug.Log("------------------------------------获取最新的资源版本 !------------------------------------");

                yield return GetStaticVersion();
            }
            else
            {
                Debug.Log("创建默认的资源包失败");
                Debug.LogWarning($"{initializationOperation.Error}");
            }
        }

        // 获取资源更新版本
        private IEnumerator GetStaticVersion()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            AssetsPackage package = YooAssets.GetAssetsPackage(packageName);
            UpdatePackageVersionOperation operation = package.UpdatePackageVersionAsync();
            yield return operation;

            if (operation.Status == EOperationStatus.Succeed)
            {
                PackageVersion = operation.PackageVersion;

                Debug.Log("------------------------------------更新资源清单！------------------------------------");

                yield return UpdateManifest();
            }
            else
            {
                Debug.LogWarning("获取资源更新版本失败");
                Debug.LogWarning(operation.Error);
            }
        }

        // 更新资源清单！
        private IEnumerator UpdateManifest()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            var package = YooAssets.GetAssetsPackage(packageName);
            var operation = package.UpdatePackageManifestAsync(PackageVersion);
            yield return operation;


            if (operation.Status == EOperationStatus.Succeed)
            {
                Debug.Log("------------------------------------创建补丁下载器！------------------------------------");

                yield return CreateDownloader();
            }
            else
            {
                Debug.LogWarning("创建补丁下载器失败");
                Debug.LogWarning(operation.Error);
            }
        }

        // 创建补丁下载器！
        IEnumerator CreateDownloader()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            tryDownloadNum++;

            int downloadingMaxNum = 10;
            int faileTryAgain = 3;
            Downloader = YooAssets.CreatePatchDownloader(downloadingMaxNum, faileTryAgain);

            if (Downloader.TotalDownloadCount == 0)
            {
                Debug.Log("没有找到任何下载文件！");

                DownloadOver();
            }
            else
            {
                Debug.Log($"总共 {Downloader.TotalDownloadCount} 个文件需要下载!");

                // 处理更新信息
                // 注意：开发者需要在下载前检测磁盘空间不足
                string totalSizeMB = ToMB(Downloader.TotalDownloadBytes);
                Debug.Log($"发现更新补丁文件，总数 {Downloader.TotalDownloadCount} 个总大小 {totalSizeMB}MB");

                yield return BeginDownload();

            }
        }

        // 开始处理下载
        private IEnumerator BeginDownload()
        {
            // 开始下载
            Downloader.OnStartDownloadFileCallback = (string fileName, long sizeBytes) =>
            {
                Log.Green($"开始下载资源：{fileName}  大小 {ToMB(sizeBytes)}MB");
            };

            // 下载失败
            Downloader.OnDownloadErrorCallback = (fileName, error) =>
            {
                Log.Error($"下载文件失败： {fileName}\r\n{error}");

                if (tryDownloadNum >= maxTryDownloadNum)
                {
                    Log.Warning($"已超过最大尝试下载次数： {maxTryDownloadNum}\r\n请检查网络！");
                    return;
                }

                Debug.Log("------------------------------------尝试再次下载网络文件！------------------------------------");

                StartCoroutine(CreateDownloader());
            };

            // 下载进度
            Downloader.OnDownloadProgressCallback = (int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes) =>
            {
                Debug.Log($"下载进度：{currentDownloadCount / totalDownloadCount * 100}%  下载大小：{ToMB(currentDownloadBytes)}MB / {ToMB(totalDownloadBytes)}MB");
            };

            Downloader.BeginDownload();
            yield return Downloader;

            if (Downloader.Status != EOperationStatus.Succeed)
            {
                yield break;
            }

            Log.Green("资源更新完毕！！！");

            DownloadOver();
        }

        // 下载完毕
        private void DownloadOver()
        {
            Debug.Log("------------------------------------下载完毕------------------------------------");

            ClearCache();
        }

        // 清理未使用的缓存文件
        private void ClearCache()
        {
            Debug.Log("清理未使用的缓存文件");

            var package = YooAssets.GetAssetsPackage(packageName);
            var operation = package.ClearUnusedCacheFilesAsync();
            operation.Completed += Operation_Completed;
        }

        // 流程更新完毕
        private void Operation_Completed(AsyncOperationBase obj)
        {
            Debug.Log("更新完毕");
            Debug.Log("开始游戏");


            // 跳转场景
            SceneOperationHandle sceneOperation = YooAssets.LoadSceneAsync("Login");

        }

        #endregion


        // 转换成 MB 显示值
        public string ToMB(long value)
        {
            float size = value / MB;
            size = Mathf.Clamp(size, 0.1f, float.MaxValue);
            string totalSize = size.ToString("f1");
            return totalSize;
        }

        /// <summary>
        /// 获取资源服务器地址
        /// </summary>
        private string GetHostServerURL()
        {

            //#if UNITY_EDITOR
            //            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
            //                return $"{hostServerIP}/CDN/Android/{gameVersion}";
            //            else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
            //                return $"{hostServerIP}/CDN/IPhone/{gameVersion}";
            //            else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
            //                return $"{hostServerIP}/CDN/WebGL/{gameVersion}";
            //            else
            //                return $"{hostServerIP}/CDN/PC/{gameVersion}";
            //#else
            //			if (Application.platform == RuntimePlatform.Android)
            //				return $"{hostServerIP}/CDN/Android/{gameVersion}";
            //			else if (Application.platform == RuntimePlatform.IPhonePlayer)
            //				return $"{hostServerIP}/CDN/IPhone/{gameVersion}";
            //			else if (Application.platform == RuntimePlatform.WebGLPlayer)
            //				return $"{hostServerIP}/CDN/WebGL/{gameVersion}";
            //			else
            //				return $"{hostServerIP}/CDN/PC/{gameVersion}";
            //#endif


            return $"{hostServerIP}/{resPath}/{PlatformUtility.platform}/{gameVersion}";
        }

        /// <summary>
        /// 内置文件查询服务类
        /// </summary>
        public class GameQueryServices : IQueryServices
        {
            public bool QueryStreamingAssets(string fileName)
            {
                string buildinFolderName = YooAssets.GetStreamingAssetBuildinFolderName();
                return StreamingAssetsHelper.FileExists($"{buildinFolderName}/{fileName}");
            }
        }

        /// <summary>
        /// 资源文件解密服务类
        /// </summary>
        private class GameDecryptionServices : IDecryptionServices
        {
            public ulong LoadFromFileOffset(DecryptFileInfo fileInfo)
            {
                return 32;
            }

            public byte[] LoadFromMemory(DecryptFileInfo fileInfo)
            {
                throw new NotImplementedException();
            }

            public Stream LoadFromStream(DecryptFileInfo fileInfo)
            {
                BundleStream bundleStream = new BundleStream(fileInfo.FilePath, FileMode.Open);
                return bundleStream;
            }

            public uint GetManagedReadBufferSize()
            {
                return 1024;
            }
        }
    }
}
