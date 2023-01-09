using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BBA RID: 3002
	[AddComponentMenu("UI/Extensions/Stepper")]
	[RequireComponent(typeof(RectTransform))]
	public class Stepper : UIBehaviour
	{
		// Token: 0x06004667 RID: 18023 RVA: 0x00164E71 File Offset: 0x00163271
		protected Stepper()
		{
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06004668 RID: 18024 RVA: 0x00164E94 File Offset: 0x00163294
		private float separatorWidth
		{
			get
			{
				if (this._separatorWidth == 0f && this.separator)
				{
					this._separatorWidth = this.separator.rectTransform.rect.width;
					Image component = this.separator.GetComponent<Image>();
					if (component)
					{
						this._separatorWidth /= component.pixelsPerUnit;
					}
				}
				return this._separatorWidth;
			}
		}

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06004669 RID: 18025 RVA: 0x00164F0F File Offset: 0x0016330F
		public Selectable[] sides
		{
			get
			{
				if (this._sides == null || this._sides.Length == 0)
				{
					this._sides = this.GetSides();
				}
				return this._sides;
			}
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x0600466A RID: 18026 RVA: 0x00164F3B File Offset: 0x0016333B
		// (set) Token: 0x0600466B RID: 18027 RVA: 0x00164F43 File Offset: 0x00163343
		public int value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x0600466C RID: 18028 RVA: 0x00164F4C File Offset: 0x0016334C
		// (set) Token: 0x0600466D RID: 18029 RVA: 0x00164F54 File Offset: 0x00163354
		public int minimum
		{
			get
			{
				return this._minimum;
			}
			set
			{
				this._minimum = value;
			}
		}

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x0600466E RID: 18030 RVA: 0x00164F5D File Offset: 0x0016335D
		// (set) Token: 0x0600466F RID: 18031 RVA: 0x00164F65 File Offset: 0x00163365
		public int maximum
		{
			get
			{
				return this._maximum;
			}
			set
			{
				this._maximum = value;
			}
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06004670 RID: 18032 RVA: 0x00164F6E File Offset: 0x0016336E
		// (set) Token: 0x06004671 RID: 18033 RVA: 0x00164F76 File Offset: 0x00163376
		public int step
		{
			get
			{
				return this._step;
			}
			set
			{
				this._step = value;
			}
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x06004672 RID: 18034 RVA: 0x00164F7F File Offset: 0x0016337F
		// (set) Token: 0x06004673 RID: 18035 RVA: 0x00164F87 File Offset: 0x00163387
		public bool wrap
		{
			get
			{
				return this._wrap;
			}
			set
			{
				this._wrap = value;
			}
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06004674 RID: 18036 RVA: 0x00164F90 File Offset: 0x00163390
		// (set) Token: 0x06004675 RID: 18037 RVA: 0x00164F98 File Offset: 0x00163398
		public Graphic separator
		{
			get
			{
				return this._separator;
			}
			set
			{
				this._separator = value;
				this._separatorWidth = 0f;
				this.LayoutSides(this.sides);
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06004676 RID: 18038 RVA: 0x00164FB8 File Offset: 0x001633B8
		// (set) Token: 0x06004677 RID: 18039 RVA: 0x00164FC0 File Offset: 0x001633C0
		public Stepper.StepperValueChangedEvent onValueChanged
		{
			get
			{
				return this._onValueChanged;
			}
			set
			{
				this._onValueChanged = value;
			}
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x00164FCC File Offset: 0x001633CC
		private Selectable[] GetSides()
		{
			Selectable[] componentsInChildren = base.GetComponentsInChildren<Selectable>();
			if (componentsInChildren.Length != 2)
			{
				throw new InvalidOperationException("A stepper must have two Button children");
			}
			for (int i = 0; i < 2; i++)
			{
				StepperSide x = componentsInChildren[i].GetComponent<StepperSide>();
				if (x == null)
				{
					x = componentsInChildren[i].gameObject.AddComponent<StepperSide>();
				}
			}
			if (!this.wrap)
			{
				this.DisableAtExtremes(componentsInChildren);
			}
			this.LayoutSides(componentsInChildren);
			return componentsInChildren;
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x00165043 File Offset: 0x00163443
		public void StepUp()
		{
			this.Step(this.step);
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x00165051 File Offset: 0x00163451
		public void StepDown()
		{
			this.Step(-this.step);
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x00165060 File Offset: 0x00163460
		private void Step(int amount)
		{
			this.value += amount;
			if (this.wrap)
			{
				if (this.value > this.maximum)
				{
					this.value = this.minimum;
				}
				if (this.value < this.minimum)
				{
					this.value = this.maximum;
				}
			}
			else
			{
				this.value = Math.Max(this.minimum, this.value);
				this.value = Math.Min(this.maximum, this.value);
				this.DisableAtExtremes(this.sides);
			}
			this._onValueChanged.Invoke(this.value);
		}

		// Token: 0x0600467C RID: 18044 RVA: 0x00165110 File Offset: 0x00163510
		private void DisableAtExtremes(Selectable[] sides)
		{
			sides[0].interactable = (this.wrap || this.value > this.minimum);
			sides[1].interactable = (this.wrap || this.value < this.maximum);
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x00165168 File Offset: 0x00163568
		private void RecreateSprites(Selectable[] sides)
		{
			for (int i = 0; i < 2; i++)
			{
				if (!(sides[i].image == null))
				{
					Sprite sprite = sides[i].image.sprite;
					if (sprite.border.x != 0f && sprite.border.z != 0f)
					{
						Rect rect = sprite.rect;
						Vector4 border = sprite.border;
						if (i == 0)
						{
							rect.xMax = border.z;
							border.z = 0f;
						}
						else
						{
							rect.xMin = border.x;
							border.x = 0f;
						}
						sides[i].image.sprite = Sprite.Create(sprite.texture, rect, sprite.pivot, sprite.pixelsPerUnit, 0U, SpriteMeshType.FullRect, border);
					}
				}
			}
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x0016525C File Offset: 0x0016365C
		public void LayoutSides(Selectable[] sides = null)
		{
			sides = (sides ?? this.sides);
			this.RecreateSprites(sides);
			RectTransform rectTransform = base.transform as RectTransform;
			float num = rectTransform.rect.width / 2f - this.separatorWidth;
			for (int i = 0; i < 2; i++)
			{
				float inset = (i != 0) ? (num + this.separatorWidth) : 0f;
				RectTransform component = sides[i].GetComponent<RectTransform>();
				component.anchorMin = Vector2.zero;
				component.anchorMax = Vector2.zero;
				component.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, inset, num);
				component.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, rectTransform.rect.height);
			}
			if (this.separator)
			{
				Transform transform = base.gameObject.transform.Find("Separator");
				Graphic graphic = (!(transform != null)) ? Object.Instantiate<GameObject>(this.separator.gameObject).GetComponent<Graphic>() : transform.GetComponent<Graphic>();
				graphic.gameObject.name = "Separator";
				graphic.gameObject.SetActive(true);
				graphic.rectTransform.SetParent(base.transform, false);
				graphic.rectTransform.anchorMin = Vector2.zero;
				graphic.rectTransform.anchorMax = Vector2.zero;
				graphic.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, num, this.separatorWidth);
				graphic.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, rectTransform.rect.height);
			}
		}

		// Token: 0x04006DC2 RID: 28098
		private Selectable[] _sides;

		// Token: 0x04006DC3 RID: 28099
		[SerializeField]
		[Tooltip("The current step value of the control")]
		private int _value;

		// Token: 0x04006DC4 RID: 28100
		[SerializeField]
		[Tooltip("The minimum step value allowed by the control. When reached it will disable the '-' button")]
		private int _minimum;

		// Token: 0x04006DC5 RID: 28101
		[SerializeField]
		[Tooltip("The maximum step value allowed by the control. When reached it will disable the '+' button")]
		private int _maximum = 100;

		// Token: 0x04006DC6 RID: 28102
		[SerializeField]
		[Tooltip("The step increment used to increment / decrement the step value")]
		private int _step = 1;

		// Token: 0x04006DC7 RID: 28103
		[SerializeField]
		[Tooltip("Does the step value loop around from end to end")]
		private bool _wrap;

		// Token: 0x04006DC8 RID: 28104
		[SerializeField]
		[Tooltip("A GameObject with an Image to use as a separator between segments. Size of the RectTransform will determine the size of the separator used.\nNote, make sure to disable the separator GO so that it does not affect the scene")]
		private Graphic _separator;

		// Token: 0x04006DC9 RID: 28105
		private float _separatorWidth;

		// Token: 0x04006DCA RID: 28106
		[SerializeField]
		private Stepper.StepperValueChangedEvent _onValueChanged = new Stepper.StepperValueChangedEvent();

		// Token: 0x02000BBB RID: 3003
		[Serializable]
		public class StepperValueChangedEvent : UnityEvent<int>
		{
		}
	}
}
