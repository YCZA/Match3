using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.UnityEngine.EventSystems.ResourceManager;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.UI;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006E8 RID: 1768
	public class M3_LevelSelectionForeshadowing : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002BFC RID: 11260 RVA: 0x000CA6AC File Offset: 0x000C8AAC
		private Vector3 OffsetPosition
		{
			get
			{
				int num = (!this.hasCollectable) ? 0 : this.yOffset;
				if (this.initialPosition == Vector3.zero)
				{
					this.initialPosition = base.transform.localPosition;
				}
				if (AUiAdjuster.SimilarOrientation == ScreenOrientation.LandscapeLeft)
				{
					return new Vector3(this.initialPosition.x - (float)num, this.initialPosition.y, this.initialPosition.z);
				}
				return new Vector3(this.initialPosition.x, this.initialPosition.y + (float)num, this.initialPosition.z);
			}
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x000CA755 File Offset: 0x000C8B55
		public void OnPointerClick(PointerEventData evt)
		{
			if (!this.expanded)
			{
				this.ToggleState();
			}
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x000CA768 File Offset: 0x000C8B68
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.expanded)
			{
				this.ToggleState();
			}
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x000CA77C File Offset: 0x000C8B7C
		private void ToggleState()
		{
			if (this.animationRoutine != null)
			{
				return;
			}
			M3_LevelSelectionUiRoot componentInParent = base.GetComponentInParent<M3_LevelSelectionUiRoot>();
			if (!this.expanded)
			{
				this.expandAnimation["ForeshadowingLevelMapExpend"].speed = 1f;
				this.expandAnimation["ForeshadowingLevelMapExpend"].time = 0f;
				this.expandAnimation.Play("ForeshadowingLevelMapExpend");
				this.expanded = true;
				QuickTableView componentInParent2 = base.GetComponentInParent<QuickTableView>();
				bool flag = AUiAdjuster.SimilarOrientation == ScreenOrientation.Portrait;
				if (componentInParent2 != null)
				{
					if (flag)
					{
						if (base.transform.position.y / (float)Screen.height >= 0.6f)
						{
							componentInParent.ScrollToLevel(this.level, 0.5f);
						}
					}
					else if (base.transform.position.x / (float)Screen.width >= 0.6f)
					{
						componentInParent.ScrollToLevel(this.level + 1, 0.5f);
					}
				}
				float xOffset = 0f;
				float num = 190f;
				if (flag)
				{
					float num2 = base.transform.position.x / (float)Screen.width;
					if (num2 < 0.3333f)
					{
						xOffset = 100f * (float)this.numberOfPreviews;
					}
					else if (num2 > 0.6666f)
					{
						xOffset = -100f * (float)this.numberOfPreviews;
					}
				}
				else
				{
					num = 220f + (float)this.numberOfPreviews * 150f;
					xOffset = 160f;
				}
				this.animationRoutine = base.StartCoroutine(this.AnimateToPosition(xOffset, num, flag));
			}
			else
			{
				this.expandAnimation["ForeshadowingLevelMapCollapsed"].speed = 1f;
				this.expandAnimation["ForeshadowingLevelMapCollapsed"].time = 0f;
				this.expandAnimation.Play("ForeshadowingLevelMapCollapsed");
				this.animationRoutine = base.StartCoroutine(this.ReturnToPosition());
				this.cancelArea.SetActive(false);
				this.pointer.SetActive(false);
				this.pointer.transform.localPosition = this.pointerInitialPosition;
			}
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x000CA9B8 File Offset: 0x000C8DB8
		public void SetVisible(List<LevelForeshadowingConfig.ForeshadowingLevelConfig> configs, int level, bool hasCollectable, bool isUnlocked)
		{
			this.hasCollectable = hasCollectable;
			this.numberOfPreviews = configs.Count;
			base.gameObject.SetActive(this.numberOfPreviews > 0);
			if (isUnlocked)
			{
				this.unlockText.SetActive(false);
			}
			if (this.numberOfPreviews > 0)
			{
				this.firstPreview.gameObject.SetActive(this.numberOfPreviews > 0);
				this.secondPreview.gameObject.SetActive(this.numberOfPreviews > 1);
				this.level = level;
				this.configs = configs;
				WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
			}
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x000CAA58 File Offset: 0x000C8E58
		private IEnumerator InitRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			if (this.configs.Count > 0)
			{
				string text = this.localizationService.GetText("ui.feature_foreshadowing.header." + this.configs[0].type, new LocaParam[0]);
				text = text + " " + this.localizationService.GetText(this.configs[0].locaKey, new LocaParam[0]);
				this.firstFeatureLabel.text = text;
				this.firstItemPreview.sprite = this.spriteManager.GetSimilar(this.configs[0].feature);
			}
			if (this.configs.Count > 1)
			{
				string text2 = this.localizationService.GetText("ui.feature_foreshadowing.header." + this.configs[1].type, new LocaParam[0]);
				text2 = text2 + " " + this.localizationService.GetText(this.configs[1].locaKey, new LocaParam[0]);
				this.secondFeatureLabel.text = text2;
				this.secondItemPreview.sprite = this.spriteManager.GetSimilar(this.configs[1].feature);
			}
			this.ResetPreviews();
			base.transform.localPosition = this.OffsetPosition;
			yield break;
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x000CAA74 File Offset: 0x000C8E74
		private IEnumerator AnimateToPosition(float xOffset, float yOffset, bool isPortrait)
		{
			this.beforeMovePosition = base.transform.localPosition;
			base.transform.DOKill(true);
			yield return base.transform.DOLocalMove(new Vector3(base.transform.localPosition.x + xOffset, base.transform.localPosition.y + yOffset, base.transform.localPosition.z), 0.3f, false).WaitForCompletion();
			if (isPortrait)
			{
				this.pointer.SetActive(true);
				this.pointer.transform.localScale = Vector3.zero;
				Vector3 localPosition = this.pointer.transform.localPosition;
				int num = (!this.hasCollectable) ? 0 : this.yOffset;
				this.pointer.transform.localPosition = new Vector3(localPosition.x, localPosition.y + (float)num, localPosition.z);
				this.pointer.transform.DOScale(Vector3.one, 0.15f);
			}
			this.cancelArea.SetActive(true);
			this.animationRoutine = null;
			yield break;
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x000CAAA4 File Offset: 0x000C8EA4
		private IEnumerator ReturnToPosition()
		{
			base.transform.DOKill(false);
			yield return base.transform.DOLocalMove(this.beforeMovePosition, 0.3f, false).WaitForCompletion();
			this.expanded = false;
			this.animationRoutine = null;
			yield break;
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x000CAAC0 File Offset: 0x000C8EC0
		private void ResetPreviews()
		{
			if (this.animationRoutine == null)
			{
				this.expandAnimation["ForeshadowingLevelMapExpend"].speed = -1f;
				this.expandAnimation["ForeshadowingLevelMapExpend"].time = 0f;
				this.expandAnimation.Play("ForeshadowingLevelMapExpend");
				this.expanded = false;
				this.cancelArea.gameObject.SetActive(false);
				this.pointer.SetActive(false);
			}
			this.pointer.transform.localPosition = this.pointerInitialPosition;
			base.transform.localPosition = this.OffsetPosition;
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x000CAB68 File Offset: 0x000C8F68
		private void Awake()
		{
			this.pointerInitialPosition = this.pointer.transform.localPosition;
		}

		// Token: 0x06002C06 RID: 11270 RVA: 0x000CAB80 File Offset: 0x000C8F80
		private void OnEnable()
		{
			AUiAdjuster.OnScreenOrientationChange.AddListener(new Action<ScreenOrientation>(this.OnOrientationChange));
			base.StartCoroutine(this.InitRoutine());
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x000CABA5 File Offset: 0x000C8FA5
		private void OnDisable()
		{
			AUiAdjuster.OnScreenOrientationChange.RemoveListener(new Action<ScreenOrientation>(this.OnOrientationChange));
			this.ResetPreviews();
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x000CABC3 File Offset: 0x000C8FC3
		private void OnOrientationChange(ScreenOrientation screenOrientation)
		{
			if (screenOrientation != this.previouScreenOrientation)
			{
				this.initialPosition = Vector3.zero;
				this.previouScreenOrientation = screenOrientation;
				base.StartCoroutine(this.InitRoutine());
			}
		}

		// Token: 0x0400551C RID: 21788
		private const string EXPAND_ANIMATION_NAME = "ForeshadowingLevelMapExpend";

		// Token: 0x0400551D RID: 21789
		private const string COLLAPSE_ANIMATION_NAME = "ForeshadowingLevelMapCollapsed";

		// Token: 0x0400551E RID: 21790
		private const string LOCA_PREFIX = "ui.feature_foreshadowing.header.";

		// Token: 0x0400551F RID: 21791
		private const float ANIMATION_TIME = 0.3f;

		// Token: 0x04005520 RID: 21792
		private const float Y_DEFAULT_OFFSET = 190f;

		// Token: 0x04005521 RID: 21793
		private const float X_OFFSET_AMOUNT = 100f;

		// Token: 0x04005522 RID: 21794
		private const float LANDSCAPE_DEFAULT_OFFSET = 220f;

		// Token: 0x04005523 RID: 21795
		private const float LANDSCAPE_MULTIPLE_ITEMS_OFFSET = 150f;

		// Token: 0x04005524 RID: 21796
		private const float LANDSCAPE_X_OFFSET = 160f;

		// Token: 0x04005525 RID: 21797
		private const float ACTIVATION_PERCENT = 0.6f;

		// Token: 0x04005526 RID: 21798
		private const float X_LEFT_BORDER = 0.3333f;

		// Token: 0x04005527 RID: 21799
		private const float X_RIGHT_BORDER = 0.6666f;

		// Token: 0x04005528 RID: 21800
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04005529 RID: 21801
		[SerializeField]
		private Animation expandAnimation;

		// Token: 0x0400552A RID: 21802
		[SerializeField]
		private GameObject firstPreview;

		// Token: 0x0400552B RID: 21803
		[SerializeField]
		private GameObject secondPreview;

		// Token: 0x0400552C RID: 21804
		[SerializeField]
		private GameObject cancelArea;

		// Token: 0x0400552D RID: 21805
		[SerializeField]
		private GameObject pointer;

		// Token: 0x0400552E RID: 21806
		[SerializeField]
		private TextMeshProUGUI firstFeatureLabel;

		// Token: 0x0400552F RID: 21807
		[SerializeField]
		private TextMeshProUGUI secondFeatureLabel;

		// Token: 0x04005530 RID: 21808
		[SerializeField]
		private Image firstItemPreview;

		// Token: 0x04005531 RID: 21809
		[SerializeField]
		private Image secondItemPreview;

		// Token: 0x04005532 RID: 21810
		[SerializeField]
		private GameObject unlockText;

		// Token: 0x04005533 RID: 21811
		[SerializeField]
		private int yOffset;

		// Token: 0x04005534 RID: 21812
		[SerializeField]
		private SpriteManagerWithOverride spriteManager;

		// Token: 0x04005535 RID: 21813
		private bool expanded;

		// Token: 0x04005536 RID: 21814
		private Vector3 initialPosition = Vector3.zero;

		// Token: 0x04005537 RID: 21815
		private Vector3 beforeMovePosition;

		// Token: 0x04005538 RID: 21816
		private int level;

		// Token: 0x04005539 RID: 21817
		private int numberOfPreviews;

		// Token: 0x0400553A RID: 21818
		private List<LevelForeshadowingConfig.ForeshadowingLevelConfig> configs;

		// Token: 0x0400553B RID: 21819
		private Coroutine animationRoutine;

		// Token: 0x0400553C RID: 21820
		private ScreenOrientation previouScreenOrientation;

		// Token: 0x0400553D RID: 21821
		private bool hasCollectable;

		// Token: 0x0400553E RID: 21822
		private Vector3 pointerInitialPosition;
	}
}
