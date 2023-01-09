using System;
using UnityEngine;
using System.Collections;

// 默认banner和插屏都是黑包广告，视频都是白包广告
// 白包状态下只显示白包广告，黑包下黑包广告和白包广告都显示

namespace AndroidTools.Tools
{
    public class AndroidAds
    {
        private static PayHandler payHandler = new PayHandler();
        private static AdsHandler adsHandler = new AdsHandler();

        /// <summary>
        /// 初始化广告，设置AdsHandler
        /// </summary>
        static AndroidAds()
        {
            AndroidTools.AndroidAgent.Call("SetAdsHandler", adsHandler);
        }
        
        public static void ShowRewardAd(Action successAction, Action failureAction)
        {
            if (IsAdsRemoved())
            {
                successAction?.Invoke();
                return;
            }
            adsHandler.rewardAction = successAction;
            adsHandler.failureAction = failureAction;
            AndroidTools.AndroidAgent.Call("ShowRewardAd");
        }

        /// <summary>
        /// 显示插屏广告，默认为黑包广告
        /// </summary>
        /// <param name="isBlackAd"></param>
        public static void ShowInterstitialAd(bool isBlackAd = true)
        {
            if (IsAdsRemoved())
            {
                return;
            }

            if (isBlackAd && !AndroidSwitch.IsBlack())
            {
                return;
            }
            AndroidTools.AndroidAgent.Call("ShowInsertAd");
        }

        public static void ShowFullScreenInterstitialAd(bool isBlackAd = true)
        {
            if (IsAdsRemoved())
            {
                return;
            }

            if (isBlackAd && !AndroidSwitch.IsBlack())
            {
                return;
            }
            AndroidTools.AndroidAgent.Call("ShowFullScreenInsertAd");
        }
        
        /// <summary>
        /// 显示Banner，默认为黑包广告
        /// </summary>
        /// <param name="isBlackAd"></param>
        public static void ShowBanner(bool isBlackAd = true, int index = 0)
        {
            if (IsAdsRemoved())
            {
                return;
            }

            if (isBlackAd && !AndroidSwitch.IsBlack())
            {
                return;
            }
            
            AndroidTools.AndroidAgent.Call("ShowBanner", index);
        }
        
        public static void ShowTimerAd(Action<IEnumerator> startCoroutineAction)
        {
            startCoroutineAction(TimerAd());
        }

        private static IEnumerator TimerAd()
        {
            while (true)
            {
                yield return new WaitForSeconds(120);
                ShowInterstitialAd();
            }
        }

        /// <summary>
        /// 移除所有Banner
        /// </summary>
        public static void RemoveBanner()
        {
            AndroidTools.AndroidAgent.Call("RemoveBanner");
        }

        /// <summary>
        /// 是否有广告
        /// </summary>
        public static bool IsLoadedVideoAd()
        {
            #if UNITY_EDITOR
                return true;
            #endif
            return AndroidTools.AndroidAgent.Call<bool>("IsLoadedVideoAd");
        }
        
        public static bool IsAdsRemoved()
        {
            return PlayerPrefs.GetString("IsAdsRemoved") == "true";
        }

        /// <summary>
        /// 购买任意商品后，移除广告
        /// </summary>
        public static void RemoveAds()
        {
            PlayerPrefs.SetString("IsAdsRemoved", "true");
            PlayerPrefs.Save();
            // RemoveBanner();
        }
    }
}