using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x020009A3 RID: 2467
	[LoadOptions(true, true, true)]
	public class DialogueSwitcherRoot : APtSceneRoot, IHandler<string>
	{
		// Token: 0x06003BDA RID: 15322 RVA: 0x00129511 File Offset: 0x00127911
		public void HandleCancelTap()
		{
			this.onStateSelected.Dispatch("cancel");
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x00129523 File Offset: 0x00127923
		private void Start()
		{
			this.buttonCancel.onClick.AddListener(new UnityAction(this.HandleCancelTap));
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x00129541 File Offset: 0x00127941
		public static void Show()
		{
			WooroutineRunner.StartCoroutine(DialogueSwitcherRoot.ShowRoutine(), null);
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x00129550 File Offset: 0x00127950
		private static IEnumerator ShowRoutine()
		{
			Wooroutine<DialogueSwitcherRoot> scene = SceneManager.Instance.LoadScene<DialogueSwitcherRoot>(null);
			yield return scene;
			yield return scene.ReturnValue.ShowDialogueRoutine();
			scene.ReturnValue.Destroy();
			yield break;
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x00129564 File Offset: 0x00127964
		public IEnumerator ShowDialogueRoutine()
		{
			IEnumerable<string> names = from d in this.configs.storyDialogue.AllDialogues
			select d.dialogue_id;
			this.ShowDialogueButtons(names);
			yield return this.onStateSelected;
			string evt = this.onStateSelected.Dispatched;
			if (evt != "cancel")
			{
				yield return this.ShowSingleDialogueRoutine(evt);
			}
			else
			{
				WoogaDebug.Log(new object[]
				{
					"switch canceled"
				});
			}
			yield break;
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x00129580 File Offset: 0x00127980
		private IEnumerator ShowSingleDialogueRoutine(string id)
		{
			this.HideUi(true);
			StoryController sc = global::UnityEngine.Object.FindObjectOfType<StoryController>();
			IEnumerator sequence = sc.PlayStoryDialogImmediate(DialogueTrigger.unknown, id);
			yield return sequence;
			this.HideUi(false);
			yield break;
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x001295A2 File Offset: 0x001279A2
		private void HideUi(bool state)
		{
			base.transform.root.gameObject.SetActive(!state);
			this.townCheats.gameObject.SetActive(!state);
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x001295D1 File Offset: 0x001279D1
		private void ShowDialogueButtons(IEnumerable<string> names)
		{
			WoogaDebug.Log(new object[]
			{
				names
			});
			this.dataSource.Show(names);
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x001295EE File Offset: 0x001279EE
		public void Handle(string evt)
		{
			this.onStateSelected.Dispatch(evt);
		}

		// Token: 0x040063F2 RID: 25586
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x040063F3 RID: 25587
		[WaitForService(true, true)]
		private ConfigService configs;

		// Token: 0x040063F4 RID: 25588
		[WaitForRoot(false, false)]
		private TownCheatsRoot townCheats;

		// Token: 0x040063F5 RID: 25589
		private const string cancelEvent = "cancel";

		// Token: 0x040063F6 RID: 25590
		public GamestateDataSource dataSource;

		// Token: 0x040063F7 RID: 25591
		public Button buttonCancel;

		// Token: 0x040063F8 RID: 25592
		private AwaitSignal<string> onStateSelected = new AwaitSignal<string>();
	}
}
