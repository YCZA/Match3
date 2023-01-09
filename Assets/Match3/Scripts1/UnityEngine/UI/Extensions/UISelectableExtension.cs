using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C3C RID: 3132
	[AddComponentMenu("UI/Extensions/UI Selectable Extension")]
	[RequireComponent(typeof(Selectable))]
	public class UISelectableExtension : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x060049E0 RID: 18912 RVA: 0x00179DDD File Offset: 0x001781DD
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (this.OnButtonPress != null)
			{
				this.OnButtonPress.Invoke(eventData.button);
			}
			this._pressed = true;
			this._heldEventData = eventData;
		}

		// Token: 0x060049E1 RID: 18913 RVA: 0x00179E09 File Offset: 0x00178209
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (this.OnButtonRelease != null)
			{
				this.OnButtonRelease.Invoke(eventData.button);
			}
			this._pressed = false;
			this._heldEventData = null;
		}

		// Token: 0x060049E2 RID: 18914 RVA: 0x00179E35 File Offset: 0x00178235
		private void Update()
		{
			if (!this._pressed)
			{
				return;
			}
			if (this.OnButtonHeld != null)
			{
				this.OnButtonHeld.Invoke(this._heldEventData.button);
			}
		}

		// Token: 0x060049E3 RID: 18915 RVA: 0x00179E64 File Offset: 0x00178264
		public void TestClicked()
		{
		}

		// Token: 0x060049E4 RID: 18916 RVA: 0x00179E66 File Offset: 0x00178266
		public void TestPressed()
		{
		}

		// Token: 0x060049E5 RID: 18917 RVA: 0x00179E68 File Offset: 0x00178268
		public void TestReleased()
		{
		}

		// Token: 0x060049E6 RID: 18918 RVA: 0x00179E6A File Offset: 0x0017826A
		public void TestHold()
		{
		}

		// Token: 0x060049E7 RID: 18919 RVA: 0x00179E6C File Offset: 0x0017826C
		private void OnDisable()
		{
			this._pressed = false;
		}

		// Token: 0x0400702C RID: 28716
		[Tooltip("Event that fires when a button is initially pressed down")]
		public UISelectableExtension.UIButtonEvent OnButtonPress;

		// Token: 0x0400702D RID: 28717
		[Tooltip("Event that fires when a button is released")]
		public UISelectableExtension.UIButtonEvent OnButtonRelease;

		// Token: 0x0400702E RID: 28718
		[Tooltip("Event that continually fires while a button is held down")]
		public UISelectableExtension.UIButtonEvent OnButtonHeld;

		// Token: 0x0400702F RID: 28719
		private bool _pressed;

		// Token: 0x04007030 RID: 28720
		private PointerEventData _heldEventData;

		// Token: 0x02000C3D RID: 3133
		[Serializable]
		public class UIButtonEvent : UnityEvent<PointerEventData.InputButton>
		{
		}
	}
}
