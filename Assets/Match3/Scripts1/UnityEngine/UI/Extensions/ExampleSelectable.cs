using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BB6 RID: 2998
	public class ExampleSelectable : MonoBehaviour, IBoxSelectable
	{
		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06004649 RID: 17993 RVA: 0x0016456A File Offset: 0x0016296A
		// (set) Token: 0x0600464A RID: 17994 RVA: 0x00164572 File Offset: 0x00162972
		public bool selected
		{
			get
			{
				return this._selected;
			}
			set
			{
				this._selected = value;
			}
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x0600464B RID: 17995 RVA: 0x0016457B File Offset: 0x0016297B
		// (set) Token: 0x0600464C RID: 17996 RVA: 0x00164583 File Offset: 0x00162983
		public bool preSelected
		{
			get
			{
				return this._preSelected;
			}
			set
			{
				this._preSelected = value;
			}
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x0016458C File Offset: 0x0016298C
		private void Start()
		{
			this.spriteRenderer = base.transform.GetComponent<SpriteRenderer>();
			this.image = base.transform.GetComponent<Image>();
			this.text = base.transform.GetComponent<Text>();
		}

		// Token: 0x0600464E RID: 17998 RVA: 0x001645C4 File Offset: 0x001629C4
		private void Update()
		{
			Color color = Color.white;
			if (this.preSelected)
			{
				color = Color.yellow;
			}
			if (this.selected)
			{
				color = Color.green;
			}
			if (this.spriteRenderer)
			{
				this.spriteRenderer.color = color;
			}
			else if (this.text)
			{
				this.text.color = color;
			}
			else if (this.image)
			{
				this.image.color = color;
			}
			else if (base.GetComponent<Renderer>())
			{
				base.GetComponent<Renderer>().material.color = color;
			}
		}

		// Token: 0x0600464F RID: 17999 RVA: 0x0016467D File Offset: 0x00162A7D
		// Transform IBoxSelectable.get_transform()
		// {
			// return base.transform;
		// }

		// Token: 0x04006DB3 RID: 28083
		private bool _selected;

		// Token: 0x04006DB4 RID: 28084
		private bool _preSelected;

		// Token: 0x04006DB5 RID: 28085
		private SpriteRenderer spriteRenderer;

		// Token: 0x04006DB6 RID: 28086
		private Image image;

		// Token: 0x04006DB7 RID: 28087
		private Text text;
	}
}
