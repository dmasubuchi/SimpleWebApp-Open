using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();  // MVC パターンをサポートするためのサービスを追加
    }

    public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {
        if (app.ApplicationServices.GetService<IHostEnvironment>().IsDevelopment())
        {
            app.UseDeveloperExceptionPage();  // 開発環境での例外ページを表示
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");  // 本番環境でのエラーハンドリング
        }

        app.UseStaticFiles();  // 静的ファイルを使用可能にする
        app.UseRouting();  // ルーティングを有効にする

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");  // デフォルトルート
        });
    }
}
