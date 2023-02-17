using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using YooAsset;

public class GameStart : MonoBehaviour
{
    /// <summary>
    /// 资源系统运行模式
    /// </summary>
    public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;


    void Start()
    {
        YooAssets.Initialize();
        YooAssets.SetOperationSystemMaxTimeSlice(30);
    }

}
