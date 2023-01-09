using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008B9 RID: 2233
namespace Match3.Scripts1
{
	public class catEyesBehavior : MonoBehaviour
	{
		// Token: 0x06003687 RID: 13959 RVA: 0x00109CE4 File Offset: 0x001080E4
		private void Start()
		{
			this.setChildren(-1);
			base.Invoke("UpdateEyes", global::UnityEngine.Random.Range(this.minDelay, this.maxDelay));
		}

		// Token: 0x06003688 RID: 13960 RVA: 0x00109D09 File Offset: 0x00108109
		private void UpdateEyes()
		{
			this.setChildren(global::UnityEngine.Random.Range(0, base.transform.childCount));
			base.Invoke("UpdateEyes", global::UnityEngine.Random.Range(this.minDelay, this.maxDelay));
		}

		// Token: 0x06003689 RID: 13961 RVA: 0x00109D40 File Offset: 0x00108140
		private void setChildren(int index)
		{
			if (this.eyesActive)
			{
				index = -1;
				this.eyesActive = false;
			}
			int num = 0;
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (index == num)
					{
						transform.gameObject.SetActive(true);
						this.eyesActive = true;
					}
					else
					{
						transform.gameObject.SetActive(false);
					}
					num++;
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
		}

		// Token: 0x04005E88 RID: 24200
		public float minDelay = 1f;

		// Token: 0x04005E89 RID: 24201
		public float maxDelay = 2f;

		// Token: 0x04005E8A RID: 24202
		private bool eyesActive;
	}
}
