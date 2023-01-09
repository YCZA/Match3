using System.Collections;
using Match3.Scripts1.Audio;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000677 RID: 1655
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/LineGemBombExplodeAnimator")]
	public class LineGemBombExplodeAnimator : AAnimator<LinegemBombExplosion>
	{
		// Token: 0x06002951 RID: 10577 RVA: 0x000B9F24 File Offset: 0x000B8324
		protected override void DoAppend(LinegemBombExplosion explosion)
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
			this.AttachShockwaves(explosion);
			this.audioService.PlaySFX(AudioId.ExplodeLineBomb, false, false, false);
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x000BA048 File Offset: 0x000B8448
		private void AttachShockwaves(LinegemBombExplosion explosion)
		{
			IntVector2 position = explosion.gem.position;
			Vector3 pos = (Vector3)position;
			GameObject go = this.SetupPrefab(this.centerExplosionFx, pos);
			go.Release(base.ModifiedDuration);
			IntVector2[] array = new IntVector2[]
			{
				position + IntVector2.Up,
				position,
				position + IntVector2.Down
			};
			for (int i = 0; i < array.Length; i++)
			{
				IntVector2 pos2 = array[i];
				GameObject gameObject = this.SetupPrefab(this.linegemExplosionFx, (Vector3)pos2);
				SinusLineRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<SinusLineRenderer>();
				int num = explosion.blockingPositions[i, 0];
				IntVector2 intVector = LineGemExplodeAnimator.GetStartPosition(pos2, num, true);
				float num2 = this.SetUpWave(componentsInChildren[0], intVector, true);
				this.boardView.StartCoroutine(this.SetUpExplosionAtEnd(num, intVector));
				int num3 = explosion.blockingPositions[i, 1];
				intVector = LineGemExplodeAnimator.GetEndPosition(pos2, num3, true);
				float num4 = this.SetUpWave(componentsInChildren[1], intVector, true);
				this.boardView.StartCoroutine(this.SetUpExplosionAtEnd(num3, intVector));
				float delay = (num2 <= num4) ? num4 : num2;
				gameObject.Release(delay);
			}
			IntVector2[] array2 = new IntVector2[]
			{
				position + IntVector2.Left,
				position,
				position + IntVector2.Right
			};
			for (int j = 0; j < array2.Length; j++)
			{
				IntVector2 pos3 = array2[j];
				GameObject gameObject2 = this.SetupPrefab(this.linegemExplosionFx, (Vector3)pos3);
				SinusLineRenderer[] componentsInChildren2 = gameObject2.GetComponentsInChildren<SinusLineRenderer>();
				int num5 = explosion.blockingPositions[j, 3];
				IntVector2 intVector2 = LineGemExplodeAnimator.GetStartPosition(pos3, num5, false);
				float num6 = this.SetUpWave(componentsInChildren2[0], intVector2, false);
				this.boardView.StartCoroutine(this.SetUpExplosionAtEnd(num5, intVector2));
				int num7 = explosion.blockingPositions[j, 2];
				intVector2 = LineGemExplodeAnimator.GetEndPosition(pos3, num7, false);
				float num8 = this.SetUpWave(componentsInChildren2[1], intVector2, false);
				this.boardView.StartCoroutine(this.SetUpExplosionAtEnd(num7, intVector2));
				float delay2 = (num6 <= num8) ? num8 : num6;
				gameObject2.Release(delay2);
			}
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x000BA2D6 File Offset: 0x000B86D6
		public float SetUpWave(SinusLineRenderer sinusRenderer, IntVector2 target, bool isHorizontal)
		{
			sinusRenderer.targetPosition = (Vector3)target;
			sinusRenderer.SetupLineRenderer();
			sinusRenderer.StayTime *= base.ModifiedDuration;
			return sinusRenderer.StayTime;
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x000BA304 File Offset: 0x000B8704
		public IEnumerator SetUpExplosionAtEnd(int outerPosition, IntVector2 position)
		{
			if (outerPosition != -1)
			{
				yield return new WaitForSeconds(this.animController.fieldDelays[position]);
				this.SetupPrefab(this.endExplosionFx, (Vector3)position);
			}
			yield break;
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x000BA330 File Offset: 0x000B8730
		private GameObject SetupPrefab(GameObject prefab, Vector3 pos)
		{
			GameObject gameObject = this.boardView.objectPool.Get(prefab);
			gameObject.transform.SetParent(this.boardView.transform);
			gameObject.transform.localPosition = pos;
			return gameObject;
		}

		// Token: 0x040052F6 RID: 21238
		public float spreadingLineGemsDuration = 0.35f;

		// Token: 0x040052F7 RID: 21239
		public GameObject linegemExplosionFx;

		// Token: 0x040052F8 RID: 21240
		public GameObject centerExplosionFx;

		// Token: 0x040052F9 RID: 21241
		public GameObject endExplosionFx;

		// Token: 0x040052FA RID: 21242
		public float cascadeDelay = 0.0765f;
	}
}
