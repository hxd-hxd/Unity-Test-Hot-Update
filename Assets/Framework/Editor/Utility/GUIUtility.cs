// -------------------------
// 创建日期：2023/5/15 18:42:33
// -------------------------

#pragma warning disable 0414
#pragma warning disable 0219

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Framework.Editor
{
    /// <summary>
    /// Unity GUI
    /// </summary>
    public static class GUIUtility
    {
        /// <summary>
        /// 获取 GUI 图标列表
        /// </summary>
        /// <returns></returns>
        public static List<GUIContent> GetIcons()
        {
            var icons = new List<GUIContent>();
            Texture2D[] textures = Resources.FindObjectsOfTypeAll<Texture2D>();
            foreach (Texture2D texture in textures)
            {
                GUIContent icon = EditorGUIUtility.IconContent(texture.name, $"|{texture.name}");
                if (icon != null && icon.image != null) icons.Add(icon);
            }
            return icons;
        }
        /// <summary>
        /// 获取 GUI 图标字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, GUIContent> GetIconDic()
        {
            var icons = new Dictionary<string, GUIContent>();
            Texture2D[] textures = Resources.FindObjectsOfTypeAll<Texture2D>();
            foreach (Texture2D texture in textures)
            {
                GUIContent icon = EditorGUIUtility.IconContent(texture.name, $"|{texture.name}");
                if (icon != null && icon.image != null) icons.Add(texture.name, icon);
            }
            return icons;
        }
    }
}