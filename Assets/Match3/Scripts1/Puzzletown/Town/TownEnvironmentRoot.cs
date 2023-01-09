using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x0200094B RID: 2379
	public class TownEnvironmentRoot : ASceneRoot
	{
		// Token: 0x060039CD RID: 14797 RVA: 0x0011C39F File Offset: 0x0011A79F
		protected override void Go()
		{
			base.Go();
			this.isInitialized = true;
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x0011C3B0 File Offset: 0x0011A7B0
		public IEnumerator Start()
		{
			while (!Camera.main || !this.isInitialized)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060039CF RID: 14799 RVA: 0x0011C3CC File Offset: 0x0011A7CC
		private void OnApplicationFocus(bool hasFocus)
		{
			if (hasFocus)
			{
				if ((DateTime.UtcNow - this.suspendTime).TotalSeconds <= 30.0)
				{
					return;
				}
				if (!BlockerManager.global.HasBlockers)
				{
					BlockerManager.global.Append(new PanCameraFlow(PanCameraTarget.CurrentFocusPoint, 0f, 1f, false));
				}
			}
			else
			{
				this.suspendTime = DateTime.UtcNow;
			}
		}

		// Token: 0x040061D9 RID: 25049
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x040061DA RID: 25050
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040061DB RID: 25051
		[WaitForService(true, true)]
		private QuestService quests;

		// Token: 0x040061DC RID: 25052
		[WaitForRoot(false, false)]
		private VillagersControllerRoot villagers;

		// Token: 0x040061DD RID: 25053
		[NonSerialized]
		public CameraPanManager cameraPan;

		// Token: 0x040061DE RID: 25054
		public BoxCollider cameraBounds;

		// Token: 0x040061DF RID: 25055
		public Camera shadowsCamera;

		// Token: 0x040061E0 RID: 25056
		private bool isInitialized;

		// Token: 0x040061E1 RID: 25057
		public TownPathfinding map;

		// Token: 0x040061E2 RID: 25058
		private DateTime suspendTime = DateTime.MinValue;
	}
}
