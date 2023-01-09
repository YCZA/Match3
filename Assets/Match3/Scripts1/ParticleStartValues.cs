using Match3.Scripts1.Puzzletown.Match3;
using UnityEngine;

// Token: 0x020004E0 RID: 1248
namespace Match3.Scripts1
{
	public class ParticleStartValues : MonoBehaviour
	{
		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x060022AF RID: 8879 RVA: 0x0009973A File Offset: 0x00097B3A
		// (set) Token: 0x060022B0 RID: 8880 RVA: 0x00099742 File Offset: 0x00097B42
		public int UseColor
		{
			get
			{
				return this.useColor;
			}
			set
			{
				this.useColor = value;
				this.SetColors();
			}
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x00099751 File Offset: 0x00097B51
		public Color GetCurrentColor()
		{
			return ParticleStartValues.colors[this.useColor];
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x00099768 File Offset: 0x00097B68
		private void SetColors()
		{
			if (ParticleStartValues.colors == null)
			{
				GemColorToColor componentInParent = base.transform.GetComponentInParent<GemColorToColor>();
				ParticleStartValues.colors = componentInParent.colors;
			}
			for (int i = 0; i < this.particleSystems.Length; i++)
			{
				var go1 = this.particleSystems[i].main; 
				go1.startColor = ParticleStartValues.colors[this.useColor];
			}
		}

		// Token: 0x04004E47 RID: 20039
		public ParticleSystem[] particleSystems;

		// Token: 0x04004E48 RID: 20040
		private static Color[] colors;

		// Token: 0x04004E49 RID: 20041
		private int useColor;
	}
}
