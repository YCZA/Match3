using System;
using Match3.Scripts2.Shop;
using UnityEngine;

// Token: 0x020009BF RID: 2495
namespace Match3.Scripts1
{
	public class BaseShopPanelItem : MonoBehaviour
	{
		// Token: 0x06003C6E RID: 15470 RVA: 0x0012E0C7 File Offset: 0x0012C4C7
		public virtual void SetData(IAPService service, IAPData data, Action<IAPData> tryPurchase)
		{
		}

		// Token: 0x06003C6F RID: 15471 RVA: 0x0012E0C9 File Offset: 0x0012C4C9
		public virtual void Init(ILocalizationService localization)
		{
		}
	}
}
