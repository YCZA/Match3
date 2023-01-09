using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Town;
using UnityEngine;

// Token: 0x02000924 RID: 2340
namespace Match3.Scripts1
{
	public class RepairBuildingsByTagAction : QuestActionHandler<RepairBuildingsByTagAction.command>
	{
		// Token: 0x060038FB RID: 14587 RVA: 0x00118C6C File Offset: 0x0011706C
		public RepairBuildingsByTagAction(BuildingsController buildings, TownUiRoot townUi)
		{
			this.buildings = buildings;
			this.townUi = townUi;
		}

		// Token: 0x060038FC RID: 14588 RVA: 0x00118C84 File Offset: 0x00117084
		public override void DoHandle(RepairBuildingsByTagAction.command action)
		{
			// eli key point 和修复有关
			RepairBuildingsByTagAction.RepairFlow repairFlow = new RepairBuildingsByTagAction.RepairFlow(this.buildings, this.townUi, action.buildingTag);
			BlockerManager.global.Append(new Func<IEnumerator>(repairFlow.Start), true);
		}

		// Token: 0x04006154 RID: 24916
		private BuildingsController buildings;

		// Token: 0x04006155 RID: 24917
		private TownUiRoot townUi;

		// Token: 0x02000925 RID: 2341
		public class command : IQuestAction
		{
			// Token: 0x060038FD RID: 14589 RVA: 0x00118CC0 File Offset: 0x001170C0
			public command(string buildingTag)
			{
				this.buildingTag = buildingTag;
			}

			// Token: 0x04006156 RID: 24918
			public string buildingTag;
		}

		// Token: 0x02000926 RID: 2342
		public class RepairFlow
		{
			// Token: 0x060038FE RID: 14590 RVA: 0x00118CD0 File Offset: 0x001170D0
			public RepairFlow(BuildingsController buildings, TownUiRoot townUi, string tag)
			{
				this.buildings = buildings;
				this.tag = tag;
				this.townUi = townUi;
				IEnumerable<BuildingInstance> enumerable = buildings.BuildingsByDecoTag(tag);
				foreach (BuildingInstance buildingInstance in enumerable)
				{
					buildingInstance.RepairInModel();
				}
			}

			// Token: 0x060038FF RID: 14591 RVA: 0x00118D48 File Offset: 0x00117148
			public IEnumerator Start()
			{
				IEnumerable<BuildingInstance> toRepair = this.buildings.BuildingsByDecoTag(this.tag);
				this.townUi.ShowUi(false);
				while (!Camera.main)
				{
					yield return null;
				}
				if (RepairBuildingsByTagAction.RepairFlow.doPanning)
				{
					BuildingInstance building = toRepair.FirstOrDefault<BuildingInstance>();
					if (building != null)
					{
						yield return Camera.main.GetComponent<CameraPanManager>().PanTo(building.view.FocusPosition, false, 0.5f);
					}
				}
				foreach (BuildingInstance b in toRepair)
				{
					b.view.GetComponent<BuildingAssetLoader>().onRefreshFinished.AddListenerOnce(new Action(this.HandleRefreshFinished));
					b.Repair(true);
					if (b.blueprint.IsRubble())
					{
						yield return new WaitForSeconds(4f);
					}
				}
				int count = toRepair.Count<BuildingInstance>();
				while (this.numFinished != count)
				{
					yield return null;
				}
				this.townUi.ShowUi(true);
				yield break;
			}

			// Token: 0x06003900 RID: 14592 RVA: 0x00118D63 File Offset: 0x00117163
			private void HandleRefreshFinished()
			{
				this.numFinished++;
			}

			// Token: 0x04006157 RID: 24919
			private const float RUBBLE_DELAY = 4f;

			// Token: 0x04006158 RID: 24920
			public static bool doPanning = true;

			// Token: 0x04006159 RID: 24921
			public BuildingsController buildings;

			// Token: 0x0400615A RID: 24922
			private TownUiRoot townUi;

			// Token: 0x0400615B RID: 24923
			private string tag;

			// Token: 0x0400615C RID: 24924
			private int numFinished;
		}
	}
}
