using Match3.Scripts2.Env;

// Token: 0x02000AB4 RID: 2740
namespace Match3.Scripts1
{
	public interface ISbsEnvironmentConfig
	{
		// Token: 0x06004102 RID: 16642
		string GetAdminId(GameEnvironment.Environment env);

		// Token: 0x06004103 RID: 16643
		string GetAdminPw(GameEnvironment.Environment env);

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06004104 RID: 16644
		string PLATFORM_PREFIX { get; }

		// Token: 0x06004105 RID: 16645
		string GetId(GameEnvironment.Environment env);

		// Token: 0x06004106 RID: 16646
		bool HasId(string id);
	}
}
