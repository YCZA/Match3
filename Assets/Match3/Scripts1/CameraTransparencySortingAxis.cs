using UnityEngine;

// Token: 0x02000695 RID: 1685
namespace Match3.Scripts1
{
	public class CameraTransparencySortingAxis : MonoBehaviour
	{
		// Token: 0x06002A11 RID: 10769 RVA: 0x000C0AEA File Offset: 0x000BEEEA
		private void OnEnable()
		{
			this.levelCamera.transparencySortMode = TransparencySortMode.CustomAxis;
			this.levelCamera.transparencySortAxis = Vector3.one;
		}

		// Token: 0x0400537C RID: 21372
		[SerializeField]
		private Camera levelCamera;
	}
}
