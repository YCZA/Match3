using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x020009A6 RID: 2470
	[LoadOptions(true, true, true)]
	public class GamestateSwitcherRoot : APtSceneRoot, IHandler<string>
	{
		// Token: 0x06003BE6 RID: 15334 RVA: 0x0012990D File Offset: 0x00127D0D
		public void HandleCancelTap()
		{
			this.onStateSelected.Dispatch("cancel");
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x0012991F File Offset: 0x00127D1F
		private void Start()
		{
			this.buttonCancel.onClick.AddListener(new UnityAction(this.HandleCancelTap));
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x0012993D File Offset: 0x00127D3D
		public static void Show()
		{
			WooroutineRunner.StartCoroutine(GamestateSwitcherRoot.ShowRoutine(), null);
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x0012994C File Offset: 0x00127D4C
		private static IEnumerator ShowRoutine()
		{
			Wooroutine<GamestateSwitcherRoot> scene = SceneManager.Instance.LoadScene<GamestateSwitcherRoot>(null);
			yield return scene;
			yield return scene.ReturnValue.SwitchGamestateRoutine();
			scene.ReturnValue.Destroy();
			yield break;
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x00129960 File Offset: 0x00127D60
		public IEnumerator SwitchGamestateRoutine()
		{
			Wooroutine<string[]> getPaths = this.abs.GetAssetNames("testing_gamestates");
			yield return getPaths;
			string[] paths = getPaths.ReturnValue;
			if (paths == null)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Couldn't load GameState bundles; are you offline?"
				});
				yield break;
			}
			IEnumerable<string> source = paths;
			if (GamestateSwitcherRoot._003C_003Ef__mg_0024cache0 == null)
			{
				GamestateSwitcherRoot._003C_003Ef__mg_0024cache0 = new Func<string, string>(Path.GetFileNameWithoutExtension);
			}
			string[] names = source.Select(GamestateSwitcherRoot._003C_003Ef__mg_0024cache0).ToArray<string>();
			this.ShowPopupSelectorButtons(names);
			yield return this.onStateSelected;
			string evt = this.onStateSelected.Dispatched;
			if (evt != "cancel")
			{
				string path = paths.First((string p) => Path.GetFileNameWithoutExtension(p) == evt);
				yield return this.DoSwitchRoutine(path);
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

		// Token: 0x06003BEB RID: 15339 RVA: 0x0012997C File Offset: 0x00127D7C
		private IEnumerator DoSwitchRoutine(string path)
		{
			WoogaDebug.Log(new object[]
			{
				"do the switch",
				path
			});
			Wooroutine<TextAsset> state = this.abs.LoadAsset<TextAsset>("testing_gamestates", path);
			yield return state;
			this.gameState.ForceGameState(state.ReturnValue.text);
			PTReloader.ReloadGame("Force applying test game state", false);
			yield break;
		}

		// Token: 0x06003BEC RID: 15340 RVA: 0x0012999E File Offset: 0x00127D9E
		private void ShowPopupSelectorButtons(string[] names)
		{
			WoogaDebug.Log(names);
			this.dataSource.Show(names);
		}

		// Token: 0x06003BED RID: 15341 RVA: 0x001299B2 File Offset: 0x00127DB2
		public void Handle(string evt)
		{
			this.onStateSelected.Dispatch(evt);
		}

		// Token: 0x040063F9 RID: 25593
		[WaitForService(true, true)]
		private AssetBundleService abs;

		// Token: 0x040063FA RID: 25594
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x040063FB RID: 25595
		private const string bundleName = "testing_gamestates";

		// Token: 0x040063FC RID: 25596
		private const string cancelEvent = "cancel";

		// Token: 0x040063FD RID: 25597
		public GamestateDataSource dataSource;

		// Token: 0x040063FE RID: 25598
		public Button buttonCancel;

		// Token: 0x040063FF RID: 25599
		private AwaitSignal<string> onStateSelected = new AwaitSignal<string>();

		// Token: 0x04006400 RID: 25600
		[CompilerGenerated]
		private static Func<string, string> _003C_003Ef__mg_0024cache0;
	}
}
