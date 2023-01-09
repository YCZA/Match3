using System;
using System.Collections.Generic;
using System.Net;
using Match3.Scripts1.Wooga.Core.Network;
using Wooga.Core.Utilities;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.Authentication;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Wooga.Leagues
{
	// Token: 0x02000419 RID: 1049
	public class LeagueService
	{
		// Token: 0x06001EEA RID: 7914 RVA: 0x00082B17 File Offset: 0x00080F17
		public LeagueService(ISbsNetworking networking, SbsAuthentication authentication)
		{
			this.networking = networking;
			this.authentication = authentication;
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x00082B30 File Offset: 0x00080F30
		public IEnumerator<StandingsQueryResponse> RegisterToLeague(string leagueID, PlayerInLeague playerConfig)
		{
			SbsRequest sbsRequest = SbsRequestFactory.LeagueService.CreateLeagueRegisterRequest(this.authentication.GetUserContext(), leagueID, playerConfig);
			return this.networking.Send(sbsRequest).ContinueWith(delegate(SbsResponse response)
			{
				StandingsQueryResponse standingsQueryResponse = StandingsQueryResponse.Failure(true);
				if (response.StatusCode == HttpStatusCode.Created)
				{
					standingsQueryResponse = response.ParseBody<StandingsQueryResponse>();
					standingsQueryResponse.couldFetchFromServer = true;
					standingsQueryResponse.playerIsMemberOfLeague = true;
					standingsQueryResponse.playerHadAlreadyRegisteredBefore = false;
					return standingsQueryResponse;
				}
				bool flag = response.StatusCode == HttpStatusCode.Conflict;
				standingsQueryResponse.playerIsMemberOfLeague = flag;
				standingsQueryResponse.playerHadAlreadyRegisteredBefore = flag;
				return standingsQueryResponse;
			}).Catch((Exception exception) => StandingsQueryResponse.Failure(false));
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x00082BA0 File Offset: 0x00080FA0
		public IEnumerator<UserQueryResponse> FetchUserData(string leagueID)
		{
			UserContext userContext = this.authentication.GetUserContext();
			SbsRequest sbsRequest = SbsRequestFactory.LeagueService.CreateGetLeagueUserRequest(userContext, leagueID);
			return this.networking.Send(sbsRequest).ContinueWith(delegate(SbsResponse response)
			{
				UserQueryResponse userQueryResponse = UserQueryResponse.Failure();
				if (response.StatusCode == HttpStatusCode.OK)
				{
					userQueryResponse.userInfo = response.ParseBody<LeagueEntry>();
					userQueryResponse.success = true;
				}
				else
				{
					Log.Debug(new object[]
					{
						"User query: " + response.StatusCode
					});
				}
				return userQueryResponse;
			}).Catch((Exception exception) => UserQueryResponse.Failure());
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x00082C14 File Offset: 0x00081014
		public IEnumerator<StandingsQueryResponse> GetLeagueStandings(string leagueID)
		{
			SbsRequest sbsRequest = SbsRequestFactory.LeagueService.CreateLeagueGetStandingsRequest(this.authentication.GetUserContext(), leagueID);
			return this.networking.Send(sbsRequest).ContinueWith(delegate(SbsResponse response)
			{
				StandingsQueryResponse standingsQueryResponse = StandingsQueryResponse.Failure(true);
				if (response.StatusCode == HttpStatusCode.OK)
				{
					standingsQueryResponse = response.ParseBody<StandingsQueryResponse>();
					standingsQueryResponse.couldFetchFromServer = true;
					standingsQueryResponse.playerIsMemberOfLeague = true;
				}
				else
				{
					standingsQueryResponse.playerIsMemberOfLeague = false;
				}
				return standingsQueryResponse;
			}).Catch((Exception exception) => StandingsQueryResponse.Failure(false));
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x00082C84 File Offset: 0x00081084
		public IEnumerator<PointsUpdateResponse> UpdatePoints(string leagueID, int currentPoints, int previousPoints)
		{
			SbsRequest sbsRequest = SbsRequestFactory.LeagueService.CreateLeaguePointsUpdateRequest(this.authentication.GetUserContext(), leagueID, currentPoints, previousPoints);
			return this.networking.Send(sbsRequest).ContinueWith(delegate(SbsResponse response)
			{
				PointsUpdateResponse result = PointsUpdateResponse.Failure(currentPoints);
				if (response.StatusCode == HttpStatusCode.NoContent)
				{
					result = PointsUpdateResponse.Success(currentPoints);
				}
				else if (response.StatusCode == HttpStatusCode.PreconditionFailed)
				{
					IDictionary<string, JSONNode> dictionary = response.ParseBody();
					int currentPoints2 = (int)dictionary["points"];
					result = PointsUpdateResponse.Success(currentPoints2);
				}
				else if (response.StatusCode == HttpStatusCode.NotFound)
				{
					result = PointsUpdateResponse.UserNotMember();
				}
				return result;
			}).Catch((Exception exception) => PointsUpdateResponse.Failure(currentPoints));
		}

		// Token: 0x04004A9F RID: 19103
		private readonly ISbsNetworking networking;

		// Token: 0x04004AA0 RID: 19104
		private readonly SbsAuthentication authentication;
	}
}
