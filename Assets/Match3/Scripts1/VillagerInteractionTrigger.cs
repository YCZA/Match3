using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;

// Token: 0x020008CA RID: 2250
namespace Match3.Scripts1
{
	public class VillagerInteractionTrigger : MonoBehaviour, IEditorDescription
	{
		// Token: 0x060036C2 RID: 14018 RVA: 0x0010AEA8 File Offset: 0x001092A8
		private void OnTriggerEnter(Collider other)
		{
			VillagerInteractionTrigger component = other.GetComponent<VillagerInteractionTrigger>();
			if (!component || component.type != this.type)
			{
				return;
			}
			this.HandleOnParent(component);
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x0010AEE0 File Offset: 0x001092E0
		public string GetEditorDescription()
		{
			return this.type.ToString();
		}

		// Token: 0x04005EF4 RID: 24308
		public VillagerInteraction type;
	}
}
