using Match3.Scripts1.Town;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;

// Token: 0x0200095C RID: 2396
namespace Match3.Scripts1
{
	public class BuildingAssetVariantor : ABuildingAssetSwitcher, IDataView<BuildingInstance>, IBuildingOnPositionChanged
	{
		// Token: 0x06003A82 RID: 14978 RVA: 0x00121D5D File Offset: 0x0012015D
		public void Show(BuildingInstance building)
		{
			this.HandlePositionChanged(building);
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x00121D68 File Offset: 0x00120168
		public void HandlePositionChanged(BuildingInstance b)
		{
			if (this.variants.Length == 0)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"No asset variants on",
					base.name
				});
				return;
			}
			if (b.blueprint.size == 0)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Blueprint size is zero for",
					b.blueprint.name
				});
				return;
			}
			IntVector2 location = BuildingLocation.GetLocation(b);
			int b2 = (location.x + location.y) / b.blueprint.size % this.variants.Length;
			base.SwitchAsset(Mathf.Max(0, b2), b);
		}
	}
}
