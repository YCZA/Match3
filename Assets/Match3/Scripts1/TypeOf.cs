using System;
using System.Linq.Expressions;

// Token: 0x02000AD1 RID: 2769
namespace Match3.Scripts1
{
	public class TypeOf<T>
	{
		// Token: 0x060041AB RID: 16811 RVA: 0x0015326C File Offset: 0x0015166C
		public static Type Property<TProp>(object obj, Expression<Func<T, TProp>> expression)
		{
			string name = NameOf<T>.Property<TProp>(expression, false);
			return obj.GetType().GetMember(name)[0].GetUnderlyingType();
		}
	}
}
