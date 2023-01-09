using UnityEngine;

// Token: 0x020006B7 RID: 1719
namespace Match3.Scripts1
{
	public abstract class ATween : MonoBehaviour
	{
		// Token: 0x06002AE7 RID: 10983 RVA: 0x000C32CD File Offset: 0x000C16CD
		private void Awake()
		{
			base.enabled = this.autoPlay;
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x000C32DB File Offset: 0x000C16DB
		public void Show(float duration, float delay = 0f, bool isLoop = false)
		{
			base.enabled = true;
			this.isLoop = isLoop;
			this.startTime = Time.time + delay;
			this.duration = duration;
			this.Show();
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x000C3305 File Offset: 0x000C1705
		public void Stop()
		{
			base.enabled = false;
			this.DoUpdate(1f);
		}

		// Token: 0x06002AEA RID: 10986
		protected abstract void Show();

		// Token: 0x06002AEB RID: 10987
		protected abstract void Finish();

		// Token: 0x06002AEC RID: 10988
		protected abstract void DoUpdate(float value);

		// Token: 0x06002AED RID: 10989 RVA: 0x000C331C File Offset: 0x000C171C
		private void Update()
		{
			float num = (Time.time - this.startTime) / (this.duration / this.loopsPerDuration);
			if (this.isLoop && num > 1f)
			{
				num -= (float)((int)num);
			}
			float value = this.curve.Evaluate(num);
			this.DoUpdate(value);
			if (num >= 1f && !this.isLoop)
			{
				this.Finish();
				base.enabled = false;
			}
		}

		// Token: 0x06002AEE RID: 10990 RVA: 0x000C3397 File Offset: 0x000C1797
		private void OnDisable()
		{
			this.Stop();
		}

		// Token: 0x04005432 RID: 21554
		[SerializeField]
		private AnimationCurve curve;

		// Token: 0x04005433 RID: 21555
		[SerializeField]
		private float duration = 1f;

		// Token: 0x04005434 RID: 21556
		[SerializeField]
		private float loopsPerDuration = 1f;

		// Token: 0x04005435 RID: 21557
		[SerializeField]
		private bool autoPlay;

		// Token: 0x04005436 RID: 21558
		private float startTime;

		// Token: 0x04005437 RID: 21559
		private bool isLoop;
	}
}
