//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using RazorPagesMovie.Models;
//using System;
//namespace RazorPagesMovie
//{
//	public class Program
//	{
//		public static void Main(string[] args)
//		{
//			var host = CreateHostBuilder(args).Build();
//			using (var scope = host.Services.CreateScope())
//			{
//				var services = scope.ServiceProvider;
//				try
//				{
//					SeedData.Initialize(services);
//				}
//				catch (Exception ex)
//				{
//					var logger = services.GetRequiredService<ILogger<Program>>();
//					logger.LogError(ex, "An error occurred seeding the DB.");
//				}
//			}
//			host.Run();
//		}
//		public static IHostBuilder CreateHostBuilder(string[] args) =>
//		Host.CreateDefaultBuilder(args)
//		.ConfigureWebHostDefaults(webBuilder =>
//		{
//			webBuilder.UseStartup<Startup>();
//			  
//		});
//	}
//}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

///
builder.Services.AddDbContext<RazorPagesMovieContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("RazorPagesMovieContext") ?? throw new InvalidOperationException("Connection string 'RazorPagesMovieContext' not found.")));

///
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

app.UseAuthorization();

app.MapRazorPages();

//
// Data seeding
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	try
	{
		SeedData.Initialize(services);
	}
	catch (Exception ex)
	{
		var logger = services.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "An error occurred seeding the DB.");
	}
}
//

app.Run();
