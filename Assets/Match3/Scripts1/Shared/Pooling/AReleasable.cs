using UnityEngine;

namespace Shared.Pooling
{
	// Token: 0x02000B0C RID: 2828
	public abstract class AReleasable : MonoBehaviour, IReleasable
	{
		// Token: 0x060042AD RID: 17069 RVA: 0x0009891F File Offset: 0x00096D1F
		public virtual void Release(float delay = 0f)
		{
			base.gameObject.Release(delay);
		}
	}
}
