using System.Collections.Generic;
using AndroidTools;
using UnityEngine;

namespace Match3.Scripts2.Android.DataStatistics
{
    public class DataStatistics: IStatisticsTool
    {
        public List<IStatisticsTool> toolList = new List<IStatisticsTool>();

        public LevelData levelData = new LevelData();

        private static DataStatistics instance = new DataStatistics();
        public static DataStatistics Instance => instance;

        public DataStatisticsHandler dataStatisticsHandler = new DataStatisticsHandler();
        
        private DataStatistics()
        {
            toolList.Add(new Firebase());
            toolList.Add(new TD(AndroidTools.AndroidAgent.Call<AndroidJavaObject>("getTd")));
            toolList.Add(new AF());
            toolList.Add(new GameServer());
        }
        
        public void TriggerLevel(string play_mode, int level_number, int level_diff, int level_duration, string event_state,
            int times_buy_steps, int times_use_booster1, int times_use_booster2, int times_use_booster3, int times_use_booster,
            int remaining_steps_number)
        {
            
		    UnityEngine.Debug.Log($"埋点: 关卡, 关卡模式:{play_mode}, 关卡编号:{level_number}, 关卡难度:{level_diff}, 关卡时长:{level_duration}, 状态:{event_state}, 买步数次数:{times_buy_steps}, 道具1使用次数:{times_use_booster1}, 道具2使用次数:{times_use_booster2}, 道具3使用次数:{times_use_booster3}, 道具使用总次数:{times_use_booster}, 剩余步数:{remaining_steps_number}");
            foreach (var tool in toolList)
            {
               tool.TriggerLevel(play_mode, level_number, level_diff, level_duration, event_state, times_buy_steps, times_use_booster1, times_use_booster2, times_use_booster3, times_use_booster, remaining_steps_number);
            }
        }

        public void TriggerPurchase(string currency, float priceNotCent, int quantity, string productName, string orderId,
            string recepitId)
        {
		    UnityEngine.Debug.Log($"埋点: IAP, 货币类型:{currency}, 价格:{priceNotCent}, 数量:{quantity}, 商品名称:{productName}, 订单id:{orderId}, 票据id:{recepitId}");
            foreach (var tool in toolList)
            {
                tool.TriggerPurchase(currency, priceNotCent, quantity, productName, orderId, recepitId);
            }
        }

        public void TriggerTutorialComplete(int index)
        {
            UnityEngine.Debug.Log("埋点: 完成引导 , 当前引导的索引：" + index);
            foreach (var tool in toolList)
            {
                tool.TriggerTutorialComplete(index);
            }
        }

        public void TriggerUseBooster(int level_id, string booster_name)
        {
            UnityEngine.Debug.Log($"埋点: 使用道具 道具名称: {booster_name} 当前关卡: {level_id}");
            foreach (var tool in toolList)
            {
                tool.TriggerUseBooster(level_id, booster_name);
            }
        }

        public void TriggerGetResources(string name, int amount, string way)
        {
            UnityEngine.Debug.Log($"埋点: 获得资源 资源名称: {name} 数量: {amount} 途径: {way}");
            foreach (var tool in toolList)
            {
                tool.TriggerGetResources(name, amount, way);
            }
        }

        public void TriggerConsumeResources(string name, int amount, string way)
        {
            UnityEngine.Debug.Log($"埋点: 消耗资源 资源名称: {name} 数量: {amount} 途径: {way}");
            foreach (var tool in toolList)
            {
                tool.TriggerConsumeResources(name, amount, way);
            }
        }

        public void TriggerViewItem(string item_name)
        {
		    UnityEngine.Debug.Log("埋点: 查看item , 当前item名称：" + item_name);
            foreach (var tool in toolList)
            {
                tool.TriggerViewItem(item_name);
            }
        }

        public void TriggerGetWheelBounty()
        {
            UnityEngine.Debug.Log("埋点: 领取转盘赏金");
            foreach (var tool in toolList)
            {
                tool.TriggerGetWheelBounty();
            }
        }

        public void TriggerBuyBankChest(int diamonds_number)
        {
            UnityEngine.Debug.Log("埋点: 购买银行宝箱, 宝箱内钻石数：" + diamonds_number);
            foreach (var tool in toolList)
            {
                tool.TriggerBuyBankChest(diamonds_number);
            }
        }

        public void TriggerCompleteBankTask1()
        {
            UnityEngine.Debug.Log("埋点: 完成银行任务1");
            foreach (var tool in toolList)
            {
                tool.TriggerCompleteBankTask1();
            }
        }

        public void TriggerCompleteBankTask2()
        {
            UnityEngine.Debug.Log("埋点: 完成银行任务2");
            foreach (var tool in toolList)
            {
                tool.TriggerCompleteBankTask2();
            }
        }

        public void TriggerBuyBuilding(string name, int shop_tag)
        {
            UnityEngine.Debug.Log("埋点: 购买建筑, 建筑名称:" + name + " 商店标签:" + shop_tag);
            foreach (var tool in toolList)
            {
                tool.TriggerBuyBuilding(name, shop_tag);
            }
        }

        public void TriggerBuyBooster(string booster_name)
        {
			UnityEngine.Debug.Log($"埋点: 购买道具 道具名称: {booster_name}");
            foreach (var tool in toolList)
            {
                tool.TriggerBuyBooster(booster_name);
            }
        }

        private string GetEventTimes(string key)
        {
	        int times = PlayerPrefs.GetInt(key, 0);
	        PlayerPrefs.SetInt(key, ++times);
            return times.ToString();
        }
        
        // 不需要传入"cur_event_times"
        // 1 打开游戏
        // 2 点击了开始按钮, 开始登录
        // 3 登录成功
        // 4 拉取存档
        // 5 拉取存档结束
        // 6 进入游戏
        // 7 第一次点击对话
        public void TriggerEnterGameEvent(int event_id, int game_seconds, string device_id, string game_version_name, string platform,
            string cur_event_times = "")
        {
	        string key = $"tc:event:enter_game_event{event_id}:times";
            cur_event_times = GetEventTimes(key);
            
			UnityEngine.Debug.Log($"埋点: enter_game_event{event_id} game_seconds:{game_seconds}, device_id:{device_id}, game_version_name:{game_version_name}, platform:{platform},  cur_event_times:{cur_event_times}");
            foreach (var tool in toolList)
            {
                tool.TriggerEnterGameEvent(event_id, game_seconds, device_id, game_version_name, platform, cur_event_times);
            }
        }

        // 1 获取到的idtoken为null
        // 2 登录华为账号失败，或取消登录
        public void TriggerEnterGameErrorEvent(int event_id, int game_seconds, string device_id, string game_version_name,
            string platform, string error, string cur_event_times = "")
        {
	        string key = $"tc:event:enter_game_error{event_id}:times";
            cur_event_times = GetEventTimes(key);
            
			UnityEngine.Debug.Log($"埋点: enter_game_error_event{event_id} game_seconds:{game_seconds}, device_id:{device_id}, game_version_name:{game_version_name}, platform:{platform}, error:{error}, cur_event_times:{cur_event_times}");
            foreach (var tool in toolList)
            {
                tool.TriggerEnterGameErrorEvent(event_id, game_seconds, device_id, game_version_name, platform, error, cur_event_times);
            }
        }
    }
}