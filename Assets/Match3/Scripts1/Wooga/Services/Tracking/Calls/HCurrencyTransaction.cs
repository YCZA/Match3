using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Services.Tracking.Calls
{
	// Token: 0x02000439 RID: 1081
	public static class HCurrencyTransaction
	{
		// Token: 0x06001FA4 RID: 8100 RVA: 0x000851D4 File Offset: 0x000835D4
		public static void Track(int amountBefore, int amountPurchased, int moneySpent, string itemPurchased)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["abb"] = amountBefore;
			dictionary["amb"] = amountPurchased;
			dictionary["ams"] = moneySpent;
			dictionary["item"] = itemPurchased;
			Tracking.Track("hact", dictionary);
		}
	}
}
