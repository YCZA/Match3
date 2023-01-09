using System.Collections;
using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C34 RID: 3124
	[AddComponentMenu("UI/Extensions/UI Tween Scale")]
	public class UI_TweenScale : MonoBehaviour
	{
		// Token: 0x060049AC RID: 18860 RVA: 0x00179072 File Offset: 0x00177472
		private void Awake()
		{
			this.myTransform = base.GetComponent<Transform>();
			this.initScale = this.myTransform.localScale;
			if (this.playAtAwake)
			{
				this.Play();
			}
		}

		// Token: 0x060049AD RID: 18861 RVA: 0x001790A2 File Offset: 0x001774A2
		public void Play()
		{
			base.StartCoroutine("Tween");
		}

		// Token: 0x060049AE RID: 18862 RVA: 0x001790B0 File Offset: 0x001774B0
		private IEnumerator Tween()
		{
			this.myTransform.localScale = this.initScale;
			float t = 0f;
			float maxT = this.animCurve.keys[this.animCurve.length - 1].time;
			while (t < maxT || this.isLoop)
			{
				t += this.speed * Time.deltaTime;
				if (!this.isUniform)
				{
					this.newScale.x = 1f * this.animCurve.Evaluate(t);
					this.newScale.y = 1f * this.animCurveY.Evaluate(t);
					this.myTransform.localScale = this.newScale;
				}
				else
				{
					this.myTransform.localScale = Vector3.one * this.animCurve.Evaluate(t);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060049AF RID: 18863 RVA: 0x001790CB File Offset: 0x001774CB
		public void ResetTween()
		{
			base.StopCoroutine("Tween");
			this.myTransform.localScale = this.initScale;
		}

		// Token: 0x04007000 RID: 28672
		public AnimationCurve animCurve;

		// Token: 0x04007001 RID: 28673
		[Tooltip("Animation speed multiplier")]
		public float speed = 1f;

		// Token: 0x04007002 RID: 28674
		[Tooltip("If true animation will loop, for best effect set animation curve to loop on start and end point")]
		public bool isLoop;

		// Token: 0x04007003 RID: 28675
		[Tooltip("If true animation will start automatically, otherwise you need to call Play() method to start the animation")]
		public bool playAtAwake;

		// Token: 0x04007004 RID: 28676
		[Space(10f)]
		[Header("Non uniform scale")]
		[Tooltip("If true component will scale by the same amount in X and Y axis, otherwise use animCurve for X scale and animCurveY for Y scale")]
		public bool isUniform = true;

		// Token: 0x04007005 RID: 28677
		public AnimationCurve animCurveY;

		// Token: 0x04007006 RID: 28678
		private Vector3 initScale;

		// Token: 0x04007007 RID: 28679
		private Transform myTransform;

		// Token: 0x04007008 RID: 28680
		private Vector3 newScale = Vector3.one;
	}
}
