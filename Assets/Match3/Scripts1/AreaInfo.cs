using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000954 RID: 2388
namespace Match3.Scripts1
{
	[Serializable]
	public class AreaInfo
	{
		// 存的是excel表中的坐标(列，行)，从0开始
		public List<IntVector2> tiles = new List<IntVector2>();

		// Token: 0x04006218 RID: 25112
		public Mesh areaMesh;

		// Token: 0x04006219 RID: 25113
		public Mesh cloudMesh;

		// Token: 0x0400621A RID: 25114
		public Texture2D cloudTexture;
	}
}
