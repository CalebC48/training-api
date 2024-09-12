using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CAP.API.Extensions;
using CAP.API.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Sentry;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace CAP.API
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
            SentrySdk.CaptureMessage("Hello Sentry");
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc();
            services.Configure<IISOptions>(options => { options.AutomaticAuthentication = false; });

            // Uncomment these when you have your auth setup

            #region Policy Configuration

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = AuthenticationSchemeConstants.BYUAuthenticationName;
            //})
            //.AddScheme<AuthenticationSchemeOptions, BYUAuthenticationHandler>
            //        (AuthenticationSchemeConstants.BYUAuthenticationName, op => { });

            //services.AddAuthorization(options =>
            //{
            //    // This is an example authentication scheme where every user only has one role
            //    // Admin have full access
            //    // Users can see data
            //    // Application is the role corresponding to the NextJS application for React projects


            //    options.AddPolicy("Admin", policy =>
            //    {
            //        policy.AddAuthenticationSchemes(AuthenticationSchemeConstants.BYUAuthenticationName);
            //        policy.Requirements.Add(new BYUAuthorizationRequirement(new string[] { "Admin" }));
            //    });

            //    options.AddPolicy("User", policy =>
            //    {
            //        policy.AddAuthenticationSchemes(AuthenticationSchemeConstants.BYUAuthenticationName);
            //        policy.Requirements.Add(new BYUAuthorizationRequirement(new string[] { "Admin", "User" }));
            //    });

            //    options.AddPolicy("Application", policy =>
            //    {
            //        policy.AddAuthenticationSchemes(AuthenticationSchemeConstants.BYUAuthenticationName);
            //        policy.Requirements.Add(new BYUAuthorizationRequirement(new string[] { "Admin", "User", "Application" }));
            //    });
            //});

            #endregion


            // You will need to add your context here
            //services.AddDbContext<CAPContext>(options =>
            //{
            //  var conn = Configuration.GetConnectionString("DefaultConnection");

            //	options.UseMySql(conn, ServerVersion.AutoDetect(conn));
            //});

            // CORS configured to allow any origin to request our API
#if DEBUG
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
#endif

#if !DEBUG
            // CORS configured to only allow one source
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                throw new NotImplementedException();
// Update this to the URL of your application
                builder.WithOrigins("https://sagegrouse.byu.edu/")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
#endif

            services.AddScoped<IAuthorizationHandler, BYUAuthorizationHandler>();
            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGen(options =>
            {
                options.AddEnumsWithValuesFixFilters();
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));


                var scheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "CAS Authorization",
                    Description = "Paste the Client ID from the NextJS Application .env.development here.",
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://api.byu.edu/authorize"),
                            TokenUrl = new Uri("https://api.byu.edu/token")
                        }
                    },
                    Type = SecuritySchemeType.OAuth2
                };

                options.AddSecurityDefinition("OAuth", scheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Id = "OAuth", Type = ReferenceType.SecurityScheme }
                        },
                        new List<string> { }
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.InjectStylesheet("/swagger-ui/SwaggerDark.css");
                    options.OAuthScopes("openid");
                    options.OAuthUsePkce();
                    options.EnablePersistAuthorization();
                });
            }

            app.UseSentryTracing();
            app.UseStaticFiles();
            app.UseGlobalErrorHandler();
            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}