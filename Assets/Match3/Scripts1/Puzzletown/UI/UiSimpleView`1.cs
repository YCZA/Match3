using Match3.Scripts1.Wooga.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A00 RID: 2560
	public class UiSimpleView<T> : AVisibleGameObject, IEditorDescription, IDataView<T>
	{
		// Token: 0x06003DA5 RID: 15781 RVA: 0x000C8D3E File Offset: 0x000C713E
		public string GetEditorDescription()
		{
			return this.state.ToString();
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x000C8D51 File Offset: 0x000C7151
		public virtual void Show(T data)
		{
			base.SetVisibility(this.CheckMatch(data));
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x000C8D60 File Offset: 0x000C7160
		public virtual bool CheckMatch(T data)
		{
			return this.state.Equals(data);
		}

		// Token: 0x04006689 RID: 26249
		public T state;
	}
}
