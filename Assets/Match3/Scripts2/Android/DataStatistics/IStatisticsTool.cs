namespace Match3.Scripts2.Android.DataStatistics
{
    public interface IStatisticsTool
    {
        void TriggerLevel(string play_mode, int level_number, int level_diff, int level_duration, string event_state, int times_buy_steps, 
            int times_use_booster1, int times_use_booster2, int times_use_booster3, int times_use_booster, int remaining_steps_number);

        void TriggerPurchase(string currency, float priceNotCent, int quantity, string productName, string orderId, string recepitId);

        void TriggerTutorialComplete(int index);

        void TriggerUseBooster(int level_id, string booster_name);
        
        void TriggerGetResources(string name, int amount, string way);
        
        void TriggerConsumeResources(string name, int amount, string way);
        
        void TriggerViewItem(string item_name);
        
        void TriggerGetWheelBounty();

        void TriggerBuyBankChest(int diamonds_number);

        void TriggerCompleteBankTask1();
        
        void TriggerCompleteBankTask2();

        void TriggerBuyBuilding(string name, int shop_tag);

        void TriggerBuyBooster(string booster_name);
        
        // 进入游戏事件

        #region EnterGameEvent
        // 参数: 进游戏后的秒数, 设备id, 游戏版本名称, 第几次执行这个步骤, 报错信息(可选)
        void TriggerEnterGameEvent(int event_id, int game_seconds, string device_id, string game_version_name, string platform, string cur_event_times);

        void TriggerEnterGameErrorEvent(int event_id, int game_seconds, string device_id, string game_version_name, string platform, string error, string cur_event_times);

        #endregion
    }
}