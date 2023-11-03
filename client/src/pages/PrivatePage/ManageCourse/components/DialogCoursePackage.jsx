import { FormControl, TextField } from '@mui/material';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import { useEffect, useState } from 'react';
import axiosClient from '../../../../utils/axiosClient';
import { toastError, toastSuccess } from '../../../../components/Toastify';
import { BiSolidEdit } from 'react-icons/bi';
import { AiFillDelete } from 'react-icons/ai';

export default function DialogCoursePackage({
  open,
  setOpen,
  selectedIdCourse,
  getAllCourses,
}) {
  const [coursePackages, setCoursePackages] = useState();
  const [openFormCreate, setOpenFormCreate] = useState(false);
  const [formCreate, setFormCreate] = useState({
    coursePackageDesc: '',
    cost: '',
    totalSession: null,
    sessionHour: null,
    ageRequired: '',
  });

  useEffect(() => {
    async function getCoursePackages() {
      await axiosClient
        .get(`/courses/${selectedIdCourse}`)
        .then((res) => {
          console.log(res);
          setCoursePackages(res?.data?.data?.course?.coursePackages);
        })
        .catch((error) => toastError(error?.response?.data?.message));
    }
    getCoursePackages();
  }, []);

  console.log('selectedIdCourse: ', selectedIdCourse);

  const getCoursePackages = async () => {
    await axiosClient
      .get(`/courses/${selectedIdCourse}`)
      .then((res) => {
        console.log(res);
        setCoursePackages(res?.data?.data?.course?.coursePackages);
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };

  console.log('coursePackages: ', coursePackages);

  const handleCreateCoursePackage = async () => {
    console.log(JSON.stringify(formCreate));
    console.log(JSON.stringify(selectedIdCourse));
    await axiosClient
      .post(`/courses/${selectedIdCourse}/packages/add`, formCreate)
      .then((res) => {
        console.log(res);
        toastSuccess(res?.data?.message);
        setOpenFormCreate(false);
        getCoursePackages();
      })
      .catch((error) => toastError(error?.response?.data.message));
  };

  return (
    <Dialog
      open={open}
      onClose={() => setOpen(false)}
      aria-labelledby='alert-dialog-title'
      aria-describedby='alert-dialog-description'
      fullWidth
      maxWidth='sm'
    >
      <DialogTitle id='alert-dialog-title'>
        <div className='text-center capitalize font-medium'>Gói khóa học</div>
      </DialogTitle>
      <DialogContent>
        <div className='my-4 flex flex-col gap-4 '>
          {coursePackages?.map((coursePackage, index) => (
            <div
              key={coursePackage?.coursePackageId}
              className='border-t-2 p-4 flex gap-4 justify-between'
            >
              <div className='flex flex-col gap-2'>
                <h3>
                  <span className='font-semibold'>Mô tả:</span>{' '}
                  {coursePackage?.coursePackageDesc}
                </h3>
                <h3>
                  <span className='font-semibold'>Tiểu tối thiểu:</span>{' '}
                  {coursePackage?.ageRequired}
                </h3>
                <h3>
                  <span className='font-semibold'>Giá:</span>{' '}
                  {coursePackage?.cost} VNĐ
                </h3>
              </div>
              <div className='flex flex-col gap-4 justify-evenly'>
                <div className='flex items-center gap-2 py-2 px-4 bg-gray-200 rounded-lg cursor-pointer hover:opacity-80'>
                  <BiSolidEdit size={20} /> Chỉnh sửa
                </div>
                <div className='flex items-center gap-2 py-2 px-4 bg-gray-200 rounded-lg cursor-pointer hover:opacity-80'>
                  <AiFillDelete size={20} className='text-red-700' />
                  Xóa
                </div>
              </div>
            </div>
          ))}
        </div>

        {openFormCreate && (
          <div className='flex flex-col gap-4 border-t-2 mt-4 pt-4'>
            <div className='text-center capitalize font-semibold'>Thêm gói</div>

            <TextField
              required={true}
              id='outlined-basic'
              label='Mô tả gói khóa học'
              variant='outlined'
              fullWidth
              value={formCreate?.coursePackageDesc}
              onChange={(e) =>
                setFormCreate((prev) => {
                  return { ...prev, coursePackageDesc: e.target.value };
                })
              }
            />
            <TextField
              required={true}
              id='outlined-basic'
              label='Tổng số buổi'
              variant='outlined'
              fullWidth
              type='number'
              value={formCreate?.totalSession}
              onChange={(e) =>
                setFormCreate((prev) => {
                  return { ...prev, totalSession: e.target.value };
                })
              }
            />
            <TextField
              required={true}
              id='outlined-basic'
              label='Thồi lượng 1 buổi học '
              variant='outlined'
              fullWidth
              type='number'
              value={formCreate?.sessionHour}
              onChange={(e) =>
                setFormCreate((prev) => {
                  return { ...prev, sessionHour: e.target.value };
                })
              }
            />
            <TextField
              required={true}
              id='outlined-basic'
              label='Tuổi tối thiểu'
              variant='outlined'
              fullWidth
              type='number'
              InputProps={{
                inputProps: {
                  min: 16,
                },
              }}
              value={formCreate?.ageRequired}
              onChange={(e) =>
                setFormCreate((prev) => {
                  return { ...prev, ageRequired: e.target.value };
                })
              }
            />

            <TextField
              required={true}
              id='outlined-basic'
              label='Giá tiền'
              variant='outlined'
              fullWidth
              type='number'
              value={formCreate?.cost}
              InputProps={{
                inputProps: {
                  min: 0,
                },
              }}
              onChange={(e) =>
                setFormCreate((prev) => {
                  return { ...prev, cost: e.target.value };
                })
              }
            />
            <button
              className='btn'
              type='submit'
              onClick={handleCreateCoursePackage}
            >
              Tạo
            </button>
          </div>
        )}
      </DialogContent>

      <DialogActions>
        <Button onClick={() => setOpen(false)}>Hủy</Button>
        <Button onClick={() => setOpenFormCreate(true)} autoFocus>
          Tạo mới
        </Button>
      </DialogActions>
    </Dialog>
  );
}
