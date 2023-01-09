using DG.Tweening;
using Match3.Scripts1.Shared.M3Engine;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200064D RID: 1613
	public abstract class AAnimator<T> : AnimatorBase, IAnimator where T : IMatchResult
	{
		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x060028D3 RID: 10451 RVA: 0x000B606E File Offset: 0x000B446E
		protected float ModifiedDuration
		{
			get
			{
				return this.duration / this.animController.speed;
			}
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x000B6082 File Offset: 0x000B4482
		public void AppendToSequence(Sequence sequence, IMatchResult matchResult)
		{
			if (this.IsValid(matchResult))
			{
				this.sequence = sequence;
				this.DoAppend((T)((object)matchResult));
			}
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x000B60A3 File Offset: 0x000B44A3
		public virtual void PlayMatchAnimation(GemView gemView, Gem gem)
		{
			this.matchAnimator.PlayMatchAnimation(gemView, gem, this.sequence);
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x000B60B8 File Offset: 0x000B44B8
		public virtual void PlayMatchWithGemAnimation(GemView gemView, Gem gem, IntVector2 target)
		{
			this.matchAnimator.PlayMatchWithGemAnimation(gemView, gem, target, this.sequence);
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x000B60CE File Offset: 0x000B44CE
		protected bool IsValid(IMatchResult matchResult)
		{
			return matchResult is T;
		}

		// Token: 0x060028D8 RID: 10456
		protected abstract void DoAppend(T matchResult);

		// Token: 0x060028D9 RID: 10457 RVA: 0x000B60D9 File Offset: 0x000B44D9
		protected GemView GetGemView(IntVector2 gridPos, bool assert = true)
		{
			return this.boardView.GetGemView(gridPos, assert);
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x000B60E8 File Offset: 0x000B44E8
		protected void BlockSequence(Sequence sequence, GameObject view, float delay)
		{
			sequence.Insert(delay, view.transform.DOLocalRotate(Vector3.zero, this.ModifiedDuration, RotateMode.Fast));
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x000B6109 File Offset: 0x000B4509
		protected void SetFxToFieldview(GameObject prefabInstance, FieldView fieldView)
		{
			prefabInstance.transform.parent = fieldView.transform;
			prefabInstance.transform.position = fieldView.transform.position;
		}

		// Token: 0x040052A5 RID: 21157
		public float duration = 1f;

		// Token: 0x040052A6 RID: 21158
		public MatchAnimator matchAnimator;

		// Token: 0x040052A7 RID: 21159
		protected Sequence sequence;

		// Token: 0x040052A8 RID: 21160
		private DropAnimator _dropAnimator;
	}
}
