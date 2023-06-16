// -------------------------
// 创建日期：2023/6/1 23:58:57
// -------------------------

#pragma warning disable 0414
#pragma warning disable 0219

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Runtime
{
    public class InstanceGameFramework : MonoBehaviour
    {
        public GameObject gameFrameworkInstance;
        [SerializeField]
        private bool _enable;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            gameFrameworkInstance?.SetActive(_enable);
        }

        void Start()
        {
        
        }

    }
}