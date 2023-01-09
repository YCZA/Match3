using UnityEngine;

// Token: 0x02000975 RID: 2421
namespace Match3.Scripts1
{
	[RequireComponent(typeof(Camera))]
	public class CopyProjectionMatrix : MonoBehaviour
	{
		// Token: 0x06003B0D RID: 15117 RVA: 0x00124686 File Offset: 0x00122A86
		private void Start()
		{
			this.cam = base.GetComponent<Camera>();
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x00124694 File Offset: 0x00122A94
		private void FixedUpdate()
		{
			this.cam.projectionMatrix = this.copyProjectionFrom.projectionMatrix;
		}

		// Token: 0x040062ED RID: 25325
		public Camera copyProjectionFrom;

		// Token: 0x040062EE RID: 25326
		private Camera cam;
	}
}
