using Match3.Scripts1.Spine.Unity;
using UnityEngine;

// Token: 0x020008FC RID: 2300
namespace Match3.Scripts1
{
	public class SpineEventController : MonoBehaviour
	{
		// Token: 0x06003805 RID: 14341 RVA: 0x00111F04 File Offset: 0x00110304
		private void Start()
		{
			SkeletonAnimation component = base.GetComponent<SkeletonAnimation>();
			if (component == null)
			{
				return;
			}
			component.state.Event += this.HandleEvent;
			component.state.Start += delegate(Spine.AnimationState state, int trackIndex)
			{
				global::UnityEngine.Debug.Log(string.Format("track {0} started a new animation.", trackIndex));
			};
			component.state.End += delegate(Spine.AnimationState A_0, int A_1)
			{
				global::UnityEngine.Debug.Log("An animation ended!");
			};
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x00111F8C File Offset: 0x0011038C
		private void HandleEvent(Spine.AnimationState state, int trackIndex, Spine.Event e)
		{
			if (e.Data.Name == this.footstepEventName)
			{
				base.GetComponent<AudioSource>().Stop();
				base.GetComponent<AudioSource>().Play();
				global::UnityEngine.Debug.Log("Play a footstep sound!");
			}
		}

		// Token: 0x0400602B RID: 24619
		[SpineEvent("", "")]
		public string footstepEventName = "page";
	}
}
