using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Town
{
	// 测试相关场景
	public class CheatPopupTestSelector : MonoBehaviour, IHandler<PopupToTest>
	{
		// Token: 0x06003C3E RID: 15422 RVA: 0x0012C4C3 File Offset: 0x0012A8C3
		public void OnCancelTap()
		{
			this.selectedPopup = new PopupWithParams();
			this.selectedPopup.isCancelled = true;
			this.isRunning = false;
		}

		// Token: 0x06003C3F RID: 15423 RVA: 0x0012C4E4 File Offset: 0x0012A8E4
		public IEnumerator SelectPopupRoutine()
		{
			this.isRunning = true;
			this.ShowPopupSelectorButtons();
			this.UpdateStatus();
			bool isConfirmedExit = false;
			while (!isConfirmedExit)
			{
				while (this.isRunning)
				{
					yield return null;
				}
				if (this.selectedPopup.popup == PopupToTest.ToggleOfflineWarning)
				{
					this.ToggleOfflineWarning();
					this.isRunning = true;
				}
				else
				{
					isConfirmedExit = true;
				}
			}
			yield return this.selectedPopup;
			yield break;
		}

		// Token: 0x06003C40 RID: 15424 RVA: 0x0012C500 File Offset: 0x0012A900
		private void ToggleOfflineWarning()
		{
			bool hasBeenTriggered = PopupMissingAssetsRoot.Trigger.hasBeenTriggered;
			bool hasBeenTriggered2 = !hasBeenTriggered;
			PopupMissingAssetsRoot.Trigger.hasBeenTriggered = hasBeenTriggered2;
			this.UpdateStatus();
		}

		// Token: 0x06003C41 RID: 15425 RVA: 0x0012C524 File Offset: 0x0012A924
		private void UpdateStatus()
		{
			this.statusLabel.text = string.Format("Offline warning popup seen? {0}", PopupMissingAssetsRoot.Trigger.hasBeenTriggered);
		}

		// Token: 0x06003C42 RID: 15426 RVA: 0x0012C545 File Offset: 0x0012A945
		private void ShowPopupSelectorButtons()
		{
			this.dataSource.Show((PopupToTest[])Enum.GetValues(typeof(PopupToTest)));
		}

		// Token: 0x06003C43 RID: 15427 RVA: 0x0012C566 File Offset: 0x0012A966
		public void Handle(PopupToTest evt)
		{
			this.selectedPopup = new PopupWithParams();
			this.selectedPopup.popup = evt;
			this.selectedPopup.parameters = null;
			this.selectedPopup.isCancelled = false;
			this.isRunning = false;
		}

		// Token: 0x0400645C RID: 25692
		public PopupDataSource dataSource;

		// Token: 0x0400645D RID: 25693
		private bool isRunning;

		// Token: 0x0400645E RID: 25694
		private PopupWithParams selectedPopup;

		// Token: 0x0400645F RID: 25695
		public Text statusLabel;
	}
}
