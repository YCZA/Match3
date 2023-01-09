using Shared.Pooling;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200059F RID: 1439
	public static class GroupPool
	{
		// Token: 0x0600259A RID: 9626 RVA: 0x000A7AE8 File Offset: 0x000A5EE8
		public static Group Get(Gem gem)
		{
			Group group = Pool<Group>.Get();
			group.Add(gem);
			group.Color = gem.color;
			return group;
		}
	}
}
