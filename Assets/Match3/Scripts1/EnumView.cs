using System.Text.RegularExpressions;
using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.Signals;
using Match3.Scripts1.Wooga.UI;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020009B6 RID: 2486
namespace Match3.Scripts1
{
	public abstract class EnumView<T> : ATableViewReusableCell, IDataView<T>
	{
		// Token: 0x06003C4B RID: 15435 RVA: 0x000CF9ED File Offset: 0x000CDDED
		public void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnClick));
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x000CFA0B File Offset: 0x000CDE0B
		private void OnClick()
		{
			this.onCheat.Dispatch(this.cheat);
			this.HandleOnParent(this.cheat);
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x000CFA2A File Offset: 0x000CDE2A
		public virtual void Show(T cheat)
		{
			this.cheat = cheat;
			this.label.text = EnumView<T>.ConvertToHumanReadable(cheat.ToString());
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06003C4E RID: 15438 RVA: 0x000CFA50 File Offset: 0x000CDE50
		public override int reusableId
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x000CFA53 File Offset: 0x000CDE53
		public static string ConvertToHumanReadable(string camelCasedString)
		{
			return Regex.Replace(camelCasedString, "(?<a>(?<!^)((?:[A-Z][a-z])|(?:(?<!^[A-Z]+)[A-Z0-9]+(?:(?=[A-Z][a-z])|$))|(?:[0-9]+)))", " ${a}");
		}

		// Token: 0x04006463 RID: 25699
		private T cheat;

		// Token: 0x04006464 RID: 25700
		public Text label;

		// Token: 0x04006465 RID: 25701
		public Button button;

		// Token: 0x04006466 RID: 25702
		public readonly Signal<T> onCheat = new Signal<T>();
	}
}
