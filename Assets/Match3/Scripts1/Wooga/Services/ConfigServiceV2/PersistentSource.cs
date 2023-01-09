using Match3.Scripts1.Wooga.Core.Data;
using Wooga.Core.Storage;
using Wooga.Core.Utilities;
using Wooga.Core.Extensions;

namespace Match3.Scripts1.Wooga.Services.ConfigServiceV2
{
	// Token: 0x02000334 RID: 820
	public class PersistentSource : IReadableConfigSource, IWritableConfigSource
	{
		// Token: 0x0600194F RID: 6479 RVA: 0x00072B48 File Offset: 0x00070F48
		public PersistentSource(string configVersion, ISbsStorage sbsStorage = null)
		{
			this._configVersion = configVersion;
			this._signedStorage = new SbsSignedStorage(sbsStorage ?? Util.Storage(), "74jndf8nkshgd9m4i");
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001950 RID: 6480 RVA: 0x00072B74 File Offset: 0x00070F74
		// (set) Token: 0x06001951 RID: 6481 RVA: 0x00072B7C File Offset: 0x00070F7C
		public string Hash { get; private set; }

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001952 RID: 6482 RVA: 0x00072B85 File Offset: 0x00070F85
		// (set) Token: 0x06001953 RID: 6483 RVA: 0x00072B8D File Offset: 0x00070F8D
		public string AbTests { get; private set; }

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001954 RID: 6484 RVA: 0x00072B96 File Offset: 0x00070F96
		// (set) Token: 0x06001955 RID: 6485 RVA: 0x00072B9E File Offset: 0x00070F9E
		public string PersonalizationString { get; private set; }

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001956 RID: 6486 RVA: 0x00072BA7 File Offset: 0x00070FA7
		private string FileName
		{
			get
			{
				return "sbs-configs-" + this._configVersion + ".json";
			}
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x00072BC0 File Offset: 0x00070FC0
		public void Write(PersistedConfigData data)
		{
			this.Hash = data.hash;
			this.AbTests = data.abTests;
			this.PersonalizationString = data.personalizationString;
			SbsRichData sbsRichData = new SbsRichData(data.json, -1);
			sbsRichData.MetaData.Data.Add("content-hash", data.hash);
			sbsRichData.MetaData.Data.Add("ab-tests", data.abTests);
			sbsRichData.MetaData.Data.Add("personalization-string", data.personalizationString);
			this._signedStorage.Save(this.FileName, sbsRichData);
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x00072C64 File Offset: 0x00071064
		public PersistedConfigData Read()
		{
			SbsRichData sbsRichData = this._signedStorage.Load(this.FileName);
			if (sbsRichData == null)
			{
				return null;
			}
			this.Hash = sbsRichData.MetaData.Data.GetHeaderValue("content-hash");
			this.AbTests = sbsRichData.MetaData.Data.GetHeaderValue("ab-tests");
			this.PersonalizationString = sbsRichData.MetaData.Data.GetHeaderValue("personalization-string");
			return new PersistedConfigData(sbsRichData.Data, this.Hash, this.AbTests, this.PersonalizationString);
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x00072CF9 File Offset: 0x000710F9
		public void Delete()
		{
			this._signedStorage.Delete(this.FileName);
		}

		// Token: 0x0400480A RID: 18442
		private const string KEY_CONTENT_HASH = "content-hash";

		// Token: 0x0400480B RID: 18443
		private const string KEY_AB_TEST_STRING = "ab-tests";

		// Token: 0x0400480C RID: 18444
		private const string KEY_PERSONALIZATION_STRING = "personalization-string";

		// Token: 0x0400480D RID: 18445
		private const string CONFIGS_FILENAME = "sbs-configs";

		// Token: 0x0400480E RID: 18446
		private readonly string _configVersion;

		// Token: 0x0400480F RID: 18447
		private readonly ISbsStorage _signedStorage;
	}
}
