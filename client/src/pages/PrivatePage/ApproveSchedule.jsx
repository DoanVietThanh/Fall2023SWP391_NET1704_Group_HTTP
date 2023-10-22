import { FormControl, InputLabel, MenuItem, Select } from '@mui/material';
import dayjs from 'dayjs';
import React, { useEffect, useState } from 'react';
import { AiOutlinePlusCircle } from 'react-icons/ai';
import { BsDashLg } from 'react-icons/bs';
import { Navigate, useNavigate, useParams } from 'react-router-dom';
import SideBar from '../../components/SideBar';
import { toastError, toastSuccess } from '../../components/Toastify';
import axiosClient from '../../utils/axiosClient';
import DialogApproveSchedule from './components/DialogApproveSchedule';
import { DataGrid } from '@mui/x-data-grid';

const ApproveSchedule = () => {
  const navigate = useNavigate();
  const { idMentor } = useParams();
  const [dataWeek, setDataWeek] = useState();
  const [currentWeek, setCurrentWeek] = useState('');
  const [open, setOpen] = useState(false);

  const [teachingScheduleId, setTeachingScheduleId] = useState(null);
  const [rollCallBookId, setRollCallBookId] = useState(0);
  const [slotSchedules, setSlotSchedules] = useState();
  const [activeVehicles, setActiveVehicles] = useState();
  const [selectedVehicle, setSelectedVehicle] = useState();
  const [mentorScheduleVehicle, setMentorScheduleVehicle] = useState();

  useEffect(() => {
    async function getAwaitSchedule() {
      try {
        const response = await axiosClient.get(
          `/teaching-schedules/mentors/${idMentor}/await-schedule`
        );
        console.log('response: ', response);
        setMentorScheduleVehicle(response?.data.data?.mentorScheduleVehicle);
        setActiveVehicles(response?.data.data?.activeVehicles);
        setDataWeek(response?.data);
        setCurrentWeek(response?.data.data?.weekdays.weekdayScheduleId);
      } catch (error) {
        toastError(error.response?.data.message);
      }
    }
    getAwaitSchedule();
  }, []);

  const handleChange = (event) => {
    setCurrentWeek(event.target.value);
    async function selectedWeek() {
      const response = await axiosClient.get(
        `/staffs/mentors/${idMentor}/schedule/filter?weekdayScheduleId=${event.target.value}`
      );
      setDataWeek(response?.data);
    }
    selectedWeek();
  };

  const columns = [
    { field: 'vehicleId', headerName: 'ID', width: 50 },
    { field: 'vehicleName', headerName: 'Tên xe', width: 150 },
    {
      field: 'vehicleImage',
      headerName: 'Ảnh xe',
      width: 250,
      renderCell: (params) => (
        <div>
          <img
            src='https://static.automotor.vn/images/upload/2022/08/28/bugatti-chay-hang-autonews.jpeg'
            alt=''
            className='w-[100px] h-[100px] object-contain'
          />
        </div>
      ),
    },
    { field: 'vehicleLicensePlate', headerName: 'Biển số xe', width: 150 },
    {
      field: 'registerDate',
      headerName: 'Ngày đăng kí',
      width: 150,
      renderCell: (params) => (
        <div>{dayjs(params.row.registerDate).format('DD/MM/YYYY')}</div>
      ),
    },
    { field: 'isActive', headerName: 'Active', width: 150 },
    {
      field: 'select',
      headerName: 'Chọn',
      width: 150,
      renderCell: (params) => (
        <div>
          <input
            required
            type='radio'
            name='selectVehicle'
            onChange={setSelectedVehicle(params.row.vehicleId)}
          />
        </div>
      ),
    },
  ];

  const submitApprove = async (e) => {
    try {
      e.preventDefault();
      const res = await axiosClient.put(
        `/teaching-schedule/mentors/${idMentor}/approve?vehicleId=${selectedVehicle}`
      );
      toastSuccess(res?.data?.message);
      navigate('/manage-await-schedule');
    } catch (error) {
      toastError(error.response.data.message);
    }
  };

  const submitAvailableApprove = async (availableVehicleID) => {
    console.log('availableVehicleID: ', availableVehicleID);
    try {
      const res = await axiosClient.put(
        `/teaching-schedule/mentors/${idMentor}/approve?vehicleId=${availableVehicleID}`
      );
      toastSuccess(res?.data?.message);
      navigate('/manage-await-schedule');
    } catch (error) {
      toastError(error.response.data.message);
    }
  };

  console.log('mentorScheduleVehicle: ', mentorScheduleVehicle);

  return (
    <div className='flex'>
      <SideBar />
      <div className='flex flex-col mt-[64px] h-[90vh] w-full border rounded overflow-y-auto p-4'>
        <div className='flex-1 w-[100%]'>
          <div>
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

                            <p className='text-red-700'>
                              (Lịch dạy đang chờ duyệt)
                            </p>
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

            {/* Note  */}
            <div className='hidden border p-4 mt-4'>
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
                <li>
                  Tổng km yêu cầu: {dataWeek?.data?.course?.totalKmRequired}
                </li>
              </ul>
            </div>

            {mentorScheduleVehicle ? (
              <div className='flex justify-center mt-4 rounded-lg'>
                <div className='w-[40vw] border-2 p-4'>
                  <h1 className='text-center capitalize font-semibold text-[24px]'>
                    Xe đang dùng
                  </h1>
                  <div className='text-[20px]'>
                    <div>
                      <span className='font-semibold'>Tên xe : </span>
                      {mentorScheduleVehicle?.vehicleName}
                    </div>

                    <div>
                      <span className='font-semibold'>Biển số xe : </span>
                      {mentorScheduleVehicle?.vehicleLicensePlate}
                    </div>
                    <div>
                      <span className='font-semibold'>Ngày đăng kí : </span>
                      {dayjs(mentorScheduleVehicle?.registerDate).format(
                        'DD/MM/YYYY'
                      )}
                    </div>
                    <div>
                      <img
                        src='https://cdnphoto.dantri.com.vn/-nWL80AvkaVfvsn5TpbRwrUGFG4=/2021/08/28/rimacneverafrontquarter-1630129338357.jpg'
                        // src={mentorScheduleVehicle?.vehicleImage}
                        alt=''
                        className=''
                      />
                    </div>
                    <div className='flex justify-end'>
                      <button
                        className='btn mt-4'
                        onClick={() =>
                          submitAvailableApprove(
                            mentorScheduleVehicle?.vehicleId
                          )
                        }
                      >
                        Duyệt Ngay
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            ) : (
              activeVehicles && (
                <div className='my-4 p-4'>
                  <h1 className='text-center capitalize font-semibold text-[24px]'>
                    Chọn xe
                  </h1>
                  <form>
                    <DataGrid
                      className='w-auto'
                      rows={activeVehicles}
                      columns={columns}
                      initialState={{
                        pagination: {
                          paginationModel: { page: 0, pageSize: 5 },
                        },
                      }}
                      density='comfortable'
                      pageSizeOptions={[2]}
                      getRowId={(row) => row.vehicleId}
                    />

                    <div className='flex justify-end'>
                      <button className='btn' onClick={(e) => submitApprove(e)}>
                        Duyệt
                      </button>
                    </div>
                  </form>
                </div>
              )
            )}
          </div>
        </div>

        <DialogApproveSchedule
          open={open}
          setOpen={setOpen}
          rollCallBookId={rollCallBookId}
          teachingScheduleId={teachingScheduleId}
          dataWeek={dataWeek}
          course={dataWeek?.data?.course}
          slotSchedules={slotSchedules}
          activeVehicles={activeVehicles}
        />
      </div>
    </div>
  );
};

export default ApproveSchedule;
