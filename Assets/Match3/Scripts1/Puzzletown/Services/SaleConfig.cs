using System;
using System.Collections.Generic;
using System.Linq;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200049D RID: 1181
	[Serializable]
	public class SaleConfig
	{
		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06002171 RID: 8561 RVA: 0x0008C7DC File Offset: 0x0008ABDC
		public IAPContent[] AsIapContent
		{
			get
			{
				return (from m in this.content.SelectMany((Materials c) => c)
				select new IAPContent
				{
					item_resource = m.type,
					item_amount = m.amount
				}).ToArray<IAPContent>();
			}
		}

		// Token: 0x04004C85 RID: 19589
		public string name;

		// Token: 0x04004C86 RID: 19590
		public string iap_name;

		// Token: 0x04004C87 RID: 19591
		public float discount;

		// Token: 0x04004C88 RID: 19592
		public float value;

		// Token: 0x04004C89 RID: 19593
		public string icon_name;

		// Token: 0x04004C8A RID: 19594
		public List<string> content_name;

		// Token: 0x04004C8B RID: 19595
		public List<Materials> content;
	}
}
