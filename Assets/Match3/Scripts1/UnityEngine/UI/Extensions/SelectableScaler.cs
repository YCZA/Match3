using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C2E RID: 3118
	[AddComponentMenu("UI/Extensions/Selectable Scalar")]
	[RequireComponent(typeof(Button))]
	public class SelectableScaler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06004993 RID: 18835 RVA: 0x00177EB4 File Offset: 0x001762B4
		public Selectable Target
		{
			get
			{
				if (this.selectable == null)
				{
					this.selectable = base.GetComponent<Selectable>();
				}
				return this.selectable;
			}
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x00177ED9 File Offset: 0x001762D9
		private void Awake()
		{
			if (this.target == null)
			{
				this.target = base.transform;
			}
			this.initScale = this.target.localScale;
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x00177F09 File Offset: 0x00176309
		private void OnEnable()
		{
			this.target.localScale = this.initScale;
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x00177F1C File Offset: 0x0017631C
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.Target != null && !this.Target.interactable)
			{
				return;
			}
			base.StopCoroutine("ScaleOUT");
			base.StartCoroutine("ScaleIN");
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x00177F57 File Offset: 0x00176357
		public void OnPointerUp(PointerEventData eventData)
		{
			if (this.Target != null && !this.Target.interactable)
			{
				return;
			}
			base.StopCoroutine("ScaleIN");
			base.StartCoroutine("ScaleOUT");
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x00177F94 File Offset: 0x00176394
		private IEnumerator ScaleIN()
		{
			if (this.animCurve.keys.Length > 0)
			{
				this.target.localScale = this.initScale;
				float t = 0f;
				float maxT = this.animCurve.keys[this.animCurve.length - 1].time;
				while (t < maxT)
				{
					t += this.speed * Time.unscaledDeltaTime;
					this.target.localScale = Vector3.one * this.animCurve.Evaluate(t);
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x00177FB0 File Offset: 0x001763B0
		private IEnumerator ScaleOUT()
		{
			if (this.animCurve.keys.Length > 0)
			{
				float t = 0f;
				float maxT = this.animCurve.keys[this.animCurve.length - 1].time;
				while (t < maxT)
				{
					t += this.speed * Time.unscaledDeltaTime;
					this.target.localScale = Vector3.one * this.animCurve.Evaluate(maxT - t);
					yield return null;
				}
				base.transform.localScale = this.initScale;
			}
			yield break;
		}

		// Token: 0x04006FDE RID: 28638
		public AnimationCurve animCurve;

		// Token: 0x04006FDF RID: 28639
		[Tooltip("Animation speed multiplier")]
		public float speed = 1f;

		// Token: 0x04006FE0 RID: 28640
		private Vector3 initScale;

		// Token: 0x04006FE1 RID: 28641
		public Transform target;

		// Token: 0x04006FE2 RID: 28642
		private Selectable selectable;
	}
}
