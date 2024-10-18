// -------------------------
// 创建日期：2023/2/20 17:54:21
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
    /// 扩展库
    /// </summary>
    public static class Express
    {
        ///// <summary>
        ///// 计算场景加载时刷新的进度条
        ///// </summary>
        ///// <param name="operation"></param>
        ///// <param name="minProgressTime">最小的进度持续时间（不会小于真实进度）</param>
        ///// <param name="progressAcion">更新进度时的回调</param>
        ///// <param name="progressFinish">更新进度完成的回调</param>
        ///// <returns></returns>
        //public static IEnumerator Progress(this SceneOperationHandle operation, float minProgressTime, Action<float> progressAcion, Action progressFinish)
        //{

        //    //yield return ExtendUtility.Progress(() =>
        //    yield return Progress(() =>
        //    {
        //        return operation.Progress;
        //    },
        //    minProgressTime,
        //    progressAcion,
        //    progressFinish);

        //}

        /// <summary>
        /// 计算场景加载时刷新的进度条
        /// </summary>
        /// <param name="pv">进度值(再次回调中返回真实进度值)</param>
        /// <param name="minProgressTime">最小的进度持续时间（不会小于真实进度）</param>
        /// <param name="progressAcion">更新进度时的回调</param>
        /// <param name="progressFinish">更新进度完成的回调</param>
        /// <returns></returns>
        public static IEnumerator Progress(Func<float> pv, float minProgressTime, Action<float> progressAcion, Action progressFinish)
        {
            float progressTime = 0f;
            float progress = 0f;

            while (pv() < 0.9f)// 进度加载完成后是 0.9 ，而不是 1 
            {
                progress = pv();
                progressTime += Time.deltaTime;
                if (progressTime < minProgressTime)
                {
                    progress = progressTime / (pv() + minProgressTime);
                }

                progressAcion?.Invoke(progress);
                yield return null;
            }

            // 补全进度条
            float currentVelocity = 0;
            while (progress <= 0.99f && progressTime < minProgressTime)
            {
                progress = Mathf.SmoothDampAngle(progress, 1, ref currentVelocity, minProgressTime - progressTime);
                progressAcion?.Invoke(progress);
                yield return null;
            }

            // 进度完成
            progress = 1;
            progressAcion?.Invoke(progress);

            yield return null;

            // 进度条完成
            progressFinish?.Invoke();

            // 界面消失
            //yield return UIVanish();
        }
    }
}