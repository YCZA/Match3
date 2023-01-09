using System;
using Match3.Scripts1;

namespace Wooga.Coroutines
{
	// Token: 0x020003D8 RID: 984
	public sealed class OptionalResult
	{
		// Token: 0x06001DD3 RID: 7635 RVA: 0x0007F5AD File Offset: 0x0007D9AD
		private OptionalResult()
		{
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x0007F5B5 File Offset: 0x0007D9B5
		public static OptionalResult<T> Create<T>(T value)
		{
			return (value != null) ? new OptionalResult<T>(value) : OptionalResult<T>.None;
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x0007F5D2 File Offset: 0x0007D9D2
		public static OptionalResult<T> Create<T>(T? value) where T : struct
		{
			return (value != null) ? new OptionalResult<T>(value.Value) : OptionalResult<T>.None;
		}

		// Token: 0x06001DD6 RID: 7638 RVA: 0x0007F5F6 File Offset: 0x0007D9F6
		public static OptionalResult<T> Some<T>(T value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return OptionalResult<T>.Some(value);
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x0007F614 File Offset: 0x0007DA14
		public static OptionalResult<T> Some<T>(T? value) where T : struct
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return OptionalResult<T>.Some(value.Value);
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06001DD8 RID: 7640 RVA: 0x0007F639 File Offset: 0x0007DA39
		public static OptionalResult None
		{
			get
			{
				return OptionalResult._none;
			}
		}

		// Token: 0x040049E2 RID: 18914
		private static OptionalResult _none;
	}
}
