using System;
using System.Linq;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.Signals;

// Token: 0x0200096B RID: 2411
namespace Match3.Scripts1
{
	public class VillageRankHarmonyObserver
	{
		// Token: 0x06003ACD RID: 15053 RVA: 0x00123730 File Offset: 0x00121B30
		public VillageRankHarmonyObserver(VillageRankConfig config, ResourceDataService resources)
		{
			this._resources = resources;
			this._resources.onChanged.AddListener(new Action<MaterialChange>(this.HandleMaterialChange));
			this._sortedRanks = (from r in config.ranks
				orderby r.village_rank
				select r).ToArray<VillageRank>();
			int amount = this._resources.GetAmount("harmony");
			this.CurrentRankData = this.RankForHarmony(amount);
			this.UpdateCollectedAndRequired();
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06003ACE RID: 15054 RVA: 0x001237D3 File Offset: 0x00121BD3
		// (set) Token: 0x06003ACF RID: 15055 RVA: 0x001237DB File Offset: 0x00121BDB
		public VillageRank CurrentRankData { get; private set; }

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06003AD0 RID: 15056 RVA: 0x001237E4 File Offset: 0x00121BE4
		public VillageRank NextRankData
		{
			get
			{
				return this.NextRank(this.CurrentRankData);
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06003AD1 RID: 15057 RVA: 0x001237F2 File Offset: 0x00121BF2
		public int CurrentRank
		{
			get
			{
				return this.CurrentRankData.village_rank;
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06003AD2 RID: 15058 RVA: 0x001237FF File Offset: 0x00121BFF
		// (set) Token: 0x06003AD3 RID: 15059 RVA: 0x00123807 File Offset: 0x00121C07
		public int HarmonyCollected { get; private set; }

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06003AD4 RID: 15060 RVA: 0x00123810 File Offset: 0x00121C10
		// (set) Token: 0x06003AD5 RID: 15061 RVA: 0x00123818 File Offset: 0x00121C18
		public int HarmonyRequried { get; private set; }

		// Token: 0x06003AD6 RID: 15062 RVA: 0x00123824 File Offset: 0x00121C24
		public VillageRank RankForHarmony(int harmony)
		{
			for (int i = this._sortedRanks.Length - 1; i >= 0; i--)
			{
				if (harmony >= this._sortedRanks[i].harmony_goal)
				{
					return this._sortedRanks[i];
				}
			}
			WoogaDebug.Log(new object[]
			{
				"Harmony is :",
				harmony
			});
			return null;
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x00123888 File Offset: 0x00121C88
		private int IndexForRank(int rank)
		{
			for (int i = 0; i < this._sortedRanks.Length; i++)
			{
				if (rank == this._sortedRanks[i].village_rank)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06003AD8 RID: 15064 RVA: 0x001238C4 File Offset: 0x00121CC4
		public VillageRank NextRank(VillageRank currentRank)
		{
			if (currentRank == this._sortedRanks[this._sortedRanks.Length - 1])
			{
				return this._sortedRanks[this._sortedRanks.Length - 1];
			}
			int num = this.IndexForRank(currentRank.village_rank);
			if (num == -1)
			{
				return null;
			}
			return this._sortedRanks[num + 1];
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x0012391C File Offset: 0x00121D1C
		private void HandleMaterialChange(MaterialChange material)
		{
			if (material.name != "harmony")
			{
				return;
			}
			int amount = this._resources.GetAmount("harmony");
			VillageRank villageRank = this.RankForHarmony(amount);
			while (villageRank.village_rank > this.CurrentRankData.village_rank)
			{
				this.CurrentRankData = this.NextRank(this.CurrentRankData);
				this.OnVillageRankChanged.Dispatch(this);
				WoogaDebug.Log(new object[]
				{
					"NEW VILLAGE RANK: ",
					this.CurrentRankData.village_rank
				});
			}
			this.UpdateCollectedAndRequired();
			this.OnVillageRankProgressChanged.Dispatch(this);
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x001239CC File Offset: 0x00121DCC
		private void UpdateCollectedAndRequired()
		{
			int amount = this._resources.GetAmount("harmony");
			VillageRank villageRank = this.NextRank(this.CurrentRankData);
			if (villageRank != null)
			{
				this.HarmonyCollected = amount - this.CurrentRankData.harmony_goal;
				this.HarmonyRequried = villageRank.harmony_goal - this.CurrentRankData.harmony_goal;
			}
			else
			{
				this.HarmonyCollected = 0;
				this.HarmonyRequried = -1;
			}
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x00123A3B File Offset: 0x00121E3B
		public void OnDestroy()
		{
			this._resources.onChanged.RemoveListener(new Action<MaterialChange>(this.HandleMaterialChange));
		}

		// Token: 0x040062B0 RID: 25264
		private ResourceDataService _resources;

		// Token: 0x040062B1 RID: 25265
		private VillageRank[] _sortedRanks;

		// Token: 0x040062B2 RID: 25266
		public readonly Signal<VillageRankHarmonyObserver> OnVillageRankProgressChanged = new Signal<VillageRankHarmonyObserver>();

		// Token: 0x040062B3 RID: 25267
		public readonly Signal<VillageRankHarmonyObserver> OnVillageRankChanged = new Signal<VillageRankHarmonyObserver>();
	}
}
