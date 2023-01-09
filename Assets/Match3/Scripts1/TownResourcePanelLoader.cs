using System.Collections;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.UnityFramework;
using UnityEngine;

// Token: 0x02000A13 RID: 2579
namespace Match3.Scripts1
{
	public class TownResourcePanelLoader : MonoBehaviour
	{
		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06003E03 RID: 15875 RVA: 0x0013A269 File Offset: 0x00138669
		// (set) Token: 0x06003E04 RID: 15876 RVA: 0x0013A274 File Offset: 0x00138674
		public TownResourceElement VisibleElements
		{
			get
			{
				return this.visibleResources;
			}
			set
			{
				this.visibleResources = value;
				this.state.visibleResources = this.visibleResources;
				if (this.resourcePanel)
				{
					this.resourcePanel.RemoveState(this.state);
					this.resourcePanel.Show(this.state, true);
				}
			}
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x0013A2CC File Offset: 0x001386CC
		public void UpdateOrder()
		{
			base.StartCoroutine(this.UpdateOrderRoutine());
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x0013A2DC File Offset: 0x001386DC
		private void OnEnable()
		{
			if (!this.resourcePanel)
			{
				this.Init();
			}
			else
			{
				this.state.canvasOrder = this.state.canvas.sortingOrder + 1;
				this.resourcePanel.Show(this.state, true);
			}
		}

		// Token: 0x06003E07 RID: 15879 RVA: 0x0013A334 File Offset: 0x00138734
		private void OnDisable()
		{
			if (this.resourcePanel)
			{
				this.resourcePanel.RemoveState(this.state);
			}
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x0013A357 File Offset: 0x00138757
		private Coroutine Init()
		{
			if (this.initRoutine == null)
			{
				this.initRoutine = base.StartCoroutine(this.InitRoutine());
			}
			return this.initRoutine;
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x0013A37C File Offset: 0x0013877C
		private IEnumerator UpdateOrderRoutine()
		{
			yield return this.Init();
			Canvas canvas = this.GetCanvas();
			this.state.canvasOrder = canvas.sortingOrder + 1;
			this.resourcePanel.RemoveState(this.state);
			this.resourcePanel.Show(this.state, true);
			yield break;
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x0013A398 File Offset: 0x00138798
		private IEnumerator InitRoutine()
		{
			Canvas canvas = this.GetCanvas();
			this.state = new TownResourcePanelRoot.State(this.visibleResources, canvas.sortingOrder + 1, canvas, false, this.keepSpaceForCloseButton);
			yield return SceneManager.Instance.Inject(this);
			yield return this.resourcePanel.OnInitialized;
			this.resourcePanel.Show(this.state, true);
			yield break;
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x0013A3B4 File Offset: 0x001387B4
		private Canvas GetCanvas()
		{
			Canvas canvas = base.GetComponentInChildren<Canvas>(true);
			if (!canvas)
			{
				canvas = base.GetComponentInParent<Canvas>();
			}
			return canvas;
		}

		// Token: 0x040066DC RID: 26332
		[WaitForRoot(false, false)]
		private TownResourcePanelRoot resourcePanel;

		// Token: 0x040066DD RID: 26333
		[EnumFlag]
		public TownResourceElement visibleResources;

		// Token: 0x040066DE RID: 26334
		[SerializeField]
		private bool keepSpaceForCloseButton;

		// Token: 0x040066DF RID: 26335
		private TownResourcePanelRoot.State state;

		// Token: 0x040066E0 RID: 26336
		private Coroutine initRoutine;
	}
}
