using BishalAgroSeed.Brands;
using BishalAgroSeed.Categories;
using BishalAgroSeed.CompanyInfos;
using BishalAgroSeed.Configurations;
using BishalAgroSeed.Customers;
using BishalAgroSeed.CycleCountNumbers;
using BishalAgroSeed.CycleCounts;
using BishalAgroSeed.DateTimes;
using BishalAgroSeed.FiscalYears;
using BishalAgroSeed.InventoryCounts;
using BishalAgroSeed.NumberGenerations;
using BishalAgroSeed.OpeningBalances;
using BishalAgroSeed.Products;
using BishalAgroSeed.TransactionDetails;
using BishalAgroSeed.TransactionPayments;
using BishalAgroSeed.Transactions;
using BishalAgroSeed.TranscationTypes;
using BishalAgroSeed.UnitTypes;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace BishalAgroSeed.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class BishalAgroSeedDbContext :
    AbpDbContext<BishalAgroSeedDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<CompanyInfo> CompanyInfos { get; set; }
    public DbSet<DateTime> DateTimes { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<OpeningBalance> OpeningBalances { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<UnitType> UnitTypes { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<TransactionType> TransactionTypes { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionDetail> TransactionDetails { get; set; }
    public DbSet<TransactionPayment> TransactionPayments { get; set; }
    public DbSet<InventoryCount> InventoryCounts { get; set; }
    public DbSet<NumberGeneration> NumberGenerations { get; set; }
    public DbSet<CycleCountNumber> CycleCountNumbers { get; set; }
    public DbSet<CycleCount> CycleCounts { get; set; }
    public DbSet<FiscalYear> FiscalYears { get; set; }

    public BishalAgroSeedDbContext(DbContextOptions<BishalAgroSeedDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(BishalAgroSeedConsts.DbTablePrefix + "YourEntities", BishalAgroSeedConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});

        builder.Entity<BishalAgroSeed.DateTimes.DateTime>(b =>
        {
            b.HasIndex(s => s.Datetime).IsUnique();
        });
    }
}
