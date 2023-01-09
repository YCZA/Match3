using System;
using System.Collections;
using Match3.Scripts1.Puzzletown;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

// Token: 0x020009DA RID: 2522
namespace Match3.Scripts1
{
	public class VisitFriendFlow : AFlow<string>
	{
		// Token: 0x06003D12 RID: 15634 RVA: 0x001340F8 File Offset: 0x001324F8
		protected override IEnumerator FlowRoutine(string input)
		{
			this.friendID = input;
			if (VisitFriendFlow.flowRunning)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"FB friend visit flow already running."
				});
				yield break;
			}
			VisitFriendFlow.flowRunning = true;
			yield return ServiceLocator.Instance.Inject(this);
			this.sessionService.onRestart.AddListenerOnce(delegate
			{
				VisitFriendFlow.flowRunning = false;
			});
			this.gameStateService.Save(true);
			Wooroutine<SBSService.VillageStateWrapper> friendStateRoutine = WooroutineRunner.StartWooroutine<SBSService.VillageStateWrapper>(this.sbsService.doRetrieveFriendGameState(this.friendID));
			yield return friendStateRoutine;
			SBSService.VillageStateWrapper friendStateWrapper = null;
			try
			{
				friendStateWrapper = friendStateRoutine.ReturnValue;
			}
			catch (Exception ex)
			{
				WoogaDebug.LogWarningFormatted("FB friend visit: {0}", new object[]
				{
					ex.Message
				});
				VisitFriendFlow.flowRunning = false;
				yield break;
			}
			this.TryTrackFriendVisit(friendStateWrapper);
			if (friendStateWrapper != null && friendStateWrapper.gamestat != null && friendStateWrapper.gamestat.data != null)
			{
				friendStateWrapper.gamestat.data.buildings.CurrentIsland = 0;
			}
			if (this.TryLoadFriendOrEmptyState(friendStateWrapper))
			{
				yield return WooroutineRunner.StartCoroutine(this.ReloadTownForNewGameState(), null);
			}
			else
			{
				WoogaDebug.LogWarning(new object[]
				{
					"FB friend visit: couldn't load friend / empty state"
				});
			}
			VisitFriendFlow.flowRunning = false;
			yield break;
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x0013411A File Offset: 0x0013251A
		private void TryTrackFriendVisit(SBSService.VillageStateWrapper friendState)
		{
			if (friendState != null)
			{
				this.trackingService.TrackUi("friendlist", string.Empty, "visit_friend", friendState.user_id, new object[0]);
			}
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x00134148 File Offset: 0x00132548
		private bool TryLoadFriendOrEmptyState(SBSService.VillageStateWrapper friendState)
		{
			if (friendState != null && this.gameStateService.doLoadSpecificGameState(friendState.gamestat))
			{
				return true;
			}
			TextAsset textAsset = Resources.Load<TextAsset>("EmptyFriendState.json");
			GameStateService.GameStateData newState = JsonUtility.FromJson<GameStateService.GameStateData>(textAsset.text);
			if (this.gameStateService.doLoadSpecificGameState(newState))
			{
				WoogaDebug.Log(new object[]
				{
					"FB friend visit: loaded fallback gamestate"
				});
				return true;
			}
			return false;
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x001341B4 File Offset: 0x001325B4
		private IEnumerator ReloadTownForNewGameState()
		{
			TownLoadingFlowWithTransition.Input loadingScreenConfig = this.ConfigureLoadingScreen();
			Wooroutine<TownMainRoot> townLoadFlow = new TownLoadingFlowWithTransition().Start(loadingScreenConfig);
			yield return townLoadFlow;
			TownMainRoot townScene = townLoadFlow.ReturnValue;
			townScene.StartView(true, false);
			yield break;
		}

		// Token: 0x06003D16 RID: 15638 RVA: 0x001341CF File Offset: 0x001325CF
		private TownLoadingFlowWithTransition.Input ConfigureLoadingScreen()
		{
			return new TownLoadingFlowWithTransition.Input(LoadingScreenConfig.Random);
		}

		// Token: 0x040065BE RID: 26046
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x040065BF RID: 26047
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x040065C0 RID: 26048
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x040065C1 RID: 26049
		[WaitForService(true, true)]
		private SessionService sessionService;

		// Token: 0x040065C2 RID: 26050
		public static bool flowRunning;

		// Token: 0x040065C3 RID: 26051
		private string friendID;
	}
}
