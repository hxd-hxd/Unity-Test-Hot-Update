// -------------------------
// 创建日期：2024/11/6 16:44:10
// -------------------------

#pragma warning disable 0414
#pragma warning disable 0219

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Test
{
    public class TestHFSUnityWebRequest : MonoBehaviour
    {
        [TextArea]
        public string url = "";

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Web());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Web()
        {
            using (var uwr = UnityWebRequest.Get(url))
            {
                uwr.timeout = 3;

                Debug.Log($"发送网络请求：\"{url}\"");
                yield return uwr.SendWebRequest();
                Debug.Log($"网络请求结果：{uwr.result}");
                if (uwr.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log($"网络请求成功：\r\n{uwr.downloadHandler.text}");

                }
                else
                {
                    Debug.LogError($"网络请求失败：{uwr.error}");
                }
            }
        }
    }
}