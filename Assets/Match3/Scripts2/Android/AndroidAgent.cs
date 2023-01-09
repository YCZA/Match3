using System;
using System.Collections.Generic;
using UnityEngine;
using AndroidTools.Tools;
using Match3.Scripts2.Android.DataStatistics;
using Debug = UnityEngine.Debug;

namespace AndroidTools
{
    public class AndroidAgent : MonoBehaviour
    {
        private static AndroidJavaObject jo;

        public static Queue<Action> buyQueue = new Queue<Action>();
        public static Queue<Action> rewardQueue = new Queue<Action>();
        public static Queue<Action> eventQueue = new Queue<Action>();

        private void Awake()
        {
            DontDestroyOnLoad(base.gameObject);

            if (Application.platform == RuntimePlatform.Android)
            {
                // 初始化jo
                jo = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                
                // 补单查询
                Call("OnEnterGame");
                
                // 显示Banner
                // AndroidAds.ShowBanner(true);

                // 获取商品信息
                // AndroidPay.InitStoreItem(StopCoroutine,(s)=>StartCoroutine(s));
                
                // 进游戏时, android端可能会调用androidPay, 所以要初始化一下
                AndroidPay.InitHandler();

                // 显示timerAd
                // AndroidAds.ShowTimerAd((e)=>StartCoroutine(e));

                // 插屏广告
                // Android.Tools.AndroidAds.ShowInterstitialAd(true);
                
                // 初始化dataStatisticsHandler
                AndroidTools.AndroidAgent.Call("SetDataStatisticsHandler", DataStatistics.Instance.dataStatisticsHandler);
            }
        }
        
        private void Update()
        {
            lock (buyQueue)
            {
                if (buyQueue.Count > 0)
                {
                    buyQueue.Dequeue()?.Invoke();
                }
            }
            lock (rewardQueue)
            {
                if (rewardQueue.Count > 0)
                {
                    rewardQueue.Dequeue()?.Invoke();
                }
            }
            lock (eventQueue)
            {
                if (eventQueue.Count > 0)
                {
                    eventQueue.Dequeue()?.Invoke();
                }
            }
        }
        
        public static void Call(string methodName, params object[] args)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if(jo != null)
                {
                    Debug.Log("call:" + methodName);
                    jo?.Call(methodName, args);
                }
                else
                {
                    Debug.LogError("jo未初始化完成:" + methodName);
                }
            }
        }

        public static T Call<T>(string methodName, params object[] args)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (jo != null)
                {
                    return jo.Call<T>(methodName, args);
                }
                else
                {
                    Debug.LogError("jo未初始化完成:" + methodName);
                }
            }

            return default(T);
        }
    }
}