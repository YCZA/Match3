using System.Collections;
using Match3.Scripts1.Puzzletown.Build;
using Match3.Scripts2.Android.DataStatistics;
using Match3.Scripts2.Env;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	[CreateAssetMenu(fileName = "ScriptOnCompleteFirstClick", menuName = "Puzzletown/Tutorials/Create/OnCompleteFirstClick")]
    public class TutorialCompleteFirstClick : ATutorialScript
    {
        protected override IEnumerator ExecuteRoutine()
        {
		    DataStatistics.Instance.TriggerEnterGameEvent(7, (int) Time.time, SystemInfo.deviceUniqueIdentifier, BuildVersion.Version, GameEnvironment.CurrentPlatform.ToString());
            yield break;
        }
    }
}