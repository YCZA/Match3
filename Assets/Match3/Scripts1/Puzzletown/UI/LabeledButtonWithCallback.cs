using System;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000863 RID: 2147
	public class LabeledButtonWithCallback : BaseButtonWithCallback
	{
		// Token: 0x060034FD RID: 13565 RVA: 0x000FE3F8 File Offset: 0x000FC7F8
		public LabeledButtonWithCallback() : base(null)
		{
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x000FE401 File Offset: 0x000FC801
		public LabeledButtonWithCallback(string text, Action callback) : base(callback)
		{
			this.text = text;
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x000FE411 File Offset: 0x000FC811
		public static LabeledButtonWithCallback[] Array(params LabeledButtonWithCallback[] buttons)
		{
			return buttons;
		}

		// Token: 0x04005CEE RID: 23790
		public string text;
	}
}
