using System.Collections;
using Match3.Scripts1.Puzzletown.Config;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200076A RID: 1898
	public class DiveForTreasureService : AWeeklyEventService
	{
		// Token: 0x06002F17 RID: 12055 RVA: 0x000DBF92 File Offset: 0x000DA392
		public DiveForTreasureService()
		{
			WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002F18 RID: 12056 RVA: 0x000DBFA7 File Offset: 0x000DA3A7
		protected override WeeklyEventType WeeklyEventType
		{
			get
			{
				return WeeklyEventType.DiveForTreasure;
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002F19 RID: 12057 RVA: 0x000DBFAA File Offset: 0x000DA3AA
		protected override AWeeklyEventDataService DataService
		{
			get
			{
				return this.gameStateService.DiveForTreasure;
			}
		}

		// Token: 0x06002F1A RID: 12058 RVA: 0x000DBFB8 File Offset: 0x000DA3B8
		private IEnumerator InitRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			WooroutineRunner.StartCoroutine(this.UpdateRoutine(), null);
			WooroutineRunner.StartCoroutine(base.WaitForSbsRoutine(), null);
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x06002F1B RID: 12059 RVA: 0x000DBFD3 File Offset: 0x000DA3D3
		public bool FeatureAvailable()
		{
			return this.sbsService.SbsConfig.feature_switches.dive_for_treasure && this.progressionDataService.UnlockedLevel >= this.DataService.UnlockLevel();
		}

		// Token: 0x06002F1C RID: 12060 RVA: 0x000DC010 File Offset: 0x000DA410
		public bool DiveForTreasureFeatureActiveAndEnabled()
		{
			return this.FeatureAvailable() && this.timeService.LocalNow > this.DataService.StartTime && this.timeService.LocalNow < this.DataService.EndTime && this.gameStateService.DiveForTreasure.Level <= 8;
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x000DC081 File Offset: 0x000DA481
		public bool AreAssetBundlesAvailable()
		{
			return this.areAssetBundlesAvailable;
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x000DC08C File Offset: 0x000DA48C
		public void ResetToCheckPoint()
		{
			int num = this.gameStateService.DiveForTreasure.CheckPoint();
			this.gameStateService.DiveForTreasure.Level = ((this.gameStateService.DiveForTreasure.Level <= num) ? 1 : (num + 1));
		}

		// Token: 0x06002F1F RID: 12063 RVA: 0x000DC0D9 File Offset: 0x000DA4D9
		protected override void PersistNewActiveEvent(EventConfigContainer activeEvent)
		{
			activeEvent.config.levelSet = this.SanityCheckSetAndCrop(activeEvent.config.levelSet);
			base.PersistNewActiveEvent(activeEvent);
		}

		// Token: 0x06002F20 RID: 12064 RVA: 0x000DC0FE File Offset: 0x000DA4FE
		protected override void OverwriteActiveEvent(EventConfigContainer updatedEvent)
		{
			updatedEvent.config.levelSet = this.SanityCheckSetAndCrop(updatedEvent.config.levelSet);
			base.OverwriteActiveEvent(updatedEvent);
		}

		// Token: 0x06002F21 RID: 12065 RVA: 0x000DC124 File Offset: 0x000DA524
		private int SanityCheckSetAndCrop(int levelSet)
		{
			int num = this.sbsService.SbsConfig.divefortreasuresets.sets.Count / 8;
			if (levelSet > num)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Level set is higher than the available level set, reset it to the last"
				});
				return num;
			}
			return levelSet;
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x000DC16C File Offset: 0x000DA56C
		private IEnumerator CheckBundles()
		{
			if (!this.areAssetBundlesAvailable)
			{
				Wooroutine<bool> checkForBundleAvailability = this.assetBundleService.AreAllBundlesAvailable(new string[]
				{
					"scene_dive_for_treasure",
					"dive_for_treasure_level"
				});
				yield return checkForBundleAvailability;
				this.areAssetBundlesAvailable = checkForBundleAvailability.ReturnValue;
			}
			yield break;
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x000DC188 File Offset: 0x000DA588
		private IEnumerator UpdateRoutine()
		{
			while (!this.areAssetBundlesAvailable)
			{
				yield return this.CheckBundles();
				yield return new WaitForSeconds(3f);
			}
			yield break;
		}

		// Token: 0x04005831 RID: 22577
		public const string DIVE_FOR_TREASURE_BUNDLE_NAME = "scene_dive_for_treasure";

		// Token: 0x04005832 RID: 22578
		public const string DIVE_FOR_TREASURE_LEVELS_BUNDLE_NAME = "dive_for_treasure_level";

		// Token: 0x04005833 RID: 22579
		public const string TROPHY_NAME = "iso_trophy_treasure_dive";

		// Token: 0x04005834 RID: 22580
		public const string TROPHY_TRACKING_DETAIL = "treasure_diving";

		// Token: 0x04005835 RID: 22581
		public const int NUMBER_OF_DFT_LEVELS = 8;

		// Token: 0x04005836 RID: 22582
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005837 RID: 22583
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionDataService;

		// Token: 0x04005838 RID: 22584
		[WaitForService(true, true)]
		private AssetBundleService assetBundleService;

		// Token: 0x04005839 RID: 22585
		private bool areAssetBundlesAvailable;
	}
}
