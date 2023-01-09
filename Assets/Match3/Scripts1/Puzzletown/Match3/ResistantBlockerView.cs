using System;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006CB RID: 1739
	public class ResistantBlockerView : ABlinkingAnimatedView, ITintableView
	{
		// Token: 0x06002B69 RID: 11113 RVA: 0x000C743D File Offset: 0x000C583D
		public void AnimateIdle(int hp)
		{
			this.AnimateState(this.GetIdleState(hp));
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x000C744C File Offset: 0x000C584C
		protected override void Awake()
		{
			this.materialPropertyBlock = new MaterialPropertyBlock();
			base.Awake();
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x000C7460 File Offset: 0x000C5860
		private void AnimateState(ResistantBlockerView.ResistantBlockerState state)
		{
			if (this.currentState == state)
			{
				return;
			}
			string name = Enum.GetName(typeof(ResistantBlockerView.ResistantBlockerState), state);
			this.animator.SetBool(name, true);
			this.currentState = state;
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x000C74A4 File Offset: 0x000C58A4
		private ResistantBlockerView.ResistantBlockerState GetIdleState(int currentHp)
		{
			ResistantBlockerView.ResistantBlockerState result = ResistantBlockerView.ResistantBlockerState.None;
			if (currentHp != 1)
			{
				if (currentHp != 2)
				{
					if (currentHp == 3)
					{
						result = ResistantBlockerView.ResistantBlockerState.IdleHp3;
					}
				}
				else
				{
					result = ResistantBlockerView.ResistantBlockerState.IdleHp2;
				}
			}
			else
			{
				result = ResistantBlockerView.ResistantBlockerState.IdleHp1;
			}
			return result;
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x000C74E4 File Offset: 0x000C58E4
		public void ApplyTintColor(Color tint)
		{
			MeshRenderer component = base.GetComponent<MeshRenderer>();
			if (component != null && this.materialPropertyBlock != null)
			{
				component.GetPropertyBlock(this.materialPropertyBlock);
				this.materialPropertyBlock.SetColor("_Tint", tint);
				component.SetPropertyBlock(this.materialPropertyBlock);
			}
		}

		// Token: 0x04005483 RID: 21635
		private ResistantBlockerView.ResistantBlockerState currentState;

		// Token: 0x04005484 RID: 21636
		private MaterialPropertyBlock materialPropertyBlock;

		// Token: 0x020006CC RID: 1740
		private enum ResistantBlockerState
		{
			// Token: 0x04005486 RID: 21638
			None,
			// Token: 0x04005487 RID: 21639
			IdleHp3,
			// Token: 0x04005488 RID: 21640
			IdleHp2,
			// Token: 0x04005489 RID: 21641
			IdleHp1
		}
	}
}
