using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;

// Token: 0x02000A4B RID: 2635
namespace Match3.Scripts1
{
	public class TournamentBadgeSpriteManager : SpriteManager<TournamentBadgeIcon>
	{
		// Token: 0x06003F1D RID: 16157 RVA: 0x0014271C File Offset: 0x00140B1C
		protected override void CreateEnumMap()
		{
			this.mapEnumToString = new Dictionary<TournamentBadgeIcon, string>();
			IEnumerator enumerator = Enum.GetValues(typeof(TournamentType)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					TournamentType tType = (TournamentType)obj;
					TournamentBadgeIcon key = new TournamentBadgeIcon(tType, false);
					string text = string.Format("{0}_{1}", "ui_icon_town_tournament", tType.ToString().ToLower());
					this.mapEnumToString[key] = text;
					TournamentBadgeIcon key2 = new TournamentBadgeIcon(tType, true);
					this.mapEnumToString[key2] = string.Format("{0}_glow", text);
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

		// Token: 0x0400689B RID: 26779
		private const string prefix = "ui_icon_town_tournament";
	}
}
