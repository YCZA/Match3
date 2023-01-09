using Match3.Scripts1.Puzzletown.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006EB RID: 1771
	public class M3_LevelSelectionLockView : UiSimpleView<LevelSelectionData.LockReason>
	{
		private void OnEnable()
		{
			// 审核版不显示未完待续
			// #if REVIEW_VERSION
			// {
			// 	if (state == LevelSelectionData.LockReason.ComingSoon)
			// 	{
			// 		transform.parent.gameObject.SetActive(false);
			// 	}
			// }
			// #endif
		}
	}
}
