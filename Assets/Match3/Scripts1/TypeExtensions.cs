using System;

// Token: 0x02000ACC RID: 2764
namespace Match3.Scripts1
{
	public static class TypeExtensions
	{
		// Token: 0x060041A3 RID: 16803 RVA: 0x00153064 File Offset: 0x00151464
		public static string GetTypeName(this Type t)
		{
			if (!t.IsGenericType)
			{
				return t.Name;
			}
			if (t.IsNested && t.DeclaringType.IsGenericType)
			{
				throw new NotImplementedException();
			}
			string str = t.Name.Substring(0, t.Name.IndexOf('`')) + "<";
			int num = 0;
			foreach (Type t2 in t.GetGenericArguments())
			{
				if (num > 0)
				{
					str += ", ";
				}
				str += t2.GetTypeName();
				num++;
			}
			return str + ">";
		}
	}
}
