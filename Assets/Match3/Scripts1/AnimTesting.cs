using UnityEngine;

// Token: 0x020004D1 RID: 1233
namespace Match3.Scripts1
{
	public class AnimTesting : MonoBehaviour
	{
		// Token: 0x06002275 RID: 8821 RVA: 0x00098430 File Offset: 0x00096830
		public void SpawnEmitter(ParticleSystem ps)
		{
			ParticleSystem particleSystem = global::UnityEngine.Object.Instantiate<ParticleSystem>(ps);
			Vector3 position = base.gameObject.transform.position;
			position.z += -0.25f;
			ps.transform.position = position;
			global::UnityEngine.Object.Destroy(particleSystem.gameObject, particleSystem.main.startLifetime.constant + 1f);
		}
	}
}
