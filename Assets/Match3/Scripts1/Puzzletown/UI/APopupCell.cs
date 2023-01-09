using Match3.Scripts1.Wooga.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x0200084D RID: 2125
	public abstract class APopupCell : ATableViewReusableCell, IDataView<object>
	{
		// Token: 0x06003498 RID: 13464
		public abstract void Show(object data);

		// Token: 0x06003499 RID: 13465
		public abstract bool CanPresent(object data);
	}
}
