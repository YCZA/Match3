using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Wooga.Services;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200082D RID: 2093
	public abstract class AWeeklyEventService : AService
	{
		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x0600340A RID: 13322
		protected abstract WeeklyEventType WeeklyEventType { get; }

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x0600340B RID: 13323
		protected abstract AWeeklyEventDataService DataService { get; }

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x0600340C RID: 13324 RVA: 0x000D8F8C File Offset: 0x000D738C
		private List<EventConfigContainer> CurrentlyRunningEvents
		{
			get
			{
				List<EventConfigContainer> result;
				if ((result = this.currentlyRunningEvents) == null)
				{
					result = (this.currentlyRunningEvents = this.GetCurrentRemoteEventsConfigs());
				}
				return result;
			}
		}

		// Token: 0x0600340D RID: 13325 RVA: 0x000D8FB5 File Offset: 0x000D73B5
		public List<EventConfigContainer> GetUpcomingEventConfigs()
		{
			return EventConfig.GetUpcomingEventConfigs<EventConfigContainer>(this.timeService.Now.ToUnixTimeStamp(), this.GetAllRemoteEventsConfigContainer());
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x000D8FD4 File Offset: 0x000D73D4
		protected IEnumerator WaitForSbsRoutine()
		{
			while (!SBS.IsAuthenticated())
			{
				yield return new WaitForSeconds(0.5f);
			}
			this.RefreshActiveEvent();
			yield break;
		}

		// Token: 0x0600340F RID: 13327 RVA: 0x000D8FEF File Offset: 0x000D73EF
		protected bool IsSavedEvent(string mostRecentId)
		{
			return mostRecentId == this.DataService.EventId;
		}

		// Token: 0x06003410 RID: 13328 RVA: 0x000D9002 File Offset: 0x000D7402
		protected virtual void PersistNewActiveEvent(EventConfigContainer activeEvent)
		{
			this.DataService.EventData.SetActiveConfig(activeEvent);
			this.SaveWeeklyEventConfigToPlayerPrefs(activeEvent);
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x000D901C File Offset: 0x000D741C
		protected virtual void OverwriteActiveEvent(EventConfigContainer updatedEvent)
		{
			this.DataService.EventData.UpdateConfig(updatedEvent);
			this.SaveWeeklyEventConfigToPlayerPrefs(updatedEvent);
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x000D9038 File Offset: 0x000D7438
		protected void SaveWeeklyEventConfigToPlayerPrefs(EventConfigContainer updatedEvent)
		{
			string value = JsonUtility.ToJson(updatedEvent.config);
			PlayerPrefs.SetString(this.DataService.ConfigKey, value);
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x000D9064 File Offset: 0x000D7464
		private void RefreshActiveEvent()
		{
			this.currentlyRunningEvents = null;
			this.activeEvents.Clear();
			foreach (EventConfigContainer item in this.CurrentlyRunningEvents)
			{
				this.activeEvents.Add(item);
			}
			if (this.activeEvents.Count > 0)
			{
				List<EventConfigContainer> list = this.activeEvents;
				if (AWeeklyEventService._003C_003Ef__mg_0024cache0 == null)
				{
					AWeeklyEventService._003C_003Ef__mg_0024cache0 = new Comparison<EventConfigContainer>(EventConfig.SortByEndTimeAscending<EventConfigContainer>);
				}
				list.Sort(AWeeklyEventService._003C_003Ef__mg_0024cache0);
				EventConfigContainer eventConfigContainer = this.activeEvents[0];
				bool flag = !this.IsSavedEvent(eventConfigContainer.id);
				if (flag)
				{
					this.PersistNewActiveEvent(eventConfigContainer);
				}
				else if (eventConfigContainer.config.shouldOverwrite)
				{
					this.OverwriteActiveEvent(eventConfigContainer);
				}
			}
		}

		// Token: 0x06003414 RID: 13332 RVA: 0x000D9158 File Offset: 0x000D7558
		private List<EventConfigContainer> GetCurrentRemoteEventsConfigs()
		{
			List<EventConfigContainer> list = new List<EventConfigContainer>();
			int now = this.timeService.Now.ToUnixTimeStamp();
			List<EventConfigContainer> allRemoteEventsConfigContainer = this.GetAllRemoteEventsConfigContainer();
			if (allRemoteEventsConfigContainer != null)
			{
				list.AddRange(from container in allRemoteEventsConfigContainer
				where container.IsOngoing(now) && container.config.weeklyEventType == this.WeeklyEventType
				select container);
			}
			return list;
		}

		// Token: 0x06003415 RID: 13333 RVA: 0x000D91B4 File Offset: 0x000D75B4
		private List<EventConfigContainer> GetAllRemoteEventsConfigContainer()
		{
			if (this.configService.SbsConfig._events != null && this.configService.SbsConfig._events.events != null)
			{
				return this.configService.SbsConfig._events.events;
			}
			return null;
		}

		// Token: 0x04005BEF RID: 23535
		[WaitForService(true, true)]
		protected TimeService timeService;

		// Token: 0x04005BF0 RID: 23536
		[WaitForService(true, true)]
		protected ConfigService configService;

		// Token: 0x04005BF1 RID: 23537
		[WaitForService(true, true)]
		protected GameStateService gameStateService;

		// Token: 0x04005BF2 RID: 23538
		private readonly List<EventConfigContainer> activeEvents = new List<EventConfigContainer>();

		// Token: 0x04005BF3 RID: 23539
		private List<EventConfigContainer> currentlyRunningEvents;

		// Token: 0x04005BF4 RID: 23540
		[CompilerGenerated]
		private static Comparison<EventConfigContainer> _003C_003Ef__mg_0024cache0;
	}
}
