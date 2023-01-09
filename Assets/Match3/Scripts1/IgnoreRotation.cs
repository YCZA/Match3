using UnityEngine;

// Token: 0x0200099C RID: 2460
namespace Match3.Scripts1
{
	public class IgnoreRotation : MonoBehaviour
	{
		// Token: 0x06003BB8 RID: 15288 RVA: 0x001289D2 File Offset: 0x00126DD2
		private void LateUpdate()
		{
			base.transform.rotation = Quaternion.identity;
		}
	}
}
