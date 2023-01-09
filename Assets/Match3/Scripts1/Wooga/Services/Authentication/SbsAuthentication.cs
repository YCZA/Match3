using System;
using System.Collections.Generic;
using System.Net;
using Match3.Scripts1.Wooga.Core.Network;
using Wooga.Core.Storage;
using Wooga.Core.Utilities;
using Wooga.Coroutines;

namespace Match3.Scripts1.Wooga.Services.Authentication
{
	// Token: 0x020003BB RID: 955
	public class SbsAuthentication : ISbsAuthentication
	{
		// Token: 0x06001CC6 RID: 7366 RVA: 0x0007D5A6 File Offset: 0x0007B9A6
		public SbsAuthentication(ISbsNetworking networking, string sbsId, SbsCredentialsStorage credentialsStorage)
		{
			this._credentials = new SbsCredentials(sbsId);
			this._networking = networking;
			this._credentialsStorage = credentialsStorage;
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x0007D5C8 File Offset: 0x0007B9C8
		public SbsAuthentication(ISbsNetworking networking, string sbsId, ISbsStorage storage = null) : this(networking, sbsId, new SbsCredentialsStorage(new SbsSignedStorage(storage ?? Util.Storage(), sbsId)))
		{
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001CC8 RID: 7368 RVA: 0x0007D5EA File Offset: 0x0007B9EA
		// (set) Token: 0x06001CC9 RID: 7369 RVA: 0x0007D5F2 File Offset: 0x0007B9F2
		[Obsolete("Direct access to the credentials may be removed in a future update. Use GetUserContext() if you need access to the user data.")]
		public SbsCredentials Credentials
		{
			get
			{
				return this._credentials;
			}
			set
			{
				this._credentials = value;
			}
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x0007D5FB File Offset: 0x0007B9FB
		public virtual IEnumerator<bool> Authenticate(int timeout = 22)
		{
			if (this.TryLoadCredentialsFromDisk())
			{
				return true.Yield<bool>();
			}
			return this.TryFetchCredentialsFromNetwork(timeout, this.GetUserContext());
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x0007D61C File Offset: 0x0007BA1C
		public virtual void Logout()
		{
			this._credentialsStorage.ClearCredentials();
			this._credentials = new SbsCredentials();
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x0007D634 File Offset: 0x0007BA34
		public virtual void UpdateUserId(string userId)
		{
			this._credentials.user_id = userId;
			this._credentialsStorage.SaveCredentials(this._credentials);
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x0007D653 File Offset: 0x0007BA53
		public virtual bool IsAuthenticated()
		{
			return this._credentials.user_id != null;
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x0007D666 File Offset: 0x0007BA66
		public virtual UserContext GetUserContext()
		{
			return new UserContext(this._credentials.device_id, this._credentials.password, this._credentials.user_id);
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x0007D68E File Offset: 0x0007BA8E
		public virtual UserContext GetUserContextForUserId(string userId)
		{
			return new UserContext(this._credentials.device_id, this._credentials.password, userId);
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x0007D6AC File Offset: 0x0007BAAC
		private bool TryLoadCredentialsFromDisk()
		{
			bool result = false;
			SbsCredentials sbsCredentials = this._credentialsStorage.LoadCredentials();
			if (sbsCredentials != null)
			{
				this.ImportCredentials(sbsCredentials);
				result = true;
			}
			return result;
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x0007D6D7 File Offset: 0x0007BAD7
		private void ImportCredentials(SbsCredentials diskCredentials)
		{
			this._credentials.device_id = diskCredentials.device_id;
			this._credentials.user_id = diskCredentials.user_id;
			this._credentials.password = diskCredentials.password;
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x0007D70C File Offset: 0x0007BB0C
		private IEnumerator<bool> TryFetchCredentialsFromNetwork(int timeout, UserContext userContext)
		{
			SbsRequest sbsRequest = SbsRequestFactory.Authentication.CreateSignInRequest(userContext, timeout);
			return this._networking.Send(sbsRequest).ContinueWith(delegate(SbsResponse response)
			{
				bool result = false;
				if (SbsAuthentication.ResponseIsValid(response))
				{
					SbsCredentials diskCredentials = response.ParseBody<SbsCredentials>();
					this.ImportCredentials(diskCredentials);
					this._credentialsStorage.SaveCredentials(this._credentials);
					result = true;
				}
				return result;
			});
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x0007D73E File Offset: 0x0007BB3E
		private static bool ResponseIsValid(SbsResponse response)
		{
			return response != null && response.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response.BodyString) && !response.HasError();
		}

		// Token: 0x040049A7 RID: 18855
		public const int DEFAULT_TIMEOUT = 22;

		// Token: 0x040049A8 RID: 18856
		private SbsCredentials _credentials;

		// Token: 0x040049A9 RID: 18857
		private readonly ISbsNetworking _networking;

		// Token: 0x040049AA RID: 18858
		private readonly SbsCredentialsStorage _credentialsStorage;
	}
}
