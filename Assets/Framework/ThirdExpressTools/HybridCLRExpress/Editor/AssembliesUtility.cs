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
        public static AssembliesCfg _cfg;
        public static AssembliesCfg cfg
        {
            get
            {
                if (!_cfg)
                {
                    _cfg = GetAssembliesCfg(Application.dataPath);
                }
                //if (!_cfg)
                //{
                //    _cfg = new AssembliesCfg();
                //    string path = AssetDatabase.GetAssetPath(_cfg);
                //    //AssetDatabase.MoveAsset(path, _cfg.copyPath);
                //}
                return _cfg;
            }
            set { _cfg = value; }
        }
        private static AssembliesCfg GetAssembliesCfg(string path)
        {
            AssembliesCfg l_cfg = null;

            if (Directory.Exists(path))
            {
                string[] dirs = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path, "*.asset");

                foreach (string file in files)
                {
                    string l_file = file.Replace("\\", "/");
                    //string[] strs = Regex.Split(l_file, "Assets/");
                    //string assetsFile = $"Assets/{strs[strs.Length - 1]}";
                    string assetsFile = $"Assets{l_file.Replace(Application.dataPath, null)}";

                    l_cfg = AssetDatabase.LoadAssetAtPath<AssembliesCfg>(assetsFile);
                    if (l_cfg) break;
                }
                if (!l_cfg)
                {
                    foreach (string dir in dirs)
                    {
                        l_cfg = GetAssembliesCfg(dir);
                    }
                }
            }

            return l_cfg;
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
            foreach (var dll in SettingsUtil.HotUpdateAssemblyFilesExcludePreserved)
            {
                string dllPath = $"{hotfixDllSrcDir}/{dll}";
                string extension = string.IsNullOrEmpty(addExtension) ? null : $".{addExtension}";// 扩展名
                string dllBytesPath = $"{hotfixAssembliesDstDir}/{target}/{dll}{extension}";
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