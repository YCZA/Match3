using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.UnityEngine;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000704 RID: 1796
	public class M3_BannersRoot : APtSceneRoot, IPersistentDialog, IHandler<PopupOperation>
	{
		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06002C71 RID: 11377 RVA: 0x000CCBA1 File Offset: 0x000CAFA1
		// (set) Token: 0x06002C72 RID: 11378 RVA: 0x000CCBA9 File Offset: 0x000CAFA9
		public PopupOperation operation { get; private set; }

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06002C73 RID: 11379 RVA: 0x000CCBB2 File Offset: 0x000CAFB2
		// (set) Token: 0x06002C74 RID: 11380 RVA: 0x000CCBBA File Offset: 0x000CAFBA
		public LevelConfig level { get; private set; }

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06002C75 RID: 11381 RVA: 0x000CCBC3 File Offset: 0x000CAFC3
		// (set) Token: 0x06002C76 RID: 11382 RVA: 0x000CCBCB File Offset: 0x000CAFCB
		public IAPData iapData { get; private set; }

		// Token: 0x06002C77 RID: 11383 RVA: 0x000CCBD4 File Offset: 0x000CAFD4
		protected override void Awake()
		{
			base.Awake();
			base.Disable();
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x000CCBE4 File Offset: 0x000CAFE4
		private void Show(M3_BannerType type, LevelConfig level)
		{
			base.Enable();
			this._currentType = type;
			this.operation = PopupOperation.None;
			this.level = level;
			this.ShowOnChildren(type, true, true);
			this.isSkipping = false;
			this.dialog.Show();
			this.audioService.PlaySFX(AudioId.BannerShowDefault, false, false, false);
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x000CCC3C File Offset: 0x000CB03C
		public IEnumerator ShowWelcome(LevelConfig level, float duration = 2f)
		{
			this.level = level;
			this.Show(M3_BannerType.LevelStart, level);
			yield return this.WaitForSeconds(duration);
			this.Hide();
			yield break;
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x000CCC68 File Offset: 0x000CB068
		public IEnumerator ShowShuffle(float duration = 2f)
		{
			this.Show(M3_BannerType.Shuffle, null);
			yield return this.WaitForSeconds(duration);
			this.Hide();
			yield break;
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x000CCC8C File Offset: 0x000CB08C
		private IEnumerator WaitForSeconds(float duration)
		{
			float time = Time.time;
			while (Time.time - time < duration && !this.isSkipping)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x000CCCAE File Offset: 0x000CB0AE
		public void ShowBuyMoreMoves(LevelConfig level, IAPData iap)
		{
			this.iapData = iap;
			this.Show(M3_BannerType.OutOfMovesNoDiamonds, level);
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x000CCCC0 File Offset: 0x000CB0C0
		public void ShowOutOfMoves(LevelConfig level)
		{
			M3_BannerType type = M3_BannerType.OutOfMoves;
			if (this.config.SbsConfig.feature_switches.buy_more_moves_offer && this.progression.Data.UnlockedLevel < this.levelUntilAddMovesAreFree && level.SelectedTier < AreaConfig.Tier.b)
			{
				type = M3_BannerType.OutOfMovesFree;
			}
			this.Show(type, level);
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x000CCD1B File Offset: 0x000CB11B
		public void Handle(PopupOperation op)
		{
			this.operation = op;
		}

		// Token: 0x06002C7F RID: 11391 RVA: 0x000CCD24 File Offset: 0x000CB124
		public void Hide()
		{
			this.dialog.Hide();
			this.audioService.PlaySFX(AudioId.BannerHideDefault, false, false, false);
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x000CCD48 File Offset: 0x000CB148
		public void Update()
		{
			if (global::UnityEngine.Input.touchCount > 0 || Input.GetMouseButton(0))
			{
				M3_BannerType currentType = this._currentType;
				if (currentType == M3_BannerType.LevelStart || currentType == M3_BannerType.Shuffle)
				{
					if (!SceneManager.IsLoadingScreenShown())
					{
						this.isSkipping = true;
					}
				}
			}
		}

		// Token: 0x040055C0 RID: 21952
		private M3_BannerType _currentType;

		// Token: 0x040055C1 RID: 21953
		private bool isSkipping;

		// Token: 0x040055C2 RID: 21954
		public const float BANNER_PAUSE = 2f;

		// Token: 0x040055C4 RID: 21956
		[WaitForService(true, true)]
		public ILocalizationService loc;

		// Token: 0x040055C5 RID: 21957
		[WaitForService(true, true)]
		public ConfigService config;

		// Token: 0x040055C6 RID: 21958
		[WaitForService(true, true)]
		public ProgressionDataService.Service progression;

		// Token: 0x040055C7 RID: 21959
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040055C8 RID: 21960
		public AnimatedUi dialog;

		// Token: 0x040055CB RID: 21963
		public int levelUntilAddMovesAreFree = 11;

		// Token: 0x040055CC RID: 21964
		[HideInInspector]
		public Match3Score score;
	}
}
