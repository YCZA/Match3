using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;

// Token: 0x02000A68 RID: 2664
namespace Match3.Scripts1
{
	public class TropicamFlow
	{
		// Token: 0x06003FCE RID: 16334 RVA: 0x00146EDE File Offset: 0x001452DE
		public TropicamFlow(TownBottomPanelRoot bottomPanel, ProgressionDataService.Service progressionService)
		{
			this.bottomPanel = bottomPanel;
			this.progressionService = progressionService;
		}

		// Token: 0x06003FCF RID: 16335 RVA: 0x00146EF4 File Offset: 0x001452F4
		public IEnumerator Start()
		{
			// 屏蔽tropicam按钮的功能
			// return WooroutineRunner.StartWooroutine<object>(this.ExecuteRoutine());
			yield break;
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x00146F04 File Offset: 0x00145304
		private IEnumerator ExecuteRoutine()
		{
			this.MarkProgression();
			this.SwitchOffUiIndicator();
			yield return this.LoadSceneAndWaitUntilDisabled();
			yield break;
		}

		// Token: 0x06003FD1 RID: 16337 RVA: 0x00146F20 File Offset: 0x00145320
		private IEnumerator LoadSceneAndWaitUntilDisabled()
		{
			Wooroutine<TownTropicamUIRoot> tropicam = SceneManager.Instance.LoadScene<TownTropicamUIRoot>(null);
			yield return tropicam;
			yield return tropicam.ReturnValue.OnInitialized;
			tropicam.ReturnValue.Init();
			this.bottomPanel.Disable();
			yield return tropicam.ReturnValue.onDisabled.Await();
			this.bottomPanel.Enable();
			yield break;
		}

		// Token: 0x06003FD2 RID: 16338 RVA: 0x00146F3B File Offset: 0x0014533B
		private void MarkProgression()
		{
			this.progressionService.Data.HasOpenedTropicam = true;
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x00146F4E File Offset: 0x0014534E
		private void SwitchOffUiIndicator()
		{
			this.bottomPanel.SetTropicamIndicator(false);
		}

		// Token: 0x04006976 RID: 26998
		private readonly TownBottomPanelRoot bottomPanel;

		// Token: 0x04006977 RID: 26999
		private readonly ProgressionDataService.Service progressionService;
	}
}
