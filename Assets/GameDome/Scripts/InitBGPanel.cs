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

namespace Test
{
    [ExecuteInEditMode]
    public class InitBGPanel : MonoBehaviour
    {
        Text screenInfo;

        void Start()
        {
            screenInfo = transform.FindOf<Text>("screenInfo");

            transform.FindOf<Button>("loadPanelBtn")?.onClick.AddListener(
                async () =>
            {
                Log.Debuger("开始加载 RightPanel");

                var ao = ResourcesManager.LoadAssetAsync<GameObject>("RightPanel");

                Log.Debuger($"加载结果 {ao}");

                await ao.Task;

                Log.Debuger($"加载完成 {ao.Status}");

                Log.Debuger($"实例化 {Instantiate(ao.AssetObject)}");

                //asset.Completed += (_) =>
                //{
                //    Log.Debuger($"加载完成 {asset}");

                //    Log.Debuger($"实例化 {Instantiate(asset.AssetObject)}");

                //};

                ao.Release();
            });

            transform.FindOf<Text>("HotUpdateText").text = "2";

            Log.Striking(TestHotUpdate.name);
        }

        private void Update()
        {
            //screenInfo.text = $"{Screen.width} x {Screen.height}";
        }
    }

}
