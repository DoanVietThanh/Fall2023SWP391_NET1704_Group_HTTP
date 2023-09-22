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
  firstName: yup
    .string()
    .max(10, 'Firstname có nhiều nhất 10 kí tự')
    .matches('^[a-zA-Z ]+$', 'Không chứa số hay kí tự đặc biệt')
    .required('Nhập FirstName'),
  lastName: yup
    .string()
    .max(10, 'Lastname có nhiều nhất 10 kí tự')
    .matches('^[a-zA-Z ]+$', 'Không chứa số hay kí tự đặc biệt')
    .required('Nhập LastName'),
  phone: yup
    .string()
    .matches('^0[0-9]{9,11}$', 'Số điện thoại có độ dài 10-12')
    .required('Nhập số điện thoại'),
  dateBirth: yup.string().required('Nhập Date Of Birth'),
  street: yup
    .string()
    .matches('^[a-zA-Z0-9 ]+$', 'Không chứa số hay kí tự đặc biệt')
    .required('Nhập Street'),
  district: yup
    .string()
    .matches('^[a-zA-Z ]+$', 'Không chứa số hay kí tự đặc biệt')
    .required('Nhập district'),
  city: yup
    .string()
    .matches('^[a-zA-Z ]+$', 'Không chứa số hay kí tự đặc biệt')
    .required('Nhập city'),
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
        toastError('Something went wrong');
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
              label='Username'
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
            <FormControl fullWidth>
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

            {/* Date Of Birth  */}
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

            {/* Phone  */}
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

            {/* Street  */}
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
            {/* District  */}
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
            {/* City  */}
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
            <button className='btn'>Create Account</button>
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
              Already have an account?{' '}
              <Link to='/login' className='text-blue-400'>
                Sign In here
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
