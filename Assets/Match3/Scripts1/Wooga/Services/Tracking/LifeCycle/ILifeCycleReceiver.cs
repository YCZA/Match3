namespace Match3.Scripts1.Wooga.Services.Tracking.LifeCycle
{
	// Token: 0x0200043F RID: 1087
	public interface ILifeCycleReceiver
	{
		// Token: 0x06001FB2 RID: 8114
		void Awake();

		// Token: 0x06001FB3 RID: 8115
		void Start();

		// Token: 0x06001FB4 RID: 8116
		void Update(float deltaTime);

		// Token: 0x06001FB5 RID: 8117
		void OnApplicationPause(bool paused);
	}
}
