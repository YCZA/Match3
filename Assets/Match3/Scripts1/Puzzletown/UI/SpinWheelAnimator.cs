using System.Collections;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000883 RID: 2179
	public class SpinWheelAnimator : MonoBehaviour
	{
		// Token: 0x06003582 RID: 13698 RVA: 0x00100C1D File Offset: 0x000FF01D
		private void Start()
		{
			this.spinnerSparkleMaterial.color = Color.clear;
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x00100C2F File Offset: 0x000FF02F
		private void OnDestroy()
		{
			this.spinnerSparkleMaterial.color = Color.white;
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x00100C44 File Offset: 0x000FF044
		public IEnumerator AnimateSpinWheel(int stopIndex)
		{
			this.wheelRotationAnimation["WheelRotation"].wrapMode = WrapMode.Loop;
			this.wheelRotationAnimation["WheelRotation"].speed = 0f;
			this.wheelRotationAnimation["WheelRotation"].enabled = true;
			this.wheelRotationAnimation["WheelRotation"].weight = 1f;
			float elapsedTime = 0f;
			float ratio = 0f;
			float startTime = this.wheelRotationAnimation["WheelRotation"].time;
			float endTime = 16f + 3.5f / (float)this.segments * (float)stopIndex;
			float closeCallExtraSpin = global::UnityEngine.Random.Range(-0.05f, 0.3f);
			endTime += closeCallExtraSpin;
			while (ratio < 1f)
			{
				elapsedTime += Time.deltaTime;
				ratio = elapsedTime / 7f;
				float curveEvaluation = this.spinCurve.Evaluate(ratio);
				this.wheelRotationAnimation["WheelRotation"].time = Mathf.Lerp(startTime, endTime, curveEvaluation);
				this.spinnerSparkleMaterial.color = new Color(1f, 1f, 1f, 1f - curveEvaluation);
				yield return null;
			}
			this.spinnerSparkleMaterial.color = Color.clear;
			this.wheelRotationAnimation["WheelRotation"].time = endTime - 16f;
			yield break;
		}

		// Token: 0x04005D62 RID: 23906
		private const float WHEEL_SLOWDOWN_ANIMATION_TIME = 7f;

		// Token: 0x04005D63 RID: 23907
		private const float WHEEL_ANIMATION_CLIP_LENGTH = 4f;

		// Token: 0x04005D64 RID: 23908
		private const float WHEEL_NUMBER_OF_TURNS_BEFORE_PRICE = 4f;

		// Token: 0x04005D65 RID: 23909
		private const string WHEEL_ANIMATION_CLIP_NAME = "WheelRotation";

		// Token: 0x04005D66 RID: 23910
		[SerializeField]
		private int segments = 4;

		// Token: 0x04005D67 RID: 23911
		[Header("Spin Animation")]
		[SerializeField]
		private Animation wheelRotationAnimation;

		// Token: 0x04005D68 RID: 23912
		[SerializeField]
		private AnimationCurve spinCurve;

		// Token: 0x04005D69 RID: 23913
		[SerializeField]
		private Material spinnerSparkleMaterial;
	}
}
