using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200085A RID: 2138
namespace Match3.Scripts1
{
	[RequireComponent(typeof(Image))]
	public class DisableWhenBlurred : MonoBehaviour
	{
		// Token: 0x060034DE RID: 13534 RVA: 0x000FD975 File Offset: 0x000FBD75
		public void SetActive(bool active)
		{
			base.GetComponent<Image>().enabled = active;
		}
	}
}
