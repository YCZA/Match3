using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C36 RID: 3126
	[AddComponentMenu("UI/Extensions/UI Highlightable Extension")]
	[RequireComponent(typeof(RectTransform), typeof(Graphic))]
	public class UIHighlightable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x060049B2 RID: 18866 RVA: 0x00179354 File Offset: 0x00177754
		// (set) Token: 0x060049B3 RID: 18867 RVA: 0x0017935C File Offset: 0x0017775C
		public bool Interactable
		{
			get
			{
				return this.m_Interactable;
			}
			set
			{
				this.m_Interactable = value;
				this.HighlightInteractable(this.m_Graphic);
				this.OnInteractableChanged.Invoke(this.m_Interactable);
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x060049B4 RID: 18868 RVA: 0x00179382 File Offset: 0x00177782
		// (set) Token: 0x060049B5 RID: 18869 RVA: 0x0017938A File Offset: 0x0017778A
		public bool ClickToHold
		{
			get
			{
				return this.m_ClickToHold;
			}
			set
			{
				this.m_ClickToHold = value;
			}
		}

		// Token: 0x060049B6 RID: 18870 RVA: 0x00179393 File Offset: 0x00177793
		private void Awake()
		{
			this.m_Graphic = base.GetComponent<Graphic>();
		}

		// Token: 0x060049B7 RID: 18871 RVA: 0x001793A1 File Offset: 0x001777A1
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.Interactable && !this.m_Pressed)
			{
				this.m_Highlighted = true;
				this.m_Graphic.color = this.HighlightedColor;
			}
		}

		// Token: 0x060049B8 RID: 18872 RVA: 0x001793D1 File Offset: 0x001777D1
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.Interactable && !this.m_Pressed)
			{
				this.m_Highlighted = false;
				this.m_Graphic.color = this.NormalColor;
			}
		}

		// Token: 0x060049B9 RID: 18873 RVA: 0x00179401 File Offset: 0x00177801
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.Interactable)
			{
				this.m_Graphic.color = this.PressedColor;
				if (this.ClickToHold)
				{
					this.m_Pressed = !this.m_Pressed;
				}
			}
		}

		// Token: 0x060049BA RID: 18874 RVA: 0x00179439 File Offset: 0x00177839
		public void OnPointerUp(PointerEventData eventData)
		{
			if (!this.m_Pressed)
			{
				this.HighlightInteractable(this.m_Graphic);
			}
		}

		// Token: 0x060049BB RID: 18875 RVA: 0x00179454 File Offset: 0x00177854
		private void HighlightInteractable(Graphic graphic)
		{
			if (this.m_Interactable)
			{
				if (this.m_Highlighted)
				{
					graphic.color = this.HighlightedColor;
				}
				else
				{
					graphic.color = this.NormalColor;
				}
			}
			else
			{
				graphic.color = this.DisabledColor;
			}
		}

		// Token: 0x04007009 RID: 28681
		private Graphic m_Graphic;

		// Token: 0x0400700A RID: 28682
		private bool m_Highlighted;

		// Token: 0x0400700B RID: 28683
		private bool m_Pressed;

		// Token: 0x0400700C RID: 28684
		[SerializeField]
		[Tooltip("Can this panel be interacted with or is it disabled? (does not affect child components)")]
		private bool m_Interactable = true;

		// Token: 0x0400700D RID: 28685
		[SerializeField]
		[Tooltip("Does the panel remain in the pressed state when clicked? (default false)")]
		private bool m_ClickToHold;

		// Token: 0x0400700E RID: 28686
		[Tooltip("The default color for the panel")]
		public Color NormalColor = Color.grey;

		// Token: 0x0400700F RID: 28687
		[Tooltip("The color for the panel when a mouse is over it or it is in focus")]
		public Color HighlightedColor = Color.yellow;

		// Token: 0x04007010 RID: 28688
		[Tooltip("The color for the panel when it is clicked/held")]
		public Color PressedColor = Color.green;

		// Token: 0x04007011 RID: 28689
		[Tooltip("The color for the panel when it is not interactable (see Interactable)")]
		public Color DisabledColor = Color.gray;

		// Token: 0x04007012 RID: 28690
		[Tooltip("Event for when the panel is enabled / disabled, to enable disabling / enabling of child or other gameobjects")]
		public UIHighlightable.InteractableChangedEvent OnInteractableChanged;

		// Token: 0x02000C37 RID: 3127
		[Serializable]
		public class InteractableChangedEvent : UnityEvent<bool>
		{
		}
	}
}
