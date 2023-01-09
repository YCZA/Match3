using UnityEngine;

// Token: 0x02000917 RID: 2327
namespace Match3.Scripts1
{
	[RequireComponent(typeof(BoxCollider))]
	public class DebugBox : MonoBehaviour
	{
		// Token: 0x060038B6 RID: 14518 RVA: 0x00117348 File Offset: 0x00115748
		protected void OnDrawGizmos()
		{
			BoxCollider component = base.GetComponent<BoxCollider>();
			Gizmos.matrix = base.transform.localToWorldMatrix;
			if (this.drawWire)
			{
				Gizmos.color = new Color(this.color.r, this.color.g, this.color.b, 1f);
				Gizmos.DrawWireCube(component.center, component.size);
			}
			if (this.drawSolid)
			{
				Gizmos.color = this.color;
				Gizmos.DrawCube(component.center, component.size);
			}
		}

		// Token: 0x0400610B RID: 24843
		public Color color = new Color(0f, 1f, 0f, 0.25f);

		// Token: 0x0400610C RID: 24844
		public bool drawWire = true;

		// Token: 0x0400610D RID: 24845
		public bool drawSolid = true;
	}
}
