using Match3.Scripts1.Wooga.Signals;

// Token: 0x02000989 RID: 2441
namespace Match3.Scripts1
{
	public abstract class AStandaloneControlButton<T> : ABaseControlButton<T>
	{
		// Token: 0x06003B77 RID: 15223 RVA: 0x00127821 File Offset: 0x00125C21
		protected override void HandleOnClick()
		{
			this.onClicked.Dispatch(this.operation);
		}

		// Token: 0x04006382 RID: 25474
		public readonly Signal<T> onClicked = new Signal<T>();
	}
}
