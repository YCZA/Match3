using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BB5 RID: 2997
	[RequireComponent(typeof(Selectable))]
	public class Segment : UIBehaviour, IPointerClickHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler, IEventSystemHandler
	{
		// Token: 0x0600462F RID: 17967 RVA: 0x0016404B File Offset: 0x0016244B
		protected Segment()
		{
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06004630 RID: 17968 RVA: 0x00164053 File Offset: 0x00162453
		internal bool leftmost
		{
			get
			{
				return this.index == 0;
			}
		}

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06004631 RID: 17969 RVA: 0x0016405E File Offset: 0x0016245E
		internal bool rightmost
		{
			get
			{
				return this.index == this.segmentControl.segments.Length - 1;
			}
		}

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06004632 RID: 17970 RVA: 0x00164077 File Offset: 0x00162477
		// (set) Token: 0x06004633 RID: 17971 RVA: 0x0016408F File Offset: 0x0016248F
		public bool selected
		{
			get
			{
				return this.segmentControl.selectedSegment == this.button;
			}
			set
			{
				this.SetSelected(value);
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06004634 RID: 17972 RVA: 0x00164098 File Offset: 0x00162498
		internal SegmentedControl segmentControl
		{
			get
			{
				return base.GetComponentInParent<SegmentedControl>();
			}
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06004635 RID: 17973 RVA: 0x001640A0 File Offset: 0x001624A0
		internal Selectable button
		{
			get
			{
				return base.GetComponent<Selectable>();
			}
		}

		// Token: 0x06004636 RID: 17974 RVA: 0x001640A8 File Offset: 0x001624A8
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.selected = true;
		}

		// Token: 0x06004637 RID: 17975 RVA: 0x001640BD File Offset: 0x001624BD
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x06004638 RID: 17976 RVA: 0x001640C5 File Offset: 0x001624C5
		public virtual void OnPointerExit(PointerEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x06004639 RID: 17977 RVA: 0x001640CD File Offset: 0x001624CD
		public virtual void OnPointerDown(PointerEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x001640D5 File Offset: 0x001624D5
		public virtual void OnPointerUp(PointerEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x0600463B RID: 17979 RVA: 0x001640DD File Offset: 0x001624DD
		public virtual void OnSelect(BaseEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x0600463C RID: 17980 RVA: 0x001640E5 File Offset: 0x001624E5
		public virtual void OnDeselect(BaseEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x0600463D RID: 17981 RVA: 0x001640ED File Offset: 0x001624ED
		public virtual void OnSubmit(BaseEventData eventData)
		{
			this.selected = true;
		}

		// Token: 0x0600463E RID: 17982 RVA: 0x001640F8 File Offset: 0x001624F8
		private void SetSelected(bool value)
		{
			if (value && this.button.IsActive() && this.button.IsInteractable())
			{
				if (this.segmentControl.selectedSegment == this.button)
				{
					if (this.segmentControl.allowSwitchingOff)
					{
						this.Deselect();
					}
					else
					{
						this.MaintainSelection();
					}
				}
				else
				{
					if (this.segmentControl.selectedSegment)
					{
						Segment component = this.segmentControl.selectedSegment.GetComponent<Segment>();
						this.segmentControl.selectedSegment = null;
						component.TransitionButton();
					}
					this.segmentControl.selectedSegment = this.button;
					this.StoreTextColor();
					this.TransitionButton();
					this.segmentControl.onValueChanged.Invoke(this.index);
				}
			}
			else if (this.segmentControl.selectedSegment == this.button)
			{
				this.Deselect();
			}
		}

		// Token: 0x0600463F RID: 17983 RVA: 0x001641FD File Offset: 0x001625FD
		private void Deselect()
		{
			this.segmentControl.selectedSegment = null;
			this.TransitionButton();
			this.segmentControl.onValueChanged.Invoke(-1);
		}

		// Token: 0x06004640 RID: 17984 RVA: 0x00164222 File Offset: 0x00162622
		private void MaintainSelection()
		{
			if (this.button != this.segmentControl.selectedSegment)
			{
				return;
			}
			this.TransitionButton(true);
		}

		// Token: 0x06004641 RID: 17985 RVA: 0x00164247 File Offset: 0x00162647
		internal void TransitionButton()
		{
			this.TransitionButton(false);
		}

		// Token: 0x06004642 RID: 17986 RVA: 0x00164250 File Offset: 0x00162650
		internal void TransitionButton(bool instant)
		{
			Color a = (!this.selected) ? this.button.colors.normalColor : this.segmentControl.selectedColor;
			Color a2 = (!this.selected) ? this.textColor : this.button.colors.normalColor;
			Sprite newSprite = (!this.selected) ? null : this.button.spriteState.pressedSprite;
			string triggername = (!this.selected) ? this.button.animationTriggers.normalTrigger : this.button.animationTriggers.pressedTrigger;
			Selectable.Transition transition = this.button.transition;
			if (transition != Selectable.Transition.ColorTint)
			{
				if (transition != Selectable.Transition.SpriteSwap)
				{
					if (transition == Selectable.Transition.Animation)
					{
						this.TriggerAnimation(triggername);
					}
				}
				else
				{
					this.DoSpriteSwap(newSprite);
				}
			}
			else
			{
				this.StartColorTween(a * this.button.colors.colorMultiplier, instant);
				this.ChangeTextColor(a2 * this.button.colors.colorMultiplier);
			}
		}

		// Token: 0x06004643 RID: 17987 RVA: 0x0016439C File Offset: 0x0016279C
		private void StartColorTween(Color targetColor, bool instant)
		{
			if (this.button.targetGraphic == null)
			{
				return;
			}
			this.button.targetGraphic.CrossFadeColor(targetColor, (!instant) ? this.button.colors.fadeDuration : 0f, true, true);
		}

		// Token: 0x06004644 RID: 17988 RVA: 0x001643F8 File Offset: 0x001627F8
		internal void StoreTextColor()
		{
			Text componentInChildren = base.GetComponentInChildren<Text>();
			if (!componentInChildren)
			{
				return;
			}
			this.textColor = componentInChildren.color;
		}

		// Token: 0x06004645 RID: 17989 RVA: 0x00164424 File Offset: 0x00162824
		private void ChangeTextColor(Color targetColor)
		{
			Text componentInChildren = base.GetComponentInChildren<Text>();
			if (!componentInChildren)
			{
				return;
			}
			componentInChildren.color = targetColor;
		}

		// Token: 0x06004646 RID: 17990 RVA: 0x0016444B File Offset: 0x0016284B
		private void DoSpriteSwap(Sprite newSprite)
		{
			if (this.button.image == null)
			{
				return;
			}
			this.button.image.overrideSprite = newSprite;
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x00164478 File Offset: 0x00162878
		private void TriggerAnimation(string triggername)
		{
			if (this.button.animator == null || !this.button.animator.isActiveAndEnabled || !this.button.animator.hasBoundPlayables || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			this.button.animator.ResetTrigger(this.button.animationTriggers.normalTrigger);
			this.button.animator.ResetTrigger(this.button.animationTriggers.pressedTrigger);
			this.button.animator.ResetTrigger(this.button.animationTriggers.highlightedTrigger);
			this.button.animator.ResetTrigger(this.button.animationTriggers.disabledTrigger);
			this.button.animator.SetTrigger(triggername);
		}

		// Token: 0x04006DB1 RID: 28081
		internal int index;

		// Token: 0x04006DB2 RID: 28082
		[SerializeField]
		private Color textColor;
	}
}
