using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Match3.Scripts1.Shared.ResourceManager;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x0200094D RID: 2381
	public class SeasonSpriteManagerFlow : AFlow
	{
		// Token: 0x060039F1 RID: 14833 RVA: 0x0011DB45 File Offset: 0x0011BF45
		public SeasonSpriteManagerFlow()
		{
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x0011DB4D File Offset: 0x0011BF4D
		public SeasonSpriteManagerFlow(SeasonPrizeInfo season)
		{
			this.seasonName = season.name;
			this.bundleName = season.PrimaryBundleName;
		}

		// Token: 0x060039F3 RID: 14835 RVA: 0x0011DB70 File Offset: 0x0011BF70
		protected override IEnumerator FlowRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			// SeasonConfig season = this.seasonService.GetActiveSeason();
			// if (this.seasonName == null && season != null && this.seasonService.GetGrandPrizeBuildingConfig() != null)
			// {
			// 	this.seasonName = season.Primary;
			// 	this.bundleName = season.PrimaryBundleName;
			// }
			if (this.seasonName != null)
			{
				// this.spriteManagerFlowParam = new BundledSpriteManagerLoaderFlow.Input
				// {
				// 	bundleName = this.bundleName,
				// 	path = SeasonConfigHelpers.SpriteManagerForSeason(this.seasonName, this.seasonService.IsSeasonalsV3)
				// };
				// Wooroutine<SpriteManager> spriteManagerFlow = new BundledSpriteManagerLoaderFlow().Start(this.spriteManagerFlowParam);
				// yield return spriteManagerFlow;
				// yield return spriteManagerFlow.ReturnValue;
			}
			else
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0400620A RID: 25098
		// [WaitForService(true, true)]
		// private SeasonService seasonService;

		// Token: 0x0400620B RID: 25099
		private BundledSpriteManagerLoaderFlow.Input spriteManagerFlowParam;

		// Token: 0x0400620C RID: 25100
		private string seasonName;

		// Token: 0x0400620D RID: 25101
		private string bundleName;
	}
}
