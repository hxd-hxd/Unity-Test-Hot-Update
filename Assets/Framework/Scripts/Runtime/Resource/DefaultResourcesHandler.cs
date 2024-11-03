// -------------------------
// 创建日期：2022/10/27 10:12:12
// -------------------------

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;
using System;
using UnityEngine.SceneManagement;

namespace Framework
{
    /// <summary>
    /// Resources 资源处理器
    /// </summary>
    public class DefaultResourcesHandler : IResourcesHandler
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public IAssetOperation LoadAllAssets(string path)
        {
            throw new NotImplementedException();
        }

        public IAssetOperation LoadAllAssets<TObject>(string path) where TObject : Object
        {
            throw new NotImplementedException();
        }

        public IAssetOperation LoadAllAssets(string path, Type type)
        {
            throw new NotImplementedException();
        }

        public IAssetOperation LoadAllAssetsAsync(string path)
        {
            throw new NotImplementedException();
        }

        public IAssetOperation LoadAllAssetsAsync<TObject>(string path) where TObject : Object
        {
            throw new NotImplementedException();
        }

        public IAssetOperation LoadAllAssetsAsync(string path, Type type)
        {
            throw new NotImplementedException();
        }

        public IAssetOperation LoadAsset(string path)
        {
            throw new NotImplementedException();
        }

        public IAssetOperation LoadAsset<TObject>(string path) where TObject : Object
        {
            throw new NotImplementedException();
        }

        public IAssetOperation LoadAsset(string path, Type type)
        {
            throw new NotImplementedException();
        }

        public IAssetOperation LoadAssetAsync(string path)
        {
            throw new NotImplementedException();
        }

        public IAssetOperation LoadAssetAsync<TObject>(string path) where TObject : Object
        {
            throw new NotImplementedException();
        }

        public IAssetOperation LoadAssetAsync(string path, Type type)
        {
            throw new NotImplementedException();
        }

        public IAssetOperation LoadScene(string path, LoadSceneMode sceneMode, LocalPhysicsMode physicsMode)
        {
            throw new NotImplementedException();
        }

        public IAssetOperation LoadSceneAsync(string path, LoadSceneMode sceneMode, LocalPhysicsMode physicsMode, bool suspendLoad, uint priority)
        {
            throw new NotImplementedException();
        }
    }
}