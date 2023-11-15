/** ----------------------------TODO TASK--------------------------**/

** Task
//----------------------------------------------//
* Course Management (Thanh)
{
	CRUD, Hide/Unhide Course
	CRUD Course Package
	Add Course Curriculum
	Add Course Mentor
}	
//----------------------------------------------//
* Staff Management (Thanh)
{
	CRUD Slot 
	CRUD, Import/Export Excel Staff
}
//---------------------------------------------//
* Member Management (Thanh)
{
	Import/Export Excel
}
//---------------------------------------------//
* Staff/Course Feedback, Comment (Thu)
{
	Member do feedback, comment in course/mentor details
}
//--------------------------------------------//
* Member license form register (Thanh)
{
	-> Member register/update license form 
	-> Approve/Deny license form 
	-> hard code register form -> export PDF
}

** Note FE
//----------------------------------------------//
{
	* Home Page (Complete all feature) -> vietnamese language(Thu)
	* Navigation (Fix same border bottom) (Thu)
	* Feedback (Thu)
		-> Feedback/comment in mentor detail page 
	* Schedule (Thanh)
		-> Show schedule in mentor detail page
		-> Image import button UI design
}

//----------------------------------------------//
* GET: teaching-schedules/await
{
	-> change response type
	+ prev: Ok(mentors)
	+ curr: 
		OK{
			StatusCode,
			Message,
			Data = mentors
		} -> response.data.data
		BadRequest(new BaseResponse { 
			StatusCode = StatusCodes.Status400BadRequest,
			Message = "Hiện chưa có lịch cần duyệt"
		});
}
//----------------------------------------------//
* GET: staffs/mentors/{id}/schedule 
{
	!isActive -> note that (schedule is awaiting approval )
}
//----------------------------------------------//
* GET: staffs/mentors/{id}/schedule/filters
{
	!isActive -> note that (schedule is awaiting approval )
}
//---------------------------------------------//
* POST: staffs/mentors/{id}/schedule-register
{
	show current day before choose
	register success -> load component by response data 
	
	StatusCode = StatusCodes.Status201Created,
	Data is same as GET: staffs/mentors/{id}/schedule
}
//---------------------------------------------//
* POST: staffs/mentors/{id}/schedule-register/range
{
	Create button for register range of schedule 
	-> totalSchedule.Length in (GET: staffs/mentors/{id}/schedule) response data 
		-> ("totalSchedule": 4)
	-> if totalSchedule > 0 ? Button create daily schedule (POST: staffs/mentors/{id}/schedule-register)
							: Button create range schedule (POST: staffs/mentors/{id}/schedule-register/range)
							
	register success -> load component by response data 
	
	StatusCode = StatusCodes.Status201Created,
	Data is same as GET: staffs/mentors/{id}/schedule						
}
//----------------------------------------------//
* DELETE: staffs/{id:Guid}/delete
{
	-> comfirm before delete -> (Xóa nhân viên sẽ xóa toàn bộ các các lịch dạy, 
		khóa học đứng lớp (nếu có))
}
//----------------------------------------------//
* DELETE: courses/{id:Guid}
{
	-> comfirm before delete -> (Xoá khóa học sẽ xóa người hướng dẫn đứng
	lớp, gói học, lịch học liên quan)
}


// TODO:
// Xoa slot : DONE!
// quan li xe, import/export excel
// huy lich day/ lich hoc 
// dang ky lai lich day/ lich hoc 
// duyet license form
// tao member validate birthdate
