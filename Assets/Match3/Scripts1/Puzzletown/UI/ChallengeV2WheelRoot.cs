using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000999 RID: 2457
	[RequireComponent(typeof(SpinWheelAnimator))]
	[LoadOptions(true, true, false)]
	public class ChallengeV2WheelRoot : APtSceneRoot<List<BuildingConfig>, BuildingConfig>, IHandler<AdSpinOperation>, IDisposableDialog
	{
		// Token: 0x06003BAF RID: 15279 RVA: 0x0012853E File Offset: 0x0012693E
		protected override void Go()
		{
			this.dialog.Show();
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.SetupPriceViews();
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x00128568 File Offset: 0x00126968
		private void SetupPriceViews()
		{
			int num = 0;
			while (num < this.parameters.Count && num < this.buildingImages.Count)
			{
				BuildingConfig building = this.parameters[num];
				Sprite asset = this.buildingResourceServiceRoot.GetWrappedSpriteOrPlaceholder(building).asset;
				this.buildingImages[num].sprite = asset;
				this.buildingImages[num].gameObject.SetActive(true);
				this.buildingImagesHighlighted[num].sprite = asset;
				this.buildingImagesHighlighted[num].gameObject.SetActive(true);
				this.highlights[num].SetActive(false);
				num++;
			}
		}

		// Token: 0x06003BB1 RID: 15281 RVA: 0x00128628 File Offset: 0x00126A28
		private IEnumerator OnSpinButtonPressed()
		{
			BuildingConfig buildingConfig = this.parameters.RandomElement(false);
			int index = this.parameters.IndexOf(buildingConfig);
			yield return this.spinWheelAnimator.AnimateSpinWheel(index);
			yield return new WaitForSeconds(0.25f);
			this.highlights[index].SetActive(true);
			yield return new WaitForSeconds(0.5f);
			this.onCompleted.Dispatch(buildingConfig);
			this.Close();
			yield break;
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x00128643 File Offset: 0x00126A43
		private void Close()
		{
			this.dialog.Hide();
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x00128664 File Offset: 0x00126A64
		public void Handle(AdSpinOperation evt)
		{
			if (evt != AdSpinOperation.None)
			{
				if (evt == AdSpinOperation.Spin)
				{
					base.StartCoroutine(this.OnSpinButtonPressed());
					this.spinButton.interactable = false;
					this.spinButtonAnimation.enabled = false;
				}
			}
		}

		// Token: 0x040063B5 RID: 25525
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040063B6 RID: 25526
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040063B7 RID: 25527
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x040063B8 RID: 25528
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x040063B9 RID: 25529
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x040063BA RID: 25530
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x040063BB RID: 25531
		[WaitForService(true, true)]
		private ExternalGamesService externalGamesService;

		// Token: 0x040063BC RID: 25532
		[WaitForRoot(false, false)]
		private BuildingResourceServiceRoot buildingResourceServiceRoot;

		// Token: 0x040063BD RID: 25533
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x040063BE RID: 25534
		[Header("Prizes")]
		[SerializeField]
		private List<Image> buildingImages;

		// Token: 0x040063BF RID: 25535
		[SerializeField]
		private List<Image> buildingImagesHighlighted;

		// Token: 0x040063C0 RID: 25536
		[SerializeField]
		private List<GameObject> highlights;

		// Token: 0x040063C1 RID: 25537
		[Header("Button")]
		[SerializeField]
		private Button spinButton;

		// Token: 0x040063C2 RID: 25538
		[SerializeField]
		private Animation spinButtonAnimation;

		// Token: 0x040063C3 RID: 25539
		[SerializeField]
		private SpinWheelAnimator spinWheelAnimator;
	}
}
