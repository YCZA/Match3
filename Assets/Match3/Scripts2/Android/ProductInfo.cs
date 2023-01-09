namespace AndroidTools
{
    public class ProductInfo
    {
        public string id;
        public string id_Str;
        public string name;
        public string price;
        public string desc;
        public string currency;
        public string payType = null;
        
        public ProductInfo(string id, string id_Str, string name, string price, string desc, string currency) {
            this.id = id;
            this.id_Str = id_Str;
            this.name = name;
            this.price = price;
            this.desc = desc;
            this.currency = currency;
        }
    }
}