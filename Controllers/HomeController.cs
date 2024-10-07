using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.Data;
using SimpleWebApp.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SimpleWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // SQL DB: キーワード追加
        [HttpPost]
        public async Task<IActionResult> AddSqlKeyword(string keywordText)
        {
            var keyword = new SqlKeyword { KeywordText = keywordText };
            await SqlDataAccess.InsertKeywordAsync(keyword);
            return RedirectToAction("Index");
        }

        // SQL DB: キーワード一覧表示
        [HttpGet]
        public async Task<IActionResult> GetSqlKeywords()
        {
            var keywords = await SqlDataAccess.GetKeywordsAsync();
            return Json(keywords);
        }

        // SQL DB: キーワード削除
        [HttpPost]
        public async Task<IActionResult> DeleteSqlKeyword(int keywordID)
        {
            await SqlDataAccess.DeleteKeywordAsync(keywordID);
            return RedirectToAction("Index");
        }

        /*

        // Cosmos DB: キーワード追加
        [HttpPost]
        public async Task<IActionResult> AddCosmosKeyword(string keywordText)
        {
            var keyword = new CosmosKeyword { KeywordText = keywordText };
            await CosmosDataAccess.InsertKeywordAsync(keyword);
            return RedirectToAction("Index");
        }

        // Cosmos DB: キーワード一覧表示
        [HttpGet]
        public async Task<IActionResult> GetCosmosKeywords()
        {
            var keywords = await CosmosDataAccess.GetKeywordsAsync();
            return Json(keywords);
        }

        */
        // Cosmos DB: キーワード追加
        [HttpPost]
        public async Task<IActionResult> AddCosmosKeyword(string keywordText)
        {
            var keyword = new CosmosKeyword { keyword = keywordText };  // 'keyword' プロパティを使用
            await CosmosDataAccess.InsertKeywordAsync(keyword);
            return RedirectToAction("Index");
        }

        // Cosmos DB: キーワード一覧表示
        [HttpGet]
        public async Task<IActionResult> GetCosmosKeywords()
        {
            var keywords = await CosmosDataAccess.GetKeywordsAsync();
            return Json(keywords);
        }


        // Cosmos DB: キーワード削除
        [HttpPost]
        public async Task<IActionResult> DeleteCosmosKeyword(string keywordID)
        {
            await CosmosDataAccess.DeleteKeywordAsync(keywordID);
        //    await CosmosDataAccess.DeleteKeywordAsync(keywordID, "defaultPartition");
            return RedirectToAction("Index");
        }

        // 初期画面表示
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // エラーハンドリング
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
