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
  const [openRegister, setOpenRegister] = useState(false);
  const [teachingScheduleId, setTeachingScheduleId] = useState(null);
  const [rollCallBookId, setRollCallBookId] = useState(0);

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
                    <td>{item.slotName}</td>
                    {item?.teachingSchedules.map((itemDate, indexItemDate) => (
                      <td key={indexItemDate}>
                        {itemDate ? (
                          <div className='text-center'>
                            <p className='font-bold text-blue-800 text-[14px]'>
                              {dataWeek?.data.course.courseTitle}
                            </p>
                            {itemDate?.coursePackage?.coursePackageDesc && (
                              <p className='font-bold text-grey font-bold bg-yellow-400 rounded-lg p-2 text-[12px]'>
                                {itemDate?.coursePackage?.coursePackageDesc}
                              </p>
                            )}
                            <div className='bg-green-500 text-white w-auto rounded-lg mx-6 text-[14px]'>
                              {item.slotDesc}
                            </div>
                            <button
                              onClick={() => {
                                setOpen(true);
                                setRollCallBookId(
                                  itemDate?.rollCallBooks[0]?.rollCallBookId
                                );
                                setTeachingScheduleId(
                                  itemDate?.teachingScheduleId
                                );
                              }}
                              className='px-2 text-black bg-gray-200 mt-2 rounded-lg hover:bg-blue-400 hover:text-white'
                            >
                              Xem chi tiết
                            </button>
                          </div>
                        ) : (
                          <div></div>
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
          <div>
            Tổng giờ yêu cầu: {dataWeek?.data?.course?.totalHoursRequired}
          </div>
          <div>Tổng km yêu cầu: {dataWeek?.data?.course?.totalKmRequired}</div>
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
        />
      </div>
    </div>
  );
};

export default ScheduleMentor;
