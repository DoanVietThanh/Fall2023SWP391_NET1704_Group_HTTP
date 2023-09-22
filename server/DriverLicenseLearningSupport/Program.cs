using Amazon.S3;
using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Mapping;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Repositories;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services;
using DriverLicenseLearningSupport.Services.impl;
using DriverLicenseLearningSupport.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
var connectionStr = builder.Configuration.GetConnectionString("ConnStr");
builder.Services.AddDbContext<DriverLicenseLearningSupportContext>(options => 
    options.UseSqlServer(connectionStr)
);

// Add AppSettings 
var appSettings = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettings);

// Add Authentication
var secretKey = builder.Configuration.GetValue<string>("AppSettings:SecretKey");
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt => {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    // auto provide token
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    // Sign in token 
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes)
                };                    
            });

// Add Services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ILicenseTypeService, LicenseTypeService>();
builder.Services.AddScoped<IJobTitleService, JobTitleService>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Add Repositories
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ILicenseTypeRepository, LicenseTypeRepository>();
builder.Services.AddScoped<IJobTitleRepository, JobTitleRepository>();


// Add Email Configs
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

// Add AutoMapper
var mapperConfig = new MapperConfiguration(mc => {
    mc.AddProfile(new ApplicationMapper());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddMvc();

// Add Config for required Email
builder.Services.Configure<IdentityOptions>(opts => 
    opts.SignIn.RequireConfirmedEmail = true);
builder.Services.Configure<DataProtectionTokenProviderOptions>(opts =>
    // token valid for next 10 hours
    opts.TokenLifespan = TimeSpan.FromHours(10));


// Add CORS
builder.Services.AddCors(p => p.AddPolicy("Cors", policy =>
{
    policy.WithOrigins("*")
          .AllowAnyHeader()
          .AllowAnyMethod();
}));

// Amazon Lambda Hosting
//builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Amazon S3
//builder.Services.AddSingleton<IAmazonS3, AmazonS3Client>();
//builder.Services.AddSingleton<IImageService, ImageService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Cors");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
