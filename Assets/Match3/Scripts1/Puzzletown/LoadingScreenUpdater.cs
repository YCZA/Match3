using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown
{
	// Token: 0x02000836 RID: 2102
	public class LoadingScreenUpdater
	{
		// Token: 0x06003452 RID: 13394 RVA: 0x000FA0E6 File Offset: 0x000F84E6
		public LoadingScreenUpdater(LoadingScreenRoot loadingScreen)
		{
			this.loadingScreen = loadingScreen;
			WooroutineRunner.StartCoroutine(this.MonitorProgressRoutine(), null);
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x000FA110 File Offset: 0x000F8510
		private IEnumerator MonitorProgressRoutine()
		{
			ServiceLocator services = ServiceLocator.Instance;
			yield return this.AddIfComplete(this.AwaitAuthentication(), LoadingScreenUpdater.Step.Authenticated);
			yield return this.AddIfComplete(services.Await<SBSService>(true), LoadingScreenUpdater.Step.SbsInitialized);
			yield return this.AddIfComplete(services.Await<GameStateService>(true), LoadingScreenUpdater.Step.GameStateLoaded);
			yield return this.AddIfComplete(services.Await<AssetBundleService>(true), LoadingScreenUpdater.Step.AssetBundlesInitialized);
			yield return this.AddIfComplete(services.Await<ConfigService>(true), LoadingScreenUpdater.Step.ConfigsLoaded);
			yield return this.AddIfComplete(SceneManager.Instance.Await<BuildingResourceServiceRoot>(true), LoadingScreenUpdater.Step.BuildingsLoaded);
			yield break;
		}

		// Token: 0x06003454 RID: 13396 RVA: 0x000FA12C File Offset: 0x000F852C
		private IEnumerator AwaitAuthentication()
		{
			// while (!SBS.IsAuthenticated())
			// {
				// yield return null;
			// }
			// 无需认证
			yield break;
		}

		// Token: 0x06003455 RID: 13397 RVA: 0x000FA140 File Offset: 0x000F8540
		private IEnumerator AddIfComplete(IEnumerator routine, LoadingScreenUpdater.Step step)
		{
			yield return routine;
			this.completedSteps.Add(step);
			LoadingScreenUpdater.ProgressInfo info = new LoadingScreenUpdater.ProgressInfo
			{
				completedStep = step,
				numTotalSteps = Enum.GetValues(typeof(LoadingScreenUpdater.Step)).Length,
				numCompletedSteps = this.completedSteps.Count
			};
			this.loadingScreen.UpdateProgress(info);
		}

		// Token: 0x04005C2F RID: 23599
		private List<LoadingScreenUpdater.Step> completedSteps = new List<LoadingScreenUpdater.Step>();

		// Token: 0x04005C30 RID: 23600
		private LoadingScreenRoot loadingScreen;

		// Token: 0x02000837 RID: 2103
		public struct ProgressInfo
		{
			// Token: 0x1700083E RID: 2110
			// (get) Token: 0x06003456 RID: 13398 RVA: 0x000FA169 File Offset: 0x000F8569
			public float Progress
			{
				get
				{
					return (float)this.numCompletedSteps / (float)this.numTotalSteps;
				}
			}

			// Token: 0x04005C31 RID: 23601
			public LoadingScreenUpdater.Step completedStep;

			// Token: 0x04005C32 RID: 23602
			public int numTotalSteps;

			// Token: 0x04005C33 RID: 23603
			public int numCompletedSteps;
		}

		// Token: 0x02000838 RID: 2104
		public enum Step
		{
			// Token: 0x04005C35 RID: 23605
			Authenticated,
			// Token: 0x04005C36 RID: 23606
			SbsInitialized,
			// Token: 0x04005C37 RID: 23607
			GameStateLoaded,
			// Token: 0x04005C38 RID: 23608
			AssetBundlesInitialized,
			// Token: 0x04005C39 RID: 23609
			BuildingsLoaded,
			// Token: 0x04005C3A RID: 23610
			ConfigsLoaded
		}
	}
}
