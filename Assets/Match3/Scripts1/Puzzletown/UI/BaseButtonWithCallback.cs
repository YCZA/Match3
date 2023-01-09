using System;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000862 RID: 2146
	public class BaseButtonWithCallback
	{
		// Token: 0x060034FC RID: 13564 RVA: 0x000FE3E9 File Offset: 0x000FC7E9
		protected BaseButtonWithCallback(Action callback)
		{
			this.callback = callback;
		}

		// Token: 0x04005CED RID: 23789
		public Action callback;
	}
}
