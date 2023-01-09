using System;

namespace Match3.Scripts1
{
	// Token: 0x02000986 RID: 2438
	public interface IHandler<T>
	{
		// Token: 0x06003B6D RID: 15213
		void Handle(T evt);
	}

	public interface IHandler<T1, T2>
	{
		// Token: 0x06003B6E RID: 15214
		void Handle(T1 evt, T2 sender);
	}
}