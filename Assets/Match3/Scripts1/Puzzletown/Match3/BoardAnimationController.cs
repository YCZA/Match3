using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.DataStructures;
using Match3.Scripts1.Shared.M3Engine;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// eli key point 关卡动画控制
	public class BoardAnimationController : MonoBehaviour
	{
		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x060029AE RID: 10670 RVA: 0x000BD7F4 File Offset: 0x000BBBF4
		// (set) Token: 0x060029AF RID: 10671 RVA: 0x000BD7FC File Offset: 0x000BBBFC
		public int ComboCounter { get; private set; }

		// Token: 0x060029B0 RID: 10672 RVA: 0x000BD808 File Offset: 0x000BBC08
		public void Init(ScoringController scoringController)
		{
			this.scoringController = scoringController;
			this.boardView = base.GetComponent<BoardView>();
			AudioService audioService = base.GetComponentInParent<M3_LevelRoot>().audioService;
			DropAnimator component = base.GetComponent<DropAnimator>();
			foreach (AnimatorBase animatorBase in this.animatorsList.animators)
			{
				animatorBase.boardView = this.boardView;
				animatorBase.audioService = audioService;
				animatorBase.animController = this;
				animatorBase.dropAnimator = component;
			}
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x000BD8AC File Offset: 0x000BBCAC
		public void StartAnimation(List<List<IMatchResult>> results)
		{
			base.StartCoroutine(this.AnimationRoutine(results));
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x000BD8BC File Offset: 0x000BBCBC
		public void ResetDelays()
		{
			for (int i = 0; i < this.fieldDelays.size; i++)
			{
				for (int j = 0; j < this.fieldDelays.size; j++)
				{
					this.fieldDelays[i, j] = 0f;
				}
			}
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x000BD913 File Offset: 0x000BBD13
		public void StartSingleAnimation(List<IMatchResult> results)
		{
			base.StartCoroutine(this.RunSequenceForStep(results));
		}

		// Token: 0x060029B4 RID: 10676 RVA: 0x000BD924 File Offset: 0x000BBD24
		public IEnumerator RunSequenceForStep(List<IMatchResult> results)
		{
			Sequence stepSeq = DOTween.Sequence();
			for (int resultIndex = 0; resultIndex < results.Count; resultIndex++)
			{
				IMatchResult result = results[resultIndex];
				IMultipleStepResult step = result as IMultipleStepResult;
				if (step != null)
				{
					for (int stepIndex = 0; stepIndex < step.Steps.Count; stepIndex++)
					{
						List<IMatchResult> s = step.Steps[stepIndex];
						yield return base.StartCoroutine(this.RunSequenceForStep(s));
					}
				}
				else
				{
					if (result is IMatchGroup && !(result is MatchCandidate))
					{
						this.ComboCounter++;
					}
					for (int i = 0; i < this.animatorsList.animators.Count; i++)
					{
						AnimatorBase animatorBase = this.animatorsList.animators[i];
						((IAnimator)animatorBase).AppendToSequence(stepSeq, result);
					}
					this.boardView.UpdateDirtBorder();
				}
			}
			stepSeq.Play<Sequence>();
			yield return stepSeq.WaitForCompletion();
			this.boardView.UpdateDirtBorder();
			if (this.breakAfterStep)
			{
				global::UnityEngine.Debug.Break();
			}
			yield break;
		}

		// Token: 0x060029B5 RID: 10677 RVA: 0x000BD948 File Offset: 0x000BBD48
		private IEnumerator AnimationRoutine(List<List<IMatchResult>> results)
		{
			for (int index = 0; index < results.Count; index++)
			{
				List<IMatchResult> step = results[index];
				yield return base.StartCoroutine(this.RunSequenceForStep(step));
				this.onStepFinished.Dispatch();
				this.ResetDelays();
			}
			this.Finish();
			yield break;
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x000BD96A File Offset: 0x000BBD6A
		private void Finish()
		{
			this.ComboCounter = 0;
			this.onFinished.Dispatch();
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x000BD980 File Offset: 0x000BBD80
		private void OnDestroy()
		{
			foreach (AnimatorBase animatorBase in this.animatorsList.animators)
			{
				animatorBase.boardView = null;
				animatorBase.audioService = null;
				animatorBase.animController = null;
				animatorBase.dropAnimator = null;
			}
		}

		// Token: 0x0400532E RID: 21294
		public float speed = 1f;

		// Token: 0x0400532F RID: 21295
		public float landingDuration = 1f;

		// Token: 0x04005330 RID: 21296
		[SerializeField]
		private bool breakAfterStep;

		// Token: 0x04005331 RID: 21297
		public readonly Signal onFinished = new Signal();

		// Token: 0x04005332 RID: 21298
		public readonly Signal onStepFinished = new Signal();

		// Token: 0x04005334 RID: 21300
		public AnimatorList animatorsList;

		// Token: 0x04005335 RID: 21301
		public Map<float> fieldDelays = new Map<float>(9);

		// Token: 0x04005336 RID: 21302
		public ScoringController scoringController;

		// Token: 0x04005337 RID: 21303
		private BoardView boardView;
	}
}
