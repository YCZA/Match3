namespace AndroidTools.Tools
{
    public class AndroidSwitch
    {
        public static bool IsBlack()
        {
            return AndroidTools.AndroidAgent.Call<bool>("IsBlack");
        }
        public static bool IsShowAdBtn()
        {
            return AndroidTools.AndroidAgent.Call<bool>("IsShowAdBtn");
        }
        public static bool IsAbroad()
        {
            return AndroidTools.AndroidAgent.Call<bool>("IsAbroad");
        }
    }
}