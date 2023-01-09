using UnityEngine;

// Token: 0x020006B6 RID: 1718
namespace Match3.Scripts1
{
	public class HighlightField : ATween
	{
		// Token: 0x06002AE3 RID: 10979 RVA: 0x000C4494 File Offset: 0x000C2894
		protected override void DoUpdate(float value)
		{
			Color color = new Color(this.highlightRGB.r, this.highlightRGB.g, this.highlightRGB.b, value);
			this.highlight.color = color;
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x000C44D6 File Offset: 0x000C28D6
		protected override void Show()
		{
			this.highlight.gameObject.SetActive(true);
			this.DoUpdate(0f);
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x000C44F4 File Offset: 0x000C28F4
		protected override void Finish()
		{
			this.highlight.gameObject.SetActive(false);
		}

		// Token: 0x04005430 RID: 21552
		[SerializeField]
		private SpriteRenderer highlight;

		// Token: 0x04005431 RID: 21553
		[SerializeField]
		private Color highlightRGB = new Color(0.875f, 0.4862f, 0.0313f);
	}
}
