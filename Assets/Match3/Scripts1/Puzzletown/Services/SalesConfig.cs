using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200049C RID: 1180
	[Serializable]
	public class SalesConfig
	{
		// Token: 0x0600216E RID: 8558 RVA: 0x0008C690 File Offset: 0x0008AA90
		public SaleConfig GetConfig(string name)
		{
			SaleConfig saleConfig = this.packages.FirstOrDefault((SaleConfig p) => p.name == name);
			if (saleConfig != null && (saleConfig.content == null || saleConfig.content.Count == 0))
			{
				saleConfig.content = new List<Materials>();
				using (List<string>.Enumerator enumerator = saleConfig.content_name.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string contentName = enumerator.Current;
						List<IAPContent> source = this.content.FindAll((IAPContent c) => c.item_tag.Contains(contentName));
						Materials item = new Materials(from c in source
						select c.materialAmount);
						saleConfig.content.Add(item);
					}
				}
			}
			return saleConfig;
		}

		// Token: 0x04004C82 RID: 19586
		[SerializeField]
		private List<SaleConfig> packages;

		// Token: 0x04004C83 RID: 19587
		[SerializeField]
		private List<IAPContent> content;
	}
}
