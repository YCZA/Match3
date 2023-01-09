using System;
using System.Collections;
using UnityEngine;
using AndroidTools.Tools;
using UnityEngine.Networking;

namespace AndroidTools
{
    public class PayHandler : AndroidJavaProxy
    {
        public static bool isEnd = false;   // 购买时会设置成false, 结束后设置为true
        public static bool isSuccess = false;
        public static Action<int> onGrantReplenishment;
        
        public PayHandler() : base("com.unity3d.utils.IPayHandler")
        {
        }

        public void RestoreItem(int id, string purchaseData, string purchaseDataSignature)
        {
            Debug.Log("发放商品");
            lock (AndroidTools.AndroidAgent.buyQueue)
            {
                AndroidTools.AndroidAgent.buyQueue.Enqueue(() => AndroidPay.RealProcess(id, purchaseData, purchaseDataSignature));
            }
        }

        public void GrantReplenishment(int id, string purchaseData, string purchaseDataSignature)
        {
            Debug.Log("发放补单商品");
            lock (AndroidTools.AndroidAgent.buyQueue)
            {
                AndroidTools.AndroidAgent.buyQueue.Enqueue(() => AndroidPay.GrantReplenishment(id, purchaseData, purchaseDataSignature));
            }
        }

        public void OnSuccess()
        {
            Debug.Log("支付成功");
        }

        public void OnFailed()
        {
            PayHandler.isEnd = true;
            PayHandler.isSuccess = false;
            Debug.Log("支付失败");
        }
    }
}