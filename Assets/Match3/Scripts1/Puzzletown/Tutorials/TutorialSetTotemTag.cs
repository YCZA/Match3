using System.Collections;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A85 RID: 2693
	[CreateAssetMenu(fileName = "ScriptTutorialSetTotemTag", menuName = "Puzzletown/Tutorials/Create/TutorialSetTotemTag")]
	public class TutorialSetTotemTag : ATutorialScript
	{
		// Token: 0x06004037 RID: 16439 RVA: 0x0014A8BC File Offset: 0x00148CBC
		protected override IEnumerator ExecuteRoutine()
		{
			MeshRenderer[] buildings = global::UnityEngine.Object.FindObjectsOfType<MeshRenderer>();
			if (buildings != null)
			{
				foreach (MeshRenderer meshRenderer in buildings)
				{
					if (meshRenderer.name == "iso_unique_totem_destroyed(Clone)")
					{
						meshRenderer.gameObject.tag = "Tutorial - Town - Totem";
					}
				}
			}
			else
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x040069E5 RID: 27109
		private const string TOTEM_ITEM_NAME = "iso_unique_totem_destroyed(Clone)";

		// Token: 0x040069E6 RID: 27110
		private const string TOTEM_TUTORIAL_TAG = "Tutorial - Town - Totem";
	}
}
