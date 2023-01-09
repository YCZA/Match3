using UnityEngine;

// Token: 0x02000AD6 RID: 2774
namespace Match3.Scripts1
{
	public static class TransformExtensions
	{
		// Token: 0x060041C6 RID: 16838 RVA: 0x0015391C File Offset: 0x00151D1C
		public static Transform CreateChild(this Transform transform, string name)
		{
			return new GameObject(name)
			{
				transform = 
				{
					parent = transform
				}
			}.transform;
		}
	}
}
