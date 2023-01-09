using System.Collections;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.Env;
using Match3.Scripts2.Shop;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007F2 RID: 2034
	public class SaleService : AWeeklyEventService
	{
		// Token: 0x0600325B RID: 12891 RVA: 0x000ECD38 File Offset: 0x000EB138
		public SaleService()
		{
			WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x000ECD50 File Offset: 0x000EB150
		private IEnumerator InitRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			WooroutineRunner.StartCoroutine(base.WaitForSbsRoutine(), null);
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x0600325D RID: 12893 RVA: 0x000ECD6C File Offset: 0x000EB16C
		public Sale CurrentSale
		{
			get
			{
				EventConfigContainer configContainer = this.DataService.ConfigContainer;
				if (configContainer == null || configContainer.config == null || configContainer.config.sale == null || this.DataService.EventId == null || this.gameStateService.Sale == null)
				{
					return null;
				}
				bool flag = !this.configService.SbsConfig.feature_switches.sales_enabled;
				bool flag2 = !configContainer.IsOngoing(this.timeService.Now.ToUnixTimeStamp());
				bool flag3 = this.DataService.UnlockLevel() > this.progressioService.UnlockedLevel;
				bool flag4 = this.DataService.EventId.Equals(this.gameStateService.Sale.BoughtSaleId);
				SaleConfig saleConfig = this.configService.SbsConfig.salesconfig.GetConfig(configContainer.config.sale.configName);
				if (flag || flag2 || flag3 || flag4 || saleConfig == null)
				{
					return null;
				}
				IAPData iapdata = this.iapService.IAPs.FirstOrDefault((IAPData i) => i.iap_name == saleConfig.iap_name);
				if (!this.iapService.initalialized || iapdata == null || !iapdata.IsAvailable)
				{
					if (GameEnvironment.CurrentEnvironment == GameEnvironment.Environment.PRODUCTION)
					{
						return null;
					}
					iapdata = null;
				}
				return new Sale(configContainer, saleConfig, iapdata);
			}
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x000ECEEE File Offset: 0x000EB2EE
		public void MarkBoughtSale(Sale sale)
		{
			this.gameStateService.Sale.BoughtSaleId = sale.id;
			this.gameStateService.Save(false);
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x0600325F RID: 12895 RVA: 0x000ECF12 File Offset: 0x000EB312
		protected override WeeklyEventType WeeklyEventType
		{
			get
			{
				return WeeklyEventType.Sale;
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06003260 RID: 12896 RVA: 0x000ECF16 File Offset: 0x000EB316
		protected override AWeeklyEventDataService DataService
		{
			get
			{
				return this.gameStateService.Sale;
			}
		}

		// Token: 0x04005AD1 RID: 23249
		public const string SALE_TAG = "sale_open_items";

		// Token: 0x04005AD2 RID: 23250
		[WaitForService(true, true)]
		private IAPService iapService;

		// Token: 0x04005AD3 RID: 23251
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressioService;
	}
}
