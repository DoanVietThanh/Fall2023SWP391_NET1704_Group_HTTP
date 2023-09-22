import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import { Checkbox, FormControlLabel, Stack, Typography } from '@mui/material';
import IconButton from '@mui/material/IconButton';
import InputAdornment from '@mui/material/InputAdornment';
import TextField from '@mui/material/TextField';
import { useFormik } from 'formik';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { Link, useNavigate, useSearchParams } from 'react-router-dom';
import * as yup from 'yup';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import { login } from '../../features/auth/authSlice';
import axios from 'axios';
import { toastError, toastSuccess } from './../../components/Toastify';

let schema = yup.object().shape({
  email: yup
    .string()
    .min(10, 'Length should be more 10 chars')
    .email('Email should be valid')
    .required('Email is Required'),
  password: yup
    .string()
    .min(8, 'Password có ít nhất 8 kí tự')
    .matches(
      '^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$',
      'Password có chữ đầu ghi Hoa, có ít nhất 1 số'
    )
    .required('Password is Required'),
  confirmPassword: yup
    .string()
    .oneOf([yup.ref('password'), null], 'Passwords chưa trùng khớp'),
});

const ShowForgetPass = () => {
  const [searchParams] = useSearchParams();
  const passwordResetToken = searchParams.get('passwordResetToken');
  const email = searchParams.get('email');
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const [showPassword, setShowPassword] = useState(false);
  const baseServerURL = process.env.REACT_APP_SERVER_API;
  const handleTogglePassword = () => {
    setShowPassword(!showPassword);
  };

  const formik = useFormik({
    initialValues: {
      email: '',
      password: '',
      confirmPassword: '',
    },
    validationSchema: schema,
    onSubmit: async (values) => {
      const result = await axios.post(
        `${baseServerURL}/authentication/reset-password`,
        values
      );
      console.log(
        '🚀 ~ file: ShowForgetPass.jsx:59 ~ onSubmit: ~ result:',
        result
      );
      if (result.data.statusCode === 200) {
        toastSuccess(result.data.message);
        navigate('/login');
        return;
      } else {
        toastError('Something went wrong');
      }
    },
  });

  return (
    <div>
      <Header />
      <div className='center my-8'>
        <div className='border border-1 w-[40%] flex flex-col justify-center items-center p-8'>
          <Typography variant='h4'>Get New Password</Typography>
          <p>Fill in the fields below to sign into your account.</p>
          <form
            action=''
            onSubmit={formik.handleSubmit}
            className='w-full py-4 flex flex-col gap-4'
          >
            <TextField
              id='outlined-basic'
              label='Email Address'
              variant='outlined'
              className='w-full'
              onChange={formik.handleChange('email')}
              onBlur={formik.handleBlur('email')}
              value={formik.values.email}
            />{' '}
            <div className='error text-red-900'>
              {formik.touched.email && formik.errors.email}
            </div>
            <TextField
              label='Mật khẩu'
              variant='outlined'
              fullWidth
              onChange={formik.handleChange('password')}
              onBlur={formik.handleBlur('password')}
              value={formik.values.password}
              type={showPassword ? 'text' : 'password'}
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
            <TextField
              label='Nhập lại Mật khẩu'
              variant='outlined'
              fullWidth
              onChange={formik.handleChange('confirmPassword')}
              onBlur={formik.handleBlur('confirmPassword')}
              value={formik.values.confirmPassword}
              type={showPassword ? 'text' : 'password'}
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
            <button className='btn'>Submit</button>
          </form>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default ShowForgetPass;
