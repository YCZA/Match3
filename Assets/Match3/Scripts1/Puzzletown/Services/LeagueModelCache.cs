using System.Collections.Generic;
using Match3.Scripts1.Wooga.Signals;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200080D RID: 2061
	public class LeagueModelCache
	{
		// Token: 0x060032E4 RID: 13028 RVA: 0x000EFD5C File Offset: 0x000EE15C
		public LeagueModelCache()
		{
			this.cache = new Dictionary<string, LeagueModel>();
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x000EFD85 File Offset: 0x000EE185
		public void Clear()
		{
			this.cache.Clear();
			this.onActiveEventStateChanged.Dispatch();
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x000EFD9D File Offset: 0x000EE19D
		public bool TryGet(string key, out LeagueModel model)
		{
			return this.cache.TryGetValue(key, out model);
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x000EFDAC File Offset: 0x000EE1AC
		public void Add(LeagueModel leagueModel)
		{
			if (leagueModel != null)
			{
				this.cache[leagueModel.config.id] = leagueModel;
				this.cacheDirty = true;
			}
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x000EFDD2 File Offset: 0x000EE1D2
		public void SendNotifications()
		{
			if (this.cacheDirty)
			{
				this.cacheDirty = false;
				this.onActiveEventStateChanged.Dispatch();
			}
		}

		// Token: 0x04005B43 RID: 23363
		public Signal onActiveEventStateChanged = new Signal();

		// Token: 0x04005B44 RID: 23364
		private bool cacheDirty;

		// Token: 0x04005B45 RID: 23365
		private Dictionary<string, LeagueModel> cache = new Dictionary<string, LeagueModel>();
	}
}
