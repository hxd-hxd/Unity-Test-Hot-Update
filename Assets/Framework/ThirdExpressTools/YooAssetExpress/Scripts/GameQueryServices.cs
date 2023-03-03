// -------------------------
// 创建日期：2023/2/22 11:24:15
// -------------------------

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace Framework.YooAssetExpress
{

    /// <summary>
    /// 内置文件查询服务类
    /// </summary>
    public class GameQueryServices : IQueryServices
    {
        public bool QueryStreamingAssets(string fileName)
        {
            string buildinFolderName = YooAssets.GetStreamingAssetBuildinFolderName();
            return StreamingAssetsHelper.FileExists($"{buildinFolderName}/{fileName}");
        }
    }

}