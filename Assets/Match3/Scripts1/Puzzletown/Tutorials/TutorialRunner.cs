using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Shared.Pooling;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.Android.DataStatistics;
using Match3.Scripts2.PlayerData;
using UnityEngine; // using Firebase.Analytics;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000AA2 RID: 2722
	public class TutorialRunner : MonoBehaviour
	{
		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060040AC RID: 16556 RVA: 0x0014FA14 File Offset: 0x0014DE14
		public GameStateService GameStateService
		{
			get
			{
				return this.gameStateService;
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060040AD RID: 16557 RVA: 0x0014FA1C File Offset: 0x0014DE1C
		public bool IsRunning
		{
			get
			{
				return this.currentTutorial != null;
			}
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x0014FA2A File Offset: 0x0014DE2A
		private void OnEnable()
		{
			this.onMarkerEnabled.AddListener(new Action<string>(this.HandleMarkerEnabled));
		}

		// Token: 0x060040AF RID: 16559 RVA: 0x0014FA43 File Offset: 0x0014DE43
		private void LateUpdate()
		{
			if (this.IsRunning)
			{
				this.currentTutorial.steps[this.currentTutorial.index].Update();
			}
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x0014FA70 File Offset: 0x0014DE70
		public Coroutine Run(LevelConfig config)
		{
			this.config = config;
			return WooroutineRunner.StartCoroutine(this.RunRoutine(), null);
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x0014FA85 File Offset: 0x0014DE85
		public void Run()
		{
			base.StartCoroutine(this.RunRoutine());
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x0014FA94 File Offset: 0x0014DE94
		public void PlayTutorial(string id)
		{
			Tutorial item = this.list.tutorials.Find((Tutorial t) => t.name == id);
			this.queue.Add(item);
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x0014FAD7 File Offset: 0x0014DED7
		public void FinishCurrentTutorial()
		{
			if (this.currentTutorial != null)
			{
				this.currentTutorial.Finish();
				this.CompleteTutorial(this.currentTutorial);
			}
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x0014FB04 File Offset: 0x0014DF04
		private void HandleMarkerEnabled(string id)
		{
			if (!this.IsRunning && this.progression != null)
			{
				int unlockLevel = this.progression.UnlockedLevel;
				if (id == "DiveForTreasure")
				{
					unlockLevel = this.gameStateService.DiveForTreasure.Level;
				}
				Tutorial tutorial = this.list.tutorials.FirstOrDefault((Tutorial t) => t.trigger == Tutorial.Trigger.Marker && t.markerId == id && (t.level == unlockLevel || t.level == -1));
				if (tutorial)
				{
					this.Run(tutorial);
				}
			}
		}

		// Token: 0x060040B5 RID: 16565 RVA: 0x0014FBB0 File Offset: 0x0014DFB0
		private IEnumerator RunRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			yield return SceneManager.Instance.Inject(this);
			VillagersControllerRoot villagerRoot = global::UnityEngine.Object.FindObjectOfType<VillagersControllerRoot>();
			if (villagerRoot)
			{
				villagerRoot.storyController.onStoryFinished.AddListener(new Action<string>(this.HandleStoryFinished));
			}
			int level = this.progression.UnlockedLevel;
			if (TutorialHelper.IsMatch3 && (this.config.LevelCollectionConfig == null || this.config.LevelCollectionConfig.level != level || this.config.SelectedTier != AreaConfig.Tier.a))
			{
				this.EnableUserInput();
				yield break;
			}
			Tutorial tutorial;
			if (!this.progression.CurrentTutorial.IsNullOrEmpty())
			{
				tutorial = this.list.tutorials.FirstOrDefault((Tutorial t) => t.name == this.progression.CurrentTutorial);
			}
			else
			{
				tutorial = this.list.tutorials.FirstOrDefault((Tutorial t) => t.level == level && t.trigger == Tutorial.Trigger.Level);
			}
			if (tutorial && this.IsGameStateCompatibel(tutorial))
			{
				this.queue.Add(tutorial);
			}
			// eli key point 审核版本屏蔽关卡引导功能(因为有些关卡被去掉了，引导不完整，不如直接屏蔽引导功能)
			// #if REVIEW_VERSION
			// {
				// this.queue = new List<Tutorial>();
			// }
			// #endif
			while (this.queue.Count > 0)
			{
				Tutorial tut = this.queue.Pop<Tutorial>();
				WoogaDebug.Log(new object[]
				{
					"run next tutorial",
					tut.name
				});
				yield return this.Run(tut);
			}
			this.EnableUserInput();
			this.onInitialized.Dispatch();
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x0014FBCC File Offset: 0x0014DFCC
		private bool IsGameStateCompatibel(Tutorial tutorial)
		{
			// if (tutorial.name == "Tiers")
			// {
			// 	QuestProgress currentQuestProgress = this.quests.questManager.CurrentQuestProgress;
			// 	if (currentQuestProgress != null && currentQuestProgress.questID == "chp3_q1" && currentQuestProgress.configData.task_count[1] == 6)
			// 	{
			// 		return false;
			// 	}
			// }
			return true;
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x0014FC30 File Offset: 0x0014E030
		private void EnableUserInput()
		{
			EventSystemRoot.isUsedByTutorial = true;
			this.overlay.eventSystem.Enable();
			BackButtonManager.Instance.SetEnabled(true);
		}

		// Token: 0x060040B8 RID: 16568 RVA: 0x0014FC50 File Offset: 0x0014E050
		private void HandleStoryFinished(string id)
		{
			Tutorial tutorial = this.list.tutorials.FirstOrDefault((Tutorial t) => t.trigger == Tutorial.Trigger.Story && t.storyId == id);
			if (tutorial)
			{
				this.Run(tutorial);
			}
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x0014FC9A File Offset: 0x0014E09A
		private Coroutine Run(Tutorial tutorial)
		{
			if (this.progression.IsTutorialCompleted(tutorial.name))
			{
				return null;
			}
			Debug.Log("run tutorial");
			return WooroutineRunner.StartCoroutine(this.RunTutorialRoutine(tutorial), null);
		}

		private IEnumerator RunTutorialRoutine(Tutorial tutorial)
		{
			// eli key point 去掉所有引导
			// #if REVIEW_VERSION
				// yield break;
			// #endif
			
			RepairBuildingsByTagAction.RepairFlow.doPanning = false;
			VillagersControllerRoot villagersController = global::UnityEngine.Object.FindObjectOfType<VillagersControllerRoot>();
			if (villagersController != null)
			{
				foreach (VillagerView villagerView in villagersController.Villagers)
				{
					villagerView.isLocked = true;
				}
			}
			if (tutorial.trigger != Tutorial.Trigger.Marker)
			{
				this.gameStateService.Progression.CurrentTutorial = tutorial.name;
			}
			this.gameStateService.Save(true);
			this.gameStateService.savingBlocked = true;
			AUiAdjuster.SetOrientationLock(true);
			this.currentTutorial = tutorial;
			this.onInitialized.Dispatch();
			BackButtonManager.Instance.SetEnabled(false);
			// 开始引导
			// Debug.LogError("引导: 开始引导");
			yield return tutorial.Run(this.overlay, this);
			if (villagersController != null)
			{
				foreach (VillagerView villagerView2 in villagersController.Villagers)
				{
					villagerView2.isLocked = false;
				}
			}
			this.CompleteTutorial(tutorial);
		}

		public void CompleteTutorial(Tutorial tutorial)
		{
			this.currentTutorial = null;
			BackButtonManager.Instance.SetEnabled(true);
			if (!this.progression.IsTutorialCompleted(tutorial.name))
			{
				if (!tutorial.dontPersistCompletion)
				{
					this.progression.CompleteTutorial(tutorial.name);
				}
				this.gameStateService.Progression.CurrentTutorial = null;
				this.gameStateService.savingBlocked = false;
				AUiAdjuster.SetOrientationLock(false);
				this.gameStateService.Save(true);
				
				// buried point: 完成引导
				DataStatistics.Instance.TriggerTutorialComplete((progression.GetNumberOfTutorialCompleted() - 1));
			}
			RepairBuildingsByTagAction.RepairFlow.doPanning = true;
		}

		// Token: 0x04006A52 RID: 27218
		[WaitForService(true, true)]
		private ProgressionDataService.Service progression;

		// Token: 0x04006A53 RID: 27219
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04006A54 RID: 27220
		// [WaitForService(true, true)]
		// private QuestService quests;

		// Token: 0x04006A55 RID: 27221
		[WaitForRoot(true, false)]
		private TutorialOverlayRoot overlay;

		// Token: 0x04006A56 RID: 27222
		public readonly Signal<string> onMarkerEnabled = new Signal<string>();

		// Token: 0x04006A57 RID: 27223
		public readonly AwaitSignal onInitialized = new AwaitSignal();

		// Token: 0x04006A58 RID: 27224
		public TutorialList list;

		// Token: 0x04006A59 RID: 27225
		public Tutorial currentTutorial;

		// Token: 0x04006A5A RID: 27226
		private LevelConfig config;

		// Token: 0x04006A5B RID: 27227
		private List<Tutorial> queue = new List<Tutorial>();
	}
}
