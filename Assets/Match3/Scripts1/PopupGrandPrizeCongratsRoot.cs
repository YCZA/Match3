using System;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Match3.UI.DataViews;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A40 RID: 2624
namespace Match3.Scripts1
{
	[LoadOptions(true, false, false)]
	public class PopupGrandPrizeCongratsRoot : ASceneRoot<BuildingConfig>, IDisposableDialog
	{
		// Token: 0x06003ED4 RID: 16084 RVA: 0x0013FBBC File Offset: 0x0013DFBC
		protected override void Go()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.buildingView.Show(new MaterialAmount(this.parameters.name, 1, MaterialAmountUsage.Undefined, 0));
			this.closeButton.onClick.AddListener(new UnityAction(this.Close));
		}

		// Token: 0x06003ED5 RID: 16085 RVA: 0x0013FC38 File Offset: 0x0013E038
		public void Close()
		{
			this.closeButton.interactable = false;
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.dialog.Hide();
		}

		// Token: 0x04006818 RID: 26648
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x04006819 RID: 26649
		[SerializeField]
		private Button closeButton;

		// Token: 0x0400681A RID: 26650
		[SerializeField]
		private BuildingMaterialAmountView buildingView;

		// Token: 0x0400681B RID: 26651
		[WaitForService(true, true)]
		private AudioService audioService;
	}
}
