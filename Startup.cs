using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Http;
using RobotJester.Models;
using Stripe;

namespace RobotJester
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddDbContext<StoreContext>(options => options.UseMySql(Configuration["DBInfo:ConnectionString"]));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();  
            services.AddScoped<LoggedInUserService>(); //Grabs specific user data
            
            
            services.AddMvc(options => 
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                //USE THIS LATER FOR REQUIRING HTTPS
                // options.Filters.Add(new RequireHttpsAttribute()); 
                

            });
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            services.AddSession(options => 
            {
                options.Cookie.Name = "LoginCookie";
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => 
                {
                    options.AccessDeniedPath="/Account/ErrorForbidden";
                    options.LoginPath="/";
                });

            services.AddAuthorization(options => 
            {
                options.AddPolicy("AdminPrivledges", p => p.RequireAuthenticatedUser().RequireRole("Admin"));
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStatusCodePagesWithReExecute("/StatusCode/{0}"); //For HTTP errors
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);

            //Security headers
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            //Enforce HTTPS, T.O.F.U, and XXss are disabled until production ready
            // app.UseHsts(options => options.MaxAge(days: 365).IncludeSubdomains());            
            // app.UseXXssProtection(options => options.EnabledWithBlockMode());
            // app.UseXContentTypeOptions();
            
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc();
            
        }
    }
}