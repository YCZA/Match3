using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A90 RID: 2704
	public class TutorialOverlayRoot : APtSceneRoot
	{
		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06004050 RID: 16464 RVA: 0x0014BAEF File Offset: 0x00149EEF
		// (set) Token: 0x06004051 RID: 16465 RVA: 0x0014BAF7 File Offset: 0x00149EF7
		public Color backgroundColor { get; private set; }

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06004052 RID: 16466 RVA: 0x0014BB00 File Offset: 0x00149F00
		public BoardView boardView
		{
			get
			{
				return global::UnityEngine.Object.FindObjectOfType<BoardView>();
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06004053 RID: 16467 RVA: 0x0014BB07 File Offset: 0x00149F07
		private bool IsMatch3
		{
			get
			{
				return SceneManager.Instance.Has(typeof(M3_LevelRoot), false);
			}
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x0014BB1E File Offset: 0x00149F1E
		private void OnValidate()
		{
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x0014BB20 File Offset: 0x00149F20
		private void Start()
		{
			this.backgroundColor = this.backgroundImage.color;
			base.GetComponentInChildren<Button>(true).onClick.AddListener(new UnityAction(this.onScreenClicked.Dispatch));
			this.maskClick.GetComponent<EventProxy>().onClicked.AddListener(new Action(this.onButtonClicked.Dispatch));
			this.tutorialArrow.Init();
			this.tutorialHand.Init();
			base.Disable();
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x0014BBA2 File Offset: 0x00149FA2
		protected override void Go()
		{
			// Debug.LogError("引导: 启动引导");
			EventSystemRoot.isUsedByTutorial = false;
			this.eventSystem.Disable();
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x0014BBB0 File Offset: 0x00149FB0
		public void PrepareOverlay(TutorialStep step)
		{
			this.tutorialArrow.Reset(step);
			this.tutorialHand.Reset(step);
			this.squareHighlight.gameObject.SetActive(false);
			this.roundHighlight.gameObject.SetActive(false);
			this.maskClick.localScale = Vector3.one;
			this.tutorialArrow.transform.localScale = Vector3.one;
			this.maskHighlight.localScale = Vector3.one;
			Image component = this.maskHighlight.GetComponent<Image>();
			component.color = this.backgroundColor;
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x0014BC44 File Offset: 0x0014A044
		public void ShowLayout(TutorialStep step, bool animate)
		{
			TutorialStepLayout tutorialStepLayout = step.overlayLayout;
			if (AUiAdjuster.SimilarOrientation == ScreenOrientation.LandscapeLeft && step.landscapeOverlayLayout != null)
			{
				tutorialStepLayout = step.landscapeOverlayLayout;
			}
			this.SetHighlightType(step.roundHighlight);
			bool flag = step.requiredAction == TutorialStep.Action.InterceptButton;
			bool flag2 = step.requiredAction == TutorialStep.Action.TapScreen;
			this.SetClickMaskRedirecting(!flag);
			if (animate)
			{
				this.Show(true);
			}
			else
			{
				this.ShowOverlay(true);
			}
			this.ActivateLayout(step.highlightTaggedObject || step.highlightQuestObject || step.forceHighlight, step.clickMaskPositionDelay);
			bool flag3 = false;
			if (step.showText)
			{
				this.speechBubble.Info = tutorialStepLayout;
			}
			if (step.highlightTaggedObject || step.highlightQuestObject || step.forceHighlight)
			{
				if (!step.forceHighlight)
				{
					this.HighlightTaggedObject(step);
				}
				else
				{
					if (tutorialStepLayout != null && tutorialStepLayout.maskHighlight != null)
					{
						tutorialStepLayout.maskHighlight.ApplyTo(this.maskHighlight);
					}
					if (tutorialStepLayout != null && tutorialStepLayout.maskClick != null)
					{
						tutorialStepLayout.maskClick.ApplyTo(this.maskClick);
					}
				}
				if (step.showAnimation == TutorialStep.AnimationOptions.ShowWithShrinkingEffect || step.showAnimation == TutorialStep.AnimationOptions.ShowWithShrinkingEffectFadeIn)
				{
					this.backgroundImage.color = this.backgroundColor;
					base.gameObject.SetActive(true);
					base.StartCoroutine(this.AnimateHighlight(0.5f, step));
					flag3 = true;
				}
			}
			else
			{
				if (tutorialStepLayout != null && tutorialStepLayout.maskHighlight != null)
				{
					tutorialStepLayout.maskHighlight.ApplyTo(this.maskHighlight);
				}
				if (tutorialStepLayout != null && tutorialStepLayout.maskClick != null)
				{
					tutorialStepLayout.maskClick.ApplyTo(this.maskClick);
				}
			}
			if (!flag3)
			{
				this.tutorialArrow.Enable(step.arrowPosition, this.maskClick, this.backgroundImage.transform);
			}
			Image component = this.maskClick.GetComponent<Image>();
			Color color = component.color;
			color.a = 0f;
			component.color = color;
			if (step.clickMaskPositionDelay > 0f)
			{
				this.maskClick.gameObject.SetActive(false);
				base.StartCoroutine(this.ActivateClickMaskWithDelayRoutine(step.clickMaskPositionDelay));
			}
			else
			{
				this.maskClick.gameObject.SetActive(!flag2);
			}
			if (step.manualClickMaskPlacement)
			{
				this.maskClick.pivot = new Vector2(0.5f, 0.5f);
				this.maskClick.anchorMax = Vector2.zero;
				this.maskClick.anchorMin = Vector2.zero;
				tutorialStepLayout.maskClick.ApplyTo(this.maskClick);
			}
			if (step.forceTownUI && !this.IsMatch3)
			{
				TownBottomPanelRoot townBottomPanelRoot = global::UnityEngine.Object.FindObjectOfType<TownBottomPanelRoot>();
				if (townBottomPanelRoot)
				{
					townBottomPanelRoot.State = TownBottomPanelRoot.UIState.InGameUI;
					BuildingLocation.Selected = null;
					AreaClouds[] array = global::UnityEngine.Object.FindObjectsOfType<AreaClouds>();
					if (array != null)
					{
						foreach (AreaClouds areaClouds in array)
						{
							areaClouds.isSelected = false;
						}
					}
				}
			}
			if (step.highlightQuestObject)
			{
				Transform parent = step.HighlightObject.transform.parent;
				RectTransform component2 = parent.GetComponent<RectTransform>();
				this.maskHighlight.sizeDelta = new Vector2(component2.sizeDelta.x * TutorialOverlayRoot.QUEST_HIGHLIGHT_MASK_SCALE_FACTOR.x, component2.sizeDelta.y * TutorialOverlayRoot.QUEST_HIGHLIGHT_MASK_SCALE_FACTOR.y);
				this.maskHighlight.position = component2.position;
				Transform parent2 = this.maskClick.transform.parent;
				this.maskClick.transform.SetParent(step.HighlightObject.transform);
				this.maskClick.anchoredPosition = Vector2.zero;
				this.maskClick.anchorMin = new Vector2(0.5f, 0.5f);
				this.maskClick.anchorMax = new Vector2(0.5f, 0.5f);
				this.maskClick.pivot = new Vector2(0.5f, 0.5f);
				RectTransform component3 = step.HighlightObject.GetComponent<RectTransform>();
				if (component3 != null)
				{
					this.maskClick.sizeDelta = new Vector2(component3.rect.width, component3.rect.height);
				}
				this.maskClick.transform.SetParent(parent2, true);
			}
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x0014C0E0 File Offset: 0x0014A4E0
		public void SetHighlightType(bool isRoundHighlight)
		{
			if (isRoundHighlight)
			{
				this.maskHighlight = this.roundHighlight.rectTransform;
				this.roundHighlight.gameObject.SetActive(true);
				this.squareHighlight.gameObject.SetActive(false);
			}
			else
			{
				this.maskHighlight = this.squareHighlight.rectTransform;
				this.roundHighlight.gameObject.SetActive(false);
				this.squareHighlight.gameObject.SetActive(true);
			}
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x0014C15E File Offset: 0x0014A55E
		private void ActivateLayout(bool isActive, float delay)
		{
			this.maskHighlight.gameObject.SetActive(isActive);
			this.maskClick.gameObject.SetActive(isActive);
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x0014C184 File Offset: 0x0014A584
		private IEnumerator ActivateClickMaskWithDelayRoutine(float delay)
		{
			yield return new WaitForSeconds(delay);
			this.maskClick.gameObject.SetActive(true);
			yield break;
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x0014C1A8 File Offset: 0x0014A5A8
		public void ShowObject(GameObject go, Padding padding, bool lockEvents = false, bool applyScaleFactor = false)
		{
			RectTransform component = go.GetComponent<RectTransform>();
			MeshRenderer component2 = go.GetComponent<MeshRenderer>();
			if (component)
			{
				this.ShowElement(component, padding, applyScaleFactor);
			}
			else if (component2)
			{
				this.ShowMesh(component2, padding);
			}
			if (lockEvents)
			{
				base.GetComponentInChildren<EventProxy>().lockToObject = go;
			}
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x0014C202 File Offset: 0x0014A602
		public void ResetArrow(TutorialStep tutorialStep)
		{
			this.tutorialArrow.Reset(tutorialStep);
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x0014C210 File Offset: 0x0014A610
		private IEnumerator AnimateHighlight(float animationTime, TutorialStep step)
		{
			Vector3 initialScale = Vector3.one * 15f;
			Vector3 targetScale = Vector3.one;
			Color initialColor = new Color(this.backgroundImage.color.r, this.backgroundImage.color.g, this.backgroundImage.color.b, 0f);
			Color targetColor = new Color(this.backgroundImage.color.r, this.backgroundImage.color.g, this.backgroundImage.color.b, this.backgroundImage.color.a);
			Image maskImage = this.maskHighlight.GetComponent<Image>();
			if (step.showAnimation == TutorialStep.AnimationOptions.ShowWithShrinkingEffectFadeIn)
			{
				maskImage.color = initialColor;
				this.backgroundImage.color = initialColor;
			}
			float timePassedSoFar = 0f;
			float percentComplete = 0f;
			while (percentComplete < 1f)
			{
				timePassedSoFar += Time.deltaTime;
				percentComplete = timePassedSoFar / animationTime;
				this.maskHighlight.localScale = Vector3.Slerp(initialScale, targetScale, percentComplete);
				if (step.showAnimation == TutorialStep.AnimationOptions.ShowWithShrinkingEffectFadeIn)
				{
					this.backgroundImage.color = Color.Lerp(initialColor, targetColor, percentComplete);
					maskImage.color = this.backgroundImage.color;
				}
				yield return null;
			}
			this.maskHighlight.localScale = targetScale;
			if (!step.manualClickMaskPlacement)
			{
				this.maskClick.position = this.maskHighlight.position;
				this.maskClick.pivot = this.maskHighlight.pivot;
				this.maskClick.anchoredPosition = this.maskHighlight.anchoredPosition;
			}
			if (step.showAnimation == TutorialStep.AnimationOptions.ShowWithShrinkingEffectFadeIn)
			{
				this.backgroundImage.color = targetColor;
				maskImage.color = targetColor;
			}
			this.tutorialArrow.Enable(step.arrowPosition, this.maskClick, this.backgroundImage.transform);
			yield break;
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x0014C239 File Offset: 0x0014A639
		public Coroutine PanToPosition(Vector3 position)
		{
			return Camera.main.GetComponent<CameraPanManager>().PanTo(position, 2f, false, 0.5f);
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x0014C256 File Offset: 0x0014A656
		public void SetClickMaskRedirecting(bool redirect)
		{
			this.maskClick.GetComponent<EventProxy>().isRedirecting = redirect;
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x0014C26C File Offset: 0x0014A66C
		public void ShowMesh(MeshRenderer renderer, Padding padding)
		{
			if (this.highlightRenderer != renderer)
			{
				this.highlightRenderer = renderer;
				this.highlightVertices = renderer.GetComponentInChildren<MeshFilter>(true).sharedMesh.vertices;
				this.edgeVertexIndices = new int[4];
				this.edgeVertices = new Vector2[]
				{
					new Vector2(float.MaxValue, float.MaxValue),
					new Vector2(float.MaxValue, float.MaxValue),
					new Vector2(float.MinValue, float.MinValue),
					new Vector2(float.MinValue, float.MinValue)
				};
				for (int i = 0; i < this.highlightVertices.Length; i++)
				{
					Vector3 v = Camera.main.WorldToScreenPoint(this.highlightRenderer.transform.TransformPoint(this.highlightVertices[i]));
					if (v.x < this.edgeVertices[0].x)
					{
						this.edgeVertexIndices[0] = i;
						this.edgeVertices[0] = v;
					}
					if (v.y < this.edgeVertices[1].y)
					{
						this.edgeVertexIndices[1] = i;
						this.edgeVertices[1] = v;
					}
					if (v.x > this.edgeVertices[2].x)
					{
						this.edgeVertexIndices[2] = i;
						this.edgeVertices[2] = v;
					}
					if (v.y > this.edgeVertices[3].y)
					{
						this.edgeVertexIndices[3] = i;
						this.edgeVertices[3] = v;
					}
				}
			}
			Vector2 screenBl = Vector2.Min(Camera.main.WorldToScreenPoint(this.highlightRenderer.transform.TransformPoint(this.highlightVertices[this.edgeVertexIndices[0]])), Camera.main.WorldToScreenPoint(this.highlightRenderer.transform.TransformPoint(this.highlightVertices[this.edgeVertexIndices[1]])));
			Vector2 screenTr = Vector2.Max(Camera.main.WorldToScreenPoint(this.highlightRenderer.transform.TransformPoint(this.highlightVertices[this.edgeVertexIndices[2]])), Camera.main.WorldToScreenPoint(this.highlightRenderer.transform.TransformPoint(this.highlightVertices[this.edgeVertexIndices[3]])));
			this.SetMaskToScreenPosition(screenBl, screenTr, padding, false);
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x0014C557 File Offset: 0x0014A957
		private void HighlightTaggedObject(TutorialStep step)
		{
			this.ShowObject(step.HighlightObject, step.StepPadding, false, false);
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x0014C56D File Offset: 0x0014A96D
		private void ShowElement(RectTransform rect, Padding padding, bool applyScaleFactor = false)
		{
			this.Show(false);
			this.AdjustPosition(rect, this.maskHighlight, padding, applyScaleFactor);
		}

		// Token: 0x06004064 RID: 16484 RVA: 0x0014C588 File Offset: 0x0014A988
		public void ShowFields(int[] positions, bool highlightMove = false, bool showHand = false)
		{
			this.Show(false);
			this.maskHighlight.gameObject.SetActive(true);
			List<IntVector2> corners = this.GetCorners(positions);
			Vector2 vector = new Vector2(-0.35f, -0.3f);
			Vector3 b = new Vector3(BoardView.fieldOffset + vector.x, BoardView.fieldOffset + vector.y, 0f);
			Vector3 v = this.boardView.cam.WorldToScreenPoint((Vector3)corners[0] + b);
			Vector3 v2 = this.boardView.cam.WorldToScreenPoint((Vector3)corners[1] - b);
			this.SetMaskToScreenPosition(v, v2, null, false);
			Move expectedMove = this.GetExpectedMove(positions);
			this.boardView.SetExpectedMove(expectedMove);
			if (highlightMove)
			{
				this.boardView.TimeSinceLastValidSwap = 7f;
			}
			if (showHand)
			{
				float duration = this.nextMoveAnimator.duration / this.boardView.BoardAnimationController.speed;
				Vector3 to = this.boardView.cam.WorldToScreenPoint((Vector3)expectedMove.to);
				Vector3 from = this.boardView.cam.WorldToScreenPoint((Vector3)expectedMove.from);
				this.tutorialHand.Enable(this.backgroundImage.transform, from, to, duration);
			}
			EventProxy component = this.maskClick.GetComponent<EventProxy>();
			component.isRedirecting = true;
			component.isPointerEnterStartingDrag = true;
			component.isRedirectingDrag = true;
		}

		// Token: 0x06004065 RID: 16485 RVA: 0x0014C718 File Offset: 0x0014AB18
		private void SetMaskToScreenPosition(Vector2 screenBl, Vector2 screenTr, Padding padding = null, bool applyScaleFactor = false)
		{
			this.maskHighlight.pivot = new Vector2(0.5f, 0.5f);
			this.maskHighlight.anchorMax = Vector2.zero;
			this.maskHighlight.anchorMin = Vector2.zero;
			this.maskClick.pivot = new Vector2(0.5f, 0.5f);
			this.maskClick.anchorMax = Vector2.zero;
			this.maskClick.anchorMin = Vector2.zero;
			if (applyScaleFactor)
			{
				this.maskClick.localScale = Vector3.one * 0.6f;
			}
			this.tutorialArrow.transform.localScale = Vector3.one;
			if (padding != null)
			{
				screenBl.x -= (float)padding.left * this.canvas.scaleFactor;
				screenBl.y -= (float)padding.bottom * this.canvas.scaleFactor;
				screenTr.x += (float)padding.right * this.canvas.scaleFactor;
				screenTr.y += (float)padding.top * this.canvas.scaleFactor;
			}
			Vector3 a = default(Vector3);
			Vector3 vector;
			RectTransformUtility.ScreenPointToWorldPointInRectangle(this.canvas.GetComponent<RectTransform>(), screenBl, this.canvas.worldCamera, out vector);
			RectTransformUtility.ScreenPointToWorldPointInRectangle(this.canvas.GetComponent<RectTransform>(), screenTr, this.canvas.worldCamera, out a);
			this.maskHighlight.position = vector;
			this.maskHighlight.sizeDelta = (a - vector) * (1f / this.canvas.scaleFactor);
			this.maskHighlight.anchoredPosition += new Vector2(this.maskHighlight.rect.width / 2f, this.maskHighlight.rect.height / 2f);
			this.maskHighlight.sizeDelta = new Vector2(Mathf.Abs(this.maskHighlight.sizeDelta.x), Mathf.Abs(this.maskHighlight.sizeDelta.y));
			this.maskClick.sizeDelta = this.maskHighlight.sizeDelta;
			this.maskClick.position = this.maskHighlight.position;
			this.maskClick.anchoredPosition = this.maskHighlight.anchoredPosition;
			this.maskClick.pivot = this.maskHighlight.pivot;
			Image component = this.maskClick.GetComponent<Image>();
			Color color = component.color;
			color.a = 0f;
			component.color = color;
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x0014C9EC File Offset: 0x0014ADEC
		public void ResetExpectedMove()
		{
			this.boardView.SetExpectedMove(default(Move));
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x0014CA10 File Offset: 0x0014AE10
		public void ShowText(TutorialStep step, string locaKey)
		{
			if (this.speechBubble != null)
			{
				this.speechBubble.PreserveAnimations();
				this.speechBubble.gameObject.SetActive(false);
			}
			if (this.speechBubbleM3)
			{
				this.speechBubbleM3.gameObject.SetActive(false);
			}
			if (this.speechBubbleM3Landscape != null)
			{
				this.speechBubbleM3Landscape.gameObject.SetActive(false);
			}
			if (this.m3ElsieRender)
			{
				this.m3ElsieRender.SetActive(this.IsMatch3);
			}
			RectTransform component = this.speechBubbleM3;
			if (this.IsMatch3)
			{
				if (AUiAdjuster.SimilarOrientation == ScreenOrientation.LandscapeLeft)
				{
					component = this.speechBubbleM3Landscape;
				}
				else
				{
					component = this.speechBubbleM3;
				}
			}
			else if (this.speechBubble != null)
			{
				component = this.speechBubble.GetComponent<RectTransform>();
			}
			if (step.showText && component != null)
			{
				component.gameObject.SetActive(true);
				List<LocaParam> list = new List<LocaParam>();
				// SeasonConfig activeSeason = this.seasonService.GetActiveSeason();
				SeasonConfig activeSeason = null;
				if (activeSeason != null)
				{
					list.Add(new LocaParam("{seasonCurrencyIcon}", activeSeason.TMProIconName));
				}
				string text = this.locaService.GetText(locaKey, list.ToArray());
				TextMeshProUGUI componentInChildren = component.GetComponentInChildren<TextMeshProUGUI>(true);
				componentInChildren.gameObject.SetActive(true);
				componentInChildren.text = text;
				if (!this.IsMatch3)
				{
					this.SetupSpeechBubble(step);
				}
			}
			if (this.m3SpeechBubbleLeftIcons != null)
			{
				foreach (Image image in this.m3SpeechBubbleLeftIcons)
				{
					image.gameObject.SetActive(false);
				}
			}
			if (this.m3SpeechBubbleRightIcons != null)
			{
				foreach (Image image2 in this.m3SpeechBubbleRightIcons)
				{
					image2.gameObject.SetActive(false);
				}
			}
			if (this.islandSpeechBubbleLeftIcon != null)
			{
				this.islandSpeechBubbleLeftIcon.gameObject.SetActive(false);
			}
			if (this.islandSpeechBubbleRightIcon != null)
			{
				this.islandSpeechBubbleRightIcon.gameObject.SetActive(false);
			}
			if (step.iconInTextBox && step.iconSprite != null)
			{
				if (this.IsMatch3)
				{
					if (step.leftBubbleIcon)
					{
						if (this.m3SpeechBubbleLeftIcons != null)
						{
							foreach (Image image3 in this.m3SpeechBubbleLeftIcons)
							{
								// 不显示icon
								// image3.gameObject.SetActive(true);
								// image3.sprite = step.iconSprite;
							}
						}
					}
					else if (this.m3SpeechBubbleRightIcons != null)
					{
						foreach (Image image4 in this.m3SpeechBubbleRightIcons)
						{
							// 不显示icon
							// image4.gameObject.SetActive(true);
							// image4.sprite = step.iconSprite;
						}
					}
				}
				else if (step.leftBubbleIcon)
				{
					if (this.islandSpeechBubbleLeftIcon != null)
					{
						this.islandSpeechBubbleLeftIcon.gameObject.SetActive(true);
						this.islandSpeechBubbleLeftIcon.sprite = step.iconSprite;
					}
				}
				else if (this.islandSpeechBubbleRightIcon != null)
				{
					this.islandSpeechBubbleRightIcon.gameObject.SetActive(true);
					this.islandSpeechBubbleRightIcon.sprite = step.iconSprite;
				}
			}
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x0014CE20 File Offset: 0x0014B220
		public void Show(bool isAnimated)
		{
			base.gameObject.SetActive(true);
			EventSystemRoot.isUsedByTutorial = true;
			this.eventSystem.Enable();
			if (isAnimated)
			{
				Image component = this.maskHighlight.GetComponent<Image>();
				List<Image> images = new List<Image>
				{
					this.backgroundImage,
					component
				};
				this.backgroundImage.color = Color.clear;
				base.StartCoroutine(TutorialOverlayRoot.FadeImages(images, 0.25f, 0f, this.backgroundColor.a));
			}
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x0014CEA8 File Offset: 0x0014B2A8
		public IEnumerator HideAnimated()
		{
			if (base.gameObject.activeSelf)
			{
				yield return base.StartCoroutine(this.HideOverlay());
			}
			yield break;
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x0014CEC4 File Offset: 0x0014B2C4
		public void HideImmediately()
		{
			if (base.gameObject.activeSelf && !this.isAnimatingHide)
			{
				this.maskHighlight.gameObject.SetActive(false);
				this.speechBubble.PreserveAnimations();
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600406B RID: 16491 RVA: 0x0014CF14 File Offset: 0x0014B314
		private IEnumerator HideOverlay()
		{
			this.isAnimatingHide = true;
			Image maskImage = this.maskHighlight.GetComponent<Image>();
			List<Image> imagesToFade = new List<Image>
			{
				this.backgroundImage,
				maskImage
			};
			yield return base.StartCoroutine(TutorialOverlayRoot.FadeImages(imagesToFade, 0.25f, this.backgroundImage.color.a, 0f));
			this.isAnimatingHide = false;
			this.maskHighlight.gameObject.SetActive(false);
			this.speechBubble.PreserveAnimations();
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x0014CF30 File Offset: 0x0014B330
		public static IEnumerator FadeImage(Image image, float animationTime, float fromAlpha, float toAlpha)
		{
			yield return WooroutineRunner.StartCoroutine(TutorialOverlayRoot.FadeImages(new List<Image>
			{
				image
			}, animationTime, fromAlpha, toAlpha), null);
			yield break;
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x0014CF60 File Offset: 0x0014B360
		public static IEnumerator FadeImages(List<Image> images, float animationTime, float fromAlpha, float toAlpha)
		{
			if (images == null)
			{
				yield break;
			}
			List<Color> initialColors = new List<Color>();
			List<Color> targetColors = new List<Color>();
			foreach (Image image in images)
			{
				if (image != null)
				{
					Color initialColor = new Color(image.color.r, image.color.g, image.color.b, fromAlpha);
					Color targetColor = new Color(image.color.r, image.color.g, image.color.b, toAlpha);
					image.color = initialColor;
					initialColors.Add(initialColor);
					targetColors.Add(targetColor);
				}
			}
			float timePassedSoFar = 0f;
			float percentComplete = 0f;
			Color newColor = Color.clear;
			while (percentComplete < 1f)
			{
				timePassedSoFar += Time.deltaTime;
				percentComplete = timePassedSoFar / animationTime;
				for (int i = 0; i < images.Count; i++)
				{
					if (images[i] != null)
					{
						newColor = Color.Lerp(initialColors[i], targetColors[i], percentComplete);
						images[i].color = newColor;
					}
				}
				yield return null;
			}
			for (int j = 0; j < images.Count; j++)
			{
				if (images[j] != null)
				{
					images[j].color = targetColors[j];
				}
			}
			yield break;
		}

		// Token: 0x0600406E RID: 16494 RVA: 0x0014CF90 File Offset: 0x0014B390
		private void ShowOverlay(bool show)
		{
			EventSystemRoot.isUsedByTutorial = true;
			this.eventSystem.Enable();
			this.roundHighlight.gameObject.SetActive(false);
			this.squareHighlight.gameObject.SetActive(false);
			this.maskClick.gameObject.SetActive(show);
			this.maskHighlight.gameObject.SetActive(show);
			if (!this.isAnimatingHide)
			{
				base.gameObject.SetActive(true);
				this.backgroundImage.gameObject.SetActive(true);
				this.backgroundImage.color = ((!show) ? Color.clear : this.backgroundColor);
			}
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x0014D038 File Offset: 0x0014B438
		public void AddClearBlockingOverlay()
		{
			this.roundHighlight.gameObject.SetActive(false);
			this.squareHighlight.gameObject.SetActive(false);
			if (!this.isAnimatingHide)
			{
				this.backgroundImage.color = Color.clear;
			}
			this.maskHighlight.gameObject.SetActive(false);
			this.maskClick.gameObject.SetActive(false);
		}

		// Token: 0x06004070 RID: 16496 RVA: 0x0014D0A4 File Offset: 0x0014B4A4
		private void SetupSpeechBubble(TutorialStep step)
		{
			TutorialStepLayout info = step.overlayLayout;
			if (AUiAdjuster.SimilarOrientation == ScreenOrientation.LandscapeLeft && step.landscapeOverlayLayout != null)
			{
				info = step.landscapeOverlayLayout;
			}
			this.speechBubble.Info = info;
			this.speechBubble.SetCharacter(step.character);
			this.SetCharacterAnimation(step);
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x0014D0FC File Offset: 0x0014B4FC
		private List<IntVector2> GetCorners(int[] positions)
		{
			IntVector2 zero = IntVector2.Zero;
			IntVector2 item = new IntVector2(9, 9);
			for (int i = 0; i < positions.Length; i++)
			{
				if (positions[i] > 0)
				{
					int b = i % 9;
					int b2 = 8 - i / 9;
					zero.x = Mathf.Max(zero.x, b);
					zero.y = Mathf.Max(zero.y, b2);
					item.x = Mathf.Min(item.x, b);
					item.y = Mathf.Min(item.y, b2);
				}
			}
			return new List<IntVector2>
			{
				item,
				zero
			};
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x0014D1AF File Offset: 0x0014B5AF
		private void SetCharacterAnimation(TutorialStep step)
		{
			base.gameObject.SetActive(true);
			base.StartCoroutine(this.SetupCharacterAnimator(step));
			base.StartCoroutine(this.speechBubble.Setup3DCharacterAnimator(step.character));
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x0014D1E4 File Offset: 0x0014B5E4
		private IEnumerator SetupCharacterAnimator(TutorialStep step)
		{
			while (!this.characterAnimator.isInitialized)
			{
				yield return null;
			}
			this.characterAnimator.SetBool(step.character.ToString() + "Enabled", true);
			IEnumerable<TutorialSpeechBubble.TutorialCharacter> characters = Enum.GetValues(typeof(TutorialSpeechBubble.TutorialCharacter)).Cast<TutorialSpeechBubble.TutorialCharacter>();
			foreach (TutorialSpeechBubble.TutorialCharacter tutorialCharacter in characters)
			{
				if (step.speechBubbleAnimation == TutorialSpeechBubble.SpeechBubleAnimation.Change)
				{
					this.characterAnimator.StopPlayback();
				}
				else if (tutorialCharacter == step.character)
				{
					this.characterAnimator.SetBool(tutorialCharacter.ToString() + "Enabled", true);
				}
				else
				{
					this.characterAnimator.SetBool(tutorialCharacter.ToString() + "Enabled", false);
				}
			}
			this.characterAnimator.SetTrigger(step.speechBubbleAnimation.ToString());
			yield break;
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x0014D208 File Offset: 0x0014B608
		public void AdjustPosition(RectTransform origin, RectTransform target, Padding padding, bool applyScaleFactor = false)
		{
			target.gameObject.SetActive(true);
			Vector2 screenBl;
			Vector2 screenTr;
			RectTransformHelper.GetScreenCorners(origin, out screenBl, out screenTr, null);
			this.SetMaskToScreenPosition(screenBl, screenTr, padding, applyScaleFactor);
			this.tutorialArrow.AdjustPosition(target);
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x0014D244 File Offset: 0x0014B644
		private Move GetExpectedMove(int[] postions)
		{
			Move result = default(Move);
			IntVector2[] array = new IntVector2[2];
			int num = 0;
			int num2 = 0;
			while (num2 < postions.Length && num < 2)
			{
				if (postions[num2] == 2)
				{
					IntVector2 intVector = default(IntVector2);
					intVector.x = num2 % 9;
					intVector.y = 8 - num2 / 9;
					array[num++] = intVector;
				}
				num2++;
			}
			result.from = array[0];
			result.to = array[1];
			return result;
		}

		// Token: 0x040069F4 RID: 27124
		private const float PAN_TIME = 2f;

		// Token: 0x040069F5 RID: 27125
		private const float MASK_CLICK_SCALE_FACTOR = 0.6f;

		// Token: 0x040069F6 RID: 27126
		private static readonly Vector2 QUEST_HIGHLIGHT_MASK_SCALE_FACTOR = new Vector2(1.15f, 1.4f);

		// Token: 0x040069F7 RID: 27127
		[WaitForRoot(false, false)]
		public EventSystemRoot eventSystem;

		// Token: 0x040069F8 RID: 27128
		[WaitForService(true, true)]
		public AudioService audioService;

		// Token: 0x040069F9 RID: 27129
		// [WaitForService(true, true)]
		// public TrackingService trackingService;

		// Token: 0x040069FA RID: 27130
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x040069FB RID: 27131
		// [WaitForService(true, true)]
		// private SeasonService seasonService;

		// Token: 0x040069FC RID: 27132
		public readonly Signal onScreenClicked = new Signal();

		// Token: 0x040069FD RID: 27133
		public readonly Signal onButtonClicked = new Signal();

		// Token: 0x040069FE RID: 27134
		[SerializeField]
		private AnimatedUi animatedUi;

		// Token: 0x040069FF RID: 27135
		[Header("Masking")]
		public Canvas canvas;

		// Token: 0x04006A00 RID: 27136
		public Image backgroundImage;

		// Token: 0x04006A01 RID: 27137
		public Image roundHighlight;

		// Token: 0x04006A02 RID: 27138
		public Image squareHighlight;

		// Token: 0x04006A03 RID: 27139
		public RectTransform maskHighlight;

		// Token: 0x04006A04 RID: 27140
		public RectTransform maskClick;

		// Token: 0x04006A05 RID: 27141
		[Header("Pointer")]
		public TutorialArrow tutorialArrow;

		// Token: 0x04006A06 RID: 27142
		public TutorialHand tutorialHand;

		// Token: 0x04006A07 RID: 27143
		[Header("Island")]
		public TutorialSpeechBubble speechBubble;

		// Token: 0x04006A08 RID: 27144
		[SerializeField]
		private Image islandSpeechBubbleLeftIcon;

		// Token: 0x04006A09 RID: 27145
		[SerializeField]
		private Image islandSpeechBubbleRightIcon;

		// Token: 0x04006A0A RID: 27146
		[Header("Match 3")]
		[SerializeField]
		private GameObject m3ElsieRender;

		// Token: 0x04006A0B RID: 27147
		[SerializeField]
		private RectTransform speechBubbleM3;

		// Token: 0x04006A0C RID: 27148
		[SerializeField]
		private RectTransform speechBubbleM3Landscape;

		// Token: 0x04006A0D RID: 27149
		[SerializeField]
		private List<Image> m3SpeechBubbleLeftIcons;

		// Token: 0x04006A0E RID: 27150
		[SerializeField]
		private List<Image> m3SpeechBubbleRightIcons;

		// Token: 0x04006A0F RID: 27151
		[SerializeField]
		private NextMoveAnimator nextMoveAnimator;

		// Token: 0x04006A10 RID: 27152
		public Animator characterAnimator;

		// Token: 0x04006A11 RID: 27153
		private bool isAnimatingHide;

		// Token: 0x04006A12 RID: 27154
		private MeshRenderer highlightRenderer;

		// Token: 0x04006A13 RID: 27155
		private Vector3[] highlightVertices;

		// Token: 0x04006A14 RID: 27156
		private Vector2[] edgeVertices;

		// Token: 0x04006A15 RID: 27157
		private int[] edgeVertexIndices;
	}
}
