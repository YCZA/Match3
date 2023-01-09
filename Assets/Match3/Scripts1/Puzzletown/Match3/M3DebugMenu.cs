using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000712 RID: 1810
	public class M3DebugMenu : ArrayDataSource<M3_CheatView, EnumWithDebugSettings>, IHandler<EnumWithDebugSettings>
	{
		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06002CD3 RID: 11475 RVA: 0x000CFB63 File Offset: 0x000CDF63
		// (set) Token: 0x06002CD4 RID: 11476 RVA: 0x000CFB6B File Offset: 0x000CDF6B
		public FieldSerializerProxy Serializer { get; private set; }

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06002CD5 RID: 11477 RVA: 0x000CFB74 File Offset: 0x000CDF74
		public bool IsVideoRecordMode
		{
			get
			{
				return !this.debugButtonText.enabled;
			}
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x000CFB84 File Offset: 0x000CDF84
		private IEnumerator Start()
		{
			yield return SceneManager.Instance.Inject(this);
			yield return ServiceLocator.Instance.Inject(this);
			this.UpdateView();
			this.levelLoader = global::UnityEngine.Object.FindObjectOfType<LevelLoader>();
			this.Serializer = new FieldSerializerProxy(this.levelLoader);
			while (this.levelLoader.MatchEngine == null)
			{
				yield return null;
			}
			this.levelLoader.MatchEngine.onStepBegin.AddListener(new Action<Fields, Move>(this.Serializer.HandleStepBegin));
			this.levelLoader.MatchEngine.AllowFreeSwapping = this.debugSettings.Settings.GetBool(M3Cheats.FreeSwapping.ToString());
			this.boardBorderFactory = global::UnityEngine.Object.FindObjectOfType<BoardBorderFactory>();
			this.backgroundImageSwitcher = global::UnityEngine.Object.FindObjectOfType<BackgroundImageSwitcher>();
			this.boardView = global::UnityEngine.Object.FindObjectOfType<BoardView>();
			this.debugInfoRoot = global::UnityEngine.Object.FindObjectOfType<DebugInfoRoot>();
			yield break;
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000CFBA0 File Offset: 0x000CDFA0
		public void Handle(EnumWithDebugSettings evt)
		{
			if (this.levelRoot.TutorialRunner.IsRunning)
			{
				return;
			}
			switch (evt.value)
			{
			case M3Cheats.LoadFields:
				this.Serializer.LoadFromPlayerPrefs();
				break;
			case M3Cheats.SaveFields:
				FieldSerializer.SaveToDisk(FieldSerializer.DEFAULT_FIELDS_PATH, this.levelLoader.Fields);
				FieldSerializer.SaveToPlayerPrefs(this.levelLoader.Fields);
				break;
			case M3Cheats.PrevStep:
				this.Serializer.PrevStep();
				break;
			case M3Cheats.ThrowExc:
				throw new Exception("Forced exception in M3");
			case M3Cheats.Win:
				this.FinishGame(true, 0);
				break;
			case M3Cheats.Lose:
				this.FinishGame(false, 1);
				break;
			case M3Cheats.LoseNoMoves:
				this.FinishGame(false, 0);
				break;
			case M3Cheats.FreeSwapping:
			{
				string key = evt.ToString();
				this.debugSettings.Settings.SetBool(key, !this.debugSettings.Settings.GetBool(key));
				this.levelLoader.MatchEngine.AllowFreeSwapping = this.debugSettings.Settings.GetBool(key);
				this.UpdateView();
				break;
			}
			case M3Cheats.SetMovesToOne:
				this.levelLoader.ScoringController.AddMoves(-(this.levelLoader.ScoringController.MovesLeft - 1));
				break;
			case M3Cheats.HideDebugUI:
			{
				bool flag = !this.debugButtonText.enabled;
				this.debugButtonImage.color = ((!flag) ? Color.clear : Color.white);
				this.debugButtonText.enabled = flag;
				if (this.debugInfoRoot != null)
				{
					this.debugInfoRoot.gameObject.SetActive(flag);
				}
				break;
			}
			case M3Cheats.ResetBoostCount:
				if (this.levelRoot.boosterUi.gameObject.activeSelf)
				{
					int currentAmount = this.stateService.Resources.Current["boost_hammer"];
					this.levelRoot.boosterUi.SetBoostsAmount("boost_hammer", currentAmount, 3);
					currentAmount = this.stateService.Resources.Current["boost_star"];
					this.levelRoot.boosterUi.SetBoostsAmount("boost_star", currentAmount, 3);
					currentAmount = this.stateService.Resources.Current["boost_rainbow"];
					this.levelRoot.boosterUi.SetBoostsAmount("boost_rainbow", currentAmount, 3);
				}
				break;
			case M3Cheats.ToggleOptionsUi:
				this.levelRoot.boosterUi.ToogleOptionsButtons();
				break;
			case M3Cheats.ToggleBoosterUi:
				this.levelRoot.boosterUi.gameObject.SetActive(!this.levelRoot.boosterUi.gameObject.activeSelf);
				break;
			case M3Cheats.NextBackground:
				this.TryLoadAllThemes();
				if (this.ThemesAvailable())
				{
					this.currentBackground = (this.currentBackground + 1) % this.allThemes.Count;
					this.backgroundImageSwitcher.Show(this.allThemes[this.currentBackground]);
				}
				break;
			case M3Cheats.NextBoard:
				this.TryLoadAllThemes();
				if (this.ThemesAvailable())
				{
					this.currentBoard = (this.currentBoard + 1) % this.allThemes.Count;
					this.boardBorderFactory.Show(this.allThemes[this.currentBoard]);
					this.boardView.RecreateBorders();
				}
				break;
			}
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000CFF0E File Offset: 0x000CE30E
		private bool ThemesAvailable()
		{
			return this.allThemes != null && this.allThemes.Count > 0;
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x000CFF2C File Offset: 0x000CE32C
		private void TryLoadAllThemes()
		{
			if (this.themesLoaded)
			{
				return;
			}
			M3ThemeRegistry m3ThemeRegistry = Resources.Load("M3ThemeRegistry", typeof(M3ThemeRegistry)) as M3ThemeRegistry;
			if (m3ThemeRegistry == null)
			{
				return;
			}
			this.allThemes = new List<LevelTheme>();
			this.AddDefaultTheme();
			foreach (string themeName in m3ThemeRegistry.themes)
			{
				base.StartCoroutine(this.LoadLevelThemeRoutine(themeName));
			}
			this.themesLoaded = true;
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x000CFFDC File Offset: 0x000CE3DC
		private void AddDefaultTheme()
		{
			LevelTheme levelTheme = ScriptableObject.CreateInstance<LevelTheme>();
			levelTheme.name = "climber";
			levelTheme.background = this.backgroundImageSwitcher.GetComponent<SpriteRenderer>().sprite;
			this.allThemes.Add(levelTheme);
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x000D001C File Offset: 0x000CE41C
		private IEnumerator LoadLevelThemeRoutine(string themeName)
		{
			Wooroutine<LevelTheme> tryLoadThemeRoutine = this.levelRoot.TryLoadThemeRoutine(themeName);
			yield return tryLoadThemeRoutine;
			LevelTheme theme = null;
			try
			{
				theme = tryLoadThemeRoutine.ReturnValue;
			}
			catch (Exception ex)
			{
				WoogaDebug.LogWarning(new object[]
				{
					ex.Message
				});
			}
			if (theme != null)
			{
				this.allThemes.Add(theme);
			}
			yield break;
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x000D003E File Offset: 0x000CE43E
		private void OnDisable()
		{
			DOTween.Clear(false);
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x000D0048 File Offset: 0x000CE448
		private void UpdateView()
		{
			M3Cheats[] array = (M3Cheats[])Enum.GetValues(typeof(M3Cheats));
			List<EnumWithDebugSettings> list = new List<EnumWithDebugSettings>();
			foreach (M3Cheats value in array)
			{
				list.Add(new EnumWithDebugSettings(value, this.debugSettings.Settings));
			}
			this.Show(list);
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x000D00AD File Offset: 0x000CE4AD
		private void FinishGame(bool success, int movesTaken)
		{
			this.StopAllRunningAnimations();
			this.levelLoader.ScoringController.FinishLevel(success, movesTaken);
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000D00C7 File Offset: 0x000CE4C7
		private void StopAllRunningAnimations()
		{
			this.levelLoader.BoardView.BoardAnimationController.StopAllCoroutines();
			this.levelLoader.BoardView.StopAllCoroutines();
			this.dooberRoot.StopAllCoroutines();
			DOTween.Clear(false);
		}

		// Token: 0x04005647 RID: 22087
		[SerializeField]
		private LevelLoader levelLoader;

		// Token: 0x04005648 RID: 22088
		[SerializeField]
		private BoardBorderFactory boardBorderFactory;

		// Token: 0x04005649 RID: 22089
		[SerializeField]
		private BackgroundImageSwitcher backgroundImageSwitcher;

		// Token: 0x0400564A RID: 22090
		[SerializeField]
		private BoardView boardView;

		// Token: 0x0400564B RID: 22091
		[SerializeField]
		private Image debugButtonImage;

		// Token: 0x0400564C RID: 22092
		[SerializeField]
		private Text debugButtonText;

		// Token: 0x0400564D RID: 22093
		[SerializeField]
		private DebugInfoRoot debugInfoRoot;

		// Token: 0x0400564E RID: 22094
		[WaitForService(true, true)]
		public DebugSettingsService debugSettings;

		// Token: 0x0400564F RID: 22095
		[WaitForService(true, true)]
		private GameStateService stateService;

		// Token: 0x04005650 RID: 22096
		[WaitForService(true, true)]
		public AssetBundleService abs;

		// Token: 0x04005651 RID: 22097
		[WaitForRoot(false, false)]
		private M3_LevelRoot levelRoot;

		// Token: 0x04005652 RID: 22098
		[WaitForRoot(false, false)]
		private DoobersRoot dooberRoot;

		// Token: 0x04005653 RID: 22099
		private List<LevelTheme> allThemes;

		// Token: 0x04005654 RID: 22100
		private int currentBackground;

		// Token: 0x04005655 RID: 22101
		private int currentBoard;

		// Token: 0x04005656 RID: 22102
		private bool themesLoaded;
	}
}
