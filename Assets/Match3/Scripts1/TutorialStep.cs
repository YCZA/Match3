using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Puzzletown.Tutorials;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.M3Engine;
using Match3.Scripts1.Town;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000AA7 RID: 2727
namespace Match3.Scripts1
{
	[Serializable]
	public class TutorialStep
	{
		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060040C2 RID: 16578 RVA: 0x0015047C File Offset: 0x0014E87C
		// (set) Token: 0x060040C3 RID: 16579 RVA: 0x00150484 File Offset: 0x0014E884
		public GameObject HighlightObject { get; private set; }

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060040C4 RID: 16580 RVA: 0x0015048D File Offset: 0x0014E88D
		private PTMatchEngine MatchEngine
		{
			get
			{
				return global::UnityEngine.Object.FindObjectOfType<LevelLoader>().MatchEngine;
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x060040C5 RID: 16581 RVA: 0x0015049C File Offset: 0x0014E89C
		public Padding StepPadding
		{
			get
			{
				Padding result = this.maskPadding;
				if (AUiAdjuster.SimilarOrientation == ScreenOrientation.LandscapeLeft)
				{
					bool flag = this.landscapeMaskPadding.top != 0;
					flag |= (this.landscapeMaskPadding.bottom != 0);
					flag |= (this.landscapeMaskPadding.left != 0);
					flag |= (this.landscapeMaskPadding.right != 0);
					if (flag)
					{
						result = this.landscapeMaskPadding;
					}
				}
				return result;
			}
		}

		// Token: 0x060040C6 RID: 16582 RVA: 0x00150518 File Offset: 0x0014E918
		public Coroutine Run(TutorialOverlayRoot overlay, string locaKey, Signal<string> onMarkerEnabled)
		{
			this.locaKey = locaKey;
			this.overlay = overlay;
			this.onMarkerEnabled = onMarkerEnabled;
			if (this.isTracked && !this.trackingEventId.IsNullOrEmpty())
			{
				// overlay.trackingService.TrackFunnelEvent(this.trackingEventId, this.funnelTrackingStep, null);
			}
			return WooroutineRunner.StartCoroutine(this.RunRoutine(), null);
		}

		// Token: 0x060040C7 RID: 16583 RVA: 0x0015057C File Offset: 0x0014E97C
		public void Update()
		{
			if (this._actDynamic && this.HighlightObject)
			{
				this.overlay.ShowObject(this.HighlightObject, this.StepPadding, false, true);
			}
			if (this.script != null)
			{
				this.script.Tick();
			}
		}

		// Token: 0x060040C8 RID: 16584 RVA: 0x001505DC File Offset: 0x0014E9DC
		private IEnumerator WaitForHighlightObjects()
		{
			this.overlay.AddClearBlockingOverlay();
			while (!this.HighlightObject)
			{
				if (this.highlightTaggedObject)
				{
					this.HighlightObject = GameObject.FindGameObjectWithTag(this.highlightTag);
					if (this.HighlightObject != null && this.highlightTag == "Tutorial - Town - Quest - Celebrate")
					{
						AnimatedUi animatedUI = this.HighlightObject.GetComponentInParent<AnimatedUi>();
						if (animatedUI != null)
						{
							yield return new WaitForSeconds(animatedUI.open.length);
						}
					}
				}
				else if (this.highlightQuestObject)
				{
					QuestTasksDataSource currentQuestView = global::UnityEngine.Object.FindObjectOfType<QuestTasksDataSource>();
					if (currentQuestView != null)
					{
						AnimatedUi animatedUI2 = currentQuestView.GetComponentInParent<AnimatedUi>();
						if (animatedUI2 != null)
						{
							yield return new WaitForSeconds(animatedUI2.open.length);
						}
						int activeChildIndex = 0;
						IEnumerator enumerator = currentQuestView.transform.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								Transform transform = (Transform)obj;
								if (transform.gameObject.activeSelf)
								{
									if (activeChildIndex == this.questTaskIndex)
									{
										QuestTaskProgressView component = transform.GetComponent<QuestTaskProgressView>();
										if (component != null)
										{
											this.HighlightObject = component.button.gameObject;
											break;
										}
									}
									activeChildIndex++;
								}
							}
						}
						finally
						{
							IDisposable disposable;
							if ((disposable = (enumerator as IDisposable)) != null)
							{
								disposable.Dispose();
							}
						}
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060040C9 RID: 16585 RVA: 0x001505F8 File Offset: 0x0014E9F8
		private IEnumerator RunRoutine()
		{
			Debug.Log("RunRoutine");
			while (BlockerManager.global.HasBlockers && !this.ingoreBlockerManager)
			{
				yield return null;
			}
			EventSystemRoot.isUsedByTutorial = false;
			this.overlay.eventSystem.Disable();
			if (this.addBlockingOverlay)
			{
				this.overlay.AddClearBlockingOverlay();
			}
			EventProxy eventProxy = this.overlay.GetComponentInChildren<EventProxy>(true);
			if (eventProxy != null)
			{
				eventProxy.altInputMode = this.altInputFlag;
			}
			if (this.highlightQuestObject || this.highlightTaggedObject)
			{
				if (this.highlightTaggedObject)
				{
					this.HighlightObject = GameObject.FindGameObjectWithTag(this.highlightTag);
				}
				if (this.HighlightObject == null)
				{
					yield return this.WaitForHighlightObjects();
				}
			}
			if (this.shouldOverrideTextKey)
			{
				this.locaKey = this.overrideTextKey;
			}
			this.overlay.ShowText(this, this.locaKey);
			this.overlay.PrepareOverlay(this);
			bool isAnimated = (this.showAnimation == TutorialStep.AnimationOptions.Show || this.showAnimation == TutorialStep.AnimationOptions.ShowAndHide) && !this.hideOverlay;
			this._actDynamic = this.dynamicHighlight;
			switch (this.requiredAction)
			{
				case TutorialStep.Action.TapScreen:
					this.overlay.ShowLayout(this, isAnimated);
					yield return this.overlay.onScreenClicked.Await();
					this.overlay.audioService.PlaySFX(AudioId.TutorialTapScreen, false, false, false);
					break;
				case TutorialStep.Action.TapButton:
					this.overlay.ShowLayout(this, isAnimated);
					yield return this.overlay.onButtonClicked.Await();
					break;
				case TutorialStep.Action.Move:
					this.overlay.ShowFields(this.gameboard, this.highlightMove, this.showHand);
					yield return this.AwaitValidMove();
					this.overlay.ResetExpectedMove();
					this._actDynamic = false;
					this.overlay.HideImmediately();
					yield return this.overlay.boardView.onAnimationFinished.Await();
					break;
				case TutorialStep.Action.ShowBoost:
					yield return WooroutineRunner.StartCoroutine(this.ShowBoostRoutine(this.boost), null);
					break;
				case TutorialStep.Action.ApplyBoost:
					this.overlay.ShowFields(this.gameboard, this.highlightMove, false);
					yield return this.AwaitValidMove();
					break;
				case TutorialStep.Action.InterceptButton:
					this.overlay.ShowLayout(this, isAnimated);
					yield return this.overlay.onButtonClicked.Await();
					break;
				case TutorialStep.Action.Script:
				{
					bool backButtonEnabled = BackButtonManager.Instance.IsEnabled;
					BackButtonManager.Instance.SetEnabled(false);
					if (isAnimated)
					{
						this.overlay.ShowLayout(this, false);
					}
					else if (!this.addBlockingOverlay)
					{
						EventSystemRoot.isUsedByTutorial = true;
						this.overlay.eventSystem.Enable();
					}
					yield return this.script.Execute(this.overlay, this);
					BackButtonManager.Instance.SetEnabled(backButtonEnabled);
					break;
				}
				case TutorialStep.Action.WaitForMarker:
				{
					string id = string.Empty;
					while (id != this.markerId)
					{
						AwaitSignal<string> sig = this.onMarkerEnabled.Await<string>();
						yield return sig;
						id = sig.Dispatched;
					}
					break;
				}
				case TutorialStep.Action.ZoomToBuilding:
				{
					BuildingInstance building = this.FindBuilding();
					yield return this.overlay.PanToPosition(building.view.FocusPosition);
					break;
				}
				case TutorialStep.Action.AnyMove:
					if (this.highlightMove)
					{
						this.overlay.boardView.TimeSinceLastValidSwap = 7f;
					}
					break;
			}
			if (this.overlay && this.overlay.gameObject.activeInHierarchy)
			{
				if (this.showAnimation == TutorialStep.AnimationOptions.Hide || this.showAnimation == TutorialStep.AnimationOptions.ShowAndHide || this.fadeOutAnimation)
				{
					yield return this.overlay.HideAnimated();
				}
				else
				{
					this._actDynamic = false;
					this.overlay.HideImmediately();
				}
			}
			this.HighlightObject = null;
		}

		// Token: 0x060040CA RID: 16586 RVA: 0x00150614 File Offset: 0x0014EA14
		private BuildingInstance FindBuilding()
		{
			BuildingsController buildingsController = global::UnityEngine.Object.FindObjectOfType<TownMainRoot>().BuildingsController;
			return buildingsController.Buildings.First((BuildingInstance b) => b.blueprint.name == this.buildingId);
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x00150643 File Offset: 0x0014EA43
		private Coroutine AwaitValidMove()
		{
			return WooroutineRunner.StartCoroutine(this.AwaitValidMoveRoutine(), null);
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x00150654 File Offset: 0x0014EA54
		private IEnumerator AwaitValidMoveRoutine()
		{
			AwaitSignal<List<List<IMatchResult>>> onCompleted = null;
			do
			{
				onCompleted = this.MatchEngine.onStepCompleted.Await<List<List<IMatchResult>>>();
				yield return onCompleted;
			}
			while (BoardView.IsInvalidMove(onCompleted.Dispatched));
			yield break;
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x00150670 File Offset: 0x0014EA70
		private IEnumerator ShowBoostRoutine(Boosts type)
		{
			BoostView view = global::UnityEngine.Object.FindObjectsOfType<BoostView>().First((BoostView v) => v.Type == type);
			BoostOperationButton button = view.GetComponentsInChildren<BoostOperationButton>().First((BoostOperationButton b) => b.operation == BoostOperation.Use);
			this.overlay.SetHighlightType(this.roundHighlight);
			this._actDynamic = true;
			this.HighlightObject = button.gameObject;
			this.overlay.ShowObject(button.gameObject, this.StepPadding, false, false);
			this.overlay.SetClickMaskRedirecting(true);
			yield return this.overlay.onButtonClicked.Await();
			yield break;
		}

		// Token: 0x04006A6D RID: 27245
		public bool hideOverlay;

		// Token: 0x04006A6E RID: 27246
		public bool isTracked;

		// Token: 0x04006A6F RID: 27247
		public string trackingEventId;

		// Token: 0x04006A70 RID: 27248
		public int funnelTrackingStep;

		// Token: 0x04006A71 RID: 27249
		public bool altInputFlag;

		// Token: 0x04006A72 RID: 27250
		public TutorialStep.AnimationOptions showAnimation;

		// Token: 0x04006A73 RID: 27251
		public bool fadeOutAnimation;

		// Token: 0x04006A74 RID: 27252
		public bool showText;

		// Token: 0x04006A75 RID: 27253
		public TutorialSpeechBubble.TutorialCharacter character;

		// Token: 0x04006A76 RID: 27254
		public TutorialStepLayout overlayLayout;

		// Token: 0x04006A77 RID: 27255
		public TutorialStepLayout landscapeOverlayLayout;

		// Token: 0x04006A78 RID: 27256
		public TutorialStep.Action requiredAction;

		// Token: 0x04006A79 RID: 27257
		public ATutorialScript script;

		// Token: 0x04006A7A RID: 27258
		public Padding maskPadding;

		// Token: 0x04006A7B RID: 27259
		public Padding landscapeMaskPadding;

		// Token: 0x04006A7C RID: 27260
		public bool highlightTaggedObject;

		// Token: 0x04006A7D RID: 27261
		public bool highlightQuestObject;

		// Token: 0x04006A7E RID: 27262
		public bool forceHighlight;

		// Token: 0x04006A7F RID: 27263
		public bool dynamicHighlight;

		// Token: 0x04006A80 RID: 27264
		public float clickMaskPositionDelay;

		// Token: 0x04006A81 RID: 27265
		public string highlightTag;

		// Token: 0x04006A82 RID: 27266
		public int questTaskIndex;

		// Token: 0x04006A83 RID: 27267
		public ArrowPosition arrowPosition;

		// Token: 0x04006A84 RID: 27268
		public bool showHand;

		// Token: 0x04006A85 RID: 27269
		public bool ingoreBlockerManager;

		// Token: 0x04006A86 RID: 27270
		public bool addBlockingOverlay;

		// Token: 0x04006A87 RID: 27271
		public bool iconInTextBox;

		// Token: 0x04006A88 RID: 27272
		public Sprite iconSprite;

		// Token: 0x04006A89 RID: 27273
		public bool leftBubbleIcon;

		// Token: 0x04006A8A RID: 27274
		public bool roundHighlight;

		// Token: 0x04006A8B RID: 27275
		public bool manualClickMaskPlacement;

		// Token: 0x04006A8C RID: 27276
		public bool shouldOverrideTextKey;

		// Token: 0x04006A8D RID: 27277
		public bool highlightMove;

		// Token: 0x04006A8E RID: 27278
		public bool forceTownUI;

		// Token: 0x04006A8F RID: 27279
		public string overrideTextKey;

		// Token: 0x04006A90 RID: 27280
		[FormerlySerializedAs("speachBubbleAnimation")]
		public TutorialSpeechBubble.SpeechBubleAnimation speechBubbleAnimation;

		// Token: 0x04006A91 RID: 27281
		[TutorialArray]
		public TutorialStep.ArrayWrapper gameboard;

		// Token: 0x04006A92 RID: 27282
		public Boosts boost;

		// Token: 0x04006A93 RID: 27283
		public string buildingId;

		// Token: 0x04006A94 RID: 27284
		public string markerId;

		// Token: 0x04006A95 RID: 27285
		[NonSerialized]
		private bool _actDynamic;

		// Token: 0x04006A96 RID: 27286
		private TutorialOverlayRoot overlay;

		// Token: 0x04006A97 RID: 27287
		private string locaKey;

		// Token: 0x04006A98 RID: 27288
		private Signal<string> onMarkerEnabled;

		// Token: 0x04006A99 RID: 27289
		private const string CELEBRATE_TAG = "Tutorial - Town - Quest - Celebrate";

		// Token: 0x02000AA8 RID: 2728
		public enum Action
		{
			// Token: 0x04006A9C RID: 27292
			TapScreen,
			// Token: 0x04006A9D RID: 27293
			TapButton,
			// Token: 0x04006A9E RID: 27294
			Move,
			// Token: 0x04006A9F RID: 27295
			ShowBoost,
			// Token: 0x04006AA0 RID: 27296
			ApplyBoost,
			// Token: 0x04006AA1 RID: 27297
			InterceptButton,
			// Token: 0x04006AA2 RID: 27298
			Script,
			// Token: 0x04006AA3 RID: 27299
			WaitForMarker,
			// Token: 0x04006AA4 RID: 27300
			ZoomToUnlockedTemple,
			// Token: 0x04006AA5 RID: 27301
			ZoomToBuilding,
			// Token: 0x04006AA6 RID: 27302
			AnyMove
		}

		// Token: 0x02000AA9 RID: 2729
		public enum AnimationOptions
		{
			// Token: 0x04006AA8 RID: 27304
			None,
			// Token: 0x04006AA9 RID: 27305
			Show,
			// Token: 0x04006AAA RID: 27306
			Hide,
			// Token: 0x04006AAB RID: 27307
			ShowAndHide,
			// Token: 0x04006AAC RID: 27308
			ShowWithShrinkingEffect,
			// Token: 0x04006AAD RID: 27309
			ShowWithShrinkingEffectFadeIn,
			// Token: 0x04006AAE RID: 27310
			FadeIn,
			// Token: 0x04006AAF RID: 27311
			FadeOut
		}

		// Token: 0x02000AAA RID: 2730
		[Serializable]
		public class ArrayWrapper
		{
			// Token: 0x060040D0 RID: 16592 RVA: 0x001506BF File Offset: 0x0014EABF
			public static implicit operator int[](TutorialStep.ArrayWrapper wrapper)
			{
				return wrapper.arr;
			}

			// Token: 0x04006AB0 RID: 27312
			[SerializeField]
			private int[] arr = new int[81];
		}
	}
}
