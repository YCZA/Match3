using System;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x02000997 RID: 2455
	public class ChallengeTeaserV2Root : APtSceneRoot, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x06003BA7 RID: 15271 RVA: 0x00128134 File Offset: 0x00126534
		protected override void Go()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.unlocksLevelLabel.text = this.localizationService.GetText("ui.challenges.teaser.text.3", new LocaParam[]
			{
				new LocaParam("{challengesUnlockLevel}", this.challengeService.Balancing.play_minimum_level)
			});
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x001281C2 File Offset: 0x001265C2
		private void Close()
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x001281F9 File Offset: 0x001265F9
		public void Handle(PopupOperation evt)
		{
			if (evt == PopupOperation.Close || evt == PopupOperation.OK)
			{
				this.Close();
			}
		}

		// Token: 0x040063A7 RID: 25511
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x040063A8 RID: 25512
		[SerializeField]
		private TMP_Text unlocksLevelLabel;

		// Token: 0x040063A9 RID: 25513
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040063AA RID: 25514
		[WaitForService(true, true)]
		private ChallengeService challengeService;

		// Token: 0x040063AB RID: 25515
		[WaitForService(true, true)]
		private ILocalizationService localizationService;
	}
}
