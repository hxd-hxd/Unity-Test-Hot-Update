// -------------------------
// 由工具自动创建，请勿手动修改
// 创建日期：2024/11/6 16:44:00
// -------------------------

#pragma warning disable 0414
#pragma warning disable 0219

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
        

        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/Framework.Editor", false, 1000)]
        private static void NamespaceMenu_Framework_Editor()
        {
            AutoNamespaceUtility.CreateCSharpScriptAsset("Framework.Editor");
        }
        // 本方法用于控制菜单按钮 Framework.Editor 的启用
        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/Framework.Editor", true, 1000)]
        private static bool NamespaceMenuEnable_Framework_Editor()
        {
            return AutoNamespaceUtility.Enable();
        }
        

        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/Framework.HybridCLRExpress", false, 1000)]
        private static void NamespaceMenu_Framework_HybridCLRExpress()
        {
            AutoNamespaceUtility.CreateCSharpScriptAsset("Framework.HybridCLRExpress");
        }
        // 本方法用于控制菜单按钮 Framework.HybridCLRExpress 的启用
        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/Framework.HybridCLRExpress", true, 1000)]
        private static bool NamespaceMenuEnable_Framework_HybridCLRExpress()
        {
            return AutoNamespaceUtility.Enable();
        }
        

        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/Framework.GameFrameworkExpress", false, 1000)]
        private static void NamespaceMenu_Framework_GameFrameworkExpress()
        {
            AutoNamespaceUtility.CreateCSharpScriptAsset("Framework.GameFrameworkExpress");
        }
        // 本方法用于控制菜单按钮 Framework.GameFrameworkExpress 的启用
        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/Framework.GameFrameworkExpress", true, 1000)]
        private static bool NamespaceMenuEnable_Framework_GameFrameworkExpress()
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
        

        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/GameFramework", false, 1000)]
        private static void NamespaceMenu_GameFramework()
        {
            AutoNamespaceUtility.CreateCSharpScriptAsset("GameFramework");
        }
        // 本方法用于控制菜单按钮 GameFramework 的启用
        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/GameFramework", true, 1000)]
        private static bool NamespaceMenuEnable_GameFramework()
        {
            return AutoNamespaceUtility.Enable();
        }
        

        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/GameFramework.Runtime", false, 1000)]
        private static void NamespaceMenu_GameFramework_Runtime()
        {
            AutoNamespaceUtility.CreateCSharpScriptAsset("GameFramework.Runtime");
        }
        // 本方法用于控制菜单按钮 GameFramework.Runtime 的启用
        [MenuItem(Constant.Menu.MenuAutoNamespacePath + "/GameFramework.Runtime", true, 1000)]
        private static bool NamespaceMenuEnable_GameFramework_Runtime()
        {
            return AutoNamespaceUtility.Enable();
        }
        
    }
}