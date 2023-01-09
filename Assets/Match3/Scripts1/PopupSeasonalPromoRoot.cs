using System;
using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.Building.Shop;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A2A RID: 2602
namespace Match3.Scripts1
{
	[LoadOptions(true, true, false)]
	public class PopupSeasonalPromoRoot : ASceneRoot, IDisposableDialog
	{
		// Token: 0x06003E83 RID: 16003 RVA: 0x0013D620 File Offset: 0x0013BA20
		protected override IEnumerator GoRoutine()
		{
			SeasonConfig promotedSeason = this.seasonService.GetActiveSeason();
			yield return this.SetupImage(promotedSeason);
			yield return this.SetupGrandPrize(promotedSeason, this.seasonService.GetGrandPrizeBuildingConfig());
			yield return this.SetupParticleFx(promotedSeason);
			this.SetupLocalization(promotedSeason);
			this.timer.SetTargetTime(promotedSeason.EndDate, true, null);
			this.TextControlGroup.SetActive(!this.seasonService.IsSeasonalsV3);
			this.TextActiveGroup.SetActive(this.seasonService.IsSeasonalsV3);
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.CloseAndQuit));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.closeButon.onClick.AddListener(new UnityAction(this.CloseAndQuit));
			this.goToShopButton.onClick.AddListener(new UnityAction(this.CloseAndGoToShop));
			this.gameStateService.SetSeenFlagWithTimestamp("seasonalPromoSeen", DateTime.UtcNow);
			yield break;
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x0013D63C File Offset: 0x0013BA3C
		private IEnumerator SetupGrandPrize(SeasonConfig seasonConfig, BuildingConfig grandPrizeConfig)
		{
			this.grandPrizeContainer.SetActive(grandPrizeConfig != null);
			if (grandPrizeConfig != null)
			{
				Wooroutine<SpriteManager> spriteManagerRoutine = new SeasonSpriteManagerFlow().Start<SpriteManager>();
				yield return spriteManagerRoutine;
				Sprite glowSprite = (!(spriteManagerRoutine.ReturnValue != null)) ? null : spriteManagerRoutine.ReturnValue.GetSimilar(grandPrizeConfig.name);
				if (glowSprite != null)
				{
					this.grandPrizeGlow.sprite = glowSprite;
				}
				else
				{
					this.grandPrizeGlow.gameObject.SetActive(false);
				}
				Sprite prizeSprite = this.resourceService.GetWrappedSpriteOrPlaceholder(grandPrizeConfig).asset;
				if (prizeSprite != null)
				{
					this.grandPrizeImage.sprite = prizeSprite;
					this.grandPrizeAnimation.Play("GrandPrizeBounce");
				}
				else
				{
					this.grandPrizeImage.gameObject.SetActive(false);
					this.grandPrizeGlow.gameObject.SetActive(false);
					this.grandPrizeAnimation.gameObject.SetActive(false);
				}
			}
			yield break;
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x0013D660 File Offset: 0x0013BA60
		private IEnumerator SetupParticleFx(SeasonConfig seasonConfig)
		{
			Wooroutine<Texture> texture = this.abService.LoadAsset<Texture>(seasonConfig.PrimaryBundleName, seasonConfig.FxTexturePath);
			yield return texture;
			this.fxRenderer.sharedMaterial.mainTexture = texture.ReturnValue;
			this.fxTarget.SetActive(texture.ReturnValue != null);
			yield break;
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x0013D684 File Offset: 0x0013BA84
		private void SetupLocalization(SeasonConfig promotedSeason)
		{
			string title_loca_key = promotedSeason.title_loca_key;
			string text = this.locaService.GetText(title_loca_key, new LocaParam[0]);
			if (!text.IsNullOrEmpty())
			{
				this.title.text = text;
			}
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x0013D6C4 File Offset: 0x0013BAC4
		private IEnumerator SetupImage(SeasonConfig promotedSeason)
		{
			string bundle = promotedSeason.PrimaryBundleName;
			string currentIllustration = promotedSeason.CurrentPromoIllustrationName;
			yield return this.TryAssignSpriteToImage(this.illustration, bundle, currentIllustration);
			yield break;
		}

		// Token: 0x06003E88 RID: 16008 RVA: 0x0013D6E8 File Offset: 0x0013BAE8
		private IEnumerator TryAssignSpriteToImage(Image img, string bundle, string spriteToLoad)
		{
			Sprite sprite = null;
			if (!string.IsNullOrEmpty(bundle) && !string.IsNullOrEmpty(spriteToLoad))
			{
				Wooroutine<Sprite> loadRoutine = this.abService.LoadAsset<Sprite>(bundle, spriteToLoad);
				yield return loadRoutine;
				try
				{
					sprite = loadRoutine.ReturnValue;
				}
				catch (Exception ex)
				{
					WoogaDebug.LogWarning(new object[]
					{
						ex.Message
					});
				}
			}
			img.sprite = sprite;
			yield break;
		}

		// Token: 0x06003E89 RID: 16009 RVA: 0x0013D718 File Offset: 0x0013BB18
		private IEnumerator CleanupAndClose()
		{
			yield return null;
			this.onClose.Dispatch(false);
			this.dialog.DisableDialogHolder();
			yield break;
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x0013D733 File Offset: 0x0013BB33
		public void CloseAndQuit()
		{
			this.Close(false);
		}

		// Token: 0x06003E8B RID: 16011 RVA: 0x0013D73C File Offset: 0x0013BB3C
		public void CloseAndGoToShop()
		{
			this.Close(true);
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x0013D748 File Offset: 0x0013BB48
		private void Close(bool showShopAfter)
		{
			if (this.isClosing)
			{
				return;
			}
			this.isClosing = true;
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			BackButtonManager.Instance.RemoveAction(new Action(this.CloseAndQuit));
			this.dialog.Hide();
			if (showShopAfter)
			{
				this.townUI.ShopDialog.Open(ShopTag.RarityLevel1);
			}
			this.onClose.Dispatch(showShopAfter);
		}

		// Token: 0x0400679D RID: 26525
		private const string ANIMATION_GRAND_PRIZE = "GrandPrizeBounce";

		// Token: 0x0400679E RID: 26526
		public const string SEASONAL_PROMO_POPUP_SEEN_FLAG_NAME = "seasonalPromoSeen";

		// Token: 0x0400679F RID: 26527
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x040067A0 RID: 26528
		[SerializeField]
		private Button goToShopButton;

		// Token: 0x040067A1 RID: 26529
		[SerializeField]
		private Button closeButon;

		// Token: 0x040067A2 RID: 26530
		[SerializeField]
		private Image illustration;

		// Token: 0x040067A3 RID: 26531
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x040067A4 RID: 26532
		[SerializeField]
		private CountdownTimer timer;

		// Token: 0x040067A5 RID: 26533
		[Header("SeasonalsV3 AB Test")]
		[SerializeField]
		private GameObject TextControlGroup;

		// Token: 0x040067A6 RID: 26534
		[SerializeField]
		private GameObject TextActiveGroup;

		// Token: 0x040067A7 RID: 26535
		[Header("Grand Prize")]
		[SerializeField]
		private GameObject grandPrizeContainer;

		// Token: 0x040067A8 RID: 26536
		[SerializeField]
		private Image grandPrizeGlow;

		// Token: 0x040067A9 RID: 26537
		[SerializeField]
		private Image grandPrizeImage;

		// Token: 0x040067AA RID: 26538
		[SerializeField]
		private Animation grandPrizeAnimation;

		// Token: 0x040067AB RID: 26539
		[Header("ParticleFx")]
		[SerializeField]
		private GameObject fxTarget;

		// Token: 0x040067AC RID: 26540
		[SerializeField]
		private ParticleSystemRenderer fxRenderer;

		// Token: 0x040067AD RID: 26541
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040067AE RID: 26542
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x040067AF RID: 26543
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x040067B0 RID: 26544
		[WaitForService(true, true)]
		private AssetBundleService abService;

		// Token: 0x040067B1 RID: 26545
		[WaitForService(true, true)]
		private SeasonService seasonService;

		// Token: 0x040067B2 RID: 26546
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x040067B3 RID: 26547
		[WaitForRoot(false, false)]
		private TownUiRoot townUI;

		// Token: 0x040067B4 RID: 26548
		[WaitForRoot(false, false)]
		private BuildingResourceServiceRoot resourceService;

		// Token: 0x040067B5 RID: 26549
		public Signal<bool> onClose = new Signal<bool>();

		// Token: 0x040067B6 RID: 26550
		private bool isClosing;

		// Token: 0x02000A2B RID: 2603
		public class Trigger : PopupManager.Trigger
		{
			// Token: 0x06003E8D RID: 16013 RVA: 0x0013D7C0 File Offset: 0x0013BBC0
			public Trigger(GameStateService gameStateService, SeasonService seasonService, TownShopRoot shop, BuildingResourceServiceRoot buildingResourceServiceRoot)
			{
				this.gameStateService = gameStateService;
				this.seasonService = seasonService;
				this.shop = shop;
				this.buildingResourceServiceRoot = buildingResourceServiceRoot;
			}

			// Token: 0x06003E8E RID: 16014 RVA: 0x0013D7E5 File Offset: 0x0013BBE5
			public override bool ShouldTrigger()
			{
				return this.ShouldShowSeasonalPromo() && this.IsShopAvailable() && this.IsShopSetup();
			}

			// Token: 0x06003E8F RID: 16015 RVA: 0x0013D808 File Offset: 0x0013BC08
			public override IEnumerator Run()
			{
				if (this.seasonService != null)
				{
					Wooroutine<bool> bundlesAvailable = this.seasonService.AreAllActiveSeasonBundlesAvailable();
					yield return bundlesAvailable;
					if (!bundlesAvailable.ReturnValue)
					{
						yield break;
					}
					Wooroutine<PopupSeasonalPromoRoot> scene = SceneManager.Instance.LoadScene<PopupSeasonalPromoRoot>(null);
					yield return scene;
					AwaitSignal<bool> showingShopAfterPopup = scene.ReturnValue.onClose.Await<bool>();
					yield return showingShopAfterPopup;
					this.SetSeenTimestamp();
					if (showingShopAfterPopup.Dispatched)
					{
						yield return this.shop.onShopReady;
						WaitForSeconds updateInterval = new WaitForSeconds(1f);
						while (this.IsTownShopVisible() || this.HasBuildingSelected() || this.HasBlocker())
						{
							yield return updateInterval;
						}
					}
				}
				yield break;
			}

			// Token: 0x06003E90 RID: 16016 RVA: 0x0013D823 File Offset: 0x0013BC23
			private bool IsTownShopVisible()
			{
				return this.shop.isActiveAndEnabled;
			}

			// Token: 0x06003E91 RID: 16017 RVA: 0x0013D830 File Offset: 0x0013BC30
			private bool HasBuildingSelected()
			{
				return BuildingLocation.Selected != null;
			}

			// Token: 0x06003E92 RID: 16018 RVA: 0x0013D83D File Offset: 0x0013BC3D
			private bool HasBlocker()
			{
				return BlockerManager.global.HasBlockers;
			}

			// Token: 0x06003E93 RID: 16019 RVA: 0x0013D84C File Offset: 0x0013BC4C
			private void SetSeenTimestamp()
			{
				int lastSeenSeasonalPromoTimeStamp = DateTime.UtcNow.ToUnixTimeStamp();
				if (this.gameStateService != null)
				{
					this.gameStateService.SeasonalData.LastSeenSeasonalPromoTimeStamp = lastSeenSeasonalPromoTimeStamp;
				}
			}

			// Token: 0x06003E94 RID: 16020 RVA: 0x0013D880 File Offset: 0x0013BC80
			private bool ShouldShowSeasonalPromo()
			{
				if (!this.seasonService.IsActive)
				{
					return false;
				}
				bool flag = this.buildingResourceServiceRoot != null && this.buildingResourceServiceRoot.onSpritesLoaded.WasDispatched;
				SeasonConfig activeSeason = this.seasonService.GetActiveSeason();
				return activeSeason != null && this.HasCooldownExpired(activeSeason) && flag;
			}

			// Token: 0x06003E95 RID: 16021 RVA: 0x0013D8E8 File Offset: 0x0013BCE8
			private bool HasCooldownExpired(SeasonConfig promotedSeason)
			{
				int num = DateTime.UtcNow.ToUnixTimeStamp();
				int num2 = num - this.gameStateService.SeasonalData.LastSeenSeasonalPromoTimeStamp;
				int num3 = promotedSeason.promo_cooldown_hours * 3600;
				return num2 > num3;
			}

			// Token: 0x06003E96 RID: 16022 RVA: 0x0013D924 File Offset: 0x0013BD24
			private bool IsShopAvailable()
			{
				return this.gameStateService.Progression.UnlockedLevel >= PopupSeasonalPromoRoot.Trigger.SHOP_UNLOCK_LEVEL;
			}

			// Token: 0x06003E97 RID: 16023 RVA: 0x0013D940 File Offset: 0x0013BD40
			private bool IsShopSetup()
			{
				return this.shop.HasSeasonalTabSetup;
			}

			// Token: 0x040067B7 RID: 26551
			public static int SHOP_UNLOCK_LEVEL = 10;

			// Token: 0x040067B8 RID: 26552
			private TownShopRoot shop;

			// Token: 0x040067B9 RID: 26553
			private GameStateService gameStateService;

			// Token: 0x040067BA RID: 26554
			private SeasonService seasonService;

			// Token: 0x040067BB RID: 26555
			private BuildingResourceServiceRoot buildingResourceServiceRoot;
		}
	}
}
