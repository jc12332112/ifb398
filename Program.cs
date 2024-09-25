using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// 配置固定查找的默认文件
var defaultFilesOptions = new DefaultFilesOptions();
defaultFilesOptions.DefaultFileNames.Clear();
defaultFilesOptions.DefaultFileNames.Add("index.html");
app.UseDefaultFiles(defaultFilesOptions); // 在静态文件服务之前使用默认文件

app.UseStaticFiles();  // 启用静态文件服务

app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Hello from LMS Plugin!");

app.Logger.LogInformation("Application started. Listening on {url}", app.Urls);
app.Run();
