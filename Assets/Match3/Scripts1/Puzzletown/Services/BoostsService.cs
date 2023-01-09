using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007A1 RID: 1953
	public class BoostsService : AService
	{
		// Token: 0x06002FDB RID: 12251 RVA: 0x000E12F8 File Offset: 0x000DF6F8
		public BoostsService(ResourceDataService resources)
		{
			this.resources = resources;
			WooroutineRunner.StartCoroutine(this.LoadRoutine(), null);
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x000E1370 File Offset: 0x000DF770
		public BoostViewData[] GetInGameBoostInfos(bool usesWaterAllFields)
		{
			return this.GetGameBoostInfos(this.ingameBoosts, usesWaterAllFields);
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x000E137F File Offset: 0x000DF77F
		public BoostViewData[] GetPreGameBoostInfos()
		{
			return this.GetGameBoostInfos(this.preGameBoosts, false);
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x000E1390 File Offset: 0x000DF790
		private BoostViewData[] GetGameBoostInfos(List<Boosts> boosts, bool usesWaterAllFields = false)
		{
			BoostViewData[] array = new BoostViewData[boosts.Count];
			for (int i = 0; i < boosts.Count; i++)
			{
				string name = Enum.GetName(typeof(Boosts), boosts[i]);
				BoosterData boosterData = this.configService.general.boosters.Find((BoosterData d) => d.type == name);
				BoostState state = BoostState.Inactive;
				if (this.progressionService.UnlockedLevel >= boosterData.unlock_at_lvl)
				{
					state = BoostState.Active;
				}
				bool waterAllFields = boosts[i] == Boosts.boost_hammer && usesWaterAllFields;
				array[i] = new BoostViewData(name, this.resources.GetAmount(name), state, waterAllFields);
			}
			return array;
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x000E145C File Offset: 0x000DF85C
		public void AddBoost(string type, int amount)
		{
			this.resources.AddMaterial(type, amount, true, "钻石购买");
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x000E146C File Offset: 0x000DF86C
		private IEnumerator LoadRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			this.resources.onChanged.AddListener(new Action<MaterialChange>(this.HandleResourcesChanged));
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x000E1487 File Offset: 0x000DF887
		private void HandleResourcesChanged(MaterialChange change)
		{
			if (change.name.StartsWith("boost_"))
			{
				this.onBoostsChanged.Dispatch(new BoostViewData(change.name, change.after), change.Delta);
			}
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x000E14C4 File Offset: 0x000DF8C4
		public bool IsAnyIngameBoostSelected()
		{
			BoostViewData[] inGameBoostInfos = this.GetInGameBoostInfos(false);
			if (inGameBoostInfos != null)
			{
				foreach (BoostViewData boostViewData in inGameBoostInfos)
				{
					if (boostViewData.state == BoostState.Selected)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040058F2 RID: 22770
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040058F3 RID: 22771
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x040058F4 RID: 22772
		public const string BOOST_PREFIX = "boost_";

		// Token: 0x040058F5 RID: 22773
		public const string PRE_BOOST_PREFIX = "boost_pre_";

		// Token: 0x040058F6 RID: 22774
		private const int INITIAL_BOOSTS_AMOUNT = 3;

		// Token: 0x040058F7 RID: 22775
		public static List<MaterialAmount> InitialBoosts = new List<MaterialAmount>
		{
			new MaterialAmount(Enum.GetName(typeof(Boosts), Boosts.boost_hammer), 3, MaterialAmountUsage.Undefined, 0),
			new MaterialAmount(Enum.GetName(typeof(Boosts), Boosts.boost_star), 3, MaterialAmountUsage.Undefined, 0),
			new MaterialAmount(Enum.GetName(typeof(Boosts), Boosts.boost_rainbow), 3, MaterialAmountUsage.Undefined, 0),
			new MaterialAmount(Enum.GetName(typeof(Boosts), Boosts.boost_pre_bomb_linegem), 3, MaterialAmountUsage.Undefined, 0),
			new MaterialAmount(Enum.GetName(typeof(Boosts), Boosts.boost_pre_double_fish), 3, MaterialAmountUsage.Undefined, 0),
			new MaterialAmount(Enum.GetName(typeof(Boosts), Boosts.boost_pre_rainbow), 3, MaterialAmountUsage.Undefined, 0)
		};

		// Token: 0x040058F8 RID: 22776
		public readonly Signal<BoostViewData, int> onBoostsChanged = new Signal<BoostViewData, int>();

		// Token: 0x040058F9 RID: 22777
		private readonly List<Boosts> ingameBoosts = new List<Boosts>
		{
			Boosts.boost_hammer,
			Boosts.boost_star,
			Boosts.boost_rainbow
		};

		// Token: 0x040058FA RID: 22778
		private readonly List<Boosts> preGameBoosts = new List<Boosts>
		{
			Boosts.boost_pre_bomb_linegem,
			Boosts.boost_pre_double_fish,
			Boosts.boost_pre_rainbow
		};

		// Token: 0x040058FB RID: 22779
		private ResourceDataService resources;
	}
}
