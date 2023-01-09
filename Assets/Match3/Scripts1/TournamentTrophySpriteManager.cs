using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;

// Token: 0x02000A65 RID: 2661
namespace Match3.Scripts1
{
	public class TournamentTrophySpriteManager : SpriteManager<TournamentTrophy>
	{
		// Token: 0x06003FBA RID: 16314 RVA: 0x00141CA9 File Offset: 0x001400A9
		protected virtual string GetPrefix()
		{
			return "ui_tournament_trophy";
		}

		// Token: 0x06003FBB RID: 16315 RVA: 0x00141CB0 File Offset: 0x001400B0
		protected override void CreateEnumMap()
		{
			string[] array = new string[]
			{
				"gold",
				"silver",
				"bronze",
				"wood"
			};
			this.mapEnumToString = new Dictionary<TournamentTrophy, string>();
			IEnumerator enumerator = Enum.GetValues(typeof(TournamentType)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					TournamentType t = (TournamentType)obj;
					for (int i = 1; i < 4; i++)
					{
						TournamentTrophy key = new TournamentTrophy(t, i);
						string arg = array[i - 1];
						this.mapEnumToString[key] = string.Format("{0}_{1}_{2}", this.GetPrefix(), t.ToString().ToLower(), arg);
					}
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
	}
}
