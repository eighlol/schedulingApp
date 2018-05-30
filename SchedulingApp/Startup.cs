using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Threading.Tasks;
using SchedulingApp.ApiLogic.Repositories;
using SchedulingApp.ApiLogic.Repositories.Interfaces;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Responses;
using SchedulingApp.ApiLogic.Services;
using SchedulingApp.ApiLogic.Services.Interfaces;
using SchedulingApp.Domain.Entities;
using SchedulingApp.Infrastucture.Filters;
using SchedulingApp.Infrastucture.Middleware.Exception;
using SchedulingApp.Infrastucture.Middlewares.Exception;
using SchedulingApp.Infrastucture.Sql;

namespace SchedulingApp
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
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateFilterAttribute));
            });

            services.AddDbContext<SchedulingAppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ConferenceUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 6;
                config.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<SchedulingAppDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Auth/Login");
                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode == 200)
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                        return Task.FromResult<object>(null);
                    }

                    context.Response.Redirect(context.RedirectUri);
                    return Task.FromResult<object>(null);
                };
            });

            services.AddSingleton<ICoordService, CoordService>();
            services.AddTransient<SchedulingAppDbContextSeedData>();
            services.AddScoped<IConferenceRepository, ConferenceRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SchedulingAppDbContextSeedData seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseExceptionMiddleware();
            }
            else
            {
                app.UseExceptionMiddleware();
                app.UseExceptionHandler();
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            Mapper.Initialize(config =>
            {
                config.CreateMap<Event, GetAllEventResponse>().ReverseMap();
                config.CreateMap<Category, CategoryViewModel>().ReverseMap();
                config.CreateMap<Location, LocationViewModel>().ReverseMap();
                config.CreateMap<Member, MemberViewModel>().ReverseMap();
            });

            app.UseMvc(
                routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=App}/{action=Index}/{id?}");
                }
            );

            seeder.EnsureSeedDataAsync().Wait();
        }
    }
}
