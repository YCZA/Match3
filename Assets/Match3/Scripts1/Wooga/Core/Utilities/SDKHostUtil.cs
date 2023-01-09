using UnityEngine;

namespace Wooga.Core.Utilities
{
	// Token: 0x020003B4 RID: 948
	public class SDKHostUtil
	{
		// Token: 0x06001C93 RID: 7315 RVA: 0x0007CBC0 File Offset: 0x0007AFC0
		public static MonoBehaviour CreateHost(string name)
		{
			GameObject gameObject;
			if ((gameObject = GameObject.Find(name)) == null)
			{
				gameObject = new GameObject
				{
					name = name
				};
			}
			GameObject gameObject2 = gameObject;
			if (gameObject2.GetComponent<SDKHostUtil.DontDestroyOnLoad>() == null)
			{
				gameObject2.AddComponent<SDKHostUtil.DontDestroyOnLoad>();
			}
			return gameObject2.GetComponent<SDKHostUtil.DontDestroyOnLoad>();
		}

		// Token: 0x020003B5 RID: 949
		public class DontDestroyOnLoad : MonoBehaviour
		{
			// Token: 0x06001C95 RID: 7317 RVA: 0x0007CC0F File Offset: 0x0007B00F
			private void Awake()
			{
				global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}
	}
}
