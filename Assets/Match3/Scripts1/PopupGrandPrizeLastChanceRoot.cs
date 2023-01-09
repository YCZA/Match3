using System;
using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009FB RID: 2555
namespace Match3.Scripts1
{
	public class PopupGrandPrizeLastChanceRoot : APtSceneRoot<SeasonPrizeInfo>, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06003D94 RID: 15764 RVA: 0x0013727E File Offset: 0x0013567E
		// (set) Token: 0x06003D95 RID: 15765 RVA: 0x00137286 File Offset: 0x00135686
		public PopupOperation Response { get; private set; }

		// Token: 0x06003D96 RID: 15766 RVA: 0x0013728F File Offset: 0x0013568F
		private void Start()
		{
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x00137294 File Offset: 0x00135694
		protected override IEnumerator GoRoutine()
		{
			SeasonPrizeInfo previousSeason = this.parameters;
			int currentProgress = previousSeason.grandPrizeProgress;
			int targetScore = this.seasonService.TargetValueForGrandPrize(previousSeason.name);
			BuildingConfig prizeBuilding = this.seasonService.GrandPrizeConfigForSeason(previousSeason.name);
			BuildingAssetWrapper<Sprite> wrappedResource = this.buildingResourceService.GetWrappedSpriteOrPlaceholder(prizeBuilding);
			Sprite prizeBuildingImage = wrappedResource.asset;
			this.prizeImage.sprite = prizeBuildingImage;
			string subtitle = this.locaService.GetText(prizeBuilding.Name_LocaleKey, new LocaParam[0]);
			this.subtitleLabel.text = subtitle;
			Wooroutine<SpriteManager> spriteManagerRoutine = new SeasonSpriteManagerFlow(previousSeason).Start<SpriteManager>();
			yield return spriteManagerRoutine;
			this.seasonSpriteManager = spriteManagerRoutine.ReturnValue;
			if (this.seasonSpriteManager != null)
			{
				this.currencyImage.sprite = this.seasonSpriteManager.GetSimilar("season_currency");
			}
			SeasonService.PriceInfo priceInfo = this.seasonService.CalculatePriceForGrandPrize(previousSeason, currentProgress);
			this.progress.SetProgress((float)currentProgress, (float)targetScore);
			this.fullPriceLabel.text = priceInfo.fullPrice.ToString();
			this.discountPriceLabel.text = priceInfo.discountPrice.ToString();
			string percentString = string.Format("{0}%", priceInfo.discountPercent);
			string discountText = this.locaService.GetText("ui.grandprize.offer.percent", new LocaParam[]
			{
				new LocaParam("{percent}", percentString)
			});
			this.discountPercentLabel.text = discountText;
			bool showDiscount = priceInfo.fullPrice != priceInfo.discountPrice;
			foreach (GameObject gameObject in this.ShowOnlyIfThereIsADiscount)
			{
				gameObject.SetActive(showDiscount);
			}
			yield break;
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x001372AF File Offset: 0x001356AF
		public void Close()
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x001372E6 File Offset: 0x001356E6
		public void Show()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x0013731D File Offset: 0x0013571D
		public void Handle(PopupOperation evt)
		{
			if (evt != PopupOperation.Close)
			{
				if (evt == PopupOperation.OK)
				{
					this.Response = PopupOperation.OK;
					this.Close();
				}
			}
			else
			{
				this.Response = PopupOperation.Close;
				this.Close();
			}
		}

		// Token: 0x0400666F RID: 26223
		[WaitForRoot(false, false)]
		private BuildingResourceServiceRoot buildingResourceService;

		// Token: 0x04006670 RID: 26224
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04006671 RID: 26225
		[WaitForService(true, true)]
		private SeasonService seasonService;

		// Token: 0x04006672 RID: 26226
		[WaitForService(true, true)]
		private AssetBundleService abs;

		// Token: 0x04006673 RID: 26227
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x04006674 RID: 26228
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x04006675 RID: 26229
		[SerializeField]
		private Image prizeImage;

		// Token: 0x04006676 RID: 26230
		[SerializeField]
		private Image currencyImage;

		// Token: 0x04006677 RID: 26231
		[SerializeField]
		private TMP_Text subtitleLabel;

		// Token: 0x04006678 RID: 26232
		[SerializeField]
		private GameObject[] ShowOnlyIfThereIsADiscount;

		// Token: 0x04006679 RID: 26233
		[SerializeField]
		private TMP_Text fullPriceLabel;

		// Token: 0x0400667A RID: 26234
		[SerializeField]
		private TMP_Text discountPriceLabel;

		// Token: 0x0400667B RID: 26235
		[SerializeField]
		private TMP_Text discountPercentLabel;

		// Token: 0x0400667C RID: 26236
		[SerializeField]
		private ProgressBarUI progress;

		// Token: 0x0400667E RID: 26238
		private SpriteManager seasonSpriteManager;
	}
}
