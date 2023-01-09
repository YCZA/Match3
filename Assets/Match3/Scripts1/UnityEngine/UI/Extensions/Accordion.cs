using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000B7D RID: 2941
	[RequireComponent(typeof(VerticalLayoutGroup), typeof(ContentSizeFitter), typeof(ToggleGroup))]
	[AddComponentMenu("UI/Extensions/Accordion/Accordion Group")]
	public class Accordion : MonoBehaviour
	{
		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x060044D3 RID: 17619 RVA: 0x0015D8BF File Offset: 0x0015BCBF
		// (set) Token: 0x060044D4 RID: 17620 RVA: 0x0015D8C7 File Offset: 0x0015BCC7
		public Accordion.Transition transition
		{
			get
			{
				return this.m_Transition;
			}
			set
			{
				this.m_Transition = value;
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x060044D5 RID: 17621 RVA: 0x0015D8D0 File Offset: 0x0015BCD0
		// (set) Token: 0x060044D6 RID: 17622 RVA: 0x0015D8D8 File Offset: 0x0015BCD8
		public float transitionDuration
		{
			get
			{
				return this.m_TransitionDuration;
			}
			set
			{
				this.m_TransitionDuration = value;
			}
		}

		// Token: 0x04006C96 RID: 27798
		[SerializeField]
		private Accordion.Transition m_Transition;

		// Token: 0x04006C97 RID: 27799
		[SerializeField]
		private float m_TransitionDuration = 0.3f;

		// Token: 0x02000B7E RID: 2942
		public enum Transition
		{
			// Token: 0x04006C99 RID: 27801
			Instant,
			// Token: 0x04006C9A RID: 27802
			Tween
		}
	}
}
