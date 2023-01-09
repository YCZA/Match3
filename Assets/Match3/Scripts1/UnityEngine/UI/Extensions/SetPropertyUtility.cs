using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C2F RID: 3119
	internal static class SetPropertyUtility
	{
		// Token: 0x0600499A RID: 18842 RVA: 0x00178284 File Offset: 0x00176684
		public static bool SetColor(ref Color currentValue, Color newValue)
		{
			if (currentValue.r == newValue.r && currentValue.g == newValue.g && currentValue.b == newValue.b && currentValue.a == newValue.a)
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x0600499B RID: 18843 RVA: 0x001782E3 File Offset: 0x001766E3
		public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
		{
			if (currentValue.Equals(newValue))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x0600499C RID: 18844 RVA: 0x00178308 File Offset: 0x00176708
		public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
		{
			if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}
	}
}
