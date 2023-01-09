using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;

// Token: 0x02000A5F RID: 2655
namespace Match3.Scripts1
{
	public class TournamentTaskSpriteManager : SpriteManager<TournamentType>
	{
		// Token: 0x06003F9F RID: 16287 RVA: 0x00146188 File Offset: 0x00144588
		protected override void CreateEnumMap()
		{
			this.mapEnumToString = new Dictionary<TournamentType, string>();
			IEnumerator enumerator = Enum.GetValues(typeof(TournamentType)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					TournamentType key = (TournamentType)obj;
					this.mapEnumToString[key] = string.Format("{0}{1}", "ui_tournament_task_", key.ToString().ToLower());
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

		// Token: 0x0400694B RID: 26955
		private const string prefix = "ui_tournament_task_";
	}
}
