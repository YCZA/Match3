using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C24 RID: 3108
	[AddComponentMenu("Layout/Extensions/NonDrawingGraphic")]
	public class NonDrawingGraphic : MaskableGraphic
	{
		// Token: 0x0600495C RID: 18780 RVA: 0x001771DC File Offset: 0x001755DC
		public override void SetMaterialDirty()
		{
		}

		// Token: 0x0600495D RID: 18781 RVA: 0x001771DE File Offset: 0x001755DE
		public override void SetVerticesDirty()
		{
		}

		// Token: 0x0600495E RID: 18782 RVA: 0x001771E0 File Offset: 0x001755E0
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
		}
	}
}
