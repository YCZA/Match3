using System;
using System.Linq.Expressions;

// Token: 0x02000ACE RID: 2766
namespace Match3.Scripts1
{
	public static class PropertyUtil
	{
		// Token: 0x060041A6 RID: 16806 RVA: 0x00153184 File Offset: 0x00151584
		public static TProp GetProperty<TObject, TProp>(this TObject type, Expression<Func<TObject, TProp>> propertyRefExpr)
		{
			if (type == null)
			{
				return default(TProp);
			}
			return propertyRefExpr.Compile()(type);
		}
	}
}
