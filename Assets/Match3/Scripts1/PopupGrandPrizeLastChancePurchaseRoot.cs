using System;
using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009FA RID: 2554
namespace Match3.Scripts1
{
	public class PopupGrandPrizeLastChancePurchaseRoot : APtSceneRoot<SeasonPrizeInfo>, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06003D8D RID: 15757 RVA: 0x001370CE File Offset: 0x001354CE
		// (set) Token: 0x06003D8E RID: 15758 RVA: 0x001370D6 File Offset: 0x001354D6
		public PopupOperation Response { get; private set; }

		// Token: 0x06003D8F RID: 15759 RVA: 0x001370E0 File Offset: 0x001354E0
		protected override IEnumerator GoRoutine()
		{
			SeasonPrizeInfo parameters = this.parameters;
			int grandPrizeProgress = parameters.grandPrizeProgress;
			BuildingConfig building = this.seasonService.GrandPrizeConfigForSeason(parameters.name);
			BuildingAssetWrapper<Sprite> wrappedSpriteOrPlaceholder = this.buildingResourceService.GetWrappedSpriteOrPlaceholder(building);
			Sprite asset = wrappedSpriteOrPlaceholder.asset;
			this.prizeImage.sprite = asset;
			SeasonService.PriceInfo priceInfo = this.seasonService.CalculatePriceForGrandPrize(parameters, grandPrizeProgress);
			this.priceLabel.text = priceInfo.discountPrice.ToString();
			yield break;
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x001370FB File Offset: 0x001354FB
		public void Close()
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x00137132 File Offset: 0x00135532
		public void Show()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
		}

		// Token: 0x06003D92 RID: 15762 RVA: 0x00137169 File Offset: 0x00135569
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

		// Token: 0x04006668 RID: 26216
		[WaitForRoot(false, false)]
		private BuildingResourceServiceRoot buildingResourceService;

		// Token: 0x04006669 RID: 26217
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x0400666A RID: 26218
		[WaitForService(true, true)]
		private SeasonService seasonService;

		// Token: 0x0400666B RID: 26219
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x0400666C RID: 26220
		[SerializeField]
		private Image prizeImage;

		// Token: 0x0400666D RID: 26221
		[SerializeField]
		private TMP_Text priceLabel;
	}
}
