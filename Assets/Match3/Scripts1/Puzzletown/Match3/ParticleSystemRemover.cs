using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006CA RID: 1738
	public class ParticleSystemRemover : MonoBehaviour
	{
		// Token: 0x06002B66 RID: 11110 RVA: 0x000C73C8 File Offset: 0x000C57C8
		private void OnEnable()
		{
			this.timer = 0f;
			this.duration = this.system.main.duration;
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x000C73F9 File Offset: 0x000C57F9
		private void Update()
		{
			this.timer += Time.deltaTime;
			if (this.timer >= this.duration)
			{
				base.transform.SetParent(null);
				base.gameObject.Release();
			}
		}

		// Token: 0x04005480 RID: 21632
		public ParticleSystem system;

		// Token: 0x04005481 RID: 21633
		private float timer;

		// Token: 0x04005482 RID: 21634
		private float duration;
	}
}
