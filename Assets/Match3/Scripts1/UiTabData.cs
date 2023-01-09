

// Token: 0x02000A33 RID: 2611
namespace Match3.Scripts1
{
	public class UiTabData<T> : IUiTabStateGetter
	{
		// Token: 0x06003EB2 RID: 16050 RVA: 0x001363E6 File Offset: 0x001347E6
		public UiTabState GetTabState()
		{
			return this.state;
		}

		// Token: 0x040067D2 RID: 26578
		public T data;

		// Token: 0x040067D3 RID: 26579
		public UiTabState state;
	}
}
