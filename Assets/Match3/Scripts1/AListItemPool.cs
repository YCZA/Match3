using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.UI;
using UnityEngine;

// Token: 0x02000700 RID: 1792
namespace Match3.Scripts1
{
	public abstract class AListItemPool<T1, T2> : MonoBehaviour, IListItemPool<T1, T2> where T1 : UiSimpleView<T2>
	{
		// Token: 0x06002C68 RID: 11368 RVA: 0x000CC9EC File Offset: 0x000CADEC
		public T1 GetItem(T2 state)
		{
			T1 t = this.TakeFromPool(state);
			if (t)
			{
				return t;
			}
			T1 t2 = this.FindPrototypeCell(state);
			if (t2)
			{
				return global::UnityEngine.Object.Instantiate<T1>(t2);
			}
			WoogaDebug.LogWarningFormatted("Could not find prototype for {0} in {1}", new object[]
			{
				state,
				base.name
			});
			return (T1)((object)null);
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x000CCA5A File Offset: 0x000CAE5A
		public void PutBack(T1 item)
		{
			item.transform.SetParent(this.vault, false);
			this.m_pool.Add(item);
		}

		// Token: 0x06002C6A RID: 11370 RVA: 0x000CCA84 File Offset: 0x000CAE84
		private T1 TakeFromPool(T2 state)
		{
			T1 t = this.m_pool.Find((T1 cell) => cell.CheckMatch(state));
			if (t)
			{
				this.m_pool.Remove(t);
			}
			return t;
		}

		// Token: 0x06002C6B RID: 11371
		protected abstract T1 FindPrototypeCell(T2 state);

		// Token: 0x040055B2 RID: 21938
		[SerializeField]
		private Transform vault;

		// Token: 0x040055B3 RID: 21939
		private readonly List<T1> m_pool = new List<T1>();
	}
}
