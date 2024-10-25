using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Framework;
using Framework.YooAssetExpress;
using YooAsset;

namespace Test
{
    public class GameStart : MonoBehaviour
    {
        protected const float KB = 1024f;
        protected const float MB = 1048576f;
        protected const float GB = 1073741824f;
        protected const float TB = 1099511627776f;

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
        [SerializeField] protected int maxTryDownloadNum = 3;
        /// <summary>
        /// 尝试下载次数
        /// </summary>
        [Header("尝试下载次数")]
        [SerializeField] protected int tryDownloadNum = 0;

        protected EDefaultBuildPipeline buildPipeline = EDefaultBuildPipeline.BuiltinBuildPipeline;

        /// <summary>
        /// 资源包的版本信息
        /// </summary>
        public string PackageVersion { set; get; }

        /// <summary>
        /// 下载器
        /// </summary>
        public ResourceDownloaderOperation Downloader { set; get; }

        protected virtual void Awake()
        {
            ResourcesManager.SetHandler<YooAssetResourcesHandler>();
        }

        protected virtual void Start()
        {
            YooAssets.Initialize();
            YooAssets.SetOperationSystemMaxTimeSlice(30);

            StartCoroutine(InitPackage());
        }


        #region 资源热更流程

        // 初始化资源包
        protected IEnumerator InitPackage()
        {
            //yield return new WaitForSeconds(1);

            EPlayMode _playMode = PlayMode;

            Debug.Log("------------------------------------创建默认的资源包------------------------------------");

            // 创建默认的资源包
            var package = YooAssets.TryGetPackage(packageName);
            if (package == null)
            {
                // 没有则创建
                package = YooAssets.CreatePackage(packageName);
                YooAssets.SetDefaultPackage(package);
            }

            InitializationOperation initializationOperation = null;
            switch (_playMode)
            {
                // 编辑器下的模拟模式
                case EPlayMode.EditorSimulateMode:
                    {
                        var simulateBuildResult = EditorSimulateModeHelper.SimulateBuild(buildPipeline, packageName);
                        var createParameters = new EditorSimulateModeParameters();
                        createParameters.EditorFileSystemParameters = FileSystemParameters.CreateDefaultEditorFileSystemParameters(simulateBuildResult);
                        initializationOperation = package.InitializeAsync(createParameters);
                    }
                    break;

                // 单机运行模式
                case EPlayMode.OfflinePlayMode:
                    {
                        var createParameters = new OfflinePlayModeParameters();
                        createParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
                        initializationOperation = package.InitializeAsync(createParameters);
                    }
                    break;

                // 联机运行模式
                case EPlayMode.HostPlayMode:
                    {
                        string defaultHostServer = GetHostServerURL();
                        string fallbackHostServer = GetHostServerURL();
                        IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                        var createParameters = new HostPlayModeParameters();
                        createParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
                        createParameters.CacheFileSystemParameters = FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices);
                        initializationOperation = package.InitializeAsync(createParameters);
                    }
                    break;
                // WebGL运行模式
                case EPlayMode.WebPlayMode:
                    {
                        var createParameters = new WebPlayModeParameters();
                        createParameters.WebFileSystemParameters = FileSystemParameters.CreateDefaultWebFileSystemParameters();
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
                yield return GetStaticVersion();
            }
            else
            {
                Debug.LogError($"创建默认的资源包失败：{initializationOperation.Error}");

                // 尝试再次初始化
                yield return InitPackage();
            }
        }

        // 获取资源更新版本
        protected IEnumerator GetStaticVersion()
        {
            Debug.Log("------------------------------------获取最新的资源版本 !------------------------------------");

            //yield return new WaitForSecondsRealtime(0.5f);

            var package = YooAssets.GetPackage(packageName);
            var operation = package.RequestPackageVersionAsync();
            yield return operation;

            if (operation.Status == EOperationStatus.Succeed)
            {
                PackageVersion = operation.PackageVersion;

                yield return UpdateManifest();
            }
            else
            {
                Debug.LogError($"获取资源更新版本失败：{operation.Error}");

                // 用户尝试再次更新静态版本
                yield return GetStaticVersion();
            }
        }

        // 更新资源清单！
        protected IEnumerator UpdateManifest()
        {
            Debug.Log("------------------------------------更新资源清单！------------------------------------");

            //yield return new WaitForSecondsRealtime(0.5f);

            var package = YooAssets.GetPackage(packageName);
            var operation = package.UpdatePackageManifestAsync(PackageVersion);
            yield return operation;

            if (operation.Status == EOperationStatus.Succeed)
            {
                yield return CreateDownloader();
            }
            else
            {
                Debug.LogError($"创建补丁下载器失败：{operation.Error}");

                // 用户尝试再次更新更新补丁清单
                yield return UpdateManifest();
            }
        }

        // 创建补丁下载器！
        protected IEnumerator CreateDownloader()
        {
            Debug.Log("------------------------------------创建补丁下载器！------------------------------------");

            //yield return new WaitForSecondsRealtime(0.5f);

            tryDownloadNum++;

            int downloadingMaxNum = 10;
            int failedTryAgain = 3;
            Downloader = YooAssets.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            //var package = YooAssets.GetPackage(packageName);
            //Downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);

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
        protected IEnumerator BeginDownload()
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
        protected void DownloadOver()
        {
            Debug.Log("------------------------------------下载完毕------------------------------------");

            ClearCache();
        }

        // 清理未使用的缓存文件
        protected void ClearCache()
        {
            Debug.Log("清理未使用的缓存文件");

            var package = YooAssets.GetPackage(packageName);
            var operation = package.ClearUnusedBundleFilesAsync();
            operation.Completed += Operation_Completed;
        }

        // 流程更新完毕
        protected void Operation_Completed(AsyncOperationBase obj)
        {
            Debug.Log("更新完毕");

            //LoadDLL.LoadMetadataForAOTAssemblies();
            // 加载程序集
#if !UNITY_EDITOR
            LoadDllFunc();
#endif

            Debug.Log("开始游戏");

            // 跳转场景
            Debug.Log("开始加载场景 ---》Login《--- ");
            var sceneOperation = YooAssets.LoadSceneAsync("Login");
            sceneOperation.Completed += (_) =>
            {
                Debug.Log($"加载场景 ---》{sceneOperation.SceneObject.name}《--- 成功");
            };
        }

        #endregion

        /// <summary>
        ///  加载 dll 程序集
        /// </summary>
        private void LoadDllFunc()
        {
            LoadDllFunc("Assembly-CSharp.dll.bytes");
            //LoadDllFunc("Assets/HotUpdateAssemblies/Use/Assembly-CSharp.dll.bytes");
        }
        /// <summary>
        ///  加载 dll 程序集
        /// </summary>
        private void LoadDllFunc(string location)
        {
            if (!islog) return;
            islog = false;

            Log.Yellow("开始加载 dll 程序集");

            var aoh = YooAssets.LoadAssetSync<TextAsset>(location);

            Debug.Log($"处理器：{aoh}，{location}");
            //Debug.Log($"Address：{aoh.GetAssetInfo().Address}，AssetPath：{aoh.GetAssetInfo().AssetPath}，AssetType：{aoh.GetAssetInfo().AssetType}，");

            TextAsset csDll = aoh?.GetAssetObject<TextAsset>();

            Debug.Log($"文本资源：{csDll != null}，{location}");

            byte[] dllDatas = csDll ? csDll.bytes : null;

            //Debug.Log($"资源元数据：{dllDatas}，大小：{dllDatas.Length}，{location}");

            if (dllDatas != null)
            {
                Debug.Log($"开始加载程序集 ---》{location}《---");
                System.Reflection.Assembly.Load(dllDatas);
                Log.Striking($"加载程序集 ---》{location}《--- 完毕");
            }
            else
            {
                Log.Error($"不存在程序集 ---》{location}《---");
            }
        }
        bool islog = true;


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
        protected virtual string GetHostServerURL()
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

    }
}
