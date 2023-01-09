

// Token: 0x0200093D RID: 2365
namespace Match3.Scripts1
{
	public abstract class ACollectTaskHandler : QuestTaskHandler
	{
		// Token: 0x06003989 RID: 14729 RVA: 0x0011B39C File Offset: 0x0011979C
		protected ACollectTaskHandler(QuestManager service) : base(service)
		{
		}

		// Token: 0x0600398A RID: 14730
		public abstract void CollectResource(MaterialAmount collected);
	}
}
