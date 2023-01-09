namespace Match3.Scripts2.SRDebug.Editor.TableToolPlus.Structure
{
    public class Node
    {
        public int row;
        public int col;
        private string content = "";
        
        public Node parentNode;

        public Node(string content, int row, int col)
        {
            this.content = content;
            this.row = row;
            this.col = col;
        }
        
        public string GetNodeName()
        {
            var array = content.Split(':');
            return array.Length > 0 ? array[0] : "";
        }

        public string GetNodeAlias()
        {
            var array = content.Split(':');
            return array.Length > 2 ? array[2].ToLower() : "";
        }

        public string GetNodeType()
        {
            var array = content.Split(':');
            return array.Length > 1 ? array[1].ToLower() : "";
        }
    }
}