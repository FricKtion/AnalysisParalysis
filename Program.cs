using AnalysisParalysis.Data;
using AnalysisParalysis.Data.Definitions;
using AnalysisParalysis.Hubs;
using AnalysisParalysis.Services;
using AnalysisParalysis.Services.Definitions;
using AnalysisParalysis.Services.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.ResponseCompression;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddSignalR(options => 
{
    options.EnableDetailedErrors = true;
    options.MaximumReceiveMessageSize = 64000;
});

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddHttpClient<IBoardGameRepository>();

builder.Services.AddScoped<User>();

builder.Services.AddSingleton<IAppSettingService, AppSettingService>();
builder.Services.AddSingleton<IBoardGameRepository, BoardGameRepository>();
builder.Services.AddSingleton<ISessionHostingService, SessionHostingService>();

// Recommended by Microsoft when implementing SignalR
builder.Services.AddResponseCompression(opts => 
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});

var app = builder.Build();

// Recommended by Microsoft when implementing SignalR
app.UseResponseCompression();

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
app.MapHub<SessionHub>("/sessionHub");

app.Run();
