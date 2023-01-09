using System.Collections.Generic;
using Match3.Scripts1;

// Token: 0x02000AB5 RID: 2741
namespace Match3.Scripts2.Env
{
	public abstract class AGameEnvironmentConfig : ISbsEnvironmentConfig
	{
		public GameEnvironment.Platform currentPlatform = GameEnvironment.Platform.HW_Abroad;
	
		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06004108 RID: 16648
		public abstract string PLATFORM_PREFIX { get; }

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06004109 RID: 16649
		public abstract string StoreUrl { get; }

		// Token: 0x0600410A RID: 16650 RVA: 0x00151A18 File Offset: 0x0014FE18
		public string GetAdminId(GameEnvironment.Environment env)
		{
			return this.mapEnvToAdminId[env];
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x00151A26 File Offset: 0x0014FE26
		public string GetAdminPw(GameEnvironment.Environment env)
		{
			return this.mapEnvToAdminPw[env];
		}

		// Token: 0x0600410C RID: 16652 RVA: 0x00151A34 File Offset: 0x0014FE34
		public string GetId(GameEnvironment.Environment env)
		{
			return this.mapEnvToId[env];
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x00151A42 File Offset: 0x0014FE42
		public string GetBundleId(GameEnvironment.Environment env)
		{
			return this.mapEnvToBundleId[env];
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x00151A50 File Offset: 0x0014FE50
		public bool HasId(string id)
		{
			return this.mapEnvToId.ContainsValue(id);
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x00151A60 File Offset: 0x0014FE60
		public GameEnvironment.Environment GetEnvironment(string id)
		{
			foreach (KeyValuePair<GameEnvironment.Environment, string> keyValuePair in this.mapEnvToId)
			{
				if (keyValuePair.Value == id)
				{
					return keyValuePair.Key;
				}
			}
			WoogaDebug.LogError(new object[]
			{
				"wrong platform for id",
				id
			});
			// return GameEnvironment.Environment.CI;
			return GameEnvironment.Environment.PRODUCTION;
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x00151AF0 File Offset: 0x0014FEF0
		public string GetPublicKey(GameEnvironment.Environment env)
		{
			return this.mapEnvToKey[env];
		}

		// Token: 0x04006AD2 RID: 27346
		protected Dictionary<GameEnvironment.Environment, string> mapEnvToBundleId;

		// Token: 0x04006AD3 RID: 27347
		protected Dictionary<GameEnvironment.Environment, string> mapEnvToId;

		// Token: 0x04006AD4 RID: 27348
		protected Dictionary<GameEnvironment.Environment, string> mapEnvToStoreUrl;

		// Token: 0x04006AD5 RID: 27349
		protected Dictionary<GameEnvironment.Environment, string> mapEnvToKey;

		// Token: 0x04006AD6 RID: 27350
		protected Dictionary<GameEnvironment.Environment, string> mapEnvToAdminId;

		// Token: 0x04006AD7 RID: 27351
		protected Dictionary<GameEnvironment.Environment, string> mapEnvToAdminPw;
	}
}
