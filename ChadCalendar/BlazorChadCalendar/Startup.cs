using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorChadCalendar.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using BlazorChadCalendar.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace BlazorChadCalendar
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChadCalendar");
            Directory.CreateDirectory(path);
            services.AddDbContext<ApplicationContext>(options => options.UseSqlite($"Data Source = { Path.Combine(path, "Calendar.db")}"));

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication();
            services.AddAuthorization();

            services.AddScoped<ILocalStorageService, LocalStorageService>();

            services.AddScoped<CustomAuthStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());

            //services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();


            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<Data.Services.TaskService>();
            services.AddSingleton<Data.Services.EventService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

           
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
