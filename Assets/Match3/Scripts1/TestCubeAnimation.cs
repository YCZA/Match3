using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B2A RID: 2858
namespace Match3.Scripts1
{
	[RequireComponent(typeof(Renderer))]
	public class TestCubeAnimation : MonoBehaviour
	{
		// Token: 0x06004318 RID: 17176 RVA: 0x001575C0 File Offset: 0x001559C0
		private void Start()
		{
			this.material = new Material(base.GetComponent<Renderer>().sharedMaterial);
			base.GetComponent<Renderer>().sharedMaterial = this.material;
			foreach (string text in this.extraKeywords)
			{
				this.keywords[text] = this.material.IsKeywordEnabled(text);
			}
			foreach (string text2 in this.material.shaderKeywords)
			{
				this.keywords[text2] = this.material.IsKeywordEnabled(text2);
			}
			for (int k = 0; k < this.makeCameraCopies; k++)
			{
				global::UnityEngine.Object.Instantiate<Camera>(Camera.main);
			}
		}

		// Token: 0x06004319 RID: 17177 RVA: 0x00157698 File Offset: 0x00155A98
		private void Update()
		{
			Vector3 a = new Vector3(this.speed * Mathf.Sin(Time.time * this.timescale), this.speed * Mathf.Cos(Time.time * this.timescale), this.speed);
			base.transform.Rotate(a * Time.deltaTime);
		}

		// Token: 0x0600431A RID: 17178 RVA: 0x001576F8 File Offset: 0x00155AF8
		private void OnGUI()
		{
			if (!this.material)
			{
				return;
			}
			float d = (float)Screen.width / 240f;
			GUI.matrix *= Matrix4x4.Scale(Vector3.one * d);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			foreach (string text in this.keywords.Keys)
			{
				bool flag = this.material.IsKeywordEnabled(text);
				bool flag2 = GUILayout.Toggle(flag, text, new GUILayoutOption[0]);
				if (flag2 != flag)
				{
					if (flag2)
					{
						this.material.EnableKeyword(text);
					}
					else
					{
						this.material.DisableKeyword(text);
					}
				}
			}
			GUILayout.EndVertical();
		}

		// Token: 0x04006BC6 RID: 27590
		private Material material;

		// Token: 0x04006BC7 RID: 27591
		private Dictionary<string, bool> keywords = new Dictionary<string, bool>();

		// Token: 0x04006BC8 RID: 27592
		public float speed = 20f;

		// Token: 0x04006BC9 RID: 27593
		public float timescale = 1f;

		// Token: 0x04006BCA RID: 27594
		public int makeCameraCopies = 10;

		// Token: 0x04006BCB RID: 27595
		public string[] extraKeywords;
	}
}
