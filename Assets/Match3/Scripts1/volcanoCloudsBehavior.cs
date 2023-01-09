using UnityEngine;

// Token: 0x020008BC RID: 2236
namespace Match3.Scripts1
{
	public class volcanoCloudsBehavior : MonoBehaviour
	{
		// Token: 0x06003692 RID: 13970 RVA: 0x0010A314 File Offset: 0x00108714
		private void Update()
		{
			this.angle += this.speed;
			this.localRotation.y = this.offset + Mathf.Sin(this.angle) * this.amplitude;
			base.transform.eulerAngles = this.localRotation;
		}

		// Token: 0x04005E9F RID: 24223
		public float speed = 0.1f;

		// Token: 0x04005EA0 RID: 24224
		public float amplitude = 5f;

		// Token: 0x04005EA1 RID: 24225
		public float offset = 3.1415927f;

		// Token: 0x04005EA2 RID: 24226
		private float angle;

		// Token: 0x04005EA3 RID: 24227
		private Vector3 localRotation;
	}
}
