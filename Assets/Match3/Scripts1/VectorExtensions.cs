using UnityEngine;

// Token: 0x02000889 RID: 2185
namespace Match3.Scripts1
{
	public static class VectorExtensions
	{
		// Token: 0x06003594 RID: 13716 RVA: 0x001015C1 File Offset: 0x000FF9C1
		public static Vector2 xz(this Vector3 vector)
		{
			return new Vector2(vector.x, vector.z);
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x001015D6 File Offset: 0x000FF9D6
		public static Vector3 x0y(this Vector2 vector)
		{
			return new Vector3(vector.x, 0f, vector.y);
		}
	}
}
