using UnityEngine;

// Token: 0x020004DF RID: 1247
namespace Match3.Scripts1
{
	public class PlayCameraShake : MonoBehaviour
	{
		// Token: 0x060022AD RID: 8877 RVA: 0x00099713 File Offset: 0x00097B13
		public void PlayShake(int num)
		{
			if (this.CamShake != null)
			{
				this.CamShake.PlayShake(num);
			}
		}

		// Token: 0x04004E46 RID: 20038
		public CameraShake CamShake;
	}
}
