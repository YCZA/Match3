using System;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts2.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020009C0 RID: 2496
namespace Match3.Scripts1
{
	public class DailyDealsPanel : BaseShopPanelItem
	{
		// Token: 0x06003C71 RID: 15473 RVA: 0x0012E0D4 File Offset: 0x0012C4D4
		public override void SetData(IAPService service, IAPData data, Action<IAPData> tryPurchase)
		{
			this._tryPurchase = tryPurchase;
			this._iapData = data;
			this.activePanel.SetActive(!this.purchased);
			this.inactivePanel.SetActive(this.purchased);
			this.currencyView.Show(new MaterialAmount(this.deal.currency_type, this.deal.currency_amount, MaterialAmountUsage.Undefined, 0));
			foreach (MaterialAmountView materialAmountView in this.bonusRewardViews)
			{
				materialAmountView.gameObject.SetActive(false);
			}
			if (!string.IsNullOrEmpty(this.deal.bonus_2_type))
			{
				this.bonusRewardViews[1].Show(new MaterialAmount(this.deal.bonus_2_type, this.deal.bonus_2_amount, MaterialAmountUsage.Undefined, 0));
			}
			bool flag = string.IsNullOrEmpty(this.deal.bonus_3_type);
			if (!flag)
			{
				this.bonusRewardViews[2].Show(new MaterialAmount(this.deal.bonus_3_type, this.deal.bonus_3_amount, MaterialAmountUsage.Undefined, 0));
			}
			this.singleBonusRewardArea.SetActive(flag);
			this.multipleBonusRewardArea.SetActive(!flag);
			this.largeRewardArea.gameObject.SetActive(false);
			if (!string.IsNullOrEmpty(this.deal.bonus_1_type))
			{
				if (this.deal.bonus_1_type.StartsWith("iso_"))
				{
					this.singleBonusRewardArea.SetActive(false);
					this.multipleBonusRewardArea.SetActive(false);
					BuildingConfig config = this.configService.buildingConfigList.GetConfig(this.deal.bonus_1_type);
					if (config != null && this.buildingSprite != null)
					{
						BuildingShopData data2 = new BuildingShopData
						{
							buildingImage = this.buildingSprite,
							buildingName = config.name,
							data = config
						};
						this.largeRewardArea.gameObject.SetActive(true);
						this.largeRewardArea.Show(data2);
					}
					else
					{
						this.largeRewardArea.gameObject.SetActive(false);
					}
				}
				else
				{
					MaterialAmount mat = new MaterialAmount(this.deal.bonus_1_type, this.deal.bonus_1_amount, MaterialAmountUsage.Undefined, 0);
					this.bonusRewardViews[0].Show(mat);
					this.mediumBonusRewardView.Show(mat);
					this.singleBonusRewardArea.SetActive(flag);
				}
			}
			this.multipleBonusRewardArea.SetActive(!flag);
			// if (data.storeProduct != null)
			// {
			// this.cost.text = data.storeProduct.metadata.localizedPriceString;
			// }
			foreach (CountdownTimer countdownTimer in this.countdownTimers)
			{
				countdownTimer.SetTargetTime(this.expirationDate, false, null);
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x0012E408 File Offset: 0x0012C808
		public void SetDeal(DailyDealsConfig.Deal deal, DateTime expirationDate, bool purchased, ConfigService configService, Sprite buildingSprite)
		{
			this.deal = deal;
			this.expirationDate = expirationDate;
			this.purchased = purchased;
			this.configService = configService;
			this.buildingSprite = buildingSprite;
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x0012E42F File Offset: 0x0012C82F
		private void TryPurchase()
		{
			this._tryPurchase(this._iapData);
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x0012E442 File Offset: 0x0012C842
		private void OnEnable()
		{
			this.purchaseButton.onClick.AddListener(new UnityAction(this.TryPurchase));
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x0012E460 File Offset: 0x0012C860
		private void OnDisable()
		{
			this.purchaseButton.onClick.RemoveListener(new UnityAction(this.TryPurchase));
		}

		// Token: 0x04006507 RID: 25863
		[SerializeField]
		private TextMeshProUGUI cost;

		// Token: 0x04006508 RID: 25864
		[SerializeField]
		private Button purchaseButton;

		// Token: 0x04006509 RID: 25865
		[SerializeField]
		private GameObject activePanel;

		// Token: 0x0400650A RID: 25866
		[SerializeField]
		private GameObject inactivePanel;

		// Token: 0x0400650B RID: 25867
		[SerializeField]
		private GameObject smallRewardArea;

		// Token: 0x0400650C RID: 25868
		[SerializeField]
		private GameObject singleBonusRewardArea;

		// Token: 0x0400650D RID: 25869
		[SerializeField]
		private GameObject multipleBonusRewardArea;

		// Token: 0x0400650E RID: 25870
		[SerializeField]
		private BuildingShopView largeRewardArea;

		// Token: 0x0400650F RID: 25871
		[SerializeField]
		private MaterialAmountView currencyView;

		// Token: 0x04006510 RID: 25872
		[SerializeField]
		private MaterialAmountView mediumBonusRewardView;

		// Token: 0x04006511 RID: 25873
		[SerializeField]
		private List<MaterialAmountView> bonusRewardViews;

		// Token: 0x04006512 RID: 25874
		[SerializeField]
		private List<CountdownTimer> countdownTimers;

		// Token: 0x04006513 RID: 25875
		private ConfigService configService;

		// Token: 0x04006514 RID: 25876
		private Sprite buildingSprite;

		// Token: 0x04006515 RID: 25877
		private IAPData _iapData;

		// Token: 0x04006516 RID: 25878
		private Action<IAPData> _tryPurchase;

		// Token: 0x04006517 RID: 25879
		private DailyDealsConfig.Deal deal;

		// Token: 0x04006518 RID: 25880
		private DateTime expirationDate;

		// Token: 0x04006519 RID: 25881
		private bool purchased;
	}
}
