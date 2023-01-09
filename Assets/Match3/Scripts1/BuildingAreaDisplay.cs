using System;
using Match3.Scripts1.Town;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;

// Token: 0x02000957 RID: 2391
namespace Match3.Scripts1
{
	public class BuildingAreaDisplay : MonoBehaviour, IDataView<BuildingInstance>
	{
		// Token: 0x06003A55 RID: 14933 RVA: 0x00120938 File Offset: 0x0011ED38
		public void Show(BuildingInstance building)
		{
			if (this.display != null && building.blueprint.IsRubble())
			{
				this.display.transform.gameObject.SetActive(false);
				return;
			}
			this.display.localScale = Vector3.one * (float)building.blueprint.size;
			building.onSelected.AddListener(new Action<BuildingInstance, bool>(this.HandleSelected));
		}

		// Token: 0x06003A56 RID: 14934 RVA: 0x001209B5 File Offset: 0x0011EDB5
		public void HandleSelected(BuildingInstance building, bool selected)
		{
			this.display.gameObject.SetActive(!selected);
		}

		// Token: 0x04006251 RID: 25169
		public Transform display;
	}
}
