using System.Collections;
using Match3.Scripts1.Puzzletown.Config;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200082F RID: 2095
	public class PirateBreakoutService : AWeeklyEventService
	{
		// Token: 0x0600341B RID: 13339 RVA: 0x000F8303 File Offset: 0x000F6703
		public PirateBreakoutService()
		{
			WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x0600341C RID: 13340 RVA: 0x000F8318 File Offset: 0x000F6718
		protected override WeeklyEventType WeeklyEventType
		{
			get
			{
				return WeeklyEventType.PirateBreakout;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x0600341D RID: 13341 RVA: 0x000F831B File Offset: 0x000F671B
		protected override AWeeklyEventDataService DataService
		{
			get
			{
				return this.gameStateService.PirateBreakout;
			}
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x000F8328 File Offset: 0x000F6728
		private IEnumerator InitRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			WooroutineRunner.StartCoroutine(this.UpdateRoutine(), null);
			WooroutineRunner.StartCoroutine(base.WaitForSbsRoutine(), null);
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x000F8344 File Offset: 0x000F6744
		public bool PirateBreakoutFeatureActiveAndEnabled()
		{
			return this.sbsService.SbsConfig.feature_switches.pirate_breakout && this.progressionDataService.UnlockedLevel >= this.gameStateService.PirateBreakout.UnlockLevel() && this.timeService.LocalNow > this.DataService.StartTime && this.timeService.LocalNow < this.DataService.EndTime && this.gameStateService.PirateBreakout.Level <= 10;
		}

		// Token: 0x06003420 RID: 13344 RVA: 0x000F83E5 File Offset: 0x000F67E5
		public bool AreAssetBundlesAvailable()
		{
			return this.areAssetBundlesAvailable;
		}

		// Token: 0x06003421 RID: 13345 RVA: 0x000F83ED File Offset: 0x000F67ED
		protected override void PersistNewActiveEvent(EventConfigContainer activeEvent)
		{
			activeEvent.config.levelSet = this.SanityCheckSetAndCrop(activeEvent.config.levelSet);
			base.PersistNewActiveEvent(activeEvent);
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x000F8412 File Offset: 0x000F6812
		protected override void OverwriteActiveEvent(EventConfigContainer updatedEvent)
		{
			updatedEvent.config.levelSet = this.SanityCheckSetAndCrop(updatedEvent.config.levelSet);
			base.OverwriteActiveEvent(updatedEvent);
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x000F8438 File Offset: 0x000F6838
		private int SanityCheckSetAndCrop(int levelSet)
		{
			int num = this.sbsService.SbsConfig.piratebreakoutsets.sets.Count / 10;
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

		// Token: 0x06003424 RID: 13348 RVA: 0x000F8480 File Offset: 0x000F6880
		private IEnumerator CheckBundles()
		{
			if (!this.areAssetBundlesAvailable)
			{
				Wooroutine<bool> checkForBundleAvailability = this.assetBundleService.AreAllBundlesAvailable(new string[]
				{
					"scene_pirate_breakout",
					"pirate_breakout_assets",
					"pirate_breakout_level"
				});
				yield return checkForBundleAvailability;
				this.areAssetBundlesAvailable = checkForBundleAvailability.ReturnValue;
			}
			yield break;
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x000F849C File Offset: 0x000F689C
		private IEnumerator UpdateRoutine()
		{
			while (!this.areAssetBundlesAvailable)
			{
				yield return this.CheckBundles();
				yield return new WaitForSeconds(3f);
			}
			yield break;
		}

		// Token: 0x04005BF6 RID: 23542
		public const string PIRATE_BREAKOUT_BUNDLE_NAME = "scene_pirate_breakout";

		// Token: 0x04005BF7 RID: 23543
		public const string PIRATE_BREAKOUT_ASSETS_BUNDLE_NAME = "pirate_breakout_assets";

		// Token: 0x04005BF8 RID: 23544
		public const string PIRATE_BREAKOUT_LEVELS_BUNDLE_NAME = "pirate_breakout_level";

		// Token: 0x04005BF9 RID: 23545
		public const string TROPHY_NAME = "iso_trophy_pirate_breakout";

		// Token: 0x04005BFA RID: 23546
		public const string TROPHY_TRACKING_DETAIL = "pirate_breakout";

		// Token: 0x04005BFB RID: 23547
		public const int NUMBER_OF_PB_LEVELS = 10;

		// Token: 0x04005BFC RID: 23548
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005BFD RID: 23549
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionDataService;

		// Token: 0x04005BFE RID: 23550
		[WaitForService(true, true)]
		private AssetBundleService assetBundleService;

		// Token: 0x04005BFF RID: 23551
		private bool areAssetBundlesAvailable;
	}
}
