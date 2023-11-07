import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { toastError, toastSuccess } from '../../../components/Toastify';
import axiosClient from '../../../utils/axiosClient';

const DialogRegisterSchedule = ({
  courseId,
  itemCourse,
  openRegisterSchedule,
  setOpenRegisterSchedule,
  setDataWeek,
}) => {
  const { user } = useSelector((state) => state.auth);
  const { accountInfo } = useSelector((state) => state.auth.user);
  const [teachingDate, setTeachingDate] = useState('');
  const [selectedSlot, setSelectedSlot] = useState('');
  const [listSlots, setListSlots] = useState('');
  console.log('accountInfo: ', accountInfo);
  console.log('courseId: ', courseId);

  useEffect(() => {
    async function getDateSlots() {
      const res = await axiosClient.get(
        `/staffs/mentors/${accountInfo?.staffId}/schedule-register`
      );
      setListSlots(res?.data?.data?.slots);
    }
    getDateSlots();
  }, []);

  const handleSubmitRegister = async (e) => {
    e.preventDefault();
    console.log({
      teachingDate,
      mentorId: accountInfo?.staffId,
      courseId: courseId,
      slotId: selectedSlot,
    });
    try {
      const res = await axiosClient
        .post(`/staffs/mentors/schedule-register`, {
          teachingDate,
          mentorId: accountInfo?.staffId,
          courseId: courseId,
          slotId: selectedSlot,
        })
        .then((res) => {
          console.log(res);
          toastSuccess('Đăng kí thành công');
          setDataWeek(res?.data);
        })
        .catch((error) =>{
          console.log("ERROR", error);
          toastError(error?.response?.data?.message);
        });
      console.log(res);
      //   toastSuccess(res?.data?.data?.message);
      
      setOpenRegisterSchedule(false);
    } catch (error) {
      console.log(error);
      // toastError(error?.response?.data?.message);
      setOpenRegisterSchedule(false);
    }
  };

  return (
    <div>
      <Dialog
        open={openRegisterSchedule}
        onClose={() => setOpenRegisterSchedule(false)}
        aria-labelledby='alert-dialog-title'
        aria-describedby='alert-dialog-description'
      >
        <form action=''>
          <div className='p-4 min-w-[36vw]'>
            <h1 className='text-center font-bold text-[24px]'>
              Đăng kí lịch dạy
            </h1>
            <div className='flex flex-col gap-4'>
              <div className='mt-4 flex justify-between'>
                <label htmlFor='#data'>Chọn ngày dạy</label>
                <input
                  id='date'
                  type='date'
                  className='px-2'
                  onChange={(e) => setTeachingDate(e.target.value)}
                />
              </div>
              {listSlots && (
                <div className='mt-4 flex justify-between'>
                  <label htmlFor='#time'>Chọn thời gian</label>
                  <select
                    id='time'
                    value={selectedSlot || ''}
                    onChange={(e) => setSelectedSlot(e.target.value)}
                  >
                    {listSlots?.map((slotItem, index) => (
                      <option value={slotItem?.slotId} key={slotItem?.slotId}>
                        {slotItem?.slotName} {slotItem?.slotDesc}
                      </option>
                    ))}
                  </select>
                </div>
              )}
            </div>
          </div>
          <DialogActions>
            <Button onClick={() => setOpenRegisterSchedule(false)}>Hủy</Button>
            <Button onClick={(e) => handleSubmitRegister(e)} autoFocus>
              Đăng kí
            </Button>
          </DialogActions>
        </form>
      </Dialog>
    </div>
  );
};

export default DialogRegisterSchedule;

// {
//     "teachingDate": "2023-10-23",
//     "mentorId": "d1b02d31-da3f-42c7-a317-7045b94824b7",
//     "courseId":"1d115018-d16d-4834-9e56-6d6925658666",
//     "slotId": 1
//   }
