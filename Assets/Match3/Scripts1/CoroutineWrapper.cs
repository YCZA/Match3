using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;

namespace Match3.Scripts1
{
	// Token: 0x02000754 RID: 1876
	public struct CoroutineWrapper : IBlocker
	{
		// Token: 0x06002E75 RID: 11893 RVA: 0x000D95FF File Offset: 0x000D79FF
		public CoroutineWrapper(Func<IEnumerator> func, bool shouldBlockInput)
		{
			this.m_func = func;
			this.shouldBlockInput = shouldBlockInput;
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06002E76 RID: 11894 RVA: 0x000D960F File Offset: 0x000D7A0F
		public bool BlockInput
		{
			get
			{
				return this.shouldBlockInput;
			}
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x000D9617 File Offset: 0x000D7A17
		public IEnumerator ExecuteRoutine()
		{
			return this.m_func();
		}

		// Token: 0x040057C1 RID: 22465
		private readonly Func<IEnumerator> m_func;

		// Token: 0x040057C2 RID: 22466
		private readonly bool shouldBlockInput;
	}
}
