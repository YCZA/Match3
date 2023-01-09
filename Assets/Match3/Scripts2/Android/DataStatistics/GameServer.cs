using System.Collections.Generic;
using Match3.Scripts2.Network;
using Wooga.Newtonsoft.Json;
using UnityEngine;

namespace Match3.Scripts2.Android.DataStatistics
{
    public class GameServer: IStatisticsTool
    {
        public void TriggerLevel(string play_mode, int level_number, int level_diff, int level_duration, string event_state,
            int times_buy_steps, int times_use_booster1, int times_use_booster2, int times_use_booster3, int times_use_booster,
            int remaining_steps_number)
        {
        }

        public void TriggerPurchase(string currency, float priceNotCent, int quantity, string productName, string orderId,
            string recepitId)
        {
        }

        public void TriggerTutorialComplete(int index)
        {
        }

        public void TriggerUseBooster(int level_id, string booster_name)
        {
        }

        public void TriggerGetResources(string name, int amount, string way)
        {
        }

        public void TriggerConsumeResources(string name, int amount, string way)
        {
        }

        public void TriggerViewItem(string item_name)
        {
        }

        public void TriggerGetWheelBounty()
        {
        }

        public void TriggerBuyBankChest(int diamonds_number)
        {
        }

        public void TriggerCompleteBankTask1()
        {
        }

        public void TriggerCompleteBankTask2()
        {
        }

        public void TriggerBuyBuilding(string name, int shop_tag)
        {
        }

        public void TriggerBuyBooster(string booster_name)
        {
        }

        private bool IsEnable()
        {
            return !Application.isEditor;
        }

        // 以json的形式发到服务器
        public void TriggerEnterGameEvent(int event_id, int game_seconds, string device_id, string game_version_name, string platform,
            string cur_event_times)
        {
            if (!IsEnable())
                return;
            
            WWWForm form = new WWWForm();
            string event_name = $"enter_game_step{event_id}";
            form.AddField("event_name", event_name);

            var jsonDic = new Dictionary<string, object>();
            jsonDic.Add("event_name", event_name);
            jsonDic.Add("game_seconds", game_seconds);
            jsonDic.Add("device_id", device_id);
            jsonDic.Add("game_version_name", game_version_name);
            jsonDic.Add("platform", platform);
            jsonDic.Add("cur_event_times", cur_event_times);
            form.AddField("event_content", JsonConvert.SerializeObject(jsonDic));
            
            ToServer.Instance.PushEventInfo(form, null, null);
        }

        public void TriggerEnterGameErrorEvent(int event_id, int game_seconds, string device_id, string game_version_name,
            string platform, string error, string cur_event_times)
        {
            if (!IsEnable())
                return;
            
            WWWForm form = new WWWForm();
            string event_name = $"enter_game_error{event_id}";
            form.AddField("event_name", event_name);

            var jsonDic = new Dictionary<string, object>();
            jsonDic.Add("event_name", event_name);
            jsonDic.Add("game_seconds", game_seconds);
            jsonDic.Add("device_id", device_id);
            jsonDic.Add("game_version_name", game_version_name);
            jsonDic.Add("platform", platform);
            jsonDic.Add("error", error);
            jsonDic.Add("cur_event_times", cur_event_times);
            form.AddField("event_content", JsonConvert.SerializeObject(jsonDic));
            
            ToServer.Instance.PushEventInfo(form, null, null);
        }
    }
}