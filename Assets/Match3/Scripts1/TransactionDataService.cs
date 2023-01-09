using System;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts2.PlayerData;

// Token: 0x020007BC RID: 1980
namespace Match3.Scripts1
{
	public class TransactionDataService : ADataService
	{
		// Token: 0x060030C4 RID: 12484 RVA: 0x000E49C6 File Offset: 0x000E2DC6
		public TransactionDataService(Func<GameState> i_getState) : base(i_getState)
		{
		}

		// Token: 0x060030C5 RID: 12485 RVA: 0x000E49CF File Offset: 0x000E2DCF
		public void AddTransactionData(string itemId, int price)
		{
			base.state.transactionData.Add(new Transaction(itemId, DateTime.UtcNow.ToUnixTimeStamp(), price));
		}

		// Token: 0x060030C6 RID: 12486 RVA: 0x000E49F4 File Offset: 0x000E2DF4
		public int GetLastTransactionTimeForId(string itemId)
		{
			Transaction transaction = new Transaction(itemId, 0, 0);
			foreach (Transaction transaction2 in base.state.transactionData)
			{
				if (transaction2.itemId == itemId && transaction2.purchaseDate > transaction.purchaseDate)
				{
					transaction = transaction2;
				}
			}
			return transaction.purchaseDate;
		}

		// Token: 0x060030C7 RID: 12487 RVA: 0x000E4A84 File Offset: 0x000E2E84
		public int GetLatestTransactionTime()
		{
			return (base.state.transactionData.Count <= 0) ? 0 : base.state.transactionData[base.state.transactionData.Count - 1].purchaseDate;
		}

		// Token: 0x060030C8 RID: 12488 RVA: 0x000E4AD4 File Offset: 0x000E2ED4
		public int GetNumberOfTransactions()
		{
			return base.state.transactionData.Count;
		}

		// Token: 0x060030C9 RID: 12489 RVA: 0x000E4AE8 File Offset: 0x000E2EE8
		public int GetLatestOfferTransactionTime()
		{
			for (int i = base.state.transactionData.Count - 1; i >= 0; i--)
			{
				Transaction transaction = base.state.transactionData[i];
				if (IAPData.IsOfferName(transaction.itemId))
				{
					return transaction.purchaseDate;
				}
			}
			return 0;
		}

		// Token: 0x060030CA RID: 12490 RVA: 0x000E4B42 File Offset: 0x000E2F42
		public void ClearTransactionData()
		{
			base.state.transactionData = new List<Transaction>();
		}

		// Token: 0x060030CB RID: 12491 RVA: 0x000E4B54 File Offset: 0x000E2F54
		public bool TransactionDataContainsId(string itemId)
		{
			return base.state.transactionData.FindAll((Transaction transaction) => transaction.itemId == itemId).Count > 0;
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x060030CC RID: 12492 RVA: 0x000E4B92 File Offset: 0x000E2F92
		public List<Transaction> Data
		{
			get
			{
				return base.state.transactionData;
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x060030CD RID: 12493 RVA: 0x000E4B9F File Offset: 0x000E2F9F
		public bool IsBuyer
		{
			get
			{
				return !base.state.transactionData.IsNullOrEmptyCollection();
			}
		}
	}
}
