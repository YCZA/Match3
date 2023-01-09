using UnityEngine;

// Token: 0x02000952 RID: 2386
namespace Match3.Scripts1
{
	public interface IPathfinder
	{
		// Token: 0x06003A01 RID: 14849
		bool GetPath(Vector2 _from, Vector2 _to, out Vector2[] path);
	}
}
