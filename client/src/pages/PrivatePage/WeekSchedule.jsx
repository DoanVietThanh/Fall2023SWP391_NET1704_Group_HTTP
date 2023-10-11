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
import { toastError } from '../../components/Toastify';
import Loading from './../../components/Loading';

const WeekSchedule = () => {
  const navigate = useNavigate();
  const { user } = useSelector((state) => state.auth);
  const [dataWeek, setDataWeek] = useState();
  const [idSchedule, setIdSchedule] = useState();
  const [slotName, setSlotName] = useState('');
  const [currentWeek, setCurrentWeek] = useState('');
  const [mentorId, setMentorId] = useState('');
  const [isLoading, setIsLoading] = useState(true);

  const handleChange = (event) => {
    setCurrentWeek(event.target.value);
    async function selectedWeek() {
      const response = await axiosClient.get(
        `/members/${user.accountInfo.memberId}/schedule/filter?weekdayScheduleId=${event.target.value}`
      );
      setDataWeek(response?.data);
    }
    selectedWeek();
  };

  const [open, setOpen] = useState(false);
  const handleClickOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  const [openRegister, setOpenRegister] = useState(false);
  const handleClickOpenRegister = (teachingScheduleId) => {
    setIdSchedule(teachingScheduleId);
    setOpenRegister(true);
  };
  const handleCloseRegister = () => setOpenRegister(false);

  useEffect(() => {
    async function getDataCourse() {
      try {
        const response = await axiosClient.get(
          `/members/${user.accountInfo.memberId}/schedule`
        );
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

  const handleSubmitSchedule = () => {
    try {
      async function submitForm() {
        setIsLoading(true);
        const response = await axiosClient.post(`/members/schedule`, {
          memberId: user?.accountInfo.memberId,
          teachingScheduleId: idSchedule,
        });
        if (response?.data.statusCode === 200) {
          setIsLoading(!isLoading);
          setOpenRegister(false);
        } else {
          toastError('Ngày học đã đăng kí');
        }
      }
      submitForm();
    } catch (error) {
      throw new Error(error);
    }
  };

  return (
    <div className='flex'>
      <SideBar />

      {isLoading ? (
        <Loading />
      ) : (
        <>
          {dataWeek && (
            <>
              <Box component='main' sx={{ flexGrow: 1, p: 3 }}>
                <div className='h-[80vh] w-full rounded overflow-y-auto mt-[64px]'>
                  <div>
                    <h1 className='font-bold text-[30px] uppercase mb-4'>
                      Lịch học theo tuần
                    </h1>
                    <div className=' min-h-[70vh]'>
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
                                  {dataWeek?.data.filter.map((item, index) => (
                                    <MenuItem value={item.id}>
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
                                    dataWeek?.data.weekdays.tuesday
                                  ).format('DD-MM')}
                                </p>
                              </div>
                            </th>
                            <th className='capitalize'>
                              <div className='flex flex-col'>
                                Thứ 4{' '}
                                <p className='text-yellow-700'>
                                  {dayjs(
                                    dataWeek?.data.weekdays.wednesday
                                  ).format('DD-MM')}
                                </p>
                              </div>
                            </th>
                            <th className='capitalize'>
                              <div className='flex flex-col'>
                                Thứ 5{' '}
                                <p className='text-yellow-700'>
                                  {dayjs(
                                    dataWeek?.data.weekdays.thursday
                                  ).format('DD-MM')}
                                </p>
                              </div>
                            </th>
                            <th className='capitalize'>
                              <div className='flex flex-col'>
                                Thứ 6{' '}
                                <p className='text-yellow-700'>
                                  {dayjs(dataWeek?.data.weekdays.friday).format(
                                    'DD-MM'
                                  )}
                                </p>
                              </div>
                            </th>
                            <th className='capitalize'>
                              <div className='flex flex-col'>
                                Thứ 7{' '}
                                <p className='text-yellow-700'>
                                  {dayjs(
                                    dataWeek?.data.weekdays.saturday
                                  ).format('DD-MM')}
                                </p>
                              </div>
                            </th>
                            <th className='capitalize'>
                              <div className='flex flex-col'>
                                Chủ nhật{' '}
                                <p className='text-yellow-700'>
                                  {dayjs(dataWeek?.data.weekdays.sunday).format(
                                    'DD-MM'
                                  )}
                                </p>
                              </div>
                            </th>
                          </tr>
                        </thead>
                        <tbody>
                          {dataWeek?.data.slotSchedules.map((item, index) => (
                            <tr key={index}>
                              <td>{item.slotName}</td>
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
                                          <div className='text-center'>
                                            <p className='font-bold text-blue-800 text-[14px]'>
                                              {
                                                dataWeek?.data.course
                                                  .courseTitle
                                              }
                                            </p>
                                            <div className='bg-green-500 text-white w-auto rounded-lg mx-6 text-[14px]'>
                                              {item.slotDesc}
                                            </div>
                                            <button
                                              onClick={handleClickOpen}
                                              className='px-2 text-black bg-gray-200 mt-2 rounded-lg hover:bg-blue-400 hover:text-white'
                                            >
                                              Xem chi tiết
                                            </button>
                                          </div>
                                        ) : (
                                          <div className='text-red-700'>
                                            Hết chỗ 1/1
                                          </div>
                                        )}
                                      </div>
                                    ) : (
                                      <div></div>
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
                          {dayjs(dataWeek?.data.weekdays.monday).format(
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
              <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby='alert-dialog-title'
                aria-describedby='alert-dialog-description'
              >
                <div className='flex gap-10 p-6'>
                  <div>
                    <h2 className='font-bold my-4 text-center font-bold text-[24px]'>
                      Chi tiết Lịch học
                    </h2>
                    <div className='flex flex-col gap-4'>
                      <div className='flex gap-4'>
                        <div>Date:</div>
                        <div>
                          {dayjs(dataWeek?.data.weekdays.monday).format(
                            'DD/MM/YYYY'
                          )}
                        </div>
                      </div>
                      <div className='flex gap-4'>
                        <div>Tiết học:</div>
                        <div> {slotName} </div>
                      </div>
                      <div className='flex gap-4'>
                        <div>Giảng viên:</div>
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
                  </div>

                  <div>
                    <h2 className='font-bold my-4 text-center font-bold text-[24px]'>
                      Phương tiện
                    </h2>
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
                  </div>
                </div>
                <DialogActions>
                  <Button onClick={handleClose}>Thoát</Button>
                  {/* <Button onClick={handleClose} autoFocus>
            Đồng ý
          </Button> */}
                </DialogActions>
              </Dialog>
            </>
          )}
        </>
      )}
    </div>
  );
};

export default WeekSchedule;
