using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AutoNamespace
{
    public class TestMenu
    {
        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/Test", false, Constant.RootMenuPriority)]
        private static void NamespaceMenu_Test()
        {
            AutoNamespaceUtility.CreateCSharpScriptAsset("Test");
        }
        // 本方法用于控制菜单按钮 Test 的启用
        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/Test", true, Constant.RootMenuPriority)]
        private static bool NamespaceMenuEnble_Test()
        {
            return AutoNamespaceUtility.Enable();
        }
    }
}