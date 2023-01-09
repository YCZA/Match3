using System;
using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020008F5 RID: 2293
namespace Match3.Scripts1
{
	[LoadOptions(true, true, false)]
	public class PopupMissingAssetsRoot : ASceneRoot<string>, IDisposableDialog
	{
		// Token: 0x060037DC RID: 14300 RVA: 0x001115C4 File Offset: 0x0010F9C4
		public static IEnumerator TryShowRoutine(string missingBundle)
		{
			TownUiRoot townUiRoot = global::UnityEngine.Object.FindObjectOfType<TownUiRoot>();
			if (townUiRoot != null)
			{
				townUiRoot.ShowUi(false);
			}
			Wooroutine<PopupMissingAssetsRoot> sceneLoadRoutine = SceneManager.Instance.LoadSceneWithParams<PopupMissingAssetsRoot, string>(missingBundle, null);
			yield return sceneLoadRoutine;
			PopupMissingAssetsRoot scene = sceneLoadRoutine.ReturnValue;
			yield return scene.onClose;
			if (townUiRoot != null)
			{
				townUiRoot.ShowUi(true);
			}
			yield break;
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x001115E0 File Offset: 0x0010F9E0
		protected override void Go()
		{
			base.Go();
			this.animator.SetBool("open", true);
			this.audioService.PlaySFX(AudioId.MenuSlideOpen, false, false, false);
			this.buttonClose.onClick.AddListener(new UnityAction(this.HandleBackButton));
			BackButtonManager.Instance.AddAction(new Action(this.HandleBackButton));
			this.tracking.TrackUi("popupMissingAssets", this.parameters, string.Empty, string.Empty, new object[0]);
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x00111670 File Offset: 0x0010FA70
		private void HandleBackButton()
		{
			BackButtonManager.Instance.RemoveAction(new Action(this.HandleBackButton));
			this.audioService.PlaySFX(AudioId.NormalClick, false, false, false);
			this.animator.SetBool("close", true);
			this.audioService.PlaySFX(AudioId.MenuSlideClose, false, false, false);
			base.Invoke("DispatchOnCloseAndDisableDialogHolder", this.closeAnimation.length + 0.125f);
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x001116E8 File Offset: 0x0010FAE8
		public void DispatchOnCloseAndDisableDialogHolder()
		{
			this.onClose.Dispatch();
			this.ExecuteOnParent(delegate(IPersistentDialog d)
			{
				d.Disable();
			});
			this.ExecuteOnParent(delegate(IDisposableDialog d)
			{
				d.Destroy();
			});
		}

		// Token: 0x04006005 RID: 24581
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04006006 RID: 24582
		[WaitForService(true, true)]
		private TrackingService tracking;

		// Token: 0x04006007 RID: 24583
		[SerializeField]
		private Button buttonClose;

		// Token: 0x04006008 RID: 24584
		public Animator animator;

		// Token: 0x04006009 RID: 24585
		public AnimationClip closeAnimation;

		// Token: 0x0400600A RID: 24586
		public readonly AwaitSignal onClose = new AwaitSignal();

		// Token: 0x0400600B RID: 24587
		private const string ANIM_PARAM_OPEN = "open";

		// Token: 0x0400600C RID: 24588
		private const string ANIM_PARAM_CLOSE = "close";

		// Token: 0x0400600D RID: 24589
		private const float A_PLEASING_LITTLE_DELAY_SECONDS = 0.125f;

		// Token: 0x020008F6 RID: 2294
		public class Trigger : PopupManager.Trigger
		{
			// Token: 0x060037E2 RID: 14306 RVA: 0x00111756 File Offset: 0x0010FB56
			public Trigger(GameStateService gameStateService, BuildingResourceServiceRoot buildingResourceService)
			{
				this.gameStateService = gameStateService;
				this.buildingResourceService = buildingResourceService;
			}

			// Token: 0x060037E3 RID: 14307 RVA: 0x0011176C File Offset: 0x0010FB6C
			public override bool ShouldTrigger()
			{
				return !PopupMissingAssetsRoot.Trigger.hasBeenTriggered && this.IsOffline() && this.IsOnHerOwnIsland() && this.IsMissingBuildingBundles();
			}

			// Token: 0x060037E4 RID: 14308 RVA: 0x00111798 File Offset: 0x0010FB98
			public override IEnumerator Run()
			{
				PopupMissingAssetsRoot.Trigger.hasBeenTriggered = true;
				yield return PopupMissingAssetsRoot.TryShowRoutine("buildingBundles");
				yield break;
			}

			// Token: 0x060037E5 RID: 14309 RVA: 0x001117AC File Offset: 0x0010FBAC
			private bool IsOffline()
			{
				return Application.internetReachability == NetworkReachability.NotReachable;
			}

			// Token: 0x060037E6 RID: 14310 RVA: 0x001117B6 File Offset: 0x0010FBB6
			private bool IsOnHerOwnIsland()
			{
				return this.gameStateService.IsMyOwnState;
			}

			// Token: 0x060037E7 RID: 14311 RVA: 0x001117C3 File Offset: 0x0010FBC3
			private bool IsMissingBuildingBundles()
			{
				return this.buildingResourceService.DoesAnyBundleNeedDownload(false);
			}

			// Token: 0x04006010 RID: 24592
			public static bool hasBeenTriggered;

			// Token: 0x04006011 RID: 24593
			private readonly GameStateService gameStateService;

			// Token: 0x04006012 RID: 24594
			private readonly BuildingResourceServiceRoot buildingResourceService;
		}
	}
}
