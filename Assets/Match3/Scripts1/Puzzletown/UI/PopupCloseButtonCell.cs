using Match3.Scripts1.UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000866 RID: 2150
	public class PopupCloseButtonCell : APopupCell<CloseButton>
	{
		// Token: 0x06003509 RID: 13577 RVA: 0x000FE521 File Offset: 0x000FC921
		private void Register()
		{
			if (!this.registered)
			{
				this.ExecuteOnChild(delegate(Button button)
				{
					button.onClick.AddListener(new UnityAction(this.OnClick));
				});
			}
			this.registered = true;
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x000FE547 File Offset: 0x000FC947
		private void Deregister()
		{
			if (this.registered)
			{
				this.ExecuteOnChild(delegate(Button button)
				{
					button.onClick.RemoveListener(new UnityAction(this.OnClick));
				});
			}
			this.registered = false;
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x000FE56D File Offset: 0x000FC96D
		private void OnEnable()
		{
			this.Register();
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x000FE575 File Offset: 0x000FC975
		private void OnDisable()
		{
			this.Deregister();
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x000FE57D File Offset: 0x000FC97D
		private void OnDestroy()
		{
			this.Deregister();
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x000FE585 File Offset: 0x000FC985
		public override void Show(CloseButton data)
		{
			this.data = data;
		}

		// Token: 0x0600350F RID: 13583 RVA: 0x000FE58E File Offset: 0x000FC98E
		public override bool CanPresent(CloseButton data)
		{
			return true;
		}

		// Token: 0x06003510 RID: 13584 RVA: 0x000FE591 File Offset: 0x000FC991
		private void OnClick()
		{
			this.HandleOnParent(this.data.callback);
		}

		// Token: 0x04005CF0 RID: 23792
		private CloseButton data;

		// Token: 0x04005CF1 RID: 23793
		private bool registered;
	}
}
