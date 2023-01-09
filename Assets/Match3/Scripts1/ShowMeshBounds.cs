using UnityEngine;

// Token: 0x0200094F RID: 2383
namespace Match3.Scripts1
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter))]
	public class ShowMeshBounds : MonoBehaviour
	{
		// Token: 0x060039F9 RID: 14841 RVA: 0x0011DF58 File Offset: 0x0011C358
		private void Start()
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (!component)
			{
				return;
			}
			Mesh sharedMesh = component.sharedMesh;
			if (!sharedMesh)
			{
				return;
			}
			sharedMesh.bounds = new Bounds(new Vector3(sharedMesh.bounds.center.x, 0f, sharedMesh.bounds.center.z), new Vector3(sharedMesh.bounds.size.x, sharedMesh.bounds.size.y, sharedMesh.bounds.size.z));
		}
	}
}
