using System;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.GameStateSelector
{
	// Token: 0x020004CF RID: 1231
	public class GameStateSelectorRoot : APtSceneRoot<MergeInfo, bool>
	{
		// Token: 0x06002267 RID: 8807 RVA: 0x00097D04 File Offset: 0x00096104
		protected override void Go()
		{
			if (base.registeredFirst)
			{
				GameState localState = JsonUtility.FromJson<GameState>(this.localStateJson.text);
				GameState serverState = JsonUtility.FromJson<GameState>(this.serverStateJson.text);
				this.parameters = new MergeInfo(localState, serverState);
			}
			this.localStateView = this.gameStateViewPrototype;
			this.serverStateView = global::UnityEngine.Object.Instantiate<GameStateView>(this.gameStateViewPrototype, this.localStateView.transform.parent);
			DateTime localDt = Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.parameters.localState.timestamp, DateTimeKind.Utc).ToLocalTime();
			DateTime serverDt = Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.parameters.serverState.timestamp, DateTimeKind.Utc).ToLocalTime();
			bool sameDay = localDt.Date == serverDt.Date;
			this.CheckInterval(localDt, serverDt);
			this.serverStateView.Show(this.loc.GetText("ui.gamestate.server.title", new LocaParam[0]), this.parameters.serverState, sameDay, true);
			this.localStateView.Show(this.loc.GetText("ui.gamestate.local.title", new LocaParam[0]), this.parameters.localState, sameDay, false);
			this.confirmButton.onClick.AddListener(delegate()
			{
				this.Close();
			});
			this.serverStateView.GetComponentInChildren<Button>().onClick.AddListener(delegate()
			{
				this.SelectGameState(this.parameters.serverState);
			});
			this.localStateView.GetComponentInChildren<Button>().onClick.AddListener(delegate()
			{
				this.SelectGameState(this.parameters.localState);
			});
			this.Refresh();
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x00097E98 File Offset: 0x00096298
		private void CheckInterval(DateTime localDt, DateTime serverDt)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["local date"] = localDt.ToString("F");
			dictionary["server date"] = serverDt.ToString("F");
			Log.Warning("MergeHandler", "Merge Popup was shown.", dictionary);
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x00097EE9 File Offset: 0x000962E9
		private void SelectGameState(GameState state)
		{
			this.selectedState = state;
			this.Refresh();
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x00097EF8 File Offset: 0x000962F8
		private void Refresh()
		{
			if (this.selectedState == null)
			{
				this.recommendedState = GameStateService.Max(this.parameters.localState, this.parameters.serverState, true);
				this.SelectGameState(this.recommendedState);
			}
			this.confirmButton.interactable = true;
			this.confirmButton.gameObject.SetActive(true);
			bool flag = this.selectedState == this.parameters.serverState;
			this.serverStateView.Highlight(flag);
			this.localStateView.Highlight(!flag);
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x00097F8C File Offset: 0x0009638C
		private void Close()
		{
			if (this.selectedState == null)
			{
				WoogaDebug.LogError(new object[]
				{
					"selected GameState is null! make sure the user chooses a GameState before closing"
				});
			}
			else
			{
				bool recommendedServer = this.recommendedState == this.parameters.serverState;
				bool flag = this.selectedState == this.parameters.serverState;
				this.trackingService.TrackGameStateSelection(recommendedServer, flag, this.parameters.serverState);
				this.onCompleted.Dispatch(flag);
				base.Destroy();
			}
		}

		// Token: 0x04004DD0 RID: 19920
		public TextAsset localStateJson;

		// Token: 0x04004DD1 RID: 19921
		public TextAsset serverStateJson;

		// Token: 0x04004DD2 RID: 19922
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04004DD3 RID: 19923
		[WaitForService(true, true)]
		private ILocalizationService loc;

		// Token: 0x04004DD4 RID: 19924
		[SerializeField]
		private GameStateView gameStateViewPrototype;

		// Token: 0x04004DD5 RID: 19925
		[SerializeField]
		private Button confirmButton;

		// Token: 0x04004DD6 RID: 19926
		private GameStateView localStateView;

		// Token: 0x04004DD7 RID: 19927
		private GameStateView serverStateView;

		// Token: 0x04004DD8 RID: 19928
		private GameState recommendedState;

		// Token: 0x04004DD9 RID: 19929
		private GameState selectedState;
	}
}
