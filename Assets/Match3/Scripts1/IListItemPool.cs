using Match3.Scripts1.Puzzletown.UI;

// Token: 0x020006FF RID: 1791
namespace Match3.Scripts1
{
	public interface IListItemPool<T1, T2> where T1 : UiSimpleView<T2>
	{
		// Token: 0x06002C65 RID: 11365
		T1 GetItem(T2 state);

		// Token: 0x06002C66 RID: 11366
		void PutBack(T1 item);
	}
}
