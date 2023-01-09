using System;
using UnityEngine;

// Token: 0x02000AB3 RID: 2739
namespace Match3.Scripts2.Env
{
	public class GameEnvironmentId
	{
		// Token: 0x060040FD RID: 16637 RVA: 0x0015199D File Offset: 0x0014FD9D
		public GameEnvironmentId()
		{
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x001519A5 File Offset: 0x0014FDA5
		public GameEnvironmentId(string id)
		{
			this.id = id;
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x001519B4 File Offset: 0x0014FDB4
		public static GameEnvironmentId Load()
		{
			GameEnvironmentId result = null;
			try
			{
				TextAsset textAsset = Resources.Load<TextAsset>(PATH);
				result = new GameEnvironmentId
				{
					id = textAsset.text
				};
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
				result = new GameEnvironmentId(null);
			}
			return result;
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x00151A0C File Offset: 0x0014FE0C
		public static void CreateAndSave(string id)
		{
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x00151A0E File Offset: 0x0014FE0E
		public void Save()
		{
		}

		// Token: 0x04006AD0 RID: 27344
		private const string PATH = "config/build/env";

		// Token: 0x04006AD1 RID: 27345
		public string id;
	}
}
