using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BE7 RID: 3047
	public class FancyScrollViewCell<TData, TContext> : MonoBehaviour where TContext : class
	{
		// Token: 0x06004778 RID: 18296 RVA: 0x0016CA6A File Offset: 0x0016AE6A
		public virtual void SetContext(TContext context)
		{
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x0016CA6C File Offset: 0x0016AE6C
		public virtual void UpdateContent(TData itemData)
		{
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x0016CA6E File Offset: 0x0016AE6E
		public virtual void UpdatePosition(float position)
		{
		}

		// Token: 0x0600477B RID: 18299 RVA: 0x0016CA70 File Offset: 0x0016AE70
		public virtual void SetVisible(bool visible)
		{
			base.gameObject.SetActive(visible);
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x0600477C RID: 18300 RVA: 0x0016CA7E File Offset: 0x0016AE7E
		// (set) Token: 0x0600477D RID: 18301 RVA: 0x0016CA86 File Offset: 0x0016AE86
		public int DataIndex { get; set; }
	}
}
