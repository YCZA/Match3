using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Spine;
using Match3.Scripts1.Spine.Unity;
using Match3.Scripts1.UnityEngine;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;

// Token: 0x02000727 RID: 1831
namespace Match3.Scripts1
{
	public class M3_ObjectivesUiRoot : APtSceneRoot
	{
		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06002D3B RID: 11579 RVA: 0x000D1F73 File Offset: 0x000D0373
		private bool isElsie3d
		{
			get
			{
				return this.characters == null || this.characters.Count == 0;
			}
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x000D1F91 File Offset: 0x000D0391
		protected override void Go()
		{
			this.SetupElsie3DExtras();
			if (base.registeredFirst)
			{
				this.UpdateObjectivesView(this.testObjectives, true, this.testRewards, false);
				this.UpdateMovesView(10);
			}
			this.SetupTournamentScoreDisplay();
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000D1FC8 File Offset: 0x000D03C8
		private void SetupElsie3DExtras()
		{
			if (this.isElsie3d)
			{
				Canvas componentInChildren = base.GetComponentInChildren<Canvas>();
				componentInChildren.worldCamera = Camera.main;
				componentInChildren.sortingLayerName = "UIMask";
				this.AddSlowUpdate(new SlowUpdate(this.UpdateFidget), 10);
			}
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x000D2014 File Offset: 0x000D0414
		private void SetupTournamentScoreDisplay()
		{
			// TournamentType apparentOngoingTournamentType = this.tournamentService.GetApparentOngoingTournamentType();
			TournamentType apparentOngoingTournamentType = TournamentType.Undefined;
			this.displayedTournamentScore = 0;
			foreach (MaterialAmountDisplayLabel materialAmountDisplayLabel in this.tournamentScoreDisplays)
			{
				materialAmountDisplayLabel.gameObject.SetActive(apparentOngoingTournamentType != TournamentType.Undefined);
				materialAmountDisplayLabel.icon.sprite = this.tournamentTaskSprites.GetSprite(apparentOngoingTournamentType);
				materialAmountDisplayLabel.SetValue(this.displayedTournamentScore);
				materialAmountDisplayLabel.InjectAudioService(this.audioService);
			}
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x000D20C0 File Offset: 0x000D04C0
		public void UpdateTournamentScoreView(int score)
		{
			foreach (MaterialAmountDisplayLabel materialAmountDisplayLabel in this.tournamentScoreDisplays)
			{
				materialAmountDisplayLabel.SetValue(score);
			}
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x000D211C File Offset: 0x000D051C
		public void UpdateObjectivesView(MaterialAmount[] objectives, bool hasCoins, MaterialAmount coins, bool forceRefresh = false)
		{
			base.StartCoroutine(this.UpdateObjectivesViewRoutine(objectives, hasCoins, coins, forceRefresh));
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x000D2130 File Offset: 0x000D0530
		private IEnumerator UpdateObjectivesViewRoutine(MaterialAmount[] objectives, bool hasCoins, MaterialAmount coins, bool forceRefresh = false)
		{
			if (forceRefresh)
			{
				yield return null;
			}
			this.CheckMaterialsChanges(objectives, ref this.objectivesOld, this.objectivesSources, forceRefresh);
			if (hasCoins)
			{
				this.CheckMaterialChanges(coins, forceRefresh);
			}
			if (this.coinsRewardViews != null)
			{
				foreach (MaterialAmountView materialAmountView in this.coinsRewardViews)
				{
					materialAmountView.gameObject.SetActive(hasCoins);
				}
			}
			this.dooberDestinations = base.GetComponentsInChildren<MaterialAmountView>();
			float objectivesScale = (objectives.Length <= 1) ? 1.5f : 1f;
			foreach (MaterialsDataSource materialsDataSource in this.objectivesSources)
			{
				materialsDataSource.tableView.transform.localScale = Vector3.one * objectivesScale;
			}
			yield break;
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000D2168 File Offset: 0x000D0568
		public void AnimateCharacter(bool isObjectiveReached, int movesLeft, int sumCascades = 0)
		{
			bool flag = movesLeft <= 3;
			if (isObjectiveReached)
			{
				this.PlayCharacterAnimation(M3_CharacterAnimation.celebration, true);
			}
			else if (flag)
			{
				this.PlayCharacterAnimation(M3_CharacterAnimation.nervous, true);
			}
			else
			{
				this.PlayCharacterAnimation(M3_CharacterAnimation.idle, true);
			}
			if (!flag && sumCascades >= 2)
			{
				this.PlayCharacterAnimation(M3_CharacterAnimation.excited, false);
			}
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x000D21C0 File Offset: 0x000D05C0
		private void CheckMaterialsChanges(MaterialAmount[] newValues, ref MaterialAmount[] oldValues, List<MaterialsDataSource> dataSources, bool forceRefresh = false)
		{
			bool flag = forceRefresh;
			if (!forceRefresh)
			{
				if (oldValues == null)
				{
					oldValues = newValues;
					flag = true;
				}
				int num = 0;
				while (!flag && num < newValues.Length)
				{
					int num2 = newValues[num].amount - oldValues[num].amount;
					if (num2 != 0)
					{
						flag = true;
					}
					num++;
				}
			}
			if (flag)
			{
				foreach (MaterialsDataSource materialsDataSource in dataSources)
				{
					materialsDataSource.ShowOrRefreshExisting(newValues);
				}
			}
			oldValues = newValues;
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x000D2274 File Offset: 0x000D0674
		private void CheckMaterialChanges(MaterialAmount newValue, bool forceRefresh = false)
		{
			if (this.oldCoinsAmount == -1 || newValue.amount != this.oldCoinsAmount || forceRefresh)
			{
				this.oldCoinsAmount = newValue.amount;
				foreach (MaterialAmountView materialAmountView in this.coinsRewardViews)
				{
					materialAmountView.Show(newValue);
				}
			}
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000D2304 File Offset: 0x000D0704
		public RectTransform GetDooberDestination(string type)
		{
			MaterialAmountView materialAmountView = Array.Find<MaterialAmountView>(this.dooberDestinations, (MaterialAmountView v) => this.DoesMaterialViewMatchType(v, type, false));
			if (materialAmountView)
			{
				return materialAmountView.image.rectTransform;
			}
			MaterialAmountView materialAmountView2 = Array.Find<MaterialAmountView>(this.dooberDestinations, (MaterialAmountView v) => this.DoesMaterialViewMatchType(v, type, true));
			return (!materialAmountView2) ? null : materialAmountView2.image.rectTransform;
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000D2388 File Offset: 0x000D0788
		public Transform GetTournamentItemDestination()
		{
			foreach (MaterialAmountDisplayLabel materialAmountDisplayLabel in this.tournamentScoreDisplays)
			{
				if (materialAmountDisplayLabel.gameObject.activeInHierarchy)
				{
					return materialAmountDisplayLabel.icon.transform;
				}
			}
			return this.tournamentScoreDisplays[0].icon.transform;
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x000D2418 File Offset: 0x000D0818
		private bool DoesMaterialViewMatchType(MaterialAmountView view, string type, bool allowCompleted)
		{
			if (!view.Data.type.EqualsIgnoreCase(type))
			{
				return false;
			}
			if (allowCompleted)
			{
				return true;
			}
			ObjectiveView objectiveView = (ObjectiveView)view;
			return !objectiveView || !objectiveView.IsComplete;
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x000D2468 File Offset: 0x000D0868
		public void UpdateMovesView(int movesLeft)
		{
			foreach (TextMeshProUGUI textMeshProUGUI in this.labelMoves)
			{
				textMeshProUGUI.text = movesLeft.ToString();
			}
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x000D24D0 File Offset: 0x000D08D0
		public void UpdateTitleView(string level)
		{
			foreach (TextMeshProUGUI textMeshProUGUI in this.labelTitles)
			{
				textMeshProUGUI.text = level;
			}
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x000D252C File Offset: 0x000D092C
		private void PlayCharacterAnimation(M3_CharacterAnimation animation, bool loop = true)
		{
			if (this.isElsie3d)
			{
				this.PlayCharacterAnimation3D(animation, loop);
			}
			else
			{
				this.PlayCharacterAnimation2D(animation, loop);
			}
		}

		// Token: 0x06002D4B RID: 11595 RVA: 0x000D2550 File Offset: 0x000D0950
		private void PlayCharacterAnimation2D(M3_CharacterAnimation animation, bool loop)
		{
			string text = animation.ToString();
			foreach (SkeletonGraphic skeletonGraphic in this.characters)
			{
				if (skeletonGraphic.gameObject.activeInHierarchy)
				{
					TrackEntry current = skeletonGraphic.AnimationState.GetCurrent(0);
					if (current.Animation.name == text)
					{
						break;
					}
					if (!loop)
					{
						skeletonGraphic.AnimationState.SetAnimation(0, text, false);
						skeletonGraphic.AnimationState.AddAnimation(0, current.Animation, true, 0f);
					}
					else
					{
						skeletonGraphic.AnimationState.SetAnimation(0, text, true);
					}
				}
			}
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x000D2634 File Offset: 0x000D0A34
		private void PlayCharacterAnimation3D(M3_CharacterAnimation animation, bool loop)
		{
			foreach (Animator animator in this.animators)
			{
				if (animator.gameObject.activeInHierarchy)
				{
					if (animation == this.currentState)
					{
						break;
					}
					this.currentState = animation;
					animator.SetTrigger(animation.ToString());
				}
			}
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x000D26CC File Offset: 0x000D0ACC
		private void UpdateFidget()
		{
			foreach (Animator animator in this.animators)
			{
				if (animator.gameObject.activeInHierarchy)
				{
					int num = global::UnityEngine.Random.Range(0, 4);
					if (num < 3)
					{
						animator.SetTrigger(string.Format("fidget{0:D2}", num));
					}
				}
			}
		}

		// Token: 0x040056D7 RID: 22231
		public const int movesCountToPlayNervous = 3;

		// Token: 0x040056D8 RID: 22232
		public const int cascadesCountToPlayExcited = 2;

		// Token: 0x040056D9 RID: 22233
		public const int maxFidgetAnimations = 3;

		// Token: 0x040056DA RID: 22234
		public const int maxFidgetRandom = 4;

		// Token: 0x040056DB RID: 22235
		public const int fidgetChangeWait = 10;

		// Token: 0x040056DC RID: 22236
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x040056DD RID: 22237
		[WaitForService(true, true)]
		public AudioService audioService;

		// Token: 0x040056DE RID: 22238
		// [WaitForService(true, true)]
		// private TournamentService tournamentService;

		// Token: 0x040056DF RID: 22239
		[WaitForService(true, true)]
		private BoostsService boostService;

		// Token: 0x040056E0 RID: 22240
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040056E1 RID: 22241
		[SerializeField]
		private List<MaterialAmountView> coinsRewardViews;

		// Token: 0x040056E2 RID: 22242
		[SerializeField]
		private List<TextMeshProUGUI> labelMoves;

		// Token: 0x040056E3 RID: 22243
		[SerializeField]
		private List<TextMeshProUGUI> labelTitles;

		// Token: 0x040056E4 RID: 22244
		[SerializeField]
		private List<SkeletonGraphic> characters;

		// Token: 0x040056E5 RID: 22245
		[SerializeField]
		private List<MaterialAmountDisplayLabel> tournamentScoreDisplays;

		// Token: 0x040056E6 RID: 22246
		[SerializeField]
		private List<MaterialsDataSource> objectivesSources;

		// Token: 0x040056E7 RID: 22247
		[SerializeField]
		private List<Animator> animators;

		// Token: 0x040056E8 RID: 22248
		[SerializeField]
		private MaterialAmount[] testObjectives;

		// Token: 0x040056E9 RID: 22249
		[SerializeField]
		private MaterialAmount testRewards;

		// Token: 0x040056EA RID: 22250
		[SerializeField]
		private TournamentTaskSpriteManager tournamentTaskSprites;

		// Token: 0x040056EB RID: 22251
		private MaterialAmountView[] dooberDestinations;

		// Token: 0x040056EC RID: 22252
		private MaterialAmount[] objectivesOld;

		// Token: 0x040056ED RID: 22253
		private int oldCoinsAmount = -1;

		// Token: 0x040056EE RID: 22254
		private int displayedTournamentScore;

		// Token: 0x040056EF RID: 22255
		private const float SINGLE_OBJECTIVE_SCALE = 1.5f;

		// Token: 0x040056F0 RID: 22256
		private M3_CharacterAnimation currentState;
	}
}
