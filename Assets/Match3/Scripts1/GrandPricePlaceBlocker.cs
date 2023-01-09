using System.Collections;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;

// Token: 0x02000A3F RID: 2623
namespace Match3.Scripts1
{
	public class GrandPricePlaceBlocker : IBlocker
	{
		// Token: 0x06003ED0 RID: 16080 RVA: 0x0013F9F7 File Offset: 0x0013DDF7
		public GrandPricePlaceBlocker(BuildingConfig config)
		{
			this.buildingConfig = config;
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x0013FA08 File Offset: 0x0013DE08
		public IEnumerator ExecuteRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			Wooroutine<PopupGrandPrizeCongratsRoot> popupRoutine = SceneManager.Instance.LoadSceneWithParams<PopupGrandPrizeCongratsRoot, BuildingConfig>(this.buildingConfig, null);
			yield return popupRoutine;
			yield return popupRoutine.ReturnValue.onDestroyed;
			this.gameStateService.Buildings.StoreBuilding(this.buildingConfig, 1);
			this.gameStateService.Resources.AddMaterial("season_currency", -this.buildingConfig.costs[0].amount, true);
			yield return new ForceUserPlaceDecoFlow(this.buildingConfig).Start();
			yield break;
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06003ED2 RID: 16082 RVA: 0x0013FA23 File Offset: 0x0013DE23
		public bool BlockInput
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04006815 RID: 26645
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04006816 RID: 26646
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04006817 RID: 26647
		private BuildingConfig buildingConfig;
	}
}
