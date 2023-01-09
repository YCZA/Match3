using System;
using Match3.Scripts1.UnityEngine.UI.Extensions.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000B7F RID: 2943
	[RequireComponent(typeof(RectTransform), typeof(LayoutElement))]
	[AddComponentMenu("UI/Extensions/Accordion/Accordion Element")]
	public class AccordionElement : Toggle
	{
		// Token: 0x060044D7 RID: 17623 RVA: 0x0015D8E1 File Offset: 0x0015BCE1
		protected AccordionElement()
		{
			if (this.m_FloatTweenRunner == null)
			{
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			}
			this.m_FloatTweenRunner.Init(this);
		}

		// Token: 0x060044D8 RID: 17624 RVA: 0x0015D918 File Offset: 0x0015BD18
		protected override void Awake()
		{
			base.Awake();
			base.transition = Selectable.Transition.None;
			this.toggleTransition = Toggle.ToggleTransition.None;
			this.m_Accordion = base.gameObject.GetComponentInParent<Accordion>();
			this.m_RectTransform = (base.transform as RectTransform);
			this.m_LayoutElement = base.gameObject.GetComponent<LayoutElement>();
			this.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
		}

		// Token: 0x060044D9 RID: 17625 RVA: 0x0015D984 File Offset: 0x0015BD84
		public void OnValueChanged(bool state)
		{
			if (this.m_LayoutElement == null)
			{
				return;
			}
			Accordion.Transition transition = (!(this.m_Accordion != null)) ? Accordion.Transition.Instant : this.m_Accordion.transition;
			if (transition == Accordion.Transition.Instant)
			{
				if (state)
				{
					this.m_LayoutElement.preferredHeight = -1f;
				}
				else
				{
					this.m_LayoutElement.preferredHeight = this.m_MinHeight;
				}
			}
			else if (transition == Accordion.Transition.Tween)
			{
				if (state)
				{
					this.StartTween(this.m_MinHeight, this.GetExpandedHeight());
				}
				else
				{
					this.StartTween(this.m_RectTransform.rect.height, this.m_MinHeight);
				}
			}
		}

		// Token: 0x060044DA RID: 17626 RVA: 0x0015DA40 File Offset: 0x0015BE40
		protected float GetExpandedHeight()
		{
			if (this.m_LayoutElement == null)
			{
				return this.m_MinHeight;
			}
			float preferredHeight = this.m_LayoutElement.preferredHeight;
			this.m_LayoutElement.preferredHeight = -1f;
			float preferredHeight2 = LayoutUtility.GetPreferredHeight(this.m_RectTransform);
			this.m_LayoutElement.preferredHeight = preferredHeight;
			return preferredHeight2;
		}

		// Token: 0x060044DB RID: 17627 RVA: 0x0015DA9C File Offset: 0x0015BE9C
		protected void StartTween(float startFloat, float targetFloat)
		{
			float duration = (!(this.m_Accordion != null)) ? 0.3f : this.m_Accordion.transitionDuration;
			FloatTween info = new FloatTween
			{
				duration = duration,
				startFloat = startFloat,
				targetFloat = targetFloat
			};
			info.AddOnChangedCallback(new UnityAction<float>(this.SetHeight));
			info.ignoreTimeScale = true;
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x060044DC RID: 17628 RVA: 0x0015DB19 File Offset: 0x0015BF19
		protected void SetHeight(float height)
		{
			if (this.m_LayoutElement == null)
			{
				return;
			}
			this.m_LayoutElement.preferredHeight = height;
		}

		// Token: 0x04006C9B RID: 27803
		[SerializeField]
		private float m_MinHeight = 18f;

		// Token: 0x04006C9C RID: 27804
		private Accordion m_Accordion;

		// Token: 0x04006C9D RID: 27805
		private RectTransform m_RectTransform;

		// Token: 0x04006C9E RID: 27806
		private LayoutElement m_LayoutElement;

		// Token: 0x04006C9F RID: 27807
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;
	}
}
