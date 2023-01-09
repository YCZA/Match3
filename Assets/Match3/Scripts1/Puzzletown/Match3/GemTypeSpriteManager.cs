using System;
using System.Collections;
using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006AD RID: 1709
	public class GemTypeSpriteManager : SpriteManager<GemType>
	{
		// Token: 0x06002AB2 RID: 10930 RVA: 0x000C3550 File Offset: 0x000C1950
		protected override void CreateEnumMap()
		{
			this.mapEnumToString = new Dictionary<GemType, string>();
			IEnumerator enumerator = Enum.GetValues(typeof(GemType)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					GemType key = (GemType)obj;
					this.mapEnumToString[key] = string.Format(this.prefix, key.ToString().ToLower());
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

		// Token: 0x040053FC RID: 21500
		private string prefix = "super_{0}";
	}
}
