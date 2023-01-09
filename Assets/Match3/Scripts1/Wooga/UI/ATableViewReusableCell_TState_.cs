using System;

namespace Match3.Scripts1.Wooga.UI
{
	// Token: 0x02000B3A RID: 2874
	public abstract class ATableViewReusableCell<TState> : ATableViewReusableCell, IEditorDescription
	{
		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x06004366 RID: 17254 RVA: 0x000C8C2C File Offset: 0x000C702C
		public override int reusableId
		{
			get
			{
				return Convert.ToInt32(this.state);
			}
		}

		// Token: 0x06004367 RID: 17255 RVA: 0x000C8C3E File Offset: 0x000C703E
		public string GetEditorDescription()
		{
			return this.state.ToString();
		}

		// Token: 0x04006BE6 RID: 27622
		public TState state;
	}
}
