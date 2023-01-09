using System;

namespace Match3.Scripts1.Shared.M3Engine
{
	// Token: 0x02000AF4 RID: 2804
	public interface IAnimationController<T>
	{
		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06004273 RID: 17011
		// (remove) Token: 0x06004274 RID: 17012
		event Action Finished;

		// Token: 0x06004275 RID: 17013
		void Start(T input);
	}
}
