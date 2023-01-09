using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C01 RID: 3073
	public abstract class Menu : MonoBehaviour
	{
		// Token: 0x06004848 RID: 18504
		public abstract void OnBackPressed();

		// Token: 0x04006F14 RID: 28436
		[Tooltip("Destroy the Game UnityEngine.Object when menu is closed (reduces memory usage)")]
		public bool DestroyWhenClosed = true;

		// Token: 0x04006F15 RID: 28437
		[Tooltip("Disable menus that are under this one in the stack")]
		public bool DisableMenusUnderneath = true;
	}
}
