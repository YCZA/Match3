using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BC4 RID: 3012
	[AddComponentMenu("UI/Effects/Extensions/BestFit Outline")]
	public class BestFitOutline : Shadow
	{
		// Token: 0x060046A6 RID: 18086 RVA: 0x001667C6 File Offset: 0x00164BC6
		protected BestFitOutline()
		{
		}

		// Token: 0x060046A7 RID: 18087 RVA: 0x001667D0 File Offset: 0x00164BD0
		public override void ModifyMesh(Mesh mesh)
		{
			if (!this.IsActive())
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			using (VertexHelper vertexHelper = new VertexHelper(mesh))
			{
				vertexHelper.GetUIVertexStream(list);
			}
			Text component = base.GetComponent<Text>();
			float num = 1f;
			if (component && component.resizeTextForBestFit)
			{
				num = (float)component.cachedTextGenerator.fontSizeUsedForBestFit / (float)(component.resizeTextMaxSize - 1);
			}
			int start = 0;
			int count = list.Count;
			base.ApplyShadowZeroAlloc(list, base.effectColor, start, list.Count, base.effectDistance.x * num, base.effectDistance.y * num);
			start = count;
			count = list.Count;
			base.ApplyShadowZeroAlloc(list, base.effectColor, start, list.Count, base.effectDistance.x * num, -base.effectDistance.y * num);
			start = count;
			count = list.Count;
			base.ApplyShadowZeroAlloc(list, base.effectColor, start, list.Count, -base.effectDistance.x * num, base.effectDistance.y * num);
			start = count;
			count = list.Count;
			base.ApplyShadowZeroAlloc(list, base.effectColor, start, list.Count, -base.effectDistance.x * num, -base.effectDistance.y * num);
			using (VertexHelper vertexHelper2 = new VertexHelper())
			{
				vertexHelper2.AddUIVertexTriangleStream(list);
				vertexHelper2.FillMesh(mesh);
			}
		}
	}
}
