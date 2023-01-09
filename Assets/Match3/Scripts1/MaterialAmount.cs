using System;
using Match3.Scripts1.Puzzletown;
using Match3.Scripts1.Wooga.UI;

namespace Match3.Scripts1
{
	// Token: 0x020007E7 RID: 2023
	[Serializable]
	public struct MaterialAmount
	{
		// Token: 0x0600320D RID: 12813 RVA: 0x000EBEAB File Offset: 0x000EA2AB
		public MaterialAmount(string type, int amount, MaterialAmountUsage usage = MaterialAmountUsage.Undefined, int offer = 0)
		{
			this = default(MaterialAmount);
			this.type = type;
			this.amount = amount;
			this.MaxAmount = 0;
			this.Period = 0;
			this.Usage = usage;
			this.SpecialOffer = offer;
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x0600320E RID: 12814 RVA: 0x000EBEDF File Offset: 0x000EA2DF
		// (set) Token: 0x0600320F RID: 12815 RVA: 0x000EBEE7 File Offset: 0x000EA2E7
		public int Period { get; set; }

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06003210 RID: 12816 RVA: 0x000EBEF0 File Offset: 0x000EA2F0
		// (set) Token: 0x06003211 RID: 12817 RVA: 0x000EBEF8 File Offset: 0x000EA2F8
		public int MaxAmount { get; set; }

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06003212 RID: 12818 RVA: 0x000EBF01 File Offset: 0x000EA301
		// (set) Token: 0x06003213 RID: 12819 RVA: 0x000EBF09 File Offset: 0x000EA309
		public int SpecialOffer { get; private set; }

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06003214 RID: 12820 RVA: 0x000EBF12 File Offset: 0x000EA312
		// (set) Token: 0x06003215 RID: 12821 RVA: 0x000EBF1A File Offset: 0x000EA31A
		public MaterialAmountUsage Usage { get; private set; }

		// Token: 0x06003216 RID: 12822 RVA: 0x000EBF23 File Offset: 0x000EA323
		public string Format(ILocalizationService loc)
		{
			return string.Format("{0}{1}", this.SpriteName, this.FormatName(loc));
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06003217 RID: 12823 RVA: 0x000EBF3C File Offset: 0x000EA33C
		public string SpriteName
		{
			get
			{
				return string.Format("<sprite=\"ui_icon_{0}\" index=0>", this.type);
			}
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x000EBF4E File Offset: 0x000EA34E
		public string FormatName(ILocalizationService loc)
		{
			return loc.GetText(string.Format("ui.shared.material.{0}", this.type), new LocaParam[0]);
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x000EBF6C File Offset: 0x000EA36C
		public string ToString(ILocalizationService loc)
		{
			if (this.Period > 0)
			{
				string format = "{0}/{1}";
				int num = 3600 * this.amount / this.Period;
				if (num > 0)
				{
					return string.Format(format, num, loc.GetText("ui.harvest.hour", new LocaParam[0]));
				}
				int num2 = 86400 * this.amount / this.Period;
				if (num2 > 0)
				{
					return string.Format(format, num2, loc.GetText("ui.harvest.day", new LocaParam[0]));
				}
				int num3 = 604800 * this.amount / this.Period;
				return string.Format(format, num3, loc.GetText("ui.harvest.week", new LocaParam[0]));
			}
			else
			{
				if (this.MaxAmount > 0)
				{
					return string.Format("{0}/{1}", this.amount, this.MaxAmount);
				}
				if (this.type == "lives_unlimited")
				{
					return TimeFormatter.ShortDuration(this.amount * 60, loc);
				}
				return this.amount.ToString();
			}
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x000EC092 File Offset: 0x000EA492
		public void Feed(IDataView<MaterialAmount> view)
		{
			view.Show(this);
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x000EC0A0 File Offset: 0x000EA4A0
		public void Mutltiply(int factor)
		{
			this.amount *= factor;
		}

		// Token: 0x04005AAA RID: 23210
		public string type;

		// Token: 0x04005AAB RID: 23211
		public int amount;
	}
}
