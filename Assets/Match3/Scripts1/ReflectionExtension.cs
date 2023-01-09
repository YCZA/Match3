using System;
using System.Reflection;

// Token: 0x02000AD0 RID: 2768
namespace Match3.Scripts1
{
	public static class ReflectionExtension
	{
		// Token: 0x060041A9 RID: 16809 RVA: 0x001531EC File Offset: 0x001515EC
		public static Type GetUnderlyingType(this MemberInfo member)
		{
			MemberTypes memberType = member.MemberType;
			switch (memberType)
			{
				case MemberTypes.Event:
					return ((EventInfo)member).EventHandlerType;
				default:
					if (memberType == MemberTypes.Method)
					{
						return ((MethodInfo)member).ReturnType;
					}
					if (memberType != MemberTypes.Property)
					{
						throw new ArgumentException("Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo");
					}
					return ((PropertyInfo)member).PropertyType;
				case MemberTypes.Field:
					return ((FieldInfo)member).FieldType;
			}
		}
	}
}
