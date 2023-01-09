using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Town;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x020008CE RID: 2254
	public class VillagersControllerRoot : APtSceneRoot, IVillagerManager, IHandler<VillagerView>
	{
		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x060036C8 RID: 14024 RVA: 0x0010AF19 File Offset: 0x00109319
		public IEnumerable<VillagerView> Villagers
		{
			get
			{
				return this.villagers;
			}
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x0010AF21 File Offset: 0x00109321
		protected override void OnDestroy()
		{
			if (this._speechBubbleShown != null)
			{
				base.StopCoroutine(this._speechBubbleShown);
			}
			this._speechBubbleShown = null;
			this._villagersTalking.Clear();
			base.OnDestroy();
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x0010AF54 File Offset: 0x00109354
		public void Init()
		{
			int unlockedLevelWithQuestAndEndOfContent = this.quests.UnlockedLevelWithQuestAndEndOfContent;
			int islandId = this.environment.map.islandId;
			foreach (VillagerData villagerData in this.config.villager.characters)
			{
				if (this.IsCharacterAvailable(villagerData, unlockedLevelWithQuestAndEndOfContent, islandId))
				{
					// 审核版不显示精灵
					// #if !REVIEW_VERSION
					// {
						VillagerView villagerView = this.CreateVillager(villagerData.id, true);
						Vector2 zero = Vector2.zero;
						if (this.environment.map.FindEmptySpotInActiveAreas(out zero))
						{
							villagerView.transform.position = new Vector3(zero.x, 0f, zero.y);
						}
					// }
					// #endif
				}
			}
			this.storyController.Init(this.config, this.quests.questManager, this.progression);
			base.InvokeRepeating("MoveAround", (float)this.config.general.villager.seconds_to_trigger_start_roaming, (float)this.config.general.villager.seconds_to_trigger_start_roaming);
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x0010B064 File Offset: 0x00109464
		private bool IsCharacterAvailable(VillagerData data, int unlockedLevel, int islandId)
		{
			for (int i = 0; i < data.level_start.Length; i++)
			{
				int inclusiveLower = data.level_start[i];
				int exclusiveUpper = (i >= data.level_end.Length) ? int.MaxValue : data.level_end[i];
				if (unlockedLevel.IsBetween(inclusiveLower, exclusiveUpper) && data.island_id[i] == islandId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x0010B0D2 File Offset: 0x001094D2
		private void OnApplicationFocus(bool hasFocus)
		{
			if (hasFocus && base.OnInitialized.WasDispatched)
			{
				this.TriggerGreetings(VillagerPlayerTrigger.start);
			}
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x0010B0F1 File Offset: 0x001094F1
		public void HandleLevelExit(bool success)
		{
			this.TriggerGreetings((!success) ? VillagerPlayerTrigger.lose : VillagerPlayerTrigger.win);
		}

		// Token: 0x060036CE RID: 14030 RVA: 0x0010B108 File Offset: 0x00109508
		private void TriggerGreetings(VillagerPlayerTrigger trigger)
		{
			if (!this.gameState.GlobalReplaceKeys.Exists((GlobalReplaceKey kvp) => kvp.key == "{userName}"))
			{
				return;
			}
			if (trigger != VillagerPlayerTrigger.lose)
			{
				if (trigger != VillagerPlayerTrigger.win)
				{
					if (trigger == VillagerPlayerTrigger.start)
					{
						if (!this.CheckProbablity(this.config.general.villager.start_game_quote_probablity))
						{
							return;
						}
					}
				}
				else if (!this.CheckProbablity(this.config.general.villager.win_game_quote_probablity))
				{
					return;
				}
			}
			else if (!this.CheckProbablity(this.config.general.villager.lose_game_quote_probablity))
			{
				return;
			}
			if (this.isInteractionInProgress)
			{
				return;
			}
			VillagerPlayerData[] array = this.config.villagerBehavior.FindVillagerPlayer(trigger, this.progression.UnlockedLevel);
			if (array.IsNullOrEmptyCollection())
			{
				return;
			}
			VillagerView villagerView = this.FindGreetingVillager();
			if (!villagerView)
			{
				return;
			}
			this.WaitForUIAndTryStartCoroutine(this.TriggerGreetings(array, villagerView));
		}

		// Token: 0x060036CF RID: 14031 RVA: 0x0010B22C File Offset: 0x0010962C
		private IEnumerator TriggerGreetings(VillagerPlayerData[] data, VillagerView speaker)
		{
			if (this._speechBubbleShown != null)
			{
				yield break;
			}
			this.StartInteraction(speaker);
			speaker.SetTrigger("talkToPlayer");
			Debug.Log("triggerGreetings");
			foreach (VillagerPlayerData line in data)
			{
				string localeKey = string.Concat(new object[]
				{
					"dialogue.player.",
					line.interaction_id,
					".",
					line.sentence
				});
				Debug.Log("triggerGreetings： " + localeKey);
				string localeText = this.localization.GetText(localeKey, new LocaParam[0]);
				this._speechBubbleShown = this.WaitForUIAndTryStartCoroutine(this.ui.ingameSpeechBubble.Show(speaker, null, localeText, SBUsage.InGameDialogue, delegate()
				{
					this.FinishGreetings(false);
				}));
				yield return this._speechBubbleShown;
				this._speechBubbleShown = null;
			}
			this.FinishGreetings(false);
		}

		// Token: 0x060036D0 RID: 14032 RVA: 0x0010B258 File Offset: 0x00109658
		public void FinishGreetings(bool startMovingAfter)
		{
			if (this._speechBubbleShown != null)
			{
				base.StopCoroutine(this._speechBubbleShown);
			}
			this._speechBubbleShown = null;
			foreach (VillagerView villagerView in this._villagersTalking)
			{
				if (villagerView != null)
				{
					villagerView.SetTrigger("idle");
					this.FinishInteraction(villagerView, startMovingAfter);
				}
			}
			this._villagersTalking.Clear();
		}

		// Token: 0x060036D1 RID: 14033 RVA: 0x0010B2F8 File Offset: 0x001096F8
		private VillagerView FindGreetingVillager()
		{
			List<VillagerView> list = this.villagers.FindAll((VillagerView v) => !v.isLocked);
			if (list.IsNullOrEmptyCollection())
			{
				return null;
			}
			return list[global::UnityEngine.Random.Range(0, list.Count)];
		}

		// Token: 0x060036D2 RID: 14034 RVA: 0x0010B34D File Offset: 0x0010974D
		public VillagerView CreateVillager(string name)
		{
			return this.CreateVillager(name, false);
		}

		// Token: 0x060036D3 RID: 14035 RVA: 0x0010B358 File Offset: 0x00109758
		public VillagerView CreateVillager(string name, bool initialize)
		{
			VillagerView villagerView = global::UnityEngine.Object.Instantiate<VillagerView>(this.villagerManager.GetSimilar(name));
			villagerView.Init(name);
			if (initialize)
			{
				villagerView.onBuildingInteraction.AddListener(new Action<VillagerView, BuildingInstance>(this.OnBuildingInteraction));
				villagerView.onVillagerInteraction.AddListener(new Action<VillagerView, VillagerView, VillagerInteraction>(this.OnVillagerInteraction));
				villagerView.transform.SetParent(this.villagersParent, false);
				this.villagers.Add(villagerView);
			}
			return villagerView;
		}

		// Token: 0x060036D4 RID: 14036 RVA: 0x0010B3D4 File Offset: 0x001097D4
		private void MoveAround()
		{
			if (this.storyController.HasStoryRunning)
			{
				return;
			}
			List<VillagerView> list = this.villagers.FindAll((VillagerView c) => c.CanMove);
			if (!list.IsNullOrEmptyCollection())
			{
				this.MoveAround(list[global::UnityEngine.Random.Range(0, list.Count)]);
			}
		}

		// Token: 0x060036D5 RID: 14037 RVA: 0x0010B440 File Offset: 0x00109840
		private void MoveAround(VillagerView villager)
		{
			Vector2 zero = Vector2.zero;
			Vector2 input = new Vector2(villager.transform.position.x, villager.transform.position.z);
			if (this.environment.map.FindEmptySpotAroundPoint(input, 2f, 8f, out zero))
			{
				villager.MoveTo(zero, this.environment.map);
			}
		}

		// Token: 0x060036D6 RID: 14038 RVA: 0x0010B4B4 File Offset: 0x001098B4
		public void OnVillagerInteraction(VillagerView a, VillagerView b, VillagerInteraction interactionType)
		{
			if (this.storyController.HasStoryRunning || !a.CanInteract || !b.CanInteract || BuildingLocation.Selected != null)
			{
				return;
			}
			if (Time.time - this.lastInteraction < (float)this.config.general.villager.villager_interaction_cooldown_period)
			{
				return;
			}
			if (this.isInteractionInProgress)
			{
				return;
			}
			if (interactionType == VillagerInteraction.LookAtVillager)
			{
				// 村民/精灵相遇对话
				// this.WaitForUIAndTryStartCoroutine(this.TriggerDialogue(new VillagerView[]
				// {
				// 	a,
				// 	b
				// }));
			}
		}

		// Token: 0x060036D7 RID: 14039 RVA: 0x0010B554 File Offset: 0x00109954
		private Vector3 CalculateMiddlePoint(params VillagerView[] views)
		{
			Vector3 middle = Vector3.zero;
			if (views.Length == 0)
			{
				return middle;
			}
			views.ForEach(delegate(VillagerView view)
			{
				middle += view.transform.position;
			});
			return middle / (float)views.Length;
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x0010B5A4 File Offset: 0x001099A4
		private IEnumerator TriggerDialogue(params VillagerView[] views)
		{
			if (this._speechBubbleShown != null)
			{
				yield break;
			}
			VillagerSpawnConfiguration spawnconfig = this.storyController.FindSpawnConfiguration(views.Length);
			if (!spawnconfig)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Could not find spawn points for",
					views.Length,
					"characters"
				});
				yield break;
			}
			Vector3 middlePoint = this.CalculateMiddlePoint(views);
			spawnconfig.SetLocation(middlePoint, false);
			this._villagersTalking.Clear();
			views.ForEach(new Action<VillagerView>(this.StartInteraction));
			Vector3[] positions = Array.ConvertAll<DedicatedPosition, Vector3>(spawnconfig.SpawnPoints, (DedicatedPosition sp) => sp.transform.position);
			int i = 0;
			int num = views.Length;
			while (i < views.Length)
			{
				VillagerView villagerView = views[i];
				Array.Sort<Vector3>(positions, i, num, new VillagersControllerRoot.ClosestDistanceComparer(villagerView.transform.position));
				villagerView.MoveTo(new Vector2(positions[i].x, positions[i].z), this.environment.map);
				i++;
				num--;
			}
			WaitForSeconds iteratorPause = new WaitForSeconds(0.25f);
			for (;;)
			{
				if (Array.FindIndex<VillagerView>(views, (VillagerView view) => view.isMoving) == -1)
				{
					break;
				}
				yield return iteratorPause;
			}
			if (views.Length == 2)
			{
				VillagerView a = views[0];
				VillagerView b = views[1];
				VillagerVillagerData[] data = this.config.villagerBehavior.FindVillagerVillager(a.villagerName, b.villagerName, this.progression.UnlockedLevel);
				if (data == null)
				{
					views.ForEach(new Action<VillagerView>(this.FinishInteraction));
				}
				else if (Array.Find<VillagerVillagerData>(data, (VillagerVillagerData vv) => vv.VillagerLikesVillager(a.villagerName, b.villagerName)) != null)
				{
					yield return this.WaitForUIAndTryStartCoroutine(this.InteractionCoroutine(data, new VillagerView[]
					{
						a,
						b
					}));
				}
				else
				{
					yield return this.WaitForUIAndTryStartCoroutine(this.InteractionCoroutine(data, new VillagerView[]
					{
						b,
						a
					}));
				}
			}
			else
			{
				views.ForEach(new Action<VillagerView>(this.FinishInteraction));
			}
			yield break;
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x0010B5C8 File Offset: 0x001099C8
		public void OnBuildingInteraction(VillagerView view, BuildingInstance building)
		{
			if (!view.CanInteract || BuildingLocation.Selected != null)
			{
				return;
			}
			if (Time.time - this.lastInteraction < (float)this.config.general.villager.deco_interaction_cooldown_period)
			{
				return;
			}
			if (building.State != BuildingState.Active)
			{
				return;
			}
			if (this.isInteractionInProgress)
			{
				return;
			}
			VillagerData villagerData = Array.Find<VillagerData>(this.config.villager.characters, (VillagerData v) => v.id == view.villagerName);
			if (villagerData == null || villagerData.likes_deco_tag == null)
			{
				return;
			}
			if (Array.IndexOf<string>(villagerData.likes_deco_tag, building.blueprint.tag) != -1)
			{
				VillagerDecoData villagerDecoData = this.config.villagerBehavior.FindVillagerDeco(villagerData, building.blueprint, this.progression.UnlockedLevel);
				if (villagerDecoData != null)
				{
					this.WaitForUIAndTryStartCoroutine(this.InteractionCoroutine(view, building, villagerDecoData));
				}
				else
				{
					WoogaDebug.LogWarning(new object[]
					{
						"Can't find VillagedDecoData for",
						villagerData.id,
						"and",
						building.blueprint.name
					});
				}
			}
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x0010B704 File Offset: 0x00109B04
		private IEnumerator InteractionCoroutine(VillagerVillagerData[] data, params VillagerView[] views)
		{
			if (this._speechBubbleShown != null)
			{
				yield break;
			}
			this._villagersTalking.Clear();
			Vector3 middlePoint = this.CalculateMiddlePoint(views);
			views.ForEach(delegate(VillagerView view)
			{
				view.LookAt(middlePoint);
			});
			views.ForEach(new Action<VillagerView>(this.StartInteraction));
			for (int i = 0; i < data.Length; i++)
			{
				VillagerView speaker = views[data[i].Speaker];
				VillagerVillagerData line = data[i];
				string localeKey = string.Concat(new object[]
				{
					"dialogue.villager.",
					line.dialogue_id,
					".",
					line.sentence
				});
				Debug.LogError("interaction: " + localeKey);
				string localeText = this.localization.GetText(localeKey, new LocaParam[0]);
				this._speechBubbleShown = this.WaitForUIAndTryStartCoroutine(this.ui.ingameSpeechBubble.Show(speaker, null, localeText, SBUsage.InGameDialogue, delegate()
				{
					this.FinishGreetings(false);
				}));
				yield return this._speechBubbleShown;
				this._speechBubbleShown = null;
			}
			this.FinishGreetings(false);
			yield break;
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x0010B730 File Offset: 0x00109B30
		private IEnumerator InteractionCoroutine(VillagerView speaker, BuildingInstance building, VillagerDecoData data)
		{
			if (this._speechBubbleShown != null)
			{
				yield break;
			}
			this._villagersTalking.Clear();
			this.StartInteraction(speaker);
			speaker.SetTrigger(data.reaction);
			string localeKey = "dialogue.deco." + data.interaction_id;
			Debug.Log("deco:" + localeKey);
			string localeText = this.localization.GetText(localeKey, new LocaParam[0]);
			this._speechBubbleShown = this.WaitForUIAndTryStartCoroutine(this.ui.ingameSpeechBubble.Show(speaker, null, localeText, SBUsage.InGameDialogue, delegate()
			{
				this.FinishGreetings(true);
			}));
			yield return this._speechBubbleShown;
			this._speechBubbleShown = null;
			this.FinishGreetings(true);
			yield break;
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x0010B759 File Offset: 0x00109B59
		private void StartInteraction(VillagerView view)
		{
			view.isLocked = true;
			view.StopMovement();
			this.UpdateInteractionTime();
			if (!this._villagersTalking.Contains(view))
			{
				this._villagersTalking.Add(view);
			}
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x0010B78B File Offset: 0x00109B8B
		private void FinishInteraction(VillagerView view)
		{
			this.FinishInteraction(view, true);
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x0010B795 File Offset: 0x00109B95
		private void FinishInteraction(VillagerView view, bool startMoving)
		{
			view.isLocked = false;
			this.UpdateInteractionTime();
			if (startMoving)
			{
				this.MoveAround(view);
			}
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x0010B7B1 File Offset: 0x00109BB1
		public void UpdateInteractionTime()
		{
			this.lastInteraction = Time.time;
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x060036E0 RID: 14048 RVA: 0x0010B7C0 File Offset: 0x00109BC0
		private bool isInteractionInProgress
		{
			get
			{
				return this.villagers.Find((VillagerView v) => v.isLocked) || this.storyController.HasStoryRunning;
			}
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x0010B80D File Offset: 0x00109C0D
		private bool CheckProbablity(int value)
		{
			return global::UnityEngine.Random.Range(0, 100) < value;
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x0010B81C File Offset: 0x00109C1C
		public void Handle(VillagerView villager)
		{
			if (villager.isLocked)
			{
				if (this.ui.ingameSpeechBubble.speaker == villager)
				{
					this.ui.ingameSpeechBubble.Skip();
				}
				return;
			}
			VillagerPlayerData[] array = this.config.villagerBehavior.FindVillagerPlayer(VillagerPlayerTrigger.tap, villager.villagerName, this.progression.UnlockedLevel);
			if (array.IsNullOrEmptyCollection())
			{
				return;
			}
			this.WaitForUIAndTryStartCoroutine(this.TriggerGreetings(array, villager));
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x0010B89E File Offset: 0x00109C9E
		private Coroutine WaitForUIAndTryStartCoroutine(IEnumerator coroutine)
		{
			if (base.gameObject.activeInHierarchy)
			{
				return base.StartCoroutine(this.WaitForUIThenStartRoutine(coroutine));
			}
			WoogaDebug.LogWarning(new object[]
			{
				"Couldn't start coroutine because inactive- warning only"
			});
			return null;
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x0010B8D4 File Offset: 0x00109CD4
		private IEnumerator WaitForUIThenStartRoutine(IEnumerator coroutine)
		{
			while (this.ui == null || !this.ui.IsSetup)
			{
				yield return null;
			}
			yield return coroutine;
			yield break;
		}

		// Token: 0x04005EF9 RID: 24313
		private const float WANDER_MIN_RADIUS = 2f;

		// Token: 0x04005EFA RID: 24314
		private const float WANDER_MAX_RADIUS = 8f;

		// Token: 0x04005EFB RID: 24315
		private const float WANDER_LOCATION_TRIES = 50f;

		// Token: 0x04005EFC RID: 24316
		[WaitForRoot(false, false)]
		private TownOverheadUiRoot ui;

		// Token: 0x04005EFD RID: 24317
		[WaitForRoot(false, false)]
		private TownUiRoot townUi;

		// Token: 0x04005EFE RID: 24318
		[WaitForRoot(true, false)]
		private TownEnvironmentRoot environment;

		// Token: 0x04005EFF RID: 24319
		[WaitForService(true, true)]
		private QuestService quests;

		// Token: 0x04005F00 RID: 24320
		[WaitForService(true, true)]
		private ConfigService config;

		// Token: 0x04005F01 RID: 24321
		[WaitForService(true, true)]
		private ProgressionDataService.Service progression;

		// Token: 0x04005F02 RID: 24322
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x04005F03 RID: 24323
		[WaitForService(true, true)]
		private ILocalizationService localization;

		// Token: 0x04005F04 RID: 24324
		[WaitForService(true, true)]
		private TrackingService tracking;

		// Token: 0x04005F05 RID: 24325
		public VillagerManager villagerManager;

		// Token: 0x04005F06 RID: 24326
		public StoryController storyController;

		// Token: 0x04005F07 RID: 24327
		private List<VillagerView> villagers = new List<VillagerView>();

		// Token: 0x04005F08 RID: 24328
		private float lastInteraction;

		// Token: 0x04005F09 RID: 24329
		public Transform villagersParent;

		// Token: 0x04005F0A RID: 24330
		private Coroutine _speechBubbleShown;

		// Token: 0x04005F0B RID: 24331
		private List<VillagerView> _villagersTalking = new List<VillagerView>();

		// Token: 0x04005F0C RID: 24332
		private const int AVAILABLE_DIALOG_LOCATION_TRIES = 20;

		// Token: 0x020008CF RID: 2255
		private class ClosestDistanceComparer : IComparer<Vector3>
		{
			// Token: 0x060036E9 RID: 14057 RVA: 0x0010B923 File Offset: 0x00109D23
			public ClosestDistanceComparer(Vector3 origin)
			{
				this.origin = origin;
			}

			// Token: 0x060036EA RID: 14058 RVA: 0x0010B934 File Offset: 0x00109D34
			public int Compare(Vector3 a, Vector3 b)
			{
				return (a - this.origin).sqrMagnitude.CompareTo((b - this.origin).sqrMagnitude);
			}

			// Token: 0x04005F11 RID: 24337
			private Vector3 origin;
		}
	}
}
