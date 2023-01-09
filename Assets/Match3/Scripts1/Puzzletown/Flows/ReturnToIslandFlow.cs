using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// eli key point 退出关卡
	public class ReturnToIslandFlow
	{
		// Token: 0x06002201 RID: 8705 RVA: 0x00091B94 File Offset: 0x0008FF94
		public ReturnToIslandFlow(Match3Score score = null, Action action = null)
		{
			this.score = score;
			this.action = action;
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x00091BAA File Offset: 0x0008FFAA
		public Coroutine Execute()
		{
			return WooroutineRunner.StartCoroutine(this.FlowRoutine(), null);
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x00091BB8 File Offset: 0x0008FFB8
		private IEnumerator FlowRoutine()
		{
			Debug.Log("返回家园流程开始");
			yield return ServiceLocator.Instance.Inject(this);
			if (Application.internetReachability != NetworkReachability.NotReachable)
			{
				this.assetBundleService.PrepareToRetryFailedDownloads();
			}
			TownLoadingFlowWithTransition.Input lScreenInput = new TownLoadingFlowWithTransition.Input(LoadingScreenConfig.Random);
			// Wooroutine<TownMainRoot> returnToTownFlow = new TownLoadingFlowWithTransition().Start(lScreenInput);
			// yield return returnToTownFlow;
			Resources.UnloadUnusedAssets();
			// TownMainRoot townScene = returnToTownFlow.ReturnValue;
			if (this.score != null)
			{
				if (this.score.success && !this.score.showLevelMap)
				{
					// townScene.SpawnRewardDoobers(this.score.Rewards);
				}
				// townScene.villagers.HandleLevelExit(this.score.success);
			}
			Application.targetFrameRate = FPSService.TargetFrameRate;
			// townScene.StartView(true, false);
			// this.tournamentService.NotifyContextChange(SceneContext.MetaGame);
			// this.levelOfDayService.NotifyContextChange(SceneContext.MetaGame);
			if (this.action != null)
			{
				this.action();
			}
			
			Debug.Log("返回家园流程结束");
		}

		// Token: 0x04004D54 RID: 19796
		[WaitForService(true, true)]
		private AssetBundleService assetBundleService;

		// Token: 0x04004D55 RID: 19797
		// [WaitForService(true, true)]
		// private TournamentService tournamentService;

		// Token: 0x04004D56 RID: 19798
		// [WaitForService(true, true)]
		// private LevelOfDayService levelOfDayService;

		// Token: 0x04004D57 RID: 19799
		// [WaitForService(true, true)]
		// private IAdjustService adjust;

		// Token: 0x04004D58 RID: 19800
		private Match3Score score;

		// Token: 0x04004D59 RID: 19801
		private Action action;
	}
}
