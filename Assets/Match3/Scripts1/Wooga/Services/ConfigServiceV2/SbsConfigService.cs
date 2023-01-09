using System;
using System.Collections.Generic;
using System.Net;
using Wooga.Core.Extensions;
using Match3.Scripts1.Wooga.Core.Network;
using Wooga.Core.Storage;
using Wooga.Core.Utilities;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.Authentication;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Wooga.Services.ConfigServiceV2
{
	// Token: 0x0200033C RID: 828
	public class SbsConfigService : ISbsConfigService
	{
		// Token: 0x0600196A RID: 6506 RVA: 0x00072D24 File Offset: 0x00071124
		public SbsConfigService(ISbsNetworking networking, string sbsId, string trackingId, string configVersion, string publicKey) : this(networking, sbsId, trackingId, configVersion, publicKey, Util.Storage(), SbsConfigService.CreateDefaultConfigValidation(), new DefaultSbsConfigMergeStrategy())
		{
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x00072D50 File Offset: 0x00071150
		public SbsConfigService(ISbsNetworking networking, string sbsId, string trackingId, string configVersion, string publicKey, ISbsStorage storage, ISbsConfigValidation validation) : this(networking, sbsId, trackingId, configVersion, publicKey, storage, validation, new DefaultSbsConfigMergeStrategy())
		{
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x00072D74 File Offset: 0x00071174
		public SbsConfigService(ISbsNetworking networking, string sbsId, string trackingId, string configVersion, string publicKey, ISbsStorage storage, ISbsConfigValidation validation, ISbsConfigMergeStrategy mergeStrategy)
		{
			Assert.That(!string.IsNullOrEmpty(sbsId), "[SbsConfigService] Configure - sbsId not provided");
			Assert.That(!string.IsNullOrEmpty(configVersion), "[SbsConfigService] Configure - configVersion not provided");
			this._configVersion = configVersion;
			this._networking = networking;
			this._sbsId = sbsId;
			this._publicKey = publicKey;
			this._trackingId = trackingId;
			this._persistence = new PersistentSource(this._configVersion, storage);
			this._bundled = new BundledSource();
			this._validation = validation;
			this._mergeStrategy = mergeStrategy;
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x0600196D RID: 6509 RVA: 0x00072E01 File Offset: 0x00071201
		public string AbTests
		{
			get
			{
				return this.CurrentSource.AbTests;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x0600196E RID: 6510 RVA: 0x00072E0E File Offset: 0x0007120E
		public string PersonalizationString
		{
			get
			{
				return this.CurrentSource.PersonalizationString;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x0600196F RID: 6511 RVA: 0x00072E1B File Offset: 0x0007121B
		// (set) Token: 0x06001970 RID: 6512 RVA: 0x00072E23 File Offset: 0x00071223
		public bool BundledOnly { get; set; }

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06001971 RID: 6513 RVA: 0x00072E2C File Offset: 0x0007122C
		private string LatestHash
		{
			get
			{
				return this._persistence.Hash ?? this._bundled.Hash;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06001972 RID: 6514 RVA: 0x00072E4B File Offset: 0x0007124B
		private IReadableConfigSource CurrentSource
		{
			get
			{
				if (!string.IsNullOrEmpty(this._currentHash) && this._currentHash == this._persistence.Hash)
				{
					return this._persistence;
				}
				return this._bundled;
			}
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x00072E85 File Offset: 0x00071285
		private static ISbsConfigValidation CreateDefaultConfigValidation()
		{
			return SbsConfigValidation.singleton;
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x00072E8C File Offset: 0x0007128C
		public string GetLatestJson()
		{
			PersistedConfigData persistedConfigData = null;
			if (!this.BundledOnly)
			{
				persistedConfigData = this._persistence.Read();
			}
			if (persistedConfigData == null)
			{
				persistedConfigData = this._bundled.Read();
			}
			this._currentHash = persistedConfigData.hash;
			Log.Info(new object[]
			{
				"switched to " + persistedConfigData.abTests + "|" + persistedConfigData.json
			});
			return persistedConfigData.json;
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x00072EFF File Offset: 0x000712FF
		public void ClearConfigCache()
		{
			this._persistence.Delete();
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x00072F0C File Offset: 0x0007130C
		public void ForceAbTests(string abTests)
		{
			this._forcedAbTests = abTests;
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x00072F15 File Offset: 0x00071315
		public void ForcePersonalizationString(string personalizationString)
		{
			this._forcedPersonalizationString = personalizationString;
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x00072F20 File Offset: 0x00071320
		public IEnumerator<AbTestConfig> GetAbTestConfig()
		{
			SbsRequest sbsRequest = new SbsRequest
			{
				Path = string.Format("/ab_config/{0}/{1}.json", this._configVersion, this._sbsId),
				Method = HttpMethod.GET
			};
			return this._networking.Send(sbsRequest).ContinueWith((SbsResponse response) => JSON.Deserialize<AbTestConfig>(response.BodyString));
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x00072F88 File Offset: 0x00071388
		[Obsolete("Please use FetchAuthenticated. The new endpoint is required to have unified configs across different devices and platforms.If you would like to get more information please ask SBS or visit: https://docs.sbs.wooga.com/swagger/index.html?url=https://api.sbs.wooga.com/config/v2/docs")]
		public IEnumerator<SbsConfigService.Result> Fetch(int timeout = 15)
		{
			SbsRequest request = SbsRequestFactory.ConfigService.CreateFetchConfigRequest(this.LatestHash, this._sbsId, this._trackingId, this._configVersion, this._forcedAbTests, timeout);
			return this.FetchInternal(request, true);
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x00072FC4 File Offset: 0x000713C4
		public IEnumerator<SbsConfigService.Result> FetchAuthenticated(int timeout = 15)
		{
			UserContext userContext = SBS.Authentication.GetUserContext();
			SbsRequest request = SbsRequestFactory.ConfigService.CreateFetchAuthenticatedConfigRequest(userContext, this.LatestHash, this._trackingId, this._configVersion, this._forcedAbTests, this._forcedPersonalizationString, timeout);
			return this.FetchInternal(request, false);
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x0007300C File Offset: 0x0007140C
		private IEnumerator<SbsConfigService.Result> FetchInternal(SbsRequest request, bool shouldValidateConfigSignature)
		{
			SbsConfigService.Result result = default(SbsConfigService.Result);
			result.result = FetchResult.Unchanged;
			if (this.BundledOnly)
			{
				return result.Yield<SbsConfigService.Result>();
			}
			if (this.fetchRoutine == null)
			{
				this.fetchRoutine = global::Wooga.Coroutines.Coroutines.Empty().ContinueWith(() => this.FetchInternal(request)).ContinueWith(delegate(SbsResponse response)
				{
					if (this.ResponseIsValid(response, shouldValidateConfigSignature))
					{
						string hash = response.Headers["x-sbs-content-hash"];
						string abTests = response.Headers["x-sbs-ab-string"];
						string personalizationString;
						response.Headers.TryGetValue("x-sbs-personalization-string", out personalizationString);
						Log.Info(new object[]
						{
							"[SbsConfigService] Fetch result: " + response.BodyString
						});
						this.PersistConfigs(response.BodyString, hash, abTests, personalizationString);
						if (response.BodyString != "{}")
						{
							result.result = FetchResult.Success;
						}
					}
					else
					{
						result.result = FetchResult.Failed;
						result.exception = new Exception("[SbsConfigService] Invalid response");
					}
					return result;
				}).Catch(delegate(Exception exception)
				{
					result.result = FetchResult.Failed;
					result.exception = exception;
					return result;
				}).Finally(delegate()
				{
					this.fetchRoutine = null;
				});
			}
			return this.fetchRoutine;
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x000730C8 File Offset: 0x000714C8
		private void PersistConfigs(string fetchedJson, string hash, string abTests, string personalizationString)
		{
			PersistedConfigData persistedConfigData = this.CurrentSource.Read();
			Assert.That(persistedConfigData != null, "[SbsConfigService] Current config data is null.");
			string json = this._mergeStrategy.Merge(persistedConfigData.json, fetchedJson);
			PersistedConfigData data = new PersistedConfigData(json, hash, abTests, personalizationString);
			this._persistence.Write(data);
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x0007311C File Offset: 0x0007151C
		private IEnumerator<SbsResponse> FetchInternal(SbsRequest request)
		{
			return this._networking.Send(request).ContinueWith(delegate(SbsResponse response)
			{
				Log.Info(new object[]
				{
					"response: " + response.BodyString
				});
				return response;
			});
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x0007314C File Offset: 0x0007154C
		private bool ResponseIsValid(SbsResponse response, bool shouldValidateConfigSignature)
		{
			bool flag = response != null && response.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response.BodyString) && !response.HasError();
			if (!flag)
			{
				return false;
			}
			if (shouldValidateConfigSignature)
			{
				flag = this._validation.VerifySBSResponse(response.BodyString, response.SignatureConfig, response.SignatureAlgo, this._publicKey);
			}
			return flag;
		}

		// Token: 0x0400481D RID: 18461
		public const string HASHING_KEY = "74jndf8nkshgd9m4i";

		// Token: 0x0400481E RID: 18462
		public const string HEADER_CONTENT_HASH = "x-sbs-content-hash";

		// Token: 0x0400481F RID: 18463
		public const string HEADER_AB_STRING = "x-sbs-ab-string";

		// Token: 0x04004820 RID: 18464
		public const string HEADER_PERSONALIZATION_STRING = "x-sbs-personalization-string";

		// Token: 0x04004821 RID: 18465
		private readonly ISbsNetworking _networking;

		// Token: 0x04004822 RID: 18466
		private readonly string _sbsId;

		// Token: 0x04004823 RID: 18467
		private readonly string _trackingId;

		// Token: 0x04004824 RID: 18468
		private readonly string _configVersion;

		// Token: 0x04004825 RID: 18469
		private readonly string _publicKey;

		// Token: 0x04004826 RID: 18470
		private readonly PersistentSource _persistence;

		// Token: 0x04004827 RID: 18471
		private readonly BundledSource _bundled;

		// Token: 0x04004828 RID: 18472
		private readonly ISbsConfigValidation _validation;

		// Token: 0x04004829 RID: 18473
		private readonly ISbsConfigMergeStrategy _mergeStrategy;

		// Token: 0x0400482A RID: 18474
		private string _currentHash;

		// Token: 0x0400482B RID: 18475
		private string _forcedAbTests;

		// Token: 0x0400482C RID: 18476
		private string _forcedPersonalizationString;

		// Token: 0x0400482D RID: 18477
		private IEnumerator<SbsConfigService.Result> fetchRoutine;

		// Token: 0x0200033D RID: 829
		public struct Result
		{
			// Token: 0x04004831 RID: 18481
			public FetchResult result;

			// Token: 0x04004832 RID: 18482
			public Exception exception;
		}
	}
}
