using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Wooga.Signals; //using Facebook.Unity;

// Token: 0x02000799 RID: 1945
namespace Match3.Scripts1
{
	public class PTFacebookLivesHandler
	{
		// Token: 0x06002FB3 RID: 12211 RVA: 0x000E0A00 File Offset: 0x000DEE00
		public PTFacebookLivesHandler(int friendRequestCooldown, FacebookAPIRunner apiRunner, IFacebookDataStore facebookData)
		{
			this._friendRequestCooldown = friendRequestCooldown;
			this._facebookData = facebookData;
			this._apiRunner = apiRunner;
			//apiRunner.OnRequestSent.AddListener(new Action<FacebookRequest, IAppRequestResult>(this.handleRequestSent));
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x000E0A40 File Offset: 0x000DEE40
		//private void handleRequestSent(FacebookRequest request, IAppRequestResult response)
		//{
		//	if (request.Op != PendingFacebookOperation.OpType.Request)
		//	{
		//		return;
		//	}
		//	foreach (string friendID in request.recipients)
		//	{
		//		this.LogRequestToUser(friendID);
		//	}
		//	this.UserRequestsSent.Dispatch(request.recipients);
		//}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x000E0ABC File Offset: 0x000DEEBC
		public void SendGiftLives(IEnumerable<string> recipientIDs, bool isResponse, string context, string context2)
		{
			WoogaDebug.Log(new object[]
			{
				"Send a life to: ",
				recipientIDs
			});
			this._apiRunner.EnqueueFBRequest(this._facebookData, FacebookRequest.Create(recipientIDs, "More Lives", "Please have a life", new Dictionary<string, object>
			{
				{
					"type",
					"gift"
				},
				{
					"item",
					"lives"
				},
				{
					"isResponse",
					isResponse
				}
			}, "send_life", context, context2));
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x000E0B44 File Offset: 0x000DEF44
		public void SendRequestLives(string requestTitle, string requestMessage, IEnumerable<string> recipientIDs, string context)
		{
			string[] array = recipientIDs.Where(new Func<string, bool>(this.CanRequestLivesFrom)).ToArray<string>();
			if (array.Length == 0)
			{
				WoogaDebug.Log(new object[]
				{
					"All recipient have been filtered, returning"
				});
				return;
			}
			FacebookRequest newOp = FacebookRequest.Create(array, requestTitle, requestMessage, new Dictionary<string, object>
			{
				{
					"type",
					"request"
				},
				{
					"item",
					"lives"
				}
			}, "send_life", context, string.Empty);
			this._apiRunner.EnqueueFBRequest(this._facebookData, newOp);
		}

		// Token: 0x06002FB7 RID: 12215 RVA: 0x000E0BD4 File Offset: 0x000DEFD4
		public FacebookRequest AskAnyFriendForLife(string requestTitle, string requestMessage, string context)
		{
			FacebookRequest facebookRequest = FacebookRequest.Create(new string[0], requestTitle, requestMessage, new Dictionary<string, object>
			{
				{
					"type",
					"request"
				},
				{
					"item",
					"lives"
				}
			}, "send_life", context, string.Empty);
			this._apiRunner.EnqueueFBRequest(this._facebookData, facebookRequest);
			return facebookRequest;
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x000E0C34 File Offset: 0x000DF034
		public bool CanRequestLivesFrom(string friendID)
		{
			DateTime d = this.TimeLastLifeRequestedFrom(friendID);
			double totalSeconds = (DateTime.UtcNow - d).TotalSeconds;
			int friendRequestCooldown = this._friendRequestCooldown;
			return totalSeconds > (double)friendRequestCooldown;
		}

		// Token: 0x06002FB9 RID: 12217 RVA: 0x000E0C70 File Offset: 0x000DF070
		public DateTime TimeLastLifeRequestedFrom(string friendID)
		{
			FacebookData.FacebookRequestLog facebookRequestLog = (from r in this._facebookData.GetRequestLog()
				where r.ID == friendID
				select r).FirstOrDefault<FacebookData.FacebookRequestLog>();
			if (facebookRequestLog == null)
			{
				return new DateTime(0L);
			}
			return facebookRequestLog.timestamp;
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x000E0CC0 File Offset: 0x000DF0C0
		public DateTime TimeLifeRequestAvailable(string friendID)
		{
			return this.TimeLifeRequestAvailable(this.TimeLastLifeRequestedFrom(friendID));
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x000E0CCF File Offset: 0x000DF0CF
		private DateTime TimeLifeRequestAvailable(DateTime lastDateTime)
		{
			return lastDateTime + TimeSpan.FromSeconds((double)this._friendRequestCooldown);
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x000E0CE4 File Offset: 0x000DF0E4
		public FacebookData.Request GetRequestFromID(string requestID)
		{
			foreach (FacebookData.Request request in this._facebookData.Requests)
			{
				if (request.ID == requestID)
				{
					return request;
				}
			}
			return null;
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x000E0D5C File Offset: 0x000DF15C
		public void LogRequestToUser(string friendID)
		{
			DateTime now = DateTime.UtcNow;
			List<FacebookData.FacebookRequestLog> list = (from r in this._facebookData.GetRequestLog()
				where r.ID != friendID
				where now < this.TimeLifeRequestAvailable(r.timestamp)
				select r).ToList<FacebookData.FacebookRequestLog>();
			list.Add(new FacebookData.FacebookRequestLog
			{
				ID = friendID,
				timestamp = DateTime.UtcNow
			});
			this._facebookData.SetRequestLog(list);
			this._facebookData.Save();
		}

		// Token: 0x040058DB RID: 22747
		public readonly Signal<IEnumerable<string>> UserRequestsSent = new Signal<IEnumerable<string>>();

		// Token: 0x040058DC RID: 22748
		private FacebookAPIRunner _apiRunner;

		// Token: 0x040058DD RID: 22749
		private IFacebookDataStore _facebookData;

		// Token: 0x040058DE RID: 22750
		private int _friendRequestCooldown;
	}
}
