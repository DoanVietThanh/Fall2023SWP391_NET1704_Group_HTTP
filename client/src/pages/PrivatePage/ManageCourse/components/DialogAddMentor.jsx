import { FormControl, InputLabel, MenuItem, Select } from '@mui/material';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import * as dayjs from 'dayjs';
import { useEffect, useState } from 'react';
import { toastError, toastSuccess } from '../../../../components/Toastify';
import axiosClient from '../../../../utils/axiosClient';
import axiosForm from '../../../../utils/axiosForm';

export default function DialogAddMentor({ open, setOpen, selectedIdCourse }) {
  const [listMentor, setListMentor] = useState([]);
  const [listUnAddMentor, setListUnAddMentor] = useState([]);
  const [openAddMentorForm, setOpenAddMentorForm] = useState(false);
  const [selectedMentor, setSelectedMentor] = useState('');

  useEffect(() => {
    async function getDataCourse() {
      await axiosClient
        .get(`/courses/${selectedIdCourse}`)
        .then((res) => {
          setListMentor(res?.data?.data?.course?.mentors);
        })
        .catch((error) => toastError(error?.respose?.data?.message));
    }
    getDataCourse();
  }, []);

  const getListUnAddMentor = async () => {
    await axiosClient
      .get(`/courses/mentor/add?courseId=${selectedIdCourse}`)
      .then((res) => {
        setListUnAddMentor(res?.data?.data);
        setOpenAddMentorForm(true);
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };

  async function getDataCourse() {
    await axiosClient
      .get(`/courses/${selectedIdCourse}`)
      .then((res) => {
        setListMentor(res?.data?.data?.course?.mentors);
      })
      .catch((error) => toastError(error?.respose?.data?.message));
  }

  const submitAddMenterToCouse = async () => {
    if (!selectedMentor) {
      toastError('Vui lòng chọn giảng viên');
      return;
    }
    await axiosForm
      .post(`/courses/mentor/add`, {
        courseId: selectedIdCourse,
        mentorId: selectedMentor,
      })
      .then((res) => {
        console.log(res);
        getDataCourse();
        setOpenAddMentorForm(false);
        toastSuccess(res?.data?.message);
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };

  console.log('Data: ', {
    courseId: selectedIdCourse,
    mentorId: selectedMentor,
  });

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
        <div class='bg-gradient-to-r from-indigo-500 via-purple-500 to-pink-500 rounded-lg text-center py-2 text-white font-semibold'>
          Danh sách giảng viên
        </div>
      </DialogTitle>
      <DialogContent>
        <div className='my-4'>
          {listMentor ? (
            <div>
              <div className='grid grid-cols-2'>
                {listMentor?.map((mentor, index) => (
                  <div className='border-b-2 py-6 px-6'>
                    <p>
                      <span className='font-semibold'>Tên giảng viên: </span>
                      {`${mentor?.firstName} ${mentor?.lastName}`}
                    </p>
                    <p>
                      <span className='font-semibold'>Ngày sinh: </span>
                      {dayjs(mentor?.dateBirth).format('DD/MM/YYYY')}
                    </p>
                    <p>
                      <span className='font-semibold'>Email: </span>
                      {mentor?.email}
                    </p>
                  </div>
                ))}
              </div>
            </div>
          ) : (
            <div className='text-center my-4 text-red-700'>
              Hiện chưa có giảng viên
            </div>
          )}

          {openAddMentorForm && (
            <div>
              <h1 className='my-4 text-[24px] font-semibold text-orange-500 text-center'>
                Thêm giảng viên
              </h1>
              <FormControl fullWidth>
                <InputLabel id='demo-simple-select-label'>
                  Tên giảng viên
                </InputLabel>
                <Select
                  labelId='demo-simple-select-label'
                  id='demo-simple-select'
                  value={selectedMentor}
                  label='Giảng viên'
                  onChange={(e) => setSelectedMentor(e.target.value)}
                >
                  {listUnAddMentor?.map((mentor, index) => (
                    <MenuItem value={mentor?.staffId}>
                      <div>{`${mentor?.firstName} ${mentor?.lastName} (${dayjs(
                        mentor?.dateBirth
                      ).format('DD/MM/YYYY')})`}</div>
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
              <div className='flex justify-end my-2'>
                <button className='btn ' onClick={submitAddMenterToCouse}>
                  Xác nhận
                </button>
              </div>
            </div>
          )}
        </div>
      </DialogContent>
      <DialogActions>
        <Button onClick={() => setOpen(false)}>Thoát</Button>
        <Button onClick={getListUnAddMentor} autoFocus>
          Thêm mới giảng viên
        </Button>
      </DialogActions>
    </Dialog>
  );
}
