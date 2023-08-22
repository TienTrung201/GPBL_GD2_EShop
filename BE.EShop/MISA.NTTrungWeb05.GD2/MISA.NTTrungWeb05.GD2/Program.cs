using Microsoft.AspNetCore.Mvc;
using MISA.NTTrungWeb05.GD2.Application.Interface.Excel;
using MISA.NTTrungWeb05.GD2.Application.Interface.Service;
using MISA.NTTrungWeb05.GD2.Application.Service;
using MISA.NTTrungWeb05.GD2.Domain;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Repository;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using MISA.NTTrungWeb05.GD2.Infastructurce.Repository;
using MISA.NTTrungWeb05.GD2.Infastructurce.Repository.Excel;
using MISA.NTTrungWeb05.GD2.Infastructurce.Repository.UnitOfWork;
using MISA.NTTrungWeb05.GD2.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Exception validate input
builder.Services.AddControllers().ConfigureApiBehaviorOptions(option =>
{
    option.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values.Reverse().SelectMany(x => x.Errors);
        string errorMessage = string.Join(", ", errors.Select(x => x.ErrorMessage));
        return new BadRequestObjectResult(new BaseException()
        {
            ErrorCode = 400,
            UserMessage = errorMessage,
            DevMessage = errorMessage,
            TraceId = context.HttpContext.TraceIdentifier,
            MoreInfo = "",
            Errors = errors
        });

    };
}).AddJsonOptions(option =>
{
    option.JsonSerializerOptions.PropertyNamingPolicy = null;
    //option.JsonSerializerOptions.Converters.Add(new LocalTimeZoneConverter());
});
// Get ConnectionString
var connectionstring = builder.Configuration.GetConnectionString("MISAEShop");


// Add DI
builder.Services.AddScoped<IUnitOfWork>(provider => new UnitOfWork(connectionstring));
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IItemCategoryRepository, ItemCategoryRepository>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
//
//builder.Services.AddScoped<IEmployeeManager, EmployeeManager>();
//builder.Services.AddScoped<IItemCategoryManager, ItemCategoryManager>();
//builder.Services.AddScoped<IUnitManager, UnitManager>();
//
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IItemCategoryService, ItemCategoryService>();
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddScoped<IInventoryExcelService, InventoryExcelService>();
builder.Services.AddScoped<IUnitExcelService, UnitExcelService>();
builder.Services.AddScoped<IItemCategoryExcelService, ItemCategoryExcelService>();

// Add auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin();
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowAnyHeader();
    });
});

//localization 
var localizationOptions = new RequestLocalizationOptions();
localizationOptions.SetDefaultCulture("en-US");


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//app.UseRequestLocalization(localizationOptions);
// Configure the HTTP request pipeline.
app.UseMiddleware<LocalizationMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionMiddleware>();

app.Run();
