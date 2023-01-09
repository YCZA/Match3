using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006AB RID: 1707
	public class GemColorSpriteManager : SpriteManager<GemColor>
	{
		// Token: 0x06002AAB RID: 10923 RVA: 0x000C343C File Offset: 0x000C183C
		protected override void CreateEnumMap()
		{
			this.mapEnumToString = new Dictionary<GemColor, string>();
			IEnumerator enumerator = Enum.GetValues(typeof(GemColor)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					GemColor key = (GemColor)obj;
					this.mapEnumToString[key] = string.Format("dot_{0}", key.ToString().ToLower());
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x000C34D8 File Offset: 0x000C18D8
		public Sprite GetSpriteByName(string fullName)
		{
			return base[fullName];
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x000C34E1 File Offset: 0x000C18E1
		public void SetSprite(GemColor color, Sprite sprite)
		{
			if (this.mapEnumToString == null)
			{
				this.CreateEnumMap();
			}
			base[this.mapEnumToString[color]] = sprite;
		}

		// Token: 0x040053F9 RID: 21497
		public bool animateRainbowGems;

		// Token: 0x040053FA RID: 21498
		private const string PREFIX = "dot_{0}";
	}
}
