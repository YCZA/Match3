namespace Match3.Scripts2.Network
{
    public class URL
    {
        // private static readonly string host = "aws1.duofanglajiao.com";
        private static readonly string host = "81.70.78.70";    // 测试服务器
        // private static readonly string host = "192.168.1.145";    // 测试服务器
        private static readonly string port = "21000";

        public static string ServerURL => $"http://{URL.host}:{URL.port}";
        
        // ----time----
        public static string Ping => $"{ServerURL}/api/time/ping";
        
        // ----archive----
        public static string PullArchive => $"{ServerURL}/api/archive/pull";
        public static string PushArchive => $"{ServerURL}/api/archive/push";
        public static string DelArchive => $"{ServerURL}/api/archive/del";
        
        // ----login----
        public static string Login => $"{ServerURL}/api/login/login";
        
        // ----purchase----
        public static string VerifyPurchaseResult => $"{ServerURL}/api/purchase/verify_purchase_result";
        
        // ----event----
        public static string PostEventInfo => $"{ServerURL}/api/event/post_event_info";
        
        // ----testarchive----
        public static string TestArchive => $"{ServerURL}/api/gm/testarchive";
        
        // ----test----
        public static string Test => $"{ServerURL}/api/testcode";
    }
}