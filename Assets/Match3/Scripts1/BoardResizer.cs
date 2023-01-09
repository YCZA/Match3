using System.Collections;
using UnityEngine;

// Token: 0x02000693 RID: 1683
namespace Match3.Scripts1
{
	public class BoardResizer : AUiAdjuster
	{
		// Token: 0x060029D4 RID: 10708 RVA: 0x000BF3D2 File Offset: 0x000BD7D2
		public void Resize()
		{
			if (this.resizeRoutine == null)
			{
				this.resizeRoutine = base.StartCoroutine(this.ResizeRoutine());
			}
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x000BF3F4 File Offset: 0x000BD7F4
		private IEnumerator ResizeRoutine()
		{
			while (Camera.main == null)
			{
				yield return null;
			}
			Camera cam = Camera.main;
			float reference = 0.5625f;
			float aspect = cam.aspect;
			float factor = reference / aspect;
			if (this.startOrthographicSize == 0f)
			{
				this.startOrthographicSize = cam.orthographicSize;
			}
			if (AUiAdjuster.SimilarOrientation == ScreenOrientation.LandscapeLeft)
			{
				// float num = 1.7777778f;
				float num = 1.7777777f;	 // 16 / 9
				/*if (AUiAdjuster.IsIPhoneX)
			{
				cam.orthographicSize = this.startOrthographicSize / num * LANDSCAPE_BOARD_SIZE_OFFSET;
			}
			else*/ if (aspect > num)	// 大于等于16/9
				{
					// 包含16比9的情况
					cam.orthographicSize = this.startOrthographicSize / num * LANDSCAPE_BOARD_SIZE_OFFSET;
				}
				else
				{
					cam.orthographicSize = this.startOrthographicSize / aspect;
				}
			}
			// else if (aspect < reference)
			// {
			// 	cam.orthographicSize = this.startOrthographicSize * factor;
			// }
			// else
			// {
			// 	cam.orthographicSize = this.startOrthographicSize;
			// }
			this.resizeRoutine = null;
			yield break;
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x000BF40F File Offset: 0x000BD80F
		protected override void AdjustValues()
		{
			this.Resize();
		}

		// Token: 0x04005350 RID: 21328
		// private const float LANDSCAPE_BOARD_SIZE_OFFSET = 1.1f;
		private const float LANDSCAPE_BOARD_SIZE_OFFSET = 1.1f;

		// Token: 0x04005351 RID: 21329
		private float startOrthographicSize;

		// Token: 0x04005352 RID: 21330
		private Coroutine resizeRoutine;
	}
}
