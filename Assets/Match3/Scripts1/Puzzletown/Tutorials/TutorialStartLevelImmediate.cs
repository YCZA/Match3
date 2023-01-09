using System.Collections;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A89 RID: 2697
	[CreateAssetMenu(fileName = "ScriptStartLevelImmediate", menuName = "Puzzletown/Tutorials/Create/StartLevelImmediate")]
	public class TutorialStartLevelImmediate : ATutorialScript
	{
		// eli key point 立即开始关卡
		protected override IEnumerator ExecuteRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			Wooroutine<LevelConfig> config = this.configService.GetLevelConfig(this.area, this.level, LevelPlayMode.Regular, this.tier);
			yield return config;
			CoreGameFlow flow = new CoreGameFlow();
			CoreGameFlow.Input input = new CoreGameFlow.Input(-1, true, config.ReturnValue, LevelPlayMode.Regular);
			flow.Start(input);
		}

		// Token: 0x040069EA RID: 27114
		public int area = 1;

		// Token: 0x040069EB RID: 27115
		public int level = 1;

		// Token: 0x040069EC RID: 27116
		public AreaConfig.Tier tier;

		// Token: 0x040069ED RID: 27117
		[WaitForService(true, true)]
		private M3ConfigService configService;
	}
}
