using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007E8 RID: 2024
	[Serializable]
	public class Materials : IEnumerable<MaterialAmount>, IEnumerable
	{
		// Token: 0x0600321C RID: 12828 RVA: 0x000EC0B0 File Offset: 0x000EA4B0
		public Materials()
		{
			this.container = new List<MaterialAmount>();
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x000EC0C3 File Offset: 0x000EA4C3
		public Materials(IEnumerable<MaterialAmount> other)
		{
			this.container = new List<MaterialAmount>(other);
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x0600321E RID: 12830 RVA: 0x000EC0D7 File Offset: 0x000EA4D7
		public int Count
		{
			get
			{
				return (this.container == null) ? 0 : this.container.Count;
			}
		}

		// Token: 0x170007E2 RID: 2018
		public MaterialAmount this[int index]
		{
			get
			{
				return this.container[index];
			}
		}

		// Token: 0x170007E3 RID: 2019
		public int this[string name]
		{
			get
			{
				return this.GetEntry(name).amount;
			}
			set
			{
				for (int i = 0; i < this.container.Count; i++)
				{
					if (this.container[i].type == name)
					{
						this.container[i] = new MaterialAmount(name, value, MaterialAmountUsage.Undefined, 0);
						return;
					}
				}
				this.container.Add(new MaterialAmount(name, value, MaterialAmountUsage.Undefined, 0));
			}
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x000EC194 File Offset: 0x000EA594
		public void Remove(string name)
		{
			this.container.RemoveAll((MaterialAmount m) => m.type == name);
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x000EC1C6 File Offset: 0x000EA5C6
		public IEnumerator<MaterialAmount> GetEnumerator()
		{
			return this.container.GetEnumerator();
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x000EC1D8 File Offset: 0x000EA5D8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x000EC1E0 File Offset: 0x000EA5E0
		public static Materials operator +(Materials a, IEnumerable<MaterialAmount> b)
		{
			Materials materials = new Materials(a);
			foreach (MaterialAmount materialAmount in b)
			{
				Materials materials2;
				string type;
				(materials2 = materials)[type = materialAmount.type] = materials2[type] + materialAmount.amount;
			}
			return materials;
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x000EC258 File Offset: 0x000EA658
		public static Materials operator -(Materials a, IEnumerable<MaterialAmount> b)
		{
			Materials materials = new Materials(a);
			foreach (MaterialAmount materialAmount in b)
			{
				Materials materials2;
				string type;
				(materials2 = materials)[type = materialAmount.type] = materials2[type] - materialAmount.amount;
			}
			return materials;
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x000EC2D0 File Offset: 0x000EA6D0
		public bool Contains(IEnumerable<MaterialAmount> other)
		{
			foreach (MaterialAmount materialAmount in this - other)
			{
				if (materialAmount.amount < 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003228 RID: 12840 RVA: 0x000EC33C File Offset: 0x000EA73C
		public bool Contains(string type)
		{
			return this.GetEntry(type).amount > 0;
		}

		// Token: 0x06003229 RID: 12841 RVA: 0x000EC35C File Offset: 0x000EA75C
		public void Add(MaterialAmount materialAmount)
		{
			string type;
			this[type = materialAmount.type] = this[type] + materialAmount.amount;
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x000EC38C File Offset: 0x000EA78C
		public void Add(IEnumerable<MaterialAmount> materialAmounts)
		{
			if (materialAmounts != null)
			{
				foreach (MaterialAmount materialAmount in materialAmounts)
				{
					this.Add(materialAmount);
				}
			}
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x000EC3E8 File Offset: 0x000EA7E8
		public IEnumerable<MaterialAmount> Select(IEnumerable<MaterialAmount> other)
		{
			foreach (MaterialAmount it in other)
			{
				yield return this.GetEntry(it.type);
			}
			yield break;
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x000EC412 File Offset: 0x000EA812
		public void Clear()
		{
			this.container.Clear();
		}

		// Token: 0x0600322D RID: 12845 RVA: 0x000EC420 File Offset: 0x000EA820
		private MaterialAmount GetEntry(string name)
		{
			foreach (MaterialAmount result in this.container)
			{
				if (result.type == name)
				{
					return result;
				}
			}
			return new MaterialAmount(name, 0, MaterialAmountUsage.Undefined, 0);
		}

		// Token: 0x04005AB0 RID: 23216
		[SerializeField]
		private List<MaterialAmount> container;
	}
}
