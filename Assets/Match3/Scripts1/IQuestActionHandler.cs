

// Token: 0x02000927 RID: 2343
namespace Match3.Scripts1
{
	public interface IQuestActionHandler
	{
		// Token: 0x06003902 RID: 14594
		bool CanHandle(IQuestAction action);

		// Token: 0x06003903 RID: 14595
		void Handle(IQuestAction action);
	}
}
