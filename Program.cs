using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var pagesPath = Path.Combine(app.Environment.ContentRootPath, "Pages");

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? "/";

    if (path != "/" && !Path.HasExtension(path))
    {
        context.Request.Path = path + ".html";
    }

    await next();
});

app.UseStaticFiles();

app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = new PhysicalFileProvider(pagesPath),
    RequestPath = ""
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(pagesPath),
    RequestPath = ""
});

app.Run();