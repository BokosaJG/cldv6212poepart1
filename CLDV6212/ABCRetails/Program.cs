using ABCRetails.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Bind configuration for Azure storage
builder.Services.Configure<AzureStorageOptions>(builder.Configuration.GetSection("AzureStorage"));

// Register storage service
builder.Services.AddSingleton<IAzureStorageService, AzureStorageService>();

var app = builder.Build();

// Initialize Azure resources at startup
using (var scope = app.Services.CreateScope())
{
    var storage = scope.ServiceProvider.GetRequiredService<IAzureStorageService>();
    await storage.InitializeAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();