using System.Collections.Generic;
using System.IO;
using System.Linq;
using Match3.Scripts1.Town;
using UnityEngine;

// Token: 0x02000950 RID: 2384
namespace Match3.Scripts1
{
	public class TownEditorMode
	{
		// Token: 0x060039FB RID: 14843 RVA: 0x0011E024 File Offset: 0x0011C424
		public void Save(IEnumerable<BuildingInstance.PersistentData> buildings)
		{
			StartBuildingConfig startBuildingConfig = JsonUtility.FromJson<StartBuildingConfig>(File.ReadAllText("Assets/Puzzletown/Config/Json/startbuildingconfig.json.txt"));
			this.pathfinding = global::UnityEngine.Object.FindObjectOfType<TownPathfinding>();
			StartBuilding[] buildings2 = (from b in buildings
				select this.ToStartBuilding(b)).ToArray<StartBuilding>();
			startBuildingConfig.UpdateIsland(this.pathfinding.islandId, buildings2);
			string message = JsonUtility.ToJson(startBuildingConfig, true);
			global::UnityEngine.Debug.Log(message);
			File.WriteAllText("Assets/Puzzletown/Config/Json/startbuildingconfig.json.txt", JsonUtility.ToJson(startBuildingConfig, true));
		}

		// Token: 0x060039FC RID: 14844 RVA: 0x0011E098 File Offset: 0x0011C498
		private StartBuilding ToStartBuilding(BuildingInstance.PersistentData data)
		{
			return new StartBuilding
			{
				area = this.pathfinding.GetLocalArea(data.position),
				building_id = data.blueprintName,
				deco_set = data.DecoSet,
				destroyed = !data.IsRepaired,
				pos_x = data.position.x,
				pos_y = data.position.y
			};
		}

		// Token: 0x04006211 RID: 25105
		private const string configPath = "Assets/Puzzletown/Config/Json/startbuildingconfig.json.txt";

		// Token: 0x04006212 RID: 25106
		private TownPathfinding pathfinding;
	}
}
