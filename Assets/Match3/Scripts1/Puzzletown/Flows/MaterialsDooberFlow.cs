using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004BE RID: 1214
	public class MaterialsDooberFlow : IBlocker
	{
		// Token: 0x0600221B RID: 8731 RVA: 0x00093B73 File Offset: 0x00091F73
		public MaterialsDooberFlow(RectTransform target, MaterialAmount mat, Transform source)
		{
			this.m_target = target;
			this.m_materialAmount = mat;
			this.m_source = source;
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x0600221C RID: 8732 RVA: 0x00093B90 File Offset: 0x00091F90
		public bool BlockInput
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x00093B94 File Offset: 0x00091F94
		public IEnumerator ExecuteRoutine()
		{
			yield return SceneManager.Instance.Inject(this);
			this.doobers.SpawnDoobers(this.m_materialAmount, this.m_source, this.m_target, null);
			yield return new WaitForSeconds(1f);
			while (Doober.ActiveDoobers > 0)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x04004D6D RID: 19821
		private const float MINIMUM_TIME = 1f;

		// Token: 0x04004D6E RID: 19822
		[WaitForRoot(false, false)]
		private DoobersRoot doobers;

		// Token: 0x04004D6F RID: 19823
		private readonly RectTransform m_target;

		// Token: 0x04004D70 RID: 19824
		private readonly MaterialAmount m_materialAmount;

		// Token: 0x04004D71 RID: 19825
		private readonly Transform m_source;
	}
}
