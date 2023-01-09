using Match3.Scripts1.Wooga.Core.Network;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Wooga.Services.ConfigServiceV2
{
	// Token: 0x0200033B RID: 827
	public class SbsConfigService<T> : SbsConfigService, ISbsConfigService<T> where T : new()
	{
		// Token: 0x06001968 RID: 6504 RVA: 0x0007330E File Offset: 0x0007170E
		public SbsConfigService(ISbsNetworking networking, string sbsId, string trackingId, string configVersion, string publicKey) : base(networking, sbsId, trackingId, configVersion, publicKey)
		{
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x0007331D File Offset: 0x0007171D
		public T GetLatest()
		{
			return JSON.Deserialize<T>(base.GetLatestJson());
		}
	}
}
