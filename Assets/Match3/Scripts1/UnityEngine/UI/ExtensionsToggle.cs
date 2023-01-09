using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI
{
	// Token: 0x02000C1C RID: 3100
	[AddComponentMenu("UI/Extensions/Extensions Toggle", 31)]
	[RequireComponent(typeof(RectTransform))]
	public class ExtensionsToggle : Selectable, IPointerClickHandler, ISubmitHandler, ICanvasElement, IEventSystemHandler
	{
		// Token: 0x0600492E RID: 18734 RVA: 0x00176BE1 File Offset: 0x00174FE1
		protected ExtensionsToggle()
		{
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x0600492F RID: 18735 RVA: 0x00176C06 File Offset: 0x00175006
		// (set) Token: 0x06004930 RID: 18736 RVA: 0x00176C0E File Offset: 0x0017500E
		public ExtensionsToggleGroup Group
		{
			get
			{
				return this.m_Group;
			}
			set
			{
				this.m_Group = value;
				this.SetToggleGroup(this.m_Group, true);
				this.PlayEffect(true);
			}
		}

		// Token: 0x06004931 RID: 18737 RVA: 0x00176C2B File Offset: 0x0017502B
		public virtual void Rebuild(CanvasUpdate executing)
		{
		}

		// Token: 0x06004932 RID: 18738 RVA: 0x00176C2D File Offset: 0x0017502D
		public virtual void LayoutComplete()
		{
		}

		// Token: 0x06004933 RID: 18739 RVA: 0x00176C2F File Offset: 0x0017502F
		public virtual void GraphicUpdateComplete()
		{
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x00176C31 File Offset: 0x00175031
		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetToggleGroup(this.m_Group, false);
			this.PlayEffect(true);
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x00176C4D File Offset: 0x0017504D
		protected override void OnDisable()
		{
			this.SetToggleGroup(null, false);
			base.OnDisable();
		}

		// Token: 0x06004936 RID: 18742 RVA: 0x00176C60 File Offset: 0x00175060
		protected override void OnDidApplyAnimationProperties()
		{
			if (this.graphic != null)
			{
				bool flag = !Mathf.Approximately(this.graphic.canvasRenderer.GetColor().a, 0f);
				if (this.m_IsOn != flag)
				{
					this.m_IsOn = flag;
					this.Set(!flag);
				}
			}
			base.OnDidApplyAnimationProperties();
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x00176CC8 File Offset: 0x001750C8
		private void SetToggleGroup(ExtensionsToggleGroup newGroup, bool setMemberValue)
		{
			ExtensionsToggleGroup group = this.m_Group;
			if (this.m_Group != null)
			{
				this.m_Group.UnregisterToggle(this);
			}
			if (setMemberValue)
			{
				this.m_Group = newGroup;
			}
			if (this.m_Group != null && this.IsActive())
			{
				this.m_Group.RegisterToggle(this);
			}
			if (newGroup != null && newGroup != group && this.IsOn && this.IsActive())
			{
				this.m_Group.NotifyToggleOn(this);
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06004938 RID: 18744 RVA: 0x00176D68 File Offset: 0x00175168
		// (set) Token: 0x06004939 RID: 18745 RVA: 0x00176D70 File Offset: 0x00175170
		public bool IsOn
		{
			get
			{
				return this.m_IsOn;
			}
			set
			{
				this.Set(value);
			}
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x00176D79 File Offset: 0x00175179
		private void Set(bool value)
		{
			this.Set(value, true);
		}

		// Token: 0x0600493B RID: 18747 RVA: 0x00176D84 File Offset: 0x00175184
		private void Set(bool value, bool sendCallback)
		{
			if (this.m_IsOn == value)
			{
				return;
			}
			this.m_IsOn = value;
			if (this.m_Group != null && this.IsActive() && (this.m_IsOn || (!this.m_Group.AnyTogglesOn() && !this.m_Group.AllowSwitchOff)))
			{
				this.m_IsOn = true;
				this.m_Group.NotifyToggleOn(this);
			}
			this.PlayEffect(this.toggleTransition == ExtensionsToggle.ToggleTransition.None);
			if (sendCallback)
			{
				this.onValueChanged.Invoke(this.m_IsOn);
				this.onToggleChanged.Invoke(this);
			}
		}

		// Token: 0x0600493C RID: 18748 RVA: 0x00176E34 File Offset: 0x00175234
		private void PlayEffect(bool instant)
		{
			if (this.graphic == null)
			{
				return;
			}
			this.graphic.CrossFadeAlpha((!this.m_IsOn) ? 0f : 1f, (!instant) ? 0.1f : 0f, true);
		}

		// Token: 0x0600493D RID: 18749 RVA: 0x00176E8E File Offset: 0x0017528E
		protected override void Start()
		{
			this.PlayEffect(true);
		}

		// Token: 0x0600493E RID: 18750 RVA: 0x00176E97 File Offset: 0x00175297
		private void InternalToggle()
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			this.IsOn = !this.IsOn;
		}

		// Token: 0x0600493F RID: 18751 RVA: 0x00176EBF File Offset: 0x001752BF
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.InternalToggle();
		}

		// Token: 0x06004940 RID: 18752 RVA: 0x00176ED3 File Offset: 0x001752D3
		public virtual void OnSubmit(BaseEventData eventData)
		{
			this.InternalToggle();
		}

		// Token: 0x06004941 RID: 18753 RVA: 0x00176EDB File Offset: 0x001752DB
		// Transform ICanvasElement.get_transform()
		// {
		// 	return base.transform;
		// }

		// Token: 0x06004942 RID: 18754 RVA: 0x00176EE3 File Offset: 0x001752E3
		bool ICanvasElement.IsDestroyed()
		{
			return base.IsDestroyed();
		}

		// Token: 0x04006FB4 RID: 28596
		public string UniqueID;

		// Token: 0x04006FB5 RID: 28597
		public ExtensionsToggle.ToggleTransition toggleTransition = ExtensionsToggle.ToggleTransition.Fade;

		// Token: 0x04006FB6 RID: 28598
		public Graphic graphic;

		// Token: 0x04006FB7 RID: 28599
		[SerializeField]
		private ExtensionsToggleGroup m_Group;

		// Token: 0x04006FB8 RID: 28600
		[Tooltip("Use this event if you only need the bool state of the toggle that was changed")]
		public ExtensionsToggle.ToggleEvent onValueChanged = new ExtensionsToggle.ToggleEvent();

		// Token: 0x04006FB9 RID: 28601
		[Tooltip("Use this event if you need access to the toggle that was changed")]
		public ExtensionsToggle.ToggleEventObject onToggleChanged = new ExtensionsToggle.ToggleEventObject();

		// Token: 0x04006FBA RID: 28602
		[FormerlySerializedAs("m_IsActive")]
		[Tooltip("Is the toggle currently on or off?")]
		[SerializeField]
		private bool m_IsOn;

		// Token: 0x02000C1D RID: 3101
		public enum ToggleTransition
		{
			// Token: 0x04006FBC RID: 28604
			None,
			// Token: 0x04006FBD RID: 28605
			Fade
		}

		// Token: 0x02000C1E RID: 3102
		[Serializable]
		public class ToggleEvent : UnityEvent<bool>
		{
		}

		// Token: 0x02000C1F RID: 3103
		[Serializable]
		public class ToggleEventObject : UnityEvent<ExtensionsToggle>
		{
		}
	}
}
