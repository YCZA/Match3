using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts2.Android.DataStatistics;
using Match3.Scripts2.Env;
using Match3.Scripts2.Network;
// using LitJson;
using Wooga.Newtonsoft.Json;
// using Prime31;
using UnityEngine;

namespace AndroidTools.Tools
{
    public class AndroidPay
    {
        private static PayHandler payHandler = new PayHandler();
        public static List<ProductInfo> productInfoList = new List<ProductInfo>();

        /// <summary>
        /// 初始化购买，设置AdsHandler
        /// </summary>
        static AndroidPay()
        {
            AndroidTools.AndroidAgent.Call("SetPayHandler", payHandler);
        }

        public static void InitHandler(){
            // 空方法，用来触发静态构造函数
        }

        private static IEnumerator getStoreItemInfo = null;
        public static void InitStoreItem(Dictionary<string ,string> itemDic, Action<IEnumerator> stopCoroutineAction, Action<IEnumerator> startCoroutineAction)
        {
            // 从表中读取商品id
            // JsonArray jsonArray= new JsonArray();
            // foreach (var kv in itemDic)
            // {
            //     JsonObject obj = new JsonObject();
            //     obj["key"] = kv.Key;
            //     obj["value"] = kv.Value;
            //     jsonArray.Add(obj);
            // }

            // string json = jsonArray.toJson();
            string json = "";
            Debug.Log("读取商品：" + json);
            AndroidTools.AndroidAgent.Call("InitStore", json);
            if (getStoreItemInfo == null)
            {
                getStoreItemInfo = GetStoreItemInfo();
            }
            stopCoroutineAction(getStoreItemInfo);
            startCoroutineAction(getStoreItemInfo);
        }

        // 补单查询, 需要登录后再执行
        public static void ReplenishmentQuery()
        {
            AndroidTools.AndroidAgent.Call("ReplenishmentQuery");
        }
        
        public static void PayItem(string strId)
        {
            ProductInfo product = GetProductByStrId(strId);
            if (product == null)
            {
                return;
            }
            
            PayItem(int.Parse(product.id), PriceToFloat(product.price), product.name);
        }
        
        public static void PayItem(int numId)
        {
            ProductInfo product = GetProductByNumId(numId);
            if (product == null)
            {
                return;
            }

            PayItem(int.Parse(product.id), PriceToFloat(product.price), product.name);
        }
        
        public static void PayItem(int id, float price, String itemName)
        {
            PayHandler.isEnd = false;
            AndroidTools.AndroidAgent.Call("PayItem", id, price, itemName);
        }

        private static void VerifyPurchaseResult(string content, string sign, Action<string> onSuccess, Action<ToServer.ErrorInfo> onfail)
        {
            // 通过服务器验证
            ToServer.Instance.VerifyPurchaseResult(content, sign, onSuccess, onfail);
        }

        private static void TriggerPurchase(string receipt)
        {
            if(GameEnvironment.CurrentPlatform == GameEnvironment.Platform.HW_Abroad)
            {
                Debug.Log("TriggerPurchase on hw");
                // 华为传过来的价格单位为分需要转成元
                var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(receipt);
                DataStatistics.Instance.TriggerPurchase(result["currency"].ToString(),
                    int.Parse(result["price"].ToString())/ 100f, 
                    1,
                    result["productName"].ToString(),
                    result["payOrderId"].ToString(),
                    result["orderId"].ToString());
            }
            else if(GameEnvironment.CurrentPlatform == GameEnvironment.Platform.Samsung_Abroad)
            {
                Debug.Log("TriggerPurchase on samsung");
                // todo samsung... 
            }
        }

        public static void RealProcess(int id, string content, string sign)
        {
            VerifyPurchaseResult(content, sign, (receipt) =>
            {
                // 校验成功, 发放奖励, 消耗商品
                Debug.Log("Verify:" + "校验成功，发放奖励: " + receipt);
                AndroidTools.AndroidAgent.Call("consumeOwnedPurchase", content);
                PayHandler.isEnd = true;
                PayHandler.isSuccess = true;
                TriggerPurchase(receipt);
            }, (s) =>
            {
                // eli todo 应该有弹窗提示
                PayHandler.isEnd = true;
                PayHandler.isSuccess = false;
                Debug.LogError("Verify:" + s);
            });
            Debug.Log("购买完成");
        }

        // 补单
        public static void GrantReplenishment(int id, string content, string sign)
        {
            VerifyPurchaseResult(content, sign, (receipt) =>
            {
                Debug.Log("Verify:" + "校验成功，发放奖励: " + receipt);
                if (PayHandler.onGrantReplenishment != null)
                {
                    PayHandler.onGrantReplenishment(id);
                    AndroidTools.AndroidAgent.Call("consumeOwnedPurchase", content);
                    
                    TriggerPurchase(receipt);
                }
                
            }, (s) =>
            {
                // eli todo 应该有弹窗提示
                Debug.LogError("Verify:" + s);
            });
        }

        /// <summary>
        /// 将类似$9.99的字符串转换成数字9.99
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        private static float PriceToFloat(string price)
        {
            float priceF = 0;
            int startIndex = 0;
            
            // 欧元显示为"EUR 9.99", 人民币显示为"9.99"
            for (int i = 0; i < price.Length; i++)
            {
                if (price[i] > 47 && price[i] < 58)
                {
                    startIndex = i;
                    break;
                }
            }

            priceF = float.Parse(price.Substring(startIndex));
            Debug.Log("解析出价格：" + priceF);
            return priceF;
        }

        private class StoreJsonData
        {
            public string id = "";
            public string id_Str = "";
            public string name = "";
            public string price = "";
            public string desc = "";
            public string currency = "";
        }
        private static IEnumerator GetStoreItemInfo()
        {
            while (true)
            {
                string json = AndroidTools.AndroidAgent.Call<string>("GetStoreItemInfo");
                
                if (string.IsNullOrEmpty(json))
                {
                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    Debug.Log("初始化商品:" + json);
                    
                    // JsonData storeList = JsonMapper.ToObject(json);
                    var productList = JsonConvert.DeserializeObject<List<ProductInfo>>(json);
                    productInfoList = productList;
                    yield break;
                }
            }
        }

        public static ProductInfo GetProductByNumId(int id)
        {
            foreach (var product in productInfoList)
            {
                if (product.id == id.ToString())
                {
                    return product;
                }
            }

            if (!Application.isEditor)
            {
                Debug.LogError("未找到相应的商品，id为：" + id);
            }
            return null;
        }

        private static ProductInfo GetProductByStrId(string id)
        {
            foreach (var product in productInfoList)
            {
                if (product.id_Str == id)
                {
                    return product;
                }
            }
            
            Debug.LogError("未找到相应的商品，id为：" + id);
            return null;
        }
    }
}