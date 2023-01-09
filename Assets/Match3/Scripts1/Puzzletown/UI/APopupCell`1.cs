using Match3.Scripts1.Wooga.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x0200084E RID: 2126
	public abstract class APopupCell<T> : APopupCell, IDataView<T>
	{
		// Token: 0x0600349B RID: 13467 RVA: 0x000FBE8F File Offset: 0x000FA28F
		public override void Show(object data)
		{
			this.Show((T)((object)data));
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x000FBE9D File Offset: 0x000FA29D
		public override bool CanPresent(object data)
		{
			return data is T && this.CanPresent((T)((object)data));
		}

		// Token: 0x0600349D RID: 13469
		public abstract bool CanPresent(T data);

		// Token: 0x0600349E RID: 13470
		public abstract void Show(T data);

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x0600349F RID: 13471 RVA: 0x000FBEBC File Offset: 0x000FA2BC
		public override int reusableId
		{
			get
			{
				return 0;
			}
		}
	}
}
