using System;
using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000840 RID: 2112
namespace Match3.Scripts1
{
	public class BannerNewChapterRoot : APtSceneRoot<int>
	{
		// Token: 0x0600346A RID: 13418 RVA: 0x000FAAEC File Offset: 0x000F8EEC
		protected override IEnumerator GoRoutine()
		{
			if (base.registeredFirst)
			{
				this.parameters = this.testChapter;
			}
			Wooroutine<ChapterIntroAssets> assetsFlow = new ChapterIntroIllustrationAssetFlow().Start(this.parameters);
			yield return assetsFlow;
			this.AssignAssets(assetsFlow.ReturnValue);
			this.Localize();
			this.audioService.PlaySFX(AudioId.NewChapterJingle, false, false, true);
			this.animator.GetBehaviour<AnimatorFinished>().onFinished.AddListener(new Action(base.Destroy));
			yield break;
		}

		// Token: 0x0600346B RID: 13419 RVA: 0x000FAB08 File Offset: 0x000F8F08
		private void Localize()
		{
			string text = this.localization.GetText("ui.levelselection.chapter", new LocaParam[0]) + " " + this.parameters;
			foreach (TextMeshProUGUI textMeshProUGUI in this.labelsChapter)
			{
				textMeshProUGUI.text = text;
			}
			string key = string.Format("quest.chapter.name_" + this.parameters, new object[0]);
			this.labelTitle.text = this.localization.GetText(key, new LocaParam[0]);
			if (this.parameters > 1)
			{
				this.pushNotificationService.SendChapterCompleteNotification(this.parameters - 1);
			}
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x000FABC8 File Offset: 0x000F8FC8
		private void AssignAssets(ChapterIntroAssets assets)
		{
			if (!assets)
			{
				return;
			}
			this.bg.sprite = assets.bg;
			this.SetLayerActive(this.bg, assets.bg);
			this.bg_far.sprite = assets.bg_far;
			this.SetLayerActive(this.bg_far, assets.bg_far);
			this.decoration.sprite = assets.decoration;
			this.SetLayerActive(this.decoration, assets.decoration);
			this.foreground.sprite = assets.foreground;
			this.SetLayerActive(this.foreground, assets.foreground);
			this.middleground.sprite = assets.middleground;
			this.SetLayerActive(this.middleground, assets.middleground);
		}

		// Token: 0x0600346D RID: 13421 RVA: 0x000FAC90 File Offset: 0x000F9090
		private void SetLayerActive(Image image, Sprite sprite)
		{
			image.gameObject.SetActive(sprite != null);
		}

		// Token: 0x04005C4A RID: 23626
		[WaitForService(true, true)]
		private AssetBundleService abs;

		// Token: 0x04005C4B RID: 23627
		[WaitForService(true, true)]
		private ILocalizationService localization;

		// Token: 0x04005C4C RID: 23628
		[WaitForService(true, true)]
		private PushNotificationService pushNotificationService;

		// Token: 0x04005C4D RID: 23629
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005C4E RID: 23630
		[SerializeField]
		private Image bg;

		// Token: 0x04005C4F RID: 23631
		[SerializeField]
		private Image bg_far;

		// Token: 0x04005C50 RID: 23632
		[SerializeField]
		private Image decoration;

		// Token: 0x04005C51 RID: 23633
		[SerializeField]
		private Image foreground;

		// Token: 0x04005C52 RID: 23634
		[SerializeField]
		private Image middleground;

		// Token: 0x04005C53 RID: 23635
		[SerializeField]
		private TextMeshProUGUI labelTitle;

		// Token: 0x04005C54 RID: 23636
		[SerializeField]
		private TextMeshProUGUI[] labelsChapter;

		// Token: 0x04005C55 RID: 23637
		[SerializeField]
		private Animator animator;

		// Token: 0x04005C56 RID: 23638
		[SerializeField]
		private int testChapter = 2;

		// Token: 0x04005C57 RID: 23639
		private const string KEY_TITLE = "quest.chapter.name_";
	}
}
