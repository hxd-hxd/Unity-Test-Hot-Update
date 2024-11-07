using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Framework;
using Framework.Event;

namespace Test
{
    public class GameStart : MonoBehaviour
    {
        
        private void Awake()
        {
            EventCenter.AddListener<PatchEventDefine.UpdateDone>(OnHandleEvent);
        }

        private void OnDestroy()
        {
            EventCenter.RemoveListener<PatchEventDefine.UpdateDone>(OnHandleEvent);
        }

        private void OnHandleEvent(PatchEventDefine.UpdateDone msg)
        {

            Debug.Log("开始游戏");

            // 跳转场景
            Debug.Log("开始加载场景 ---》Login《--- ");
            var sceneOperation = ResourcesManager.LoadSceneAsync("Login");
            sceneOperation.Completed += (_) =>
            {
                Debug.Log($"加载场景 ---》{sceneOperation.SceneObject.Value.name}《--- 成功");
            };
        }
    }
}
