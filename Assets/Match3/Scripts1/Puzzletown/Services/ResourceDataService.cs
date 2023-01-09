using System;
using System.Collections.Generic;
using Match3.Scripts1.Wooga.Signals;
using Match3.Scripts2.Android.DataStatistics;
using Match3.Scripts2.PlayerData; // using Firebase.Analytics;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007E9 RID: 2025
	public class ResourceDataService : ADataService
	{
		// Token: 0x0600322E RID: 12846 RVA: 0x000EC670 File Offset: 0x000EAA70
		public ResourceDataService(Func<GameState> getState) : base(getState)
		{
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x0600322F RID: 12847 RVA: 0x000EC68F File Offset: 0x000EAA8F
		private Materials More
		{
			get
			{
				return base.state.resources.more;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06003230 RID: 12848 RVA: 0x000EC6A1 File Offset: 0x000EAAA1
		private Materials Less
		{
			get
			{
				return base.state.resources.less;
			}
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x000EC6B3 File Offset: 0x000EAAB3
		public int GetAmount(string name)
		{
			return this.More[name] - this.Less[name];
		}

		// Token: 0x06003232 RID: 12850 RVA: 0x000EC6CE File Offset: 0x000EAACE
		public int GetCollectedTotal(string name)
		{
			return this.More[name];
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06003233 RID: 12851 RVA: 0x000EC6DC File Offset: 0x000EAADC
		public Materials Current
		{
			get
			{
				return this.More - this.Less;
			}
		}

		// Token: 0x06003234 RID: 12852 RVA: 0x000EC6F0 File Offset: 0x000EAAF0
		public void AddMaterial(string name, int amount, int max, bool needsSave = true, string way = "unknown")
		{
			// eli key point add material 消耗或增加资源
			MaterialChange value = new MaterialChange(name, this.GetAmount(name), 0);
			if (amount > 0)
			{
				// 增加
				int num = Math.Min(max - this.GetAmount(name), amount);
				int actualNum = max != -1 ? num : amount;
				Materials materials;
				(materials = this.More)[name] = materials[name] + actualNum;

				if (amount != 0)
				{
					DataStatistics.Instance.TriggerGetResources(name, actualNum, way);
				}
			}
			else
			{
				// 消耗
				Materials materials;
				(materials = this.Less)[name] = materials[name] - amount;
				
				if (amount != 0)
				{
					DataStatistics.Instance.TriggerConsumeResources(name, amount ,way);
				}
			}
			
			value.after = this.GetAmount(name);
			this.onChanged.Dispatch(value);
			if (needsSave)
			{
				this.onNeedsSave.Dispatch();
			}
		}

		// Token: 0x06003235 RID: 12853 RVA: 0x000EC796 File Offset: 0x000EAB96
		public void RemoveMaterial(string name)
		{
			this.More.Remove(name);
			this.Less.Remove(name);
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x000EC7B0 File Offset: 0x000EABB0
		public void AddMaterial(string name, int amount, bool needsSave = true, string way = "unknown")
		{
			this.AddMaterial(name, amount, -1, needsSave, way);
		}

		// Token: 0x06003237 RID: 12855 RVA: 0x000EC7BC File Offset: 0x000EABBC
		public void AddMaterial(MaterialAmount material, bool needsSave = true, string way = "unknown")
		{
			this.AddMaterial(material.type, material.amount, needsSave, way);
		}

		// Token: 0x06003238 RID: 12856 RVA: 0x000EC7D4 File Offset: 0x000EABD4
		public void AddMaterials(IEnumerable<MaterialAmount> materials, bool needsSave = true, string way = "unknown")
		{
			foreach (MaterialAmount materialAmount in materials)
			{
				this.AddMaterial(materialAmount.type, materialAmount.amount, false, way);
			}
			if (needsSave)
			{
				this.onNeedsSave.Dispatch();
			}
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x000EC848 File Offset: 0x000EAC48
		public bool HasEnoughMaterial(MaterialAmount amount)
		{
			return this.HasEnoughMaterial(amount.type, amount.amount);
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x000EC85E File Offset: 0x000EAC5E
		public bool HasEnoughMaterial(string name, int amount)
		{
			return this.GetAmount(name) >= amount;
		}

		// Token: 0x0600323B RID: 12859 RVA: 0x000EC870 File Offset: 0x000EAC70
		public bool HasMaterials(IEnumerable<MaterialAmount> materials)
		{
			foreach (MaterialAmount materialAmount in materials)
			{
				if (!this.HasEnoughMaterial(materialAmount.type, materialAmount.amount))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x000EC8E0 File Offset: 0x000EACE0
		public bool Pay(IEnumerable<MaterialAmount> materials)
		{
			if (this.HasMaterials(materials))
			{
				foreach (MaterialAmount materialAmount in materials)
				{
					this.AddMaterial(materialAmount.type, -materialAmount.amount, true);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x000EC954 File Offset: 0x000EAD54
		public bool Pay(MaterialAmount mat)
		{
			if (this.HasEnoughMaterial(mat.type, mat.amount))
			{
				this.AddMaterial(mat.type, -mat.amount, true);
				return true;
			}
			return false;
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x000EC988 File Offset: 0x000EAD88
		public void AddPendingRewards(Materials materials, PendingRewards.TrackingCall trackingCall)
		{
			PendingRewards item = new PendingRewards(materials, trackingCall);
			base.state.resources.pendingResources.Add(item);
		}

		// Token: 0x0600323F RID: 12863 RVA: 0x000EC9B3 File Offset: 0x000EADB3
		public bool HasPendingRewards()
		{
			return !base.state.resources.pendingResources.IsNullOrEmptyCollection();
		}

		// Token: 0x06003240 RID: 12864 RVA: 0x000EC9CD File Offset: 0x000EADCD
		public PendingRewards GetPendingRewards()
		{
			return base.state.resources.pendingResources[0];
		}

		// Token: 0x06003241 RID: 12865 RVA: 0x000EC9E5 File Offset: 0x000EADE5
		public void RemovePendingRewards(PendingRewards rewards)
		{
			base.state.resources.pendingResources.Remove(rewards);
		}

		// Token: 0x04005AB1 RID: 23217
		public readonly Signal<MaterialChange> onChanged = new Signal<MaterialChange>();

		// Token: 0x04005AB2 RID: 23218
		public readonly Signal onNeedsSave = new Signal();
	}
}
