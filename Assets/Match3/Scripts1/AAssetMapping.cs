using System;
using System.Collections.Generic;

// Token: 0x02000B18 RID: 2840
namespace Match3.Scripts1
{
	[Serializable]
	public abstract class AAssetMapping<TKey, TValue>
	{
		// Token: 0x1700099F RID: 2463
		public TValue this[TKey key]
		{
			get
			{
				return this.list.PtFirst((AssetMappingEntry<TKey, TValue> e) => e.key.Equals(key)).value;
			}
		}

		// Token: 0x04006B93 RID: 27539
		public List<AssetMappingEntry<TKey, TValue>> list;
	}
}
