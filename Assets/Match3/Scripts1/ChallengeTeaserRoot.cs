using System;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000996 RID: 2454
namespace Match3.Scripts1
{
	[LoadOptions(true, true, false)]
	public class ChallengeTeaserRoot : ASceneRoot, IDisposableDialog
	{
		// Token: 0x06003BA4 RID: 15268 RVA: 0x00128048 File Offset: 0x00126448
		protected override void Go()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.okButton.onClick.AddListener(new UnityAction(this.Close));
			this.unlocksLevelLabel.text = this.localizationService.GetText("ui.challenges.teaser.text.3", new LocaParam[]
			{
				new LocaParam("{challengesUnlockLevel}", this.challengeService.Balancing.play_minimum_level)
			});
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x001280F2 File Offset: 0x001264F2
		public void Close()
		{
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.dialog.Hide();
		}

		// Token: 0x040063A0 RID: 25504
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x040063A1 RID: 25505
		[SerializeField]
		private Button okButton;

		// Token: 0x040063A2 RID: 25506
		[SerializeField]
		private TextMeshProUGUI unlocksLevelLabel;

		// Token: 0x040063A3 RID: 25507
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040063A4 RID: 25508
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x040063A5 RID: 25509
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x040063A6 RID: 25510
		[WaitForService(true, true)]
		private ChallengeService challengeService;
	}
}
