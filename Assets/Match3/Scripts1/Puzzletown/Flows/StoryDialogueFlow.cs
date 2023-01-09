using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Town;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004CA RID: 1226
	public class StoryDialogueFlow : IBlocker
	{
		// Token: 0x0600224F RID: 8783 RVA: 0x00096820 File Offset: 0x00094C20
		public StoryDialogueFlow(DialogueSetupData dialogueSetupData)
		{
			this.setupData = dialogueSetupData;
			WoogaDebug.Log(new object[]
			{
				"StoryDialogueFlow",
				Time.frameCount
			});
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06002250 RID: 8784 RVA: 0x0009685A File Offset: 0x00094C5A
		public bool BlockInput
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06002251 RID: 8785 RVA: 0x0009685D File Offset: 0x00094C5D
		// (set) Token: 0x06002252 RID: 8786 RVA: 0x00096865 File Offset: 0x00094C65
		public bool isActive { get; private set; }

		// Token: 0x06002253 RID: 8787 RVA: 0x00096870 File Offset: 0x00094C70
		public IEnumerator ExecuteRoutine()
		{
			WoogaDebug.Log(new object[]
			{
				"ExecuteRoutine",
				Time.frameCount
			});
			Action closeAction = delegate()
			{
				this.OnUserOperation(PopupOperation.Close);
			};
			BackButtonManager.Instance.AddAction(closeAction);
			yield return ServiceLocator.Instance.Inject(this);
			if (this.gameState.Buildings.CurrentIsland != this.setupData.island_id)
			{
				yield return new SwitchIslandFlow().Start(new SwitchIslandFlow.SwitchIslandFlowData(this.setupData.island_id, false));
			}
			yield return SceneManager.Instance.Inject(this);
			while (BuildingLocation.Selected != null)
			{
				yield return null;
			}
			while (!Camera.main)
			{
				yield return null;
			}
			yield return new WaitForEndOfFrame();
			string[] names = this.setupData.Characters;
			if (names.IsNullOrEmptyCollection())
			{
				WoogaDebug.LogWarning(new object[]
				{
					"No names specified in",
					this.setupData.dialogue_id
				});
				yield break;
			}
			Vector3 location;
			// 对话时不聚焦相机到指定建筑上
			// if (!this.FindLocationByName(this.setupData.place, out location))
			// {
				// WoogaDebug.LogWarning(new object[]
				// {
					// "Could not find location for",
					// this.setupData.dialogue_id
				// });
				// yield break;
			// }
			VillagerSpawnConfiguration spawnPoints = this.villagers.storyController.FindSpawnConfiguration(names.Length);
			if (spawnPoints == null)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Could not find spawn points for",
					this.setupData.dialogue_id
				});
				yield break;
			}
			// VillagerView[] characters = new VillagerView[names.Length];
			// spawnPoints.SetLocation(location, true);
			// 不把精灵放到指定位置
			for (int i = 0; i < names.Length; i++)
			{
				// 审核版不显示精灵
				// #if !REVIEW_VERSION
				// {
					// VillagerView villagerView = this.villagers.CreateVillager(names[i]);
					// DedicatedPosition dedicatedPosition = spawnPoints.SpawnPoints[i];
					// villagerView.DedicatedPosition = dedicatedPosition;
					// characters[i] = villagerView;
				// }
				// #endif
			}
			// IEnumerable<IVisible> hiddenObjects = this.EnterStoryMode(location);
			IEnumerable<IVisible> hiddenObjects = this.EnterStoryMode(Vector3.zero);
			Vector3 oldCameraPosition = Camera.main.transform.position;
			Vector3 locationOffset = new Vector3(-1.5f, 0f, -1.5f);
			// 不移动相机
			// yield return this.PanAndZoom(location + locationOffset, CameraInputController.current.zoomLimit.x);
			DialogueDetailsData[] dialogue = this.config.storyDialogue.GetDialogue(this.setupData.dialogue_id);
			foreach (Area area in this.config.areas.areas)
			{
				foreach (AreaConfig areaConfig in area.levels)
				{
					if (areaConfig.wait_for_dialog == this.setupData.dialogue_id)
					{
						this.gameState.Quests.Quests.completedQuestDialogs.AddIfNotAlreadyPresent(this.setupData.dialogue_id, false);
						break;
					}
				}
			}
			this.TrackFunnelStart(this.setupData.dialogue_id);
			for (int idx = 0; idx < dialogue.Length; idx++)
			{
				if (!this.isActive)
				{
					break;
				}
				DialogueDetailsData line = dialogue[idx];
				// VillagerView speaker = Array.Find<VillagerView>(characters, (VillagerView c) => c.villagerName == line.character);
				int targetIndex = 0;
				for (int i = 0; i < names.Length; i++)
				{
					if (line.character == names[i])
						targetIndex = i;
				}
				// if (!speaker)
				// {
					// WoogaDebug.LogWarning(new object[]
					// {
						// "Could not find character",
						// line.character,
						// "in dialogue",
						// this.setupData.dialogue_id
					// });
				// }
				// else
				{
					if (!string.IsNullOrEmpty(line.change_anim))
					{
						// speaker.SetTrigger(line.change_anim);
					}
					if (!string.IsNullOrEmpty(line.sentence))
					{
						string localeKey = string.Format("dialogue.story.{0}.{1}", line.dialogue_id, line.sentence);
						string localeText = this.localization.GetText(localeKey, new LocaParam[0]);
						string localeName = this.localization.GetText("char_" + names[targetIndex].ToLower(), new LocaParam[0]);
						// 对话气泡的方向
						// SBOrientation orientation = spawnPoints.SpawnPoints[targetIndex].orientation;
						SBOrientation orientation = SBOrientation.Left; // 只使用left
						this.currentBubble = this.ui.FindStoryBubble(orientation);
						// CameraInputController.current.SetPositionAnchor(Vector3.Lerp(location, speaker.transform.position, 0.3f) + locationOffset);
						// yield return WooroutineRunner.StartCoroutine(this.currentBubble.Show(speaker, localeName, localeText, SBUsage.StoryDialogue, null), null);
						yield return WooroutineRunner.StartCoroutine(this.currentBubble.Show(null, localeName, localeText, SBUsage.StoryDialogue, null), null);
					}
				}
			}
			// CameraAfterStory cameraAfter = this.setupData.CameraAfter;
			// if (cameraAfter != CameraAfterStory.last_position)
			// {
			// 	if (cameraAfter == CameraAfterStory.no_action)
			// 	{
			// 		yield return this.PanAndZoom(CameraInputController.CameraPosition, CameraInputController.current.CamDistancePreferred);
			// 	}
			// }
			// else
			// {
			// 	yield return Camera.main.transform.DOMove(oldCameraPosition, 1f, false).WaitForCompletion();
			// }
			hiddenObjects.ForEach(delegate(IVisible obj)
			{
				obj.Show();
			});
			this.ui.inGameUi.SetVisible(true);
			this.ExitStoryMode();
			// using (IEnumerator<VillagerView> enumerator3 = this.villagers.Villagers.GetEnumerator())
			// {
			// 	while (enumerator3.MoveNext())
			// 	{
			// 		VillagerView v = enumerator3.Current;
			// 		v.isLocked = false;
			// 		v.transform.DOKill(true);
			// 		VillagerView villagerView2 = Array.Find<VillagerView>(characters, (VillagerView v2) => v2.villagerName == v.villagerName);
			// 		if (villagerView2)
			// 		{
			// 			v.transform.position = villagerView2.transform.position;
			// 			v.isFlipped = villagerView2.isFlipped;
			// 			villagerView2.gameObject.SetActive(false);
			// 		}
			// 	}
			// }
			// characters.ForEach(delegate(VillagerView c)
			// {
			// 	UnityEngine.Object.Destroy(c.gameObject);
			// });
			this.ui.overlay.enabled = false;
			BackButtonManager.Instance.RemoveAction(closeAction);
			this.onCompleted.Dispatch();
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x0009688C File Offset: 0x00094C8C
		private void TrackFunnelStart(string dialogueId)
		{
			string format = "{0}_story_{1}_start";
			int funnelId = this.GetFunnelId(dialogueId);
			if (funnelId != 0)
			{
				this.TrackFunnelEvent(funnelId, string.Format(format, funnelId, dialogueId));
			}
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x000968C4 File Offset: 0x00094CC4
		private void TrackFunnelSkip(string dialogueId)
		{
			string format = "{0}_story_{1}_skip";
			int num = this.GetFunnelId(dialogueId) + 1;
			if (num != 0)
			{
				this.TrackFunnelEvent(num, string.Format(format, num, dialogueId));
			}
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x000968FC File Offset: 0x00094CFC
		private int GetFunnelId(string dialogueId)
		{
			switch (dialogueId)
			{
			case "t1chp1_q1":
				return 95;
			case "t2chp1_q1":
				return 145;
			case "t3chp1_q1":
				return 275;
			case "ftchp1_q1":
				return 285;
			case "chp2_q1":
				return 295;
			case "t1chp2_q1":
				return 410;
			case "crabchp1_q1":
				return 455;
			case "t2chp2_q1":
				return 485;
			case "ftchp2_q1":
				return 495;
			case "chp3_q1":
				return 505;
			}
			return 0;
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x00096A1F File Offset: 0x00094E1F
		private void TrackFunnelEvent(int id, string token)
		{
			this.tracking.TrackFunnelEvent(token, id, null);
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x00096A30 File Offset: 0x00094E30
		private bool FindLocationByName(string name, out Vector3 location)
		{
			// eli todo 找不到指定建筑时程序会卡住，取消这一功能
			foreach (BuildingInstance buildingInstance in this.env.map.Buildings.Buildings)
			{
				if (!(buildingInstance.blueprint.name != name))
				{
					location = new Vector3((float)buildingInstance.position.x, 0f, (float)buildingInstance.position.y);
					return true;
				}
			}
			Log.Warning("BuildingMissing", string.Format("Could not find location {0} for story dialogue.", name), null);
			location = Vector3.zero;
			return false;
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x00096B04 File Offset: 0x00094F04
		private IEnumerator PanAndZoom(Vector3 location, float zoom)
		{
			Vector3 direction = Vector3.Normalize(Camera.main.transform.position - CameraInputController.CameraPosition);
			CameraInputController input = CameraInputController.current;
			input.enabled = false;
			yield return Camera.main.transform.DOMove(location + direction * zoom, 1f, false).WaitForCompletion();
			input.ResetVelocity();
			input.enabled = true;
			yield break;
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x00096B28 File Offset: 0x00094F28
		private IEnumerable<IVisible> EnterStoryMode(Vector3 location)
		{
			// 不隐藏建筑
			IEnumerable<IVisible> enumerable = new List<IVisible>();
			// if (this.config.FeatureSwitchesConfig.show_buildings_during_story)
			// {
				// List<IVisible> list = new List<IVisible>(100);
				// list.AddRange(this.villagers.Villagers.Cast<IVisible>());
				// foreach (BuildingInstance buildingInstance in this.env.map.Buildings.Buildings)
				// {
					// if (StoryDialogueFlow.ShouldHideBuilding(location, buildingInstance.view.transform))
					// {
					// 	list.Add(buildingInstance.view);
					// }
				// }
				// enumerable = list;
			// }
			// else
			// {
				// enumerable = this.villagers.Villagers.Cast<IVisible>();
			// }
			// enumerable.ForEach(delegate(IVisible obj)
			// {
			// 	obj.Hide();
			// });
			// this.ui.storySpeechBubbles.ForEach(delegate(UiSpeechBubble sb)
			// {
			// 	sb.Hide();
			// });
			this.ui.overlay.enabled = true;
			this.ui.blackBars.ShowOnChildren(PopupOperation.Close | PopupOperation.OK, true, true);
			this.ui.ShowBlackBars(true);
			this.ui.inGameUi.SetVisible(false);
			this.ui.onTap.AddListener(new Action<PopupOperation>(this.OnUserOperation));
			this.townUi.ShowUi(false);
			this.isActive = true;
			return enumerable;
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x00096CB8 File Offset: 0x000950B8
		private static bool ShouldHideBuilding(Vector3 location, Transform building)
		{
			return building.position.x + building.position.y < location.x + location.y - 2f;
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x00096CFC File Offset: 0x000950FC
		private void ExitStoryMode()
		{
			this.isActive = false;
			this.ui.ShowBlackBars(false);
			this.ui.blackBars.ShowOnChildren(PopupOperation.None, true, true);
			this.ui.onTap.RemoveListener(new Action<PopupOperation>(this.OnUserOperation));
			this.townUi.ShowUi(true);
			this.villagers.UpdateInteractionTime();
			this.villagers.storyController.onStoryFinished.Dispatch(this.setupData.dialogue_id);
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x00096D84 File Offset: 0x00095184
		private void OnUserOperation(PopupOperation op)
		{
			if (op != PopupOperation.Close)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Operation",
					op,
					"is not supported"
				});
			}
			else
			{
				this.isActive = false;
				this.TrackFunnelSkip(this.setupData.dialogue_id);
			}
			if (this.currentBubble)
			{
				this.currentBubble.Skip();
				this.currentBubble = null;
			}
		}

		// Token: 0x04004DB7 RID: 19895
		[WaitForRoot(false, false)]
		private TownEnvironmentRoot env;

		// Token: 0x04004DB8 RID: 19896
		[WaitForRoot(false, false)]
		private TownOverheadUiRoot ui;

		// Token: 0x04004DB9 RID: 19897
		[WaitForRoot(false, false)]
		private TownUiRoot townUi;

		// Token: 0x04004DBA RID: 19898
		[WaitForRoot(false, false)]
		private VillagersControllerRoot villagers;

		// Token: 0x04004DBB RID: 19899
		[WaitForService(true, true)]
		private TrackingService tracking;

		// Token: 0x04004DBC RID: 19900
		[WaitForService(true, true)]
		private ConfigService config;

		// Token: 0x04004DBD RID: 19901
		[WaitForService(true, true)]
		private ILocalizationService localization;

		// Token: 0x04004DBE RID: 19902
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x04004DBF RID: 19903
		public AwaitSignal onCompleted = new AwaitSignal();

		// Token: 0x04004DC0 RID: 19904
		private readonly DialogueSetupData setupData;

		// Token: 0x04004DC1 RID: 19905
		private const float CAMERA_PAN_TIME = 1f;

		// Token: 0x04004DC2 RID: 19906
		private const float SPEAKER_FOCUS_WEIGHT = 0.3f;

		// Token: 0x04004DC3 RID: 19907
		private UiSpeechBubble currentBubble;

		// Token: 0x04004DC4 RID: 19908
		private const float LOCATION_OFFSET = -1.5f;
	}
}
