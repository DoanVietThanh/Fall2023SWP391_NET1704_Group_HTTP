import React, { useState } from 'react';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import TextField from '@mui/material/TextField';
import IconButton from '@mui/material/IconButton';
import InputAdornment from '@mui/material/InputAdornment';
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import { Link, useNavigate } from 'react-router-dom';
import * as yup from 'yup';
import { useFormik } from 'formik';
import {
  Checkbox,
  FormControlLabel,
  FormGroup,
  Stack,
  Typography,
} from '@mui/material';

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
});

const LoginPage = () => {
  const navigate = useNavigate();
  const [showPassword, setShowPassword] = useState(false);

  const handleTogglePassword = () => {
    setShowPassword(!showPassword);
  };

  const formik = useFormik({
    initialValues: {
      email: '',
      password: '',
    },
    validationSchema: schema,
    onSubmit: (values) => {
      navigate('/home');
    },
  });

  return (
    <div>
      <Header />
      <div className='center my-8'>
        <div className='border border-1 w-[40%] flex flex-col justify-center items-center p-8'>
          <Typography variant='h4'>Sign In</Typography>
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
            <Stack direction={'row-reverse'}>
              <FormControlLabel
                control={<Checkbox />}
                label='Remember Password'
              />
            </Stack>
            <button className='btn'>Login</button>
          </form>

          <div>
            <p className='font-medium capitalize'>
              Don’t have an account?{' '}
              <Link to='/register' className='text-blue-400'>
                Sign up here
              </Link>
            </p>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default LoginPage;
