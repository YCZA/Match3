using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000711 RID: 1809
	public class M3_DebugUiRoot : APtSceneRoot
	{
		// Token: 0x06002CD0 RID: 11472 RVA: 0x000CFB11 File Offset: 0x000CDF11
		private void Start()
		{
			this.panel.gameObject.SetActive(false);
			this.buttonOpen.onClick.AddListener(delegate()
			{
				this.panel.SetActive(!this.panel.activeSelf);
			});
		}

		// Token: 0x04005645 RID: 22085
		[SerializeField]
		private Button buttonOpen;

		// Token: 0x04005646 RID: 22086
		[SerializeField]
		private GameObject panel;
	}
}
