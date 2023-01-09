using System.Collections;

namespace Wooga.Coroutines
{
	// Token: 0x020003CB RID: 971
	public class EmptyEnumerator : IEnumerator
	{
		// Token: 0x06001D72 RID: 7538 RVA: 0x0007E88E File Offset: 0x0007CC8E
		protected EmptyEnumerator()
		{
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x0007E896 File Offset: 0x0007CC96
		public bool MoveNext()
		{
			return false;
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x0007E899 File Offset: 0x0007CC99
		public void Reset()
		{
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001D75 RID: 7541 RVA: 0x0007E89B File Offset: 0x0007CC9B
		public object Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040049C8 RID: 18888
		public static readonly EmptyEnumerator Instance = new EmptyEnumerator();
	}
}
