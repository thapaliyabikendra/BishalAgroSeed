using BishalAgroSeed.EntityFrameworkCore;
using BishalAgroSeed.Hangfire;
using BishalAgroSeed.MultiTenancy;
using BishalAgroSeed.Options;
using Hangfire;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Timing;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace BishalAgroSeed;

[DependsOn(
    typeof(BishalAgroSeedHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(BishalAgroSeedApplicationModule),
    typeof(BishalAgroSeedEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpBlobStoringFileSystemModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpDistributedLockingModule),
    typeof(AbpBackgroundJobsHangfireModule))]
    public class BishalAgroSeedHttpApiHostModule : AbpModule
{
    private void ConfigureHttpsForwardingBehindProxy(IApplicationBuilder app)
    {
        var forwardedHeaderOptions = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        };
        forwardedHeaderOptions.KnownNetworks.Clear();
        forwardedHeaderOptions.KnownProxies.Clear();

        app.UseForwardedHeaders(forwardedHeaderOptions);
    }

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("BishalAgroSeed");
                options.UseLocalServer();
                options.UseAspNetCore();
            });

            if (!context.Services.GetHostingEnvironment().IsDevelopment())
            {
                builder.AddServer(o =>
                {
                    var uriString = configuration["App:SelfUrl"]; // omit trailing slash here
                    var uri = new Uri(uriString, new UriCreationOptions { DangerousDisablePathAndQueryCanonicalization = true });
                    o.SetIssuer(uri);
                    o.UseAspNetCore().DisableTransportSecurityRequirement();
                });
            }
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        ConfigureAuthentication(context);
        ConfigureBundles();
        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        ConfigureCache(configuration);
        ConfigureVirtualFileSystem(context);
        ConfigureDataProtection(context, configuration, hostingEnvironment);
        ConfigureDistributedLocking(context, configuration);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);
        Configure<AbpClockOptions>(options =>
        {
            options.Kind = DateTimeKind.Utc;
        });
        ConfigureLocalization();
        ConfigureBlobStorage();
        ConfigureHangfire(context, configuration);
        context.Services.Configure<BulkUploadCycleCountOption>(configuration.GetSection("FileUpload:BulkCycleCount"));

        context.Services.Configure<AbpAntiForgeryOptions>(options =>
        {
            options.AutoValidate = false;
        });
    }
    private void ConfigureHangfire( ServiceConfigurationContext context, IConfiguration configuration) 
    {
        context.Services.AddHangfire(options =>
        {
            options.UseSqlServerStorage(configuration.GetConnectionString("Default"));
        });
    }

    private void ConfigureCache(IConfiguration configuration)
    {
        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "BishalAgroSeed:"; });
    }
    private void ConfigureDataProtection(
    ServiceConfigurationContext context,
    IConfiguration configuration,
    IWebHostEnvironment hostingEnvironment)
    {
        var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("BishalAgroSeed");
        if (!hostingEnvironment.IsDevelopment())
        {
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "BishalAgroSeed-Protection-Keys");
        }
    }
    
    private void ConfigureDistributedLocking(
    ServiceConfigurationContext context,
    IConfiguration configuration)
    {
        context.Services.AddSingleton<IDistributedLockProvider>(sp =>
        {
            var connection = ConnectionMultiplexer
                .Connect(configuration["Redis:Configuration"]);
            return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
        });
    }

    private void ConfigureBlobStorage()
    {
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                //container.UseFileSystem(fileSystem => {
                //    fileSystem.BasePath = "C:\\BishalAgroSeed\\Images";
                //});
                container.UseDatabase();
            });
        });
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options => {
            options.Languages.Clear();
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());

            options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<BishalAgroSeedDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}BishalAgroSeed.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<BishalAgroSeedDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}BishalAgroSeed.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<BishalAgroSeedApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}BishalAgroSeed.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<BishalAgroSeedApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}BishalAgroSeed.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(BishalAgroSeedApplicationModule).Assembly);
        });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"],
            new Dictionary<string, string>
            {
                    {"BishalAgroSeed", "BishalAgroSeed API"}
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "BishalAgroSeed API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        var configuration = context.GetConfiguration();
        var pathBase = configuration["App:PathBase"];

        if (!string.IsNullOrWhiteSpace(pathBase))
        {
            app.UsePathBase(pathBase);
        }

        if (configuration.GetValue<bool>("App:ConfigureHttpsForwardingBehindProxy"))
        {
            ConfigureHttpsForwardingBehindProxy(app);
        }

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseAuthorization();


        if (!string.IsNullOrEmpty(pathBase))
        {
            app.UseSwagger(swaggerOptions =>
            {
                swaggerOptions.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    var paths = new OpenApiPaths();
                    foreach (var path in swaggerDoc.Paths)
                    {
                        paths.Add(pathBase + path.Key, path.Value);
                    }
                    swaggerDoc.Paths = paths;
                });
            });
        }
        else
        {
            app.UseSwagger();
        }

        app.UseAbpSwaggerUI(c =>
        {
            c.SwaggerEndpoint(pathBase + "/swagger/v1/swagger.json", "BishalAgroSeed API");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            c.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            c.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
            c.OAuthScopes("BishalAgroSeed");
        });

        app.UseHangfireDashboard();
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();

        RecurringJob.AddOrUpdate<HangfireAppService>("saveInventoryJob", s => s.SaveInventoryAsync(), Cron.Daily);
    }
}
