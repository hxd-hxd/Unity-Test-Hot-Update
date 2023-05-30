// -------------------------
// 创建日期：2023/5/11 15:47:57
// -------------------------

#pragma warning disable 0414
#pragma warning disable 0219

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Framework.HybridCLRExpress
{
    [CreateAssetMenu(menuName = "Framework HybridCLRExpress/Assemblies Cfg", fileName = "Assemblies Cfg")]
    public class AssembliesCfg : ScriptableObject
    {
        [Header("热更新程序集作为资源存放的路径")]
        [Header("保存 HybridCLR 生成程序集相关操作的设置")]

        /// <summary>
        /// 要拷贝到的目标路径 
        /// </summary>
        [TextArea(2, 10)]
        public string copyPath = "Assets/HotUpdateAssemblies";
        /// <summary>
        /// 当前要使用的 dll 存放的路径名
        /// </summary>
        [Header("当前要使用的 dll 存放的路径名")]
        public string currentUsePathName = "Use";

        /// <summary>
        /// 当前要使用的 dll 存放的完整路径（copyPath/currentUsePathName）
        /// </summary>
        public string currentUsePath => Path.Combine(copyPath, currentUsePathName);

#if UNITY_EDITOR
        // 等同使用 CreateAssetMenu 的效果
        [MenuItem("Assets/Create/Framework HybridCLRExpress/Create Assemblies Cfg")]
        static void InteriorCreate()
        {
            Create();
        }
        public static AssembliesCfg Create()
        {
            AssembliesCfg cfg = null;
            string path = null;
            Object target = Selection.activeObject;
            if (target)
            {
                path = AssetDatabase.GetAssetPath(target);
                if (File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }
            }
            else
            {
                path = cfg.copyPath;
            }
            cfg = ScriptableObjectUtility.Create<AssembliesCfg>(path);
            //cfg.copyPath = path;
            return cfg;
        }
#endif
    }
}