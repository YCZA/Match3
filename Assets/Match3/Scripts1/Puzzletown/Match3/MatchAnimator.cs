using System;
using System.Collections;
using DG.Tweening;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200067A RID: 1658
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/MatchAnimator")]
	public class MatchAnimator : AAnimator<Match>
	{
		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06002963 RID: 10595 RVA: 0x000BAA4C File Offset: 0x000B8E4C
		private AudioId MatchSoundId
		{
			get
			{
				int value = this.animController.ComboCounter - 1;
				return this.matchSoundIds[Mathf.Clamp(value, 0, this.matchSoundIds.Length - 1)];
			}
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x000BAA7F File Offset: 0x000B8E7F
		public void OnEnable()
		{
			if (this.matchSoundIds.IsNullOrEmptyCollection())
			{
				this.InitMatchedAudioIds();
			}
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x000BAA98 File Offset: 0x000B8E98
		protected override void DoAppend(Match match)
		{
			this.audioService.PlaySFX(this.MatchSoundId, false, false, false);
			foreach (Gem gem in match.Group)
			{
				GemView gemView = base.GetGemView(gem.position, true);
				this.PlayMatchAnimation(gemView, gem);
			}
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x000BAB1C File Offset: 0x000B8F1C
		public override void PlayMatchAnimation(GemView gemView, Gem gem)
		{
			this.PlayMatchAnimation(gemView, gem, this.sequence);
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x000BAB2C File Offset: 0x000B8F2C
		public void PlayMatchAnimation(GemView gemView, Gem gem, Sequence sequence)
		{
			float delay = this.animController.fieldDelays[gem.position];
			this.boardView.StartCoroutine(this.DoPlayMatchAnimation(gemView, gem, sequence, delay));
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x000BAB68 File Offset: 0x000B8F68
		public void PlayMatchWithGemAnimation(GemView gemView, Gem gem, IntVector2 target, Sequence sequence)
		{
			float delay = this.animController.fieldDelays[gem.position];
			this.boardView.StartCoroutine(this.DoPlayMatchWithGemAnimation(gemView, gem, target, sequence, delay));
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x000BABA8 File Offset: 0x000B8FA8
		private IEnumerator DoPlayMatchAnimation(GemView gemView, Gem gem, Sequence sequence, float delay)
		{
			base.BlockSequence(sequence, gemView.gameObject, delay);
			yield return new WaitForSeconds(delay);
			bool isCollectable = this.IsCollectable(gem.color);
			if (isCollectable)
			{
				this.PlayRecipeAnimation(gemView, gem);
			}
			else
			{
				this.MakeGemDisappear(gemView, base.ModifiedDuration, gem.color);
			}
			if (this.IsCollectableForTournament(gem.color))
			{
				TournamentType currentOngoingTournament = this.animController.scoringController.CurrentOngoingTournament;
				this.boardView.StartCoroutine(this.ShowTournamentItemCollectedDoober((Vector3)gem.position, currentOngoingTournament));
			}
			yield break;
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x000BABE0 File Offset: 0x000B8FE0
		private IEnumerator DoPlayMatchWithGemAnimation(GemView gemView, Gem gem, IntVector2 target, Sequence sequence, float delay)
		{
			base.BlockSequence(sequence, gemView.gameObject, delay);
			yield return new WaitForSeconds(delay);
			sequence.Insert(0f, gemView.transform.DOMove((Vector3)target, base.ModifiedDuration / 2f, false).SetEase(this.curve));
			bool isCollectable = this.IsCollectable(gem.color);
			if (isCollectable)
			{
				this.boardView.StartCoroutine(this.StartDoobers(gemView, gem));
			}
			else
			{
				this.boardView.ReleaseView(gemView, base.ModifiedDuration);
			}
			if (this.IsCollectableForTournament(gem.color))
			{
				TournamentType currentOngoingTournament = this.animController.scoringController.CurrentOngoingTournament;
				this.boardView.StartCoroutine(this.ShowTournamentItemCollectedDoober((Vector3)gem.position, currentOngoingTournament));
			}
			yield break;
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x000BAC20 File Offset: 0x000B9020
		private IEnumerator StartDoobers(GemView gemView, Gem gem)
		{
			yield return new WaitForSeconds(this.durationForObjectives / this.animController.speed);
			this.boardView.ReleaseView(gemView, 0f);
			this.boardView.onGemCollected.Dispatch(gemView.transform, gem.color, gem.type);
			yield break;
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x000BAC4C File Offset: 0x000B904C
		private void PlayRecipeAnimation(GemView gemView, Gem gem)
		{
			FieldView fieldView = this.boardView.GetFieldView(gem.position);
			GameObject gameObject = gemView.objectPool.Get(this.recipeMatchBurst);
			gameObject.transform.SetParent(fieldView.transform);
			gameObject.transform.position = (Vector3)gem.position;
			gameObject.SetActive(true);
			gameObject.Release(this.duration + this.fxStayDelay);
			this.boardView.StartCoroutine(this.StartDoobers(gemView, gem));
		}

		// Token: 0x0600296D RID: 10605 RVA: 0x000BACD4 File Offset: 0x000B90D4
		private IEnumerator ShowTournamentItemCollectedDoober(Vector3 startPosition, TournamentType type)
		{
			TournamentItemCollectedView item = this.boardView.GetTournamentCollectedItemView(startPosition, type);
			this.boardView.onTournamentItemCollected.Dispatch(item);
			yield return null;
			item.gameObject.Release();
			yield break;
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x000BAD00 File Offset: 0x000B9100
		private bool IsCollectable(GemColor color)
		{
			return (this.animController.scoringController.AreCoinsCollectable() && color == GemColor.Coins) || this.animController.scoringController.IsObjective(color.ToString().ToLower());
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x000BAD50 File Offset: 0x000B9150
		private bool IsCollectableForTournament(GemColor color)
		{
			TournamentType currentOngoingTournament = this.animController.scoringController.CurrentOngoingTournament;
			return !this.animController.scoringController.LevelWon && TournamentConfig.IsFruitTournament(currentOngoingTournament) && TournamentConfig.IsGemColorMatchingFruitTournament(currentOngoingTournament, color);
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x000BAD98 File Offset: 0x000B9198
		private void MakeGemDisappear(GemView gemView, float duration, GemColor color)
		{
			GameObject gameObject = gemView.objectPool.Get(this.fruitBurst);
			gameObject.transform.SetParent(gemView.transform.parent);
			gameObject.transform.position = gemView.transform.position;
			ParticleStartValues component = gameObject.GetComponent<ParticleStartValues>();
			component.UseColor = (int)color;
			Color currentColor = component.GetCurrentColor();
			ParticleSystem[] particleSystems = component.particleSystems;
			foreach (ParticleSystem particleSystem in particleSystems)
			{
				ParticleSystem.MainModule main = particleSystem.main;
				ParticleSystem.MinMaxGradient startColor = main.startColor;
				startColor.color = currentColor;
				main.startColor = startColor;
			}
			gameObject.SetActive(true);
			gemView.Play(this.clipMatched, duration, true);
			this.boardView.ReleaseView(gemView, duration);
			gameObject.Release(duration + this.fxStayDelay);
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x000BAE78 File Offset: 0x000B9278
		private void InitMatchedAudioIds()
		{
			this.matchSoundIds = new AudioId[7];
			for (int i = 0; i < this.matchSoundIds.Length; i++)
			{
				this.matchSoundIds[i] = (AudioId)Enum.Parse(typeof(AudioId), "Matched" + (i + 1));
			}
		}

		// Token: 0x04005303 RID: 21251
		public AnimationCurve curve;

		// Token: 0x04005304 RID: 21252
		public GameObject fruitBurst;

		// Token: 0x04005305 RID: 21253
		public GameObject recipeMatchBurst;

		// Token: 0x04005306 RID: 21254
		public AnimationClip clipMatched;

		// Token: 0x04005307 RID: 21255
		public float durationForObjectives = 0.28f;

		// Token: 0x04005308 RID: 21256
		public float fxStayDelay = 0.2f;

		// Token: 0x04005309 RID: 21257
		private AudioId[] matchSoundIds;
	}
}
