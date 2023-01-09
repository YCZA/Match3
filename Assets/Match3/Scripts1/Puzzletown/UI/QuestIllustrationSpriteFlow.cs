using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Match3.Scripts1.Shared.ResourceManager;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009FF RID: 2559
	public class QuestIllustrationSpriteFlow : AFlowR<QuestData, Sprite>
	{
		// Token: 0x06003DA3 RID: 15779 RVA: 0x00137D38 File Offset: 0x00136138
		protected override IEnumerator FlowRoutine(QuestData questData)
		{
			yield return ServiceLocator.Instance.Inject(this);
			int chapter = this.configService.chapter.ChapterForLevel(questData.level);
			string path = string.Format("Assets/Puzzletown/Town/Ui/Art/Quests/QuestIllustrations/QuestIllustrationsChapter{0}/QuestIllustrationsChapter{0}.prefab", chapter);
			string bundleName = string.Format("quest_illustrations_chapter_{0}", chapter);
			Wooroutine<SpriteManager> spriteManagerFlow = new BundledSpriteManagerLoaderFlow().Start(new BundledSpriteManagerLoaderFlow.Input
			{
				bundleName = bundleName,
				path = path
			});
			yield return spriteManagerFlow;
			if (spriteManagerFlow.ReturnValue)
			{
				yield return spriteManagerFlow.ReturnValue.GetSimilar(questData.id);
			}
			else
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Chapter Illustrations missing: {0}",
					path
				});
				yield return null;
			}
			yield break;
		}

		// Token: 0x04006686 RID: 26246
		private const string PATH = "Assets/Puzzletown/Town/Ui/Art/Quests/QuestIllustrations/QuestIllustrationsChapter{0}/QuestIllustrationsChapter{0}.prefab";

		// Token: 0x04006687 RID: 26247
		private const string BUNDLE_NAME = "quest_illustrations_chapter_{0}";

		// Token: 0x04006688 RID: 26248
		[WaitForService(true, true)]
		private ConfigService configService;
	}
}
