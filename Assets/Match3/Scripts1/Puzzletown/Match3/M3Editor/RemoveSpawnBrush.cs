using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000521 RID: 1313
	public class RemoveSpawnBrush : ABrush
	{
		// Token: 0x0600235B RID: 9051 RVA: 0x0009CFD5 File Offset: 0x0009B3D5
		public RemoveSpawnBrush(Sprite sprite) : base(sprite, null, 0)
		{
			this.brushComponents.Add(new RemoveSpawnComponent());
		}
	}
}
