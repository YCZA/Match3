using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009FC RID: 2556
	public class ChapterIntroIllustrationAssetFlow : AFlowR<int, ChapterIntroAssets>
	{
		// Token: 0x06003D9C RID: 15772 RVA: 0x001376AC File Offset: 0x00135AAC
		protected override IEnumerator FlowRoutine(int chapter)
		{
			yield return ServiceLocator.Instance.Inject(this);
			string path = string.Format("Assets/Puzzletown/Town/Ui/Art/Chapters/Chapter{0}/ChapterIntroAssets.asset", chapter);
			string bundleName = string.Format("chapter_intro_{0}", chapter);
			Wooroutine<bool> isAvailableRoutine = this.abs.IsBundleAvailable(bundleName);
			yield return isAvailableRoutine;
			bool isAvailable = false;
			try
			{
				isAvailable = isAvailableRoutine.ReturnValue;
			}
			catch (Exception ex)
			{
				WoogaDebug.Log(new object[]
				{
					ex.Message
				});
			}
			if (!isAvailable)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"not available",
					bundleName
				});
				yield return null;
				yield break;
			}
			Wooroutine<ChapterIntroAssets> assets = this.abs.LoadAsset<ChapterIntroAssets>(bundleName, path);
			yield return assets;
			ChapterIntroAssets bundledAssets = null;
			try
			{
				bundledAssets = assets.ReturnValue;
			}
			catch (Exception ex2)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"ChapterIntro: Exception: ",
					ex2.Message
				});
			}
			yield return bundledAssets;
			yield break;
		}

		// Token: 0x0400667F RID: 26239
		private const string PATH = "Assets/Puzzletown/Town/Ui/Art/Chapters/Chapter{0}/ChapterIntroAssets.asset";

		// Token: 0x04006680 RID: 26240
		private const string BUNDLE_NAME = "chapter_intro_{0}";

		// Token: 0x04006681 RID: 26241
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04006682 RID: 26242
		[WaitForService(true, true)]
		private AssetBundleService abs;
	}
}
