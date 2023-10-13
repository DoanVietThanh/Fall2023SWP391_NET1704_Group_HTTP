USE MASTER 
GO 
DROP DATABASE IF EXISTS DriverLicenseLearningSupport
GO
CREATE DATABASE DriverLicenseLearningSupport
GO
USE DriverLicenseLearningSupport

GO 
CREATE TABLE [dbo].Role(
	role_id INT PRIMARY KEY identity(1,1),
	name NVARCHAR(155)
)
GO
CREATE TABLE [dbo].Account(
	email NVARCHAR(255) PRIMARY KEY NOT NULL,
	password NVARCHAR(255) NOT NULL,
	role_id INT NOT NULL,
	is_active BIT
)
GO
CREATE TABLE [dbo].Address(
	address_id NVARCHAR(255) PRIMARY KEY,
	street NVARCHAR(255) NOT NULL,
	district NVARCHAR(155) NOT NULL,
	city NVARCHAR(155) NOT NULL,
	zipcode NVARCHAR(20)
)
GO
CREATE TABLE [dbo].Member(
	member_id NVARCHAR(200) PRIMARY KEY,--guid type
	first_name NVARCHAR(155) NOT NULL,
	last_name NVARCHAR(155) NOT NULL,
	date_birth DATETIME,
	phone NVARCHAR(15),
	is_active BIT,
	avatar_image NVARCHAR(100),
	address_id NVARCHAR(255),
	email nvarchar(255),
	license_type_id INT,
	license_form_id INT
)
GO
CREATE TABLE [dbo].License_Type(
	license_type_id INT PRIMARY KEY identity(1,1),
	license_type_desc NVARCHAR(155) NOT NULL

)
GO
CREATE TABLE [dbo].Job_Title(
	job_title_id INT PRIMARY KEY identity(1,1),
	job_title_desc NVARCHAR(155) NOT NULL
)
GO
CREATE TABLE [dbo].Staff(
	staff_id NVARCHAR(200) PRIMARY KEY, 
	first_name NVARCHAR(155) NOT NULL,
	last_name NVARCHAR(155) NOT NULL,
	date_birth DATETIME NOT NULL,
	phone NVARCHAR(15) NOT NULL,
	is_active BIT,
	avatar_image NVARCHAR(100),
	email NVARCHAR(255),
	address_id NVARCHAR(255),
	job_title_id INT,
	license_type_id INT,
	seft_description NVARCHAR(MAX)
)
GO
CREATE TABLE [dbo].Course(
	course_id NVARCHAR(200) PRIMARY KEY,
	course_title NVARCHAR(255) NOT NULL,
	course_desc NVARCHAR(MAX),
	cost FLOAT NOT NULL,
	total_session INT,
	total_month INT,
	start_date DATETIME,
	is_active BIT,
	license_type_id INT
)
GO
CREATE TABLE [dbo].Course_Mentor(
	course_id NVARCHAR(200),
	mentor_id NVARCHAR(200),

	PRIMARY KEY (course_id, mentor_id)
)
GO
CREATE TABLE [dbo].Curriculum(
	curriculum_id INT PRIMARY KEY identity(1,1),
	curriculum_desc NVARCHAR(155) NOT NULL,
	curriculum_detail NVARCHAR(MAX) 
)
GO
CREATE TABLE [dbo].Course_Curriculum(
	course_id NVARCHAR(200),
	curriculum_id INT,

	PRIMARY KEY (course_id, curriculum_id)
)
GO
CREATE TABLE [dbo].Course_Reservation(
	course_reservation_id NVARCHAR(255) PRIMARY KEY,
	course_start_date DATETIME NOT NULL,
	create_date DATETIME,
	last_modified_date DATETIME,
	last_modified_by INT,
	member_id NVARCHAR(200) NOT NULL,
	course_id NVARCHAR(200) NOT NULL,
	staff_id NVARCHAR(200) NOT NULL,
	course_reservation_status_id INT,
	payment_type_id INT NOT NULL,
	payment_ammount FLOAT,
	vehicle_id INT
)
GO
CREATE TABLE [dbo].Course_Reservation_Status(
	course_reservation_status_id INT PRIMARY KEY identity(1,1),
	course_reservation_status_desc NVARCHAR(50) NOT NULL
)
GO
CREATE TABLE [dbo].Payment_Type(
	payment_type_id INT PRIMARY KEY identity(1,1),
	payment_type_desc NVARCHAR(155)
)
GO
CREATE TABLE [dbo].Vehicle_Type(
	vehicle_type_id INT PRIMARY KEY identity(1,1),
	license_type_id INT,
	vehicle_type_desc NVARCHAR(155),
	cost FLOAT
)
GO
CREATE TABLE [dbo].Vehicle(
	vehicle_id INT PRIMARY KEY identity(1,1),
	vehicle_name NVARCHAR(155) NOT NULL,
	vehicle_license_plate NVARCHAR(155) NOT NULL,
	register_date DATETIME,
	vehicle_type_id INT,
	vehicle_image NVARCHAR(155)
)
GO
CREATE TABLE [dbo].Slot(
	slot_id INT PRIMARY KEY identity(1,1),
	slot_name NVARCHAR(100),
	duration INT,
	time TIME(7),
	slot_desc NVARCHAR(155)
)
GO
CREATE TABLE Weekday_Schedule(
	weekday_schedule_id INT PRIMARY KEY identity(1,1),
	monday DATETIME,
	tuesday DATETIME,
	wednesday DATETIME,
	thursday DATETIME,
	friday DATETIME,
	saturday DATETIME,
	sunday DATETIME,
	course_id NVARCHAR(200),
	weekday_schedule_desc NVARCHAR(200)
)
GO
CREATE TABLE [dbo].Teaching_Schedule(
	teaching_schedule_id INT PRIMARY KEY identity(1,1),
	teaching_date DATETIME,
	staff_id NVARCHAR(200),
	slot_id INT,
	vehicle_id INT,
	weekday_schedule_id INT
)
GO
CREATE TABLE [dbo].Roll_Call_Book(
	roll_call_book_id INT PRIMARY KEY identity(1,1),
	isAbsence BIT,
	comment NVARCHAR(255),
	member_id NVARCHAR(200) NOT NULL,
	member_total_session INT,
	teaching_schedule_id INT NOT NULL
)
GO
CREATE TABLE [dbo].FeedBack(
	feedback_id INT PRIMARY KEY identity(1,1),
	content NVARCHAR(255),
	rating_star INT,
	create_date DATETIME,
	member_id NVARCHAR(200),
	staff_id NVARCHAR(200),
	course_id NVARCHAR(200)
)
GO
CREATE TABLE [dbo].License_Register_Form_Status(
	register_form_status_id INT PRIMARY KEY identity(1,1),
	register_form_status_desc NVARCHAR(155)
)
GO
CREATE TABLE [dbo].License_Register_Form(
	license_form_id INT PRIMARY KEY identity(1,1),
	license_form_desc NVARCHAR(255),
	image NVARCHAR(100),
	identity_card_image NVARCHAR(100),
	health_certification_image NVARCHAR(100),
	create_date DATETIME,
	register_form_status_id INT,
	license_type_id INT
)
GO
CREATE TABLE [dbo].Question(
	question_id INT PRIMARY KEY identity(1,1),
	question_answer_desc NVARCHAR(MAX),
	is_Paralysis BIT,
	image NVARCHAR(100),
	license_type_id INT,
	is_active BIT
)
GO
CREATE TABLE [dbo].Question_Answer(
	question_answer_id INT PRIMARY KEY identity(1,1),
	answer NVARCHAR(MAX),
	is_true BIT,
	question_id INT
)
GO
CREATE TABLE [dbo].Theory_Exam(
	theory_exam_id INT PRIMARY KEY identity(1,1),
	total_question INT,
	total_time INT,
	total_answer_required INT,
	license_type_id INT
)
GO
CREATE TABLE [dbo].Exam_Question(
	question_id INT,
	theory_exam_id INT,

	PRIMARY KEY (question_id, theory_exam_id)
)
GO
CREATE TABLE [dbo].Exam_Grade(
	exam_grade_id INT PRIMARY KEY identity(1,1),
	member_id NVARCHAR(200),
	theory_exam_id INT,
	point FLOAT,
	question_id INT NOT NULL,
	selected_answer_id INT NOT NULL,
	email NVARCHAR(255),
	start_date DATETIME
)
GO
CREATE TABLE [dbo].Exam_History(
	exam_history_id INT PRIMARY KEY identity(1,1),
	member_id NVARCHAR(200),
	theory_exam_id INT,
	total_grade INT,
	total_right_answer INT,
	total_question INT,
	total_time INT,
	wrong_paralysis_question BIT,
	is_passed BIT,
	date DATETIME
)
GO
CREATE TABLE [dbo].Blog(
	blog_id INT PRIMARY KEY identity(1,1),
	staff_id NVARCHAR(200),
	content NVARCHAR(MAX),
	create_date DATETIME,
	last_modified_date DATETIME
)
GO
CREATE TABLE [dbo].Tag(
	tag_id INT PRIMARY KEY identity(1,1),
	tag_name NVARCHAR(155)
)
GO
CREATE TABLE [dbo].Blog_Tag(
	blog_id INT,
	tag_id INT,

	PRIMARY KEY (blog_id, tag_id)
)
GO
CREATE TABLE [dbo].Comment(
	comment_id INT PRIMARY KEY identity(1,1),
	content NVARCHAR(255),
	blog_id INT,
	email NVARCHAR(255),
	full_name NVARCHAR(255),
	avatar_image NVARCHAR(100)
)

-- CONSTRAINTS 
-- dbo.Account - dbo.Role
ALTER TABLE Account
ADD CONSTRAINT FK_Account_RoleId FOREIGN KEY (role_id) REFERENCES Role (role_id)
-- dbo.Account - dbo.Member
ALTER TABLE Member
ADD CONSTRAINT FK_Member_Email FOREIGN KEY (email) REFERENCES Account (email)
-- dbo.Member - dbo.Address
ALTER TABLE Member
ADD CONSTRAINT FK_Member_AddressId FOREIGN KEY (address_id) REFERENCES Address (address_id)
-- dbo.Member - dbo.License_Type
ALTER TABLE Member
ADD CONSTRAINT FK_Member_LicenseTypeId FOREIGN KEY (license_type_id) REFERENCES License_Type (license_type_id)
-- dbo.Member - dbo.License_Register_Form
ALTER TABLE	Member
ADD CONSTRAINT FK_Member_LicenseRegisterFormId FOREIGN KEY (license_form_id) REFERENCES License_Register_Form(license_form_id)
-- dbo.Staff - dbo.Job_Title
ALTER TABLE Staff
ADD CONSTRAINT FK_Staff_JobTitleId FOREIGN KEY (job_title_id) REFERENCES Job_Title (job_title_id)
-- dbo.Staff - dbo.Account
ALTER TABLE Staff
ADD CONSTRAINT FK_Staff_Email FOREIGN KEY (email) REFERENCES Account (email)
-- dbo.Staff - dbo.Address
ALTER TABLE Staff
ADD CONSTRAINT FK_Staff_AddressId FOREIGN KEY (address_id) REFERENCES Address (address_id)
-- dbo.Staff - dbo.License_Type
ALTER TABLE Staff
ADD CONSTRAINT FK_Staff_LicenseTypeId FOREIGN KEY (license_type_id) REFERENCES License_Type (license_type_id)
-- dbo.Course_Mentor - dbo.Course
ALTER TABLE Course_Mentor
ADD CONSTRAINT FK_CourseMentor_CourseId FOREIGN KEY (course_id) REFERENCES Course (course_id)
-- dbo.Course_Mentor - dbo.Staff
ALTER TABLE Course_Mentor
ADD CONSTRAINT FK_CourseMentor_MentorId FOREIGN KEY (mentor_id) REFERENCES Staff (staff_id)
-- dbo.Curriculum - dbo.Course_Curriculum
ALTER TABLE Course_Curriculum
ADD CONSTRAINT FK_CourseCurriculum_CurriculumnId FOREIGN KEY (curriculum_id) REFERENCES Curriculum (curriculum_id)
-- dbo.Course - dbo.Course_Curriculum
ALTER TABLE Course_Curriculum
ADD CONSTRAINT FK_CourseCurriculum_CourseId FOREIGN KEY (course_id) REFERENCES Course (course_id)
-- dbo.Course - dbo.LicenseType
ALTER TABLE Course
ADD CONSTRAINT FK_Course_LicenseTypeId FOREIGN KEY (license_type_id) REFERENCES License_Type (license_type_id)
-- dbo.Course_Reservation - dbo.Course
ALTER TABLE Course_Reservation
ADD CONSTRAINT FK_CourseReservation_CourseId FOREIGN KEY (course_id) REFERENCES Course (course_id)
-- dbo.Course_Reservation - dbo.Member
ALTER TABLE Course_Reservation
ADD CONSTRAINT FK_CourseReservation_MemberId FOREIGN KEY (member_id) REFERENCES Member (member_id)
-- dbo.Course_Reservation - dbo.Staff
ALTER TABLE Course_Reservation
ADD CONSTRAINT FK_CourseReservation_StaffId FOREIGN KEY (staff_id) REFERENCES Staff (staff_id)
-- dbo.Course_Reservation - dbo.Course_Reservation_Status
ALTER TABLE Course_Reservation
ADD CONSTRAINT FK_CourseReservation_StatusId FOREIGN KEY (course_reservation_status_id) REFERENCES Course_Reservation_Status (course_reservation_status_id)
-- dbo.Course_Reservation - dbo.Vehicle
ALTER TABLE Course_Reservation
ADD CONSTRAINT FK_CourseReservation_VehicleId FOREIGN KEY (vehicle_id) REFERENCES Vehicle (vehicle_id)
-- dbo.Course_Reservation - dbo.Payment_Type
ALTER TABLE Course_Reservation
ADD CONSTRAINT FK_CourseReservation_PaymentTypeId FOREIGN KEY (payment_type_id) REFERENCES Payment_Type (payment_type_id)
-- dbo.Vehicle - dbo.Vehicle_Type
ALTER TABLE Vehicle
ADD CONSTRAINT FK_Vehicle_TypeId FOREIGN KEY (vehicle_type_id) REFERENCES Vehicle_Type (vehicle_type_id)
-- dbo.Teaching_Schedule - dbo.Staff
ALTER TABLE Teaching_Schedule
ADD CONSTRAINT FK_TeachingSchedule_StaffId FOREIGN KEY (staff_id) REFERENCES Staff (staff_id)
-- dbo.Teaching_Schedule - dbo.Slot
ALTER TABLE Teaching_Schedule
ADD CONSTRAINT FK_TeachingSchedule_SlotId FOREIGN KEY (slot_id) REFERENCES Slot (slot_id)
-- dbo.Teaching_Schedule - dbo.Weekday_Schedule
ALTER TABLE Teaching_Schedule
ADD CONSTRAINT FK_TeachingSchedule_WeekdayScheduleId FOREIGN KEY (weekday_schedule_id) REFERENCES Weekday_Schedule (weekday_schedule_id)
-- dbo.Teaching_Schedule - dbi.Vehicle
ALTER TABLE Teaching_Schedule
ADD CONSTRAINT FK_TeachingSchedule_VehicleId FOREIGN KEY (vehicle_id) REFERENCES Vehicle (vehicle_id)
-- dbo.Weekday_Schedule - dbo.Course
ALTER TABLE Weekday_Schedule
ADD CONSTRAINT FK_WeekdaySchedule_CourseId FOREIGN KEY (course_id) REFERENCES Course (course_id)
-- dbo.Roll_Call_Book - dbo.Teaching_Schedule
ALTER TABLE Roll_Call_Book 
ADD CONSTRAINT FK_RollCallBook_TeachingScheduleId FOREIGN KEY (teaching_schedule_id) REFERENCES Teaching_Schedule (teaching_schedule_id)
-- dbo.Roll_Call_Book - dbo.Member
ALTER TABLE Roll_Call_Book 
ADD CONSTRAINT FK_RollCallBook_MemberId FOREIGN KEY (member_id) REFERENCES Member (member_id)
-- dbo.Feedback - dbo.Member
ALTER TABLE FeedBack
ADD CONSTRAINT FK_FeedBack_MemberId FOREIGN KEY (member_id) REFERENCES Member (member_id)
-- dbo.Feedback - dbo.Course
ALTER TABLE FeedBack
ADD CONSTRAINT FK_FeedBack_CourseId FOREIGN KEY (course_id) REFERENCES Course (course_id)
-- dbo.Feedback - dbo.Staff
ALTER TABLE FeedBack
ADD CONSTRAINT FK_FeedBack_StaffId FOREIGN KEY (staff_id) REFERENCES Staff (staff_id)
-- dbo.License_Register_Form - dbo.License_Register_Form_Status
ALTER TABLE	License_Register_Form
ADD CONSTRAINT FK_LicenseRegisterFormId FOREIGN KEY (register_form_status_id) REFERENCES License_Register_Form_Status(register_form_status_id)
-- dbo.License_Register_Form - dbo.License_Type
ALTER TABLE	License_Register_Form
ADD CONSTRAINT FK_LicenseTypeId FOREIGN KEY (license_type_id) REFERENCES License_Type(license_type_id)
-- dbo.Question - dbo.License_Type
ALTER TABLE Question
ADD CONSTRAINT FK_Question_LicenseTypeId FOREIGN KEY (license_type_id) REFERENCES License_Type (license_type_id)
-- dbo.Question_Answer - dbo.Question
ALTER TABLE Question_Answer 
ADD CONSTRAINT FK_QuestionAnswer_QuestionId FOREIGN KEY (question_id) REFERENCES Question (question_id)
-- dbo.Exam_Question - dbo.Question
ALTER TABLE Exam_Question 
ADD CONSTRAINT FK_ExamQuestion_QuestionId FOREIGN KEY (question_id) REFERENCES Question (question_id)
-- dbo.Exam_Question - dbo.Theory_Exam
ALTER TABLE Exam_Question 
ADD CONSTRAINT FK_ExamQuestion_TheoryExamId FOREIGN KEY (theory_exam_id) REFERENCES Theory_Exam (theory_exam_id)
-- dbo.Theory_Exam - dbo.License_Type
ALTER TABLE Theory_Exam
ADD CONSTRAINT FK_PracticeExam_LicenseTypeId FOREIGN KEY (license_type_id) REFERENCES License_Type (license_type_id)
-- dbo.Exam_Grade - dbo.Member
ALTER TABLE Exam_Grade 
ADD CONSTRAINT FK_ExamGrade_MemberId FOREIGN KEY (member_id) REFERENCES Member (member_id)
-- dbo.Exam_Grade - dbo.Theory_Exam
ALTER TABLE Exam_Grade
ADD CONSTRAINT FK_ExamGrade_TheoryExamId FOREIGN KEY (theory_exam_id) REFERENCES Theory_Exam (theory_exam_id)
-- dbo.Exam_Grade - dbo.Question
ALTER TABLE Exam_Grade
ADD CONSTRAINT FK_ExamGrade_QuestionId FOREIGN KEY (question_id) REFERENCES Question (question_id)
-- dbo.Exam_History - dbo.Member
ALTER TABLE Exam_History
ADD CONSTRAINT FK_ExamHistory_MemberId FOREIGN KEY (member_id) REFERENCES Member (member_id)
-- dbo.Exam_History - dbo.Theory_Exam
ALTER TABLE Exam_History
ADD CONSTRAINT FK_History_TheoryExamId FOREIGN KEY (theory_exam_id) REFERENCES Theory_Exam (theory_exam_id)
-- dbo.Blog - dbo.Staff
ALTER TABLE Blog
ADD CONSTRAINT FK_Blog_StaffId FOREIGN KEY (staff_id) REFERENCES Staff (staff_id)
-- dbo.Comment - dbo.Blog
ALTER TABLE Comment
ADD CONSTRAINT FK_Comment_BlogId FOREIGN KEY (blog_id) REFERENCES Blog (blog_id)
-- dbo.Blog_Tag - dbo.Blog
ALTER TABLE Blog_Tag
ADD CONSTRAINT FK_BlogTag_BlogId FOREIGN KEY (blog_id) REFERENCES Blog (blog_id)
-- dbo.Blog_Tag - dbo.Tag
ALTER TABLE Blog_Tag
ADD CONSTRAINT FK_BlogTag_TagId FOREIGN KEY (tag_id) REFERENCES Tag (tag_id)


-- default data
INSERT INTO [dbo].Role(name)
VALUES (N'Admin'),(N'Staff'),(N'Mentor'),(N'Member')
INSERT INTO [dbo].Account(email, password, role_id, is_active)
VALUES (N'admin@gmail.com', N'QEFkbWluMTIz', 1, 1)
INSERT INTO [dbo].Job_Title(job_title_desc)
VALUES (N'Quản trị hệ thống'), (N'Quản lí nhân sự'), (N'Giảng viên')
INSERT INTO [dbo].License_Type(license_type_desc)
VALUES (N'A1'),(N'A2'),(N'B1'),(N'B1.1'),(N'B2')
INSERT INTO [dbo].License_Register_Form_Status(register_form_status_desc)
VALUES (N'Chưa duyệt'), (N'Đã duyệt')
INSERT INTO [dbo].Course_Reservation_Status(course_reservation_status_desc)
VALUES (N'Chưa thanh toán'), (N'Đã thanh toán'), (N'Đã kết thúc')
INSERT INTO [dbo].Payment_Type(payment_type_desc)
VALUES (N'Thanh toán trực tiếp'), (N'Credit Card'), (N'VNPAY')
INSERT INTO [dbo].Vehicle_Type(vehicle_type_desc, license_type_id)
VALUES (N'Xe số sàn', 3), (N'Xe số tự động', 4)

