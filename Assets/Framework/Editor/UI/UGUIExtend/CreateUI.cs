// -------------------------
// 创建日期：2023/4/12 11:20:05
// -------------------------

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Framework.Editor
{
    /// <summary>
    /// 用于创建 UI 
    /// </summary>
    public static class CreateUI
    {
        /// <summary>
        /// 编辑器资源的根节点
        /// </summary>
        public static string EditorResRootPath => "Framework/Prefabs/";
        //public const string FullRootPath = "Assets/Framework/Editor/Resources/Framework/Prefabs/";
        public static string FullRootPath => "Assets/Framework/Prefabs/";

        /// <summary>
        /// 创建目标 UI
        /// </summary>
        /// <param name="rootPath">需要创建的 UI 的预制体所在的目录</param>
        /// <param name="name">UI 的名称</param>
        /// <returns></returns>
        public static GameObject Create(string rootPath, string name)
        {
            Object obj = Selection.objects.Length > 0 ? Selection.objects[0] : null;

            var prefab = EditorGUIUtility.Load($"{rootPath}{name}") as GameObject;
            //var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{rootPath}{name}");
            var ins = Object.Instantiate(prefab);

            if (obj is GameObject go)
            {
                ins.transform.SetParent(go.transform, false);
            }

            Selection.activeGameObject = ins;

            return ins;
        }

        /// <summary>
        /// 创建目标 UI
        /// </summary>
        /// <param name="rootPath">需要创建的 UI 的预制体所在的目录</param>
        /// <param name="name">UI 的名称</param>
        /// <returns></returns>
        public static GameObject Create(string name)
        {
            return Create(FullRootPath, name);
        }
    }
}