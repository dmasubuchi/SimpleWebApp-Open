using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SimpleWebApp.Models;

namespace SimpleWebApp.Data
{
    public class SqlDataAccess
    {
        // 実際の接続文字列に置き換えてください
        private static string connectionString = "Server=tcp:XXXXX.database.windows.net,1433;Initial Catalog=KeywordsDB;Persist Security Info=False;User ID=tciuser01;Password=XXXXXXXXXXX;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";  

        // キーワードを追加
        public static async Task InsertKeywordAsync(SqlKeyword keyword)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Keywords (KeywordText) VALUES (@KeywordText)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@KeywordText", keyword.KeywordText);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        // キーワードの一覧を取得
        public static async Task<List<SqlKeyword>> GetKeywordsAsync()
        {
            var keywords = new List<SqlKeyword>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Keywords";
                SqlCommand command = new SqlCommand(query, connection);

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        keywords.Add(new SqlKeyword
                        {
                            KeywordID = (int)reader["KeywordID"],
                            // Nullチェックを追加
                            KeywordText = reader["KeywordText"] != null ? reader["KeywordText"].ToString() : string.Empty
                        });
                    }
                }
            }

            return keywords;
        }

        // キーワードを削除
        public static async Task DeleteKeywordAsync(int keywordID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Keywords WHERE KeywordID = @KeywordID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@KeywordID", keywordID);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
