﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class DriverLicenseLearningSupportContext : DbContext
    {
        public DriverLicenseLearningSupportContext()
        {
        }

        public DriverLicenseLearningSupportContext(DbContextOptions<DriverLicenseLearningSupportContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseReservation> CourseReservations { get; set; }
        public virtual DbSet<CourseReservationStatus> CourseReservationStatuses { get; set; }
        public virtual DbSet<CourseSchedule> CourseSchedules { get; set; }
        public virtual DbSet<Curriculum> Curricula { get; set; }
        public virtual DbSet<ExamGrade> ExamGrades { get; set; }
        public virtual DbSet<ExamHistory> ExamHistories { get; set; }
        public virtual DbSet<FeedBack> FeedBacks { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<JobTitle> JobTitles { get; set; }
        public virtual DbSet<LicenseRegisterForm> LicenseRegisterForms { get; set; }
        public virtual DbSet<LicenseRegisterFormStatus> LicenseRegisterFormStatuses { get; set; }
        public virtual DbSet<LicenseType> LicenseTypes { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<PracticeExam> PracticeExams { get; set; }
        public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual DbSet<QuestionBank> QuestionBanks { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RollCallBook> RollCallBooks { get; set; }
        public virtual DbSet<StatisticalReport> StatisticalReports { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<VehicleType> VehicleTypes { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=ASUSG513;Initial Catalog=DriverLicenseLearningSupport;uid=sa;password=12345");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("PK__Account__AB6E6165C7635D88");

                entity.ToTable("Account");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_RoleId");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.AddressId)
                    .HasMaxLength(255)
                    .HasColumnName("address_id");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(155)
                    .HasColumnName("city");

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasMaxLength(155)
                    .HasColumnName("district");

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("street");

                entity.Property(e => e.Zipcode)
                    .HasMaxLength(20)
                    .HasColumnName("zipcode");
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("Blog");

                entity.Property(e => e.BlogId).HasColumnName("blog_id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_modified_date");

                entity.Property(e => e.StaffId)
                    .HasMaxLength(255)
                    .HasColumnName("staff_id");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK_Blog_StaffId");

                entity.HasMany(d => d.Tags)
                    .WithMany(p => p.Blogs)
                    .UsingEntity<Dictionary<string, object>>(
                        "BlogTag",
                        l => l.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_BlogTag_TagId"),
                        r => r.HasOne<Blog>().WithMany().HasForeignKey("BlogId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_BlogTag_BlogId"),
                        j =>
                        {
                            j.HasKey("BlogId", "TagId").HasName("PK__Blog_Tag__5D5CC0030A761718");

                            j.ToTable("Blog_Tag");

                            j.IndexerProperty<int>("BlogId").HasColumnName("blog_id");

                            j.IndexerProperty<int>("TagId").HasColumnName("tag_id");
                        });
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.AvatarImage)
                    .HasMaxLength(100)
                    .HasColumnName("avatar_image");

                entity.Property(e => e.BlogId).HasColumnName("blog_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .HasColumnName("content");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .HasColumnName("full_name");

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.BlogId)
                    .HasConstraintName("FK_Comment_BlogId");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.CourseId)
                    .HasMaxLength(255)
                    .HasColumnName("course_id");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.CourseDesc).HasColumnName("course_desc");

                entity.Property(e => e.CourseTitle)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("course_title");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.TotalSession).HasColumnName("total_session");

                entity.HasMany(d => d.Curricula)
                    .WithMany(p => p.Courses)
                    .UsingEntity<Dictionary<string, object>>(
                        "CourseCurriculum",
                        l => l.HasOne<Curriculum>().WithMany().HasForeignKey("CurriculumId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CourseCurriculum_CurriculumnId"),
                        r => r.HasOne<Course>().WithMany().HasForeignKey("CourseId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CourseCurriculum_CourseId"),
                        j =>
                        {
                            j.HasKey("CourseId", "CurriculumId").HasName("PK__Course_C__FE6B74697BF9D46E");

                            j.ToTable("Course_Curriculum");

                            j.IndexerProperty<string>("CourseId").HasMaxLength(255).HasColumnName("course_id");

                            j.IndexerProperty<int>("CurriculumId").HasColumnName("curriculum_id");
                        });
            });

            modelBuilder.Entity<CourseReservation>(entity =>
            {
                entity.ToTable("Course_Reservation");

                entity.Property(e => e.CourseReservationId)
                    .HasMaxLength(255)
                    .HasColumnName("course_reservation_id");

                entity.Property(e => e.CourseId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("course_id");

                entity.Property(e => e.CourseReservationStatusId).HasColumnName("course_reservation_status_id");

                entity.Property(e => e.CourseStartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("course_start_date");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date");

                entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");

                entity.Property(e => e.LastModifiedBy).HasColumnName("last_modified_by");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_modified_date");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("member_id");

                entity.Property(e => e.StaffId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("staff_id");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseReservations)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseReservation_CourseId");

                entity.HasOne(d => d.CourseReservationStatus)
                    .WithMany(p => p.CourseReservations)
                    .HasForeignKey(d => d.CourseReservationStatusId)
                    .HasConstraintName("FK_CourseReservation_StatusId");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.CourseReservations)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK_CourseReservation_InvoiceId");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.CourseReservations)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseReservation_MemberId");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.CourseReservations)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseReservation_StaffId");
            });

            modelBuilder.Entity<CourseReservationStatus>(entity =>
            {
                entity.ToTable("Course_Reservation_Status");

                entity.Property(e => e.CourseReservationStatusId).HasColumnName("course_reservation_status_id");

                entity.Property(e => e.CourseReservationStatusDesc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("course_reservation_status_desc");
            });

            modelBuilder.Entity<CourseSchedule>(entity =>
            {
                entity.ToTable("Course_Schedule");

                entity.Property(e => e.CourseScheduleId).HasColumnName("course_schedule_id");

                entity.Property(e => e.CourseReservationId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("course_reservation_id");

                entity.Property(e => e.StaffId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("staff_id");

                entity.Property(e => e.TeachingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("teaching_date");

                entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

                entity.HasOne(d => d.CourseReservation)
                    .WithMany(p => p.CourseSchedules)
                    .HasForeignKey(d => d.CourseReservationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseSchedule_ReservationId");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.CourseSchedules)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseSchedule_StaffId");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.CourseSchedules)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("FK_CourseSchedule_VehicleId");
            });

            modelBuilder.Entity<Curriculum>(entity =>
            {
                entity.ToTable("Curriculum");

                entity.Property(e => e.CurriculumId).HasColumnName("curriculum_id");

                entity.Property(e => e.CurriculumDesc)
                    .IsRequired()
                    .HasMaxLength(155)
                    .HasColumnName("curriculum_desc");

                entity.Property(e => e.CurriculumDetail).HasColumnName("curriculum_detail");
            });

            modelBuilder.Entity<ExamGrade>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.PracticeExamId })
                    .HasName("PK__Exam_Gra__870D3ACB636CFC4B");

                entity.ToTable("Exam_Grade");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(255)
                    .HasColumnName("member_id");

                entity.Property(e => e.PracticeExamId).HasColumnName("practice_exam_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Point).HasColumnName("point");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.SelectedAnswerId).HasColumnName("selected_answer_id");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.ExamGrades)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamGrade_MemberId");

                entity.HasOne(d => d.PracticeExam)
                    .WithMany(p => p.ExamGrades)
                    .HasForeignKey(d => d.PracticeExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamGrade_PracticeExamId");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ExamGrades)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamGrade_QuestionId");
            });

            modelBuilder.Entity<ExamHistory>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.PracticeExamId })
                    .HasName("PK__Exam_His__870D3ACB6108E8F5");

                entity.ToTable("Exam_History");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(255)
                    .HasColumnName("member_id");

                entity.Property(e => e.PracticeExamId).HasColumnName("practice_exam_id");

                entity.Property(e => e.IsPassed).HasColumnName("is_passed");

                entity.Property(e => e.TotalGrade).HasColumnName("total_grade");

                entity.Property(e => e.TotalQuestion).HasColumnName("total_question");

                entity.Property(e => e.TotalRightAnswer).HasColumnName("total_right_answer");

                entity.Property(e => e.TotalTime).HasColumnName("total_time");

                entity.Property(e => e.WrongParalysisQuestion).HasColumnName("wrong_paralysis_question");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.ExamHistories)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamHistory_MemberId");

                entity.HasOne(d => d.PracticeExam)
                    .WithMany(p => p.ExamHistories)
                    .HasForeignKey(d => d.PracticeExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_History_PracticeExamId");
            });

            modelBuilder.Entity<FeedBack>(entity =>
            {
                entity.ToTable("FeedBack");

                entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .HasColumnName("content");

                entity.Property(e => e.CourseId)
                    .HasMaxLength(255)
                    .HasColumnName("course_id");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(255)
                    .HasColumnName("member_id");

                entity.Property(e => e.RatingStar).HasColumnName("rating_star");

                entity.Property(e => e.StaffId)
                    .HasMaxLength(255)
                    .HasColumnName("staff_id");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.FeedBacks)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_FeedBack_CourseId");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.FeedBacks)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_FeedBack_MemberId");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.FeedBacks)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK_FeedBack_StaffId");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoice");

                entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");

                entity.Property(e => e.Ammount).HasColumnName("ammount");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date");

                entity.Property(e => e.PaymentTypeId).HasColumnName("payment_type_id");

                entity.HasOne(d => d.PaymentType)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.PaymentTypeId)
                    .HasConstraintName("FK_Invoice_PaymentTypeId");
            });

            modelBuilder.Entity<JobTitle>(entity =>
            {
                entity.ToTable("Job_Title");

                entity.Property(e => e.JobTitleId).HasColumnName("job_title_id");

                entity.Property(e => e.JobTitleDesc)
                    .IsRequired()
                    .HasMaxLength(155)
                    .HasColumnName("job_title_desc");
            });

            modelBuilder.Entity<LicenseRegisterForm>(entity =>
            {
                entity.HasKey(e => e.LicenseFormId)
                    .HasName("PK__License___CBEF3B14EB4B032E");

                entity.ToTable("License_Register_Form");

                entity.Property(e => e.LicenseFormId).HasColumnName("license_form_id");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date");

                entity.Property(e => e.HealthCertificationImage)
                    .HasMaxLength(100)
                    .HasColumnName("health_certification_image");

                entity.Property(e => e.IdentityCardImage)
                    .HasMaxLength(100)
                    .HasColumnName("identity_card_image");

                entity.Property(e => e.Image)
                    .HasMaxLength(100)
                    .HasColumnName("image");

                entity.Property(e => e.LicenseFormDesc)
                    .HasMaxLength(255)
                    .HasColumnName("license_form_desc");

                entity.Property(e => e.LicenseTypeId).HasColumnName("license_type_id");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(255)
                    .HasColumnName("member_id");

                entity.Property(e => e.RegisterFormStatusId).HasColumnName("register_form_status_id");

                entity.HasOne(d => d.LicenseType)
                    .WithMany(p => p.LicenseRegisterForms)
                    .HasForeignKey(d => d.LicenseTypeId)
                    .HasConstraintName("FK_LicenseTypeId");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.LicenseRegisterForms)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_MemberId");

                entity.HasOne(d => d.RegisterFormStatus)
                    .WithMany(p => p.LicenseRegisterForms)
                    .HasForeignKey(d => d.RegisterFormStatusId)
                    .HasConstraintName("FK_LicenseRegisterFormId");
            });

            modelBuilder.Entity<LicenseRegisterFormStatus>(entity =>
            {
                entity.HasKey(e => e.RegisterFormStatusId)
                    .HasName("PK__License___BD2B9B64245822C8");

                entity.ToTable("License_Register_Form_Status");

                entity.Property(e => e.RegisterFormStatusId).HasColumnName("register_form_status_id");

                entity.Property(e => e.RegisterFormStatusDesc)
                    .HasMaxLength(155)
                    .HasColumnName("register_form_status_desc");
            });

            modelBuilder.Entity<LicenseType>(entity =>
            {
                entity.ToTable("License_Type");

                entity.Property(e => e.LicenseTypeId).HasColumnName("license_type_id");

                entity.Property(e => e.LicenseTypeDesc)
                    .IsRequired()
                    .HasMaxLength(155)
                    .HasColumnName("license_type_desc");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("Member");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(255)
                    .HasColumnName("member_id");

                entity.Property(e => e.AddressId)
                    .HasMaxLength(255)
                    .HasColumnName("address_id");

                entity.Property(e => e.DateBirth)
                    .HasColumnType("datetime")
                    .HasColumnName("date_birth");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(155)
                    .HasColumnName("first_name");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(155)
                    .HasColumnName("last_name");

                entity.Property(e => e.LicenseTypeId).HasColumnName("license_type_id");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .HasColumnName("phone");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_Member_AddressId");

                entity.HasOne(d => d.EmailNavigation)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.Email)
                    .HasConstraintName("FK_Member_Email");

                entity.HasOne(d => d.LicenseType)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.LicenseTypeId)
                    .HasConstraintName("FK_Member_LicenseTypeId");
            });

            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.ToTable("Payment_Type");

                entity.Property(e => e.PaymentTypeId).HasColumnName("payment_type_id");

                entity.Property(e => e.PaymentTypeDesc)
                    .HasMaxLength(155)
                    .HasColumnName("payment_type_desc");
            });

            modelBuilder.Entity<PracticeExam>(entity =>
            {
                entity.ToTable("Practice_Exam");

                entity.Property(e => e.PracticeExamId).HasColumnName("practice_exam_id");

                entity.Property(e => e.LicenseTypeId).HasColumnName("license_type_id");

                entity.Property(e => e.TotalAnswerRequired).HasColumnName("total_answer_required");

                entity.Property(e => e.TotalQuestion).HasColumnName("total_question");

                entity.Property(e => e.TotalTime).HasColumnName("total_time");

                entity.HasOne(d => d.LicenseType)
                    .WithMany(p => p.PracticeExams)
                    .HasForeignKey(d => d.LicenseTypeId)
                    .HasConstraintName("FK_PracticeExam_LicenseTypeId");
            });

            modelBuilder.Entity<QuestionAnswer>(entity =>
            {
                entity.ToTable("Question_Answer");

                entity.Property(e => e.QuestionAnswerId).HasColumnName("question_answer_id");

                entity.Property(e => e.Answer).HasColumnName("answer");

                entity.Property(e => e.IsTrue).HasColumnName("is_true");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionAnswers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_QuestionAnswer_QuestionId");
            });

            modelBuilder.Entity<QuestionBank>(entity =>
            {
                entity.HasKey(e => e.QuestionId)
                    .HasName("PK__Question__2EC21549596626B6");

                entity.ToTable("Question_Bank");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.Image)
                    .HasMaxLength(100)
                    .HasColumnName("image");

                entity.Property(e => e.IsParalysis).HasColumnName("is_Paralysis");

                entity.Property(e => e.LicenseTypeId).HasColumnName("license_type_id");

                entity.Property(e => e.QuestionAnswerDesc).HasColumnName("question_answer_desc");

                entity.HasOne(d => d.LicenseType)
                    .WithMany(p => p.QuestionBanks)
                    .HasForeignKey(d => d.LicenseTypeId)
                    .HasConstraintName("FK_QuestionBank_LicenseTypeId");

                entity.HasMany(d => d.PracticeExams)
                    .WithMany(p => p.Questions)
                    .UsingEntity<Dictionary<string, object>>(
                        "ExamQuestion",
                        l => l.HasOne<PracticeExam>().WithMany().HasForeignKey("PracticeExamId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ExamQuestion_PracticeExamId"),
                        r => r.HasOne<QuestionBank>().WithMany().HasForeignKey("QuestionId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ExamQuestion_QuestionId"),
                        j =>
                        {
                            j.HasKey("QuestionId", "PracticeExamId").HasName("PK__Exam_Que__1B54AAB65F5D805A");

                            j.ToTable("Exam_Question");

                            j.IndexerProperty<int>("QuestionId").HasColumnName("question_id");

                            j.IndexerProperty<int>("PracticeExamId").HasColumnName("practice_exam_id");
                        });
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(155)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<RollCallBook>(entity =>
            {
                entity.ToTable("Roll_Call_Book");

                entity.Property(e => e.RollCallBookId).HasColumnName("roll_call_book_id");

                entity.Property(e => e.Comment)
                    .HasMaxLength(255)
                    .HasColumnName("comment");

                entity.Property(e => e.CourseScheduleId).HasColumnName("course_schedule_id");

                entity.Property(e => e.IsAbsence).HasColumnName("isAbsence");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("member_id");

                entity.HasOne(d => d.CourseSchedule)
                    .WithMany(p => p.RollCallBooks)
                    .HasForeignKey(d => d.CourseScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RollCallBook_CourseScheduleId");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.RollCallBooks)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RollCallBook_MemberId");
            });

            modelBuilder.Entity<StatisticalReport>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("PK__Statisti__779B7C58578556A3");

                entity.ToTable("Statistical_Report");

                entity.Property(e => e.ReportId).HasColumnName("report_id");

                entity.Property(e => e.TotalActiveMember).HasColumnName("total_active_member");

                entity.Property(e => e.TotalBlog).HasColumnName("total_blog");

                entity.Property(e => e.TotalCourse).HasColumnName("total_course");

                entity.Property(e => e.TotalCourseSchedule).HasColumnName("total_course_schedule");

                entity.Property(e => e.TotalMember).HasColumnName("total_member");

                entity.Property(e => e.TotalMentor).HasColumnName("total_mentor");

                entity.Property(e => e.TotalRevenue).HasColumnName("total_revenue");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.Property(e => e.TagName)
                    .HasMaxLength(155)
                    .HasColumnName("tag_name");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("Vehicle");

                entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

                entity.Property(e => e.RegisterDate)
                    .HasColumnType("datetime")
                    .HasColumnName("register_date");

                entity.Property(e => e.VehicleLicensePlate)
                    .IsRequired()
                    .HasMaxLength(155)
                    .HasColumnName("vehicle_license_plate");

                entity.Property(e => e.VehicleTypeId).HasColumnName("vehicle_type_id");

                entity.HasOne(d => d.VehicleType)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.VehicleTypeId)
                    .HasConstraintName("FK_Vehicle_TypeId");
            });

            modelBuilder.Entity<VehicleType>(entity =>
            {
                entity.ToTable("Vehicle_Type");

                entity.Property(e => e.VehicleTypeId).HasColumnName("vehicle_type_id");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.LicenseTypeId).HasColumnName("license_type_id");

                entity.Property(e => e.VehicleTypeDesc)
                    .HasMaxLength(155)
                    .HasColumnName("vehicle_type_desc");
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.ToTable("Staff");

                entity.Property(e => e.StaffId)
                    .HasMaxLength(255)
                    .HasColumnName("staff_id");

                entity.Property(e => e.AddressId)
                    .HasMaxLength(255)
                    .HasColumnName("address_id");

                entity.Property(e => e.AvatarImage)
                    .HasMaxLength(100)
                    .HasColumnName("avatar_image");

                entity.Property(e => e.DateBirth)
                    .HasColumnType("datetime")
                    .HasColumnName("date_birth");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(155)
                    .HasColumnName("first_name");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.JobTitleId).HasColumnName("job_title_id");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(155)
                    .HasColumnName("last_name");

                entity.Property(e => e.LicenseTypeId).HasColumnName("license_type_id");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("phone");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Staffs)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_Staff_AddressId");

                entity.HasOne(d => d.EmailNavigation)
                    .WithMany(p => p.Staffs)
                    .HasForeignKey(d => d.Email)
                    .HasConstraintName("FK_Staff_Email");

                entity.HasOne(d => d.JobTitle)
                    .WithMany(p => p.Staffs)
                    .HasForeignKey(d => d.JobTitleId)
                    .HasConstraintName("FK_Staff_JobTitleId");

                entity.HasOne(d => d.LicenseType)
                    .WithMany(p => p.Staffs)
                    .HasForeignKey(d => d.LicenseTypeId)
                    .HasConstraintName("FK_Staff_LicenseTypeId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}