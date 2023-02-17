﻿// -------------------------
// 创建日期：2022/10/29 10:29:37
// -------------------------

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;

namespace Framework
{
    /// <summary>
    /// Unity 单例模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
    {
        readonly static object locker = new object();

        protected static T instance;
        public static T Instance
        {
            get
            {
                InitAutoInstance();
                return instance;
            }
            protected set { instance = value; }
        }

        /// <summary>
        /// 是否保持单实例
        /// <para>ps1：新加入的实例会被自动销毁掉 <see cref="Object.Destroy(Object)"/></para>
        /// <para>ps2：请在 <see cref="Awake()"/> 之前设置（即调用 <code>
        /// protected override void Awake()
        /// {
        ///    isSoloInstance= true;
        ///    base.Awake();
        /// }
        /// </code> 之前）</para>
        /// </summary>
        public static bool isSoloInstance = false;

        /// <summary>
        /// 是否自动实例化
        /// </summary>
        public static bool autoInstance = false;

        static void InitAutoInstance()
        {
            if (instance) return;

            lock (locker)
            {
                instance = FindObjectOfType<T>(true);

                if (autoInstance)
                {
                    //autoInstance = false;
                    if (!instance)
                    {
                        instance = new GameObject().AddComponent<T>();
                        instance.gameObject.name = $"[{instance.GetType().Name}] Singleton";
                    }
                }
            }
        }


        protected virtual void Awake()
        {
            if (isSoloInstance && instance && instance.GetType() == typeof(T))
            {
                isSoloInstance = false;

                Destroy(gameObject);

                //Log.Yellow($"为  {instance.name}  销毁多余实例");

                return;
            }

            instance = (T)(Component)this;
        }

    }
}