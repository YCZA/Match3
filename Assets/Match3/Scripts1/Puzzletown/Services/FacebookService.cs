using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine; //using Facebook.Unity;
//using Facebook.Unity.Settings;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000795 RID: 1941
	public class FacebookService : AService
	{
		// Token: 0x06002F91 RID: 12177 RVA: 0x000DF268 File Offset: 0x000DD668
		public FacebookService()
		{
			WooroutineRunner.StartCoroutine(this.LoadRoutine(), null);
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06002F92 RID: 12178 RVA: 0x000DF2CC File Offset: 0x000DD6CC
		// (set) Token: 0x06002F93 RID: 12179 RVA: 0x000DF2D4 File Offset: 0x000DD6D4
		public FacebookService.Status CurrentStatus
		{
			get
			{
				return this._currentStatus;
			}
			private set
			{
				if (value == this._currentStatus)
				{
					return;
				}
				this._currentStatus = value;
				this.OnLoginStatusChanged.Dispatch(this.CurrentStatus);
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002F94 RID: 12180 RVA: 0x000DF2FC File Offset: 0x000DD6FC
		public string FB_GAME_ID
		{
			get
			{
				//int index = (SbsEnvironment.CurrentEnvironment != SbsEnvironment.Environment.PRODUCTION) ? 0 : 1;
				//return FacebookSettings.AppIds[index];
				return "";
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06002F95 RID: 12181 RVA: 0x000DF328 File Offset: 0x000DD728
		public string FB_MY_ID
		{
			get
			{
				//return (AccessToken.CurrentAccessToken == null) ? string.Empty : AccessToken.CurrentAccessToken.UserId;
				return "";
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002F96 RID: 12182 RVA: 0x000DF358 File Offset: 0x000DD758
		public FacebookData.Friend Me
		{
			get
			{
				//if (this._me == null && FB.IsLoggedIn)
				//{
				//	this._me = new FacebookData.Friend();
				//	this._me.ID = this.FB_MY_ID;
				//	this._me.Name = "You";
				//}
				return this._me;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002F97 RID: 12183 RVA: 0x000DF3AC File Offset: 0x000DD7AC
		// (set) Token: 0x06002F98 RID: 12184 RVA: 0x000DF3B4 File Offset: 0x000DD7B4
		public PTFacebookLivesHandler Friends { get; private set; }

		// Token: 0x06002F99 RID: 12185 RVA: 0x000DF3C0 File Offset: 0x000DD7C0
		private IEnumerator LoadRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			//FB.Init(this.FB_GAME_ID, null, true, true, true, false, true, null, "en_US", null, null);
			//while (!FB.IsInitialized)
			//{
			//	yield return null;
			//}
			//FB.ActivateApp();
			//this.apiRunner = new FacebookAPIRunner(this.gameState.Facebook);
			//this.apiRunner.OnRequestSent.AddListener(new Action<FacebookRequest, IAppRequestResult>(this.OnRequestSent.Dispatch));
			//this.apiRunner.Start();
			//this.imageRunner = new FacebookImageRunner();
			//this.imageRunner.Start();
			// Debug.Log("Load routine" + this.gameConfig);
			// Debug.Log("Load routine" + this.gameConfig.general);
			// Debug.Log("Load routine" + this.gameConfig.general.lives);
			// Debug.Log("Load routine" + this.gameConfig.general.lives.request_from_friend_cooldown);
			// Debug.Log("Load routine" + apiRunner);
			// Debug.Log("Load routine" + gameState);
			// Debug.Log("Load routine" + gameState.Facebook);
			// this.Friends = new PTFacebookLivesHandler(this.gameConfig.general.lives.request_from_friend_cooldown, this.apiRunner, this.gameState.Facebook);
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x000DF3DB File Offset: 0x000DD7DB
		public bool LoggedIn()
		{
			//return FB.IsLoggedIn;
			return false;
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x000DF3E2 File Offset: 0x000DD7E2
		public bool RecievedLoginReward()
		{
			return this.gameState.Facebook.receivedLoginReward;
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x000DF3F4 File Offset: 0x000DD7F4
		public void Logout()
		{
			//if (!this.LoggedIn())
			//{
			//	return;
			//}
			//this.sbsService.DissocWithFB(AccessToken.CurrentAccessToken);
			//FB.LogOut();
			//this.CurrentStatus = FacebookService.Status.LoggedOut;
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x000DF420 File Offset: 0x000DD820
		public IEnumerator DoLogin()
		{
			//ILoginResult fbLoginResult = null;
			//FB.LogInWithReadPermissions(new List<string>
			//{
			//	"user_friends",
			//	"email"
			//}, delegate(ILoginResult result)
			//{
			//	fbLoginResult = result;
			//});
			//while (fbLoginResult == null)
			//{
			//	yield return null;
			//}
			//FacebookService.FBLoginResult loginResult = default(FacebookService.FBLoginResult);
			//if (FB.IsLoggedIn)
			//{
			//	this.CurrentStatus = FacebookService.Status.LoggedIn;
			//	AccessToken accessToken = AccessToken.CurrentAccessToken;
			//	Wooroutine<SBSService.AssocWithFBResult> assocRoutine = WooroutineRunner.StartWooroutine<SBSService.AssocWithFBResult>(this.sbsService.DoAssociateWithFB(accessToken));
			//	yield return assocRoutine;
			//	loginResult.sbsUserChanged = assocRoutine.ReturnValue.SBSUserChanged;
			//	yield return this.GetFriends(FacebookData.Friend.Type.Playing);
			//	this.lastFetch = DateTime.MinValue;
			//	yield return this.GetUserFirstName();
			//}
			//yield return loginResult;
			yield break;
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x000DF43C File Offset: 0x000DD83C
		public IEnumerator FetchRequests()
		{
			double secondsSinceLastFetch = (DateTime.Now - this.lastFetch).TotalSeconds;
			this.DeleteInvalidRequests();
			if (secondsSinceLastFetch < (double)this.gameConfig.general.lives.refresh_cooldown && this.cachedRequests != null)
			{
				List<FacebookData.Request> filteredRequests = new List<FacebookData.Request>();
				foreach (FacebookData.Request request in this.cachedRequests)
				{
					if (!request.deleted)
					{
						filteredRequests.Add(request);
					}
				}
				filteredRequests = this.DeleteDuplicateRequests(filteredRequests);
				this.cachedRequests = filteredRequests;
				yield return this.cachedRequests;
			}
			else
			{
				List<FacebookData.Request> requests = new List<FacebookData.Request>();
				yield return this.apiRunner.PaginatedGETRequest("me/apprequests?fields=data,from.fields(first_name),to.fields(first_name)", delegate(string apiData)
				{
					requests.AddRange(FacebookData.GetRequests(apiData));
					return true;
				});
				Dictionary<string, FacebookData.Request> allRequests = new Dictionary<string, FacebookData.Request>();
				foreach (FacebookData.Request request2 in requests)
				{
					if (!request2.deleted)
					{
						allRequests[request2.ID] = request2;
					}
				}
				foreach (FacebookData.Request request3 in this.gameState.Facebook.Data.Requests)
				{
					if (!request3.deleted)
					{
						allRequests[request3.ID] = request3;
					}
				}
				this.cachedRequests = allRequests.Values.ToList<FacebookData.Request>();
				this.gameState.Facebook.Data.Requests = this.cachedRequests;
				this.gameState.Save(false);
				this.lastFetch = DateTime.Now;
				this.gameState.Facebook.Data.Requests = this.DeleteDuplicateRequests(this.gameState.Facebook.Data.Requests);
				yield return this.gameState.Facebook.Data.Requests;
			}
			yield break;
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x000DF458 File Offset: 0x000DD858
		private void DeleteInvalidRequests()
		{
			if (FacebookData.invalidRequests != null)
			{
				foreach (FacebookData.AppRequestsResponse.Request request in FacebookData.invalidRequests)
				{
					this.gameState.Facebook.PendingOps.Enqueue(FacebookDeleteRequest.Create(request.id));
				}
				FacebookData.invalidRequests.Clear();
			}
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x000DF4E0 File Offset: 0x000DD8E0
		private List<FacebookData.Request> DeleteDuplicateRequests(List<FacebookData.Request> requests)
		{
			List<FacebookData.Request> list = new List<FacebookData.Request>();
			List<FacebookData.Request> list2 = new List<FacebookData.Request>();
			foreach (FacebookData.Request request in requests)
			{
				bool flag = false;
				foreach (FacebookData.Request request2 in list)
				{
					if (request.fromID == request2.fromID && request.type == request2.type)
					{
						flag = true;
						list2.Add(request2);
					}
				}
				if (!flag)
				{
					list.Add(request);
				}
			}
			foreach (FacebookData.Request request3 in list2)
			{
				request3.deleted = true;
				this.gameState.Facebook.PendingOps.Enqueue(FacebookDeleteRequest.Create(request3.ID));
			}
			return list;
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x000DF634 File Offset: 0x000DDA34
		public void DeleteRequest(string requestID)
		{
			foreach (FacebookData.Request request in this.gameState.Facebook.Data.Requests)
			{
				if (request.ID == requestID)
				{
					this.apiRunner.DeleteRequest(this.gameState.Facebook, request);
					request.deleted = true;
					WoogaDebug.Log(new object[]
					{
						"Setting Deleted: " + requestID
					});
				}
			}
			this.gameState.Save(false);
		}

		// Token: 0x06002FA2 RID: 12194 RVA: 0x000DF6EC File Offset: 0x000DDAEC
		public GameStateService.villageRankBucketData FriendProgress(string friendID)
		{
			if (this._friendsProgress.ContainsKey(friendID))
			{
				return this._friendsProgress[friendID];
			}
			return default(GameStateService.villageRankBucketData);
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x000DF720 File Offset: 0x000DDB20
		public bool HasCachedFriends(FacebookData.Friend.Type type)
		{
			return this.cachedFriendListData[(int)type] != null;
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x000DF730 File Offset: 0x000DDB30
		public void AddFriends(MultiFriendsSelectorRoot.FriendSelectorType selectorType)
		{
			FacebookRequest fbRequest = new FacebookRequest();
			//FB.AppRequest(this.localizationService.GetText("ui.social.facebook.app_request", new LocaParam[0]), null, null, null, null, null, null, delegate(IAppRequestResult result)
			//{
			//	fbRequest.trackingType = "invite";
			//	fbRequest.context1 = selectorType.ToString();
			//	if (result != null && result.To != null)
			//	{
			//		fbRequest.recipients = new List<string>(result.To);
			//		this.trackingService.TrackRequest(fbRequest, result);
			//	}
			//});
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06002FA5 RID: 12197 RVA: 0x000DF791 File Offset: 0x000DDB91
		public int FriendsPlayingCount
		{
			get
			{
				return (this.friendMap == null) ? 0 : this.friendMap.Count;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06002FA6 RID: 12198 RVA: 0x000DF7AF File Offset: 0x000DDBAF
		public int FriendsTotalCount
		{
			get
			{
				return (this.cachedFriendListData[1] == null) ? 0 : this.cachedFriendListData[1].totalCount;
			}
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x000DF7D1 File Offset: 0x000DDBD1
		public Wooroutine<FacebookAPIRunner.FriendListData> GetFriends(FacebookData.Friend.Type type)
		{
			if (type == FacebookData.Friend.Type.All)
			{
				return WooroutineRunner.StartWooroutine<FacebookAPIRunner.FriendListData>(this.GetAllFriendsRoutine());
			}
			return WooroutineRunner.StartWooroutine<FacebookAPIRunner.FriendListData>(this.GetFriendsRoutine(type, int.MaxValue));
		}

		// Token: 0x06002FA8 RID: 12200 RVA: 0x000DF7F8 File Offset: 0x000DDBF8
		private IEnumerator GetAllFriendsRoutine()
		{
			Wooroutine<FacebookAPIRunner.FriendListData> playingFriends = WooroutineRunner.StartWooroutine<FacebookAPIRunner.FriendListData>(this.GetFriendsRoutine(FacebookData.Friend.Type.Playing, int.MaxValue));
			yield return playingFriends;
			int limit = this.gameConfig.general.invite_friends.maxNumFriendsShown;
			int numNeededFriends = limit - playingFriends.ReturnValue.friends.Count<FacebookData.Friend>();
			Wooroutine<FacebookAPIRunner.FriendListData> invitableFriends = WooroutineRunner.StartWooroutine<FacebookAPIRunner.FriendListData>(this.GetFriendsRoutine(FacebookData.Friend.Type.Invitable, limit));
			yield return invitableFriends;
			List<FacebookData.Friend> list = new List<FacebookData.Friend>();
			list.AddRange(playingFriends.ReturnValue.friends);
			list.AddRange(invitableFriends.ReturnValue.friends.Take(numNeededFriends));
			FacebookAPIRunner.FriendListData friends = new FacebookAPIRunner.FriendListData
			{
				friends = list,
				totalCount = list.Count
			};
			yield return friends;
			yield break;
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x000DF814 File Offset: 0x000DDC14
		private IEnumerator GetFriendsRoutine(FacebookData.Friend.Type type, int limit = 2147483647)
		{
			limit = this.gameConfig.general.invite_friends.maxNumFriendsShown;
			if (DateTime.UtcNow > this.lastRefreshTimeStamp[(int)type].AddSeconds(3600.0))
			{
				Wooroutine<FacebookAPIRunner.FriendListData> friendList = WooroutineRunner.StartWooroutine<FacebookAPIRunner.FriendListData>(this.apiRunner.DownloadFriendsData(type, limit));
				yield return friendList;
				this.friendMap.Clear();
				foreach (FacebookData.Friend friend in friendList.ReturnValue.friends)
				{
					this.friendMap[friend.ID] = friend;
				}
				this.cachedFriendListData[(int)type] = friendList.ReturnValue;
				this.lastRefreshTimeStamp[(int)type] = DateTime.UtcNow;
				if (this.LoggedIn() && type == FacebookData.Friend.Type.Playing)
				{
					Wooroutine<Dictionary<string, GameStateService.villageRankBucketData>> ranksBuckets = WooroutineRunner.StartWooroutine<Dictionary<string, GameStateService.villageRankBucketData>>(this.sbsService.doRetrieveFriendsRanks(this.friendMap.Keys));
					yield return ranksBuckets;
					this._friendsProgress = ranksBuckets.ReturnValue;
				}
			}
			this.gameState.Facebook.UpdateFriendsCount(this.FriendsPlayingCount);
			yield return this.cachedFriendListData[(int)type];
			yield break;
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x000DF840 File Offset: 0x000DDC40
		private FacebookData.Friend GetMeOrFriend(string friendID)
		{
			if (friendID == this.FB_MY_ID)
			{
				return this.Me;
			}
			if (string.IsNullOrEmpty(friendID) || !this.friendMap.ContainsKey(friendID))
			{
				return null;
			}
			return this.friendMap[friendID];
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x000DF88F File Offset: 0x000DDC8F
		public Wooroutine<FacebookService.BoxedSprite> LoadProfilePicture(FacebookData.Friend friend)
		{
			return WooroutineRunner.StartWooroutine<FacebookService.BoxedSprite>(this.DoLoadProfilePicture(friend));
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x000DF8A0 File Offset: 0x000DDCA0
		public void CleanUnknownOthersImageFromCache()
		{
			HashSet<string> hashSet = new HashSet<string>();
			if (!string.IsNullOrEmpty(this.FB_MY_ID))
			{
				hashSet.Add(this.FB_MY_ID);
			}
			if (this.friendMap != null)
			{
				foreach (string item in this.friendMap.Keys)
				{
					hashSet.Add(item);
				}
			}
			this.imageRunner.DeleteLocalProfileImageForOthers(hashSet);
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x000DF93C File Offset: 0x000DDD3C
		public Sprite GetProfilePicture(FacebookData.Friend friend)
		{
			if (friend.PictureSprite != null)
			{
				return friend.PictureSprite;
			}
			friend.PictureSprite = this.imageRunner.LoadLocalProfileImage(friend.ID);
			return friend.PictureSprite;
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x000DF974 File Offset: 0x000DDD74
		private IEnumerator GetUserFirstName()
		{
			//IGraphResult nameResult = null;
			//FB.API("/me?fields=first_name", HttpMethod.GET, delegate(IGraphResult result)
			//{
			//	nameResult = result;
			//});
			//while (nameResult == null)
			//{
			//	yield return null;
			//}
			//if (nameResult != null && nameResult.ResultDictionary != null)
			//{
			//	IDictionary<string, object> resultDictionary = nameResult.ResultDictionary;
			//	this.gameState.Facebook.loggedInUserFirstName = resultDictionary["first_name"].ToString();
			//	if (string.IsNullOrEmpty(this.gameState.PlayerName))
			//	{
			//		this.gameState.PlayerName = this.gameState.Facebook.loggedInUserFirstName;
			//	}
			//}
			yield break;
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x000DF98F File Offset: 0x000DDD8F
		//public void AddRequestSentListener(Action<FacebookRequest, IAppRequestResult> OnMessageSent)
		//{
		//	this.apiRunner.OnRequestSent.AddListener(OnMessageSent);
		//}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x000DF9A2 File Offset: 0x000DDDA2
		//public void RemoveRequestSentListener(Action<FacebookRequest, IAppRequestResult> OnMessageSent)
		//{
		//	this.apiRunner.OnRequestSent.RemoveListener(OnMessageSent);
		//}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x000DF9B8 File Offset: 0x000DDDB8
		private IEnumerator DoLoadProfilePicture(FacebookData.Friend friend)
		{
			if (friend.PictureSprite != null)
			{
				yield return new FacebookService.BoxedSprite(friend.PictureSprite);
			}
			else
			{
				FacebookData.Friend f = this.GetMeOrFriend(friend.ID);
				if (f != null && f.PictureSprite != null)
				{
					global::UnityEngine.Debug.Log("Return existing sprite");
					yield return new FacebookService.BoxedSprite(f.PictureSprite);
				}
				else
				{
					Wooroutine<FacebookService.BoxedSprite> valSprite = WooroutineRunner.StartWooroutine<FacebookService.BoxedSprite>(this.imageRunner.LoadOrDownloadProfileImage(friend));
					yield return valSprite;
					if (f != null)
					{
						f.PictureSprite = valSprite.ReturnValue.spr;
					}
					yield return valSprite.ReturnValue;
				}
			}
			yield break;
		}

		// Token: 0x040058C1 RID: 22721
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x040058C2 RID: 22722
		[WaitForService(true, true)]
		private ConfigService gameConfig;

		// Token: 0x040058C3 RID: 22723
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x040058C4 RID: 22724
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x040058C5 RID: 22725
		[WaitForService(false, true)]
		private TrackingService trackingService;

		// Token: 0x040058C6 RID: 22726
		public const int DEV_FACEBOOK_INDEX = 0;

		// Token: 0x040058C7 RID: 22727
		public const int PRODUCTION_FACEBOOK_INDEX = 1;

		// Token: 0x040058C8 RID: 22728
		private const int REFRESH_INTERVAL = 3600;

		// Token: 0x040058C9 RID: 22729
		public readonly Signal<FacebookService.Status> OnLoginStatusChanged = new Signal<FacebookService.Status>();

		// Token: 0x040058CA RID: 22730
		//public readonly Signal<FacebookRequest, IAppRequestResult> OnRequestSent = new Signal<FacebookRequest, IAppRequestResult>();

		// Token: 0x040058CB RID: 22731
		private readonly FacebookAPIRunner.FriendListData[] cachedFriendListData = new FacebookAPIRunner.FriendListData[2];

		// Token: 0x040058CC RID: 22732
		private readonly DateTime[] lastRefreshTimeStamp = new DateTime[2];

		// Token: 0x040058CD RID: 22733
		private DateTime lastFetch;

		// Token: 0x040058CE RID: 22734
		private List<FacebookData.Request> cachedRequests;

		// Token: 0x040058CF RID: 22735
		private FacebookData.Friend _me;

		// Token: 0x040058D0 RID: 22736
		private Dictionary<string, FacebookData.Friend> friendMap = new Dictionary<string, FacebookData.Friend>();

		// Token: 0x040058D1 RID: 22737
		private FacebookAPIRunner apiRunner;

		// Token: 0x040058D2 RID: 22738
		private FacebookImageRunner imageRunner;

		// Token: 0x040058D3 RID: 22739
		private FacebookService.Status _currentStatus;

		// Token: 0x040058D4 RID: 22740
		private Dictionary<string, GameStateService.villageRankBucketData> _friendsProgress = new Dictionary<string, GameStateService.villageRankBucketData>();

		// Token: 0x02000796 RID: 1942
		public struct FBLoginResult
		{
			// Token: 0x040058D6 RID: 22742
			public bool sbsUserChanged;
		}

		// Token: 0x02000797 RID: 1943
		public enum Status
		{
			// Token: 0x040058D8 RID: 22744
			LoggedOut,
			// Token: 0x040058D9 RID: 22745
			LoggedIn
		}

		// Token: 0x02000798 RID: 1944
		public struct BoxedSprite
		{
			// Token: 0x06002FB2 RID: 12210 RVA: 0x000DF9DA File Offset: 0x000DDDDA
			public BoxedSprite(Sprite spr)
			{
				this.spr = spr;
			}

			// Token: 0x040058DA RID: 22746
			public Sprite spr;
		}
	}
}
