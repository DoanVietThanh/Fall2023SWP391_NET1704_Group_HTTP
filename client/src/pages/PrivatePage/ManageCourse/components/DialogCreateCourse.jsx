import { useEffect, useState } from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import {
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  TextField,
} from '@mui/material';
import axiosClient from '../../../../utils/axiosClient';
import { toastError, toastSuccess } from '../../../../components/Toastify';

export default function DialogCreateCourse({ open, setOpen, getAllCourses }) {
  const [formCreate, setFormCreate] = useState({
    courseTitle: '',
    courseDesc: '',
    totalMonth: '',
    startDate: '',
    totalHoursRequired: '',
    totalKmRequired: '',
    licenseTypeId: '',
  });

  const [listLicenseType, setListLicenseType] = useState([]);
  const [selectedLicenseType, setSelectedLicenseType] = useState([]);

  useEffect(() => {
    async function getLicenseTypeID() {
      await axiosClient
        .get(`/courses/add`)
        .then((res) => {
          console.log(res?.data);
          setListLicenseType(res?.data?.data);
        })
        .catch((error) => toastError(error?.response?.data?.message));
    }
    getLicenseTypeID();
  }, []);

  const submitCreate = async () => {
    console.log('submit formCreate: ', JSON.stringify(formCreate));
    await axiosClient
      .post(`/courses/add`, formCreate)
      .then((res) => {
        console.log('res: ', res);
        setOpen(false);
        toastSuccess(res?.data?.message);
        setFormCreate([]);
        getAllCourses();
      })
      .catch((error) => {
        console.log('error: ', error);
        // setOpen(false);
        // setFormCreate([]);
        toastError(JSON.stringify(error?.response?.data?.message));
      });
  };

  console.log('formCreate: ', formCreate);

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
          Tạo mới khóa học
        </div>
      </DialogTitle>
      <DialogContent>
        <div className='my-4 flex flex-col gap-4'>
          <TextField
            required={true}
            id='outlined-basic'
            label='courseTitle'
            variant='outlined'
            fullWidth
            value={formCreate?.courseTitle}
            onChange={(e) =>
              setFormCreate((prev) => {
                return { ...prev, courseTitle: e.target.value };
              })
            }
          />
          <TextField
            required={true}
            id='outlined-basic'
            label='courseDesc'
            variant='outlined'
            fullWidth
            value={formCreate?.courseDesc}
            onChange={(e) =>
              setFormCreate((prev) => {
                return { ...prev, courseDesc: e.target.value };
              })
            }
          />
          <TextField
            required={true}
            id='outlined-basic'
            label='totalMonth'
            variant='outlined'
            fullWidth
            type='number'
            value={formCreate?.totalMonth}
            onChange={(e) =>
              setFormCreate((prev) => {
                return { ...prev, totalMonth: e.target.value };
              })
            }
          />
          <div className='flex gap-4 items-center'>
            <label htmlFor='#startDate' className='w-[20%]'>
              Ngày bắt đầu:{' '}
            </label>
            <input
              type='date'
              name=''
              id='startDate'
              className='flex-1 p-4 border'
              value={formCreate?.startDate}
              onChange={(e) =>
                setFormCreate((prev) => {
                  return { ...prev, startDate: e.target.value };
                })
              }
            />
          </div>

          <div className='flex items-center'>
            <InputLabel id='demo-simple-select-label' className='w-[20%]'>
              Loại bằng lái
            </InputLabel>
            <Select
              labelId='demo-simple-select-label'
              id='demo-simple-select'
              value={selectedLicenseType}
              label='Age'
              onChange={(e) => {
                setSelectedLicenseType(e.target.value);
                setFormCreate((prev) => {
                  return { ...prev, licenseTypeId: e.target.value };
                });
              }}
              fullWidth
              className='flex-1'
            >
              {listLicenseType?.map((item, index) => (
                <MenuItem value={item?.licenseTypeId} key={item?.licenseTypeId}>
                  {item?.licenseTypeDesc}
                </MenuItem>
              ))}
            </Select>
          </div>

          <TextField
            required={true}
            id='outlined-basic'
            label='totalHoursRequired'
            variant='outlined'
            fullWidth
            type='number'
            value={formCreate?.totalHoursRequired}
            onChange={(e) =>
              setFormCreate((prev) => {
                return { ...prev, totalHoursRequired: e.target.value };
              })
            }
          />
          <TextField
            required={true}
            id='outlined-basic'
            label='totalKmRequired'
            variant='outlined'
            fullWidth
            type='number'
            value={formCreate?.totalKmRequired}
            onChange={(e) =>
              setFormCreate((prev) => {
                return { ...prev, totalKmRequired: e.target.value };
              })
            }
          />
        </div>
      </DialogContent>
      <DialogActions>
        <Button onClick={() => setOpen(false)}>Hủy</Button>
        <Button onClick={submitCreate} autoFocus>
          Tạo mới
        </Button>
      </DialogActions>
    </Dialog>
  );
}
