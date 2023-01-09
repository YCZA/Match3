using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A39 RID: 2617
namespace Match3.Scripts1
{
	public class BuildingShopView : ATableViewReusableCell, IDataView<BuildingShopData>, IEditorDescription
	{
		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06003EBE RID: 16062 RVA: 0x0013EE05 File Offset: 0x0013D205
		public string BuildingId
		{
			get
			{
				return (this.building != null) ? this.building.name : string.Empty;
			}
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x0013EE28 File Offset: 0x0013D228
		public void Show(BuildingShopData data)
		{
			this.building = data.data;
			if (this.buildingName)
			{
				this.buildingName.text = data.buildingName;
			}
			if (this.buildingImage)
			{
				this.buildingImage.sprite = data.buildingImage;
				this.buildingImage.color = Color.white;
			}
			if (this.message)
			{
				this.message.text = data.message;
			}
			if (this.newBadge)
			{
				this.newBadge.SetVisibility(!data.isReviewed);
			}
			if (this.income)
			{
				this.income.Show(this.building.Harmony);
			}
			bool flag = this.building.costs.Length > 0 && this.building.costs[0].type == "earned_season_currency";
			bool flag2 = !flag && this.building.season_currency > 0 && data.seasonSpriteManager != null;
			if (this.costs && this.building.costs.Length > 0)
			{
				if (flag)
				{
					MaterialAmount mat = new MaterialAmount("season_currency", this.building.costs[0].amount, MaterialAmountUsage.Undefined, 0);
					this.costs.manager = data.seasonSpriteManager;
					this.costs.Show(mat);
				}
				else
				{
					this.costs.manager = data.normalSpriteManager;
					this.costs.Show(this.building.costs[0]);
				}
			}
			if (this.incomeBubbleSmall)
			{
				this.incomeBubbleSmall.SetActive(!flag2);
			}
			if (this.incomeBubbleBig)
			{
				this.incomeBubbleBig.SetActive(flag2);
			}
			if (this.incomeSeason)
			{
				if (flag2)
				{
					this.incomeSeason.manager = data.seasonSpriteManager;
					this.incomeSeason.Show(this.building.SeasonCurrency);
				}
				else
				{
					this.incomeSeason.Hide();
				}
			}
			this.SetupButton();
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x0013F08C File Offset: 0x0013D48C
		private void SetupButton()
		{
			Button component = base.GetComponent<Button>();
			if (component != null)
			{
				component.onClick.RemoveAllListeners();
				component.onClick.AddListener(new UnityAction(this.CreateBuildingRequest));
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06003EC1 RID: 16065 RVA: 0x0013F0CE File Offset: 0x0013D4CE
		public override int reusableId
		{
			get
			{
				return (int)this.usage;
			}
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x0013F0D6 File Offset: 0x0013D4D6
		public string GetEditorDescription()
		{
			return this.usage.ToString();
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x0013F0EC File Offset: 0x0013D4EC
		private void CreateBuildingRequest()
		{
			if (this.usage == BuildingShopDataState.Available || this.usage == BuildingShopDataState.Stored || this.usage == BuildingShopDataState.InviteFriend)
			{
				float zoomInRatio = BuildingShopView.BuildingBuildRequest.CalculateZoomFactorBasedOnSize(this.building);
				this.HandleOnParent(new BuildingShopView.BuildingBuildRequest
				{
					Config = this.building,
					shouldPanCamera = true,
					shouldZoomIn = true,
					zoomInRatio = zoomInRatio,
					isFromStorage = (this.usage == BuildingShopDataState.Stored)
				});
			}
		}

		// Token: 0x040067EB RID: 26603
		private BuildingConfig building;

		// Token: 0x040067EC RID: 26604
		public BuildingShopDataState usage;

		// Token: 0x040067ED RID: 26605
		public Image buildingImage;

		// Token: 0x040067EE RID: 26606
		public TMP_Text buildingName;

		// Token: 0x040067EF RID: 26607
		public TMP_Text message;

		// Token: 0x040067F0 RID: 26608
		public MaterialAmountView costs;

		// Token: 0x040067F1 RID: 26609
		public UiIndicator newBadge;

		// Token: 0x040067F2 RID: 26610
		public MaterialAmountView income;

		// Token: 0x040067F3 RID: 26611
		public MaterialAmountView incomeSeason;

		// Token: 0x040067F4 RID: 26612
		public GameObject incomeBubbleSmall;

		// Token: 0x040067F5 RID: 26613
		public GameObject incomeBubbleBig;

		// Token: 0x02000A3A RID: 2618
		public struct BuildingBuildRequest
		{
			// Token: 0x06003EC4 RID: 16068 RVA: 0x0013F170 File Offset: 0x0013D570
			public static float CalculateZoomFactorBasedOnSize(BuildingConfig building)
			{
				float num = 5f;
				float zoom_IN_FACTOR_PLACING_SMALLEST_BUILDING = BuildingShopView.BuildingBuildRequest.ZOOM_IN_FACTOR_PLACING_SMALLEST_BUILDING;
				float zoom_IN_FACTOR_PLACING_LARGEST_BUILDING = BuildingShopView.BuildingBuildRequest.ZOOM_IN_FACTOR_PLACING_LARGEST_BUILDING;
				return zoom_IN_FACTOR_PLACING_SMALLEST_BUILDING - (zoom_IN_FACTOR_PLACING_SMALLEST_BUILDING - zoom_IN_FACTOR_PLACING_LARGEST_BUILDING) * ((float)(building.size - 1) / num);
			}

			// Token: 0x040067F6 RID: 26614
			public static float ZOOM_IN_FACTOR_PLACING_SMALLEST_BUILDING = 1f;

			// Token: 0x040067F7 RID: 26615
			public static float ZOOM_IN_FACTOR_PLACING_LARGEST_BUILDING = 0.5f;

			// Token: 0x040067F8 RID: 26616
			public BuildingConfig Config;

			// Token: 0x040067F9 RID: 26617
			public bool shouldZoomIn;

			// Token: 0x040067FA RID: 26618
			public bool isFree;

			// Token: 0x040067FB RID: 26619
			public bool isFromStorage;

			// Token: 0x040067FC RID: 26620
			public bool shouldPanCamera;

			// Token: 0x040067FD RID: 26621
			public float zoomInRatio;
		}
	}
}
