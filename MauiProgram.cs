using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentSearch.models;

namespace StudentSearch;

public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddDbContext<Dat154Context>(options =>
            options.UseSqlServer("data source=dat154.hvl.no,1443;initial catalog=dat154;user id=dat154_rw;password=dat154_rw;TrustServerCertificate=True"));

        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}