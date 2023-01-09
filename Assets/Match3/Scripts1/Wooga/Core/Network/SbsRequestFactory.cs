using System.Collections.Generic;
using Wooga.Core.Utilities;
using Match3.Scripts1.Wooga.Leagues;
using Match3.Scripts1.Wooga.Services.Authentication;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Wooga.Core.Network
{
	// Token: 0x02000341 RID: 833
	public class SbsRequestFactory
	{
		// Token: 0x06001992 RID: 6546 RVA: 0x00073638 File Offset: 0x00071A38
		public static SbsRequest CreateDeleteSocialAssociationRequest(UserContext userContext, string etag, string identityProviderId, string oldSocialId)
		{
			return new SbsRequest
			{
				Method = HttpMethod.DELETE,
				Path = string.Format("/social/{0}/{1}", identityProviderId, oldSocialId),
				ETag = etag,
				UserContext = userContext,
				UseSignature = true
			};
		}

		// Token: 0x02000342 RID: 834
		public class ConfigService
		{
			// Token: 0x06001994 RID: 6548 RVA: 0x00073684 File Offset: 0x00071A84
			public static SbsRequest CreateFetchConfigRequest(string contentHash, string sbsGameId, string trackingId, string configVersion, string forceAbTests, int timeout = 15)
			{
				string text = (!string.IsNullOrEmpty(forceAbTests)) ? ("?ab_string=" + forceAbTests) : string.Empty;
				string path = string.Format("/config/v2/patched/{0}/{1}/{2}{3}", new object[]
				{
					sbsGameId,
					configVersion,
					trackingId,
					text
				});
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				if (!string.IsNullOrEmpty(contentHash))
				{
					Log.Info(new object[]
					{
						"fetch with hash: " + contentHash
					});
					dictionary.Add("x-sbs-content-hash", contentHash);
				}
				return new SbsRequest
				{
					Method = HttpMethod.GET,
					Path = path,
					TimeoutInSeconds = timeout,
					Headers = dictionary
				};
			}

			// Token: 0x06001995 RID: 6549 RVA: 0x00073730 File Offset: 0x00071B30
			public static SbsRequest CreateFetchAuthenticatedConfigRequest(UserContext userContext, string contentHash, string trackingId, string configVersion, string forceAbTests, string forcedPersonalizationString, int timeout = 15)
			{
				string path = "/config/v2/auth/patched";
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>
				{
					{
						"tracking_id",
						trackingId
					},
					{
						"version",
						configVersion
					}
				};
				if (!string.IsNullOrEmpty(forceAbTests))
				{
					dictionary2["ab_string"] = forceAbTests;
				}
				if (!string.IsNullOrEmpty(forcedPersonalizationString))
				{
					dictionary2["personalization_string"] = forcedPersonalizationString;
				}
				if (!string.IsNullOrEmpty(contentHash))
				{
					Log.Info(new object[]
					{
						"fetch with hash: " + contentHash
					});
					dictionary.Add("x-sbs-content-hash", contentHash);
				}
				return new SbsRequest
				{
					Method = HttpMethod.POST,
					Path = path,
					TimeoutInSeconds = timeout,
					Headers = dictionary,
					UseSignature = true,
					UserContext = userContext,
					Body = JSON.Serialize(dictionary2, false, 1, ' '),
					SendAsBytes = true
				};
			}
		}

		// Token: 0x020003BE RID: 958
		public class Authentication
		{
			// Token: 0x06001CEC RID: 7404 RVA: 0x00073828 File Offset: 0x00071C28
			public static SbsRequest CreateSignInRequest(UserContext userContext, int timeout = 60)
			{
				return new SbsRequest
				{
					Method = HttpMethod.POST,
					Path = "/devices",
					UseSignature = false,
					TimeoutInSeconds = timeout,
					UserContext = userContext
				};
			}

			// Token: 0x06001CED RID: 7405 RVA: 0x00073864 File Offset: 0x00071C64
			public static SbsRequest CreateNewUserRequest(UserContext userContext)
			{
				return new SbsRequest
				{
					Method = HttpMethod.POST,
					Path = "/users",
					UseSignature = true,
					UserContext = userContext,
					Body = "{}",
					SendAsBytes = true
				};
			}
		}

		// Token: 0x0200040B RID: 1035
		public class Identity
		{
			// Token: 0x06001EAC RID: 7852 RVA: 0x000738B4 File Offset: 0x00071CB4
			public static SbsRequest CreateCreateSocialAssociationSbsRequest(UserContext userContext, string userId, string accessToken, int timeout)
			{
				Dictionary<string, object> o = new Dictionary<string, object>
				{
					{
						"fb_token",
						accessToken
					}
				};
				return new SbsRequest
				{
					Method = HttpMethod.PUT,
					Path = string.Format("/social/facebook/{0}", userId),
					UserContext = userContext,
					UseSignature = true,
					TimeoutInSeconds = timeout,
					Body = JSON.Serialize(o, false, 1, ' ')
				};
			}

			// Token: 0x06001EAD RID: 7853 RVA: 0x0007391C File Offset: 0x00071D1C
			public static SbsRequest CreateUpdateSocialAssociationSbsRequest(UserContext userContext, string userId, List<string> userIds, int timeout)
			{
				Dictionary<string, object> o = new Dictionary<string, object>
				{
					{
						"user_ids",
						userIds
					}
				};
				return new SbsRequest
				{
					Method = HttpMethod.PUT,
					Path = string.Format("/social/facebook/{0}", userId),
					UserContext = userContext,
					UseSignature = true,
					TimeoutInSeconds = timeout,
					Body = JSON.Serialize(o, false, 1, ' ')
				};
			}
		}

		// Token: 0x02000417 RID: 1047
		public class KeyValueStore
		{
			// Token: 0x06001EE5 RID: 7909 RVA: 0x0007398C File Offset: 0x00071D8C
			public static SbsRequest CreateBucketReadRequest(UserContext userContext, string bucket, string sbsUserId, int timeout)
			{
				return new SbsRequest
				{
					Method = HttpMethod.GET,
					Path = string.Format("/{0}/{1}", bucket, sbsUserId),
					UseSignature = true,
					UserContext = userContext,
					TimeoutInSeconds = timeout
				};
			}

			// Token: 0x06001EE6 RID: 7910 RVA: 0x000739D0 File Offset: 0x00071DD0
			public static SbsRequest CreateBucketWriteRequest(UserContext userContext, string bucket, string data, int timeout)
			{
				return new SbsRequest
				{
					Method = HttpMethod.PUT,
					Path = string.Format("/{0}/{1}", bucket, userContext.user_id),
					UseSignature = true,
					Body = data,
					UserContext = userContext,
					TimeoutInSeconds = timeout
				};
			}

			// Token: 0x06001EE7 RID: 7911 RVA: 0x00073A20 File Offset: 0x00071E20
			public static SbsRequest CreateBucketsOfFacebookIdsReadRequest(UserContext userContext, string bucket, List<string> facebookIds)
			{
				Dictionary<string, object> o = new Dictionary<string, object>
				{
					{
						"fb_ids",
						(facebookIds != null) ? facebookIds : new List<string>()
					}
				};
				return new SbsRequest
				{
					Method = HttpMethod.POST,
					Path = string.Format("/{0}", bucket),
					UseSignature = true,
					UserContext = userContext,
					Body = JSON.Serialize(o, false, 1, ' '),
					SendAsBytes = true
				};
			}
		}

		// Token: 0x0200041D RID: 1053
		public class LeagueService
		{
			// Token: 0x06001EFC RID: 7932 RVA: 0x00073AA0 File Offset: 0x00071EA0
			public static SbsRequest CreateGetLeagueUserRequest(UserContext userContext, string leagueID)
			{
				return new SbsRequest
				{
					UseSignature = true,
					UserContext = userContext,
					Method = HttpMethod.GET,
					Path = string.Format("{0}/user", SbsRequestFactory.LeagueService.GetLeaguePath(leagueID))
				};
			}

			// Token: 0x06001EFD RID: 7933 RVA: 0x00073AE0 File Offset: 0x00071EE0
			public static SbsRequest CreateLeagueGetStandingsRequest(UserContext userContext, string leagueID)
			{
				return new SbsRequest
				{
					Method = HttpMethod.GET,
					Path = string.Format("{0}/standings", SbsRequestFactory.LeagueService.GetLeaguePath(leagueID)),
					UseSignature = true,
					UserContext = userContext
				};
			}

			// Token: 0x06001EFE RID: 7934 RVA: 0x00073B20 File Offset: 0x00071F20
			public static SbsRequest CreateLeagueRegisterRequest(UserContext userContext, string leagueID, PlayerInLeague leaguePlayerConfig)
			{
				return new SbsRequest
				{
					UseSignature = true,
					UserContext = userContext,
					Method = HttpMethod.POST,
					Path = string.Format("{0}/register", SbsRequestFactory.LeagueService.GetLeaguePath(leagueID)),
					Body = JSON.Serialize(leaguePlayerConfig, false, 1, ' '),
					SendAsBytes = true,
					IsForceReload = true
				};
			}

			// Token: 0x06001EFF RID: 7935 RVA: 0x00073B80 File Offset: 0x00071F80
			public static SbsRequest CreateLeaguePointsUpdateRequest(UserContext userContext, string leagueID, int currentPoints, int previousPoints)
			{
				Dictionary<string, object> o = new Dictionary<string, object>
				{
					{
						"points",
						currentPoints
					},
					{
						"previous_points",
						previousPoints
					}
				};
				return new SbsRequest
				{
					UseSignature = true,
					UserContext = userContext,
					Method = HttpMethod.POST,
					Path = string.Format("{0}/points", SbsRequestFactory.LeagueService.GetLeaguePath(leagueID)),
					SendAsBytes = true,
					Body = JSON.Serialize(o, false, 1, ' ')
				};
			}

			// Token: 0x06001F00 RID: 7936 RVA: 0x00073C00 File Offset: 0x00072000
			private static string GetLeaguePath(string leagueID)
			{
				return string.Format("/leagues/v1/league/{0}", leagueID);
			}
		}

		// Token: 0x0200042A RID: 1066
		public class PaymentValidation
		{
			// Token: 0x06001F52 RID: 8018 RVA: 0x00073C18 File Offset: 0x00072018
			public static SbsRequest CreatePaymentValidateRequest(UserContext userContext, Dictionary<string, object> data, object store)
			{
				return new SbsRequest
				{
					UseSignature = true,
					UserContext = userContext,
					Method = HttpMethod.POST,
					Path = string.Format("/payments/{0}", store),
					SendAsBytes = true,
					Body = JSON.Serialize(data, false, 1, ' ')
				};
			}
		}
	}
}
