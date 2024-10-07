using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using SimpleWebApp.Models;
using System;

namespace SimpleWebApp.Data
{
    public class CosmosDataAccess
    {
        private static string cosmosEndpoint = "https://XXXXXXX.documents.azure.com:443";  // 実際のエンドポイントに置き換えてください
        private static string cosmosKey = "XXXXXXXXXXXXXXXX==";  // 実際のキーに置き換えてください
        private static string databaseId = "KeywordsDB";
        private static string containerId = "KeywordsContainer";

        private static CosmosClient client = new CosmosClient(cosmosEndpoint, cosmosKey);

        // キーワードを追加
        public static async Task InsertKeywordAsync(CosmosKeyword keyword)
        {
            var container = client.GetContainer(databaseId, containerId);

            // ID と PartitionKey の設定
            keyword.KeywordID = Guid.NewGuid().ToString();  // ID をユニークに生成
            keyword.partitionKey = keyword.KeywordID;       // PartitionKey に KeywordID を設定

            // キーワードを Cosmos DB に保存 (PartitionKey は KeywordID を使用)
            await container.CreateItemAsync(keyword, new PartitionKey(keyword.KeywordID));
        }

        // キーワードの一覧を取得
        public static async Task<List<CosmosKeyword>> GetKeywordsAsync()
        {
            var container = client.GetContainer(databaseId, containerId);
            var sqlQueryText = "SELECT * FROM c";
            var queryDefinition = new QueryDefinition(sqlQueryText);
            var queryResultSetIterator = container.GetItemQueryIterator<CosmosKeyword>(queryDefinition);

            List<CosmosKeyword> keywords = new List<CosmosKeyword>();

            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (var keyword in currentResultSet)
                {
                    keywords.Add(keyword);  // 正しいプロパティが使われるように修正
                }
            }

            return keywords;
        }

        // キーワードを削除 (KeywordID ベース)
        public static async Task DeleteKeywordAsync(string keywordID)
        {
            var container = client.GetContainer(databaseId, containerId);

            // PartitionKey を KeywordID に設定して削除
            await container.DeleteItemAsync<CosmosKeyword>(keywordID, new PartitionKey(keywordID));
        }

        // キーワードを削除 (keyword ベース)
        public static async Task DeleteKeywordByKeywordAsync(string keyword)
        {
            var container = client.GetContainer(databaseId, containerId);
            
            // キーワードに基づいて該当するアイテムを検索
            var sqlQueryText = "SELECT * FROM c WHERE c.keyword = @keyword";
            var queryDefinition = new QueryDefinition(sqlQueryText).WithParameter("@keyword", keyword);
            var queryResultSetIterator = container.GetItemQueryIterator<CosmosKeyword>(queryDefinition);

            // 該当アイテムを削除
            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (var item in currentResultSet)
                {
                    await container.DeleteItemAsync<CosmosKeyword>(item.KeywordID, new PartitionKey(item.KeywordID));
                }
            }
        }

    }
}
