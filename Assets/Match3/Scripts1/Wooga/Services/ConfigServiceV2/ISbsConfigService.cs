using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Services.ConfigServiceV2
{
	// Token: 0x02000336 RID: 822
	public interface ISbsConfigService
	{
		// Token: 0x0600195B RID: 6491
		string GetLatestJson();

		// Token: 0x0600195C RID: 6492
		IEnumerator<SbsConfigService.Result> Fetch(int timeout = 15);

		// Token: 0x0600195D RID: 6493
		IEnumerator<SbsConfigService.Result> FetchAuthenticated(int timeout = 15);

		// Token: 0x0600195E RID: 6494
		void ClearConfigCache();

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x0600195F RID: 6495
		// (set) Token: 0x06001960 RID: 6496
		bool BundledOnly { get; set; }

		// Token: 0x06001961 RID: 6497
		IEnumerator<AbTestConfig> GetAbTestConfig();

		// Token: 0x06001962 RID: 6498
		void ForceAbTests(string abTestString);

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06001963 RID: 6499
		string AbTests { get; }

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06001964 RID: 6500
		string PersonalizationString { get; }
	}
}
