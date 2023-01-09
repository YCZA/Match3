using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Match3.Scripts1.Wooga.Core.Network;
using Wooga.Core.Utilities;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.Authentication;

namespace Match3.Scripts1.Wooga.Services.Identity
{
	// Token: 0x02000408 RID: 1032
	public class FacebookIdentityService
	{
		// Token: 0x06001E9D RID: 7837 RVA: 0x00081BE7 File Offset: 0x0007FFE7
		public FacebookIdentityService(ISbsNetworking networking) : this(networking, SBS.Authentication)
		{
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x00081BF5 File Offset: 0x0007FFF5
		public FacebookIdentityService(ISbsNetworking networking, SbsAuthentication authentication)
		{
			this._networking = networking;
			this._authentication = authentication;
			this._etagCache = new Dictionary<string, string>();
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x00081C18 File Offset: 0x00080018
		public IEnumerator<IdentityResult> AssociateWithFacebook(string facebookUserId, string accessToken, Func<string, string, IEnumerator<string>> mergeHandler, int timeout = 20)
		{
			SbsRequest request = SbsRequestFactory.Identity.CreateCreateSocialAssociationSbsRequest(this._authentication.GetUserContext(), facebookUserId, accessToken, timeout);
			return this._networking.Send(request).ContinueWith(delegate(SbsResponse response)
			{
				this.CacheEtag(response, request);
				if (this.IsPossibleMergeConflict(response))
				{
					IdentityUserIdsResponse result = response.ParseBody<IdentityUserIdsResponse>();
					if (this.IsMergeConflict(result))
					{
						return this.MergeUser(facebookUserId, result, mergeHandler, timeout);
					}
					return IdentityResult.AssociationSuccess.Yield<IdentityResult>();
				}
				else
				{
					if (response.StatusCode == HttpStatusCode.NoContent)
					{
						return IdentityResult.AssociationSuccess.Yield<IdentityResult>();
					}
					return IdentityResult.AssociationError.Yield<IdentityResult>();
				}
			});
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x00081C8D File Offset: 0x0008008D
		public void ClearEtagCache()
		{
			this._etagCache.Clear();
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x00081C9C File Offset: 0x0008009C
		private IEnumerator<IdentityResult> MergeUser(string facebookUserId, IdentityUserIdsResponse result, Func<string, string, IEnumerator<string>> mergeHandler, int timeout)
		{
			string user_id = this._authentication.GetUserContext().user_id;
			string arg = result.UserIds[0];
			return mergeHandler(user_id, arg).ContinueWith(delegate(string chosenUserId)
			{
				List<string> userIds = this.MergeUserIds(chosenUserId, result.UserIds);
				return this.UpdateUserIds(facebookUserId, userIds, timeout).ContinueWith(delegate(IdentityResult updateResponse)
				{
					if (updateResponse == IdentityResult.MergeSuccess)
					{
						UpdateLocalSbsUserId(chosenUserId);
					}
					return updateResponse;
				});
			});
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x00081D04 File Offset: 0x00080104
		private IEnumerator<IdentityResult> UpdateUserIds(string facebookUserId, List<string> userIds, int timeout)
		{
			SbsRequest sbsRequest = SbsRequestFactory.Identity.CreateUpdateSocialAssociationSbsRequest(this._authentication.GetUserContext(), facebookUserId, userIds, timeout);
			if (this._etagCache.ContainsKey(sbsRequest.Path))
			{
				sbsRequest.ETag = this._etagCache[sbsRequest.Path];
			}
			return this._networking.Send(sbsRequest).ContinueWith(delegate(SbsResponse response)
			{
				if (response.StatusCode == HttpStatusCode.NoContent)
				{
					return IdentityResult.MergeSuccess.Yield<IdentityResult>();
				}
				return IdentityResult.MergeError.Yield<IdentityResult>();
			});
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x00081D80 File Offset: 0x00080180
		private void CacheEtag(SbsResponse response, SbsRequest request)
		{
			if (this.ResponseIsValid(response))
			{
				this._etagCache[request.Path] = response.ETag;
			}
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x00081DA5 File Offset: 0x000801A5
		private bool IsPossibleMergeConflict(SbsResponse response)
		{
			return response.StatusCode == HttpStatusCode.PreconditionFailed || response.StatusCode == (HttpStatusCode)428;
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x00081DC8 File Offset: 0x000801C8
		private bool IsMergeConflict(IdentityUserIdsResponse result)
		{
			string user_id = this._authentication.GetUserContext().user_id;
			string b = result.UserIds[0];
			return user_id != b;
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x00081DF8 File Offset: 0x000801F8
		private List<string> MergeUserIds(string chosenUserId, string[] associatedUsers)
		{
			List<string> list = new List<string>
			{
				chosenUserId
			};
			foreach (string item in associatedUsers)
			{
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
			string user_id = this._authentication.GetUserContext().user_id;
			if (!list.Contains(user_id))
			{
				list.Add(user_id);
			}
			return list;
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x00081E70 File Offset: 0x00080270
		private void UpdateLocalSbsUserId(string userId)
		{
			if (userId != this._authentication.GetUserContext().user_id)
			{
				Log.Info(new object[]
				{
					"Change to new SBS user: " + userId
				});
				this._authentication.UpdateUserId(userId);
			}
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x00081EBD File Offset: 0x000802BD
		private bool ResponseIsValid(SbsResponse response)
		{
			return response != null && !response.Exceptions.Any<Exception>();
		}

		// Token: 0x04004A7D RID: 19069
		private readonly ISbsNetworking _networking;

		// Token: 0x04004A7E RID: 19070
		private readonly SbsAuthentication _authentication;

		// Token: 0x04004A7F RID: 19071
		private readonly Dictionary<string, string> _etagCache;
	}
}
