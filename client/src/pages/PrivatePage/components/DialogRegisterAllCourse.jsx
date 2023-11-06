import { TextField } from '@mui/material';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import { useCallback, useEffect, useState } from 'react';
import axiosClient from '../../../utils/axiosClient';
import { toastError, toastSuccess } from '../../../components/Toastify';

export default function DialogRegisterAllCourse({
  open,
  setOpen,
  slots,
  weekdays,
  courseId,
  mentorId,
  setDataWeek,
}) {
  // {
  //     "courseId": "c91a27ee-ab5a-473b-a5b5-5fce7652c50e",
  //     "mentorId": "9f2db427-5a6f-4313-98f8-35287820f903",
  //     "slotId": 1,
  //     "weekdays": "2,4,6"
  //   }

  console.log('slots: ', slots);
  console.log('weekdays: ', weekdays);
  console.log({
    slots,
    weekdays,
    courseId,
    mentorId,
  });
  const [comboDate, setComboDate] = useState('');
  const [selectedSlot, setSelectedSlot] = useState('');

  const [formData, setFormData] = useState({
    courseId,
    mentorId,
    slotId: '',
    weekdays: '',
  });

  useEffect(() => {
    setFormData((prev) => ({
      ...prev,
      slotId: slots[0].slotId,
      weekdays: weekdays[0],
    }));
  }, []);

  const handleSubmitRegisterAllCourse = async () => {
    console.log(formData);
    await axiosClient
      .post(`/staffs/mentors/schedule-register/range`, formData)
      .then((res) => {
        console.log(res);
        setDataWeek(res?.data);
        setOpen(false);
        toastSuccess(res?.data?.message);
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };

  console.log('slots: ', slots);
  console.log('weekdays: ', weekdays);
  console.log('comboDate: ', comboDate);
  console.log('formData: ', formData);
  return (
    <Dialog
      open={open}
      onClose={() => setOpen(false)}
      aria-labelledby='alert-dialog-title'
      aria-describedby='alert-dialog-description'
      fullWidth
      maxWidth='md'
    >
      <DialogTitle id='alert-dialog-title'>
        <div className='text-center capitalize font-medium'>
          Đăng kí lịch dạy cả khóa
        </div>
      </DialogTitle>
      <DialogContent>
        <div className='p-4 min-w-[36vw]'>
          <div className='flex flex-col gap-4'>
            {weekdays && slots && (
              <div className='mt-4 flex justify-between'>
                <label htmlFor='#time'>Chọn buổi dạy trong tuần</label>
                <select
                  id='time'
                  value={formData.weekdays}
                  onChange={(e) =>
                    setFormData((prev) => ({
                      ...prev,
                      weekdays: e.target.value,
                    }))
                  }
                >
                  {weekdays?.map((item, index) => (
                    <option value={item} key={item}>
                      {item}
                    </option>
                  ))}
                </select>
              </div>
            )}
            {slots && weekdays && (
              <div className='mt-4 flex justify-between'>
                <label htmlFor='#time'>Chọn thời gian</label>
                <select
                  id='time'
                  value={formData.slotId}
                  onChange={(e) =>
                    setFormData((prev) => ({ ...prev, slotId: e.target.value }))
                  }
                >
                  {slots?.map((slotItem, index) => (
                    <option value={slotItem?.slotId} key={slotItem?.slotId}>
                      {slotItem?.slotName} {slotItem?.slotDesc}
                    </option>
                  ))}
                </select>
              </div>
            )}
          </div>
        </div>
      </DialogContent>
      <DialogActions>
        <Button onClick={() => setOpen(false)}>Hủy</Button>
        <Button onClick={handleSubmitRegisterAllCourse} autoFocus>
          Đăng kí lịch
        </Button>
      </DialogActions>
    </Dialog>
  );
}
