using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Match3.UI.DataViews;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Shared.ResourceManager;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.Building.Shop;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A28 RID: 2600
namespace Match3.Scripts1
{
	[LoadOptions(true, false, false)]
	public class PopupSeasonalPromoAdvancedRoot : ASceneRoot<PopupSeasonalPromoAdvancedRoot.Params>, IDisposableDialog
	{
		// Token: 0x06003E7B RID: 15995 RVA: 0x0013D0E8 File Offset: 0x0013B4E8
		protected override IEnumerator GoRoutine()
		{
			this.SetupLocalization();
			yield return this.ShowDecoSet();
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.CloseAndQuit));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.closeButton.onClick.AddListener(new UnityAction(this.CloseAndQuit));
			this.goToShopButton.onClick.AddListener(new UnityAction(this.CloseAndGoToShop));
			this.gameStateService.PromoPopupData.SetSeenToday();
			yield break;
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x0013D104 File Offset: 0x0013B504
		private void SetupLocalization()
		{
			this.title.text = this.locaService.GetText(this.parameters.titleKey, new LocaParam[0]);
			this.speechbubble.text = this.locaService.GetText(this.parameters.speechbubbleKey, new LocaParam[0]);
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x0013D160 File Offset: 0x0013B560
		private IEnumerator ShowDecoSet()
		{
			Wooroutine<SpriteManager> spriteManagerFlow = new BundledSpriteManagerLoaderFlow().Start(new BundledSpriteManagerLoaderFlow.Input
			{
				bundleName = this.parameters.assetBundleName,
				path = string.Format("Assets/Puzzletown/Town/Art/Buildings/{0}/SpriteManager_{0}.prefab", this.parameters.promoName)
			});
			yield return spriteManagerFlow;
			SpriteManager spriteManager = spriteManagerFlow.ReturnValue;
			if (spriteManager)
			{
				Sprite similar = spriteManagerFlow.ReturnValue.GetSimilar("illustration");
				if (similar != null)
				{
					this.previewImage.sprite = similar;
				}
				List<string> list = new List<string>();
				if (spriteManager.Count > 1)
				{
					for (int i = 0; i < this.buildingviews.Count; i++)
					{
						string substring = string.Format("building_{0}", i + 1);
						Sprite similar2 = spriteManager.GetSimilar(substring);
						if (similar2 != null)
						{
							list.Add(string.Format("building_{0}", i + 1));
						}
					}
				}
				else
				{
					foreach (BuildingConfig buildingConfig in this.configService.buildingConfigList.buildings)
					{
						if (buildingConfig.season == this.parameters.promoName)
						{
							list.Add(buildingConfig.name);
						}
					}
				}
				int num = 0;
				while (num < list.Count && num < this.buildingviews.Count)
				{
					this.buildingviews[num].manager = spriteManager;
					this.buildingviews[num].Show(new MaterialAmount(list[num], 1, MaterialAmountUsage.Undefined, 0));
					num++;
				}
			}
			yield break;
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x0013D17B File Offset: 0x0013B57B
		private void CloseAndQuit()
		{
			this.Close(false);
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x0013D184 File Offset: 0x0013B584
		private void CloseAndGoToShop()
		{
			this.Close(true);
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x0013D190 File Offset: 0x0013B590
		private void Close(bool showShopAfter)
		{
			if (this.isClosing)
			{
				return;
			}
			this.isClosing = true;
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			BackButtonManager.Instance.RemoveAction(new Action(this.CloseAndGoToShop));
			this.dialog.Hide();
			if (showShopAfter)
			{
				this.townUI.ShopDialog.Open(this.parameters.buildingType);
			}
			this.onClose.Dispatch(showShopAfter);
		}

		// Token: 0x04006787 RID: 26503
		private const string SPRITE_MANAGER_PATH = "Assets/Puzzletown/Town/Art/Buildings/{0}/SpriteManager_{0}.prefab";

		// Token: 0x04006788 RID: 26504
		private const string ILLUSTRATION = "illustration";

		// Token: 0x04006789 RID: 26505
		private const string BUILDING_NAME = "building_{0}";

		// Token: 0x0400678A RID: 26506
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x0400678B RID: 26507
		[SerializeField]
		private Image previewImage;

		// Token: 0x0400678C RID: 26508
		[SerializeField]
		private List<BuildingMaterialAmountView> buildingviews;

		// Token: 0x0400678D RID: 26509
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x0400678E RID: 26510
		[SerializeField]
		private TextMeshProUGUI speechbubble;

		// Token: 0x0400678F RID: 26511
		[SerializeField]
		private Button closeButton;

		// Token: 0x04006790 RID: 26512
		[SerializeField]
		private Button goToShopButton;

		// Token: 0x04006791 RID: 26513
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04006792 RID: 26514
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04006793 RID: 26515
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04006794 RID: 26516
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x04006795 RID: 26517
		[WaitForRoot(false, false)]
		private TownUiRoot townUI;

		// Token: 0x04006796 RID: 26518
		public Signal<bool> onClose = new Signal<bool>();

		// Token: 0x04006797 RID: 26519
		private bool isClosing;

		// Token: 0x02000A29 RID: 2601
		public class Params
		{
			// Token: 0x06003E81 RID: 16001 RVA: 0x0013D212 File Offset: 0x0013B612
			public Params(string promoName, string assetBundleName, string titleKey, string speechbubbleKey, ShopTag buildingType)
			{
				this.promoName = promoName;
				this.assetBundleName = assetBundleName;
				this.titleKey = titleKey;
				this.speechbubbleKey = speechbubbleKey;
				this.buildingType = buildingType;
			}

			// Token: 0x04006798 RID: 26520
			public readonly string promoName;

			// Token: 0x04006799 RID: 26521
			public readonly string assetBundleName;

			// Token: 0x0400679A RID: 26522
			public readonly ShopTag buildingType;

			// Token: 0x0400679B RID: 26523
			public readonly string titleKey;

			// Token: 0x0400679C RID: 26524
			public readonly string speechbubbleKey;
		}
	}
}
