using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.ResourceManager;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI.Challenge
{
	// Token: 0x02000998 RID: 2456
	[LoadOptions(true, true, false)]
	public class ChallengeV2InfoRoot : APtSceneRoot<List<BuildingConfig>>, IDisposableDialog
	{
		// Token: 0x06003BAB RID: 15275 RVA: 0x00128224 File Offset: 0x00126624
		protected override IEnumerator GoRoutine()
		{
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			yield return this.ShowDecoSet();
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.closeButton.onClick.AddListener(new UnityAction(this.Close));
			yield break;
		}

		// Token: 0x06003BAC RID: 15276 RVA: 0x00128240 File Offset: 0x00126640
		private IEnumerator ShowDecoSet()
		{
			int num = 0;
			while (num < this.parameters.Count && num < this.buildingViews.Count)
			{
				this.buildingViews[num].sprite = this.resourceServiceRoot.GetWrappedSpriteOrPlaceholder(this.parameters[num]).asset;
				this.buildingViews[num].gameObject.SetActive(true);
				num++;
			}
			Wooroutine<SpriteManager> spriteManagerFlow = new BundledSpriteManagerLoaderFlow().Start(new BundledSpriteManagerLoaderFlow.Input
			{
				bundleName = "buildings_challenges_2018",
				path = "Assets/Puzzletown/Town/Ui/Challenge/SetIllustrations/SpriteManager_Challenge_SetIllustrations.prefab"
			});
			yield return spriteManagerFlow;
			if (spriteManagerFlow.ReturnValue)
			{
				Sprite similar = spriteManagerFlow.ReturnValue.GetSimilar(this.gameStateService.Challenges.CurrentDecoSet.ToString());
				if (similar != null)
				{
					this.previewImage.sprite = similar;
				}
			}
			yield break;
		}

		// Token: 0x06003BAD RID: 15277 RVA: 0x0012825B File Offset: 0x0012665B
		public void Close()
		{
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.dialog.Hide();
		}

		// Token: 0x040063AC RID: 25516
		private const string PATH = "Assets/Puzzletown/Town/Ui/Challenge/SetIllustrations/SpriteManager_Challenge_SetIllustrations.prefab";

		// Token: 0x040063AD RID: 25517
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x040063AE RID: 25518
		[SerializeField]
		private Image previewImage;

		// Token: 0x040063AF RID: 25519
		[SerializeField]
		private Button closeButton;

		// Token: 0x040063B0 RID: 25520
		[SerializeField]
		private List<Image> buildingViews;

		// Token: 0x040063B1 RID: 25521
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040063B2 RID: 25522
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x040063B3 RID: 25523
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040063B4 RID: 25524
		[WaitForRoot(false, false)]
		private BuildingResourceServiceRoot resourceServiceRoot;
	}
}
