// -------------------------
// 创建日期：2023/5/9 15:33:17
// -------------------------

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using HybridCLR.Editor;
using System.Linq;

using FGUIUtility = Framework.Editor.GUIUtility;

namespace Framework.HybridCLRExpress
{

    using static AssembliesUtility;
    using static Codice.Client.Common.Connection.AskCredentialsToUser;


    /// <summary>
    /// 处理由 HybridCLR 生成的程序集
    /// </summary>
    public class AssembliesWindow : EditorWindow
    {
        [MenuItem("Tools/Framework/HybridCLRAssembliesWindow")]
        static void Open()
        {
            var window = GetWindow<AssembliesWindow>("处理由 HybridCLR 生成的程序集");
            window.minSize = new Vector2(450, 450);
        }

        FlatFoldoutBranch rootFoldout;
        bool showAllDll = false;
        Vector2 rootFoldoutSV = Vector2.zero;
        Vector2 fullSV = Vector2.zero;

        // 拷贝
        bool copyAllDll = false;        // 拷贝所有 DLL
        bool copyCurPlatformDll = true; // 拷贝当前平台 DLL
        string copyPath { get => AssembliesUtility.cfg.copyPath; set => AssembliesUtility.cfg.copyPath = value; }
        //string copyPath = "Assets/HotUpdateAssemblies";

        GUIStyle whiteStyle = new GUIStyle();
        GUIStyle whiteButtonStyle = new GUIStyle();
        GUIStyle yellowButtonStyle = new GUIStyle();

        private void OnEnable()
        {
            string dir = hotUpdateDLLDir;

            rootFoldout = new FlatFoldoutBranch(dir);
            rootFoldout.path = dir;

            // 样式
            whiteStyle.normal.textColor = Color.white;
            whiteStyle.fontSize = 14;

            if (EditorStyles.miniButton != null)
                whiteButtonStyle = EditorStyles.miniButton.CloneSelf();
            whiteButtonStyle.normal.textColor = Color.white;
            whiteButtonStyle.fontSize = 14;
            whiteButtonStyle.stretchHeight = true;
            whiteButtonStyle.fixedHeight = 32;

            yellowButtonStyle = whiteButtonStyle.CloneSelf();

            //var cfg = AssembliesUtility.cfg;
        }

        private void OnGUI()
        {
            fullSV = EditorGUILayout.BeginScrollView(fullSV);

            var target = EditorUserBuildSettings.activeBuildTarget;

            bool hotUpdateDLLDirExists = Directory.Exists(hotUpdateDLLDir);// 检查根目录是否存在

            if (GUILayout.Button("打开 HybridCLR 设置", whiteButtonStyle))
            {
                //SettingsService.OpenProjectSettings("Project/HybridCLR Settings");
                MenuProvider.OpenSettings();
            }

            GUILayout.Space(8);

            // 根目录
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("热更新 DLL 所在根目录", GUILayout.MaxWidth(141));
                    EditorGUILayout.LabelField(hotUpdateDLLDir);
                    if (!hotUpdateDLLDirExists) GUILayout.Label("不存在", "RightLabel", GUILayout.MaxWidth(48));
                    if (GUILayout.Button("打开目录", GUILayout.MaxWidth(72)))
                    {
                        EditorUtility.RevealInFinder(hotUpdateDLLDir);
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(16);
                    GUILayout.Label("当前平台", GUILayout.MaxWidth(125));
                    EditorGUILayout.LabelField(target.ToString());
                    if (!Directory.Exists(hotUpdateDLLDirCurPlatform)) GUILayout.Label("不存在", "RightLabel", GUILayout.MaxWidth(48));
                    if (GUILayout.Button("打开目录", GUILayout.MaxWidth(72)))
                    {
                        EditorUtility.RevealInFinder(hotUpdateDLLDirCurPlatform);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(8);

            // DLL 资源文件
            EditorGUILayout.LabelField("热更新 DLL 文件", EditorStyles.boldLabel);
            var dlls = GetHotUpdateAssemblyFiles();
            EditorGUILayout.BeginVertical("box");
            {
                foreach (var dllFile in dlls)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(16);
                        //GUILayout.Label(dllFile);
                        EditorGUILayout.LabelField(dllFile);
                        if (!File.Exists(dllFile)) GUILayout.Label("不存在", "RightLabel", GUILayout.MaxWidth(48));
                        if (GUILayout.Button("打开目录", GUILayout.MaxWidth(72)))
                        {
                            EditorUtility.RevealInFinder(dllFile);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(8);

            // 拷贝操作
            EditorGUILayout.LabelField("拷贝热更新 DLL 文件", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(16);
                GUILayout.Label("为方便统一做热更新，可将 DLL 拷贝到 Assets 子目录下，用以配置 AssetsBundle", whiteStyle);
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(8);

            // 配置文件
            EditorGUILayout.ObjectField("相关保存配置", AssembliesUtility.cfg, AssembliesUtility.cfg.GetType(), false);

            GUILayout.Space(8);

            // 拷贝
            copyPath = GUILayout.TextArea(copyPath, GUILayout.MinHeight(48));
            //AssembliesUtility.cfg.copyPath = copyPath;
            //copyCurPlatformDll = EditorGUILayout.ToggleLeft("拷贝当前平台 DLL", copyCurPlatformDll);
            copyAllDll = EditorGUILayout.Toggle("拷贝所有 DLL", copyAllDll);
            if (GUILayout.Button("拷贝当前平台 DLL", whiteButtonStyle))
            {
                AssembliesUtility.CopyHotUpdateAssembliesToDir(copyPath, copyAllDll);

                AssetDatabase.Refresh();
            }
            if (GUILayout.Button("拷贝所有平台 DLL", whiteButtonStyle))
            {

                AssetDatabase.Refresh();
            }

            GUILayout.Space(8);

            // 树状结构
            GUILayout.Label("查看 DLL ", EditorStyles.boldLabel);
            //showAllDll = EditorGUILayout.ToggleLeft("显示所有 DLL", showAllDll);
            showAllDll = EditorGUILayout.Toggle("显示所有 DLL", showAllDll);
            EditorGUILayout.BeginVertical("box");
            {
                rootFoldoutSV = EditorGUILayout.BeginScrollView(rootFoldoutSV);
                if (hotUpdateDLLDirExists)
                {
                    // 检查存在
                    ClearInvalidBranch(rootFoldout);

                    // 树状结构
                    BranchTree(rootFoldout, 0);
                }
                else
                {
                    GUILayout.Label("未生成任何目录");
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
        }

        #region GUI

        static Texture _fileIconUnity;
        static Texture fileIconUnity
        {
            get
            {
                if (_fileIconUnity == null) _fileIconUnity = AssetDatabase.GetCachedIcon("Assets");
                return _fileIconUnity;
            }
        }

        // 显示树状结构分支
        public void BranchTree(FlatFoldoutBranch branchRoot, int level = 0)
        {
            string[] dllPlatformDirs = Directory.GetDirectories(branchRoot.path);
            List<string> dllPlatformFilesUsable = AssembliesUtility.GetDllFiles(branchRoot.path);// 存储过滤后的可用文件

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(16 * level);
            GUIContent folderGUI = null;
            //fileGUI = new GUIContent(branchRoot.name, fileIconUnity);

            bool folderEmpty = dllPlatformDirs.Length <= 0 && dllPlatformFilesUsable.Count <= 0;
            var dlls = GetHotUpdateAssemblyFiles();
            if (branchRoot.IsVacancyBranch)
            {
                folderGUI = FGUIUtility.IconNew.FolderEmptyOn;
            }
            else
            {
                //if (branchRoot.name == "StandaloneWindows64")
                //{
                //    Debug.Log("");
                //}

                // 筛分h
                if (!showAllDll)
                    foreach (var dllFile in dllPlatformFilesUsable)
                    {
                        if (dlls.Contains(dllFile))
                        {
                            folderEmpty = false;
                            break;
                        }
                            folderEmpty = true;
                    }

                if (folderEmpty)
                {
                    folderGUI = FGUIUtility.IconNew.FolderEmptyOn;
                }
                else
                {
                    if (branchRoot.foldout) folderGUI = FGUIUtility.IconNew.FolderOpened;
                    else folderGUI = FGUIUtility.IconNew.Folder;
                }
            }
            folderGUI.tooltip = null;
            folderGUI.text = branchRoot.name;

            //Debug.Log($"检查 {branchRoot.name}  文件夹：{dllPlatformDirs.Length}  可用文件：{dllPlatformFilesUsable.Count}  是否空显示：{folderEmpty}");

            if (folderEmpty)
            {
                EditorGUILayout.LabelField(folderGUI);
            }
            else
            {
                //branchRoot.foldout = EditorGUILayout.Foldout(branchRoot.foldout, folderGUI, true);
                branchRoot.foldout = EditorGUILayout.BeginFoldoutHeaderGroup(branchRoot.foldout, folderGUI);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.EndHorizontal();

            // 目录分支
            if (branchRoot.foldout)
            {
                level++;
                foreach (var dllDir in dllPlatformDirs)
                {
                    string dllDirName = Path.GetFileName(dllDir);
                    var branch = branchRoot.Expect(dllDirName);
                    branch.path = dllDir;
                    branch.isBranch = true;

                    // 继续向下分支
                    BranchTree(branch, level);
                }

                // 非分支
                foreach (var dllFile in dllPlatformFilesUsable)
                {
                    Action dllShowItem = () =>
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(16 * level);
                        string fileName = Path.GetFileName(dllFile);
                        var branch = branchRoot.Expect(fileName);
                        branch.path = dllFile;
                        branch.isBranch = false;
                        branch.foldout = false;
                        GUIContent gui = FGUIUtility.IconNew.AssemblyDefinitionAsset;
                        gui.tooltip = null;
                        gui.text = branch.name;
                        //gui.text += branch.name;
                        EditorGUILayout.LabelField(gui);
                        EditorGUILayout.EndHorizontal();
                    };

                    if (showAllDll)
                    {
                        dllShowItem();
                    }
                    else
                    {
                        if (dlls.Contains(dllFile)) dllShowItem();
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
        // 清除无效的分支
        public void ClearInvalidBranch(FlatFoldoutBranch branchRoot)
        {
            List<string> invalids = new List<string>();
            foreach (var item in branchRoot.subBranchDic)
            {
                var _b = item.Value as FlatFoldoutBranch;
                if (_b.isBranch)
                {
                    if (!Directory.Exists(_b.path))
                    {
                        invalids.Add(item.Key);
                    }

                    // 子分支
                    ClearInvalidBranch(_b);
                }
                else
                {
                    if (!File.Exists(_b.path))
                    {
                        invalids.Add(item.Key);
                    }
                }
            }

            // 移除
            branchRoot.Remove(invalids);
        }

        /// <summary>
        /// 平面结构折叠分支
        /// <para>用于做树状结构</para>
        /// </summary>
        [Serializable]
        public class FlatFoldoutBranch : FlatBranch<FlatFoldoutBranch>
        {

            protected bool _foldout = true;
            /// <summary>
            /// 折叠
            /// </summary>
            public bool foldout
            {
                get
                {
                    if (!isBranch) _foldout = false;
                    return _foldout;
                }
                set
                {
                    if (isBranch)
                        _foldout = value;
                }
            }
            public string path { get; set; }

            //public new Dictionary<string, FlatFoldoutBranch> subBranchDic { get; set; } = new Dictionary<string, FlatFoldoutBranch>();

            public FlatFoldoutBranch() : base()
            {

            }

            public FlatFoldoutBranch(string name) : base(name)
            {

            }

            /// <summary>
            /// 折叠所有
            /// </summary>
            /// <param name="isFoldout"></param>
            public void FoldoutAllBranch(bool isFoldout)
            {
                FoldoutAllBranch(this, isFoldout);
            }
            protected void FoldoutAllBranch(FlatFoldoutBranch branch, bool isFoldout)
            {
                if (branch == null) return;

                branch.foldout = isFoldout;
                foreach (var item in branch.subBranchDic)
                {
                    var ffb = item.Value as FlatFoldoutBranch;
                    if (ffb != null && ffb.isBranch)
                    {
                        FoldoutAllBranch(ffb, isFoldout);
                    }
                }
            }

            public new FlatFoldoutBranch Expect(string name)
            {
                return base.Expect<FlatFoldoutBranch>(name);
            }
        }

        /// <summary>
        /// 平面结构分支
        /// <para>用于做树状结构</para>
        /// </summary>
        [Serializable]
        public class FlatBranch<B> : Branch
            where B : Branch
        {
            public FlatBranch() : base()
            {

            }

            public FlatBranch(string name) : base(name)
            {

            }
        }

        /// <summary>
        /// 结构分支
        /// </summary>
        [Serializable]
        public class Branch
        {
            /// <summary>
            /// 是否分支
            /// </summary>
            public bool isBranch = true;

            protected string _name;

            protected int _id;

            public int id { get => _id; protected set => _id = value; }
            public string name { get => _name; protected set => _name = value; }

            public Dictionary<string, Branch> subBranchDic { get; set; } = new Dictionary<string, Branch>();

            public Branch this[string name]
            {
                get => Get(name);
                set => subBranchDic[name] = value;
            }
            public Branch this[int id]
            {
                get
                {
                    foreach (var item in subBranchDic)
                    {
                        if (item.Value.id == id)
                        {
                            return item.Value;
                        }
                    }
                    return null;
                }
                //set => subBranchDic[id.ToString()] = value;
            }
            /// <summary>
            /// 是否空分支
            /// </summary>
            public bool IsVacancyBranch
            {
                get
                {
                    if (isBranch)
                    {
                        return subBranchDic.Count < 0;
                    }
                    return true;
                }
            }

            #region 构造

            public Branch()
            {
                id = GetHashCode();
                name = id.ToString();
            }

            protected Branch(string name)
            {
                id = GetHashCode();
                _name = name;
            }

            protected Branch(int id)
            {
                _id = id;
                name = id.ToString();
            }

            protected Branch(string name, int id)
            {
                _name = name;
                _id = id;
            }

            #endregion

            public virtual Branch Expect(string name)
            {
                return (Get(name) ?? Add(name));
            }
            public virtual B Expect<B>(string name) where B : Branch, new()
            {
                return (Get(name) ?? Add<B>(name)) as B;
            }

            public virtual Branch Get(string name)
            {
                return subBranchDic.ContainsKey(name) ? subBranchDic[name] : null;
            }
            public virtual B Get<B>(string name) where B : Branch
            {
                return Get(name) as B;
            }

            public virtual Branch Add(string name)
            {
                return Add(name);
            }
            public virtual B Add<B>(string name) where B : Branch, new()
            {
                if (subBranchDic.ContainsKey(name)) return this[name] as B;
                return Add(new B() { name = name });
            }
            public virtual B Add<B>(B branch) where B : Branch
            {
                string name = branch.name;
                if (!subBranchDic.ContainsKey(name))
                    subBranchDic.Add(name, branch);
                return branch;
            }

            public virtual void Remove(string name)
            {
                if (subBranchDic.ContainsKey(name))
                {
                    subBranchDic.Remove(name);
                }
            }
            public virtual void Remove(List<string> names)
            {
                foreach (var item in names)
                {
                    Remove(item);
                }
            }
            public virtual void Remove<B>(B branch) where B : Branch
            {
                if (subBranchDic.ContainsValue(branch))
                {
                    subBranchDic.Remove(branch.name);
                }
            }
        }
        #endregion

        #region HybridCLR


        #endregion
    }
}