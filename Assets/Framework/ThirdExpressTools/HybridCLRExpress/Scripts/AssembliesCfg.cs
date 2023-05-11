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
        /// <summary>
        /// 要拷贝到的目标路径
        /// </summary>
        [TextArea]
        public string copyPath = "Assets/HotUpdateAssemblies";

#if UNITY_EDITOR
        // 等同使用 CreateAssetMenu 的效果
        [MenuItem("Assets/Create/Framework HybridCLRExpress/Create Assemblies Cfg")]
        public static void Create()
        {
            //AssembliesCfg cfg = CreateInstance<AssembliesCfg>();
            AssembliesCfg cfg = new AssembliesCfg();

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
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            //cfg.copyPath = path;
            string cfgPath = $"{path}/Assemblies Cfg.asset";
            if (File.Exists(cfgPath))
                for (int i = 1; i < int.MaxValue; i++)
                {
                    cfgPath = $"{path}/Assemblies Cfg {i}.asset";
                    if (!File.Exists(cfgPath)) break;
                }
            AssetDatabase.CreateAsset(cfg, cfgPath);

            Selection.activeObject = cfg;

            AssetDatabase.Refresh();
        }
#endif
    }
}