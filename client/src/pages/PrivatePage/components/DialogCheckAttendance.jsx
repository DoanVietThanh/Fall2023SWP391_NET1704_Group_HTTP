import { Dialog, Switch, TextField } from '@mui/material';
import React, { useEffect, useState } from 'react';
import axiosClient from '../../../utils/axiosClient';
import { useSelector } from 'react-redux';
import { toastError, toastSuccess } from '../../../components/Toastify';
import { AiOutlineCheckCircle, AiOutlineCloseCircle } from 'react-icons/ai';
import * as dayjs from 'dayjs';

const DialogCheckAttendance = ({
  open,
  course,
  setOpen,
  dataWeek,
  rollCallBookId,
  teachingScheduleId,
}) => {
  const { accountInfo } = useSelector((state) => state.auth.user);
  const [memberInfo, setMemberInfo] = useState();
  const [vehicle, setVehicle] = useState();
  const [formAttendance, setFormAttendance] = useState({
    comment: '',
    isAbsence: false,
    totalHoursDriven: 0,
    totalKmDriven: 0,
  });

  console.log('course: ', course);
  console.log('dataWeek: ', dataWeek);
  console.log('vehicle: ', vehicle);

  useEffect(() => {
    async function initialCheckAttendance() {
      if (teachingScheduleId) {
        const response = await axiosClient.get(
          `/teaching-schedules/${teachingScheduleId}`
        );
        setVehicle(response?.data?.data?.teachingSchedule.vehicle);
        setMemberInfo(
          response?.data?.data?.teachingSchedule?.rollCallBooks[0]?.member
        );
        setFormAttendance({
          comment:
            response?.data?.data?.teachingSchedule.rollCallBooks[0].comment,
          isAbsence:
            response?.data?.data?.teachingSchedule.rollCallBooks[0].isAbsence,
          totalHoursDriven:
            response?.data?.data?.teachingSchedule.rollCallBooks[0]
              .totalHoursDriven,
          totalKmDriven:
            response?.data?.data?.teachingSchedule.rollCallBooks[0]
              .totalKmDriven,
        });
      }
    }
    initialCheckAttendance();
  }, [teachingScheduleId]);

  const handleCheckAttendance = async (e) => {
    e.preventDefault();
    try {
      const res = await axiosClient.put(
        `/staffs/mentors/${accountInfo.staffId}/schedule/rollcallbook/${rollCallBookId}`,
        formAttendance
      );
      console.log('res: ', res);
      if (res?.data.statusCode === 200) {
        toastSuccess(res?.data.message);
      }
    } catch (error) {
      toastError(error.response.data.message);
    }
    setOpen(false);
  };

  return (
    <div>
      <Dialog
        open={open}
        onClose={() => setOpen(false)}
        aria-labelledby='alert-dialog-title'
        aria-describedby='alert-dialog-description'
      >
        <div className='flex gap-10 p-6'>
          <div>
            <h2 className='font-bold my-4 text-center font-bold text-[24px]'>
              Chi tiết Lịch học
            </h2>
            <div className='flex flex-col gap-4'>
              <div>
                <span className='font-semibold'>Tên khóa học: </span>
                {course?.courseTitle}
              </div>
              <div>
                <span className='font-semibold'>Ngày bắt đầu: </span>
                {dayjs(course?.startDate).format('DD/MM/YYYY')}
              </div>
              <div>
                <span className='font-semibold'>Tiết học: </span>
                {dayjs(course?.startDate).format('DD/MM/YYYY')}
              </div>
              <div>
                <span className='font-semibold'>Thời gian học: </span>
                {course?.totalMonth} tháng
              </div>
              <div>
                <span className='font-semibold'>Tổng giờ yêu cầu: </span>
                {course?.totalHoursRequired} giờ
              </div>
              <div>
                <span className='font-semibold'>Tổng km yêu cầu: </span>
                {course?.totalKmRequired} km
              </div>
            </div>
          </div>

          <div>
            <h2 className='font-bold my-4 text-center font-bold text-[24px]'>
              Phương tiện
            </h2>
            <div className='flex flex-col gap-4'>
              <div>
                <span className='font-semibold'>Tên xe: </span>
                {vehicle?.vehicleName}
              </div>
              <div>
                <span className='font-semibold'>Biển số xe: </span>
                {vehicle?.vehicleLicensePlate}
              </div>
              <div>
                <span className='font-semibold'>Ngày đăng kí xe: </span>
                {dayjs(vehicle?.registerDate).format('DD/MM/YYYY')}
              </div>
              <div className='w-full'>
                <img
                  src='https://upload.wikimedia.org/wikipedia/commons/thumb/1/18/Land_Rover_Range_Rover_Autobiography_2016.jpg/1200px-Land_Rover_Range_Rover_Autobiography_2016.jpg'
                  alt='transport'
                  className='w-[300px]'
                />
              </div>
            </div>
          </div>
        </div>
        <div className='p-4 border-t-2 m-2 '>
          <div className='p-4 border-4 mb-4'>
            <h1 className='font-semibold text-center capitalize text-[24px] mb-4'>
              Điểm danh học viên
            </h1>
            <div>
              <span className='font-semibold'>Họ tên:</span>
              {` ${memberInfo?.firstName} ${memberInfo?.lastName}`}
            </div>
            <div>
              <span className='font-semibold'>Email:</span>
              {` ${memberInfo?.email}`}
            </div>
            <div>
              <span className='font-semibold'>Số điện thoại:</span>
              {` ${memberInfo?.phone}`}
            </div>
          </div>
          <form action=''>
            <TextField
              id='outlined-basic'
              label='Nhận xét'
              variant='outlined'
              className='w-full'
              required={true}
              value={formAttendance?.comment}
              onChange={(e) =>
                setFormAttendance((prev) => ({
                  ...prev,
                  comment: e.target.value,
                }))
              }
            />
            <div className='flex justify-end'>
              <div>
                <label
                  htmlFor='#isAbsence'
                  className={`font-semibold text-yellow-700 ${
                    formAttendance?.isAbsence
                      ? 'text-red-700'
                      : 'text-green-700'
                  }`}
                >
                  {formAttendance?.isAbsence ? 'Vắng' : 'Có mặt'}
                </label>
                <Switch
                  id='isAbsence'
                  checked={!formAttendance?.isAbsence}
                  onChange={(e) =>
                    setFormAttendance((prev) => ({
                      ...prev,
                      isAbsence: !e.target.checked,
                    }))
                  }
                  size='medium'
                />
              </div>
            </div>

            {!formAttendance?.isAbsence && (
              <div>
                <div className=''>
                  <TextField
                    id='outlined-basic'
                    label='Tổng quãng đường đi được'
                    variant='outlined'
                    className='w-full mt-4'
                    type='number'
                    value={formAttendance?.totalKmDriven}
                    InputProps={{
                      inputProps: { min: 0 }, // Set the minimum value here
                    }}
                    onChange={(e) =>
                      setFormAttendance((prev) => ({
                        ...prev,
                        totalKmDriven: e.target.value,
                      }))
                    }
                  />
                </div>
                <div className='mt-4'>
                  <TextField
                    id='outlined-basic'
                    label='Thời gian đã đi'
                    variant='outlined'
                    className='w-full mt-4'
                    type='number'
                    value={formAttendance?.totalHoursDriven}
                    InputProps={{
                      inputProps: { min: 0 }, // Set the minimum value here
                    }}
                    onChange={(e) =>
                      setFormAttendance((prev) => ({
                        ...prev,
                        totalHoursDriven: e.target.value,
                      }))
                    }
                  />
                </div>
              </div>
            )}

            <div className='flex justify-end mt-4'>
              <button
                className='btn'
                type='submit'
                onClick={(e) => handleCheckAttendance(e)}
              >
                Điểm danh
              </button>
            </div>
          </form>
        </div>
      </Dialog>
    </div>
  );
};

export default DialogCheckAttendance;