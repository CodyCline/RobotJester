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
using RobotJester.Models;

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
            services.AddMvc(options => 
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                //USE THIS LATER FOR REQUIRING HTTPS
                // options.Filters.Add(new RequireHttpsAttribute()); 
                

            });
            services.AddSession(options => 
            {
                options.Cookie.Name = ".Session.cart";
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => 
                {
                    options.AccessDeniedPath="/LoginRegister/ErrorForbidden";
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

            //SECURITY HEADERS
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });           
            //NEED TO ENABLE HTTPS SSL TO USE THESE HEADERS
            //ENFORCE HTTPS (DISABLE T.O.F.U.) COMMENT OUT FOR NOW UNTIL DEPLOYMENT AND PRODUCTION READY

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