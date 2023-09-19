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
	member_id NVARCHAR(255) PRIMARY KEY,--guid type
	first_name NVARCHAR(155) NOT NULL,
	last_name NVARCHAR(155) NOT NULL,
	date_birth DATETIME,
	phone NVARCHAR(15),
	is_active BIT,
	address_id NVARCHAR(255),
	email nvarchar(255),
	license_type_id INT
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
	staff_id NVARCHAR(255) PRIMARY KEY, 
	first_name NVARCHAR(155) NOT NULL,
	last_name NVARCHAR(155) NOT NULL,
	date_birth DATETIME NOT NULL,
	phone NVARCHAR(15) NOT NULL,
	is_active BIT,
	avatar_image NVARCHAR(100),
	email NVARCHAR(255),
	address_id NVARCHAR(255),
	job_title_id INT,
	license_type_id INT
)
GO
CREATE TABLE [dbo].Course(
	course_id NVARCHAR(255) PRIMARY KEY,
	course_title NVARCHAR(255) NOT NULL,
	course_desc NVARCHAR(MAX),
	cost FLOAT NOT NULL,
	total_session INT,
	is_active BIT
)
GO
CREATE TABLE [dbo].Curriculum(
	curriculum_id INT PRIMARY KEY identity(1,1),
	curriculum_desc NVARCHAR(155) NOT NULL,
	curriculum_detail NVARCHAR(MAX) 
)
GO
CREATE TABLE [dbo].Course_Curriculum(
	course_id NVARCHAR(255),
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
	member_id NVARCHAR(255) NOT NULL,
	course_id NVARCHAR(255) NOT NULL,
	staff_id NVARCHAR(255) NOT NULL,
	course_reservation_status_id INT,
	invoice_id INT
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
CREATE TABLE [dbo].Invoice(
	invoice_id INT PRIMARY KEY identity(1,1),
	ammount FLOAT,
	create_date DATETIME,
	payment_type_id INT
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
	vehicle_license_plate NVARCHAR(155) NOT NULL,
	register_date DATETIME,
	vehicle_type_id INT
)
GO
CREATE TABLE [dbo].Course_Schedule(
	course_schedule_id INT PRIMARY KEY identity(1,1),
	teaching_date DATETIME NOT NULL,
	staff_id NVARCHAR(255) NOT NULL,
	course_reservation_id NVARCHAR(255) NOT NULL,
	vehicle_id INT
)
GO
CREATE TABLE [dbo].Roll_Call_Book(
	roll_call_book_id INT PRIMARY KEY identity(1,1),
	isAbsence BIT,
	comment NVARCHAR(255),
	member_id NVARCHAR(255) NOT NULL,
	course_schedule_id INT NOT NULL
)
GO
CREATE TABLE [dbo].FeedBack(
	feedback_id INT PRIMARY KEY identity(1,1),
	content NVARCHAR(255),
	rating_star INT,
	member_id NVARCHAR(255),
	staff_id NVARCHAR(255),
	course_id NVARCHAR(255)
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
	member_id NVARCHAR(255),
	register_form_status_id INT,
	license_type_id INT
)
GO
CREATE TABLE [dbo].Question_Bank(
	question_id INT PRIMARY KEY identity(1,1),
	question_answer_desc NVARCHAR(MAX),
	is_Paralysis BIT,
	image NVARCHAR(100),
	license_type_id INT,
)
GO
CREATE TABLE [dbo].Question_Answer(
	question_answer_id INT PRIMARY KEY identity(1,1),
	answer NVARCHAR(MAX),
	is_true BIT,
	question_id INT
)
GO
CREATE TABLE [dbo].Practice_Exam(
	practice_exam_id INT PRIMARY KEY identity(1,1),
	total_question INT,
	total_time INT,
	total_answer_required INT,
	license_type_id INT
)
GO
CREATE TABLE [dbo].Exam_Question(
	question_id INT,
	practice_exam_id INT,

	PRIMARY KEY (question_id, practice_exam_id)
)
GO
CREATE TABLE [dbo].Exam_Grade(
	member_id NVARCHAR(255),
	practice_exam_id INT,
	point FLOAT,
	question_id INT NOT NULL,
	selected_answer_id INT NOT NULL,
	email NVARCHAR(255)

	PRIMARY KEY (member_id, practice_exam_id)
)
GO
CREATE TABLE [dbo].Exam_History(
	member_id NVARCHAR(255),
	practice_exam_id INT,
	total_grade INT,
	total_right_answer INT,
	total_question INT,
	total_time INT,
	wrong_paralysis_question BIT,
	is_passed BIT,

	PRIMARY KEY (member_id, practice_exam_id)
)
GO
CREATE TABLE [dbo].Blog(
	blog_id INT PRIMARY KEY identity(1,1),
	staff_id NVARCHAR(255),
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
CREATE TABLE [dbo].Statistical_Report(
	report_id INT PRIMARY KEY identity(1,1),
	total_member INT,
	total_mentor INT,
	total_course INT,
	total_course_schedule INT,
	total_blog INT,
	total_revenue INT,
	total_active_member INT
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
-- dbo.Curriculum - dbo.Course_Curriculum
ALTER TABLE Course_Curriculum
ADD CONSTRAINT FK_CourseCurriculum_CurriculumnId FOREIGN KEY (curriculum_id) REFERENCES Curriculum (curriculum_id)
-- dbo.Course - dbo.Course_Curriculum
ALTER TABLE Course_Curriculum
ADD CONSTRAINT FK_CourseCurriculum_CourseId FOREIGN KEY (course_id) REFERENCES Course (course_id)
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
-- dbo.Course_Reservation - dbo.Invoice
ALTER TABLE Course_Reservation
ADD CONSTRAINT FK_CourseReservation_InvoiceId FOREIGN KEY (invoice_id) REFERENCES Invoice (invoice_id)
-- dbo.Invoice - dbo.Payment_Type
ALTER TABLE Invoice
ADD CONSTRAINT FK_Invoice_PaymentTypeId FOREIGN KEY (payment_type_id) REFERENCES Payment_Type (payment_type_id)
-- dbo.Vehicle - dbo.Vehicle_Type
ALTER TABLE Vehicle
ADD CONSTRAINT FK_Vehicle_TypeId FOREIGN KEY (vehicle_type_id) REFERENCES Vehicle_Type (vehicle_type_id)
-- dbo.Course_Schedule - dbo.Vehicle
ALTER TABLE Course_Schedule
ADD CONSTRAINT FK_CourseSchedule_VehicleId FOREIGN KEY (vehicle_id) REFERENCES Vehicle (vehicle_id)
-- dbo.Course_Schedule - dbo.Course_Reservation
ALTER TABLE Course_Schedule
ADD CONSTRAINT FK_CourseSchedule_ReservationId FOREIGN KEY (course_reservation_id) REFERENCES Course_Reservation (course_reservation_id)
-- dbo.Course_Schedule - dbo.Staff
ALTER TABLE Course_Schedule
ADD CONSTRAINT FK_CourseSchedule_StaffId FOREIGN KEY (staff_id) REFERENCES Staff (staff_id)
-- dbo.Roll_Call_Book - dbo.Course_Schedule
ALTER TABLE Roll_Call_Book 
ADD CONSTRAINT FK_RollCallBook_CourseScheduleId FOREIGN KEY (course_schedule_id) REFERENCES Course_Schedule (course_schedule_id)
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
-- dbo.License_Register_Form - dbo.License_Type
ALTER TABLE	License_Register_Form
ADD CONSTRAINT FK_MemberId FOREIGN KEY (member_id) REFERENCES Member(member_id)
-- dbo.Question_Bank - dbo.License_Type
ALTER TABLE Question_Bank
ADD CONSTRAINT FK_QuestionBank_LicenseTypeId FOREIGN KEY (license_type_id) REFERENCES License_Type (license_type_id)
-- dbo.Question_Answer - dbo.Question_Bank
ALTER TABLE Question_Answer 
ADD CONSTRAINT FK_QuestionAnswer_QuestionId FOREIGN KEY (question_id) REFERENCES Question_Bank (question_id)
-- dbo.Exam_Question - dbo.Question_Bank
ALTER TABLE Exam_Question 
ADD CONSTRAINT FK_ExamQuestion_QuestionId FOREIGN KEY (question_id) REFERENCES Question_Bank (question_id)
-- dbo.Exam_Question - dbo.Practice_Exam
ALTER TABLE Exam_Question 
ADD CONSTRAINT FK_ExamQuestion_PracticeExamId FOREIGN KEY (practice_exam_id) REFERENCES Practice_Exam (practice_exam_id)
-- dbo.Practice_Exam - dbo.License_Type
ALTER TABLE Practice_Exam
ADD CONSTRAINT FK_PracticeExam_LicenseTypeId FOREIGN KEY (license_type_id) REFERENCES License_Type (license_type_id)
-- dbo.Exam_Grade - dbo.Member
ALTER TABLE Exam_Grade 
ADD CONSTRAINT FK_ExamGrade_MemberId FOREIGN KEY (member_id) REFERENCES Member (member_id)
-- dbo.Exam_Grade - dbo.Practice_Exam
ALTER TABLE Exam_Grade
ADD CONSTRAINT FK_ExamGrade_PracticeExamId FOREIGN KEY (practice_exam_id) REFERENCES Practice_Exam (practice_exam_id)
-- dbo.Exam_Grade - dbo.Question_Bank
ALTER TABLE Exam_Grade
ADD CONSTRAINT FK_ExamGrade_QuestionId FOREIGN KEY (question_id) REFERENCES Question_Bank (question_id)
-- dbo.Exam_History - dbo.Member
ALTER TABLE Exam_History
ADD CONSTRAINT FK_ExamHistory_MemberId FOREIGN KEY (member_id) REFERENCES Member (member_id)
-- dbo.Exam_History - dbo.Practice_Exam
ALTER TABLE Exam_History
ADD CONSTRAINT FK_History_PracticeExamId FOREIGN KEY (practice_exam_id) REFERENCES Practice_Exam (practice_exam_id)
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