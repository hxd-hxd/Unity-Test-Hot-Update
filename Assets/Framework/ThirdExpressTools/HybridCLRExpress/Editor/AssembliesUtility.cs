// -------------------------
// 创建日期：2023/5/9 17:40:06
// -------------------------

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HybridCLR.Editor;
using UnityEditor;
using System.Linq;

using System.Text;
using System.Text.RegularExpressions;

namespace Framework.HybridCLRExpress
{
    public static class AssembliesUtility
    {
        private static AssembliesCfg _cfg;
        /// <summary>
        /// 程序集操作配置文件
        /// </summary>
        public static AssembliesCfg cfg
        {
            get
            {
                if (!_cfg)
                {
                    _cfg = GetAssembliesCfg();

                    //Debug.Log("查找配置文件 AssembliesCfg");

                    if (!_cfg)
                    {
                        _cfg = AssembliesCfg.Create();

                        //Debug.Log("未找到配置文件 AssembliesCfg，创建");
                    }
                }
                return _cfg;
            }
            set { _cfg = value; }
        }

        /// <summary>
        /// 热更新 Dll 目录
        /// </summary>
        public static string hotUpdateDLLDir => Path.GetDirectoryName(hotUpdateDLLDirCurPlatform);

        /// <summary>
        /// 当前平台热更新 Dll 目录
        /// </summary>
        public static string hotUpdateDLLDirCurPlatform => SettingsUtil.GetHotUpdateDllsOutputDirByTarget(EditorUserBuildSettings.activeBuildTarget);

        /// <summary>
        /// 热更新程序集文件排除保留
        /// </summary>
        public static List<string> HotUpdateAssemblyFilesExcludePreserved => SettingsUtil.HotUpdateAssemblyFilesExcludePreserved;

        public static AssembliesCfg GetAssembliesCfg()
        {
           return GetAssembliesCfg(Application.dataPath);
        }
        /// <summary>
        /// 获取 <see cref="AssembliesCfg"/> 配置
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static AssembliesCfg GetAssembliesCfg(string path)
        {
            AssembliesCfg l_cfg = null;

            if (Directory.Exists(path))
            {
                string[] dirs = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path, "*.asset");

                foreach (string file in files)
                {
                    string assetsFile = PathUtility.GetUnityAssetPath(file);

                    l_cfg = AssetDatabase.LoadAssetAtPath<AssembliesCfg>(assetsFile);

                    //Log.Yellow($"找到 SO 文件：{assetsFile}");
                    if (l_cfg)
                    {
                        //Log.Striking($"成功加载 AssembliesCfg SO 文件：{assetsFile}");
                        break;
                    }
                }
                if (!l_cfg)
                {
                    foreach (string dir in dirs)
                    {
                        l_cfg = GetAssembliesCfg(dir);
                        if (l_cfg)
                        {
                            //Log.Striking($"成功加载 AssembliesCfg SO 文件，在目录：{dir}");
                            break;
                        }
                    }
                }
            }

            return l_cfg;
        }

        public static void CopyHotUpdateAssembliesToStreamingAssets()
        {
            CopyHotUpdateAssembliesToDir(Application.streamingAssetsPath, EditorUserBuildSettings.activeBuildTarget, "bytes");
        }
        public static void CopyHotUpdateAssembliesToDir(string dir)
        {
            CopyHotUpdateAssembliesToDir(dir, "bytes");
        }
        public static void CopyHotUpdateAssembliesToDir(string dir, string addExtension)
        {
            CopyHotUpdateAssembliesToDir(dir, EditorUserBuildSettings.activeBuildTarget, addExtension);
        }
        /// <summary>
        /// 拷贝指定平台的 DLL 到指定的 Assets 目录下
        /// </summary>
        /// <param name="targetDir">要拷贝的目标目录</param>
        /// <param name="buildTarget">目标平台</param>
        /// <param name="addExtension">要为拷贝后的文件添加的扩展名</param>
        public static void CopyHotUpdateAssembliesToDir(string targetDir, BuildTarget buildTarget, string addExtension)
        {
            var target = buildTarget;

            string hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);// 获取对应平台的 DLL 生成目录
            string hotfixAssembliesDstDir = targetDir;// 目标目录
            string hotfixAssembliesPlatformDir = $"{hotfixAssembliesDstDir}/{target}";// 目标平台目录

            // 检查目录存在
            if (!Directory.Exists(hotfixAssembliesPlatformDir))
            {
                Directory.CreateDirectory(hotfixAssembliesPlatformDir);
            }

            foreach (var dll in SettingsUtil.HotUpdateAssemblyFilesExcludePreserved)
            {
                string dllPath = $"{hotfixDllSrcDir}/{dll}";
                string extension = string.IsNullOrEmpty(addExtension) ? null : $".{addExtension}";// 扩展名
                string dllBytesPath = $"{hotfixAssembliesPlatformDir}/{dll}{extension}";
                File.Copy(dllPath, dllBytesPath, true);
                Debug.Log($"[CopyHotUpdateAssembliesToDir] copy hotfix dll {dllPath} -> {dllBytesPath}");
            }
        }

        /// <summary>
        /// 获取要热更新的 DLL 文件
        /// </summary>
        /// <returns></returns>
        public static List<string> GetHotUpdateAssemblyFiles()
        {
            return GetHotUpdateAssemblyFiles(EditorUserBuildSettings.activeBuildTarget);
        }
        /// <summary>
        /// 获取要热更新的 DLL 文件
        /// </summary>
        /// <returns></returns>
        public static List<string> GetHotUpdateAssemblyFiles(BuildTarget buildTarget)
        {
            var target = buildTarget;

            string hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
            return SettingsUtil.HotUpdateAssemblyFilesExcludePreserved.Select((dll) => $"{hotfixDllSrcDir}/{dll}").ToList();
        }

        /// <summary>
        /// 获取所有平台的 DLL 文件
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDllPlatformFiles()
        {
            string[] dllPlatformFiles = Directory.GetFiles(hotUpdateDLLDir);
            List<string> dllPlatformFilesUsable = new List<string>();// 存储过滤后的可用文件
            // 过滤文件
            for (int i = 0; i < dllPlatformFiles.Length; i++)
            {
                string file = dllPlatformFiles[i];
                string extension = Path.GetExtension(file);
                if (extension == ".dll")
                {
                    // 转换文件分割格式
                    file = file.Replace("\\", "/");

                    dllPlatformFilesUsable.Add(file);
                }
            }

            return dllPlatformFilesUsable;
        }
    }
}