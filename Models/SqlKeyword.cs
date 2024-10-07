namespace SimpleWebApp.Models
{
    public class SqlKeyword
    {
        public int KeywordID { get; set; }  // SQL用のID (int)
        public string KeywordText { get; set; } = string.Empty;  // キーワードのテキスト
    }
}
