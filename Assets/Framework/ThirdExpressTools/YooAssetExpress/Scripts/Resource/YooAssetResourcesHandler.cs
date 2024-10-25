// -------------------------
// 创建日期：2022/10/27 10:12:12
// -------------------------

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using YooAsset;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Framework
{
    /// <summary>
    /// YooAsset 资源处理器
    /// </summary>
    public class YooAssetResourcesHandler : IResourcesHandler
    {
        public IAssetOperation LoadAsset(string path)
        {
            var h = YooAssets.LoadAssetSync(path);
            return YooAssetResourcesOperation.Get(h);
        }
        public IAssetOperation LoadAsset<TObject>(string path) where TObject : Object
        {
            var h = YooAssets.LoadAssetSync<TObject>(path);
            return YooAssetResourcesOperation.Get(h);
        }
        public IAssetOperation LoadAsset(string path, Type type)
        {
            var h = YooAssets.LoadAssetSync(path, type);
            return YooAssetResourcesOperation.Get(h);
        }

        public IAssetOperation LoadAssetAsync(string path)
        {
            var h = YooAssets.LoadAssetAsync(path);
            return YooAssetResourcesOperation.Get(h);
        }
        public IAssetOperation LoadAssetAsync<TObject>(string path) where TObject : Object
        {
            var h = YooAssets.LoadAssetAsync<TObject>(path);
            return YooAssetResourcesOperation.Get(h);
        }
        public IAssetOperation LoadAssetAsync(string path, Type type)
        {
            var h = YooAssets.LoadAssetAsync(path, type);
            return YooAssetResourcesOperation.Get(h);
        }

        public IAssetOperation LoadAllAssets(string path)
        {
            var h = YooAssets.LoadAllAssetsSync(path);
            return YooAssetResourcesOperation.Get(h);
        }
        public IAssetOperation LoadAllAssets<TObject>(string path) where TObject : Object
        {
            var h = YooAssets.LoadAllAssetsSync<TObject>(path);
            return YooAssetResourcesOperation.Get(h);
        }
        public IAssetOperation LoadAllAssets(string path, Type type)
        {
            var h = YooAssets.LoadAllAssetsSync(path, type);
            return YooAssetResourcesOperation.Get(h);
        }
        public IAssetOperation LoadAllAssetsAsync(string path)
        {
            var h = YooAssets.LoadAllAssetsAsync(path);
            return YooAssetResourcesOperation.Get(h);
        }
        public IAssetOperation LoadAllAssetsAsync<TObject>(string path) where TObject : Object
        {
            var h = YooAssets.LoadAllAssetsAsync<TObject>(path);
            return YooAssetResourcesOperation.Get(h);
        }
        public IAssetOperation LoadAllAssetsAsync(string path, Type type)
        {
            var h = YooAssets.LoadAllAssetsAsync(path, type);
            return YooAssetResourcesOperation.Get(h);
        }

        public IAssetOperation LoadScene(string path, LoadSceneMode sceneMode, LocalPhysicsMode physicsMode)
        {
            var h = YooAssets.LoadSceneSync(path, sceneMode, physicsMode);
            return YooAssetResourcesOperation.Get(h);
        }
        public IAssetOperation LoadSceneAsync(string path, LoadSceneMode sceneMode, LocalPhysicsMode physicsMode, bool allowSceneActivation, uint priority)
        {
            // 注意：YooAssets 这里的 suspendLoad 参数是在加载时挂起，从而不激活场景，而接口 allowSceneActivation 是允许激活，所以这里要反向操作
            var h = YooAssets.LoadSceneAsync(path, sceneMode, physicsMode, !allowSceneActivation, priority);
            return YooAssetResourcesOperation.Get(h);
        }


    }
}