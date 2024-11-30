using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WebApplicationTest.Data;
using WebApplicationTest.Models.Identity;
using WebApplicationTest.Models.Repositories;
using WebApplicationTest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
                                                    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<CompanyRepository>();
builder.Services.AddTransient<CompanyService>();
builder.Services.AddTransient<ProductRepository>();
builder.Services.AddTransient<ProductService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<TransactionRepository>();
builder.Services.AddTransient<TransactionService>();
builder.Services.AddTransient<CreditCardRepository>();
builder.Services.AddTransient<CreditCardService>();
builder.Services.AddTransient<ReviewRepository>();
builder.Services.AddTransient<ReviewService>();
builder.Services.AddTransient<CommentRepository>();
builder.Services.AddTransient<CommentService>();
builder.Services.AddTransient<RatingRepository>();
builder.Services.AddTransient<RatingService>();
builder.Services.AddTransient<RefundRepository>();
builder.Services.AddTransient<RefundService>();
builder.Services.AddTransient<CouponRepository>();
builder.Services.AddTransient<CouponService>();
builder.Services.AddTransient<ShoppingCartRepository>();
builder.Services.AddTransient<ShoppingCartService>();
builder.Services.AddTransient<ContactRepository>();
builder.Services.AddTransient<ContactService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.ConfigureApplicationCookie(x => x.LoginPath = "/AppUser/Login");
builder.Services.AddSingleton(u => new BlobServiceClient(
        builder.Configuration.GetValue<string>("BlobConnection")
            ));
builder.Services.AddSingleton<IBlobService, BlobService>();
builder.Services.AddRazorPages()
.AddMvcOptions(options =>
{
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
        _ => "");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
