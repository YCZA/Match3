using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200055C RID: 1372
	public class FieldViewIndicator : MonoBehaviour
	{
		// Token: 0x0600240F RID: 9231 RVA: 0x000A053C File Offset: 0x0009E93C
		public void DisplayFieldForEditor(Field field)
		{
			this.spawnSprite.SetActive(field.IsSpawner && !field.CanSpawnDropItems);
			this.dropItemSpawnSprite.SetActive(field.CanSpawnDropItems);
			this.climberSpawnSprite.SetActive(field.CanSpawnClimber);
			this.definedGemSpawn.SetActive(field.IsDefinedGemSpawner && field.HasCrates);
			this.chameleonSpawn.SetActive(field.CanSpawnChameleons);
			this.label.gameObject.SetActive(false);
			if (field.portalId > 0)
			{
				this.label.gameObject.SetActive(true);
				this.SetPortalId(field.portalId);
			}
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x000A05F8 File Offset: 0x0009E9F8
		private void SetPortalId(int id)
		{
			string stringId = Portal.GetStringId(id);
			this.label.text = stringId;
			// this.label.sortingLayerID = SortingLayer.NameToID("FieldsOverlay");
		}

		// Token: 0x04004F86 RID: 20358
		[SerializeField]
		private GameObject spawnSprite;

		// Token: 0x04004F87 RID: 20359
		[SerializeField]
		private GameObject dropItemSpawnSprite;

		// Token: 0x04004F88 RID: 20360
		[SerializeField]
		private GameObject climberSpawnSprite;

		// Token: 0x04004F89 RID: 20361
		[SerializeField]
		private GameObject definedGemSpawn;

		// Token: 0x04004F8A RID: 20362
		[SerializeField]
		private GameObject chameleonSpawn;

		// Token: 0x04004F8B RID: 20363
		[SerializeField]
		private Text label;
		// private TextMeshPro label;
	}
}
