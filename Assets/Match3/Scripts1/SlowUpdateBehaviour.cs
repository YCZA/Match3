using System.Collections;
using UnityEngine;

// Token: 0x02000882 RID: 2178
namespace Match3.Scripts1
{
	public class SlowUpdateBehaviour : MonoBehaviour
	{
		// Token: 0x0600357D RID: 13693 RVA: 0x00100A62 File Offset: 0x000FEE62
		public void Initialise(SlowUpdate slowUpdate, int updateTime)
		{
			this.slowUpdate = slowUpdate;
			this.updateTime = updateTime;
			this.isInitialised = true;
			if (base.gameObject.activeSelf)
			{
				this.slowUpdateRoutine = base.StartCoroutine(this.SlowUpdateRoutine());
			}
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x00100A9C File Offset: 0x000FEE9C
		private void OnEnable()
		{
			if (this.slowUpdateRoutine != null)
			{
				base.StopCoroutine(this.SlowUpdateRoutine());
			}
			if (this.isInitialised && this.slowUpdateRoutine != null)
			{
				this.slowUpdateRoutine = base.StartCoroutine(this.SlowUpdateRoutine());
			}
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x00100AE8 File Offset: 0x000FEEE8
		private void OnDisable()
		{
			if (this.slowUpdateRoutine != null)
			{
				base.StopCoroutine(this.SlowUpdateRoutine());
			}
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x00100B04 File Offset: 0x000FEF04
		private IEnumerator SlowUpdateRoutine()
		{
			WaitForSeconds waitTime = new WaitForSeconds((float)this.updateTime);
			while (base.gameObject != null && base.gameObject.activeInHierarchy)
			{
				if (this.slowUpdate != null)
				{
					this.slowUpdate();
				}
				yield return waitTime;
			}
			yield break;
		}

		// Token: 0x04005D5E RID: 23902
		private SlowUpdate slowUpdate;

		// Token: 0x04005D5F RID: 23903
		private int updateTime = 1;

		// Token: 0x04005D60 RID: 23904
		private Coroutine slowUpdateRoutine;

		// Token: 0x04005D61 RID: 23905
		private bool isInitialised;
	}
}
