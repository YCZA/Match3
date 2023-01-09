using System.Collections;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000664 RID: 1636
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Fields/CrateAnimator")]
	public class CrateAnimator : AAnimator<CrateExplosion>
	{
		// Token: 0x0600291E RID: 10526 RVA: 0x000B8621 File Offset: 0x000B6A21
		private void OnEnable()
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}

		// Token: 0x0600291F RID: 10527 RVA: 0x000B8630 File Offset: 0x000B6A30
		protected override void DoAppend(CrateExplosion explosion)
		{
			FieldView fieldView = this.boardView.GetFieldView(explosion.Position);
			float num = this.animController.fieldDelays[explosion.CreatedFrom];
			if (explosion.Position != explosion.CreatedFrom)
			{
				float num2 = IntVector2.Distance(explosion.Position, explosion.CreatedFrom);
				num += this.cascadeDelay / this.animController.speed * num2;
			}
			base.BlockSequence(this.sequence, fieldView.gameObject, num);
			this.boardView.StartCoroutine(this.DelayedDoAppend(explosion, fieldView, num));
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x000B86D4 File Offset: 0x000B6AD4
		private IEnumerator DelayedDoAppend(CrateExplosion explosion, FieldView fieldView, float delay)
		{
			yield return new WaitForSeconds(delay);
			GemColor crateColor = Crate.GetColor(explosion.NewAmount);
			int newHp = Crate.GetHp(explosion.NewAmount);
			bool needsTint = crateColor != GemColor.Undefined;
			GameObject explosionFx = this.GetExplosionFx(needsTint, newHp);
			if (needsTint)
			{
				Renderer[] componentsInChildren = explosionFx.GetComponentsInChildren<Renderer>(true);
				float valueForLookUpTableColumn = FieldView.GetValueForLookUpTableColumn(crateColor);
				foreach (Renderer renderer in componentsInChildren)
				{
					renderer.GetPropertyBlock(this.propertyBlock);
					this.propertyBlock.SetFloat("_ColorColumn", valueForLookUpTableColumn);
					renderer.SetPropertyBlock(this.propertyBlock);
				}
			}
			base.SetFxToFieldview(this.boardView.objectPool.Get(this.explosionDustFx), fieldView);
			base.SetFxToFieldview(explosionFx, fieldView);
			fieldView.SetCrate(explosion.NewAmount);
			this.audioService.PlaySFX(AudioId.CrateHit, false, false, false);
			yield break;
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x000B8704 File Offset: 0x000B6B04
		private GameObject GetExplosionFx(bool needsTint, int newHp)
		{
			if (needsTint)
			{
				return this.boardView.objectPool.Get(this.tintableCrateExplosionFx[newHp]);
			}
			return this.boardView.objectPool.Get(this.crateExplosionFx[newHp]);
		}

		// Token: 0x040052D3 RID: 21203
		public float cascadeDelay = 0.0425f;

		// Token: 0x040052D4 RID: 21204
		public GameObject explosionDustFx;

		// Token: 0x040052D5 RID: 21205
		public GameObject[] crateExplosionFx;

		// Token: 0x040052D6 RID: 21206
		public GameObject[] tintableCrateExplosionFx;

		// Token: 0x040052D7 RID: 21207
		private MaterialPropertyBlock propertyBlock;
	}
}
