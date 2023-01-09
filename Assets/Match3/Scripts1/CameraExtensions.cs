using UnityEngine;

// Token: 0x02000857 RID: 2135
namespace Match3.Scripts1
{
	public static class CameraExtensions
	{
		// Token: 0x060034C5 RID: 13509 RVA: 0x000FCAB8 File Offset: 0x000FAEB8
		public static bool IsLayerVisible(this Camera self, ObjectLayer layer)
		{
			int num = 1 << (int)layer;
			return (self.cullingMask & num) == num;
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x000FCAD8 File Offset: 0x000FAED8
		public static void SetLayerVisible(this Camera self, ObjectLayer layer, bool visible)
		{
			int num = 1 << (int)layer;
			if (!visible)
			{
				self.cullingMask &= ~num;
			}
			else
			{
				self.cullingMask |= num;
			}
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x000FCB14 File Offset: 0x000FAF14
		public static void SetVisible(this CanvasGroup self, bool active)
		{
			if (active)
			{
				self.alpha = 1f;
				self.interactable = true;
				self.blocksRaycasts = true;
			}
			else
			{
				self.alpha = 0f;
				self.interactable = false;
				self.blocksRaycasts = false;
			}
		}

		// Token: 0x04005CBD RID: 23741
		public static string ABOVE_FORSHADOWING_LAYER = "Field";
	}
}
