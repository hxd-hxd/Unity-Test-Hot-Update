using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using YooAsset;

public class GameStart : MonoBehaviour
{
    /// <summary>
    /// ��Դϵͳ����ģʽ
    /// </summary>
    public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;


    void Start()
    {
        YooAssets.Initialize();
        YooAssets.SetOperationSystemMaxTimeSlice(30);
    }

}
