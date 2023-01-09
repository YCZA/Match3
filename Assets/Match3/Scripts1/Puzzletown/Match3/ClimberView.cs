using System;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200069F RID: 1695
	public class ClimberView : MonoBehaviour, ITintableView
	{
		// Token: 0x06002A3E RID: 10814 RVA: 0x000C161C File Offset: 0x000BFA1C
		public void SwitchState(ClimberState state)
		{
			bool value = state == ClimberState.Move;
			this.animator.SetBool("isMoving", value);
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x000C163F File Offset: 0x000BFA3F
		private void Awake()
		{
			this.animator = base.GetComponent<Animator>();
			this.meshRenderer = base.GetComponent<MeshRenderer>();
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x000C1659 File Offset: 0x000BFA59
		private void Start()
		{
			this.materialPropertyBlock = new MaterialPropertyBlock();
			this.boardView = base.GetComponentInParent<BoardView>();
			this.boardView.onAnimationFinished.AddListener(new Action(this.HandleAnimationFinished));
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x000C168E File Offset: 0x000BFA8E
		private void OnEnable()
		{
			this.SwitchState(ClimberState.Idle);
			if (this.boardView)
			{
				this.boardView.onAnimationFinished.AddListener(new Action(this.HandleAnimationFinished));
			}
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x000C16C3 File Offset: 0x000BFAC3
		private void OnDisable()
		{
			if (this.boardView)
			{
				this.boardView.onAnimationFinished.RemoveListener(new Action(this.HandleAnimationFinished));
			}
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x000C16F1 File Offset: 0x000BFAF1
		private void HandleAnimationFinished()
		{
			this.SwitchState(ClimberState.Idle);
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x000C16FC File Offset: 0x000BFAFC
		public void ApplyTintColor(Color tint)
		{
			if (this.meshRenderer != null && this.materialPropertyBlock != null)
			{
				this.meshRenderer.GetPropertyBlock(this.materialPropertyBlock);
				this.materialPropertyBlock.SetColor("_Tint", tint);
				this.meshRenderer.SetPropertyBlock(this.materialPropertyBlock);
			}
		}

		// Token: 0x0400539D RID: 21405
		private Animator animator;

		// Token: 0x0400539E RID: 21406
		private const string EVENT_NAME = "isMoving";

		// Token: 0x0400539F RID: 21407
		private BoardView boardView;

		// Token: 0x040053A0 RID: 21408
		private MeshRenderer meshRenderer;

		// Token: 0x040053A1 RID: 21409
		private MaterialPropertyBlock materialPropertyBlock;
	}
}
