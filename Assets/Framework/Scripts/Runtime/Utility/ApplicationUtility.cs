// -------------------------
// 创建日期：2022/11/25 19:27:09
// -------------------------

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Framework
{

    public enum Platform
    {
        PC,
        Android,
        IPhone,
        WebGL,
    }

    /// <summary>
    /// 应用程序
    /// </summary>
    public static class ApplicationUtility
    {
        /// <summary>
        /// 当前平台
        /// </summary>
        public static Platform platform
        {
            get
            {
#if UNITY_EDITOR
                if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
                    return Platform.Android;
                else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
                    return Platform.IPhone;
                else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
                    return Platform.WebGL;
                else
                    return Platform.PC;
#else
                if (Application.platform == RuntimePlatform.Android)
                    return Platform.Android;
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                    return Platform.IPhone;
                else if (Application.platform == RuntimePlatform.WebGLPlayer)
                    return Platform.WebGL;
                else
                    return Platform.PC;
#endif
            }
        }


        /// <summary>
        /// 是否有网络
        /// <para>注意：此方法只在 Unity 编辑器中有效</para>
        /// </summary>
        public static bool NetworkAvailable
        {
            get
            {
                return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork ||
                    Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork;
            }
        }

        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        /// <summary>
        /// C#判断是否联网
        /// </summary>
        /// <returns></returns>
        public static bool IsConnectedInternet()
        {
            int i = 0;
            if (InternetGetConnectedState(out i, 0))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 退出
        /// </summary>
        public static void Quit()
        {
            Application.Quit();

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }
}