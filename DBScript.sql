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
	self_description NVARCHAR(MAX)
)
GO
CREATE TABLE [dbo].Course(
	course_id NVARCHAR(200) PRIMARY KEY,
	course_title NVARCHAR(255) NOT NULL,
	course_desc NVARCHAR(MAX),
	total_month INT,
	start_date DATETIME,
	is_active BIT,
	license_type_id INT,
	total_hours_required INT,
	total_km_required INT
)
GO
CREATE TABLE [dbo].Course_Package(
	course_package_id NVARCHAR(200) PRIMARY KEY,
	course_package_desc NVARCHAR(255),
	total_session INT,
	session_hour INT,
	cost FLOAT,
	age_required INT,
	course_id NVARCHAR(200)
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
CREATE TABLE [dbo].Course_Package_Reservation(
	course_package_reservation_id NVARCHAR(200) PRIMARY KEY,
	create_date DATETIME,
	member_id NVARCHAR(200) NOT NULL,
	course_package_id NVARCHAR(200) NOT NULL,
	staff_id NVARCHAR(200) NOT NULL,
	reservation_status_id INT,
	payment_type_id INT NOT NULL,
	payment_ammount FLOAT
)
GO
CREATE TABLE [dbo].Reservation_Status(
	reservation_status_id INT PRIMARY KEY identity(1,1),
	reservation_status_desc NVARCHAR(50) NOT NULL
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
	vehicle_image NVARCHAR(155),
	is_active BIT NOT NULL
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
	weekday_schedule_id INT,
	course_package_id NVARCHAR(200),
	is_active BIT
)
GO
CREATE TABLE [dbo].Roll_Call_Book(
	roll_call_book_id INT PRIMARY KEY identity(1,1),
	isAbsence BIT,
	comment NVARCHAR(255),
	member_id NVARCHAR(200) NOT NULL,
	member_total_session INT,
	teaching_schedule_id INT NOT NULL,
	total_hours_driven INT,
	total_km_driven INT,
	is_active BIT,
	cancel_message NVARCHAR(200)
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
	gender NVARCHAR(10),
	permanent_address NVARCHAR(200),
	identity_number NVARCHAR(15),
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
	license_type_id INT,
	start_date DATETIME,
	start_time TIME,
	is_mock_exam BIT
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
GO
CREATE TABLE [dbo].Simulation_Situation(
	simulation_id INT PRIMARY KEY identity(1,1),
	simulation_video NVARCHAR(200),
	image_result NVARCHAR(200),
	time_result INT,
	license_type_id INT
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
-- dbo.Course_Package - dbo.Course
ALTER TABLE Course_Package
ADD CONSTRAINT FK_CoursePackage_CourseId FOREIGN KEY (course_id) REFERENCES Course (course_id)
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
-- dbo.Course_Package_Reservation - dbo.Course
ALTER TABLE Course_Package_Reservation
ADD CONSTRAINT FK_CoursePackageReservation_CoursePackageId FOREIGN KEY (course_package_id) REFERENCES Course_Package (course_package_id)
-- dbo.Course_Package_Reservation - dbo.Member
ALTER TABLE Course_Package_Reservation
ADD CONSTRAINT FK_CoursePackageReservation_MemberId FOREIGN KEY (member_id) REFERENCES Member (member_id)
-- dbo.Course__Package_Reservation - dbo.Staff
ALTER TABLE Course_Package_Reservation
ADD CONSTRAINT FK_CoursePackageReservation_StaffId FOREIGN KEY (staff_id) REFERENCES Staff (staff_id)
-- dbo.Course_Package_Reservation - dbo.Reservation_Status
ALTER TABLE Course_Package_Reservation
ADD CONSTRAINT FK_CoursePackageReservation_StatusId FOREIGN KEY (reservation_status_id) REFERENCES Reservation_Status (reservation_status_id)
-- dbo.Course_Package_Reservation - dbo.Payment_Type
ALTER TABLE Course_Package_Reservation
ADD CONSTRAINT FK_CoursePackageReservation_PaymentTypeId FOREIGN KEY (payment_type_id) REFERENCES Payment_Type (payment_type_id)
-- dbo.Vehicle - dbo.Vehicle_Type
ALTER TABLE Vehicle
ADD CONSTRAINT FK_Vehicle_TypeId FOREIGN KEY (vehicle_type_id) REFERENCES Vehicle_Type (vehicle_type_id)
-- dbo.Vehicle - dbo.Vehicle_Type
ALTER TABLE Vehicle_Type
ADD CONSTRAINT FK_VehicleType_LicenseTypeId FOREIGN KEY (license_type_id) REFERENCES License_Type(license_type_id)
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
ALTER TABLE Teaching_Schedule
ADD CONSTRAINT FK_TeachingSchedule_CoursePackageId FOREIGN KEY (course_package_id) REFERENCES Course_Package(course_package_id)
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
-- dbo.Simulation_Situation - dbo.License_Type
ALTER TABLE Simulation_Situation
ADD CONSTRAINT FK_SimulationSituation_LicenseTypeId FOREIGN KEY (license_type_id) REFERENCES License_Type (license_type_id)

-- default data
INSERT INTO [dbo].Role(name)
VALUES (N'Admin'),(N'Staff'),(N'Mentor'),(N'Member')
INSERT INTO [dbo].Account(email, password, role_id, is_active)
VALUES (N'admin@gmail.com', N'QEFkbWluMTIz', 1, 1)
INSERT INTO [dbo].Job_Title(job_title_desc)
VALUES (N'Quản trị hệ thống'), (N'Quản lí nhân sự'), (N'Người hướng dẫn')
INSERT INTO [dbo].License_Type(license_type_desc)
VALUES (N'A1'),(N'A2'),(N'B1'),(N'B1.1'),(N'B2')
INSERT INTO [dbo].License_Register_Form_Status(register_form_status_desc)
VALUES (N'Chưa duyệt'), (N'Đã duyệt'), (N'Đã hủy')
INSERT INTO [dbo].Reservation_Status(reservation_status_desc)
VALUES (N'Chưa thanh toán'), (N'Đã thanh toán')
INSERT INTO [dbo].Payment_Type(payment_type_desc)
VALUES (N'Thanh toán trực tiếp'), (N'Credit Card'), (N'VNPAY')
INSERT INTO [dbo].Vehicle_Type(vehicle_type_desc, license_type_id)
VALUES (N'Xe số sàn', 3), (N'Xe số tự động', 4)
INSERT INTO [dbo].Vehicle(vehicle_name, register_date, vehicle_image, vehicle_license_plate, is_active, vehicle_type_id)
VALUES 
(N'Toyota Innova', CAST(('2019/02/04') AS DATE),
 N'04d6fe8c-6347-415e-9cb5-ad837de30f12',
 N'51F-922.87',
 1, 2),
 (N'Mistubishi', CAST(('2017/09/21') AS DATE),
 N'11f98cbb-7103-4880-8b52-18a4bc67057c',
 N'51F-225.94',
 1, 2),
 (N'KIA K3', CAST(('2022/07/09') AS DATE),
 N'7d9d38f9-4172-460c-8291-c362c96688f1',
 N'51F-881.91',
 1, 2),
 (N'Suzuki', CAST(('2022/04/22') AS DATE),
 N'a6b9291e-759b-4dd6-a2e9-e11113464ba3',
 N'51F-114.72',
 1, 2)
INSERT INTO [dbo].Slot(slot_name, slot_desc, duration, time)
VALUES (N'Slot 1', N'7:30 - 9:30', 2, CAST(('07:30') AS TIME)),
(N'Slot 2', N'9:45 - 11:45', 2, CAST(('09:45') AS TIME)),
(N'Slot 3', N'13:30 - 15:30', 2, CAST(('13:30') AS TIME)),
(N'Slot 4', N'15:45 - 17:45', 2, CAST(('15:45') AS TIME))
GO
-- Courses
INSERT INTO [dbo].Course(course_id, course_desc, course_title, start_date, total_hours_required, total_km_required, total_month, license_type_id, is_active)
VALUES 
(N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
 N'waiting',
 N'Khóa học bằng lái B1',
 CAST(N'2023/10/30' AS DATE),
 14,714,3,3,1)
INSERT INTO [dbo].Weekday_Schedule(monday, tuesday, wednesday, thursday, friday, saturday, sunday, course_id, weekday_schedule_desc)
VALUES 
(
CAST('2023-10-30 00:00:00.000' AS DATE),
CAST('2023-10-31 00:00:00.000' AS DATE),
CAST('2023-11-01 00:00:00.000' AS DATE),
CAST('2023-11-02 00:00:00.000' AS DATE),
CAST('2023-11-03 00:00:00.000' AS DATE),
CAST('2023-11-04 00:00:00.000' AS DATE),
CAST('2023-11-05 00:00:00.000' AS DATE),
N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
N'30/10 To 05/11'
),
(
CAST('2023-11-06 00:00:00.000' AS DATE),
CAST('2023-11-07 00:00:00.000' AS DATE),
CAST('2023-11-08 00:00:00.000' AS DATE),
CAST('2023-11-09 00:00:00.000' AS DATE),
CAST('2023-11-10 00:00:00.000' AS DATE),
CAST('2023-11-11 00:00:00.000' AS DATE),
CAST('2023-11-12 00:00:00.000' AS DATE),
N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
N'06/11 To 12/11'
),
(
CAST('2023-11-13 00:00:00.000' AS DATE),
CAST('2023-11-14 00:00:00.000' AS DATE),
CAST('2023-11-15 00:00:00.000' AS DATE),
CAST('2023-11-16 00:00:00.000' AS DATE),
CAST('2023-11-17 00:00:00.000' AS DATE),
CAST('2023-11-18 00:00:00.000' AS DATE),
CAST('2023-11-19 00:00:00.000' AS DATE),
N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
N'13/11 To 19/11'
),
(
CAST('2023-11-20 00:00:00.000' AS DATE),
CAST('2023-11-21 00:00:00.000' AS DATE),
CAST('2023-11-22 00:00:00.000' AS DATE),
CAST('2023-11-23 00:00:00.000' AS DATE),
CAST('2023-11-24 00:00:00.000' AS DATE),
CAST('2023-11-25 00:00:00.000' AS DATE),
CAST('2023-11-26 00:00:00.000' AS DATE),
N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
N'20/11 To 26/11'
),
(
CAST('2023-11-27 00:00:00.000' AS DATE),
CAST('2023-11-28 00:00:00.000' AS DATE),
CAST('2023-11-29 00:00:00.000' AS DATE),
CAST('2023-11-30 00:00:00.000' AS DATE),
CAST('2023-12-01 00:00:00.000' AS DATE),
CAST('2023-12-02 00:00:00.000' AS DATE),
CAST('2023-12-03 00:00:00.000' AS DATE),
N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
N'27/11 To 03/12'
),
(
CAST('2023-12-04 00:00:00.000' AS DATE),
CAST('2023-12-05 00:00:00.000' AS DATE),
CAST('2023-12-06 00:00:00.000' AS DATE),
CAST('2023-12-07 00:00:00.000' AS DATE),
CAST('2023-12-08 00:00:00.000' AS DATE),
CAST('2023-12-09 00:00:00.000' AS DATE),
CAST('2023-12-10 00:00:00.000' AS DATE),
N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
N'04/12 To 10/12'
),
(
CAST('2023-12-11 00:00:00.000' AS DATE),
CAST('2023-12-12 00:00:00.000' AS DATE),
CAST('2023-12-13 00:00:00.000' AS DATE),
CAST('2023-12-14 00:00:00.000' AS DATE),
CAST('2023-12-15 00:00:00.000' AS DATE),
CAST('2023-12-16 00:00:00.000' AS DATE),
CAST('2023-12-17 00:00:00.000' AS DATE),
N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
N'11/12 To 17/12'
),
(
CAST('2023-12-18 00:00:00.000' AS DATE),
CAST('2023-12-19 00:00:00.000' AS DATE),
CAST('2023-12-20 00:00:00.000' AS DATE),
CAST('2023-12-21 00:00:00.000' AS DATE),
CAST('2023-12-22 00:00:00.000' AS DATE),
CAST('2023-12-23 00:00:00.000' AS DATE),
CAST('2023-12-24 00:00:00.000' AS DATE),
N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
N'18/12 To 24/12'
),
(
CAST('2023-12-25 00:00:00.000' AS DATE),
CAST('2023-12-26 00:00:00.000' AS DATE),
CAST('2023-12-27 00:00:00.000' AS DATE),
CAST('2023-12-28 00:00:00.000' AS DATE),
CAST('2023-12-29 00:00:00.000' AS DATE),
CAST('2023-12-30 00:00:00.000' AS DATE),
CAST('2023-12-31 00:00:00.000' AS DATE),
N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
N'25/12 To 31/12'
),
(
CAST('2023-01-01 00:00:00.000' AS DATE),
CAST('2023-01-02 00:00:00.000' AS DATE),
CAST('2023-01-03 00:00:00.000' AS DATE),
CAST('2023-01-04 00:00:00.000' AS DATE),
CAST('2023-01-05 00:00:00.000' AS DATE),
CAST('2023-01-06 00:00:00.000' AS DATE),
CAST('2023-01-07 00:00:00.000' AS DATE),
N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
N'01/01 To 07/01'
),
(
CAST('2023-01-08 00:00:00.000' AS DATE),
CAST('2023-01-09 00:00:00.000' AS DATE),
CAST('2023-01-10 00:00:00.000' AS DATE),
CAST('2023-01-11 00:00:00.000' AS DATE),
CAST('2023-01-12 00:00:00.000' AS DATE),
CAST('2023-01-13 00:00:00.000' AS DATE),
CAST('2023-01-14 00:00:00.000' AS DATE),
N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
N'08/01 To 14/01'
),
(
CAST('2023-01-15 00:00:00.000' AS DATE),
CAST('2023-01-16 00:00:00.000' AS DATE),
CAST('2023-01-17 00:00:00.000' AS DATE),
CAST('2023-01-18 00:00:00.000' AS DATE),
CAST('2023-01-19 00:00:00.000' AS DATE),
CAST('2023-01-20 00:00:00.000' AS DATE),
CAST('2023-01-21 00:00:00.000' AS DATE),
N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
N'15/01 To 21/01'
),
(
CAST('2023-01-22 00:00:00.000' AS DATE),
CAST('2023-01-23 00:00:00.000' AS DATE),
CAST('2023-01-24 00:00:00.000' AS DATE),
CAST('2023-01-25 00:00:00.000' AS DATE),
CAST('2023-01-26 00:00:00.000' AS DATE),
CAST('2023-01-27 00:00:00.000' AS DATE),
CAST('2023-01-28 00:00:00.000' AS DATE),
N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
N'22/01 To 28/01'
),
(
CAST('2023-01-29 00:00:00.000' AS DATE),
CAST('2023-01-30 00:00:00.000' AS DATE),
CAST('2023-01-31 00:00:00.000' AS DATE),
CAST('2023-01-01 00:00:00.000' AS DATE),
CAST('2023-01-02 00:00:00.000' AS DATE),
CAST('2023-01-03 00:00:00.000' AS DATE),
CAST('2023-01-04 00:00:00.000' AS DATE),
N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
N'29/01 To 04/01'
)
INSERT INTO [dbo].Course_Package(course_package_id, course_package_desc, course_id, cost, age_required,session_hour,total_session)
VALUES
(N'069af18a-54b3-4de7-bac2-27d778431cc3',
 N'Học bằng lái B1 cơ bản',
 N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
 14500000,18,2,6)
INSERT INTO [dbo].Course_Package(course_package_id, course_package_desc, course_id, cost, age_required)
VALUES
(N'825e8a00-ae5a-404e-994b-ebef5028e1ab',
 N'Học bằng lái B1 bổ túc',
 N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e',
 4200000,18)
GO
-- Curriculum
INSERT INTO [dbo].Curriculum(curriculum_desc, curriculum_detail)
VALUES
(N'XUẤT PHÁT', N'Yêu cầu của bài này: khi xuất phát phải bật đèn xi – nhan trái (với ý nghĩa là xe chuẩn bị đi ra làn đường bên ngoài).'),
(N'DỪNG XE,NHƯỜNG ĐƯỜNG CHO NGƯỜI ĐI BỘ', N'Yêu cầu của bài này là dừng xe đúng chỗ trước vạch trắng và đường vằn dành cho người đi bộ'),
(N'DỪNG VÀ KHỞI HÀNH XE NGANG DỐC', N'Yêu cầu của bài này là xe không vượt quá vạch quy định, không bị tuột dốc quá 50 cm, phải vượt khỏi dốc trong khoảng thời gian 30 giây.'),
(N'VỆT BÁNH XE VÀ ĐƯỜNG HẸP VUÔNG GÓC', N'Yêu cầu của bài này là hai bánh xe bên phải phải đi lọt qua một đoạn đường có bề rộng khoảng 30-35 cm; Cho xe đi không bị chạm vạch ở gần vỉa hè hai bên đường.'),
(N'QUA NGÃ TƯ', N'Cũng giống như ở ngoài đường, tại ngã tư này có đèn tín hiệu và bạn chỉ được cho xe qua ngã tư khi có đèn xanh'),
(N'ĐƯỜNG VÒNG QUANH CO', N'Yêu cầu của bài này là khi cho xe đi hình chữ S không bị chạm vạch ở gần vỉa hè hai bên đường.'),
(N'GHÉP XE DỌC VÀO NƠI ĐỖ', N'Yêu cầu của bài này là trong vòng 2 phút bạn phải cho xe lùi được vào nơi đỗ (chuồng), không chạm vạch và tiến ra khỏi chuồng.'),
(N'TẠM DỪNG Ở CHỖ CÓ ĐƯỜNG SẮT CHẠY QUA', N'Yêu cầu của bài này là dừng xe đúng chỗ trước vạch trắng và đường vằn dành cho người đi bộ trước đường sắt.'),
(N'THAY ĐỔI SỐ TRÊN ĐƯỜNG BẰNG', N'Yêu cầu của bài này là phải lên được số 2 và đạt tốc độ trên 20 km/h trước biển báo 20 màu xanh; Về số 1 và giảm tốc độ xuống dưới 20 km/h trước biển báo 20 màu trắng.'),
(N'GHÉP XE NGANG VÀO NƠI ĐỖ', N'Yêu cầu của bài này là lùi xe vào nơi cần đỗ khi 2 đầu và 1 mặt bên đều bị khóa bởi vật cản hoặc với những xe khác'),
(N'KẾT THÚC', N'Để kết thúc bài sát hạch, bạn bật đèn xi-nhan phải khi lái xe qua vạch kết thúc. Bạn không bật đèn xi – nhan sẽ bị trừ 5 điểm.')
GO
-- Courses - Curriculum
INSERT INTO [dbo].Course_Curriculum(course_id, curriculum_id)
VALUES
(N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e', 1),
(N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e', 2),
(N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e', 3),
(N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e', 4),
(N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e', 5),
(N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e', 7),
(N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e', 8),
(N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e', 9),
(N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e', 10),
(N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e', 11)
GO
-- Mentors Account
INSERT INTO [dbo].Account(email, password, role_id, is_active)
VALUES (N'mentor@gmail.com', N'QE1lbnRvcjEyMw==', 3, 1), --@Mentor123
(N'dinhxuanquy@gmail.com', N'QE1lbnRvcjEyMw==', 3, 1),
(N'daothanhminh@gmail.com', N'QE1lbnRvcjEyMw==', 3, 1),
(N'quocvy@gmail.com', N'QE1lbnRvcjEyMw==', 3, 1)
INSERT INTO [dbo].Address(address_id, street, district, city)
VALUES 
(N'3556e356-346e-4983-aac2-9aba16b6156e',
 N'Phan Văn Long', N'Quận 2', N'TP.Hồ Chí Minh'),
 (N'4740c596-6e0f-4ec9-a268-25fc9f2cb09f',
 N'Điện Biên Phủ', N'Bình Thạnh', N'TP.Hồ Chí Minh'),
 (N'd353b40e-ca0f-4ac7-a4a2-395cc02d00ad',
 N'Cách Mạng Tháng 8', N'Quận 10', N'TP.Hồ Chí Minh'),
 (N'aa70b171-43ca-4dfc-af9b-b9e243ba6bcf',
 N'Nguyễn Thái Sơn', N'Gò Vấp', N'TP.Hồ Chí Minh')
GO
INSERT INTO [dbo].Staff(staff_id, first_name, last_name, avatar_image,
date_birth, email, phone, address_id, self_description,
job_title_id, is_active)
VALUES
(N'c5708f3c-daf1-444e-bed7-240c2ed6ad50',
 N'Mentor',N'',
 N'https://img.freepik.com/premium-vector/man-avatar-profiel-op-ronde-pictogram_24640-14044.jpg?w=2000',
 CAST(N'1995/02/10' AS DATE),
 N'mentor@gmail.com',
 N'0777155780',
 N'3556e356-346e-4983-aac2-9aba16b6156e',
 N'<p>Giới thiệu bản th&acirc;n l&agrave; việc tr&igrave;nh b&agrave;y th&ocirc;ng tin cơ bản về bản th&acirc;n m&igrave;nh cho người kh&aacute;c để họ hiểu th&ecirc;m về ch&uacute;ng ta, đến từ đ&acirc;u v&agrave; một số th&ocirc;ng tin quan trọng về cuộc sống v&agrave; sở th&iacute;ch của c&aacute; nh&acirc;n. Mục đ&iacute;ch ch&iacute;nh của việc giới thiệu bản th&acirc;n l&agrave; tạo ra một sự kết nối ban đầu, thiết lập mối quan hệ hoặc bắt đầu một cuộc tr&ograve; chuyện. Th&ocirc;ng qua việc giới thiệu, người kh&aacute;c c&oacute; cơ hội biết về bản th&acirc;n m&igrave;nh, c&oacute; thể tạo ra một cơ sở cho giao tiếp v&agrave; tương t&aacute;c tiếp theo.</p>
 <p>Th&ocirc;ng tin trong việc giới thiệu bản th&acirc;n thường bao gồm t&ecirc;n, tuổi, nơi sống, nghề nghiệp hoặc tr&igrave;nh độ học vấn v&agrave; những sở th&iacute;ch hoặc đặc điểm c&aacute; nh&acirc;n m&agrave; m&igrave;nh muốn người kh&aacute;c biết. Việc n&agrave;y c&oacute; thể diễn ra trong nhiều t&igrave;nh huống, chẳng hạn như trong cuộc phỏng vấn việc l&agrave;m, trong một tập trung x&atilde; hội, hoặc khi ch&uacute;ng ta mới quen biết ai đ&oacute;.</p>
 <p>Để giới thiệu bản th&acirc;n hiệu quả cần phải truyền đạt th&ocirc;ng tin một c&aacute;ch r&otilde; r&agrave;ng v&agrave; tự tin, tạo ấn tượng t&iacute;ch cực v&agrave; tạo điều kiện tốt cho mối quan hệ v&agrave; giao tiếp tiếp theo.</p>',
 3,1),
 (N'9f2db427-5a6f-4313-98f8-35287820f903',
 N'Đinh Xuân',N'Quý',
 N'https://img.freepik.com/premium-vector/man-avatar-profiel-op-ronde-pictogram_24640-14046.jpg?w=2000',
 CAST(N'2000/09/08' AS DATE),
 N'dinhxuanquy@gmail.com',
 N'0982816456',
 N'4740c596-6e0f-4ec9-a268-25fc9f2cb09f',
 N'<p>Giới thiệu bản th&acirc;n l&agrave; việc tr&igrave;nh b&agrave;y th&ocirc;ng tin cơ bản về bản th&acirc;n m&igrave;nh cho người kh&aacute;c để họ hiểu th&ecirc;m về ch&uacute;ng ta, đến từ đ&acirc;u v&agrave; một số th&ocirc;ng tin quan trọng về cuộc sống v&agrave; sở th&iacute;ch của c&aacute; nh&acirc;n. Mục đ&iacute;ch ch&iacute;nh của việc giới thiệu bản th&acirc;n l&agrave; tạo ra một sự kết nối ban đầu, thiết lập mối quan hệ hoặc bắt đầu một cuộc tr&ograve; chuyện. Th&ocirc;ng qua việc giới thiệu, người kh&aacute;c c&oacute; cơ hội biết về bản th&acirc;n m&igrave;nh, c&oacute; thể tạo ra một cơ sở cho giao tiếp v&agrave; tương t&aacute;c tiếp theo.</p>
 <p>Th&ocirc;ng tin trong việc giới thiệu bản th&acirc;n thường bao gồm t&ecirc;n, tuổi, nơi sống, nghề nghiệp hoặc tr&igrave;nh độ học vấn v&agrave; những sở th&iacute;ch hoặc đặc điểm c&aacute; nh&acirc;n m&agrave; m&igrave;nh muốn người kh&aacute;c biết. Việc n&agrave;y c&oacute; thể diễn ra trong nhiều t&igrave;nh huống, chẳng hạn như trong cuộc phỏng vấn việc l&agrave;m, trong một tập trung x&atilde; hội, hoặc khi ch&uacute;ng ta mới quen biết ai đ&oacute;.</p>
 <p>Để giới thiệu bản th&acirc;n hiệu quả cần phải truyền đạt th&ocirc;ng tin một c&aacute;ch r&otilde; r&agrave;ng v&agrave; tự tin, tạo ấn tượng t&iacute;ch cực v&agrave; tạo điều kiện tốt cho mối quan hệ v&agrave; giao tiếp tiếp theo.</p>',
 3,1),
 (N'4af99375-1b6e-40c1-83cb-a291574d31ed',
 N'Đào Thanh',N'Minh',
 N'https://cdn.iconscout.com/icon/free/png-256/free-avatar-370-456322.png?f=webp',
 CAST(N'1985/03/01' AS DATE),
 N'daothanhminh@gmail.com',
 N'0982816456',
 N'd353b40e-ca0f-4ac7-a4a2-395cc02d00ad',
 N'<p>Giới thiệu bản th&acirc;n l&agrave; việc tr&igrave;nh b&agrave;y th&ocirc;ng tin cơ bản về bản th&acirc;n m&igrave;nh cho người kh&aacute;c để họ hiểu th&ecirc;m về ch&uacute;ng ta, đến từ đ&acirc;u v&agrave; một số th&ocirc;ng tin quan trọng về cuộc sống v&agrave; sở th&iacute;ch của c&aacute; nh&acirc;n. Mục đ&iacute;ch ch&iacute;nh của việc giới thiệu bản th&acirc;n l&agrave; tạo ra một sự kết nối ban đầu, thiết lập mối quan hệ hoặc bắt đầu một cuộc tr&ograve; chuyện. Th&ocirc;ng qua việc giới thiệu, người kh&aacute;c c&oacute; cơ hội biết về bản th&acirc;n m&igrave;nh, c&oacute; thể tạo ra một cơ sở cho giao tiếp v&agrave; tương t&aacute;c tiếp theo.</p>
 <p>Th&ocirc;ng tin trong việc giới thiệu bản th&acirc;n thường bao gồm t&ecirc;n, tuổi, nơi sống, nghề nghiệp hoặc tr&igrave;nh độ học vấn v&agrave; những sở th&iacute;ch hoặc đặc điểm c&aacute; nh&acirc;n m&agrave; m&igrave;nh muốn người kh&aacute;c biết. Việc n&agrave;y c&oacute; thể diễn ra trong nhiều t&igrave;nh huống, chẳng hạn như trong cuộc phỏng vấn việc l&agrave;m, trong một tập trung x&atilde; hội, hoặc khi ch&uacute;ng ta mới quen biết ai đ&oacute;.</p>
 <p>Để giới thiệu bản th&acirc;n hiệu quả cần phải truyền đạt th&ocirc;ng tin một c&aacute;ch r&otilde; r&agrave;ng v&agrave; tự tin, tạo ấn tượng t&iacute;ch cực v&agrave; tạo điều kiện tốt cho mối quan hệ v&agrave; giao tiếp tiếp theo.</p>',
 3,1),
 (N'ecc62d4f-cb38-4fd5-ac46-f998d4940146',
 N'Quốc',N'Vỹ',
 N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSpaMoHpU5LhCtxzrDShIo_ZLuwTe1JojPc2A&usqp=CAU',
 CAST(N'1987/07/16' AS DATE),
 N'quocvy@gmail.com',
 N'0918456982',
 N'aa70b171-43ca-4dfc-af9b-b9e243ba6bcf',
 N'<p>Giới thiệu bản th&acirc;n l&agrave; việc tr&igrave;nh b&agrave;y th&ocirc;ng tin cơ bản về bản th&acirc;n m&igrave;nh cho người kh&aacute;c để họ hiểu th&ecirc;m về ch&uacute;ng ta, đến từ đ&acirc;u v&agrave; một số th&ocirc;ng tin quan trọng về cuộc sống v&agrave; sở th&iacute;ch của c&aacute; nh&acirc;n. Mục đ&iacute;ch ch&iacute;nh của việc giới thiệu bản th&acirc;n l&agrave; tạo ra một sự kết nối ban đầu, thiết lập mối quan hệ hoặc bắt đầu một cuộc tr&ograve; chuyện. Th&ocirc;ng qua việc giới thiệu, người kh&aacute;c c&oacute; cơ hội biết về bản th&acirc;n m&igrave;nh, c&oacute; thể tạo ra một cơ sở cho giao tiếp v&agrave; tương t&aacute;c tiếp theo.</p>
 <p>Th&ocirc;ng tin trong việc giới thiệu bản th&acirc;n thường bao gồm t&ecirc;n, tuổi, nơi sống, nghề nghiệp hoặc tr&igrave;nh độ học vấn v&agrave; những sở th&iacute;ch hoặc đặc điểm c&aacute; nh&acirc;n m&agrave; m&igrave;nh muốn người kh&aacute;c biết. Việc n&agrave;y c&oacute; thể diễn ra trong nhiều t&igrave;nh huống, chẳng hạn như trong cuộc phỏng vấn việc l&agrave;m, trong một tập trung x&atilde; hội, hoặc khi ch&uacute;ng ta mới quen biết ai đ&oacute;.</p>
 <p>Để giới thiệu bản th&acirc;n hiệu quả cần phải truyền đạt th&ocirc;ng tin một c&aacute;ch r&otilde; r&agrave;ng v&agrave; tự tin, tạo ấn tượng t&iacute;ch cực v&agrave; tạo điều kiện tốt cho mối quan hệ v&agrave; giao tiếp tiếp theo.</p>',
 3,1)
-- Mentors - Course
INSERT INTO [dbo].Course_Mentor(course_id, mentor_id)
VALUES 
(N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e', N'c5708f3c-daf1-444e-bed7-240c2ed6ad50'),
(N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e', N'9f2db427-5a6f-4313-98f8-35287820f903'),
(N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e', N'4af99375-1b6e-40c1-83cb-a291574d31ed'),
(N'c91a27ee-ab5a-473b-a5b5-5fce7652c50e', N'ecc62d4f-cb38-4fd5-ac46-f998d4940146')
GO
-- Member - Account
INSERT INTO [dbo].Account(email, password, role_id, is_active)
VALUES (N'member@gmail.com', N'QE1lbWJlcjEyMw==', 4, 1), --@Member123
(N'dinhtienthanh@gmail.com', N'QE1lbWJlcjEyMw==', 4, 1),
(N'daogiabao@gmail.com', N'QE1lbWJlcjEyMw==', 4, 1),
(N'truonggiathanh@gmail.com', N'QE1lbWJlcjEyMw==', 4, 1)
GO
-- Members - Address
INSERT INTO [dbo].Address(address_id, street, district, city)
VALUES 
(N'b5a7d95d-8e29-4294-a36f-19b1962f7abf',
 N'Hồ Văn Huê', N'Quận Bình Tân', N'TP.Hồ Chí Minh'),
(N'3e9e776e-6e58-4e9c-a0ab-9705452b1f85',
 N'Sư Vạn Hạnh', N'Quận 10', N'TP.Hồ Chí Minh'),
(N'0bd6b79b-2bb6-4b8b-aae6-452dc2977e2a',
 N'Xô Viết Nghệ Tĩnh', N'Quận Phú Nhuận', N'TP.Hồ Chí Minh'),
(N'85ae5335-4044-4acf-b54a-a01a46608f56',
 N'Trường Sa', N'Quận Phú Nhuận', N'TP.Hồ Chí Minh')
GO
-- Members
INSERT INTO [dbo].Member(member_id, first_name, last_name,
avatar_image, date_birth, email, phone, address_id,license_form_id,is_active)
VALUES 
(N'6f391bb6-e5ad-4bb4-b890-cfb37723f65e',
 N'Đinh Tiến', N'Thành',
 N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSpaMoHpU5LhCtxzrDShIo_ZLuwTe1JojPc2A&usqp=CAU',
 CAST(('2003/02/10') AS DATE),
 N'dinhtienthanh@gmail.com',
 N'0972657271',
 N'b5a7d95d-8e29-4294-a36f-19b1962f7abf',
 NULL,1),
 (N'3d9b7556-97d2-4192-8e83-cfe243c8ad15',
 N'Đào Gia', N'Bảo',
 N'https://www.svgrepo.com/show/382106/male-avatar-boy-face-man-user-9.svg',
 CAST(('2000/09/27') AS DATE),
 N'daogiabao@gmail.com',
 N'0767822919',
 N'3e9e776e-6e58-4e9c-a0ab-9705452b1f85',
 NULL,1),
 (N'1b4edcbd-6eb4-432f-8671-208b5ee9b9c5',
 N'Trương Gia', N'Thành',
 N'https://avatars.githubusercontent.com/u/56773306?v=4',
 CAST(('1999/08/22') AS DATE),
 N'truonggiathanh@gmail.com',
 N'0972657271',
 N'0bd6b79b-2bb6-4b8b-aae6-452dc2977e2a',
 NULL,1),
 (N'f5c63634-1858-4ad5-8d0e-38c73f8ef8a8',
 N'Member', N'',
 N'https://i.pinimg.com/550x/93/d3/e3/93d3e31639a4d07613de9dccdc8bd5e8.jpg',
 CAST(('1992/06/12') AS DATE),
 N'member@gmail.com',
 N'098277155',
 N'85ae5335-4044-4acf-b54a-a01a46608f56',
 NULL,1)
GO
-- Member - CoursePackageReservation
INSERT INTO [dbo].Course_Package_Reservation(course_package_reservation_id,course_package_id, create_date, member_id, staff_id,
payment_ammount, payment_type_id, reservation_status_id)
VALUES
(N'0343c03c-0d01-4ae9-83ab-8e08ce599646',
 N'069af18a-54b3-4de7-bac2-27d778431cc3',
 CAST(('2023/11/15') AS DATE),
 N'f5c63634-1858-4ad5-8d0e-38c73f8ef8a8',
 N'c5708f3c-daf1-444e-bed7-240c2ed6ad50',
 14500000, 3, 2),
 (N'638d59a5-2ab7-4e85-9ae2-aecdbc5b8a60',
 N'069af18a-54b3-4de7-bac2-27d778431cc3',
 CAST(('2023/12/02') AS DATE),
 N'1b4edcbd-6eb4-432f-8671-208b5ee9b9c5',
 N'c5708f3c-daf1-444e-bed7-240c2ed6ad50',
 14500000, 3, 2),
 (N'921330ce-43a2-44b8-bfd4-059655468092',
 N'825e8a00-ae5a-404e-994b-ebef5028e1ab',
 CAST(('2023/12/25') AS DATE),
 N'6f391bb6-e5ad-4bb4-b890-cfb37723f65e',
 N'9f2db427-5a6f-4313-98f8-35287820f903',
 4200000, 3, 2),
 (N'dd2cb8c2-d65e-49ba-8c08-185bcfcca57a',
 N'825e8a00-ae5a-404e-994b-ebef5028e1ab',
 CAST(('2023/11/05') AS DATE),
 N'3d9b7556-97d2-4192-8e83-cfe243c8ad15',
 N'4af99375-1b6e-40c1-83cb-a291574d31ed',
 4200000, 3, 2)
GO
-- Questions
SET IDENTITY_INSERT [dbo].Question OFF;
INSERT INTO [dbo].Question(question_answer_desc,is_Paralysis,image,license_type_id,is_active)
VALUES
(N'Khái niệm “phương tiện giao thông thô sơ đường bộ” được hiểu như thế nào là đúng?',1,null,1,1),
(N'Phương tiện tham gia giao thông đường bộ” gồm những loại nào?',0,null,1,1),
(N'Người tham gia giao thông đường bộ” gồm những đối tượng nào?',0,null,1,1),
(N'Người điều khiển phương tiện tham gia giao thông đường bộ” gồm những đối tượng nào dưới đây?',0,null,1,1),
(N'Khái niệm “người điều khiển giao thông” được hiểu như thế nào là đúng?',0,null,1,1),
(N'Khái niệm “đỗ xe” được hiểu như thế nào là đúng?',1,null,1,1),
(N'Cuộc đua xe chỉ được thực hiện khi nào?',0,null,1,1),
(N'Người điều khiển phương tiện giao thông đường bộ mà trong cơ thể có chất ma túy có bị nghiêm cấm hay không?',0,null,1,1),
(N'Sử dụng rượu, bia khi lái xe, nếu bị phát hiện thì bị xử lý như thế nào?',1,null,1,1),
(N'Theo luật phòng chống tác hại của rượu, bia, đối tượng nào dưới đây bị cấm sử dụng rượu, bia khi tham gia giao thông?',1,null,1,1),
(N'Khái niệm “phương tiện giao thông thô sơ đường bộ” được hiểu như thế nào là đúng?',1,null,1,1),
(N'Phương tiện tham gia giao thông đường bộ” gồm những loại nào?',0,null,1,1),
(N'Người tham gia giao thông đường bộ” gồm những đối tượng nào?',0,null,1,1),
(N'Người điều khiển phương tiện tham gia giao thông đường bộ” gồm những đối tượng nào dưới đây?',0,null,1,1),
(N'Khái niệm “người điều khiển giao thông” được hiểu như thế nào là đúng?',0,null,1,1),
(N'Khái niệm “đỗ xe” được hiểu như thế nào là đúng?',1,null,1,1),
(N'Cuộc đua xe chỉ được thực hiện khi nào?',0,null,1,1),
(N'Người điều khiển phương tiện giao thông đường bộ mà trong cơ thể có chất ma túy có bị nghiêm cấm hay không?',0,null,1,1),
(N'Sử dụng rượu, bia khi lái xe, nếu bị phát hiện thì bị xử lý như thế nào?',1,null,1,1),
(N'Theo luật phòng chống tác hại của rượu, bia, đối tượng nào dưới đây bị cấm sử dụng rượu, bia khi tham gia giao thông?',1,null,1,1),
(N'Khái niệm “đỗ xe” được hiểu như thế nào là đúng?',1,null,1,1),
(N'Phương tiện tham gia giao thông đường bộ” gồm những loại nào?',0,null,1,1),
(N'Sử dụng rượu, bia khi lái xe, nếu bị phát hiện thì bị xử lý như thế nào?',1,null,1,1),
(N'Theo luật phòng chống tác hại của rượu, bia, đối tượng nào dưới đây bị cấm sử dụng rượu, bia khi tham gia giao thông?',1,null,1,1),
(N'Khái niệm “người điều khiển giao thông” được hiểu như thế nào là đúng?',0,null,1,1)
SET IDENTITY_INSERT [dbo].Question ON;
INSERT INTO [dbo].Question_Answer(answer,is_true,question_id)
VALUES 
(N'Gồm xe đạp (kể cả xe đạp máy, xe đạp điện), xe xích lô, xe lăn dùng cho người khuyết tật, xe súc vật kéo và các loại xe tương tự.',1,1),
(N'Gồm xe đạp (kể cả xe đạp máy, xe đạp điện), xe gắn máy, xe cơ giới dùng cho người khuyết tật và xe máy chuyên dùng.',0,1),
(N'Gồm xe ô tô, máy kéo, rơ moóc hoặc sơ mi rơ moóc được kéo bởi xe ô tô, máy kéo.',0,1),
(N'Phương tiện giao thông cơ giới đường bộ.',0,2),
(N'Phương tiện giao thông thô sơ đường bộ và xe máy chuyên dùng.',0,2),
(N'Cả ý 1 và ý 2.',1,2),
(N'Người điều khiển, người sử dụng phương tiện tham gia giao thông đường bộ.',0,3),
(N'Người điều khiển, dẫn dắt súc vật; người đi bộ trên đường bộ.',0,3),
(N'Cả ý 1 và ý 2.',1,3),
(N'Người điều khiển, người sử dụng phương tiện tham gia giao thông đường bộ.',0,4),
(N'Người điều khiển, dẫn dắt súc vật; người đi bộ trên đường bộ.',0,4),
(N'Cả ý 1 và ý 2.',1,4),
(N'Là người điều khiển phương tiện tham gia giao thông tại nơi thi công, nơi ùn tắc giao thông, ở bến phà, tại cầu đường bộ đi chung với đường sắt.',0,5),
(N'Là cảnh sát giao thông, người được giao nhiệm vụ hướng dẫn giao thông tại nơi thi công, nơi ùn tắc giao thông, ở bến phà, tại cầu đường bộ đi chung với đường sắt.',1,5),
(N'Là người tham gia giao thông tại nơi thi công, nơi ùn tắc giao thông, ở bến phà, tại cầu đường bộ đi chung với đường sắt.',0,5),
(N'Là trạng thái đứng yên của phương tiện giao thông không giới hạn thời gian để cho người lên, xuống phương tiện, xếp dỡ hàng hóa hoặc thực hiện công việc khác.',0,6),
(N'Là trạng thái đứng yên tạm thời của phương tiện giao thông trong một khoảng thời gian cần thiết đủ để cho người lên, xuống phương tiện, xếp dỡ hàng hóa hoặc thực hiện công việc khác.',0,6),
(N'Là trạng thái đứng yên của phương tiện giao thông không giới hạn thời gian để cho người lên, xuống phương tiện, xếp dỡ hàng hóa hoặc thực hiện công việc khác.',1,6),
(N'Là trạng thái đứng yên của phương tiện giao thông có thời hạn trong một khoảng thời gian cần thiết đủ để cho người lên, xuống phương tiện, xếp dỡ hàng hóa hoặc thực hiện công việc khác..',0,7),
(N'Là trạng thái đứng yên của phương tiện giao thông không giới hạn thời gian.',1,7),
(N'Diễn ra trên đường phố không có người qua lại.',0,8),
(N'Là trạng thái đứng yên của phương tiện giao thông không giới hạn thời gian để cho người lên, xuống phương tiện, xếp dỡ hàng hóa hoặc thực hiện công việc khác.',1,8),
(N'Được người dân ủng hộ.',0,8),
(N'Bị nghiêm cấm.',1,9),
(N'Không bị nghiêm cấm.',0,9),
(N'Không bị nghiêm cấm, nếu có chất ma túy ở mức nhẹ, có thể điều khiển phương tiện tham gia giao thông.',0,9),
(N'Chỉ bị nhắc nhở.',0,10),
(N'Bị xử phạt hành chính hoặc có thể bị xử lý hình sự tùy theo mức độ vi phạm.',1,10),
(N'Không bị xử lý hình sự.',0,10),
(N'Là người điều khiển phương tiện tham gia giao thông tại nơi thi công, nơi ùn tắc giao thông, ở bến phà, tại cầu đường bộ đi chung với đường sắt.',0,11),
(N'Là cảnh sát giao thông, người được giao nhiệm vụ hướng dẫn giao thông tại nơi thi công, nơi ùn tắc giao thông, ở bến phà, tại cầu đường bộ đi chung với đường sắt.',1,11),
(N'Là người tham gia giao thông tại nơi thi công, nơi ùn tắc giao thông, ở bến phà, tại cầu đường bộ đi chung với đường sắt.',0,11),
(N'Người điều khiển, người sử dụng phương tiện tham gia giao thông đường bộ.',0,12),
(N'Người điều khiển, dẫn dắt súc vật; người đi bộ trên đường bộ.',0,12),
(N'Cả ý 1 và ý 2.',1,12),
(N'Là trạng thái đứng yên của phương tiện giao thông có thời hạn trong một khoảng thời gian cần thiết đủ để cho người lên, xuống phương tiện, xếp dỡ hàng hóa hoặc thực hiện công việc khác..',0,13),
(N'Là trạng thái đứng yên của phương tiện giao thông không giới hạn thời gian.',1,13),
(N'Diễn ra trên đường phố không có người qua lại.',0,14),
(N'Là trạng thái đứng yên của phương tiện giao thông không giới hạn thời gian để cho người lên, xuống phương tiện, xếp dỡ hàng hóa hoặc thực hiện công việc khác.',1,14),
(N'Được người dân ủng hộ.',0,14),
(N'Bị nghiêm cấm.',1,15),
(N'Không bị nghiêm cấm.',0,15),
(N'Không bị nghiêm cấm, nếu có chất ma túy ở mức nhẹ, có thể điều khiển phương tiện tham gia giao thông.',0,15),
(N'Chỉ bị nhắc nhở.',0,16),
(N'Bị xử phạt hành chính hoặc có thể bị xử lý hình sự tùy theo mức độ vi phạm.',1,16),
(N'Không bị xử lý hình sự.',0,16),
(N'Diễn ra trên đường phố không có người qua lại.',0,17),
(N'Là trạng thái đứng yên của phương tiện giao thông không giới hạn thời gian để cho người lên, xuống phương tiện, xếp dỡ hàng hóa hoặc thực hiện công việc khác.',1,17),
(N'Được người dân ủng hộ.',0,17),
(N'Bị nghiêm cấm.',1,18),
(N'Không bị nghiêm cấm.',0,18),
(N'Là người điều khiển phương tiện tham gia giao thông tại nơi thi công, nơi ùn tắc giao thông, ở bến phà, tại cầu đường bộ đi chung với đường sắt.',0,19),
(N'Là cảnh sát giao thông, người được giao nhiệm vụ hướng dẫn giao thông tại nơi thi công, nơi ùn tắc giao thông, ở bến phà, tại cầu đường bộ đi chung với đường sắt.',1,19),
(N'Là người tham gia giao thông tại nơi thi công, nơi ùn tắc giao thông, ở bến phà, tại cầu đường bộ đi chung với đường sắt.',0,19),
(N'Là trạng thái đứng yên của phương tiện giao thông không giới hạn thời gian để cho người lên, xuống phương tiện, xếp dỡ hàng hóa hoặc thực hiện công việc khác.',0,20),
(N'Là trạng thái đứng yên tạm thời của phương tiện giao thông trong một khoảng thời gian cần thiết đủ để cho người lên, xuống phương tiện, xếp dỡ hàng hóa hoặc thực hiện công việc khác.',0,20),
(N'Là trạng thái đứng yên của phương tiện giao thông không giới hạn thời gian để cho người lên, xuống phương tiện, xếp dỡ hàng hóa hoặc thực hiện công việc khác.',1,20),
(N'Là người điều khiển phương tiện tham gia giao thông tại nơi thi công, nơi ùn tắc giao thông, ở bến phà, tại cầu đường bộ đi chung với đường sắt.',0,21),
(N'Là cảnh sát giao thông, người được giao nhiệm vụ hướng dẫn giao thông tại nơi thi công, nơi ùn tắc giao thông, ở bến phà, tại cầu đường bộ đi chung với đường sắt.',1,21),
(N'Là người tham gia giao thông tại nơi thi công, nơi ùn tắc giao thông, ở bến phà, tại cầu đường bộ đi chung với đường sắt.',0,21),
(N'Là trạng thái đứng yên của phương tiện giao thông không giới hạn thời gian để cho người lên, xuống phương tiện, xếp dỡ hàng hóa hoặc thực hiện công việc khác.',0,22),
(N'Là trạng thái đứng yên tạm thời của phương tiện giao thông trong một khoảng thời gian cần thiết đủ để cho người lên, xuống phương tiện, xếp dỡ hàng hóa hoặc thực hiện công việc khác.',0,22),
(N'Là trạng thái đứng yên của phương tiện giao thông không giới hạn thời gian để cho người lên, xuống phương tiện, xếp dỡ hàng hóa hoặc thực hiện công việc khác.',1,22),
(N'Chỉ bị nhắc nhở.',0,23),
(N'Bị xử phạt hành chính hoặc có thể bị xử lý hình sự tùy theo mức độ vi phạm.',1,23),
(N'Không bị xử lý hình sự.',0,23),
(N'Diễn ra trên đường phố không có người qua lại.',0,24),
(N'Là trạng thái đứng yên của phương tiện giao thông không giới hạn thời gian để cho người lên, xuống phương tiện, xếp dỡ hàng hóa hoặc thực hiện công việc khác.',1,24),
(N'Được người dân ủng hộ.',0,24),
(N'Gồm xe đạp (kể cả xe đạp máy, xe đạp điện), xe xích lô, xe lăn dùng cho người khuyết tật, xe súc vật kéo và các loại xe tương tự.',1,25),
(N'Gồm xe đạp (kể cả xe đạp máy, xe đạp điện), xe gắn máy, xe cơ giới dùng cho người khuyết tật và xe máy chuyên dùng.',0,25),
(N'Gồm xe ô tô, máy kéo, rơ moóc hoặc sơ mi rơ moóc được kéo bởi xe ô tô, máy kéo.',0,25)
GO
INSERT INTO [dbo].Theory_Exam(total_question, total_time, total_answer_required, license_type_id)
VALUES(25, 15, 24, 2)
GO
-- Exam Questions
INSERT INTO [dbo].Exam_Question(question_id, theory_exam_id)
VALUES
	(1,1),
	(2,1),
	(3,1),
	(4,1),
	(5,1),
	(6,1),
	(7,1),
	(8,1),
	(9,1),
	(10,1),
	(11,1),
	(12,1),
	(13,1),
	(14,1),
	(15,1),
	(16,1),
	(17,1),
	(18,1),
	(19,1),
	(20,1),
	(21,1),
	(22,1),
	(23,1),
	(24,1),
	(25,1)