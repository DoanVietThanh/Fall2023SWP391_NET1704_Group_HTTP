import { Box } from '@mui/material';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import * as dayjs from 'dayjs';
import React, { useEffect, useState } from 'react';
import {
  AiOutlineCheckCircle,
  AiOutlineCloseCircle,
  AiOutlinePlusCircle,
} from 'react-icons/ai';

import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import SideBar from '../../components/SideBar';
import axiosClient from './../../utils/axiosClient';
import { toastError, toastSuccess } from '../../components/Toastify';
import Loading from './../../components/Loading';
import DialogDetailSchedule from './components/DialogDetailSchedule';
import DialogCheckAttendance from './components/DialogCheckAttendance';
import { BsDashLg } from 'react-icons/bs';

const WeekSchedule = () => {
  const navigate = useNavigate();
  const { user } = useSelector((state) => state.auth);
  const [dataWeek, setDataWeek] = useState();
  const [idSchedule, setIdSchedule] = useState();
  const [slotName, setSlotName] = useState('');
  const [currentWeek, setCurrentWeek] = useState('');
  const [mentorId, setMentorId] = useState('');
  const [isLoading, setIsLoading] = useState(true);
  const [open, setOpen] = useState(false);
  const [openRegister, setOpenRegister] = useState(false);
  const [dialogDetail, setDialogDetail] = useState();

  const [teachingScheduleId, setTeachingScheduleId] = useState(null);
  const [rollCallBookId, setRollCallBookId] = useState(0);
  const [slotSchedules, setSlotSchedules] = useState();

  useEffect(() => {
    async function getDataCourse() {
      try {
        const response = await axiosClient.get(
          `/members/${user.accountInfo.memberId}/schedule`
        );
        console.log('response: ', response);
        setIsLoading(false);
        setMentorId(response?.data.data.mentor.staffId);
        setDataWeek(response?.data);
        setCurrentWeek(response?.data.data?.weekdays.weekdayScheduleId);
      } catch (error) {
        throw new Error(error);
      }
    }
    getDataCourse();
  }, [isLoading]);

  const handleClickOpen = async (teachingScheduleId) => {
    const dataTeachingSchedule = await axiosClient.get(
      `/teaching-schedules/${teachingScheduleId}`
    );
    setDialogDetail(dataTeachingSchedule?.data);
    setOpen(true);
  };

  const handleClose = () => setOpen(false);

  const handleCloseRegister = () => setOpenRegister(false);

  const handleChange = (event) => {
    setCurrentWeek(event.target.value);
    async function selectedWeek() {
      const response = await axiosClient.get(
        `/members/${user.accountInfo.memberId}/schedule/filter?weekdayScheduleId=${event.target.value}`
      );
      console.log('response filter:', response);
      setDataWeek(response?.data);
    }
    selectedWeek();
  };

  const handleClickOpenRegister = (teachingScheduleId) => {
    setIdSchedule(teachingScheduleId);
    setOpenRegister(true);
  };

  const handleSubmitSchedule = () => {
    try {
      async function submitForm() {
        //setIsLoading(true);
        const response = await axiosClient
          .post(`/members/schedule`, {
            memberId: user?.accountInfo.memberId,
            teachingScheduleId: idSchedule,
          })
          .catch((e) => {
            console.log(e);
            toastError(e.response.data.message);
            setOpenRegister(false);
          });
        if (response?.data.statusCode === 200) {
          setIsLoading(!isLoading);
          setOpenRegister(false);
          toastSuccess(response.data.message);
        }
      }
      submitForm();
    } catch (error) {
      console.log(error);
    }
  };

  console.log('dataWeek: ', dataWeek);

  return (
    <div className='flex'>
      <SideBar />

      {isLoading ? (
        <Loading />
      ) : (
        <>
          {dataWeek && (
            <>
              <Box
                component='main'
                sx={{ flexGrow: 1, p: 3 }}
                className='overflow-y-auto'
              >
                <div className='w-full rounded overflow-y-auto mt-[64px] overflow-y-auto'>
                  <div>
                    <h1 className='font-bold text-[30px] uppercase mb-4'>
                      Lịch học theo tuần
                    </h1>
                    <div className=''>
                      <table className='w-full border-2 border-black'>
                        <thead>
                          <tr>
                            <th className='w-[8%]'>
                              <FormControl fullWidth>
                                <InputLabel id='demo-simple-select-label'>
                                  Tuần
                                </InputLabel>
                                <Select
                                  labelId='demo-simple-select-label'
                                  id='demo-simple-select'
                                  value={currentWeek}
                                  label='Tuần'
                                  onChange={handleChange}
                                >
                                  {dataWeek.data.filter?.map((item, index) => (
                                    <MenuItem value={item.id} key={index}>
                                      {item.desc}
                                    </MenuItem>
                                  ))}
                                </Select>
                              </FormControl>
                            </th>
                            <th className='capitalize'>
                              <div className='flex flex-col'>
                                Thứ 2{' '}
                                <p className='text-yellow-700'>
                                  {dayjs(
                                    dataWeek?.data?.weekdays?.monday
                                  ).format('DD-MM')}
                                </p>
                              </div>
                            </th>
                            <th className='capitalize'>
                              <div className='flex flex-col'>
                                Thứ 3{' '}
                                <p className='text-yellow-700'>
                                  {dayjs(
                                    dataWeek?.data?.weekdays.tuesday
                                  ).format('DD-MM')}
                                </p>
                              </div>
                            </th>
                            <th className='capitalize'>
                              <div className='flex flex-col'>
                                Thứ 4{' '}
                                <p className='text-yellow-700'>
                                  {dayjs(
                                    dataWeek?.data?.weekdays?.wednesday
                                  ).format('DD-MM')}
                                </p>
                              </div>
                            </th>
                            <th className='capitalize'>
                              <div className='flex flex-col'>
                                Thứ 5{' '}
                                <p className='text-yellow-700'>
                                  {dayjs(
                                    dataWeek?.data?.weekdays?.thursday
                                  ).format('DD-MM')}
                                </p>
                              </div>
                            </th>
                            <th className='capitalize'>
                              <div className='flex flex-col'>
                                Thứ 6{' '}
                                <p className='text-yellow-700'>
                                  {dayjs(
                                    dataWeek?.data?.weekdays?.friday
                                  ).format('DD-MM')}
                                </p>
                              </div>
                            </th>
                            <th className='capitalize'>
                              <div className='flex flex-col'>
                                Thứ 7{' '}
                                <p className='text-yellow-700'>
                                  {dayjs(
                                    dataWeek?.data?.weekdays?.saturday
                                  ).format('DD-MM')}
                                </p>
                              </div>
                            </th>
                            <th className='capitalize'>
                              <div className='flex flex-col'>
                                Chủ nhật{' '}
                                <p className='text-yellow-700'>
                                  {dayjs(
                                    dataWeek?.data?.weekdays?.sunday
                                  ).format('DD-MM')}
                                </p>
                              </div>
                            </th>
                          </tr>
                        </thead>
                        <tbody>
                          {dataWeek?.data?.slotSchedules?.map((item, index) => (
                            <tr key={index}>
                              <td>
                                <div className='font-semibold'>
                                  {item.slotName}
                                </div>
                                <h1 className='mb-4 text-white bg-green-600 rounded-lg p-2'>
                                  {item.slotDesc}
                                </h1>
                              </td>
                              {item?.teachingSchedules.map(
                                (itemDate, indexItemDate) => (
                                  <td key={indexItemDate}>
                                    {itemDate ? (
                                      <div>
                                        {itemDate.rollCallBooks.length === 0 ? (
                                          <div className='flex justify-center items-center gap-4'>
                                            <AiOutlinePlusCircle
                                              size={24}
                                              className='text-green-700 cursor-pointer'
                                              onClick={() => {
                                                handleClickOpenRegister(
                                                  itemDate?.teachingScheduleId
                                                );
                                                setSlotName(item?.slotName);
                                              }}
                                            />
                                            0/1
                                          </div>
                                        ) : itemDate.rollCallBooks[0]
                                            .memberId ===
                                          user.accountInfo.memberId ? (
                                          <div className='text-center font-semibold'>
                                            <p className='font-bold text-blue-800 text-[16px]'>
                                              {
                                                dataWeek?.data.course
                                                  .courseTitle
                                              }
                                            </p>
                                            {itemDate?.coursePackage
                                              ?.coursePackageDesc && (
                                              <p className='font-bold text-grey font-bold bg-yellow-400 rounded-lg p-2 text-[16px]'>
                                                {
                                                  itemDate?.coursePackage
                                                    ?.coursePackageDesc
                                                }
                                              </p>
                                            )}
                                            <button
                                              onClick={() => {
                                                handleClickOpen(
                                                  itemDate?.teachingScheduleId
                                                );
                                                setRollCallBookId(
                                                  itemDate?.rollCallBooks[0]
                                                    ?.rollCallBookId
                                                );
                                                setTeachingScheduleId(
                                                  itemDate?.teachingScheduleId
                                                );
                                                setSlotSchedules(item);
                                              }}
                                              className='px-2 text-black bg-gray-200 mt-2 rounded-lg hover:bg-blue-400 hover:text-white'
                                            >
                                              Xem chi tiết
                                            </button>
                                            {dataWeek?.data
                                              ?.registeredSession &&
                                              dataWeek?.data
                                                ?.packageTotalSession && (
                                                <div className='mt-4 text-red-600 w-auto rounded-lg mx-6 text-[14px]'>
                                                  {`Đã đăng kí ${dataWeek?.data?.registeredSession} / ${dataWeek?.data?.packageTotalSession} buổi`}
                                                </div>
                                              )}
                                          </div>
                                        ) : (
                                          <div className='font-bold text-[24px] flex justify-center'>
                                            {/* Hết chỗ 1/1 */} <BsDashLg />
                                          </div>
                                        )}
                                      </div>
                                    ) : (
                                      <div className='font-bold text-[24px] flex justify-center'>
                                        <BsDashLg />
                                      </div>
                                    )}
                                  </td>
                                )
                              )}
                            </tr>
                          ))}
                        </tbody>
                      </table>
                    </div>
                  </div>
                </div>

                <div className='border p-4 mt-4'>
                  <div className='font-bold'>Chú thích: </div>

                  <p className='flex items-center mt-4'>
                    <AiOutlinePlusCircle size={24} className='text-green-700' />
                    <span> : Ấn để đăng kí buổi học</span>
                  </p>
                  <p className='flex items-center mt-4'>
                    <BsDashLg size={24} className='text-black' />
                    <span> : Chưa có lịch</span>
                  </p>
                  <ul className='list-disc ml-6 flex flex-col gap-4 mt-4'>
                    <li>
                      Tổng giờ yêu cầu:{' '}
                      {dataWeek?.data?.course?.totalHoursRequired}
                    </li>
                    <li>
                      Tổng km yêu cầu: {dataWeek?.data?.course?.totalKmRequired}
                    </li>
                  </ul>
                </div>
              </Box>

              <Dialog
                open={openRegister}
                onClose={handleCloseRegister}
                aria-labelledby='alert-dialog-title'
                aria-describedby='alert-dialog-description'
              >
                <div className='p-8'>
                  <div className='m-8'>
                    <h2 className='font-bold my-4 text-center font-bold text-[24px]'>
                      Chi tiết Lịch học
                    </h2>
                    <div className='flex flex-col gap-4'>
                      <div className='flex gap-4'>
                        <div>Date:</div>
                        <div>
                          {dayjs(dataWeek?.data?.weekdays?.monday).format(
                            'DD/MM/YYYY'
                          )}
                        </div>
                      </div>
                      <div className='flex gap-4'>
                        <div>Tiết học:</div>
                        <div> {slotName} </div>
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
                    </div>
                  </div>

                  <div className='flex justify-between gap-8 px-2 '>
                    <Button onClick={handleCloseRegister}>Thoát</Button>
                    <button className='btn' onClick={handleSubmitSchedule}>
                      Xác nhận Đăng kí
                    </button>
                  </div>
                </div>
              </Dialog>

              <DialogDetailSchedule
                open={open}
                setOpen={setOpen}
                rollCallBookId={rollCallBookId}
                teachingScheduleId={teachingScheduleId}
                dataWeek={dataWeek}
                course={dataWeek?.data?.course}
                slotSchedules={slotSchedules}
              />
            </>
          )}
        </>
      )}
    </div>
  );
};

export default WeekSchedule;
