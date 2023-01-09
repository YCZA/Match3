using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A2D RID: 2605
	public class SeasonalPromoBadgeUi : MonoBehaviour
	{
		// Token: 0x06003EA1 RID: 16033 RVA: 0x0013E618 File Offset: 0x0013CA18
		public void Init(TownBottomPanelRoot townBottomPanel)
		{
			WooroutineRunner.StartCoroutine(this.SetupRoutine(townBottomPanel), null);
		}

		// Token: 0x06003EA2 RID: 16034 RVA: 0x0013E628 File Offset: 0x0013CA28
		private IEnumerator SetupRoutine(TownBottomPanelRoot tBPRoot)
		{
			this.townBottomPanelRoot = tBPRoot;
			if (!this.initialized)
			{
				yield return ServiceLocator.Instance.Inject(this);
			}
			this.Refresh();
			this.AddSlowUpdate(new SlowUpdate(this.Refresh), 3);
			yield break;
		}

		// Token: 0x06003EA3 RID: 16035 RVA: 0x0013E64A File Offset: 0x0013CA4A
		private void Refresh()
		{
			this.SetupNotification();
			base.StartCoroutine(this.RefreshRoutine());
		}

		// Token: 0x06003EA4 RID: 16036 RVA: 0x0013E660 File Offset: 0x0013CA60
		private IEnumerator RefreshRoutine()
		{
			Wooroutine<bool> bundlesAvailableRoutine = this.seasonService.AreAllActiveSeasonBundlesAvailable();
			yield return bundlesAvailableRoutine;
			SeasonConfig season = this.seasonService.GetActiveSeason();
			bool active = this.seasonService.IsActive && season != null && this.seasonService.GetGrandPrizeBuildingConfig() != null && bundlesAvailableRoutine.ReturnValue;
			if (active != base.gameObject.activeSelf || !this.initialized)
			{
				if (active)
				{
					yield return this.SetupSeason(season);
				}
				base.gameObject.SetActive(active);
				this.initialized = true;
			}
			yield break;
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x0013E67C File Offset: 0x0013CA7C
		private IEnumerator SetupSeason(SeasonConfig season)
		{
			this.timer.SetTargetTime(season.EndDate, true, null);
			Wooroutine<SpriteManager> spriteManagerRoutine = new SeasonSpriteManagerFlow().Start<SpriteManager>();
			yield return spriteManagerRoutine;
			if (spriteManagerRoutine.ReturnValue != null)
			{
				string substring = "hud" + "_" + "season_currency";
				this.icon.sprite = spriteManagerRoutine.ReturnValue.GetSimilar(substring);
				if (this.icon.sprite == null)
				{
					this.icon.sprite = spriteManagerRoutine.ReturnValue.GetSimilar("season_currency");
					string primary = season.Primary;
					string substring2 = string.Concat(new string[]
					{
						"season_currency",
						"_",
						primary,
						"_",
						"glow"
					});
					this.glow.sprite = spriteManagerRoutine.ReturnValue.GetSimilar(substring2);
				}
				this.glow.gameObject.SetActive(this.glow.sprite != null);
			}
			yield break;
		}

		// Token: 0x06003EA6 RID: 16038 RVA: 0x0013E6A0 File Offset: 0x0013CAA0
		private void SetupNotification()
		{
			bool active = true;
			if (this.gameStateService.IsSeenFlagTimestampSet("seasonalPromoSeen"))
			{
				DateTime timeStamp = this.gameStateService.GetTimeStamp("seasonalPromoSeen");
				active = (DateTime.UtcNow > timeStamp.AddSeconds((double)this.configService.general.notifications.attention_indicator_cooldown));
			}
			this.notificationBlob.SetActive(active);
		}

		// Token: 0x06003EA7 RID: 16039 RVA: 0x0013E709 File Offset: 0x0013CB09
		public void OnButtonTap()
		{
			if (this.townBottomPanelRoot == null || !this.townBottomPanelRoot.IsInteractable)
			{
				return;
			}
			this.notificationBlob.SetActive(false);
			base.StartCoroutine(this.ShowSeasonalPromoPopupRoutine());
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x0013E748 File Offset: 0x0013CB48
		private IEnumerator ShowSeasonalPromoPopupRoutine()
		{
			Wooroutine<PopupSeasonalPromoRoot> scene = SceneManager.Instance.LoadScene<PopupSeasonalPromoRoot>(null);
			yield return scene;
			yield break;
		}

		// Token: 0x040067C1 RID: 26561
		[WaitForService(true, true)]
		private SeasonService seasonService;

		// Token: 0x040067C2 RID: 26562
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x040067C3 RID: 26563
		[WaitForService(true, true)]
		private TimeService timeService;

		// Token: 0x040067C4 RID: 26564
		[WaitForService(true, true)]
		private AssetBundleService assetBundleService;

		// Token: 0x040067C5 RID: 26565
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040067C6 RID: 26566
		[SerializeField]
		private CountdownTimer timer;

		// Token: 0x040067C7 RID: 26567
		[SerializeField]
		private Image icon;

		// Token: 0x040067C8 RID: 26568
		[SerializeField]
		private Image glow;

		// Token: 0x040067C9 RID: 26569
		[SerializeField]
		private GameObject notificationBlob;

		// Token: 0x040067CA RID: 26570
		private bool initialized;

		// Token: 0x040067CB RID: 26571
		private TownBottomPanelRoot townBottomPanelRoot;

		// Token: 0x040067CC RID: 26572
		private const string HUD = "hud";

		// Token: 0x040067CD RID: 26573
		private const string GLOW = "glow";

		// Token: 0x040067CE RID: 26574
		private const string UNDERSCORE = "_";
	}
}
