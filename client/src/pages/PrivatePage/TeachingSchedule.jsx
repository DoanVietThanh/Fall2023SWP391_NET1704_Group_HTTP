import React from 'react';
import SideBar from '../../components/SideBar';
import { Box } from '@mui/material';
import * as dayjs from 'dayjs';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import { AiOutlineCheckCircle, AiOutlineCloseCircle } from 'react-icons/ai';
const response = {
  statusCode: 200,
  message: null,
  data: {
    course: {
      courseId: '6479a6bc-1396-4147-b899-62c97009651b',
      courseTitle: 'Khóa học bằng lái B2',
      courseDesc:
        '<div>hello</div> <big>Oops, something went wrong</big> <small>Yups, good job</small><b><i><u>Nah, too bad</u></i></b>',
      cost: 120000,
      totalSession: 12,
      totalMonth: 1,
      startDate: '2023-10-04T00:00:00',
      isActive: true,
      feedBacks: [],
      curricula: [],
      mentors: [
        {
          staffId: '29ae4265-269d-4a11-8df6-b05f3c345cd4',
          firstName: 'Thanh',
          lastName: 'Sa',
          dateBirth: '2003-09-22T00:00:00',
          phone: '892131231',
          isActive: true,
          avatarImage: 'a42e811d-d22b-4ede-b955-1437ebaeeb9d',
          email: 'dawnk1003@gmail.com',
          addressId: '07a43b7a-6156-4c5a-a18d-382e574b4ba8',
          jobTitleId: 3,
          licenseTypeId: 3,
          address: null,
          emailNavigation: null,
          jobTitle: null,
          licenseType: null,
          feedBacks: [],
          courses: [],
        },
      ],
    },
    filter: [
      {
        id: 1,
        desc: '02/10 To 08/10',
      },
      {
        id: 2,
        desc: '09/10 To 15/10',
      },
      {
        id: 3,
        desc: '16/10 To 22/10',
      },
      {
        id: 4,
        desc: '23/10 To 29/10',
      },
      {
        id: 5,
        desc: '30/10 To 02/11',
      },
    ],
    weekdays: {
      weekdayScheduleId: 2,
      monday: '2023-10-09T00:00:00',
      tuesday: '2023-10-10T00:00:00',
      wednesday: '2023-10-11T00:00:00',
      thursday: '2023-10-12T00:00:00',
      friday: '2023-10-13T00:00:00',
      saturday: '2023-10-14T00:00:00',
      sunday: '2023-10-15T00:00:00',
      courseId: '6479a6bc-1396-4147-b899-62c97009651b',
      weekdayScheduleDesc: '09/10 To 15/10',
      course: null,
      teachingSchedules: [],
    },
    slotSchedules: [
      {
        slotId: 1,
        slotName: 'Slot 1',
        duration: 2,
        time: '07:30:00',
        slotDesc: '07:30 - 09:30',
        teachingSchedules: [
          null,
          {
            teachingScheduleId: 3,
            teachingDate: '2023-10-10T00:00:00',
            staffId: '29ae4265-269d-4a11-8df6-b05f3c345cd4',
            slotId: 1,
            vehicleId: 1,
            weekdayScheduleId: 2,
            staff: null,
            vehicle: null,
          },
          null,
          null,
          null,
          null,
          {
            teachingScheduleId: 7,
            teachingDate: '2023-10-15T00:00:00',
            staffId: '29ae4265-269d-4a11-8df6-b05f3c345cd4',
            slotId: 1,
            vehicleId: null,
            weekdayScheduleId: 2,
            staff: null,
            vehicle: null,
          },
        ],
      },
      {
        slotId: 2,
        slotName: 'Slot 2',
        duration: 2,
        time: '09:45:00',
        slotDesc: '09:45 - 11:45',
        teachingSchedules: [
          null,
          null,
          {
            teachingScheduleId: 4,
            teachingDate: '2023-10-11T00:00:00',
            staffId: '29ae4265-269d-4a11-8df6-b05f3c345cd4',
            slotId: 2,
            vehicleId: null,
            weekdayScheduleId: 2,
            staff: null,
            vehicle: null,
          },
          null,
          null,
          null,
          null,
        ],
      },
      {
        slotId: 3,
        slotName: 'Slot 3',
        duration: 2,
        time: '13:30:00',
        slotDesc: '13:30 - 15:30',
        teachingSchedules: [
          null,
          null,
          null,
          {
            teachingScheduleId: 5,
            teachingDate: '2023-10-12T00:00:00',
            staffId: '29ae4265-269d-4a11-8df6-b05f3c345cd4',
            slotId: 3,
            vehicleId: null,
            weekdayScheduleId: 2,
            staff: null,
            vehicle: null,
          },
          null,
          null,
          null,
        ],
      },
      {
        slotId: 4,
        slotName: 'Slot 3',
        duration: 2,
        time: '15:45:00',
        slotDesc: '15:45 - 17:45',
        teachingSchedules: [
          null,
          null,
          null,
          null,
          null,
          {
            teachingScheduleId: 6,
            teachingDate: '2023-10-14T00:00:00',
            staffId: '29ae4265-269d-4a11-8df6-b05f3c345cd4',
            slotId: 4,
            vehicleId: null,
            weekdayScheduleId: 2,
            staff: null,
            vehicle: null,
          },
          null,
        ],
      },
    ],
  },
};

const weekdays = [
  'monday',
  'tuesday',
  'wednesday',
  'thursday',
  'friday',
  'saturday',
  'sunday',
];

const TeachingSchedule = () => {
  const [open, setOpen] = React.useState(false);

  const handleClickOpen = () => setOpen(true);

  const handleClose = () => setOpen(false);

  return (
    <div className='flex'>
      <SideBar />

      <Box component='main' sx={{ flexGrow: 1, p: 3 }}>
        <div className='h-[80vh] w-full rounded overflow-y-auto mt-[64px]'>
          <div>
            <h1 className='font-bold text-[30px] uppercase mb-4'>
              Lịch học theo tuần
            </h1>
            <div className=' min-h-[70vh]'>
              <table className='w-full border-2 border-black'>
                <tr>
                  <th className='w-[8%]'>Select</th>
                  <th className='capitalize'>
                    <div className='flex flex-col'>
                      Thứ 2{' '}
                      <p className='text-yellow-700'>
                        {dayjs(response.data.weekdays.monday).format('DD-MM')}
                      </p>
                    </div>
                  </th>
                  <th className='capitalize'>
                    <div className='flex flex-col'>
                      Thứ 3{' '}
                      <p className='text-yellow-700'>
                        {dayjs(response.data.weekdays.tuesday).format('DD-MM')}
                      </p>
                    </div>
                  </th>
                  <th className='capitalize'>
                    <div className='flex flex-col'>
                      Thứ 4{' '}
                      <p className='text-yellow-700'>
                        {dayjs(response.data.weekdays.wednesday).format(
                          'DD-MM'
                        )}
                      </p>
                    </div>
                  </th>
                  <th className='capitalize'>
                    <div className='flex flex-col'>
                      Thứ 5{' '}
                      <p className='text-yellow-700'>
                        {dayjs(response.data.weekdays.thursday).format('DD-MM')}
                      </p>
                    </div>
                  </th>
                  <th className='capitalize'>
                    <div className='flex flex-col'>
                      Thứ 6{' '}
                      <p className='text-yellow-700'>
                        {dayjs(response.data.weekdays.friday).format('DD-MM')}
                      </p>
                    </div>
                  </th>
                  <th className='capitalize'>
                    <div className='flex flex-col'>
                      Thứ 7{' '}
                      <p className='text-yellow-700'>
                        {dayjs(response.data.weekdays.saturday).format('DD-MM')}
                      </p>
                    </div>
                  </th>
                  <th className='capitalize'>
                    <div className='flex flex-col'>
                      Chủ nhật{' '}
                      <p className='text-yellow-700'>
                        {dayjs(response.data.weekdays.sunday).format('DD-MM')}
                      </p>
                    </div>
                  </th>
                </tr>
                {response?.data.slotSchedules.map((item, index) => (
                  <tr>
                    <td>{item.slotName}</td>
                    {item.teachingSchedules.map((itemDate, indexItemDate) => (
                      <td>
                        {itemDate ? (
                          <div className='text-center'>
                            <p className='font-bold text-blue-800'>
                              {response.data.course.courseTitle}
                            </p>
                            <div className='bg-green-500 text-white w-auto rounded-lg mx-2 text-[12px]'>
                              {item.slotDesc}
                            </div>
                            <button
                              onClick={handleClickOpen}
                              className='p-2 text-black bg-gray-200 mt-2 rounded-lg hover:bg-blue-400 hover:text-white'
                            >
                              Xem chi tiết
                            </button>
                          </div>
                        ) : (
                          ''
                        )}
                      </td>
                    ))}
                  </tr>
                ))}
              </table>
            </div>
          </div>
        </div>
      </Box>

      <Dialog
        open={open}
        onClose={handleClose}
        aria-labelledby='alert-dialog-title'
        aria-describedby='alert-dialog-description'
      >
        <div className='flex gap-10 p-2'>
          <DialogContent>
            <DialogTitle id='alert-dialog-title'>
              <h2 className='font-bold'> Chi tiết Lịch học</h2>
            </DialogTitle>
            <DialogContentText id='alert-dialog-description'>
              <div className='flex flex-col gap-4'>
                <div className='flex gap-4'>
                  <div>Date:</div>
                  <div>
                    {dayjs(response.data.weekdays.monday).format('DD/MM/YYYY')}
                  </div>
                </div>
                <div className='flex gap-4'>
                  <div>Tiết học:</div>
                  <div> ??? </div>
                </div>
                <div className='flex gap-4'>
                  <div>Người hướng dẫn:</div>
                  <div>Thanh Đoàn</div>
                </div>
                <div className='flex gap-4'>
                  <div>Khóa học:</div>
                  <div>Bằng lái B2 </div>
                </div>
                <div className='flex gap-4'>
                  <div>Tổng buổi học:</div>
                  <div>12</div>
                </div>
                <div className='flex gap-4'>
                  <div>Điểm danh:</div>
                  <div className='text-green-700 flex justify-center items-center gap-8'>
                    <span>
                      <AiOutlineCheckCircle size={20} />
                    </span>
                    <span className='text-red-800'>
                      <AiOutlineCloseCircle size={20} />
                    </span>
                  </div>
                </div>
              </div>
            </DialogContentText>
          </DialogContent>

          <DialogContent>
            <DialogTitle id='alert-dialog-title'>
              <h2 className='font-bold'>Phương tiện</h2>
            </DialogTitle>
            <DialogContentText id='alert-dialog-description'>
              <div className='flex flex-col gap-4'>
                <div className='flex gap-4'>
                  <div>Tên xe:</div>
                  <div>Rand Rover</div>
                </div>
                <div className='flex gap-4'>
                  <div>Biển số:</div>
                  <div>51F-891.12</div>
                </div>
                <div className='w-full'>
                  <img
                    src='https://upload.wikimedia.org/wikipedia/commons/thumb/1/18/Land_Rover_Range_Rover_Autobiography_2016.jpg/1200px-Land_Rover_Range_Rover_Autobiography_2016.jpg'
                    alt='transport'
                    className='w-[300px]'
                  />
                </div>
              </div>
            </DialogContentText>
          </DialogContent>
        </div>
        <DialogActions>
          <Button onClick={handleClose}>Thoát</Button>
          {/* <Button onClick={handleClose} autoFocus>
            Đồng ý
          </Button> */}
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default TeachingSchedule;

/**
 * Date:
 * Slot
 * Mentor:
 * Course:
 * totalSession:
 */
