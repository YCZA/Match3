using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Build;
using Match3.Scripts1.Wooga.Core.Network;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services;
using Match3.Scripts1.Wooga.Services.Authentication;
using Match3.Scripts1.Wooga.Services.ConfigServiceV2;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Match3.Scripts1.Wooga.Services.Tracking;
using Wooga.UnityFramework;
using Match3.Scripts2.Env;
using Match3.Scripts2.PlayerData;
using UnityEngine; //using Facebook.Unity;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007F8 RID: 2040
	public class SBSService : AService
	{
		// Token: 0x06003268 RID: 12904 RVA: 0x000ED063 File Offset: 0x000EB463
		public SBSService()
		{
			if (Application.isPlaying)
			{
				WooroutineRunner.StartCoroutine(this.LoadRoutine(), null);
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06003269 RID: 12905 RVA: 0x000ED082 File Offset: 0x000EB482
		// (set) Token: 0x0600326A RID: 12906 RVA: 0x000ED08A File Offset: 0x000EB48A
		public bool IsAuthenticated { get; private set; }

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x0600326B RID: 12907 RVA: 0x000ED093 File Offset: 0x000EB493
		// (set) Token: 0x0600326C RID: 12908 RVA: 0x000ED09B File Offset: 0x000EB49B
		public PTConfig SbsConfig { get; private set; }

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x0600326D RID: 12909 RVA: 0x000ED0A4 File Offset: 0x000EB4A4
		public ISbsConfigService ConfigService
		{
			get
			{
				return this.configService;
			}
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x000ED0AC File Offset: 0x000EB4AC
		//private SbsRequest CreateAssociatedFBRequest(AccessToken token, string eTag)
		//{
		//	return SbsRequestFactory.Identity.CreateCreateSocialAssociationSbsRequest(SBS.Authentication.GetUserContext(), token.UserId, token.TokenString, 5);
		//}

		// Token: 0x0600326F RID: 12911 RVA: 0x000ED0D8 File Offset: 0x000EB4D8
		public bool CheckForForcedAbTests(string abTests)
		{
			DebugSettings debugSettings = APlayerPrefsObject<DebugSettings>.Load();
			if (debugSettings.forcedAbTests != abTests)
			{
				debugSettings.forcedAbTests = abTests;
				debugSettings.Save();
				PTReloader.ReloadGame("Forcing ab tests", false);
				return true;
			}
			return false;
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x000ED118 File Offset: 0x000EB518
		//public IEnumerator DoAssociateWithFB(AccessToken token)
		//{
		//	SBSService.AssocWithFBResult assocResult = default(SBSService.AssocWithFBResult);
		//	SbsRequest fbAssocReq = this.CreateAssociatedFBRequest(token, null);
		//	Wooroutine<SbsResponse> responseRoutine = this.serviceRunner.WaitForSBSRequest(fbAssocReq);
		//	yield return responseRoutine;
		//	SbsResponse response = responseRoutine.ReturnValue;
		//	if (response != null && (response.StatusCode == HttpStatusCode.PreconditionFailed || response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == (HttpStatusCode)428))
		//	{
		//		SBSService.FBAssocResponse fbassocResponse = response.ParseBody<SBSService.FBAssocResponse>();
		//		List<string> user_ids = fbassocResponse.user_ids;
		//		if (user_ids.Count > 0)
		//		{
		//			string a = user_ids[0];
		//			if (a != SBS.Authentication.GetUserContext().user_id)
		//			{
		//				WoogaDebug.Log(new object[]
		//				{
		//					"Change to new SBS user: ",
		//					user_ids[0]
		//				});
		//				SBS.Authentication.UpdateUserId(user_ids[0]);
		//				assocResult.SBSUserChanged = true;
		//			}
		//		}
		//	}
		//	yield return assocResult;
		//	yield break;
		//}

		//// Token: 0x06003271 RID: 12913 RVA: 0x000ED13C File Offset: 0x000EB53C
		//public IEnumerator doDissocWithFB(AccessToken token)
		//{
		//	SbsRequest fbDissocReq = SbsRequestFactory.CreateDeleteSocialAssociationRequest(SBS.Authentication.GetUserContext(), string.Empty, "facebook", token.UserId);
		//	Wooroutine<SbsResponse> responseRoutine = this.serviceRunner.WaitForSBSRequest(fbDissocReq);
		//	yield return responseRoutine;
		//	yield break;
		//}

		// Token: 0x06003272 RID: 12914 RVA: 0x000ED160 File Offset: 0x000EB560
		public IEnumerator doRetrieveFriendsRanks(IEnumerable<string> friendIDs)
		{
			Dictionary<string, GameStateService.villageRankBucketData> friendsRanks = new Dictionary<string, GameStateService.villageRankBucketData>();
			if (friendIDs.IsNullOrEmptyEnumerable())
			{
				yield return friendsRanks;
				yield break;
			}
			SbsRequest fbFriendsReq = SbsRequestFactory.KeyValueStore.CreateBucketsOfFacebookIdsReadRequest(SBS.Authentication.GetUserContext(), "rankstate", friendIDs.ToList<string>());
			fbFriendsReq.ETag = "force_fresh";
			Wooroutine<SbsResponse> responseRoutine = this.serviceRunner.WaitForSBSRequest(fbFriendsReq);
			yield return responseRoutine;
			SbsResponse response = responseRoutine.ReturnValue;
			if (response != null)
			{
				SBSService.FriendStateWrapperArray friendStateWrapperArray = JsonUtility.FromJson<SBSService.FriendStateWrapperArray>(response.BodyString);
				if (friendStateWrapperArray != null && friendStateWrapperArray.rankstate != null)
				{
					foreach (SBSService.VillageStateWrapper villageStateWrapper in friendStateWrapperArray.rankstate)
					{
						villageStateWrapper.rankstat.data.sbsId = villageStateWrapper.user_id;
						if (villageStateWrapper.rankstat != null)
						{
							friendsRanks[villageStateWrapper.fb_id] = villageStateWrapper.rankstat.data;
						}
						else
						{
							friendsRanks[villageStateWrapper.fb_id] = default(GameStateService.villageRankBucketData);
						}
					}
				}
			}
			yield return friendsRanks;
			yield break;
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x000ED184 File Offset: 0x000EB584
		public IEnumerator doRetrieveFriendGameState(string friendID)
		{
			SbsRequest fbFriendsReq = SbsRequestFactory.KeyValueStore.CreateBucketsOfFacebookIdsReadRequest(SBS.Authentication.GetUserContext(), "gamestate", new List<string>
			{
				friendID
			});
			fbFriendsReq.ETag = "force_fresh";
			Wooroutine<SbsResponse> responseRoutine = this.serviceRunner.WaitForSBSRequest(fbFriendsReq);
			yield return responseRoutine;
			SBSService.VillageStateWrapper retValue = null;
			try
			{
				SBSService.FriendStateWrapperArray friendStateWrapperArray = JsonUtility.FromJson<SBSService.FriendStateWrapperArray>(responseRoutine.ReturnValue.BodyString);
				if (friendStateWrapperArray.gamestate != null && friendStateWrapperArray.gamestate.Length != 0 && friendStateWrapperArray.gamestate[0].gamestat != null)
				{
					retValue = friendStateWrapperArray.gamestate[0];
				}
			}
			catch (Exception)
			{
			}
			yield return retValue;
			yield break;
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x000ED1A6 File Offset: 0x000EB5A6
		public Coroutine RetrieveFriendsRanks(IEnumerable<string> friendIDs)
		{
			return WooroutineRunner.StartCoroutine(this.doRetrieveFriendsRanks(friendIDs), null);
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x000ED1B5 File Offset: 0x000EB5B5
		//public Coroutine AssocWithFB(AccessToken token)
		//{
		//	return WooroutineRunner.StartCoroutine(this.DoAssociateWithFB(token), null);
		//}

		//// Token: 0x06003276 RID: 12918 RVA: 0x000ED1C4 File Offset: 0x000EB5C4
		//public Coroutine DissocWithFB(AccessToken token)
		//{
		//	return WooroutineRunner.StartCoroutine(this.doDissocWithFB(token), null);
		//}

		// Token: 0x06003277 RID: 12919 RVA: 0x000ED1D4 File Offset: 0x000EB5D4
		private IEnumerator LoadRoutine()
		{
			SBS.Init(GameEnvironment.CurrentId);
			IEnumerator<bool> authentication = SBS.Authentication.Authenticate(5).Catch(delegate(Exception exception)
			{
				WoogaDebug.LogWarning(new object[]
				{
					exception.Message
				});
				return false;
			});
			float authStartTime = Time.realtimeSinceStartup;
			yield return authentication;
			float authElapsedTime = Time.realtimeSinceStartup - authStartTime;
			if (!authentication.Current)
			{
				string text = (SBS.Authentication.GetUserContext().user_id == null) ? "NULL" : SBS.Authentication.GetUserContext().user_id.Substring(0, 4);
				WoogaDebug.LogWarning(new object[]
				{
					string.Concat(new object[]
					{
						"AuthFail: ",
						GameEnvironment.CurrentEnvShortCode,
						": ",
						text,
						":",
						authElapsedTime
					})
				});
			}
			else
			{
				this.IsAuthenticated = true;
			}
			UserContext user = SBS.Authentication.GetUserContext();
			ErrorAnalytics.UpdateSbsInfo(user.device_id, user.user_id);
			this.serviceRunner = new SBSServiceRunner(SBS.Networking);
			yield return this.SetupConfigServiceRoutine();
			yield break;
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x000ED1F0 File Offset: 0x000EB5F0
		private IEnumerator SetupConfigServiceRoutine()
		{
			this.configService = new SbsConfigService<PTConfig>(SBS.Networking, GameEnvironment.CurrentId, Tracking.Id, BuildVersion.SbsConfigVersion, GameEnvironment.CurrentPublicKey);
			DebugSettings settings = APlayerPrefsObject<DebugSettings>.Load();
			this.configService.ForceAbTests(settings.forcedAbTests);
			this.SetSbsConfig();
			Wooroutine<SbsConfigService.Result> fetch = this.configService.FetchAuthenticated(this.SbsConfig.sbs_timeouts.default_request).StartWooroutine<SbsConfigService.Result>();
			float time = 0f;
			while (time <= 3f && fetch.keepWaiting)
			{
				time += Time.deltaTime;
				yield return null;
			}
			if (fetch.Completed && fetch.ReturnValue.result == FetchResult.Success)
			{
				this.SetSbsConfig();
			}
			base.OnInitialized.Dispatch();
			yield return fetch;
			yield return fetch.ReturnValue;
			yield break;
		}

		// Token: 0x06003279 RID: 12921 RVA: 0x000ED20C File Offset: 0x000EB60C
		protected void SetSbsConfig()
		{
			string latestJson = this.configService.GetLatestJson();
			this.SbsConfig = JsonUtility.FromJson<PTConfig>(latestJson);
			this.SbsConfig.Init();
		}

		// Token: 0x04005AF4 RID: 23284
		public const string KEY_USE_BUNDLED_SBS_CONFIG = "UseOnlyBundledSbsConfig";

		// Token: 0x04005AF5 RID: 23285
		public SBSServiceRunner serviceRunner;

		// Token: 0x04005AF6 RID: 23286
		protected SbsConfigService<PTConfig> configService;

		// Token: 0x04005AF7 RID: 23287
		private ISbsNetworking sbsNetwork;

		// Token: 0x020007F9 RID: 2041
		public struct AssocWithFBResult
		{
			// Token: 0x04005AFA RID: 23290
			public bool SBSUserChanged;
		}

		// Token: 0x020007FA RID: 2042
		[Serializable]
		public class VillageRankBucketWrapper
		{
			// Token: 0x04005AFB RID: 23291
			public GameStateService.villageRankBucketData data;
		}

		// Token: 0x020007FB RID: 2043
		[Serializable]
		public class VillageStateWrapper
		{
			// Token: 0x04005AFC RID: 23292
			public string user_id = string.Empty;

			// Token: 0x04005AFD RID: 23293
			public SBSService.VillageRankBucketWrapper rankstat;

			// Token: 0x04005AFE RID: 23294
			public GameStateService.GameStateData gamestat;

			// Token: 0x04005AFF RID: 23295
			public string fb_id = string.Empty;
		}

		// Token: 0x020007FC RID: 2044
		[Serializable]
		private class FriendStateWrapperArray
		{
			// Token: 0x04005B00 RID: 23296
			public SBSService.VillageStateWrapper[] rankstate;

			// Token: 0x04005B01 RID: 23297
			public SBSService.VillageStateWrapper[] gamestate;
		}

		// Token: 0x020007FD RID: 2045
		[Serializable]
		private class FBAssocResponse
		{
			// Token: 0x04005B02 RID: 23298
			public List<string> user_ids = new List<string>();
		}
	}
}
