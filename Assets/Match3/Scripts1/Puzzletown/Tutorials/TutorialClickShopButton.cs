using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Datasources;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A76 RID: 2678
	[CreateAssetMenu(fileName = "ScriptWaitForSeconds", menuName = "Puzzletown/Tutorials/Create/ClickShopButton")]
	public class TutorialClickShopButton : ATutorialScript
	{
		// Token: 0x0600400F RID: 16399 RVA: 0x00148BC4 File Offset: 0x00146FC4
		protected override IEnumerator ExecuteRoutine()
		{
			BuildingShopDataSource source = null;
			while (source == null)
			{
				source = global::UnityEngine.Object.FindObjectOfType<BuildingShopDataSource>();
				yield return null;
			}
			TableViewSnapper snapper = source.transform.parent.gameObject.AddComponent<TableViewSnapper>();
			while (!snapper.scrollRect.content)
			{
				yield return null;
			}
			WoogaDebug.Log(new object[]
			{
				"snap"
			});
			snapper.scrollRect.verticalNormalizedPosition = 0.5f;
			yield return new WaitForSeconds(0.5f);
			IEnumerator enumerator = source.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform child = (Transform)obj;
					BuildingShopView view = child.GetComponent<BuildingShopView>();
					if (view.BuildingId == this.id)
					{
						TutorialOverlayRoot root = global::UnityEngine.Object.FindObjectOfType<TutorialOverlayRoot>();
						WoogaDebug.Log(new object[]
						{
							view.gameObject
						});
						root.ShowObject(view.gameObject, new Padding(), false, false);
						yield return view.GetComponent<Button>().onClick.Await();
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			yield break;
		}

		// Token: 0x040069C5 RID: 27077
		public string id = "iso_nature_tree_palm";
	}
}
