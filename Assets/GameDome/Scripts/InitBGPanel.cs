// -------------------------
// 创建日期：2023/2/24 15:30:19
// -------------------------

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using YooAsset;
using Framework;

namespace Test.TestYooAsset
{
    public class InitBGPanel : MonoBehaviour
    {

        void Start()
        {
            

            transform.FindOf<Button>("loadPanelBtn")?.onClick.AddListener(() =>
            {
                Log.Debuger("开始加载 RightPanel");

                var asset = YooAssets.LoadAssetAsync<GameObject>("RightPanel");

                Log.Debuger($"加载结果 {asset}");

                asset.Completed += (_) =>
                {
                    Log.Debuger($"加载完成 {asset}");

                    Log.Debuger($"实例化 {Instantiate(asset.AssetObject)}");

                };
            });
        }

    }
}
