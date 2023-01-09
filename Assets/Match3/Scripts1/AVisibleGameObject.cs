using UnityEngine;

// Token: 0x020008E1 RID: 2273
namespace Match3.Scripts1
{
	public abstract class AVisibleGameObject : MonoBehaviour, IVisible
	{
		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06003749 RID: 14153 RVA: 0x000C8CF4 File Offset: 0x000C70F4
		public virtual bool IsVisible
		{
			get
			{
				return base.gameObject.activeSelf;
			}
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x000C8D01 File Offset: 0x000C7101
		public virtual void Show()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600374B RID: 14155 RVA: 0x000C8D0F File Offset: 0x000C710F
		public virtual void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600374C RID: 14156 RVA: 0x000C8D1D File Offset: 0x000C711D
		public void SetVisibility(bool value)
		{
			if (value)
			{
				this.Show();
			}
			else
			{
				this.Hide();
			}
		}
	}
}
