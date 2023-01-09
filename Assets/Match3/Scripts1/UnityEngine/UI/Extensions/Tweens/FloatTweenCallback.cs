using UnityEngine;
using UnityEngine.Events;

namespace Match3.Scripts1.UnityEngine.UI.Extensions.Tweens
{
	// Token: 0x02000B80 RID: 2944
	public struct FloatTween : ITweenValue
	{
		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x060044DD RID: 17629 RVA: 0x0015DB39 File Offset: 0x0015BF39
		// (set) Token: 0x060044DE RID: 17630 RVA: 0x0015DB41 File Offset: 0x0015BF41
		public float startFloat
		{
			get
			{
				return this.m_StartFloat;
			}
			set
			{
				this.m_StartFloat = value;
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x060044DF RID: 17631 RVA: 0x0015DB4A File Offset: 0x0015BF4A
		// (set) Token: 0x060044E0 RID: 17632 RVA: 0x0015DB52 File Offset: 0x0015BF52
		public float targetFloat
		{
			get
			{
				return this.m_TargetFloat;
			}
			set
			{
				this.m_TargetFloat = value;
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x060044E1 RID: 17633 RVA: 0x0015DB5B File Offset: 0x0015BF5B
		// (set) Token: 0x060044E2 RID: 17634 RVA: 0x0015DB63 File Offset: 0x0015BF63
		public float duration
		{
			get
			{
				return this.m_Duration;
			}
			set
			{
				this.m_Duration = value;
			}
		}

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x060044E3 RID: 17635 RVA: 0x0015DB6C File Offset: 0x0015BF6C
		// (set) Token: 0x060044E4 RID: 17636 RVA: 0x0015DB74 File Offset: 0x0015BF74
		public bool ignoreTimeScale
		{
			get
			{
				return this.m_IgnoreTimeScale;
			}
			set
			{
				this.m_IgnoreTimeScale = value;
			}
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x0015DB7D File Offset: 0x0015BF7D
		public void TweenValue(float floatPercentage)
		{
			if (!this.ValidTarget())
			{
				return;
			}
			this.m_Target.Invoke(Mathf.Lerp(this.m_StartFloat, this.m_TargetFloat, floatPercentage));
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x0015DBA8 File Offset: 0x0015BFA8
		public void AddOnChangedCallback(UnityAction<float> callback)
		{
			if (this.m_Target == null)
			{
				this.m_Target = new FloatTween.FloatTweenCallback();
			}
			this.m_Target.AddListener(callback);
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x0015DBCC File Offset: 0x0015BFCC
		public void AddOnFinishCallback(UnityAction callback)
		{
			if (this.m_Finish == null)
			{
				this.m_Finish = new FloatTween.FloatFinishCallback();
			}
			this.m_Finish.AddListener(callback);
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x0015DBF0 File Offset: 0x0015BFF0
		public bool GetIgnoreTimescale()
		{
			return this.m_IgnoreTimeScale;
		}

		// Token: 0x060044E9 RID: 17641 RVA: 0x0015DBF8 File Offset: 0x0015BFF8
		public float GetDuration()
		{
			return this.m_Duration;
		}

		// Token: 0x060044EA RID: 17642 RVA: 0x0015DC00 File Offset: 0x0015C000
		public bool ValidTarget()
		{
			return this.m_Target != null;
		}

		// Token: 0x060044EB RID: 17643 RVA: 0x0015DC0E File Offset: 0x0015C00E
		public void Finished()
		{
			if (this.m_Finish != null)
			{
				this.m_Finish.Invoke();
			}
		}

		// Token: 0x04006CA0 RID: 27808
		private float m_StartFloat;

		// Token: 0x04006CA1 RID: 27809
		private float m_TargetFloat;

		// Token: 0x04006CA2 RID: 27810
		private float m_Duration;

		// Token: 0x04006CA3 RID: 27811
		private bool m_IgnoreTimeScale;

		// Token: 0x04006CA4 RID: 27812
		private FloatTween.FloatTweenCallback m_Target;

		// Token: 0x04006CA5 RID: 27813
		private FloatTween.FloatFinishCallback m_Finish;

		// Token: 0x02000B81 RID: 2945
		public class FloatTweenCallback : UnityEvent<float>
		{
		}

		// Token: 0x02000B82 RID: 2946
		public class FloatFinishCallback : UnityEvent
		{
		}
	}
}
