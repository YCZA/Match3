using UnityEngine;

// Token: 0x02000AD7 RID: 2775
namespace Match3.Scripts1
{
	public static class Vector2Extension
	{
		// Token: 0x060041C7 RID: 16839 RVA: 0x00153942 File Offset: 0x00151D42
		public static Vector2 Snap(this Vector2 v)
		{
			return new Vector2(Mathf.Floor(v.x + 0.5f), Mathf.Floor(v.y + 0.5f));
		}
	}
}
