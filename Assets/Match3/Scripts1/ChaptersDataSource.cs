

// Token: 0x020006DC RID: 1756
namespace Match3.Scripts1
{
	public class ChaptersDataSource : ArrayDataSource<ChapterView, ChapterInfo>
	{
		// Token: 0x06002BB8 RID: 11192 RVA: 0x000C8C0E File Offset: 0x000C700E
		public override int GetReusableIdForIndex(int index)
		{
			return (int)this.GetDataForIndex(index).state;
		}
	}
}
