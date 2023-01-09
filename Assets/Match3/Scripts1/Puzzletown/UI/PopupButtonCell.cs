using System;
using Match3.Scripts1.UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000864 RID: 2148
	public class PopupButtonCell : APopupCell<LabeledButtonWithCallback>
	{
		// Token: 0x06003501 RID: 13569 RVA: 0x000FE41C File Offset: 0x000FC81C
		private void OnEnable()
		{
			Button componentInChildren = base.GetComponentInChildren<Button>();
			if (componentInChildren)
			{
				componentInChildren.onClick.AddListener(new UnityAction(this.OnClick));
			}
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x000FE454 File Offset: 0x000FC854
		private void OnDisable()
		{
			Button componentInChildren = base.GetComponentInChildren<Button>();
			if (componentInChildren)
			{
				componentInChildren.onClick.RemoveListener(new UnityAction(this.OnClick));
			}
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x000FE48C File Offset: 0x000FC88C
		public override void Show(LabeledButtonWithCallback data)
		{
			this.data = data;
			this.ExecuteOnChild(delegate(TMP_Text label)
			{
				label.text = data.text;
			});
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x000FE4C4 File Offset: 0x000FC8C4
		private void OnClick()
		{
			IHandler<Action> componentInParent = base.GetComponentInParent<IHandler<Action>>();
			if (componentInParent != null)
			{
				componentInParent.Handle(this.data.callback);
			}
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x000FE4EF File Offset: 0x000FC8EF
		public override bool CanPresent(LabeledButtonWithCallback data)
		{
			return true;
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06003506 RID: 13574 RVA: 0x000FE4F2 File Offset: 0x000FC8F2
		public override int reusableId
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x04005CEF RID: 23791
		private LabeledButtonWithCallback data;
	}
}
