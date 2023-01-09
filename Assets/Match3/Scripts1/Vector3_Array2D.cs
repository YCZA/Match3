using System;
using UnityEngine;

namespace Match3.Scripts1
{
	// Token: 0x02000BC8 RID: 3016
	[Serializable]
	public struct Vector3_Array2D
	{
		// Token: 0x17000A47 RID: 2631
		public Vector3 this[int _idx]
		{
			get
			{
				return this.array[_idx];
			}
			set
			{
				this.array[_idx] = value;
			}
		}

		// Token: 0x04006E0E RID: 28174
		[SerializeField]
		public Vector3[] array;
	}
}
