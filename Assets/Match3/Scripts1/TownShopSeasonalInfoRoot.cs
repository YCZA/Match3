using System;
using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A42 RID: 2626
namespace Match3.Scripts1
{
	[LoadOptions(true, false, false)]
	public class TownShopSeasonalInfoRoot : ASceneRoot, IDisposableDialog
	{
		// Token: 0x06003EFE RID: 16126 RVA: 0x001418DC File Offset: 0x0013FCDC
		protected override IEnumerator GoRoutine()
		{
			BuildingConfig buildingConfig = this.seasonService.GetGrandPrizeBuildingConfig();
			Wooroutine<SpriteManager> spriteManagerRoutine = new SeasonSpriteManagerFlow().Start<SpriteManager>();
			yield return spriteManagerRoutine;
			if (spriteManagerRoutine.ReturnValue != null && buildingConfig != null)
			{
				SpriteManager returnValue = spriteManagerRoutine.ReturnValue;
				this.seasonCurrencyIcon.sprite = returnValue.GetSimilar("season_currency");
				this.grandPrizeGlow.sprite = returnValue.GetSimilar(buildingConfig.name);
				this.grandPrizeImage.sprite = this.resourceService.GetWrappedSpriteOrPlaceholder(buildingConfig).asset;
				LocaParam[] arr = new LocaParam[]
				{
					new LocaParam("{seasonCurrencyIcon}", this.seasonService.GetActiveSeason().TMProIconName)
				};
				this.text1.text = this.localizationService.GetText("ui.specialdecos.preview.step1", arr);
				this.text2.text = this.localizationService.GetText("ui.specialdecos.preview.step2", arr);
			}
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.closeButton.onClick.AddListener(new UnityAction(this.Close));
			yield break;
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x001418F8 File Offset: 0x0013FCF8
		private void Close()
		{
			this.closeButton.interactable = false;
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.dialog.Hide();
		}

		// Token: 0x0400684D RID: 26701
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x0400684E RID: 26702
		[SerializeField]
		private Button closeButton;

		// Token: 0x0400684F RID: 26703
		[Header("Season")]
		[SerializeField]
		private Image seasonCurrencyIcon;

		// Token: 0x04006850 RID: 26704
		[SerializeField]
		private TMP_Text text1;

		// Token: 0x04006851 RID: 26705
		[SerializeField]
		private TMP_Text text2;

		// Token: 0x04006852 RID: 26706
		[Header("Grand Prize")]
		[SerializeField]
		private Image grandPrizeGlow;

		// Token: 0x04006853 RID: 26707
		[SerializeField]
		private Image grandPrizeImage;

		// Token: 0x04006854 RID: 26708
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04006855 RID: 26709
		[WaitForService(true, true)]
		private SeasonService seasonService;

		// Token: 0x04006856 RID: 26710
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04006857 RID: 26711
		[WaitForRoot(false, false)]
		private BuildingResourceServiceRoot resourceService;
	}
}
