using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x0200099D RID: 2461
	public class TownOverheadUiRoot : APtSceneRoot, IHandler<PopupOperation>
	{
		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06003BBA RID: 15290 RVA: 0x001289F7 File Offset: 0x00126DF7
		// (set) Token: 0x06003BBB RID: 15291 RVA: 0x001289FF File Offset: 0x00126DFF
		public new bool IsSetup { get; private set; }

		// Token: 0x06003BBC RID: 15292 RVA: 0x00128A08 File Offset: 0x00126E08
		protected override void Awake()
		{
			base.Awake();
			base.GetComponentsInChildren<ABuildingUiView>().ForEach(delegate(ABuildingUiView v)
			{
				v.Hide();
			});
			this.ingameSpeechBubble.Hide();
			this.chapterIndicator.gameObject.SetActive(false);
			this.overlay.enabled = false;
			this.blackBars.gameObject.SetActive(false);
			base.StartCoroutine(this.SetupCamera());
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x00128A8C File Offset: 0x00126E8C
		protected override void Go()
		{
			if (!this.gameState.isInteractable)
			{
				CanvasGroup component = this.canvas.GetComponent<CanvasGroup>();
				component.alpha = 0f;
				component.interactable = false;
				component.blocksRaycasts = false;
			}
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x00128AD0 File Offset: 0x00126ED0
		public IEnumerator SetupCamera()
		{
			while (!Camera.main)
			{
				yield return null;
			}
			this.canvas.worldCamera = TownOverheadUiRoot.GetOverheadUiCamera(Camera.main);
			this.canvas.transform.rotation = Quaternion.Euler(30f, 45f, 0f);
			this.IsSetup = true;
			yield break;
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x00128AEC File Offset: 0x00126EEC
		public UiSpeechBubble FindStoryBubble(SBOrientation orientation)
		{
			return Array.Find<UiSpeechBubble>(this.storySpeechBubbles, (UiSpeechBubble sb) => sb.orientation == orientation);
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x00128B1D File Offset: 0x00126F1D
		private static Camera GetOverheadUiCamera(Camera source)
		{
			return Array.Find<Camera>(source.GetComponentsInChildren<Camera>(), (Camera cam) => cam.IsLayerVisible(ObjectLayer.UI));
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x00128B47 File Offset: 0x00126F47
		public void ShowBlackBars(bool show)
		{
			if (show)
			{
				this.blackBars.Show();
			}
			else
			{
				this.blackBars.Hide();
			}
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x00128B6A File Offset: 0x00126F6A
		public void Handle(PopupOperation op)
		{
			this.onTap.Dispatch(op);
		}

		// Token: 0x040063CC RID: 25548
		public Canvas canvas;

		// Token: 0x040063CD RID: 25549
		public Canvas overlay;

		// Token: 0x040063CE RID: 25550
		public readonly Signal<PopupOperation> onTap = new Signal<PopupOperation>();

		// Token: 0x040063CF RID: 25551
		public UiSpeechBubble ingameSpeechBubble;

		// Token: 0x040063D0 RID: 25552
		public UiSpeechBubble[] storySpeechBubbles;

		// Token: 0x040063D1 RID: 25553
		public BuildingUiHarvestCallout harvestCallout;

		// Token: 0x040063D2 RID: 25554
		public ChapterIndicator chapterIndicator;

		// Token: 0x040063D3 RID: 25555
		public AnimatedUi blackBars;

		// Token: 0x040063D4 RID: 25556
		public CanvasGroup inGameUi;

		// Token: 0x040063D6 RID: 25558
		[WaitForService(true, true)]
		public AudioService audioService;

		// Token: 0x040063D7 RID: 25559
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x040063D8 RID: 25560
		[HideInInspector]
		[WaitForRoot(false, false)]
		public TownEnvironmentRoot env;
	}
}
