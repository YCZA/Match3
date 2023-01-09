using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x02000913 RID: 2323
	[Serializable]
	public class DailyDealsConfig
	{
		// Token: 0x040060F4 RID: 24820
		public DailyDealsConfig.Balancing balancing;

		// Token: 0x040060F5 RID: 24821
		public List<DailyDealsConfig.Deal> deals;

		// Token: 0x040060F6 RID: 24822
		public List<DailyDealsConfig.DayOrder> order;

		// Token: 0x02000914 RID: 2324
		[Serializable]
		public class Balancing
		{
			// Token: 0x170008B5 RID: 2229
			// (get) Token: 0x060038AF RID: 14511 RVA: 0x001171B8 File Offset: 0x001155B8
			public List<string> TriggerPackages
			{
				get
				{
					if (this.triggerPackages == null)
					{
						if (string.IsNullOrEmpty(this.trigger_packages))
						{
							this.triggerPackages = new List<string>();
						}
						else
						{
							this.triggerPackages = new List<string>(this.trigger_packages.Split(new char[]
							{
								','
							}));
						}
						this.triggerPackages = (from t in this.triggerPackages
						select Regex.Replace(t, "\\s+", string.Empty)).ToList<string>();
					}
					return this.triggerPackages;
				}
			}

			// Token: 0x170008B6 RID: 2230
			// (get) Token: 0x060038B0 RID: 14512 RVA: 0x0011724C File Offset: 0x0011564C
			public List<string> ExcludePackages
			{
				get
				{
					if (this.excludePackages == null)
					{
						if (string.IsNullOrEmpty(this.exclude_packages))
						{
							this.excludePackages = new List<string>();
						}
						else
						{
							this.excludePackages = new List<string>(this.exclude_packages.Split(new char[]
							{
								','
							}));
						}
						this.excludePackages = (from t in this.excludePackages
						select Regex.Replace(t, "\\s+", string.Empty)).ToList<string>();
					}
					return this.excludePackages;
				}
			}

			// Token: 0x040060F7 RID: 24823
			public int unlock_level;

			// Token: 0x040060F8 RID: 24824
			public string trigger_packages;

			// Token: 0x040060F9 RID: 24825
			public string exclude_packages;

			// Token: 0x040060FA RID: 24826
			public int unlock_time;

			// Token: 0x040060FB RID: 24827
			public int number_of_island_loads;

			// Token: 0x040060FC RID: 24828
			private List<string> triggerPackages;

			// Token: 0x040060FD RID: 24829
			private List<string> excludePackages;
		}

		// Token: 0x02000915 RID: 2325
		[Serializable]
		public class Deal
		{
			// Token: 0x04006100 RID: 24832
			public string offer_id;

			// Token: 0x04006101 RID: 24833
			public string bundle_id;

			// Token: 0x04006102 RID: 24834
			public string currency_type;

			// Token: 0x04006103 RID: 24835
			public int currency_amount;

			// Token: 0x04006104 RID: 24836
			public string bonus_1_type;

			// Token: 0x04006105 RID: 24837
			public int bonus_1_amount;

			// Token: 0x04006106 RID: 24838
			public string bonus_2_type;

			// Token: 0x04006107 RID: 24839
			public int bonus_2_amount;

			// Token: 0x04006108 RID: 24840
			public string bonus_3_type;

			// Token: 0x04006109 RID: 24841
			public int bonus_3_amount;
		}

		// Token: 0x02000916 RID: 2326
		[Serializable]
		public class DayOrder
		{
			// Token: 0x0400610A RID: 24842
			public int offer_id;
		}
	}
}
