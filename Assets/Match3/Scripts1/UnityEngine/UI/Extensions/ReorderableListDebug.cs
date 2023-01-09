using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BAE RID: 2990
	public class ReorderableListDebug : MonoBehaviour
	{
		// Token: 0x06004608 RID: 17928 RVA: 0x00162B14 File Offset: 0x00160F14
		private void Awake()
		{
			foreach (ReorderableList reorderableList in global::UnityEngine.Object.FindObjectsOfType<ReorderableList>())
			{
				reorderableList.OnElementDropped.AddListener(new UnityAction<ReorderableList.ReorderableListEventStruct>(this.ElementDropped));
			}
		}

		// Token: 0x06004609 RID: 17929 RVA: 0x00162B58 File Offset: 0x00160F58
		private void ElementDropped(ReorderableList.ReorderableListEventStruct droppedStruct)
		{
			this.DebugLabel.text = string.Empty;
			Text debugLabel = this.DebugLabel;
			debugLabel.text = debugLabel.text + "Dropped Object: " + droppedStruct.DroppedObject.name + "\n";
			Text debugLabel2 = this.DebugLabel;
			string text = debugLabel2.text;
			debugLabel2.text = string.Concat(new object[]
			{
				text,
				"Is Clone ?: ",
				droppedStruct.IsAClone,
				"\n"
			});
			if (droppedStruct.IsAClone)
			{
				Text debugLabel3 = this.DebugLabel;
				debugLabel3.text = debugLabel3.text + "Source Object: " + droppedStruct.SourceObject.name + "\n";
			}
			Text debugLabel4 = this.DebugLabel;
			debugLabel4.text += string.Format("From {0} at Index {1} \n", droppedStruct.FromList.name, droppedStruct.FromIndex);
			Text debugLabel5 = this.DebugLabel;
			debugLabel5.text += string.Format("To {0} at Index {1} \n", droppedStruct.ToList.name, droppedStruct.ToIndex);
		}

		// Token: 0x04006D87 RID: 28039
		public Text DebugLabel;
	}
}
