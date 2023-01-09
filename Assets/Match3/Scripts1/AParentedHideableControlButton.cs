using Match3.Scripts1.Wooga.UI;

// Token: 0x0200098B RID: 2443
namespace Match3.Scripts1
{
	public abstract class AParentedHideableControlButton<T> : AParentedControlButton<T>, IDataView<T>
	{
		// Token: 0x06003B7B RID: 15227 RVA: 0x000D0C37 File Offset: 0x000CF037
		public void Show(T op)
		{
			if (this.hideWhenDisabled)
			{
				base.button.gameObject.SetActive(this.CanShow(op));
			}
			else
			{
				base.button.interactable = this.CanShow(op);
			}
		}

		// Token: 0x06003B7C RID: 15228
		protected abstract bool CanShow(T op);

		// Token: 0x04006383 RID: 25475
		public bool hideWhenDisabled = true;
	}
}
