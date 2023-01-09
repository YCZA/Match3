using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;

// Token: 0x020008A5 RID: 2213
namespace Match3.Scripts1
{
	public class WatchVideoFlow : AFlow
	{
		// Token: 0x06003624 RID: 13860 RVA: 0x00106118 File Offset: 0x00104518
		protected override IEnumerator FlowRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			bool adSuccessful = false;
			bool userAborted = false;
			Wooroutine<VideoShowResult> showAdRoutine = WooroutineRunner.StartWooroutine<VideoShowResult>(this.videoAdService.ShowAd(AdPlacement.LivesShop));
			yield return showAdRoutine;
			if (showAdRoutine.ReturnValue == VideoShowResult.Success)
			{
				adSuccessful = true;
			}
			else if (showAdRoutine.ReturnValue == VideoShowResult.Aborted)
			{
				userAborted = true;
			}
			List<object> content = new List<object>();
			if (adSuccessful)
			{
				this.livesService.AddLives(1);
				MaterialAmount materialAmount = new MaterialAmount("lives", 1, MaterialAmountUsage.Undefined, 0);
				string text = this.localizationService.GetText("ui.shared.purchase.diamonds.proceed", new LocaParam[0]);
				content.Add(TextData.Title(this.localizationService.GetText("ui.lives.watch_ad.success.title", new LocaParam[0])));
				content.Add(materialAmount);
				content.Add(TextData.Content(this.localizationService.GetText("ui.lives.watch_ad.success.body", new LocaParam[0])));
				content.Add(new CloseButton(null));
				content.Add(new LabeledButtonWithCallback(text, null));
			}
			else
			{
				string text2 = this.localizationService.GetText("ui.shared.button.ok", new LocaParam[0]);
				content.Add(TextData.Title(this.localizationService.GetText("ui.lives.watch_ad.failure.title", new LocaParam[0])));
				content.Add(TextData.Content(this.localizationService.GetText("ui.lives.watch_ad.failure.body", new LocaParam[0])));
				content.Add(new CloseButton(null));
				content.Add(new LabeledButtonWithCallback(text2, null));
			}
			if (!userAborted)
			{
				Wooroutine<PopupDialogRoot> popup = PopupDialogRoot.Show(content.ToArray());
				yield return popup;
				yield return popup.ReturnValue.onClose;
			}
			yield return adSuccessful;
			yield break;
		}

		// Token: 0x04005E23 RID: 24099
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x04005E24 RID: 24100
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005E25 RID: 24101
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04005E26 RID: 24102
		[WaitForService(true, true)]
		private IVideoAdService videoAdService;
	}
}
