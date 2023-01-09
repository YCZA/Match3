using System;
using UnityEngine;

// Token: 0x020004DD RID: 1245
namespace Match3.Scripts1
{
	public class CameraShake : MonoBehaviour
	{
		// Token: 0x060022A5 RID: 8869 RVA: 0x000994BC File Offset: 0x000978BC
		private void OnEnable()
		{
			this.startPosition = Vector3.zero;
			AUiAdjuster.OnScreenOrientationChange.AddListener(new Action<ScreenOrientation>(this.OnOrientationChanged));
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x000994DF File Offset: 0x000978DF
		private void OnDisable()
		{
			AUiAdjuster.OnScreenOrientationChange.RemoveListener(new Action<ScreenOrientation>(this.OnOrientationChanged));
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x000994F8 File Offset: 0x000978F8
		public void PlayShake(int shakeStyleNum)
		{
			if (shakeStyleNum >= this.shakeStyles.Length)
			{
				return;
			}
			this.isAnimating = true;
			this.currentStrength = this.shakeStyles[shakeStyleNum].ShakeStrength;
			this.currentFrequency = this.shakeStyles[shakeStyleNum].ShakeFrequency;
			this.currentAnimationLength = this.shakeStyles[shakeStyleNum].AnimationLength;
			this.currentIsLooping = this.shakeStyles[shakeStyleNum].Loop;
			this.startTime = Time.realtimeSinceStartup;
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x00099572 File Offset: 0x00097972
		public void PlayShake(CameraShakeStyle shakeStyle)
		{
			this.isAnimating = true;
			this.currentStrength = shakeStyle.ShakeStrength;
			this.currentFrequency = shakeStyle.ShakeFrequency;
			this.currentAnimationLength = shakeStyle.AnimationLength;
			this.startTime = Time.realtimeSinceStartup;
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x000995AC File Offset: 0x000979AC
		private void Update()
		{
			if (this.isAnimating)
			{
				if (this.startPosition == Vector3.zero)
				{
					this.startPosition = base.transform.position;
				}
				float num = (Time.realtimeSinceStartup - this.startTime) / this.currentAnimationLength;
				if (num > 1f)
				{
					num = 1f;
					if (this.currentIsLooping)
					{
						this.startTime = Time.realtimeSinceStartup;
					}
					else
					{
						this.isAnimating = false;
					}
				}
				float d = this.currentStrength.Evaluate(num);
				float num2 = this.currentFrequency.Evaluate(num);
				if (Time.realtimeSinceStartup > this.lastPositionChange + num2)
				{
					this.lastPositionChange = Time.realtimeSinceStartup;
					this.newPosition = Vector3.Normalize(new Vector3(global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f))) * d;
					this.newPosition += this.startPosition;
				}
				base.transform.position = Vector3.Lerp(base.transform.position, this.newPosition, num);
			}
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x000996E4 File Offset: 0x00097AE4
		private void OnOrientationChanged(ScreenOrientation orientation)
		{
			this.isAnimating = false;
			this.startPosition = Vector3.zero;
		}

		// Token: 0x04004E38 RID: 20024
		public CameraShakeStyle[] shakeStyles;

		// Token: 0x04004E39 RID: 20025
		private float startTime;

		// Token: 0x04004E3A RID: 20026
		private bool isAnimating;

		// Token: 0x04004E3B RID: 20027
		private AnimationCurve currentStrength;

		// Token: 0x04004E3C RID: 20028
		private AnimationCurve currentFrequency;

		// Token: 0x04004E3D RID: 20029
		private float currentAnimationLength;

		// Token: 0x04004E3E RID: 20030
		private bool currentIsLooping;

		// Token: 0x04004E3F RID: 20031
		private Vector3 startPosition;

		// Token: 0x04004E40 RID: 20032
		private Vector3 newPosition;

		// Token: 0x04004E41 RID: 20033
		private float lastPositionChange;
	}
}
