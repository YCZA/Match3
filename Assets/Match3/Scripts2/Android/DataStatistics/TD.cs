using UnityEngine;

namespace Match3.Scripts2.Android.DataStatistics
{
    public class TD: IStatisticsTool
    {
        private AndroidJavaObject jo;
        public TD(AndroidJavaObject jo)
        {
            this.jo = jo;
        }

        private void Call(string methodName, params object[] args)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if(jo != null)
                {
                    UnityEngine.Debug.Log("call:" + methodName);
                    jo?.Call(methodName, args);
                }
                else
                {
                    UnityEngine.Debug.LogError("jo未初始化完成:" + methodName);
                }
            }
        }

        private T Call<T>(string methodName, params object[] args)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (jo != null)
                {
                    return jo.Call<T>(methodName, args);
                }
                else
                {
                    UnityEngine.Debug.LogError("jo未初始化完成:" + methodName);
                }
            }

            return default(T);
        }
        
        public void TriggerLevel(string play_mode, int level_number, int level_diff, int level_duration, string event_state,
            int times_buy_steps, int times_use_booster1, int times_use_booster2, int times_use_booster3, int times_use_booster,
            int remaining_steps_number)
        {
            Call("TriggerLevel", play_mode, level_number, level_diff, level_duration, event_state, times_buy_steps, times_use_booster1, times_use_booster2, times_use_booster3, times_use_booster, remaining_steps_number);
        }

        public void TriggerPurchase(string currency, float priceNotCent, int quantity, string productName, string orderId,
            string recepitId)
        {
        }

        public void TriggerTutorialComplete(int index)
        {
            Call("TriggerTutorialComplete", index);
        }

        public void TriggerUseBooster(int level_id, string booster_name)
        {
            Call("TriggerUseBooster", level_id, booster_name);
        }

        public void TriggerGetResources(string name, int amount, string way)
        {
            Call("TriggerGetResources", name, amount, way);
        }

        public void TriggerConsumeResources(string name, int amount, string way)
        {
            Call("TriggerConsumeResources", name, amount ,way);
        }

        public void TriggerViewItem(string item_name)
        {
            Call("TriggerViewItem", item_name);
        }

        public void TriggerGetWheelBounty()
        {
            Call("TriggerGetWheelBounty");
        }

        public void TriggerBuyBankChest(int diamonds_number)
        {
            Call("TriggerBuyBankChest", diamonds_number);
        }

        public void TriggerCompleteBankTask1()
        {
            Call("TriggerCompleteBankTask1");
        }

        public void TriggerCompleteBankTask2()
        {
            Call("TriggerCompleteBankTask2");
        }

        public void TriggerBuyBuilding(string name, int shop_tag)
        {
            Call("TriggerBuyBuilding", name, shop_tag);
        }

        public void TriggerBuyBooster(string booster_name)
        {
            Call("TriggerBuyBooster", booster_name);
        }

        public void TriggerEnterGameEvent(int event_id, int game_seconds, string device_id, string game_version_name, string platform,
            string cur_event_times)
        {
            Call("TriggerEnterGameEvent", event_id, game_seconds, device_id, game_version_name, platform, cur_event_times);
        }

        public void TriggerEnterGameErrorEvent(int event_id, int game_seconds, string device_id, string game_version_name,
            string platform, string error, string cur_event_times)
        {
            Call("TriggerEnterGameErrorEvent", event_id, game_seconds, device_id, game_version_name, platform, error, cur_event_times);
        }
    }
}