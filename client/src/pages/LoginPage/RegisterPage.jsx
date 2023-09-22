import Header from '../../components/Header';
import Footer from '../../components/Footer';
import TextField from '@mui/material/TextField';
import React, { useState } from 'react';
import IconButton from '@mui/material/IconButton';
import InputAdornment from '@mui/material/InputAdornment';
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import { Link } from 'react-router-dom';
import {
  Checkbox,
  FormControlLabel,
  FormGroup,
  Stack,
  Typography,
} from '@mui/material';

const RegisterPage = () => {
  const [showPassword, setShowPassword] = useState(false);

  const handleTogglePassword = () => {
    setShowPassword(!showPassword);
  };
  return (
    <div>
      <Header />
      <div className='center my-8'>
        <div className='border border-1 w-[40%] flex flex-col justify-center items-center p-8'>
          <Typography variant='h4'>Create Account</Typography>
          <p>Fill in the fields below to sign into your account.</p>
          <FormGroup action='' className='w-full py-4 flex flex-col gap-4'>
            <TextField
              id='outlined-basic'
              label='Name'
              variant='outlined'
              className='w-full'
            />
            <TextField
              id='outlined-basic'
              label='Email Address'
              variant='outlined'
              className='w-full'
            />
            <TextField
              label='Password'
              variant='outlined'
              fullWidth
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
            <TextField
              label='Confirmed Password'
              variant='outlined'
              fullWidth
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

            <button className='btn'>Create Account</button>
          </FormGroup>

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
