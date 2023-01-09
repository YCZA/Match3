using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BB3 RID: 2995
	[AddComponentMenu("UI/Extensions/Segmented Control")]
	[RequireComponent(typeof(RectTransform))]
	public class SegmentedControl : UIBehaviour
	{
		// Token: 0x0600461E RID: 17950 RVA: 0x00163B3B File Offset: 0x00161F3B
		protected SegmentedControl()
		{
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x0600461F RID: 17951 RVA: 0x00163B58 File Offset: 0x00161F58
		protected float SeparatorWidth
		{
			get
			{
				if (this.m_separatorWidth == 0f && this.separator)
				{
					this.m_separatorWidth = this.separator.rectTransform.rect.width;
					Image component = this.separator.GetComponent<Image>();
					if (component)
					{
						this.m_separatorWidth /= component.pixelsPerUnit;
					}
				}
				return this.m_separatorWidth;
			}
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06004620 RID: 17952 RVA: 0x00163BD3 File Offset: 0x00161FD3
		public Selectable[] segments
		{
			get
			{
				if (this.m_segments == null || this.m_segments.Length == 0)
				{
					this.m_segments = this.GetChildSegments();
				}
				return this.m_segments;
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x06004621 RID: 17953 RVA: 0x00163BFF File Offset: 0x00161FFF
		// (set) Token: 0x06004622 RID: 17954 RVA: 0x00163C07 File Offset: 0x00162007
		public Graphic separator
		{
			get
			{
				return this.m_separator;
			}
			set
			{
				this.m_separator = value;
				this.m_separatorWidth = 0f;
				this.LayoutSegments();
			}
		}

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x06004623 RID: 17955 RVA: 0x00163C21 File Offset: 0x00162021
		// (set) Token: 0x06004624 RID: 17956 RVA: 0x00163C29 File Offset: 0x00162029
		public bool allowSwitchingOff
		{
			get
			{
				return this.m_allowSwitchingOff;
			}
			set
			{
				this.m_allowSwitchingOff = value;
			}
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x06004625 RID: 17957 RVA: 0x00163C32 File Offset: 0x00162032
		// (set) Token: 0x06004626 RID: 17958 RVA: 0x00163C48 File Offset: 0x00162048
		public int selectedSegmentIndex
		{
			get
			{
				return Array.IndexOf<Selectable>(this.segments, this.selectedSegment);
			}
			set
			{
				value = Math.Max(value, -1);
				value = Math.Min(value, this.segments.Length - 1);
				this.m_selectedSegmentIndex = value;
				if (value == -1)
				{
					if (this.selectedSegment)
					{
						this.selectedSegment.GetComponent<Segment>().selected = false;
						this.selectedSegment = null;
					}
				}
				else
				{
					this.segments[value].GetComponent<Segment>().selected = true;
				}
			}
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x06004627 RID: 17959 RVA: 0x00163CBE File Offset: 0x001620BE
		// (set) Token: 0x06004628 RID: 17960 RVA: 0x00163CC6 File Offset: 0x001620C6
		public SegmentedControl.SegmentSelectedEvent onValueChanged
		{
			get
			{
				return this.m_onValueChanged;
			}
			set
			{
				this.m_onValueChanged = value;
			}
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x00163CCF File Offset: 0x001620CF
		protected override void Start()
		{
			base.Start();
			this.LayoutSegments();
			if (this.m_selectedSegmentIndex != -1)
			{
				this.selectedSegmentIndex = this.m_selectedSegmentIndex;
			}
		}

		// Token: 0x0600462A RID: 17962 RVA: 0x00163CF8 File Offset: 0x001620F8
		private Selectable[] GetChildSegments()
		{
			Selectable[] componentsInChildren = base.GetComponentsInChildren<Selectable>();
			if (componentsInChildren.Length < 2)
			{
				throw new InvalidOperationException("A segmented control must have at least two Button children");
			}
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Segment segment = componentsInChildren[i].GetComponent<Segment>();
				if (segment == null)
				{
					segment = componentsInChildren[i].gameObject.AddComponent<Segment>();
				}
				segment.index = i;
			}
			return componentsInChildren;
		}

		// Token: 0x0600462B RID: 17963 RVA: 0x00163D5F File Offset: 0x0016215F
		public void SetAllSegmentsOff()
		{
			this.selectedSegment = null;
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x00163D68 File Offset: 0x00162168
		private void RecreateSprites()
		{
			for (int i = 0; i < this.segments.Length; i++)
			{
				if (!(this.segments[i].image == null))
				{
					Sprite sprite = this.segments[i].image.sprite;
					if (sprite.border.x != 0f && sprite.border.z != 0f)
					{
						Rect rect = sprite.rect;
						Vector4 border = sprite.border;
						if (i > 0)
						{
							rect.xMin = border.x;
							border.x = 0f;
						}
						if (i < this.segments.Length - 1)
						{
							rect.xMax = border.z;
							border.z = 0f;
						}
						this.segments[i].image.sprite = Sprite.Create(sprite.texture, rect, sprite.pivot, sprite.pixelsPerUnit, 0U, SpriteMeshType.FullRect, border);
					}
				}
			}
		}

		// Token: 0x0600462D RID: 17965 RVA: 0x00163E80 File Offset: 0x00162280
		public void LayoutSegments()
		{
			this.RecreateSprites();
			RectTransform rectTransform = base.transform as RectTransform;
			float num = rectTransform.rect.width / (float)this.segments.Length - this.SeparatorWidth * (float)(this.segments.Length - 1);
			for (int i = 0; i < this.segments.Length; i++)
			{
				float num2 = (num + this.SeparatorWidth) * (float)i;
				RectTransform component = this.segments[i].GetComponent<RectTransform>();
				component.anchorMin = Vector2.zero;
				component.anchorMax = Vector2.zero;
				component.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, num2, num);
				component.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, rectTransform.rect.height);
				if (this.separator && i > 0)
				{
					Transform transform = base.gameObject.transform.Find("Separator " + i);
					Graphic graphic = (!(transform != null)) ? Object.Instantiate<GameObject>(this.separator.gameObject).GetComponent<Graphic>() : transform.GetComponent<Graphic>();
					graphic.gameObject.name = "Separator " + i;
					graphic.gameObject.SetActive(true);
					graphic.rectTransform.SetParent(base.transform, false);
					graphic.rectTransform.anchorMin = Vector2.zero;
					graphic.rectTransform.anchorMax = Vector2.zero;
					graphic.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, num2 - this.SeparatorWidth, this.SeparatorWidth);
					graphic.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, rectTransform.rect.height);
				}
			}
		}

		// Token: 0x04006DA9 RID: 28073
		private Selectable[] m_segments;

		// Token: 0x04006DAA RID: 28074
		[SerializeField]
		[Tooltip("A GameObject with an Image to use as a separator between segments. Size of the RectTransform will determine the size of the separator used.\nNote, make sure to disable the separator GO so that it does not affect the scene")]
		private Graphic m_separator;

		// Token: 0x04006DAB RID: 28075
		private float m_separatorWidth;

		// Token: 0x04006DAC RID: 28076
		[SerializeField]
		[Tooltip("When True, it allows each button to be toggled on/off")]
		private bool m_allowSwitchingOff;

		// Token: 0x04006DAD RID: 28077
		[SerializeField]
		[Tooltip("The selected default for the control (zero indexed array)")]
		private int m_selectedSegmentIndex = -1;

		// Token: 0x04006DAE RID: 28078
		[SerializeField]
		[Tooltip("Event to fire once the selection has been changed")]
		private SegmentedControl.SegmentSelectedEvent m_onValueChanged = new SegmentedControl.SegmentSelectedEvent();

		// Token: 0x04006DAF RID: 28079
		protected internal Selectable selectedSegment;

		// Token: 0x04006DB0 RID: 28080
		[SerializeField]
		public Color selectedColor;

		// Token: 0x02000BB4 RID: 2996
		[Serializable]
		public class SegmentSelectedEvent : UnityEvent<int>
		{
		}
	}
}
