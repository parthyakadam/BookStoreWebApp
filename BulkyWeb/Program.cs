using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Addding entity core to the project
builder.Services.AddDbContext<ApplicationDbContext>(
	//using sqlServer and retriving the connection string from appsettings.json using builder objects methods

	options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Adding the ICategoryRepository Service as we'll need its object in CategoryController file directly through dependency injection

//first paramter represents which object will be needed for depenedency injection and second parameter represents where its implementation is given as the first parameter is a interface and not a direct class.
//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

//as we've implemented the UnitOfWork design pattern, we'll need the dependency injection of object of DbContext in UnitOfWork file instead of CatergoryRepository, hence we're builing the services for UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
