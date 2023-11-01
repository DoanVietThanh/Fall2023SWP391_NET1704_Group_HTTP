import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import {
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  Typography,
} from '@mui/material';
import IconButton from '@mui/material/IconButton';
import InputAdornment from '@mui/material/InputAdornment';
import TextField from '@mui/material/TextField';
import axios from 'axios';
import * as yup from 'yup';
import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import { useFormik } from 'formik';
import { toastError, toastSuccess } from '../../components/Toastify';

let schema = yup.object().shape({
  username: yup
    .string()
    .min(10, 'Email có ít nhất 10 kí tự')
    .email('Email phải hợp lệ')
    .required('Yêu cầu nhập email'),
  password: yup
    .string()
    .min(8, 'Mật khẩu có ít nhất 8 kí tự')
    .matches(
      '^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$',
      'Mật khẩu có chữ đầu ghi Hoa, có ít nhất 1 số'
    )
    .required('Yêu cầu nhập mật khẩu'),
  confirmPassword: yup
    .string()
    .oneOf([yup.ref('password'), null], 'Mật khẩu chưa trùng khớp'),
  firstName: yup
    .string()
    .max(10, 'Tên có nhiều nhất 10 kí tự')
    .matches('^[a-zA-Z ]+$', 'Không chứa số hay kí tự đặc biệt')
    .required('Nhập tên'),
  lastName: yup
    .string()
    .max(10, 'Họ có nhiều nhất 10 kí tự')
    .matches('^[a-zA-Z ]+$', 'Không chứa số hay kí tự đặc biệt')
    .required('Nhập họ'),
  phone: yup
    .string()
    .matches('^0[0-9]{9,11}$', 'Số điện thoại có độ dài 10-12')
    .required('Nhập số điện thoại'),
  dateBirth: yup.string().required('Nhập ngày sinh'),
  street: yup
    .string()
    .matches('^[a-zA-Z0-9 ]+$', 'Không chứa số hay kí tự đặc biệt')
    .required('Nhập đường'),
  district: yup
    .string()
    .matches('^[a-zA-Z ]+$', 'Không chứa số hay kí tự đặc biệt')
    .required('Nhập huyện/quận'),
  city: yup
    .string()
    .matches('^[a-zA-Z ]+$', 'Không chứa số hay kí tự đặc biệt')
    .required('Nhập tỉnh/thành phố'),
});

const RegisterPage = () => {
  const baseServer = process.env.REACT_APP_SERVER_API;
  const [showPassword, setShowPassword] = useState(false);
  const [lisenceType, setLisenceType] = useState([]);
  const [selectType, setSelectType] = useState('1');
  const navigate = useNavigate();
  const handleTogglePassword = () => {
    setShowPassword(!showPassword);
  };

  useEffect(() => {
    async function fetchType() {
      const response = await axios.get(`${baseServer}/authentication`);
      setLisenceType(response.data.data);
    }
    fetchType();
  }, []);

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
      licenseTypeId: selectType,
    },
    validationSchema: schema,
    onSubmit: async (values) => {
      const result = await axios.post(`${baseServer}/authentication`, values);
      if (result.data.statusCode === 200) {
        toastSuccess(result.data.message);
        navigate('/login');
      } else {
        toastError('Đăng nhập thất bại');
      }
      console.log('result: ', result);
    },
  });

  return (
    <div>
      <Header />
      <div className='center my-8'>
        <div className='border border-1 w-[60%] flex flex-col justify-center items-center p-8'>
          <Typography variant='h4'>Create Account</Typography>
          <p>Fill in the fields below to sign into your account.</p>

          <form
            action=''
            onSubmit={formik.handleSubmit}
            className='w-full py-4 flex flex-col gap-4'
          >
            {/* User Name  */}
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

            {/* Select Type  */}
            {/* <FormControl fullWidth>
              <InputLabel id='demo-simple-select-label'>
                Lisence Type
              </InputLabel>
              <Select
                labelId='demo-simple-select-label'
                id='demo-simple-select'
                value={formik.values.licenseTypeId}
                label='Lisence Type'
                onChange={(event) => setSelectType(event.target.value)}
              >
                {lisenceType?.map((item) => (
                  <MenuItem value={item.licenseTypeId} key={item.lisenceTypeId}>
                    {item.licenseTypeDesc}
                  </MenuItem>
                ))}
              </Select>
            </FormControl> */}

            <div className='flex gap-4 w-full'>
              {/* First Name  */}
              <div className='flex-1'>
                <TextField
                  id='outlined-basic'
                  label='Tên'
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
                  label='Họ'
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

            {/* Password  */}
            <TextField
              label='Mật khẩu'
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
              label='Xác nhận mật khẩu'
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

            {/* Date Of Birth  */}
            <div className='flex justify-between items-center'>
              <h2 className='whitespace-nowrap pr-8 '>Ngày sinh</h2>
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

            {/* Phone  */}
            <TextField
              id='outlined-basic'
              label='Số điện thoại'
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

            {/* Street  */}
            <TextField
              id='outlined-basic'
              label='Đường'
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
            {/* District  */}
            <TextField
              id='outlined-basic'
              label='Huyện/Quận'
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
            {/* City  */}
            <TextField
              id='outlined-basic'
              label='Tỉnh/Thành phố'
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
            <button className='btn'>Tạo tài khoản</button>
          </form>

          <div>
            <p className='font-medium capitalize'>
              <Link
                to='/forgot-password'
                className='text-blue-400 text-[20px] py-2'
              >
                Forgot Password
              </Link>
            </p>
          </div>

          <div>
            <p className='font-medium capitalize'>
              Đã có tài khoản?{' '}
              <Link to='/login' className='text-blue-400'>
                Đăng nhập
              </Link>
            </p>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default RegisterPage;
