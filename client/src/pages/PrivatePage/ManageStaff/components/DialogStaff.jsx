import * as yup from 'yup';
import { useFormik } from 'formik?';
import { TextField } from '@mui/material';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import { useEffect, useState } from 'react';
import DialogTitle from '@mui/material/DialogTitle';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import axiosClient from '../../../../utils/axiosClient';
import { toastError, toastSuccess } from '../../../../components/Toastify';
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import {
  FormControl,
  IconButton,
  InputAdornment,
  InputLabel,
  MenuItem,
  Select,
} from '@mui/material';

import * as dayjs from 'dayjs';
import axiosForm from '../../../../utils/axiosForm';

let schema = yup.object().shape({
  username: yup
    .string()
    .email('Email không hợp lệ')
    .required('Vui lòng nhập Email'),
  firstName: yup
    .string()
    .max(10, 'Firstname có nhiều nhất 10 kí tự')
    .required('Vui lòng nhập họ'),
  lastName: yup
    .string()
    .max(10, 'Lastname có nhiều nhất 10 kí tự')
    .required('Vui lòng nhập tên'),
  password: yup
    .string()
    .min(8, 'Mật khẩu có ít nhất 8 kí tự')
    .matches(
      '^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$',
      'Mật khẩu có chữ đầu ghi Hoa, có ít nhất 1 số'
    )
    .required('Vui lòng nhập mật khẩu'),
  confirmPassword: yup
    .string()
    .oneOf([yup.ref('password'), null], 'Mật khẩu chưa trùng khớp'),
  phone: yup
    .string()
    .matches('^0[0-9]{9,11}$', 'Số điện thoại có độ dài 10-12')
    .required('Vui lòng nhập số điện thoại'),
  dateBirth: yup.string().required('Vui lòng nhập Ngày sinh'),
  street: yup.string().required('Vui lòng nhập tên đường'),
  district: yup.string().required('Vui lòng nhập tên quận'),
  city: yup.string().required('Vui lòng nhập tên thành phố'),
  jobTitleId: yup.string().required('Vui lòng chọn Công việc'),
  selfDescription: yup.string().required('Vui lòng nhập Mô tả'),
  courseId: yup.string().required('Vui lòng chọn Khóa học'),
});

export default function DialogStaff({
  open,
  setOpen,
  actionDialog,
  getAllStaffs,
  staffId,
}) {
  const enableReinitialize = true;
  const [showPassword, setShowPassword] = useState(false);

  const [availableValue, setAvailableValue] = useState();

  const [jobTitles, setJobTitles] = useState();
  const [courses, setCourses] = useState();

  const formik = useFormik({
    initialValues: availableValue || {
      username: '',
      password: '',
      firstName: '',
      lastName: '',
      dateBirth: '',
      phone: '',
      street: '',
      district: '',
      city: '',
      confirmPassword: '',
    },
    enableReinitialize,
    validationSchema: schema,
  });

  useEffect(() => {
    async function getData() {
      await axiosClient
        .get(`staffs/add`)
        .then((res) => {
          console.log(res);
          setJobTitles(res?.data?.data?.jobTitles);
          setCourses(res?.data?.data.courses);
        })
        .catch((error) => toastError(error?.response?.data?.message));
      if (actionDialog == 'create') {
        setAvailableValue({
          username: '',
          password: '',
          firstName: '',
          lastName: '',
          dateBirth: '',
          phone: '',
          street: '',
          district: '',
          city: '',
          selfDescription: '',
          jobTitleId: '',
          courseId: '',
          confirmPassword: '',
        });
      } else if (actionDialog == 'edit' && staffId) {
        await axiosClient.get(`/staffs/${staffId}`).then((res) => {
          console.log(res);
          setAvailableValue({
            firstName: res?.data?.data.firstName,
            lastName: res?.data?.data.lastName,
            dateBirth: dayjs(res?.data?.data.dateBirth).format('YYYY-MM-DD'),
            phone: res?.data?.data.phone,
            street: res?.data?.data.address.street,
            district: res?.data?.data.address.district,
            city: res?.data?.data.address.city,
            jobTitleId: 2,
          });
        });
      }
    }
    getData();
  }, [actionDialog]);

  console.log('Value Formik?: ', formik?.values);

  const handleEditStaff = async () => {
    await axiosForm
      .put(`/staffs/${staffId}/update`, formik.values)
      .then((res) => {
        console.log('res: ', res);
        toastSuccess(res?.data?.message);
        getAllStaffs();
        setOpen(false);
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };

  const handleCreateStaff = async () => {
    await axiosClient
      .post(`/staffs/add`, formik.values)
      .then((res) => {
        console.log('res: ', res);
        toastSuccess(res?.data?.message);
        getAllStaffs();
        setOpen(false);
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };

  const handleTogglePassword = () => {
    setShowPassword(!showPassword);
  };

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
        <div className='rounded-lg text-white text-center capitalize font-medium bg-gradient-to-r from-indigo-500 from-10% via-sky-500 via-30% to-emerald-500 to-90%'>
          Tạo mới nhân viên
        </div>
      </DialogTitle>
      <DialogContent>
        {formik.values && (
          <form
            onSubmit={formik?.handleSubmit}
            className='w-full py-4 flex flex-col gap-4'
          >
            {actionDialog == 'create' && (
              <div className='flex flex-col gap-4'>
                <div>
                  <TextField
                    id='outlined-basic'
                    label='Email'
                    variant='outlined'
                    className='w-full'
                    onChange={formik.handleChange('username')}
                    onBlur={formik.handleBlur('username')}
                    value={formik.values.username}
                  />
                  <div className='error text-red-900'>
                    {formik.touched.username && formik.errors.username}
                  </div>
                </div>

                {/* Password  */}
                <div className='mt-4'>
                  <TextField
                    label='Password'
                    variant='outlined'
                    fullWidth
                    type={showPassword ? 'text' : 'password'}
                    onChange={formik.handleChange('password')}
                    onBlur={formik.handleBlur('password')}
                    value={formik.values.password}
                    InputProps={{
                      endAdornment: (
                        <InputAdornment position='end'>
                          <IconButton
                            aria-label='Toggle password visibility'
                            onClick={handleTogglePassword}
                            edge='end'
                          >
                            {showPassword ? (
                              <VisibilityIcon />
                            ) : (
                              <VisibilityOffIcon />
                            )}
                          </IconButton>
                        </InputAdornment>
                      ),
                    }}
                  />
                  <div className='error text-red-900'>
                    {formik.touched.password && formik.errors.password}
                  </div>
                </div>

                {/* Confirmed Password  */}
                <TextField
                  label='Confirmed Password'
                  variant='outlined'
                  fullWidth
                  type={showPassword ? 'text' : 'password'}
                  onChange={formik.handleChange('confirmPassword')}
                  onBlur={formik.handleBlur('confirmPassword')}
                  value={formik.values.confirmPassword}
                  InputProps={{
                    endAdornment: (
                      <InputAdornment position='end'>
                        <IconButton
                          aria-label='Toggle password visibility'
                          onClick={handleTogglePassword}
                          edge='end'
                        >
                          {showPassword ? (
                            <VisibilityIcon />
                          ) : (
                            <VisibilityOffIcon />
                          )}
                        </IconButton>
                      </InputAdornment>
                    ),
                  }}
                />
                <div className='error text-red-900'>
                  {formik.touched.confirmPassword &&
                    formik.errors.confirmPassword}
                </div>
              </div>
            )}

            <div className='flex gap-4 w-full'>
              {/* First Name  */}
              <div className='flex-1'>
                <TextField
                  id='outlined-basic'
                  label='First Name'
                  variant='outlined'
                  className='w-full'
                  onChange={formik?.handleChange('firstName')}
                  onBlur={formik?.handleBlur('firstName')}
                  value={formik?.values?.firstName}
                />
                <div className='error text-red-900'>
                  {formik?.touched.firstName && formik?.errors.firstName}
                </div>
              </div>

              {/* Last Name  */}
              <div className='flex-1'>
                <TextField
                  id='outlined-basic'
                  label='Last Name'
                  variant='outlined'
                  className='w-full'
                  onChange={formik?.handleChange('lastName')}
                  onBlur={formik?.handleBlur('lastName')}
                  value={formik?.values.lastName}
                />
                <div className='error text-red-900'>
                  {formik?.touched.lastName && formik?.errors.lastName}
                </div>
              </div>
            </div>

            {/* Date Of Birth  */}
            <div>
              <div className='flex justify-between items-center'>
                <h2 className='whitespace-nowrap pr-8 '>Date Of Birth</h2>
                <TextField
                  id='outlined-basic'
                  variant='outlined'
                  className='w-full'
                  type='date'
                  onChange={formik?.handleChange('dateBirth')}
                  onBlur={formik?.handleBlur('dateBirth')}
                  value={formik?.values.dateBirth}
                />
              </div>
              <div className='error text-red-900'>
                {formik?.touched.dateBirth && formik?.errors.dateBirth}
              </div>
            </div>

            {/* Phone  */}
            <div>
              <TextField
                id='outlined-basic'
                label='Phone'
                variant='outlined'
                className='w-full'
                type='text'
                onChange={formik?.handleChange('phone')}
                onBlur={formik?.handleBlur('phone')}
                value={formik?.values.phone}
              />
              <div className='error text-red-900'>
                {formik?.touched.phone && formik?.errors.phone}
              </div>
            </div>

            {/* Street  */}
            <div>
              <TextField
                id='outlined-basic'
                label='Street'
                variant='outlined'
                className='w-full'
                type='text'
                onChange={formik?.handleChange('street')}
                onBlur={formik?.handleBlur('street')}
                value={formik?.values?.street}
              />
              <div className='error text-red-900'>
                {formik?.touched.street && formik?.errors.street}
              </div>
            </div>

            <div>
              <TextField
                id='outlined-basic'
                label='District'
                variant='outlined'
                className='w-full'
                type='text'
                onChange={formik?.handleChange('district')}
                onBlur={formik?.handleBlur('district')}
                value={formik?.values?.district}
              />
              <div className='error text-red-900'>
                {formik?.touched.district && formik?.errors.district}
              </div>
            </div>

            {/* City  */}
            <div>
              <TextField
                id='outlined-basic'
                label='City'
                variant='outlined'
                className='w-full'
                type='text'
                onChange={formik?.handleChange('city')}
                onBlur={formik?.handleBlur('city')}
                value={formik?.values?.city}
              />
              <div className='error text-red-900'>
                {formik?.touched.city && formik?.errors.city}
              </div>
            </div>

            {/* jobTitles */}
            {actionDialog == 'create' && (
              <div className='flex flex-col gap-4'>
                <FormControl fullWidth>
                  <InputLabel id='demo-simple-select-label'>
                    Loại công việc
                  </InputLabel>
                  <Select
                    labelId='demo-simple-select-label'
                    id='demo-simple-select'
                    value={formik.values.jobTitleId}
                    label='Loại công việc'
                    onChange={(e) => {
                      formik.setFieldValue('jobTitleId', e.target.value);
                      if (e.target.value != 3) {
                        formik.setFieldValue('selfDescription', '');
                        formik.setFieldValue('courseId', '');
                      }
                    }}
                  >
                    {jobTitles?.map((jobTitle, indxex) => (
                      <MenuItem value={jobTitle.jobTitleId}>
                        {jobTitle.jobTitleDesc}
                      </MenuItem>
                    ))}
                  </Select>

                  {formik?.values.jobTitleId == 3 && (
                    <div className='flex flex-col gap-4'>
                      <div className='mt-4'>
                        <TextField
                          id='outlined-basic'
                          label='Mô tả'
                          variant='outlined'
                          className='w-full'
                          type='text'
                          onChange={formik?.handleChange('selfDescription')}
                          onBlur={formik?.handleBlur('selfDescription')}
                          value={formik?.values?.selfDescription}
                        />
                        <div className='error text-red-900'>
                          {formik?.touched.selfDescription &&
                            formik?.errors.selfDescription}
                        </div>
                      </div>

                      <FormControl fullWidth>
                        <InputLabel id='demo-simple-select-label'>
                          Khóa học
                        </InputLabel>
                        <Select
                          labelId='demo-simple-select-label'
                          id='demo-simple-select'
                          value={formik.values.courseId}
                          label='Khóa học'
                          onChange={(e) =>
                            formik.setFieldValue('courseId', e.target.value)
                          }
                        >
                          {courses?.map((course, indxex) => (
                            <MenuItem value={course.courseId}>
                              {course.courseTitle}
                            </MenuItem>
                          ))}
                        </Select>
                      </FormControl>
                    </div>
                  )}
                </FormControl>
              </div>
            )}
          </form>
        )}
      </DialogContent>
      <DialogActions>
        <Button onClick={() => setOpen(false)}>Hủy</Button>
        {actionDialog == 'edit' && (
          <Button onClick={() => handleEditStaff()} autoFocus>
            Chỉnh sửa
          </Button>
        )}
        {actionDialog == 'create' && (
          <Button onClick={() => handleCreateStaff()} autoFocus>
            Tạo mới
          </Button>
        )}
      </DialogActions>
    </Dialog>
  );
}
