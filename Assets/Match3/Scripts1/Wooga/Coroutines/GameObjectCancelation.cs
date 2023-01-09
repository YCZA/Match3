using UnityEngine;

namespace Wooga.Coroutines
{
	// Token: 0x020003C4 RID: 964
	public static class GameObjectCancelation
	{
		// Token: 0x06001D13 RID: 7443 RVA: 0x0007DC45 File Offset: 0x0007C045
		public static GameObjectCancelationToken CreateCancelationToken(this Behaviour behaviour)
		{
			return new GameObjectCancelationToken(behaviour.gameObject);
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x0007DC52 File Offset: 0x0007C052
		public static GameObjectCancelationToken CreateCancelationToken(this GameObject gameobject)
		{
			return new GameObjectCancelationToken(gameobject);
		}
	}
}
