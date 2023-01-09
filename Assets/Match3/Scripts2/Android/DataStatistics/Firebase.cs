

// using Firebase.Analytics;

namespace Match3.Scripts2.Android.DataStatistics
{
    public class Firebase: IStatisticsTool
    {
        public void TriggerLevel(string play_mode, int level_number, int level_diff, int level_duration, string event_state,
            int times_buy_steps, int times_use_booster1, int times_use_booster2, int times_use_booster3, int times_use_booster,
            int remaining_steps_number)
        {
	        // Dictionary<int, string> level_name_prefix = new Dictionary<int, string>() {{0, "a"}, {1, "b"}, {2, "c"}};
	        // string level_name = level_name_prefix[level_diff] + level_number;
			// Parameter[] paras = new[]
			// {
			// 	new Parameter("play_mode", play_mode),
			// 	new Parameter("level_number", level_number),
			// 	new Parameter("level_diff", level_diff),
			// 	new Parameter("level_duration", level_duration),
			// 	new Parameter("event_state", event_state),
			// 	new Parameter("times_buy_steps", times_buy_steps),
			// 	new Parameter("times_use_booster1", times_use_booster1),
			// 	new Parameter("times_use_booster2", times_use_booster2),
			// 	new Parameter("times_use_booster3", times_use_booster3),
			// 	new Parameter("times_use_booster", times_use_booster),
			// 	new Parameter("remaining_steps_number", remaining_steps_number),
			// 	
			// 	new Parameter("level_name_custom", level_name),
			// 	new Parameter("level_diff_string", level_diff.ToString()),
			// 	new Parameter(FirebaseAnalytics.ParameterLevelName, "level " + level_name),
			// };
			// FirebaseAnalytics.LogEvent("level", paras);
        }

        public void TriggerPurchase(string currency, float priceNotCent, int quantity, string productName, string orderId,
	        string recepitId)
        {
        }

        public void TriggerTutorialComplete(int index)
        {
	        // FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventTutorialComplete, FirebaseAnalytics.ParameterIndex, index);
        }

        public void TriggerUseBooster(int level_id, string booster_name)
        {
			// Parameter[] paras = new[]
			// {
			// 	new Parameter("level_id", level_id),
			// 	new Parameter("booster_name", booster_name)
			// };
			// FirebaseAnalytics.LogEvent("use_booster", paras);
        }

        public void TriggerGetResources(string name, int amount, string way)
        {
			// Parameter[] paras = new[]
			// {
			// 	new Parameter("name", name),
			// 	new Parameter("amount", amount),
			// 	new Parameter("way", way)
			// };
			// FirebaseAnalytics.LogEvent("get_resources", paras);
        }

        public void TriggerConsumeResources(string name, int amount, string way)
        {
			// Parameter[] paras = new[]
			// {
			// 	new Parameter("name", name),
			// 	new Parameter("amount", amount),
			// 	new Parameter("way", way)
			// };
			// FirebaseAnalytics.LogEvent("consume_resources", paras);
        }

        public void TriggerViewItem(string item_name)
        {
			// FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventViewItem, FirebaseAnalytics.ParameterItemName, item_name);
        }

        public void TriggerGetWheelBounty()
        {
	        // FirebaseAnalytics.LogEvent("get_wheel_bounty");
        }

        public void TriggerBuyBankChest(int diamonds_number)
        {
	        // FirebaseAnalytics.LogEvent("buy_bank_chest", "diamonds_number", diamonds_number);
        }

        public void TriggerCompleteBankTask1()
        {
	        // FirebaseAnalytics.LogEvent("complete_bank_task1");
        }

        public void TriggerCompleteBankTask2()
        {
	        // FirebaseAnalytics.LogEvent("complete_bank_task2");
        }

        public void TriggerBuyBuilding(string name, int shop_tag)
        {
			// Parameter[] paras = new[]
			// {
			// 	new Parameter("name", name),
			// 	new Parameter("shop_tag", shop_tag),
			// };
			// FirebaseAnalytics.LogEvent("buy_building", paras);
        }

        public void TriggerBuyBooster(string booster_name)
        {
			// FirebaseAnalytics.LogEvent("buy_booster", "booster_name" , booster_name);
        }

        public void TriggerEnterGameEvent(int event_id, int game_seconds, string device_id, string game_version_name, string platform,
	        string cur_event_times)
        {
	  //       Parameter[] paras = new[]
	  //       {
		 //        new Parameter("game_seconds", game_seconds),
		 //        new Parameter("device_id", device_id),
		 //        new Parameter("game_version_name", game_version_name),
		 //        new Parameter("platform", platform),
		 //        new Parameter("cur_event_times", cur_event_times),
	  //       };
			// FirebaseAnalytics.LogEvent($"enter_game_step{event_id}", paras);
        }

        public void TriggerEnterGameErrorEvent(int event_id, int game_seconds, string device_id, string game_version_name,
	        string platform, string error, string cur_event_times)
        {
	  //       Parameter[] paras = new[]
	  //       {
		 //        new Parameter("game_seconds", game_seconds),
		 //        new Parameter("device_id", device_id),
		 //        new Parameter("game_version_name", game_version_name),
		 //        new Parameter("platform", platform),
		 //        new Parameter("error", error),
		 //        new Parameter("cur_event_times", cur_event_times),
	  //       };
			// FirebaseAnalytics.LogEvent($"enter_game_error{event_id}", paras);
        }
    }
}