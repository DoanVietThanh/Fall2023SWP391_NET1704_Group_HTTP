using Amazon;
using Amazon.S3;
using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Exceptions;
using DriverLicenseLearningSupport.Mapping;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Repositories;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services;
using DriverLicenseLearningSupport.Services.Impl;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
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
var courseSettings = builder.Configuration.GetSection("CourseSettings");
builder.Services.Configure<CourseSettings>(courseSettings);

// Add VnPay config 
var vnpayConfig = builder.Configuration.GetSection("VnPayConfig");
builder.Services.Configure<VnPayConfig>(vnpayConfig);

// Add Theory Exam config
var theoryExamConfig = builder.Configuration.GetSection("TheoryExamSettings");
builder.Services.Configure<TheoryExamSettings>(theoryExamConfig); 
// Add AppSettings Json config
var appSettingsConfig = builder.Configuration
    .AddJsonFile("appsettings.json")
    .SetBasePath(Directory.GetCurrentDirectory())
    .Build();
builder.Services.Configure<AppSettingsConfig>(appSettingsConfig);


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
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ILicenseTypeService, LicenseTypeService>();
builder.Services.AddScoped<IJobTitleService, JobTitleService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ILicenseRegisterFormService, LicenseRegisterFormService>();
builder.Services.AddScoped<ICurriculumService, CurriculumService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IPaymentTypeService, PaymentTypeService>();
builder.Services.AddScoped<IWeekDayScheduleService, WeekDayScheduleService>();
builder.Services.AddScoped<ISlotService, SlotService>();
builder.Services.AddScoped<ICoursePackageReservationService, CoursePackageReservationService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<ITeachingScheduleService, TeachingScheduleService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<ITheoryExamService, TheoryExamService>();
builder.Services.AddScoped<IExamGradeService, ExamGradeService>();
builder.Services.AddScoped<IExamHistoryService, ExamHistoryService>();
builder.Services.AddScoped<IRollCallBookService, RollCallBookService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IBlogService,BlogService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ISimulationSituationService, SimulationSituationService>();

// Add Repositories
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ILicenseTypeRepository, LicenseTypeRepository>();
builder.Services.AddScoped<IJobTitleRepository, JobTitleRepository>();
builder.Services.AddScoped<ILicenseRegisterFormRepository, LicenseRegisterFormRepository>();
builder.Services.AddScoped<ICurriculumRepository, CurriculumRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IPaymentTypeRepository, PaymentTypeRepository>();
builder.Services.AddScoped<IWeekDayScheduleRepository, WeekDayScheduleRepository>();
builder.Services.AddScoped<ISlotRepository, SlotRepository>();
builder.Services.AddScoped<ICoursePackageReservationRepository, CoursePackageReservationRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<ITeachingScheduleRepository, TeachingScheduleRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<ITheoryExamRepository, TheoryExamRepository>();
builder.Services.AddScoped<IExamGradeRepository, ExamGradeRepository>();
builder.Services.AddScoped<IExamHistoryRepository, ExamHistoryRepostory>();
builder.Services.AddScoped<IRollCallBookRepository, RollCallBookRepository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ICommentRepository,CommentRepository>();
builder.Services.AddScoped<ISimulationSituationRepo,SimulationSituationRepo>();


// Add Mediator
builder.Services.AddMediatR();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

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
builder.Services.AddSingleton<IAmazonS3, AmazonS3Client>();
builder.Services.AddSingleton<IImageService, ImageService>();

// Middleware Exception
//builder.Services.AddTransient<ExceptionMiddleware>();`

var app = builder.Build();
AWSConfigs.AWSRegion = "ap-southeast-1";
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

// Add Middleware Exceptions
//app.ConfigureExceptionMiddleware();

app.MapControllers();

app.Run();