using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Legal;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000832 RID: 2098
	public class WoogaLegalTermsFlow : AFlow
	{
		// Token: 0x06003431 RID: 13361 RVA: 0x000F8B20 File Offset: 0x000F6F20
		protected override IEnumerator FlowRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			bool abTestEnabled = this.sbsService.SbsConfig.feature_switches.gdpr_popup_enabled;
			// eli key point 屏蔽隐私协议
			if (abTestEnabled && !Terms.HasAccepted() && false)
			{
				string language = this.settingsService.Language.ToTwoLetterIsoCode();
				TermsLocalisation.language = language;
				TermsLocalisation.termsLink = "2512";
				TermsLocalisation.policyLink = "2425";
				Wooroutine<PopupGDPRRoot> popup = SceneManager.Instance.LoadScene<PopupGDPRRoot>(null);
				yield return popup;
				yield return popup.ReturnValue.onDestroyed;
			}
			yield break;
		}

		// Token: 0x04005C0E RID: 23566
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005C0F RID: 23567
		[WaitForService(true, true)]
		private GameSettingsService settingsService;
	}
}
