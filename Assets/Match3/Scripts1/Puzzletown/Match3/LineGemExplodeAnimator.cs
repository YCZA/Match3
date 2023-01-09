using System.Collections;
using Match3.Scripts1.Audio;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000678 RID: 1656
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/LineGemExplodeAnimator")]
	public class LineGemExplodeAnimator : AAnimator<LineGemExplosion>
	{
		// Token: 0x06002957 RID: 10583 RVA: 0x000BA4B8 File Offset: 0x000B88B8
		public static IntVector2 GetStartPosition(IntVector2 pos, int minBlockingPos, bool isHorizontal)
		{
			if (isHorizontal)
			{
				pos.x = ((minBlockingPos != -1) ? minBlockingPos : (pos.x - LineGemExplodeAnimator.DEFAULT_WAVE_LENGTH));
			}
			else
			{
				pos.y = ((minBlockingPos != -1) ? minBlockingPos : (pos.y - LineGemExplodeAnimator.DEFAULT_WAVE_LENGTH));
			}
			return pos;
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x000BA514 File Offset: 0x000B8914
		public static IntVector2 GetEndPosition(IntVector2 pos, int maxBlockingPos, bool isHorizontal)
		{
			if (isHorizontal)
			{
				pos.x = ((maxBlockingPos != -1) ? maxBlockingPos : (pos.x + LineGemExplodeAnimator.DEFAULT_WAVE_LENGTH));
			}
			else
			{
				pos.y = ((maxBlockingPos != -1) ? maxBlockingPos : (pos.y + LineGemExplodeAnimator.DEFAULT_WAVE_LENGTH));
			}
			return pos;
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x000BA56F File Offset: 0x000B896F
		public float SetUpWave(SinusLineRenderer sinusRenderer, IntVector2 target, bool isHorizontal)
		{
			sinusRenderer.targetPosition = (Vector3)target;
			sinusRenderer.SetupLineRenderer();
			sinusRenderer.StayTime *= base.ModifiedDuration;
			return sinusRenderer.StayTime;
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x000BA59C File Offset: 0x000B899C
		protected override void DoAppend(LineGemExplosion explosion)
		{
			float num = this.animController.fieldDelays[explosion.Center];
			float num2 = this.cascadeDelay / this.animController.speed;
			foreach (IntVector2 intVector in explosion.HighlightPositions)
			{
				int num3 = IntVector2.SimpleDistance(explosion.Center, intVector);
				this.animController.fieldDelays[intVector] = num + num2 * (float)num3;
			}
			foreach (Gem gem in explosion.Group)
			{
				GemView gemView = base.GetGemView(gem.position, true);
				this.PlayMatchAnimation(gemView, gem);
			}
			this.boardView.StartCoroutine(this.DelayedDoAppend(explosion, num));
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x000BA6BC File Offset: 0x000B8ABC
		private IEnumerator DelayedDoAppend(LineGemExplosion explosion, float delay)
		{
			yield return new WaitForSeconds(delay);
			this.AttachFx(explosion.Center, explosion.blockingPosStart, explosion.blockingPosEnd, explosion.gem.type, delay);
			this.audioService.PlaySFX(AudioId.LineGemExplode, false, false, false);
			yield break;
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x000BA6E8 File Offset: 0x000B8AE8
		public void AttachFx(IntVector2 centerPosition, int minPos, int maxPos, GemType type, float pastDelay)
		{
			bool flag = type == GemType.LineHorizontal;
			Vector3 pos = (Vector3)centerPosition;
			GameObject gameObject = this.SetupPrefab(this.centerExplosionFx, pos);
			Quaternion rotation = (!flag) ? this.verticalRotation : this.horizontalRotation;
			gameObject.transform.rotation = rotation;
			gameObject.Release(base.ModifiedDuration * this.releaseCenterVxPercent);
			GameObject gameObject2 = this.SetupPrefab(this.linegemExplosionFx, pos);
			SinusLineRenderer[] componentsInChildren = gameObject2.GetComponentsInChildren<SinusLineRenderer>();
			IntVector2 intVector = LineGemExplodeAnimator.GetStartPosition(centerPosition, minPos, flag);
			float num = this.SetUpWave(componentsInChildren[0], intVector, flag);
			this.boardView.StartCoroutine(this.SetUpExplosionAtEnd(minPos, intVector, pastDelay));
			intVector = LineGemExplodeAnimator.GetEndPosition(centerPosition, maxPos, flag);
			float num2 = this.SetUpWave(componentsInChildren[1], intVector, flag);
			this.boardView.StartCoroutine(this.SetUpExplosionAtEnd(maxPos, intVector, pastDelay));
			float delay = (num <= num2) ? num2 : num;
			gameObject2.Release(delay);
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x000BA7DC File Offset: 0x000B8BDC
		public IEnumerator SetUpExplosionAtEnd(int outerPosition, IntVector2 position, float pastDelay)
		{
			if (outerPosition != -1)
			{
				yield return new WaitForSeconds(this.animController.fieldDelays[position] - pastDelay);
				this.SetupPrefab(this.endExplosionFx, (Vector3)position);
			}
			yield break;
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x000BA80C File Offset: 0x000B8C0C
		private GameObject SetupPrefab(GameObject prefab, Vector3 pos)
		{
			GameObject gameObject = this.boardView.objectPool.Get(prefab);
			gameObject.transform.SetParent(this.boardView.transform);
			gameObject.transform.localPosition = pos;
			return gameObject;
		}

		// Token: 0x040052FB RID: 21243
		[SerializeField]
		private GameObject linegemExplosionFx;

		// Token: 0x040052FC RID: 21244
		[SerializeField]
		private GameObject centerExplosionFx;

		// Token: 0x040052FD RID: 21245
		[SerializeField]
		private GameObject endExplosionFx;

		// Token: 0x040052FE RID: 21246
		[SerializeField]
		private float cascadeDelay = 0.07225f;

		// Token: 0x040052FF RID: 21247
		[SerializeField]
		private float releaseCenterVxPercent = 0.8f;

		// Token: 0x04005300 RID: 21248
		private static int DEFAULT_WAVE_LENGTH = 8;

		// Token: 0x04005301 RID: 21249
		private Quaternion horizontalRotation = new Quaternion(0f, 0f, 0f, 1f);

		// Token: 0x04005302 RID: 21250
		private Quaternion verticalRotation = new Quaternion(0f, 0f, Mathf.Sqrt(0.5f), Mathf.Sqrt(0.5f));
	}
}
