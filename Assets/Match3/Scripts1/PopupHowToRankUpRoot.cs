using System;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.UnityFramework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A21 RID: 2593
namespace Match3.Scripts1
{
	[LoadOptions(true, true, false)]
	public class PopupHowToRankUpRoot : ASceneRoot, IDisposableDialog
	{
		// Token: 0x06003E4C RID: 15948 RVA: 0x0013BED8 File Offset: 0x0013A2D8
		protected override void Go()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.OpenShop));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.closeButon.onClick.AddListener(new UnityAction(this.OpenShop));
			this.openShopButton.onClick.AddListener(new UnityAction(this.OpenShop));
		}

		// Token: 0x06003E4D RID: 15949 RVA: 0x0013BF54 File Offset: 0x0013A354
		private void OpenShop()
		{
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			BackButtonManager.Instance.RemoveAction(new Action(this.OpenShop));
			this.townUI.ShopDialog.Open();
			this.dialog.Hide();
		}

		// Token: 0x04006750 RID: 26448
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x04006751 RID: 26449
		[SerializeField]
		private Button openShopButton;

		// Token: 0x04006752 RID: 26450
		[SerializeField]
		private Button closeButon;

		// Token: 0x04006753 RID: 26451
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04006754 RID: 26452
		[WaitForRoot(false, false)]
		private TownUiRoot townUI;
	}
}
