import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import { Checkbox, FormControlLabel, Stack, Typography } from '@mui/material';
import IconButton from '@mui/material/IconButton';
import InputAdornment from '@mui/material/InputAdornment';
import TextField from '@mui/material/TextField';
import { useFormik } from 'formik';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import * as yup from 'yup';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import { login } from '../../features/auth/authSlice';

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
});

const LoginPage = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const { user, isSuccess, isError, isLoading } = useSelector(
    (state) => state.auth
  );
  const [showPassword, setShowPassword] = useState(false);

  const handleTogglePassword = () => {
    setShowPassword(!showPassword);
  };

  const formik = useFormik({
    initialValues: {
      username: '',
      password: '',
    },
    validationSchema: schema,
    onSubmit: async (values) => {
      dispatch(login(values));
    },
  });

  useEffect(() => {
    if (
      isSuccess
      //  && user?.accountInfo.emailNavigation.role.roleId === 4
    ) {
      navigate('/home');
    } else {
      navigate('');
    }
  }, [user, isError, isLoading, isSuccess]);

  return (
    <div>
      <Header />
      <div className='center my-8'>
        <div className='border border-1 w-[40%] flex flex-col justify-center items-center p-8'>
          <Typography variant='h4'>Đăng nhập</Typography>
          <p>Điền tài khoản và mật khẩu để đăng nhập</p>
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
              onChange={formik.handleChange('username')}
              onBlur={formik.handleBlur('username')}
              value={formik.values.username}
            />{' '}
            <div className='error text-red-900'>
              {formik.touched.username && formik.errors.username}
            </div>
            <TextField
              label='Mật khẩu'
              variant='outlined'
              fullWidth
              onChange={formik.handleChange('password')}
              onBlur={formik.handleBlur('password')}
              value={formik.values.password}gap-5
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
            {/* tuong lai lam */}
            {/* <Stack direction={'row-reverse'}>
              <FormControlLabel
                control={<Checkbox />}
                label='Nhớ mật khẩu'
              />
            </Stack> */}
            <button className='btn'>Đăng nhập</button>
          </form>

          <div>
            <p className='font-medium capitalize'>
              Quên mật khẩu?{' '}
              <Link to='/forgot-password' className='text-blue-400 py-2'>
                Lấy lại mật khẩu
              </Link>
            </p>
          </div>

          <div>
            <p className='font-medium capitalize'>
              Chưa có tài khoản?{' '}
              <Link to='/register' className='text-blue-400'>
                Đăng kí
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

// {{LOCAL}}/authentication/forgot-password?email=thanhdvse171867@fpt.edu.vn
