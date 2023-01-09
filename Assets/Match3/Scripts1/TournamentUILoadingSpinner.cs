using UnityEngine;

// Token: 0x02000A66 RID: 2662
namespace Match3.Scripts1
{
	public class TournamentUILoadingSpinner : MonoBehaviour
	{
		// Token: 0x06003FBD RID: 16317 RVA: 0x0014681C File Offset: 0x00144C1C
		private void Update()
		{
			if (this.cachedTransform == null)
			{
				this.cachedTransform = base.gameObject.transform;
			}
			if (this.rotationVector == Vector3.zero)
			{
				float num = this.rotationDegreesPerSecond / (float)Application.targetFrameRate;
				if (this.clockwise)
				{
					num *= -1f;
				}
				this.rotationVector = new Vector3(0f, 0f, num);
			}
			this.cachedTransform.Rotate(this.rotationVector);
		}

		// Token: 0x0400695F RID: 26975
		[Range(10f, 720f)]
		public float rotationDegreesPerSecond;

		// Token: 0x04006960 RID: 26976
		public bool clockwise = true;

		// Token: 0x04006961 RID: 26977
		private Transform cachedTransform;

		// Token: 0x04006962 RID: 26978
		private Vector3 rotationVector = Vector3.zero;
	}
}
