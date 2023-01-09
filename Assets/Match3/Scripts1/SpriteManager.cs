using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1
{
	// Token: 0x02000B20 RID: 2848
	public class SpriteManager : ResourceManager<Sprite>
	{
	}

	public abstract class SpriteManager<T> : SpriteManager
	{
		// Token: 0x060042EA RID: 17130 RVA: 0x000C33F8 File Offset: 0x000C17F8
		public Sprite GetSprite(T type)
		{
			if (this.mapEnumToString == null)
			{
				this.CreateEnumMap();
			}
			string key;
			if (this.mapEnumToString.TryGetValue(type, out key))
			{
				return base[key];
			}
			return null;
		}

		// Token: 0x060042EB RID: 17131
		protected abstract void CreateEnumMap();

		// Token: 0x04006B9F RID: 27551
		protected Dictionary<T, string> mapEnumToString;
	}
}