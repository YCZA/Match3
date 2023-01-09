using System;
using UnityEngine;

namespace AndroidTools
{
    public class AdsHandler : AndroidJavaProxy
    {
        public Action rewardAction;
        public Action failureAction;
	
        public AdsHandler() : base("com.unity3d.utils.IAdsHandler")
        {
            
        }

        public void OnSuccess()
        {
            // 广告奖励发放
            Debug.Log("发放广告奖励");
		
            lock (AndroidTools.AndroidAgent.rewardQueue)
            {
                AndroidTools.AndroidAgent.rewardQueue.Enqueue(rewardAction);
            }
        }

        public void OnFailed()
        {
            Debug.Log("广告播放失败");
            
            lock (AndroidTools.AndroidAgent.rewardQueue)
            {
                AndroidTools.AndroidAgent.rewardQueue.Enqueue(failureAction);
            }
        }
    }
}