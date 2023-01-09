using AndroidTools;
using Match3.Scripts1.Puzzletown.Build;
using Match3.Scripts2.Env;
using UnityEngine;

namespace Match3.Scripts2.Android.DataStatistics
{
    public class DataStatisticsHandler : AndroidJavaProxy
    {
        public DataStatisticsHandler() : base("com.unity3d.utils.IDataStatisticsHandler")
        {
            
        }

        public void TriggerEnterGameErrorEvent(int event_id, string error)
        {
            lock (AndroidTools.AndroidAgent.eventQueue)
            {
                AndroidTools.AndroidAgent.eventQueue.Enqueue(() => 
                    DataStatistics.Instance.TriggerEnterGameErrorEvent(event_id, (int) Time.time,
                        SystemInfo.deviceUniqueIdentifier, BuildVersion.Version,
                        GameEnvironment.CurrentPlatform.ToString(), error)
                    );
            }
        }
    }
}