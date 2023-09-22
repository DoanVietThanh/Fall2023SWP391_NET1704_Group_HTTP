import { Typography } from '@mui/material';
import TextField from '@mui/material/TextField';
import axios from 'axios';
import { useFormik } from 'formik';
import React from 'react';
import { Link } from 'react-router-dom';
import * as yup from 'yup';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import { toastSuccess } from '../../components/Toastify';

let schema = yup.object().shape({
  username: yup
    .string()
    .min(10, 'Length should be more 10 chars')
    .email('Email should be valid')
    .required('Email is Required'),
});

const ForgotPassword = () => {
  const url_server = process.env.REACT_APP_SERVER_API;

  const formik = useFormik({
    initialValues: {
      username: '',
    },
    validationSchema: schema,
    onSubmit: async (values) => {
      try {
        await axios.post(
          `${url_server}/authentication/forgot-password?email=${values.username}`
        );
        toastSuccess('Check Mail to get New Password');
      } catch (error) {
        console.log(error);
      }
    },
  });

  return (
    <div>
      <Header />
      <div className='center my-8'>
        <div className='border border-1 w-[40%] flex flex-col justify-center items-center p-8'>
          <Typography variant='h4'>Forgot Password</Typography>
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
              onChange={formik.handleChange('username')}
              onBlur={formik.handleBlur('username')}
              value={formik.values.username}
            />{' '}
            <div className='error text-red-900'>
              {formik.touched.username && formik.errors.username}
            </div>
            <button className='btn'>Send Mail</button>
          </form>

          <div className='py-4'>
            <p className='font-medium capitalize'>
              Have an account?{' '}
              <Link to='/login' className='text-blue-400'>
                Sign in here
              </Link>
            </p>
          </div>

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

export default ForgotPassword;

// {{LOCAL}}/authentication/forgot-password?email=thanhdvse171867@fpt.edu.vn
