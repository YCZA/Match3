using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A6F RID: 2671
	public class WorldMapRoot : APtSceneRoot<int>, IDisposableDialog, IHandler<PopupOperation>
	{
		// Token: 0x06003FF0 RID: 16368 RVA: 0x0014771C File Offset: 0x00145B1C
		protected override IEnumerator GoRoutine()
		{
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.swipePanel.onSwipe.AddListener(new Action<RelativeDirection>(this.HandleSwipe));
			this.leftArrow.onClick.AddListener(new UnityAction(this.HandleLeftButton));
			this.rightArrow.onClick.AddListener(new UnityAction(this.HandleRightButton));
			this.currentIsland = this.gameStateService.Buildings.CurrentIsland;
			if (base.registeredFirst)
			{
				this.currentIsland = this.mockCurrentIsland;
			}
			this.currentIndex = this.currentIsland;
			if (this.parameters >= 0)
			{
				this.currentIndex = this.parameters;
			}
			this.availableBundles = new HashSet<int>
			{
				0
			};
			yield return WooroutineRunner.StartCoroutine(this.WaitForBundleCheckRoutine(), null);
			this.SetupViews();
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.dialog.Show();
			this.AddSlowUpdate(new SlowUpdate(this.SlowUpdate), 1);
			yield break;
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x00147737 File Offset: 0x00145B37
		private void SlowUpdate()
		{
			base.StartCoroutine(this.WaitForBundleCheckRoutine());
		}

		// Token: 0x06003FF2 RID: 16370 RVA: 0x00147748 File Offset: 0x00145B48
		private IEnumerator WaitForBundleCheckRoutine()
		{
			bool viewsNeedUpdate = false;
			for (int i = 1; i < this.islandSprites.Count; i++)
			{
				string requiredIslandBundle = "scene_townenvironment_" + i;
				string requiredBuildingsBundle = "buildings_adventure_island_" + i.ToString("00");
				Wooroutine<bool> available = this.assetBundleService.AreAllBundlesAvailable(new List<string>
				{
					requiredIslandBundle,
					requiredBuildingsBundle
				});
				yield return available;
				if (available.ReturnValue)
				{
					viewsNeedUpdate = !this.availableBundles.Contains(i);
					this.availableBundles.Add(i);
				}
			}
			if (viewsNeedUpdate && this.islandViews != null && this.availableBundles != null)
			{
				foreach (IslandView islandView in this.islandViews)
				{
					islandView.RefreshBundleAvailability(this.availableBundles.Contains(islandView.islandId));
				}
			}
			yield break;
		}

		// Token: 0x06003FF3 RID: 16371 RVA: 0x00147764 File Offset: 0x00145B64
		private void SetupViews()
		{
			this.islandViews = new LinkedList<IslandView>();
			this.cloudsViews = new LinkedList<CloudsView>();
			this.CreateIslandView(this.currentIndex, SwipeableViewPosition.center);
			CloudsType cloudsType = CloudsType.center;
			if (this.currentIndex > 0)
			{
				this.CreateIslandView(this.currentIndex - 1, SwipeableViewPosition.left);
			}
			else
			{
				cloudsType = CloudsType.leftEnd;
			}
			if (this.currentIndex + 1 < this.islandSprites.Count)
			{
				this.CreateIslandView(this.currentIndex + 1, SwipeableViewPosition.right);
			}
			else
			{
				cloudsType = CloudsType.rightEnd;
			}
			if (this.currentIndex > 0)
			{
				this.leftArrow.gameObject.SetActive(true);
			}
			if (this.currentIndex < this.islandSprites.Count - 1)
			{
				this.rightArrow.gameObject.SetActive(true);
			}
			string text = "ui.worldmap.islandname." + this.currentIndex;
			this.islandNameLabel.text = this.localizationService.GetText(text, new LocaParam[0]);
			if (this.islandNameLabel.text == text)
			{
				this.islandNameLabel.text = this.localizationService.GetText(this.localizationService.GetText("ui.coming_soon", new LocaParam[0]), new LocaParam[0]);
			}
			this.islandInfoLabel.gameObject.SetActive(true);
			this.CreateCloudsView(SwipeableViewPosition.center, cloudsType);
			this.bottomPanel.DisableClouds();
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x001478D0 File Offset: 0x00145CD0
		private IslandView CreateIslandView(int islandId, SwipeableViewPosition position)
		{
			IslandView islandView = global::UnityEngine.Object.Instantiate<IslandView>(this.islandViewPrefab, this.islandViewsParent);
			bool lockStatus = !this.IsIslandUnlocked(islandId);
			bool bundleAvailable = this.availableBundles.Contains(islandId);
			DateTime? unlockDate = this.contentUnlockService.UnlockDateForIsland(islandId);
			islandView.SetupView(islandId, this.islandSprites[islandId], position, lockStatus, bundleAvailable, unlockDate, this.swipePanel, new WorldMapRoot.SelectionDelegate(this.HandleIslandSelection));
			this.InsertIntoLinkedList<IslandView>(islandView, position, this.islandViews);
			return islandView;
		}

		// Token: 0x06003FF5 RID: 16373 RVA: 0x0014794C File Offset: 0x00145D4C
		private CloudsView CreateCloudsView(SwipeableViewPosition position, CloudsType cloudsType)
		{
			CloudsView cloudsView = null;
			if (cloudsType != CloudsType.leftEnd)
			{
				if (cloudsType != CloudsType.center)
				{
					if (cloudsType == CloudsType.rightEnd)
					{
						cloudsView = global::UnityEngine.Object.Instantiate<CloudsView>(this.rightCloudViewPrefab, this.cloudViewParent);
					}
				}
				else
				{
					cloudsView = global::UnityEngine.Object.Instantiate<CloudsView>(this.centerCloudViewPrefab, this.cloudViewParent);
				}
			}
			else
			{
				cloudsView = global::UnityEngine.Object.Instantiate<CloudsView>(this.leftCloudViewPrefab, this.cloudViewParent);
			}
			if (cloudsView != null)
			{
				cloudsView.SetupView(position);
			}
			this.InsertIntoLinkedList<CloudsView>(cloudsView, position, this.cloudsViews);
			return cloudsView;
		}

		// Token: 0x06003FF6 RID: 16374 RVA: 0x001479DC File Offset: 0x00145DDC
		private void InsertIntoLinkedList<T>(T value, SwipeableViewPosition position, LinkedList<T> list) where T : ISwipeableView
		{
			LinkedListNode<T> linkedListNode = new LinkedListNode<T>(value);
			LinkedListNode<T> linkedListNode2 = list.First;
			if (linkedListNode2 == null)
			{
				list.AddFirst(linkedListNode);
			}
			while (linkedListNode2 != null)
			{
				T value2 = linkedListNode2.Value;
				if (position <= value2.currentPosition)
				{
					list.AddBefore(linkedListNode2, linkedListNode);
					break;
				}
				if (linkedListNode2.Next == null)
				{
					list.AddAfter(linkedListNode2, linkedListNode);
					break;
				}
				linkedListNode2 = linkedListNode2.Next;
			}
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x00147A55 File Offset: 0x00145E55
		private void HandleLeftButton()
		{
			this.HandleSwipe(RelativeDirection.Left);
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x00147A5E File Offset: 0x00145E5E
		private void HandleRightButton()
		{
			this.HandleSwipe(RelativeDirection.Right);
		}

		// Token: 0x06003FF9 RID: 16377 RVA: 0x00147A67 File Offset: 0x00145E67
		private void HandleSwipe(RelativeDirection direction)
		{
			if (direction == RelativeDirection.Left || direction == RelativeDirection.Right)
			{
				base.StartCoroutine(this.AnimateSwipeRoutine(direction));
			}
		}

		// Token: 0x06003FFA RID: 16378 RVA: 0x00147A90 File Offset: 0x00145E90
		private IEnumerator AnimateSwipeRoutine(RelativeDirection direction)
		{
			if (this.animating)
			{
				yield break;
			}
			bool animateRight = direction == RelativeDirection.Left;
			if (animateRight)
			{
				if (this.currentIndex <= 0)
				{
					yield break;
				}
				if (this.currentIndex - 1 <= 0)
				{
					this.leftArrow.gameObject.SetActive(false);
					this.CreateCloudsView(SwipeableViewPosition.left, CloudsType.leftEnd);
				}
				else
				{
					this.CreateCloudsView(SwipeableViewPosition.left, CloudsType.center);
				}
				if (this.currentIndex - 2 >= 0)
				{
					this.CreateIslandView(this.currentIndex - 2, SwipeableViewPosition.offscreenLeft);
				}
				this.rightArrow.gameObject.SetActive(true);
			}
			else
			{
				if (this.currentIndex + 1 >= this.islandSprites.Count)
				{
					this.rightArrow.gameObject.SetActive(false);
					yield break;
				}
				if (this.currentIndex + 2 < this.islandSprites.Count)
				{
					this.CreateIslandView(this.currentIndex + 2, SwipeableViewPosition.offscreenRight);
				}
				if (this.currentIndex + 2 >= this.islandSprites.Count)
				{
					this.rightArrow.gameObject.SetActive(false);
					this.CreateCloudsView(SwipeableViewPosition.right, CloudsType.rightEnd);
				}
				else
				{
					this.CreateCloudsView(SwipeableViewPosition.right, CloudsType.center);
				}
				this.leftArrow.gameObject.SetActive(true);
			}
			this.animating = true;
			foreach (IslandView islandView in this.islandViews)
			{
				islandView.AnimateView(direction);
			}
			foreach (CloudsView cloudsView in this.cloudsViews)
			{
				cloudsView.AnimateView(direction);
			}
			yield return new WaitForSeconds(this.islandViews.First.Value.AnimationTime);
			this.currentIndex = ((!animateRight) ? (this.currentIndex + 1) : (this.currentIndex - 1));
			if (animateRight && this.currentIndex < this.islandSprites.Count - 2)
			{
				global::UnityEngine.Object.Destroy(this.islandViews.Last.Value.gameObject);
				this.islandViews.RemoveLast();
			}
			else if (!animateRight && this.currentIndex > 1)
			{
				global::UnityEngine.Object.Destroy(this.islandViews.First.Value.gameObject);
				this.islandViews.RemoveFirst();
			}
			if (this.IsIslandUnlocked(this.currentIndex))
			{
				this.islandInfoLabel.gameObject.SetActive(true);
				string text = "ui.worldmap.islandname." + this.currentIndex;
				this.islandNameLabel.text = this.localizationService.GetText(text, new LocaParam[0]);
				if (this.islandNameLabel.text == text)
				{
					this.islandNameLabel.text = this.localizationService.GetText(this.localizationService.GetText("ui.coming_soon", new LocaParam[0]), new LocaParam[0]);
				}
			}
			else
			{
				this.islandNameLabel.text = this.localizationService.GetText(this.localizationService.GetText("ui.coming_soon", new LocaParam[0]), new LocaParam[0]);
				this.islandInfoLabel.gameObject.SetActive(false);
			}
			this.animating = false;
			if (this.cloudsViews.First.Value.currentPosition == SwipeableViewPosition.left)
			{
				global::UnityEngine.Object.Destroy(this.cloudsViews.First.Value.gameObject);
				this.cloudsViews.RemoveFirst();
			}
			if (this.cloudsViews.Last.Value.currentPosition == SwipeableViewPosition.right)
			{
				global::UnityEngine.Object.Destroy(this.cloudsViews.Last.Value.gameObject);
				this.cloudsViews.RemoveLast();
			}
			yield break;
		}

		// Token: 0x06003FFB RID: 16379 RVA: 0x00147AB4 File Offset: 0x00145EB4
		private bool IsIslandUnlocked(int islandId)
		{
			int lockedByContentIslandId = this.contentUnlockService.GetLockedByContentIslandId();
			if (lockedByContentIslandId > 0 && lockedByContentIslandId >= islandId)
			{
				return true;
			}
			bool flag = this.questService.IsIslandFirstQuestUnlocked(islandId);
			return this.gameStateService.Progression.LastUnlockedArea >= this.configService.SbsConfig.islandareaconfig.FirstGlobalArea(islandId) || flag;
		}

		// Token: 0x06003FFC RID: 16380 RVA: 0x00147B20 File Offset: 0x00145F20
		private void Close()
		{
			if (this.animating)
			{
				return;
			}
			this.animating = true;
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.dialog.Hide();
		}

		// Token: 0x06003FFD RID: 16381 RVA: 0x00147B56 File Offset: 0x00145F56
		public void Handle(PopupOperation operation)
		{
			if (operation == PopupOperation.Close)
			{
				this.Close();
			}
		}

		// Token: 0x06003FFE RID: 16382 RVA: 0x00147B70 File Offset: 0x00145F70
		private void HandleIslandSelection(int islandId)
		{
			if (this.IsIslandUnlocked(islandId) && this.availableBundles.Contains(islandId))
			{
				new SwitchIslandFlow().Start(new SwitchIslandFlow.SwitchIslandFlowData(islandId, true));
				if (this.currentIsland == islandId)
				{
					this.Close();
				}
			}
		}

		// Token: 0x0400699F RID: 27039
		private const string ISLAND_BUNDLE_PREFIX = "scene_townenvironment_";

		// Token: 0x040069A0 RID: 27040
		private const string ISLAND_BUILDING_PREFIX = "buildings_adventure_island_";

		// Token: 0x040069A1 RID: 27041
		private const string ISLAND_NAME_PREFIX = "ui.worldmap.islandname.";

		// Token: 0x040069A2 RID: 27042
		[WaitForRoot(false, false)]
		private TownBottomPanelRoot bottomPanel;

		// Token: 0x040069A3 RID: 27043
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040069A4 RID: 27044
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x040069A5 RID: 27045
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x040069A6 RID: 27046
		[WaitForService(true, true)]
		private AssetBundleService assetBundleService;

		// Token: 0x040069A7 RID: 27047
		[WaitForService(true, true)]
		private QuestService questService;

		// Token: 0x040069A8 RID: 27048
		[WaitForService(true, true)]
		private ContentUnlockService contentUnlockService;

		// Token: 0x040069A9 RID: 27049
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040069AA RID: 27050
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x040069AB RID: 27051
		[SerializeField]
		private List<Sprite> islandSprites;

		// Token: 0x040069AC RID: 27052
		[SerializeField]
		private SwipePanel swipePanel;

		// Token: 0x040069AD RID: 27053
		[SerializeField]
		private IslandView islandViewPrefab;

		// Token: 0x040069AE RID: 27054
		[SerializeField]
		private CloudsView leftCloudViewPrefab;

		// Token: 0x040069AF RID: 27055
		[SerializeField]
		private CloudsView centerCloudViewPrefab;

		// Token: 0x040069B0 RID: 27056
		[SerializeField]
		private CloudsView rightCloudViewPrefab;

		// Token: 0x040069B1 RID: 27057
		[SerializeField]
		private Transform islandViewsParent;

		// Token: 0x040069B2 RID: 27058
		[SerializeField]
		private Transform cloudViewParent;

		// Token: 0x040069B3 RID: 27059
		[SerializeField]
		private Button leftArrow;

		// Token: 0x040069B4 RID: 27060
		[SerializeField]
		private Button rightArrow;

		// Token: 0x040069B5 RID: 27061
		[SerializeField]
		private TextMeshProUGUI islandNameLabel;

		// Token: 0x040069B6 RID: 27062
		[SerializeField]
		private TextMeshProUGUI islandInfoLabel;

		// Token: 0x040069B7 RID: 27063
		[Header("Testing")]
		[SerializeField]
		private int mockCurrentIsland;

		// Token: 0x040069B8 RID: 27064
		private LinkedList<IslandView> islandViews;

		// Token: 0x040069B9 RID: 27065
		private LinkedList<CloudsView> cloudsViews;

		// Token: 0x040069BA RID: 27066
		private bool animating;

		// Token: 0x040069BB RID: 27067
		private int currentIsland;

		// Token: 0x040069BC RID: 27068
		private int currentIndex;

		// Token: 0x040069BD RID: 27069
		private HashSet<int> availableBundles;

		// Token: 0x02000A70 RID: 2672
		// (Invoke) Token: 0x06004000 RID: 16384
		public delegate void SelectionDelegate(int islandId);

		// Token: 0x02000A71 RID: 2673
		public class Trigger : PopupManager.Trigger
		{
			// Token: 0x06004003 RID: 16387 RVA: 0x00147BBE File Offset: 0x00145FBE
			public Trigger(ContentUnlockService contentUnlockService, TownBottomPanelRoot bottomPanel)
			{
				this.contentUnlockService = contentUnlockService;
				this.bottomPanel = bottomPanel;
			}

			// Token: 0x06004004 RID: 16388 RVA: 0x00147BD4 File Offset: 0x00145FD4
			public override bool ShouldTrigger()
			{
				this.islandToUnlock = this.contentUnlockService.GetLockedByContentIslandId();
				return this.islandToUnlock > 0;
			}

			// Token: 0x06004005 RID: 16389 RVA: 0x00147BF0 File Offset: 0x00145FF0
			public override IEnumerator Run()
			{
				this.bottomPanel.AnimatedClouds(true);
				Wooroutine<WorldMapRoot> scene = SceneManager.Instance.LoadSceneWithParams<WorldMapRoot, int>(this.islandToUnlock, null);
				yield return scene;
				yield return scene.ReturnValue.onDestroyed;
				yield break;
			}

			// Token: 0x040069BE RID: 27070
			private ContentUnlockService contentUnlockService;

			// Token: 0x040069BF RID: 27071
			private int islandToUnlock;

			// Token: 0x040069C0 RID: 27072
			private TownBottomPanelRoot bottomPanel;
		}
	}
}
