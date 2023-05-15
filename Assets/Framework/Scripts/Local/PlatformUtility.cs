﻿// -------------------------
// 创建日期：2023/3/29 17:11:30
// -------------------------

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Framework
{

    public static class PlatformUtility
    {

        /// <summary>
        /// 当前平台
        /// <para>统一的标识（<see cref="BuildTarget"/>，<see cref="RuntimePlatform"/>）</para>
        /// </summary>
        public static Platform platform
        {
            get
            {
#if UNITY_EDITOR
                return PlatformUntie(EditorUserBuildSettings.activeBuildTarget);
#else
                return PlatformUntie(Application.platform);
#endif
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// 转换成 <see cref="Platform"/> 对应的平台标识，编辑器打包时
        /// </summary>
        /// <param name="target">Unity 编辑器打包时的平台</param>
        /// <returns></returns>
        public static Platform PlatformUntie(BuildTarget target)
        {
            if (target == UnityEditor.BuildTarget.Android)
                return Platform.Android;
            else if (target == UnityEditor.BuildTarget.iOS)
                return Platform.IPhone;
            else if (target == UnityEditor.BuildTarget.WebGL)
                return Platform.WebGL;
            else
                return Platform.PC;
        }
#endif
        /// <summary>
        /// 转换成 <see cref="Platform"/> 对应的平台标识
        /// </summary>
        /// <param name="target">Unity 运行时平台</param>
        /// <returns></returns>
        public static Platform PlatformUntie(RuntimePlatform target)
        {
            if (target == RuntimePlatform.Android)
                return Platform.Android;
            else if (target == RuntimePlatform.IPhonePlayer)
                return Platform.IPhone;
            else if (target == RuntimePlatform.WebGLPlayer)
                return Platform.WebGL;
            else
                return Platform.PC;
        }
    }
}