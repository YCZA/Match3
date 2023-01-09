using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;

// Token: 0x0200099A RID: 2458
namespace Match3.Scripts1
{
	public class DedicatedPosition : MonoBehaviour, IEditorDescription
	{
		// Token: 0x06003BB5 RID: 15285 RVA: 0x00128814 File Offset: 0x00126C14
		protected void OnDrawGizmos()
		{
			Vector3 vector = Vector3.up * 0.33f * 0.5f;
			switch (this.purpose)
			{
				case DedicatedPosition.Purpose.StoryDialogue:
					Gizmos.color = new Color(1f, 0f, 1f, 1f);
					break;
				case DedicatedPosition.Purpose.QuestCharacter:
					Gizmos.color = new Color(0f, 0f, 1f, 1f);
					break;
				case DedicatedPosition.Purpose.FocusPoint:
					Gizmos.color = new Color(0f, 1f, 1f, 1f);
					Gizmos.matrix = base.transform.localToWorldMatrix;
					Gizmos.DrawCube(vector, Vector3.one * 0.33f * 0.75f);
					return;
				case DedicatedPosition.Purpose.WorkerStart:
					Gizmos.color = new Color(1f, 1f, 0f, 1f);
					break;
			}
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawLine(vector, vector + Vector3.forward);
			Gizmos.DrawLine(vector + Vector3.forward, vector + (Vector3.forward + Vector3.right) * 0.5f);
			Gizmos.DrawLine(vector + Vector3.forward, vector + (Vector3.forward + Vector3.left) * 0.5f);
			Gizmos.DrawCube(vector, Vector3.one * 0.33f);
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x001289A8 File Offset: 0x00126DA8
		public string GetEditorDescription()
		{
			return string.Format("{0},{1}", this.purpose, this.orientation);
		}

		// Token: 0x040063C4 RID: 25540
		private const float SIZE = 0.33f;

		// Token: 0x040063C5 RID: 25541
		public DedicatedPosition.Purpose purpose;

		// Token: 0x040063C6 RID: 25542
		public SBOrientation orientation;

		// Token: 0x0200099B RID: 2459
		public enum Purpose
		{
			// Token: 0x040063C8 RID: 25544
			StoryDialogue,
			// Token: 0x040063C9 RID: 25545
			QuestCharacter,
			// Token: 0x040063CA RID: 25546
			FocusPoint,
			// Token: 0x040063CB RID: 25547
			WorkerStart
		}
	}
}
