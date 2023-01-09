using System;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000725 RID: 1829
	[Serializable]
	public class BoostViewData
	{
		// Token: 0x06002D37 RID: 11575 RVA: 0x000D1F06 File Offset: 0x000D0306
		public BoostViewData(string name, int amount)
		{
			this.name = name;
			this.amount = amount;
			this.state = BoostState.Inactive;
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x000D1F23 File Offset: 0x000D0323
		public BoostViewData(string name, int amount, BoostState state, bool waterAllFields = false)
		{
			this.name = name;
			this.amount = amount;
			this.state = state;
			this.waterAllFields = waterAllFields;
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06002D39 RID: 11577 RVA: 0x000D1F48 File Offset: 0x000D0348
		public Boosts Type
		{
			get
			{
				return (Boosts)Enum.Parse(typeof(Boosts), this.name);
			}
		}

		// Token: 0x040056CE RID: 22222
		public string name;

		// Token: 0x040056CF RID: 22223
		public int amount;

		// Token: 0x040056D0 RID: 22224
		public BoostState state;

		// Token: 0x040056D1 RID: 22225
		public bool waterAllFields;
	}
}
