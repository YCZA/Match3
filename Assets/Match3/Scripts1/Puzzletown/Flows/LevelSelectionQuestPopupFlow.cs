using System.Collections;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004BB RID: 1211
	public class LevelSelectionQuestPopupFlow : AFlowR<LevelSelectionFlow.Input, QuestsPopupRoot.Output>
	{
		// Token: 0x06002216 RID: 8726 RVA: 0x00093734 File Offset: 0x00091B34
		protected override IEnumerator FlowRoutine(LevelSelectionFlow.Input input)
		{
			if (SceneManager.IsLoadingScreenShown())
			{
				WoogaDebug.LogWarning(new object[]
				{
					"LevelSelectionFlow: Trying to show quests popup while on m3"
				});
				yield break;
			}
			Wooroutine<QuestsPopupRoot> popup = SceneManager.Instance.LoadScene<QuestsPopupRoot>(null);
			yield return popup;
			popup.ReturnValue.Show();
			AwaitSignal<QuestsPopupRoot.Output> sClose = popup.ReturnValue.onClose.Await<QuestsPopupRoot.Output>();
			yield return sClose;
			yield return sClose.Dispatched;
			yield break;
		}
	}
}
