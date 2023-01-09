namespace AndroidTools.Tools
{
    public class AndroidTip
    {
        public static void ShowTip(string content)
        {
            AndroidTools.AndroidAgent.Call("showTip", content);
        }
		
        public static void ShowTip(string contentZh, string contentEn)
        {
            string content = AndroidSwitch.IsAbroad() ? contentEn : contentZh; 
            AndroidTools.AndroidAgent.Call("showTip", content);
        }
    }
}