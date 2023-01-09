using Match3.Scripts1.Puzzletown.UI;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006F8 RID: 1784
	public abstract class AListItemPlaceholder<T1, T2> : MonoBehaviour where T1 : UiSimpleView<T2>
	{
		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06002C4C RID: 11340 RVA: 0x000CC168 File Offset: 0x000CA568
		// (set) Token: 0x06002C4D RID: 11341 RVA: 0x000CC170 File Offset: 0x000CA570
		public T1 current { get; private set; }

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06002C4E RID: 11342 RVA: 0x000CC179 File Offset: 0x000CA579
		private IListItemPool<T1, T2> pool
		{
			get
			{
				return base.GetComponentInParent<IListItemPool<T1, T2>>();
			}
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x000CC184 File Offset: 0x000CA584
		protected T1 LoadListItem(T2 state)
		{
			this.Clear();
			this.current = this.pool.GetItem(state);
			T1 current = this.current;
			this.ResetTransform(current.transform);
			return this.current;
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x000CC1C9 File Offset: 0x000CA5C9
		private void ResetTransform(Transform target)
		{
			target.SetParent(base.transform, false);
			target.localPosition = Vector3.zero;
			// target.localRotation = Quaternion.identity;
			target.localScale = Vector3.one;
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x000CC1F9 File Offset: 0x000CA5F9
		protected void Clear()
		{
			if (this.current)
			{
				this.pool.PutBack(this.current);
				this.current = (T1)((object)null);
			}
		}
	}
}
