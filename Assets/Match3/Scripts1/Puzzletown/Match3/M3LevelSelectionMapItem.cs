using Match3.Scripts1.Wooga.UI;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006FE RID: 1790
	public class M3LevelSelectionMapItem : ATableViewReusableCell, IDataView<LevelSelectionData>
	{
		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06002C62 RID: 11362 RVA: 0x000CC8E0 File Offset: 0x000CACE0
		public override int reusableId
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x000CC8E8 File Offset: 0x000CACE8
		public void Show(LevelSelectionData data)
		{
			this.level = data.setConfig.level;
			foreach (IDataView<LevelSelectionData> dataView in base.GetComponentsInChildren<IDataView<LevelSelectionData>>(true))
			{
				if (dataView != this)
				{
					dataView.Show(data);
				}
			}
			this.SetupPath(data);
		}

		// Token: 0x06002C64 RID: 11364 RVA: 0x000CC944 File Offset: 0x000CAD44
		private void SetupPath(LevelSelectionData data)
		{
			for (int i = 0; i < this.paths.Length; i++)
			{
				this.paths[i].SetActive(false);
			}
			int num = 0;
			LevelSelectionData.State currentState = data.CurrentState;
			if (currentState != LevelSelectionData.State.Active)
			{
				if (data.itemState != M3_LevelMapItemState.None)
				{
					num = (int)data.VisualState;
				}
			}
			else
			{
				num = (int)(data.CurrentState + ((data.levelOrder != LevelSelectionData.LevelOrder.NextLatest) ? 1 : 0));
			}
			this.paths[num].SetActive(true);
		}

		// Token: 0x040055AF RID: 21935
		public int id;

		// Token: 0x040055B0 RID: 21936
		public GameObject[] paths;

		// Token: 0x040055B1 RID: 21937
		public int level;
	}
}
