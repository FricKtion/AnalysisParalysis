using AnalysisParalysis.Data;
using AnalysisParalysis.Data.Definitions;
using AnalysisParalysis.Services;
using AnalysisParalysis.Services.Definitions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

builder.Services.AddHttpClient<IBoardGameRepository>();

builder.Services.AddScoped<IAppSettingService, AppSettingService>();
builder.Services.AddScoped<IBoardGameRepository, BoardGameRepository>();

builder.Services.AddSingleton<ISessionHostingService, SessionHostingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
