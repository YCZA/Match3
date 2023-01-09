using UnityEngine;

namespace Wooga.Coroutines
{
	// Token: 0x020003C6 RID: 966
	public class GameObjectCancelationToken : CancellationToken
	{
		// Token: 0x06001D1B RID: 7451 RVA: 0x0007DD27 File Offset: 0x0007C127
		public GameObjectCancelationToken(GameObject gameobject)
		{
			this._gameobject = gameobject;
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001D1C RID: 7452 RVA: 0x0007DD36 File Offset: 0x0007C136
		public override bool IsCanceled
		{
			get
			{
				return base.IsCanceled || this._gameobject == null;
			}
		}

		// Token: 0x040049C1 RID: 18881
		private readonly GameObject _gameobject;
	}
}
