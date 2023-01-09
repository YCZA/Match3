

// using AppsFlyerSDK;

namespace Match3.Scripts2.Android.DataStatistics
{
    public class AF: IStatisticsTool
    {
        public void TriggerLevel(string play_mode, int level_number, int level_diff, int level_duration, string event_state,
            int times_buy_steps, int times_use_booster1, int times_use_booster2, int times_use_booster3, int times_use_booster,
            int remaining_steps_number)
        {
            // Dictionary<string, string> dic = new Dictionary<string, string> ();
            // dic.Add("play_mode", play_mode);
            // dic.Add("level_number", level_number.ToString());
            // dic.Add("level_diff", level_diff.ToString());
            // dic.Add("level_duration", level_duration.ToString());
            // dic.Add("event_state", event_state);
            // dic.Add("times_buy_steps", times_buy_steps.ToString());
            // dic.Add("times_use_booster1", times_use_booster1.ToString());
            // dic.Add("times_use_booster2", times_use_booster2.ToString());
            // dic.Add("times_use_booster3", times_use_booster3.ToString());
            // dic.Add("times_use_booster", times_use_booster.ToString());
            // dic.Add("remaining_steps_number", remaining_steps_number.ToString());
            // AppsFlyer.sendEvent ("level", dic);
        }

        // 价格如200.12, 使用元为单位
        public void TriggerPurchase(string currency, float priceNotCent, int quantity, string productName, string orderId, string recepitId)
        {
            // Dictionary<string, string> purchaseEvent = new Dictionary<string, string> ();
            // purchaseEvent.Add(AFInAppEvents.CURRENCY, currency);
            // purchaseEvent.Add(AFInAppEvents.REVENUE, priceNotCent.ToString());
            // purchaseEvent.Add(AFInAppEvents.QUANTITY, quantity.ToString()); // 数量
            // purchaseEvent.Add(AFInAppEvents.CONTENT_TYPE, productName);    // 商品名
            // purchaseEvent.Add(AFInAppEvents.ORDER_ID, orderId);    // 订单id
            // purchaseEvent.Add(AFInAppEvents.RECEIPT_ID, recepitId);  // 收据id
            // AppsFlyer.sendEvent ("af_purchase", purchaseEvent);
        }

        public void TriggerTutorialComplete(int index)
        {
        //     Dictionary<string, string> dic = new Dictionary<string, string> ();
        //     dic.Add("index", index.ToString());
        //     AppsFlyer.sendEvent ("tutorial_complete", dic);
        }

        public void TriggerUseBooster(int level_id, string booster_name)
        {
            // Dictionary<string, string> dic = new Dictionary<string, string> ();
            // dic.Add("level_id", level_id.ToString());
            // dic.Add("booster_name", booster_name);
            // AppsFlyer.sendEvent ("use_booster", dic);
        }

        public void TriggerGetResources(string name, int amount, string way)
        {
            // Dictionary<string, string> dic = new Dictionary<string, string> ();
            // dic.Add("name", name);
            // dic.Add("amount", amount.ToString());
            // dic.Add("way", way);
            // AppsFlyer.sendEvent ("get_resources", dic);
        }

        public void TriggerConsumeResources(string name, int amount, string way)
        {
            // Dictionary<string, string> dic = new Dictionary<string, string> ();
            // dic.Add("name", name);
            // dic.Add("amount", amount.ToString());
            // dic.Add("way", way);
            // AppsFlyer.sendEvent ("consume_resources", dic);
        }

        public void TriggerViewItem(string item_name)
        {
            // Dictionary<string, string> dic = new Dictionary<string, string> ();
            // dic.Add("item_name", item_name);
            // AppsFlyer.sendEvent ("view_item", dic);
        }

        public void TriggerGetWheelBounty()
        {
            // Dictionary<string, string> dic = new Dictionary<string, string> ();
            // AppsFlyer.sendEvent ("get_wheel_bounty", dic);
        }

        public void TriggerBuyBankChest(int diamonds_number)
        {
            // Dictionary<string, string> dic = new Dictionary<string, string> ();
            // dic.Add("diamonds_number", diamonds_number.ToString());
            // AppsFlyer.sendEvent ("buy_bank_chest", dic);
        }

        public void TriggerCompleteBankTask1()
        {
            // Dictionary<string, string> dic = new Dictionary<string, string> ();
            // AppsFlyer.sendEvent ("complete_bank_task1", dic);
        }

        public void TriggerCompleteBankTask2()
        {
            // Dictionary<string, string> dic = new Dictionary<string, string> ();
            // AppsFlyer.sendEvent ("complete_bank_task2", dic);
        }

        public void TriggerBuyBuilding(string name, int shop_tag)
        {
            // Dictionary<string, string> dic = new Dictionary<string, string> ();
            // dic.Add("name", name);
            // dic.Add("shop_tag", shop_tag.ToString());
            // AppsFlyer.sendEvent ("buy_building", dic);
        }

        public void TriggerBuyBooster(string booster_name)
        {
            // Dictionary<string, string> dic = new Dictionary<string, string> ();
            // dic.Add("booster_name", booster_name);
            // AppsFlyer.sendEvent ("buy_booster", dic);
        }

        public void TriggerEnterGameEvent(int event_id, int game_seconds, string device_id, string game_version_name, string platform,
            string cur_event_times)
        {
            // Dictionary<string, string> dic = new Dictionary<string, string> ();
            // dic.Add("game_seconds", game_seconds.ToString());
            // dic.Add("device_id", device_id.ToString());
            // dic.Add("game_version_name", game_version_name.ToString());
            // dic.Add("platform", platform.ToString());
            // dic.Add("cur_event_times", cur_event_times.ToString());
            // AppsFlyer.sendEvent($"enter_game_step{event_id}", dic);
        }

        public void TriggerEnterGameErrorEvent(int event_id, int game_seconds, string device_id, string game_version_name,
            string platform, string error, string cur_event_times)
        {
            // Dictionary<string, string> dic = new Dictionary<string, string> ();
            // dic.Add("game_seconds", game_seconds.ToString());
            // dic.Add("device_id", device_id.ToString());
            // dic.Add("game_version_name", game_version_name.ToString());
            // dic.Add("platform", platform.ToString());
            // dic.Add("error", error.ToString());
            // dic.Add("cur_event_times", cur_event_times.ToString());
            // AppsFlyer.sendEvent($"enter_game_error{event_id}", dic);
        }
    }
}