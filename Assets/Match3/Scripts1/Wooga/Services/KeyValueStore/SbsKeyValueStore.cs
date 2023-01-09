using System.Collections.Generic;
using System.Net;
using System.Threading;
using Match3.Scripts1.Wooga.Core.Data;
using Wooga.Core.Extensions;
using Match3.Scripts1.Wooga.Core.Network;
using Wooga.Core.Utilities.RequestCache;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.Authentication;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Wooga.Services.KeyValueStore
{
	// Token: 0x02000414 RID: 1044
	public class SbsKeyValueStore : ISbsKeyValueStore
	{
		// Token: 0x06001EC9 RID: 7881 RVA: 0x00082099 File Offset: 0x00080499
		public SbsKeyValueStore(ISbsNetworking networking, SbsAuthentication authentication, IRequestCache requestCache = null)
		{
			this._authentication = authentication;
			this._networking = networking;
			this._requestCache = (requestCache ?? SBS.CreateRequestCache("wdkcache.3.db"));
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x000820C8 File Offset: 0x000804C8
		public IEnumerator<SbsWriteResult> WriteToBucket(string bucket, string data, int formatVersion, SbsMergeHandler mergeHandler, string etag = null)
		{
			SbsRichData data2 = new SbsRichData(JSON.Serialize(data, false, 1, ' '), formatVersion);
			return this.WriteSbsDataToBucket(bucket, data2, mergeHandler, etag);
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x000820F4 File Offset: 0x000804F4
		public IEnumerator<SbsWriteResult> WriteToBucket(string bucket, string data, int formatVersion, SbsMergeOperationHandler mergeHandler, string etag = null)
		{
			SbsRichData data2 = new SbsRichData(JSON.Serialize(data, false, 1, ' '), formatVersion);
			return this.WriteSbsDataToBucketWithUserContext(bucket, data2, mergeHandler, this._authentication.GetUserContext(), etag, false, 60);
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x0008212C File Offset: 0x0008052C
		public IEnumerator<SbsWriteResult> WriteJsonToBucket(string bucket, string json, int formatVersion, SbsMergeHandler mergeHandler, string etag = null)
		{
			SbsRichData data = new SbsRichData(json, formatVersion);
			return this.WriteSbsDataToBucket(bucket, data, mergeHandler, etag);
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x00082150 File Offset: 0x00080550
		public IEnumerator<SbsWriteResult> WriteJsonToBucket(string bucket, string json, int formatVersion, SbsMergeOperationHandler mergeHandler, string etag = null)
		{
			SbsRichData data = new SbsRichData(json, formatVersion);
			return this.WriteSbsDataToBucketWithUserContext(bucket, data, mergeHandler, this._authentication.GetUserContext(), etag, false, 60);
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x0008217F File Offset: 0x0008057F
		public void ClearRequestCache()
		{
			this._requestCache.Clear();
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x0008218C File Offset: 0x0008058C
		private IEnumerator<SbsWriteResult> WriteSbsDataToBucket(string bucket, ISbsData data, SbsMergeHandler mergeHandler, string etag)
		{
			return this.WriteSbsDataToBucketWithUserContext(bucket, data, delegate(SbsMergeOperation operation)
			{
				operation.Resolve(mergeHandler(operation.Mine, operation.Theirs));
			}, this._authentication.GetUserContext(), etag, false, 60);
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x000821CA File Offset: 0x000805CA
		private static void PerformWriteAssertions(string bucket, string data, SbsMergeOperationHandler mergeHandler)
		{
			Assert.That(bucket.IsNotNull(), "You have to provide a bucket name");
			Assert.That(data.IsNotNull(), "Data to write cannot be null");
			Assert.That(mergeHandler.IsNotNull(), "You have to provide a mergeHandler delegate");
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x000821FC File Offset: 0x000805FC
		private IEnumerator<SbsWriteResult> WriteSbsDataToBucketWithUserContext(string bucket, ISbsData data, SbsMergeOperationHandler mergeHandler, UserContext userContext, string etag = null, bool isMergeWrite = false, int timeoutInSeconds = 60)
		{
			SbsKeyValueStore.PerformWriteAssertions(bucket, data.Data, mergeHandler);
			SbsRequest request = SbsRequestFactory.KeyValueStore.CreateBucketWriteRequest(userContext, bucket, data.ToString(), timeoutInSeconds);
			if (etag == null)
			{
				etag = this._requestCache.GetEtagForPath(request.Path);
			}
			request.ETag = etag;
			return this._networking.Send(request).ContinueWith(delegate(SbsResponse response)
			{
				if (response.StatusCode == HttpStatusCode.NoContent)
				{
					this._requestCache.Cache(request.Path, request.Body, response.ETag);
				}
				if (!response.StatusCode.In(new int[]
				{
					428,
					412
				}))
				{
					SbsWriteResult obj = new SbsWriteResult
					{
						Success = (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotModified),
						ETag = response.ETag
					};
					if (isMergeWrite)
					{
						obj.MergedSbsData = data;
					}
					return obj.Yield<SbsWriteResult>();
				}
				Assert.That(response.BodyString.IsNotNull(), "Could not create overrideRequest: body is empty");
				ISbsData sbsData2 = SbsDataHelper.TryCreateFromJson(response.BodyString);
				Assert.That(sbsData2.IsNotNull(), "Could not parse SbsData from body [" + response.BodyString + "]");
				ISbsData mergeResult = null;
				bool mergeHasSucceeded = false;
				ManualResetEvent mre = new ManualResetEvent(false);
				mergeHandler(new SbsMergeOperation(data, sbsData2, delegate(ISbsData sbsData)
				{
					mergeHasSucceeded = true;
					mergeResult = sbsData;
					mre.Set();
				}, delegate()
				{
					mre.Set();
				}));
				mre.WaitOne();
				if (mergeHasSucceeded)
				{
					SbsKeyValueStore.PerformWriteAssertions(bucket, mergeResult.Data, mergeHandler);
					return this.WriteSbsDataToBucketWithUserContext(bucket, mergeResult, mergeHandler, userContext, response.ETag, true, 60);
				}
				return new SbsWriteResult
				{
					Success = false,
					ETag = null
				}.Yield<SbsWriteResult>();
			});
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x000822CC File Offset: 0x000806CC
		public IEnumerator<SbsReadResult> ReadFromBucket(string bucket, string sbsUserId = null, int timeoutInSeconds = 60)
		{
			bool flag = sbsUserId != null;
			UserContext userContext = this._authentication.GetUserContext();
			string sbsUserId2 = sbsUserId ?? userContext.user_id;
			string etag = (!flag) ? null : "enforce_fresh";
			return this.ReadFromBucket(bucket, sbsUserId2, timeoutInSeconds, etag);
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x00082318 File Offset: 0x00080718
		private IEnumerator<SbsReadResult> ReadFromBucket(string bucket, string sbsUserId, int timeoutInSeconds, string etag = null)
		{
			UserContext userContext = this._authentication.GetUserContext();
			SbsRequest request = SbsRequestFactory.KeyValueStore.CreateBucketReadRequest(userContext, bucket, sbsUserId, timeoutInSeconds);
			request.ETag = ((etag == null) ? this._requestCache.GetEtagForPath(request.Path) : etag);
			return this._networking.Send(request).ContinueWith(delegate(SbsResponse response)
			{
				this.CacheReadResponse(request, response);
				string cacheReadResponse = this.GetCacheReadResponse(request, response);
				bool flag = response.StatusCode == HttpStatusCode.NotModified && cacheReadResponse == null;
				if (flag)
				{
					this._requestCache.Remove(request.Path);
					return this.ReadFromBucket(bucket, sbsUserId, timeoutInSeconds, "enforce_fresh");
				}
				return this.CreateSbsReadResult(cacheReadResponse, response).Yield<SbsReadResult>();
			});
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x000823C2 File Offset: 0x000807C2
		private void CacheReadResponse(SbsRequest request, SbsResponse response)
		{
			if (response.StatusCode == HttpStatusCode.OK)
			{
				this._requestCache.Cache(request.Path, response.BodyString, response.ETag);
			}
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x000823F1 File Offset: 0x000807F1
		private string GetCacheReadResponse(SbsRequest request, SbsResponse response)
		{
			if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotModified)
			{
				return this._requestCache.GetResource(request.Path);
			}
			return null;
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x00082428 File Offset: 0x00080828
		private SbsReadResult CreateSbsReadResult(string responseJson, SbsResponse response)
		{
			bool flag = responseJson == null;
			if (flag)
			{
				return new SbsReadResult
				{
					Data = null,
					ETag = response.ETag
				};
			}
			bool flag2 = responseJson == string.Empty;
			if (flag2)
			{
				return new SbsReadResult
				{
					Data = new SbsRichData(null, -1),
					ETag = response.ETag
				};
			}
			return new SbsReadResult
			{
				Data = SbsDataHelper.TryCreateFromJson(responseJson),
				ETag = response.ETag
			};
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x000824BC File Offset: 0x000808BC
		public IEnumerator<List<SbsKeyValueStoreFbData>> ReadFromBucketWithFacebookIds(string bucket, List<string> facebookUserIds)
		{
			SbsRequest sbsRequest = SbsRequestFactory.KeyValueStore.CreateBucketsOfFacebookIdsReadRequest(this._authentication.GetUserContext(), bucket, facebookUserIds);
			sbsRequest.ETag = "enforce_fresh";
			return this._networking.Send(sbsRequest).ContinueWith(delegate(SbsResponse response)
			{
				string dataFromResponse = this.GetDataFromResponse(response);
				return SbsKeyValueStore.GetDataFromReadWithFacebookIds(bucket, dataFromResponse);
			});
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x00082520 File Offset: 0x00080920
		private static List<SbsKeyValueStoreFbData> GetDataFromReadWithFacebookIds(string bucket, string json)
		{
			List<SbsKeyValueStoreFbData> list = null;
			JSONNode jsonnode = JSON.Deserialize(json);
			if (jsonnode.HasKey(bucket))
			{
				list = new List<SbsKeyValueStoreFbData>();
				JSONArray collection = jsonnode.GetCollection(bucket, null);
				string key = bucket.Remove(bucket.Length - 1, 1);
				foreach (JSONNode jsonnode2 in collection)
				{
					SbsKeyValueStoreFbData sbsKeyValueStoreFbData = new SbsKeyValueStoreFbData();
					sbsKeyValueStoreFbData.fb_id = jsonnode2.GetString("fb_id", null);
					sbsKeyValueStoreFbData.user_id = jsonnode2.GetString("user_id", null);
					JSONNode node = jsonnode2.GetNode(key, null);
					sbsKeyValueStoreFbData.data = new SbsKeyValueStoreData(node.GetInt("format_version", 0), node.GetString("data", null));
					list.Add(sbsKeyValueStoreFbData);
				}
			}
			return list;
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x00082610 File Offset: 0x00080A10
		private string GetDataFromResponse(SbsResponse response)
		{
			HttpStatusCode statusCode = response.StatusCode;
			if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NotModified)
			{
				return null;
			}
			return response.BodyString;
		}

		// Token: 0x04004A90 RID: 19088
		private readonly SbsAuthentication _authentication;

		// Token: 0x04004A91 RID: 19089
		private readonly ISbsNetworking _networking;

		// Token: 0x04004A92 RID: 19090
		private readonly IRequestCache _requestCache;
	}
}
