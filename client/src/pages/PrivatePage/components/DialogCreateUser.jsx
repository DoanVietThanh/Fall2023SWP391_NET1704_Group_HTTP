import * as React from 'react';
import { useFormik } from 'formik';
import { useEffect, useState } from 'react';
import * as yup from 'yup';
import axiosClient from '../../../utils/axiosClient';
import {
  FormControl,
  IconButton,
  InputAdornment,
  InputLabel,
  MenuItem,
  Select,
  TextField,
} from '@mui/material';
import * as dayjs from 'dayjs';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import { toastError, toastSuccess } from '../../../components/Toastify';
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';

let schema = yup.object().shape({
  username: yup
    .string()
    .email('Email không hợp lệ')
    .required('Vui lòng nhập Email'),
  password: yup
    .string()
    .min(8, 'Password có ít nhất 8 kí tự')
    .matches(
      '^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$',
      'Password có chữ đầu ghi Hoa, có ít nhất 1 số'
    )
    .required('Vui lòng nhập Password'),
  confirmPassword: yup
    .string()
    .oneOf([yup.ref('password'), null], 'Passwords chưa trùng khớp'),
  firstName: yup
    .string()
    .max(10, 'Firstname có nhiều nhất 10 kí tự')
    .matches('^[a-zA-Z ]+$', 'Không chứa số hay kí tự đặc biệt')
    .required('Vui lòng nhập FirstName'),
  lastName: yup
    .string()
    .max(10, 'Lastname có nhiều nhất 10 kí tự')
    .matches('^[a-zA-Z ]+$', 'Không chứa số hay kí tự đặc biệt')
    .required('Vui lòng nhập  LastName'),
  phone: yup
    .string()
    .matches('^0[0-9]{9,11}$', 'Số điện thoại có độ dài 10-12')
    .required('Vui lòng nhập số điện thoại'),
  dateBirth: yup.string().required('Vui lòng nhập Ngày sinh'),
  street: yup
    .string()
    .matches('^[a-zA-Z0-9 ]+$', 'Không chứa số hay kí tự đặc biệt')
    .required('Vui lòng nhập tên đường'),
  district: yup
    .string()
    .matches('^[a-zA-Z ]+$', 'Không chứa số hay kí tự đặc biệt')
    .required('Vui lòng nhập tên quận'),
  city: yup
    .string()
    .matches('^[a-zA-Z ]+$', 'Không chứa số hay kí tự đặc biệt')
    .required('Vui lòng nhập tên thành phố'),
});

const DialogCreateUser = ({ open, setOpen, getAllUsers }) => {
  const [user, setUser] = useState();
  const [listType, setListType] = useState([]);
  const [selectType, setSelectType] = useState();
  const enableReinitialize = true;
  // const [formik, setFormik] = useState()
  const [availableValue, setAvailableValue] = useState();
  const [labelSelected, setLabelSelected] = useState([]);
  const [showPassword, setShowPassword] = useState(false);
  const handleTogglePassword = () => {
    setShowPassword(!showPassword);
  };
  const formik = useFormik({
    initialValues: {
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
      licenseTypeId: 1,
    },
    validationSchema: schema,
    onSubmit: async (values) => {
      console.log('values: ', values);
      await axiosClient
        .post(`/authentication`, values)
        .then((res) => {
          console.log('res: ', res);
          toastSuccess(res?.data?.message);
          setOpen(false);
          getAllUsers();
        })
        .catch((error) => toastError(error?.response?.data?.message));
    },
  });

  const submitCreateUser = async (e) => {
    e.preventDefault();
    console.log('formik.values: ', formik.values);
    // const response = await axiosClient.put(
    //   `/members/${userId}/update`,
    //   formik.values
    // );
    // toastSuccess(response?.data?.data);
    // setOpen(false);
    // getAllUsers();
    // console.log('response: ', response);
  };

  return (
    <div>
      <Dialog
        open={open}
        onClose={() => setOpen(false)}
        aria-labelledby='alert-dialog-title'
        aria-describedby='alert-dialog-description'
      >
        <DialogTitle id='alert-dialog-title'>
          <h1 className='text-center font-bold capitalize'>
            Tạo mới người dùng
          </h1>
        </DialogTitle>
        <DialogContent>
          <form
            action=''
            // onSubmit={formik.handleSubmit}
            onSubmit={formik.handleSubmit}
            className='w-full py-4 flex flex-col gap-4'
          >
            {/* User Name  */}
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

            {/* Select Type  */}

            <FormControl fullWidth>
              <InputLabel id='demo-simple-select-label'>
                {labelSelected[0]?.licenseTypeDesc}
              </InputLabel>
              <Select
                labelId='demo-simple-select-label'
                id='demo-simple-select'
                value={formik.values.licenseTypeId}
                label={labelSelected[0]?.licenseTypeDesc}
                // onChange={(event) => setSelectType(event.target.value)}
                onChange={(event) =>
                  setAvailableValue((prev) => {
                    return {
                      ...prev,
                      licenseTypeId: event.target.value,
                    };
                  })
                }
              >
                <MenuItem value={'1'} selected>
                  1
                </MenuItem>
              </Select>
            </FormControl>

            <div className='flex gap-4 w-full'>
              {/* First Name  */}
              <div className='flex-1'>
                <TextField
                  id='outlined-basic'
                  label='First Name'
                  variant='outlined'
                  className='w-full'
                  onChange={formik.handleChange('firstName')}
                  onBlur={formik.handleBlur('firstName')}
                  value={formik.values.firstName}
                />
                <div className='error text-red-900'>
                  {formik.touched.firstName && formik.errors.firstName}
                </div>
              </div>

              {/* Last Name  */}
              <div className='flex-1'>
                <TextField
                  id='outlined-basic'
                  label='Last Name'
                  variant='outlined'
                  className='w-full'
                  onChange={formik.handleChange('lastName')}
                  onBlur={formik.handleBlur('lastName')}
                  value={formik.values.lastName}
                />
                <div className='error text-red-900'>
                  {formik.touched.lastName && formik.errors.lastName}
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
                  onChange={formik.handleChange('dateBirth')}
                  onBlur={formik.handleBlur('dateBirth')}
                  value={formik.values.dateBirth}
                />
              </div>
              <div className='error text-red-900'>
                {formik.touched.dateBirth && formik.errors.dateBirth}
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
                onChange={formik.handleChange('phone')}
                onBlur={formik.handleBlur('phone')}
                value={formik.values.phone}
              />
              <div className='error text-red-900'>
                {formik.touched.phone && formik.errors.phone}
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
                onChange={formik.handleChange('street')}
                onBlur={formik.handleBlur('street')}
                value={formik.values.street}
              />
              <div className='error text-red-900'>
                {formik.touched.street && formik.errors.street}
              </div>
            </div>

            {/* District  */}
            <div>
              <TextField
                id='outlined-basic'
                label='District'
                variant='outlined'
                className='w-full'
                type='text'
                onChange={formik.handleChange('district')}
                onBlur={formik.handleBlur('district')}
                value={formik.values.district}
              />
              <div className='error text-red-900'>
                {formik.touched.district && formik.errors.district}
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
                onChange={formik.handleChange('city')}
                onBlur={formik.handleBlur('city')}
                value={formik.values.city}
              />
              <div className='error text-red-900'>
                {formik.touched.city && formik.errors.city}
              </div>
            </div>

            {/* Password  */}
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
              {formik.touched.confirmPassword && formik.errors.confirmPassword}
            </div>

            <button className='btn' type='submit'>
              Tạo mới
            </button>
          </form>
        </DialogContent>
      </Dialog>
    </div>
  );
};

export default DialogCreateUser;
