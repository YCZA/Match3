using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

// Token: 0x02000AD2 RID: 2770
namespace Match3.Scripts1
{
	public static class ReflectionUtils
	{
		// Token: 0x060041AC RID: 16812 RVA: 0x00153294 File Offset: 0x00151694
		public static object GetValueInObjectViaReflection(object source, string name)
		{
			if (source == null)
			{
				return null;
			}
			Type type = source.GetType();
			FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (field != null)
			{
				return field.GetValue(source);
			}
			PropertyInfo property = type.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property != null)
			{
				return property.GetValue(source, null);
			}
			return null;
		}

		// Token: 0x060041AD RID: 16813 RVA: 0x001532E4 File Offset: 0x001516E4
		public static object GetValueInEnumerableViaReflection(object source, string name, int index)
		{
			IEnumerable enumerable = ReflectionUtils.GetValueInObjectViaReflection(source, name) as IEnumerable;
			IEnumerator enumerator = enumerable.GetEnumerator();
			while (index >= 0)
			{
				if (!enumerator.MoveNext())
				{
					return null;
				}
				index--;
			}
			return enumerator.Current;
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x00153330 File Offset: 0x00151730
		public static string GetCurrentClassAndMethodName()
		{
			StackFrame frame = new StackTrace().GetFrame(2);
			return frame.GetMethod().DeclaringType.Name + "." + frame.GetMethod().Name;
		}
	}
}
