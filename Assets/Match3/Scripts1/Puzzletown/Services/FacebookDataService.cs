using System;
using System.Collections.Generic;
using System.IO;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200078D RID: 1933
	public class FacebookDataService : ADataService, IFacebookDataStore
	{
		// Token: 0x06002F6B RID: 12139 RVA: 0x000DDDBD File Offset: 0x000DC1BD
		public FacebookDataService(Func<GameState> i_getState) : base(i_getState)
		{
			this.Load();
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x000DDDD7 File Offset: 0x000DC1D7
		public FacebookDataService(Action doSave, Func<GameState> getState) : base(getState)
		{
			this._doSave = doSave;
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06002F6D RID: 12141 RVA: 0x000DDDF2 File Offset: 0x000DC1F2
		private string _requestLogLocation
		{
			get
			{
				return Application.persistentDataPath + "/fbRequestLog.txt";
			}
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x000DDE04 File Offset: 0x000DC204
		public void Load()
		{
			this._requestCache = null;
			try
			{
				if (File.Exists(this._requestLogLocation))
				{
					string input = File.ReadAllText(this._requestLogLocation);
					this._requestCache = JSON.Deserialize<FBRequestCache>(input);
				}
			}
			catch (Exception ex)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Error loading request log: ",
					ex
				});
			}
			if (this._requestCache == null)
			{
				this._requestCache = new FBRequestCache();
			}
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x000DDE88 File Offset: 0x000DC288
		public void Save()
		{
			if (this._doSave != null)
			{
				this._doSave();
			}
			try
			{
				string contents = JSON.Serialize(this._requestCache, false, 1, ' ');
				File.WriteAllText(this._requestLogLocation, contents);
			}
			catch (Exception ex)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Error saving FB Request log: ",
					ex
				});
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06002F70 RID: 12144 RVA: 0x000DDEFC File Offset: 0x000DC2FC
		public FBRequestCache Data
		{
			get
			{
				return this._requestCache;
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06002F71 RID: 12145 RVA: 0x000DDF04 File Offset: 0x000DC304
		public Queue<PendingFacebookOperation> PendingOps
		{
			get
			{
				return this._requestCache.PendingFBOps;
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06002F72 RID: 12146 RVA: 0x000DDF11 File Offset: 0x000DC311
		public List<FacebookData.Request> Requests
		{
			get
			{
				return this._requestCache.Requests;
			}
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x000DDF1E File Offset: 0x000DC31E
		public List<FacebookData.FacebookRequestLog> GetRequestLog()
		{
			if (base.state.fbData.RequestLog == null)
			{
				base.state.fbData.RequestLog = new List<FacebookData.FacebookRequestLog>();
			}
			return base.state.fbData.RequestLog;
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x000DDF5A File Offset: 0x000DC35A
		public void SetRequestLog(List<FacebookData.FacebookRequestLog> newLog)
		{
			base.state.fbData.RequestLog = newLog;
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x000DDF6D File Offset: 0x000DC36D
		public void UpdateFriendsCount(int newCount)
		{
			this.friendCountHighWater = Math.Max(newCount, this.friendCountHighWater);
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06002F76 RID: 12150 RVA: 0x000DDF81 File Offset: 0x000DC381
		// (set) Token: 0x06002F77 RID: 12151 RVA: 0x000DDF93 File Offset: 0x000DC393
		public bool receivedLoginReward
		{
			get
			{
				return base.state.fbData.receivedLoginReward;
			}
			set
			{
				base.state.fbData.receivedLoginReward = value;
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002F78 RID: 12152 RVA: 0x000DDFA6 File Offset: 0x000DC3A6
		// (set) Token: 0x06002F79 RID: 12153 RVA: 0x000DDFB8 File Offset: 0x000DC3B8
		public int friendCountHighWater
		{
			get
			{
				return base.state.fbData.friendCountHighWater;
			}
			set
			{
				base.state.fbData.friendCountHighWater = value;
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002F7A RID: 12154 RVA: 0x000DDFCB File Offset: 0x000DC3CB
		// (set) Token: 0x06002F7B RID: 12155 RVA: 0x000DDFDD File Offset: 0x000DC3DD
		public string loggedInUserFirstName
		{
			get
			{
				return base.state.fbData.loggedInUserFirstName;
			}
			set
			{
				base.state.fbData.loggedInUserFirstName = value;
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002F7C RID: 12156 RVA: 0x000DDFF0 File Offset: 0x000DC3F0
		// (set) Token: 0x06002F7D RID: 12157 RVA: 0x000DE002 File Offset: 0x000DC402
		public int LastSeenInviteFriends
		{
			get
			{
				return base.state.fbData.lastSeenInviteFriends;
			}
			set
			{
				base.state.fbData.lastSeenInviteFriends = value;
			}
		}

		// Token: 0x0400589D RID: 22685
		private Action _doSave;

		// Token: 0x0400589E RID: 22686
		private FBRequestCache _requestCache = new FBRequestCache();
	}
}
