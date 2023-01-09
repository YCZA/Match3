using System;
using System.Collections;
using DG.Tweening;
using Match3.Scripts1.Spine.Unity;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;
using UnityEngine.UI;
using Shared.Pooling;

// Token: 0x02000918 RID: 2328
namespace Match3.Scripts1
{
	public class Doober : MonoBehaviour
	{
		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x060038B8 RID: 14520 RVA: 0x00117440 File Offset: 0x00115840
		// (set) Token: 0x060038B9 RID: 14521 RVA: 0x00117447 File Offset: 0x00115847
		public static int ActiveDoobers { get; private set; }

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x060038BA RID: 14522 RVA: 0x0011744F File Offset: 0x0011584F
		private float Progress
		{
			get
			{
				return this.easing.Evaluate(Mathf.Clamp01(this.time / this.total));
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x060038BB RID: 14523 RVA: 0x0011746E File Offset: 0x0011586E
		private Vector3 Curve
		{
			get
			{
				return this.curve * this.elevation.Evaluate(this.Progress);
			}
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x0011748C File Offset: 0x0011588C
		public void ApplyTrajectory(Doober.Location start, Doober.Location target, Vector2 scale)
		{
			this.start = start;
			this.target = target;
			this.time = 0f;
			this.total = this.flyTime;
			Vector3 a = this.start.Project();
			Vector3 b = this.target.Project();
			a.Scale(new Vector3(1f / (float)Screen.width, 1f / (float)Screen.height));
			b.Scale(new Vector3(1f / (float)Screen.width, 1f / (float)Screen.height));
			float magnitude = (a - b).magnitude;
			Vector2 vector = new Vector2((float)Screen.width * this.curveValue.x, (float)Screen.height * this.curveValue.y);
			this.curve = new Vector3(Mathf.Lerp(vector.x, -vector.x, a.x), Mathf.Lerp(vector.y, -vector.y, a.y)) * magnitude;
			this.total *= Mathf.Lerp(0.5f, 1f, magnitude);
			this.UpdatePosition();
			this.scaleRoutine = base.StartCoroutine(this.Scaling(this.total, scale));
		}

		// Token: 0x060038BD RID: 14525 RVA: 0x001175DC File Offset: 0x001159DC
		private IEnumerator Scaling(float time, Vector2 scale)
		{
			if (this.scaleUpTime > 0f)
			{
				base.transform.localScale = Vector3.zero;
				base.transform.DOScale(scale, this.scaleUpTime);
			}
			else
			{
				base.transform.DOScale(this.targetScale * this.scaleAmount, time * this.halfTime);
				base.transform.DOScale(this.targetScale, time * (1f - this.halfTime)).SetDelay(time * this.halfTime);
				base.transform.localScale = scale;
			}
			yield return new WaitForSeconds(time);
			if (this.onFinished != null && this.onFinished.HasListener())
			{
				this.onFinished.Dispatch(this.amount);
				this.onFinished.RemoveAllListeners();
			}
			base.transform.DOScale(Vector3.zero, this.scaleDownTime);
			yield return new WaitForSeconds(this.scaleDownTime);
			base.gameObject.Release();
			this.scaleRoutine = null;
			yield break;
		}

		// Token: 0x060038BE RID: 14526 RVA: 0x00117605 File Offset: 0x00115A05
		public void Update()
		{
			this.time += Time.deltaTime;
			this.UpdatePosition();
		}

		// Token: 0x060038BF RID: 14527 RVA: 0x0011761F File Offset: 0x00115A1F
		private void UpdatePosition()
		{
			base.transform.position = Vector3.Lerp(this.start.Project(), this.target.Project(), this.Progress) + this.Curve;
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x00117658 File Offset: 0x00115A58
		private void OnOrientationChange(ScreenOrientation screenOrientation)
		{
			if (this != null)
			{
				this.time = 0f;
				if (this.scaleRoutine != null)
				{
					base.StopCoroutine(this.scaleRoutine);
					this.scaleRoutine = null;
					if (this.onFinished != null && this.onFinished.HasListener())
					{
						this.onFinished.Dispatch(this.amount);
						this.onFinished.RemoveAllListeners();
					}
				}
				if (base.transform != null)
				{
					base.transform.DOKill(false);
				}
				if (base.gameObject)
				{
					base.gameObject.Release();
				}
			}
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x0011770A File Offset: 0x00115B0A
		private void OnEnable()
		{
			Doober.ActiveDoobers++;
			AUiAdjuster.OnScreenOrientationChange.AddListener(new Action<ScreenOrientation>(this.OnOrientationChange));
			this.initialScale = base.transform.localScale;
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x0011773F File Offset: 0x00115B3F
		private void OnDisable()
		{
			Doober.ActiveDoobers--;
			WooroutineRunner.StartCoroutine(this.OnDisableRoutine(), null);
			base.transform.localScale = this.initialScale;
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x0011776C File Offset: 0x00115B6C
		private IEnumerator OnDisableRoutine()
		{
			yield return null;
			AUiAdjuster.OnScreenOrientationChange.RemoveListener(new Action<ScreenOrientation>(this.OnOrientationChange));
			yield break;
		}

		// Token: 0x0400610E RID: 24846
		public const string ANIMATION_NAME = "Doober";

		// Token: 0x0400610F RID: 24847
		private const float MIN_TIME = 0.5f;

		// Token: 0x04006111 RID: 24849
		public Image image;

		// Token: 0x04006112 RID: 24850
		public SkeletonGraphic animatedView;

		// Token: 0x04006113 RID: 24851
		public AnimationCurve easing;

		// Token: 0x04006114 RID: 24852
		public AnimationCurve elevation;

		// Token: 0x04006115 RID: 24853
		public float scaleUpTime = 0.2f;

		// Token: 0x04006116 RID: 24854
		public float scaleDownTime = 0.1f;

		// Token: 0x04006117 RID: 24855
		public Vector2 curveValue;

		// Token: 0x04006118 RID: 24856
		public float flyTime = 2f;

		// Token: 0x04006119 RID: 24857
		public int amount;

		// Token: 0x0400611A RID: 24858
		public Signal<int> onFinished = new Signal<int>();

		// Token: 0x0400611B RID: 24859
		public float halfTime = 0.66f;

		// Token: 0x0400611C RID: 24860
		public float scaleAmount = 1.4f;

		// Token: 0x0400611D RID: 24861
		[NonSerialized]
		public Vector3 targetScale = Vector3.one;

		// Token: 0x0400611E RID: 24862
		private Doober.Location start;

		// Token: 0x0400611F RID: 24863
		private Doober.Location target;

		// Token: 0x04006120 RID: 24864
		private Vector3 curve;

		// Token: 0x04006121 RID: 24865
		private float time;

		// Token: 0x04006122 RID: 24866
		private float total;

		// Token: 0x04006123 RID: 24867
		private Coroutine scaleRoutine;

		// Token: 0x04006124 RID: 24868
		private Vector3 initialScale;

		// Token: 0x02000919 RID: 2329
		public struct Location
		{
			// Token: 0x060038C4 RID: 14532 RVA: 0x00117788 File Offset: 0x00115B88
			public Location(Vector3 worldPosition, Transform transform)
			{
				this.worldPosition = worldPosition;
				if (transform is RectTransform && transform.gameObject.activeInHierarchy)
				{
					this.camera = transform.GetComponentInParent<Canvas>().worldCamera;
				}
				else
				{
					this.camera = Camera.main;
				}
			}

			// Token: 0x060038C5 RID: 14533 RVA: 0x001177D8 File Offset: 0x00115BD8
			public Vector3 Project()
			{
				if (this.camera && this.camera.transform)
				{
					return this.camera.WorldToScreenPoint(this.worldPosition);
				}
				return this.worldPosition;
			}

			// Token: 0x04006125 RID: 24869
			private Vector3 worldPosition;

			// Token: 0x04006126 RID: 24870
			private Camera camera;
		}
	}
}
