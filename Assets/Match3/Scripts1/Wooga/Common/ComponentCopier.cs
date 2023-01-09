using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Wooga.Common
{
	// Token: 0x02000AB8 RID: 2744
	public static class ComponentCopier
	{
		// Token: 0x06004117 RID: 16663 RVA: 0x00151CD0 File Offset: 0x001500D0
		public static T CopyComponent<T>(Component original, GameObject destination) where T : Component
		{
			Type typeFromHandle = typeof(T);
			T t = destination.AddComponent<T>();
			FieldInfo[] fields = typeFromHandle.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				fieldInfo.SetValue(t, fieldInfo.GetValue(original));
			}
			return t;
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x00151D2C File Offset: 0x0015012C
		public static LayoutElement CopyLayoutElement(LayoutElement element, GameObject destination)
		{
			LayoutElement layoutElement = destination.AddComponent<LayoutElement>();
			layoutElement.minWidth = element.minWidth;
			layoutElement.minHeight = element.minHeight;
			layoutElement.flexibleWidth = element.flexibleWidth;
			layoutElement.flexibleHeight = element.flexibleHeight;
			layoutElement.preferredWidth = element.preferredWidth;
			layoutElement.preferredHeight = element.preferredHeight;
			return layoutElement;
		}
	}
}
