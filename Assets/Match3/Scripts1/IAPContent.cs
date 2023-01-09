using System;

// Token: 0x020007C5 RID: 1989
namespace Match3.Scripts1
{
	[Serializable]
	public class IAPContent
	{
		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06003108 RID: 12552 RVA: 0x000E60BD File Offset: 0x000E44BD
		public MaterialAmount materialAmount
		{
			get
			{
				return new MaterialAmount(this.item_resource, this.item_amount, MaterialAmountUsage.Undefined, 0);
			}
		}

		// Token: 0x040059B3 RID: 22963
		public string[] item_tag;

		// Token: 0x040059B4 RID: 22964
		public string item_resource;

		// Token: 0x040059B5 RID: 22965
		public string item_image;

		// Token: 0x040059B6 RID: 22966
		public int item_amount;
	}
}
