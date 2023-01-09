using System;
using System.Collections;
using System.Linq;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200071F RID: 1823
	public class BoostsHintAnimation : MonoBehaviour
	{
		// Token: 0x06002D15 RID: 11541 RVA: 0x000D11E7 File Offset: 0x000CF5E7
		public void Init(Signal<int> onMovesChanged, Signal<Match3Score> onGameOver, Signal onClicked)
		{
			this.onMovesChanged = onMovesChanged;
			onMovesChanged.AddListener(new Action<int>(this.HandleMovesChanged));
			onGameOver.AddListener(new Action<Match3Score>(this.HandleGameOver));
			onClicked.AddListener(new Action(this.HandleBoostClicked));
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x000D1226 File Offset: 0x000CF626
		public void HandleMovesChanged(int movesLeft)
		{
			if (movesLeft <= this.movesToTrigger && this.animationRoutine == null)
			{
				this.animationRoutine = base.StartCoroutine(this.AnimationRoutine());
			}
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x000D1251 File Offset: 0x000CF651
		public void HandleBoostClicked()
		{
			this.Stop();
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x000D1259 File Offset: 0x000CF659
		private void HandleGameOver(Match3Score score)
		{
			this.Stop();
			this.onMovesChanged.RemoveListener(new Action<int>(this.HandleMovesChanged));
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x000D1278 File Offset: 0x000CF678
		private void Stop()
		{
			if (this.animationRoutine != null)
			{
				base.StopCoroutine(this.animationRoutine);
				this.animationRoutine = null;
			}
			if (this.activeBoosts == null)
			{
				return;
			}
			foreach (BoostView boostView in this.activeBoosts)
			{
				Animation component = boostView.GetComponent<Animation>();
				component.clip.SampleAnimation(boostView.gameObject, component.clip.length);
				component.Stop();
				boostView.Reset();
			}
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x000D1300 File Offset: 0x000CF700
		private IEnumerator AnimationRoutine()
		{
			WaitForSeconds awaitInterval = new WaitForSeconds(this.interval);
			WaitForSeconds awaitWait = new WaitForSeconds(this.waitTime);
			this.activeBoosts = (from b in base.GetComponentsInChildren<BoostView>()
			where b.state == BoostState.Active
			select b).ToArray<BoostView>();
			for (;;)
			{
				for (int i = 0; i < this.activeBoosts.Length; i++)
				{
					Animation animation = this.activeBoosts[i].GetComponent<Animation>();
					animation.Play();
					yield return awaitInterval;
				}
				yield return awaitWait;
			}
			yield break;
		}

		// Token: 0x040056A1 RID: 22177
		public float interval;

		// Token: 0x040056A2 RID: 22178
		public float waitTime;

		// Token: 0x040056A3 RID: 22179
		public int movesToTrigger = 5;

		// Token: 0x040056A4 RID: 22180
		private BoostView[] activeBoosts;

		// Token: 0x040056A5 RID: 22181
		private Coroutine animationRoutine;

		// Token: 0x040056A6 RID: 22182
		private Signal<int> onMovesChanged;
	}
}
