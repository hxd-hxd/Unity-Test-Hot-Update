// -------------------------
// 由工具自动创建，请勿手动修改
// 创建日期：2023/2/24 15:28:09
// -------------------------

using UnityEngine;
using UnityEditor;

namespace AutoNamespace
{
    public static class AutoNamespace_Menu
    {
        

        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/AutoNamespace", false, 81)]
        private static void NamespaceMenu_AutoNamespace()
        {
            AutoNamespaceUtility.CreateCSharpScriptAsset("AutoNamespace");
        }
        // 本方法用于控制菜单按钮 AutoNamespace 的启用
        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/AutoNamespace", true, 81)]
        private static bool NamespaceMenuEnable_AutoNamespace()
        {
            return AutoNamespaceUtility.Enable();
        }
        

        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/TestMenu", false, 1000)]
        private static void NamespaceMenu_TestMenu()
        {
            AutoNamespaceUtility.CreateCSharpScriptAsset("TestMenu");
        }
        // 本方法用于控制菜单按钮 TestMenu 的启用
        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/TestMenu", true, 1000)]
        private static bool NamespaceMenuEnable_TestMenu()
        {
            return AutoNamespaceUtility.Enable();
        }
        

        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/Framework", false, 1000)]
        private static void NamespaceMenu_Framework()
        {
            AutoNamespaceUtility.CreateCSharpScriptAsset("Framework");
        }
        // 本方法用于控制菜单按钮 Framework 的启用
        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/Framework", true, 1000)]
        private static bool NamespaceMenuEnable_Framework()
        {
            return AutoNamespaceUtility.Enable();
        }
        

        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/TMPro", false, 1000)]
        private static void NamespaceMenu_TMPro()
        {
            AutoNamespaceUtility.CreateCSharpScriptAsset("TMPro");
        }
        // 本方法用于控制菜单按钮 TMPro 的启用
        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/TMPro", true, 1000)]
        private static bool NamespaceMenuEnable_TMPro()
        {
            return AutoNamespaceUtility.Enable();
        }
        

        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/Framework.Test", false, 1000)]
        private static void NamespaceMenu_Framework_Test()
        {
            AutoNamespaceUtility.CreateCSharpScriptAsset("Framework.Test");
        }
        // 本方法用于控制菜单按钮 Framework.Test 的启用
        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/Framework.Test", true, 1000)]
        private static bool NamespaceMenuEnable_Framework_Test()
        {
            return AutoNamespaceUtility.Enable();
        }
        

        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/Framework.YooAssetExpress", false, 1000)]
        private static void NamespaceMenu_Framework_YooAssetExpress()
        {
            AutoNamespaceUtility.CreateCSharpScriptAsset("Framework.YooAssetExpress");
        }
        // 本方法用于控制菜单按钮 Framework.YooAssetExpress 的启用
        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/Framework.YooAssetExpress", true, 1000)]
        private static bool NamespaceMenuEnable_Framework_YooAssetExpress()
        {
            return AutoNamespaceUtility.Enable();
        }
        

        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/Test.TestYooAsset", false, 1000)]
        private static void NamespaceMenu_Test_TestYooAsset()
        {
            AutoNamespaceUtility.CreateCSharpScriptAsset("Test.TestYooAsset");
        }
        // 本方法用于控制菜单按钮 Test.TestYooAsset 的启用
        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/Test.TestYooAsset", true, 1000)]
        private static bool NamespaceMenuEnable_Test_TestYooAsset()
        {
            return AutoNamespaceUtility.Enable();
        }
        
    }
}