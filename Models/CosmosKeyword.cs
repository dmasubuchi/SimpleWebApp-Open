namespace SimpleWebApp.Models
{
    public class CosmosKeyword
    {
        // ID プロパティ
        public string id { get; set; } = Guid.NewGuid().ToString(); // null 非許容のプロパティに初期化

        // キーワード
        public string keyword { get; set; } = string.Empty; // 初期値を空文字列で初期化

        // PartitionKey
        public string partitionKey { get; set; } = string.Empty; // 初期値を空文字列で初期化

        // KeywordID を追加
        public string KeywordID { get; set; } = Guid.NewGuid().ToString(); // これも初期化
    }
}
