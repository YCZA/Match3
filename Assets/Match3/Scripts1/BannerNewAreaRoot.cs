using System;
using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

// Token: 0x0200083F RID: 2111
namespace Match3.Scripts1
{
	public class BannerNewAreaRoot : APtSceneRoot, IDisposableDialog
	{
		// Token: 0x06003467 RID: 13415 RVA: 0x000FA700 File Offset: 0x000F8B00
		public static bool ShouldShow(QuestService quests, ProgressionDataService.Service progression)
		{
			bool flag = progression.LastUnlockedArea < quests.UnlockedAreaWithQuestAndEndOfContent;
			return flag && progression.UnlockedLevel > 1;
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x000FA730 File Offset: 0x000F8B30
		protected override IEnumerator GoRoutine()
		{
			int globalArea = this.quests.UnlockedAreaWithQuestAndEndOfContent;
			int localArea = this.sbs.SbsConfig.islandareaconfig.GlobalAreaToLocalArea(globalArea);
			Canvas canvas = base.GetComponentInChildren<Canvas>();
			Clouds[] clouds = this.town.GetComponentsInChildren<Clouds>();
			Clouds unlocked = Array.Find<Clouds>(clouds, (Clouds c) => c.mode == CloudsMode.Unlocked);
			if (unlocked)
			{
				unlocked.FadeOut(localArea);
			}
			this.progression.LastUnlockedArea = globalArea;
			EAHelper.AddBreadcrumb(string.Format("Repair areas on town load: {0}, LastUnlock Area {1} is set to UnlockedAreaWithQuestAndEndOfContent: {2}", this.sbs.SbsConfig.feature_switches.repair_areas, this.progression.LastUnlockedArea, globalArea));
			canvas.gameObject.SetActive(false);
			this.town.uiRoot.ShowUi(false);
			this.town.PopulateArea(globalArea, this.gameStateService.Buildings.IsAreaDeployed(globalArea));
			int previousArea = globalArea - 1;
			int prevIsland = this.sbs.SbsConfig.islandareaconfig.IslandForArea(previousArea);
			int currIsland = this.sbs.SbsConfig.islandareaconfig.IslandForArea(globalArea);
			float cameraPanTimeToNewArea = 4f;
			if (prevIsland == currIsland)
			{
				yield return new PanCameraFlow(PanCameraTarget.PreviousUnlockedArea, -100f, 0f, false).ExecuteRoutine();
			}
			else
			{
				cameraPanTimeToNewArea = 0f;
			}
			PanCameraTarget targetType = PanCameraTarget.LatestUnlockedArea;
			float time = cameraPanTimeToNewArea;
			yield return new PanCameraFlow(targetType, -100f, time, false).ExecuteRoutine();
			this.audioService.PlaySFX(AudioId.NewAreaUnlocked, false, false, false);
			canvas.enabled = true;
			canvas.gameObject.SetActive(true);
			if (this.progression.CurrentLevel == 11)
			{
				this.tracking.TrackFunnelEvent("515_area2_unlocked", 515, null);
				this.adjustService.TrackTutorialFinished();
			}
			yield return new WaitForSeconds(5f);
			this.town.uiRoot.ShowUi(true);
			base.Destroy();
			yield break;
		}

		// Token: 0x04005C40 RID: 23616
		[WaitForRoot(false, false)]
		private TownMainRoot town;

		// Token: 0x04005C41 RID: 23617
		[WaitForService(true, true)]
		private QuestService quests;

		// Token: 0x04005C42 RID: 23618
		[WaitForService(true, true)]
		private ProgressionDataService.Service progression;

		// Token: 0x04005C43 RID: 23619
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005C44 RID: 23620
		[WaitForService(true, true)]
		private TrackingService tracking;

		// Token: 0x04005C45 RID: 23621
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005C46 RID: 23622
		[WaitForService(true, true)]
		private IAdjustService adjustService;

		// Token: 0x04005C47 RID: 23623
		[WaitForService(true, true)]
		private SBSService sbs;

		// Token: 0x04005C48 RID: 23624
		private const float DISPLAY_TIME = 5f;

		// Token: 0x04005C49 RID: 23625
		private const float CAMERA_PAN_TIME = 4f;
	}
}
