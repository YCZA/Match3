

// Token: 0x02000595 RID: 1429
namespace Match3.Scripts1
{
	public class Portal : AFieldModifier
	{
		// Token: 0x06002539 RID: 9529 RVA: 0x000A5A8A File Offset: 0x000A3E8A
		public Portal(int id) : base(id)
		{
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x000A5A94 File Offset: 0x000A3E94
		public static string GetStringId(int id)
		{
			int num = 65;
			id = id / 2 + id % 2;
			return ((char)(num + id - 1)).ToString();
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x000A5AC1 File Offset: 0x000A3EC1
		public static bool IsExit(int id)
		{
			return id > 0 && id % 2 == 0;
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x000A5AD3 File Offset: 0x000A3ED3
		public static bool IsEntrance(int id)
		{
			return id > 0 && !Portal.IsExit(id);
		}
	}
}
