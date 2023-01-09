using System.Collections;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A02 RID: 2562
	public class ShowQuestPopupFlow : AFlow
	{
		// Token: 0x06003DAA RID: 15786 RVA: 0x00137F3C File Offset: 0x0013633C
		protected override IEnumerator FlowRoutine()
		{
			// eli key point 任务界面显示流
			yield return SceneManager.Instance.Inject(this);
			StartNewContentFlow newContent = new StartNewContentFlow(false, true);
			yield return newContent.ExecuteRoutine();
			Wooroutine<QuestsPopupRoot> popup = SceneManager.Instance.LoadScene<QuestsPopupRoot>(null);
			yield return popup;
			popup.ReturnValue.Show();
			AwaitSignal<QuestsPopupRoot.Output> result = popup.ReturnValue.onClose.Await<QuestsPopupRoot.Output>();
			yield return result;
			if (!result.Dispatched.isTaskCompleted)
			{
				QuestTaskType selectedTaskType = result.Dispatched.selectedTaskType;
				if (selectedTaskType != QuestTaskType.collect_and_interact && selectedTaskType != QuestTaskType.collect_and_repair)
				{
					if (selectedTaskType == QuestTaskType.reach_rank)
					{
						SceneManager.Instance.LoadScene<PopupHowToRankUpRoot>(null);
					}
				}
				else
				{
					new CoreGameFlow().Start(new CoreGameFlow.Input(result.Dispatched.selectedLevel, false, null, LevelPlayMode.Regular));
				}
			}
			else if (result.Dispatched.islandAction != null)
			{
				result.Dispatched.islandAction();
			}
			yield break;
		}

		// Token: 0x0400668A RID: 26250
		[WaitForRoot(false, false)]
		private TownUiRoot uiRoot;
	}
}
