using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using UnityEngine; //using Facebook.Unity;

// Token: 0x02000777 RID: 1911
namespace Match3.Scripts1
{
	public class FacebookAPIRunner
	{
		// Token: 0x06002F42 RID: 12098 RVA: 0x000DCD46 File Offset: 0x000DB146
		public FacebookAPIRunner(IFacebookDataStore facebookData)
		{
			this._facebookData = facebookData;
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x000DCD6C File Offset: 0x000DB16C
		public void Start()
		{
			for (int i = 0; i < 8; i++)
			{
				this.coroutines.Add(WooroutineRunner.StartCoroutine(this.RunFBSyncProcessor(), null));
			}
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x000DCDA4 File Offset: 0x000DB1A4
		public void Stop()
		{
			foreach (Coroutine coroutine in this.coroutines)
			{
				WooroutineRunner.Stop(coroutine);
			}
			this.coroutines.Clear();
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x000DCE0C File Offset: 0x000DB20C
		private FacebookAPIRunner.OpResult sendRequest(FacebookRequest request)
		{
			try
			{
				if (request.recipients.Count == 0)
				{
					Log.Error("FB Request", "Attempting to send request to 0 recipients", null);
					return FacebookAPIRunner.OpResult.FailCancel;
				}
				//FB.AppRequest(request.message, request.recipients, null, null, null, request.data, request.title, delegate(IAppRequestResult result)
				//{
				//	if (!result.Cancelled)
				//	{
				//		this.OnRequestSent.Dispatch(request, result);
				//	}
				//});
			}
			catch (Exception ex)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Error making request to Facebook: " + ex.Message
				});
				return FacebookAPIRunner.OpResult.FailRetry;
			}
			return FacebookAPIRunner.OpResult.Success;
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x000DCEE0 File Offset: 0x000DB2E0
		public IEnumerator doDeleteRequest(string requestID)
		{
			//IGraphResult deleteResult = null;
			//FB.API(requestID, HttpMethod.DELETE, delegate(IGraphResult result)
			//{
			//	deleteResult = result;
			//});
			//while (deleteResult == null)
			//{
			//	yield return null;
			//}
			//if (!string.IsNullOrEmpty(deleteResult.Error))
			//{
			//	WoogaDebug.LogWarning(new object[]
			//	{
			//		deleteResult.Error
			//	});
			//}
			//yield return (!string.IsNullOrEmpty(deleteResult.Error)) ? FacebookAPIRunner.OpResult.FailRetry : FacebookAPIRunner.OpResult.Success;
			yield break;
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x000DCEFC File Offset: 0x000DB2FC
		private IEnumerator RunFBSyncProcessor()
		{
			for (;;)
			{
				FacebookAPIRunner.OpResult opResult = FacebookAPIRunner.OpResult.FailRetry;
				Queue<PendingFacebookOperation> opQueue = this._facebookData.PendingOps;
				if (opQueue.Count == 0)
				{
					yield return null;
				}
				else
				{
					PendingFacebookOperation cOp = opQueue.Dequeue();
					if (cOp != null)
					{
						if (!(cOp.DelayUntil > DateTime.UtcNow))
						{
							if (cOp.Op == PendingFacebookOperation.OpType.Request)
							{
								try
								{
									opResult = this.sendRequest(cOp as FacebookRequest);
								}
								catch (Exception ex)
								{
									WoogaDebug.LogWarning(new object[]
									{
										"Queued request raised an exception: " + ex.Message
									});
									opResult = FacebookAPIRunner.OpResult.Success;
								}
							}
							else if (cOp.Op == PendingFacebookOperation.OpType.DeleteLifeRequest)
							{
								FacebookDeleteRequest deleteRequest = cOp as FacebookDeleteRequest;
								Wooroutine<FacebookAPIRunner.OpResult> deleteRoutine = WooroutineRunner.StartWooroutine<FacebookAPIRunner.OpResult>(this.doDeleteRequest(deleteRequest.requestID));
								yield return deleteRoutine;
								opResult = deleteRoutine.ReturnValue;
							}
							switch (opResult)
							{
								case FacebookAPIRunner.OpResult.Success:
									cOp.retired = true;
									yield return null;
									continue;
								case FacebookAPIRunner.OpResult.FailRetry:
									WoogaDebug.LogWarning(new object[]
									{
										"Queued request failed"
									});
									cOp.DelayUntil = DateTime.UtcNow + TimeSpan.FromSeconds(30.0);
									opQueue.Enqueue(cOp);
									continue;
								case FacebookAPIRunner.OpResult.FailCancel:
									WoogaDebug.LogWarning(new object[]
									{
										"Queued request failed permanantly"
									});
									cOp.retired = true;
									continue;
							}
							break;
						}
						opQueue.Enqueue(cOp);
						yield return null;
					}
				}
			}
			throw new ArgumentOutOfRangeException();
			yield break;
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x000DCF17 File Offset: 0x000DB317
		public void EnqueueFBRequest(IFacebookDataStore facebookData, PendingFacebookOperation newOp)
		{
			facebookData.PendingOps.Enqueue(newOp);
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x000DCF28 File Offset: 0x000DB328
		public IEnumerator PaginatedGETRequest(string apiCall, Func<string, bool> onResult)
		{
			//if (!FB.IsLoggedIn)
			//{
			//	yield break;
			//}
			//string nextURL = apiCall;
			//for (;;)
			//{
			//	IDictionary<string, object> apiResultObject = null;
			//	string apiResult = null;
			//	string error = null;
			//	FB.API(nextURL, HttpMethod.GET, delegate(IGraphResult result)
			//	{
			//		apiResult = result.RawResult;
			//		apiResultObject = result.ResultDictionary;
			//		error = result.Error;
			//	});
			
			//	while (apiResult == null && error == null)
			//	{
			//		yield return null;
			//	}
			//	if (apiResult == null)
			//	{
			//		break;
			//	}
			//	if (onResult(apiResult))
			//	{
			//		nextURL = apiResultObject.GetIn(FacebookData.KeyPagingURL, string.Empty);
			//	}
			//	else
			//	{
			//		nextURL = null;
			//	}
			//	if (string.IsNullOrEmpty(nextURL))
			//	{
			//		goto IL_15F;
			//	}
			//}
			//WoogaDebug.Log(new object[]
			//{
			//	"FacebookAPIRunner->WoogaDebug.Log"
			//	// <PaginatedGETRequest>c__AnonStorey.error
			//});
			//IL_15F:
			yield break;
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x000DCF4A File Offset: 0x000DB34A
		public void DeleteRequest(IFacebookDataStore facebookData, FacebookData.Request request)
		{
			facebookData.PendingOps.Enqueue(FacebookDeleteRequest.Create(request.ID));
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x000DCF64 File Offset: 0x000DB364
		public IEnumerator DownloadFriendsData(FacebookData.Friend.Type friendType, int limit)
		{
			string query = (friendType != FacebookData.Friend.Type.Invitable) ? "me/friends?fields=first_name" : "me/invitable_friends?fields=first_name,picture";
			int count2 = 0;
			List<FacebookData.Friend> list = new List<FacebookData.Friend>();
			yield return this.PaginatedGETRequest(query, delegate(string apiResultStr)
			{
				int count = limit - list.Count;
				list.AddRange(FacebookData.GetFriends(apiResultStr).Take(count));
				if (count == 0)
				{
					count = FacebookData.GetFriendCount(apiResultStr);
				}
				return list.Count < limit;
			});
			yield return new FacebookAPIRunner.FriendListData
			{
				friends = list,
				totalCount = count2
			};
			yield break;
		}

		// Token: 0x04005860 RID: 22624
		private const int CONCURRENT_FB_REQUESTS = 8;

		// Token: 0x04005861 RID: 22625
		private readonly List<Coroutine> coroutines = new List<Coroutine>();

		// Token: 0x04005862 RID: 22626
		private readonly IFacebookDataStore _facebookData;

		// Token: 0x04005863 RID: 22627
		//public readonly Signal<FacebookRequest, IAppRequestResult> OnRequestSent = new Signal<FacebookRequest, IAppRequestResult>();

		// Token: 0x02000778 RID: 1912
		private enum OpResult
		{
			// Token: 0x04005865 RID: 22629
			Success,
			// Token: 0x04005866 RID: 22630
			FailRetry,
			// Token: 0x04005867 RID: 22631
			FailCancel
		}

		// Token: 0x02000779 RID: 1913
		public class FriendListData
		{
			// Token: 0x04005868 RID: 22632
			public IEnumerable<FacebookData.Friend> friends;

			// Token: 0x04005869 RID: 22633
			public int totalCount;
		}
	}
}
