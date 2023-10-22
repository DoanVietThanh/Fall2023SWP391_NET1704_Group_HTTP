import Select from '@mui/material/Select';
import * as dayjs from 'dayjs';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { toastError } from '../../../components/Toastify';
import axiosClient from '../../../utils/axiosClient';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { Button, Dialog } from '@mui/material';
import Accordion from '@mui/material/Accordion';
import AccordionDetails from '@mui/material/AccordionDetails';
import AccordionSummary from '@mui/material/AccordionSummary';
import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import Typography from '@mui/material/Typography';
import DialogCheckAttendance from './DialogCheckAttendance';
import { AiFillPlusCircle, AiOutlinePlusCircle } from 'react-icons/ai';
import { BsDashLg } from 'react-icons/bs';
import DialogRegisterSchedule from './DialogRegisterSchedule';

const ScheduleMentor = ({ itemCourse, courseId }) => {
  const { user } = useSelector((state) => state.auth);
  const { accountInfo } = useSelector((state) => state.auth.user);
  const [dataWeek, setDataWeek] = useState();
  const [idSchedule, setIdSchedule] = useState();
  const [slotName, setSlotName] = useState('');
  const [currentWeek, setCurrentWeek] = useState('');
  const [mentorId, setMentorId] = useState('');
  const [isLoading, setIsLoading] = useState(true);
  const [open, setOpen] = useState(false);
  const [openRegisterSchedule, setOpenRegisterSchedule] = useState(false);
  const [openRegister, setOpenRegister] = useState(false);
  const [teachingScheduleId, setTeachingScheduleId] = useState(null);
  const [rollCallBookId, setRollCallBookId] = useState(0);
  const [slotSchedules, setSlotSchedules] = useState();

  useEffect(() => {
    async function getDataCourse() {
      try {
        const response = await axiosClient.get(
          `/staffs/mentors/${accountInfo.staffId}/schedule?courseId=${courseId}`
        );
        console.log('response: ', response);

        setIsLoading(false);
        setMentorId(accountInfo.staffId);
        setDataWeek(response?.data);
        setCurrentWeek(response?.data.data?.weekdays.weekdayScheduleId);
      } catch (error) {
        throw new Error(error);
      }
    }
    getDataCourse();
  }, [isLoading]);

  const handleCloseRegister = () => setOpenRegister(false);

  const handleChange = (event) => {
    setCurrentWeek(event.target.value);
    async function selectedWeek() {
      const response = await axiosClient.get(
        `/staffs/mentors/${accountInfo.staffId}/schedule/filter?weekdayScheduleId=${event.target.value}`
      );
      setDataWeek(response?.data);
    }
    selectedWeek();
  };

  console.log('dataWeek: ', dataWeek);

  const handleSubmitSchedule = () => {
    try {
      async function submitForm() {
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
    <div className='border-t-2'>
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls='panel1a-content'
          id='panel1a-header'
        >
          <Typography>{itemCourse.courseTitle}</Typography>
        </AccordionSummary>
        <AccordionDetails>
          <div className=''>
            <div className='flex justify-end mb-4'>
              <div
                className='bg-blue-400 cursor-pointer flex items-center p-2 rounded-lg gap-2'
                onClick={() => {
                  setOpenRegisterSchedule(true);
                }}
              >
                <AiFillPlusCircle size={24} className='text-white' />
                <p className='font-semibold text-white'>Đăng kí lịch</p>
              </div>
            </div>
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
                        {dataWeek?.data?.filter?.map((item, index) => (
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
                        {dayjs(dataWeek?.data?.weekdays?.monday).format(
                          'DD-MM'
                        )}
                      </p>
                    </div>
                  </th>
                  <th className='capitalize'>
                    <div className='flex flex-col'>
                      Thứ 3{' '}
                      <p className='text-yellow-700'>
                        {dayjs(dataWeek?.data?.weekdays.tuesday).format(
                          'DD-MM'
                        )}
                      </p>
                    </div>
                  </th>
                  <th className='capitalize'>
                    <div className='flex flex-col'>
                      Thứ 4{' '}
                      <p className='text-yellow-700'>
                        {dayjs(dataWeek?.data?.weekdays?.wednesday).format(
                          'DD-MM'
                        )}
                      </p>
                    </div>
                  </th>
                  <th className='capitalize'>
                    <div className='flex flex-col'>
                      Thứ 5{' '}
                      <p className='text-yellow-700'>
                        {dayjs(dataWeek?.data?.weekdays?.thursday).format(
                          'DD-MM'
                        )}
                      </p>
                    </div>
                  </th>
                  <th className='capitalize'>
                    <div className='flex flex-col'>
                      Thứ 6{' '}
                      <p className='text-yellow-700'>
                        {dayjs(dataWeek?.data?.weekdays?.friday).format(
                          'DD-MM'
                        )}
                      </p>
                    </div>
                  </th>
                  <th className='capitalize'>
                    <div className='flex flex-col'>
                      Thứ 7{' '}
                      <p className='text-yellow-700'>
                        {dayjs(dataWeek?.data?.weekdays?.saturday).format(
                          'DD-MM'
                        )}
                      </p>
                    </div>
                  </th>
                  <th className='capitalize'>
                    <div className='flex flex-col'>
                      Chủ nhật{' '}
                      <p className='text-yellow-700'>
                        {dayjs(dataWeek?.data?.weekdays?.sunday).format(
                          'DD-MM'
                        )}
                      </p>
                    </div>
                  </th>
                </tr>
              </thead>
              <tbody>
                {dataWeek?.data?.slotSchedules?.map((item, index) => (
                  <tr key={index}>
                    <td>
                      <div className='font-semibold'>{item.slotName}</div>
                      <h1 className='mb-4 text-white bg-green-600 rounded-lg p-2'>
                        {item.slotDesc}
                      </h1>
                    </td>
                    {item?.teachingSchedules.map((itemDate, indexItemDate) => (
                      <td key={indexItemDate}>
                        {itemDate ? (
                          <div className='text-center'>
                            <p className='font-bold text-blue-800 text-[16px]'>
                              {dataWeek?.data.course.courseTitle}
                            </p>
                            {itemDate?.coursePackage?.coursePackageDesc && (
                              <p className='font-bold text-grey font-bold bg-yellow-400 rounded-lg p-2 text-[16px]'>
                                {itemDate?.coursePackage?.coursePackageDesc}
                              </p>
                            )}

                            <button
                              onClick={() => {
                                setOpen(true);
                                setRollCallBookId(
                                  itemDate?.rollCallBooks[0]?.rollCallBookId
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
                          </div>
                        ) : (
                          <div className='font-bold text-[24px] flex justify-center'>
                            <BsDashLg />
                          </div>
                        )}
                      </td>
                    ))}
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </AccordionDetails>
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
          <div className='mt-4 flex items-center gap-4'>
            <div className='rounded bg-yellow-400 h-[20px] w-[20px]'></div>
            <span>Ngày dạy đã có học viên đăng kí</span>
          </div>
          <ul className='list-disc ml-6 flex flex-col gap-4 mt-4'>
            <li>
              Tổng giờ yêu cầu: {dataWeek?.data?.course?.totalHoursRequired}
            </li>
            <li>Tổng km yêu cầu: {dataWeek?.data?.course?.totalKmRequired}</li>
          </ul>
        </div>
      </Accordion>
      <div>
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

        <DialogCheckAttendance
          open={open}
          setOpen={setOpen}
          rollCallBookId={rollCallBookId}
          teachingScheduleId={teachingScheduleId}
          dataWeek={dataWeek}
          course={dataWeek?.data?.course}
          slotSchedules={slotSchedules}
        />

        <DialogRegisterSchedule
          openRegisterSchedule={openRegisterSchedule}
          setOpenRegisterSchedule={setOpenRegisterSchedule}
          itemCourse={itemCourse}
          courseId={courseId}
        />
      </div>
    </div>
  );
};

export default ScheduleMentor;
