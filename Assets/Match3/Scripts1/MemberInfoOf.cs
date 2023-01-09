using System;
using System.Linq.Expressions;
using System.Reflection;

// Token: 0x02000ACF RID: 2767
namespace Match3.Scripts1
{
	public class MemberInfoOf<T>
	{
		// Token: 0x060041A8 RID: 16808 RVA: 0x001531BC File Offset: 0x001515BC
		public static MemberInfo Property<TProp>(Expression<Func<T, TProp>> expression)
		{
			MemberExpression memberExpression = expression.Body as MemberExpression;
			if (memberExpression == null)
			{
				throw new ArgumentException("'expression' should be a member expression");
			}
			return memberExpression.Member;
		}
	}
}
