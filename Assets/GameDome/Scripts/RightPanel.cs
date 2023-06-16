// -------------------------
// 创建日期：2023/2/28 16:52:31
// -------------------------

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Framework;
using UnityEngine.UI;

namespace Test 
{
    public class RightPanel : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            transform.FindOf<Button>("closeBtn")?.onClick.AddListener(() =>
            {
                Destroy(gameObject);
            });
        }

    }
}
