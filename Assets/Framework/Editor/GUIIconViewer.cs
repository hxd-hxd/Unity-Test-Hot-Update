// -------------------------
// 创建日期：2023/5/15 17:24:28
// -------------------------

#pragma warning disable 0414
#pragma warning disable 0219

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Framework.Editor
{
	/// <summary>
	/// 查看 untiy 图标
	/// </summary>
	public class GUIIconViewer : EditorWindow
	{
	    // 滑动条
	    Vector2 sv = Vector2.zero;
	    // 搜索
	    private string search = "";
	    private int currentNum;// 当前数量
	    // 图标
	    private static List<GUIContent> icons;
	
	    /*
	    * 这里的菜单路径自行修改
	    */
	    [MenuItem("Tools/Window/GUIIconViewer")]
	    static void Open()
	    {
	        GUIIconViewer my = GetWindow<GUIIconViewer>("GUIIconViewer");
	        my.minSize = new Vector2(900, 800);
	
	    }
	
	    // 窗口打开时调用
	    private void OnEnable()
	    {
	        icons = new List<GUIContent>();
	        Texture2D[] textures = Resources.FindObjectsOfTypeAll<Texture2D>();
	        foreach (Texture2D texture in textures)
	        {
	            GUIContent icon = EditorGUIUtility.IconContent(texture.name, $"|{texture.name}");
	            if (icon != null && icon.image != null) icons.Add(icon);
	        }
	    }
	
	    // 编辑器 UI
	    private void OnGUI()
	    {
	        //GUILayout.Label("在这里编写你的编辑器");
	
	        // 搜索
	        currentNum = 0;
	        GUILayout.BeginHorizontal("HelpBox");
	        {
	            GUILayout.Space(10);
	            EditorGUILayout.LabelField("搜索", GUILayout.MaxWidth(36));
	            search = EditorGUILayout.TextField("", search, "SearchTextField", GUILayout.MaxWidth(400));
	
	            foreach (var item in icons)
	            {
	                if (item.tooltip.ToLower().Contains(search.ToLower()))
	                {
	                    currentNum++;
	                }
	            }
	            EditorGUILayout.LabelField(string.Format("{0}/{1}", currentNum, icons.Count), GUILayout.MaxWidth(50));
	            //GUILayout.Label("取消", "SearchCancelButtonEmpty");
	            if (!string.IsNullOrEmpty(search))
	                if (GUILayout.Button("", "SearchCancelButton"))
	                {
	                    search = "";
	                }
	        }
	        GUILayout.EndHorizontal();
	
	        sv = EditorGUILayout.BeginScrollView(sv);
	        //Debug.Log(sv);
	
	        int num = 0;
	        int noeLineNum = 20;
	        for (int i = 0; i < icons.Count; i += noeLineNum)
	        {
	            GUILayout.BeginHorizontal();
	            for (int j = 0; j < noeLineNum; j++)
	            {
	                if (i + j < icons.Count)
	                {
	                    num++;
	                    var item = icons[i + j];
	                    if (item.tooltip.ToLower().Contains(search.ToLower()))
	                    {
	                        if(GUILayout.Button(item, GUILayout.Width(40), GUILayout.Height(40)))
	                        {
	                            TextEditor textEditor = new TextEditor();
	                            textEditor.text = item.tooltip;
	                            textEditor.OnFocus();
	                            textEditor.Copy();
	                        }
	
	                    }
	                }
	            }
	            GUILayout.EndHorizontal();
	        }
	
	        EditorGUILayout.EndScrollView();
	    }
	}
}