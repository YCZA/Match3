using Match3.Scripts1.Wooga.Signals;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008E2 RID: 2274
namespace Match3.Scripts1
{
	[RequireComponent(typeof(Graphic))]
	public abstract class AVisibleGraphic : MonoBehaviour, IVisible
	{
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x0600374E RID: 14158 RVA: 0x0010DC9F File Offset: 0x0010C09F
		public bool IsVisible
		{
			get
			{
				return base.GetComponent<Graphic>().enabled;
			}
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x0010DCAC File Offset: 0x0010C0AC
		public void Show()
		{
			this.SetVisibility(true);
		}

		// Token: 0x06003750 RID: 14160 RVA: 0x0010DCB5 File Offset: 0x0010C0B5
		public void Hide()
		{
			this.SetVisibility(false);
		}

		// Token: 0x06003751 RID: 14161 RVA: 0x0010DCBE File Offset: 0x0010C0BE
		public void SetVisibility(bool value)
		{
			if (this.IsVisible == value)
			{
				return;
			}
			base.GetComponent<Graphic>().enabled = value;
			this.onVisibilityChanged.Dispatch(value);
		}

		// Token: 0x04005F6F RID: 24431
		public readonly Signal<bool> onVisibilityChanged = new Signal<bool>();
	}
}
