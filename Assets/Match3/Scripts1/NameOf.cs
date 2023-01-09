using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

// Token: 0x02000ACD RID: 2765
namespace Match3.Scripts1
{
	public class NameOf<T>
	{
		// Token: 0x060041A5 RID: 16805 RVA: 0x00153128 File Offset: 0x00151528
		public static string Property<TProp>(Expression<Func<T, TProp>> expression, bool addSpaceBeforeUppercaseLetter = true)
		{
			MemberExpression memberExpression = expression.Body as MemberExpression;
			if (memberExpression == null)
			{
				throw new ArgumentException("'expression' should be a member expression");
			}
			if (addSpaceBeforeUppercaseLetter)
			{
				return Regex.Replace(memberExpression.Member.Name, "[A-Z]", " $0").Trim();
			}
			return memberExpression.Member.Name;
		}
	}
}
