import Dialog from '@mui/material/Dialog';
import TextField from '@mui/material/TextField';
import axios from 'axios';
import { useFormik } from 'formik';
import React, { useEffect } from 'react';
import { useSelector } from 'react-redux';
import { Navigate } from 'react-router-dom';
import * as yup from 'yup';
import { toastError, toastSuccess } from '../../../../components/Toastify';
import { DialogTitle } from '@mui/material';

const DialogEditProfile = ({ open, setOpen }) => {
  const { user, isLoading } = useSelector((state) => state.auth);
  const accInfo = user.accountInfo;
  const baseServer = process.env.REACT_APP_SERVER_API;

  // useEffect(() => {
  //   async function fetchType() {
  //     const response = await axios.get(`${baseServer}/authentication`);
  //   }
  //   fetchType();
  // }, []);
  const schema = yup.object().shape({
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
    dateBirth: yup.string().required('Nhập ngày sinh'),
    phone: yup
      .string()
      .matches('^0[0-9]{9,11}$', 'Số điện thoại có độ dài 10-12')
      .required('Nhập số điện thoại'),
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

  const formik = useFormik({
    initialValues: {
      firstName: accInfo.firstName,
      lastName: accInfo.lastName,
      dateBirth: accInfo.dateBirth,
      phone: accInfo.phone,
      street: accInfo.address.street,
      district: accInfo.address.district,
      city: accInfo.address.city,
    },
    validationSchema: schema,
    onSubmit: async (values) => {
      const result = await axios.post(`${baseServer}/authentication`, values);
      if (result.data.statusCode === 200) {
        toastSuccess(result.data.message);
        Navigate('/login');
      } else {
        toastError('Something went wrong');
      }
      console.log('result: ', result);
    },
  });

  return (
    <div>
      <Dialog open={open} onClose={() => setOpen(false)}>
        <DialogTitle id='alert-dialog-title'>
          <div className='bg-gradient-to-r from-blue-500 to-fuchsia-500 text-center text-white font-bold p-2 rounded capitalize'>
            Chỉnh sửa thông tin cá nhân
          </div>
        </DialogTitle>
        <div className='py-4 px-8 flex flex-col gap-4'>
          <div className='flex gap-4'>
            {/* lastName */}
            <div className='flex-1'>
              <TextField
                id='field'
                label='Họ'
                fullWidth
                variant='outlined'
                onChange={formik.handleChange('lastName')}
                onBlur={formik.handleBlur('lastName')}
                value={formik.values.lastName}
              />

              <div className='error text-red-900'>
                {formik.touched.lastName && formik.errors.lastName}
              </div>
            </div>
            {/* firstName */}
            <div className='flex-1'>
              <TextField
                id='field'
                label='Tên'
                fullWidth
                variant='outlined'
                onChange={formik.handleChange('firstName')}
                onBlur={formik.handleBlur('firstName')}
                value={formik.values.firstName}
              />
              <div className='error text-red-900'>
                {formik.touched.firstName && formik.errors.firstName}
              </div>
            </div>
          </div>

          {/* dateBirth */}
          <div>
            <div className='flex gap-4 justify-between items-center '>
              <p className='whitespace-nowrap text-lg'>Ngày sinh</p>
              <TextField
                id='field'
                type='date'
                fullWidth
                variant='outlined'
                onChange={formik.handleChange('dateBirth')}
                onBlur={formik.handleBlur('dateBirth')}
                value={formik.values.dateBirth}
              />
            </div>
            <div className='error text-red-900'>
              {formik.touched.dateBirth && formik.errors.dateBirth}
            </div>
          </div>

          {/* phone */}
          <div>
            <TextField
              id='field'
              label='Số điện thoại'
              fullWidth
              variant='outlined'
              onChange={formik.handleChange('phone')}
              onBlur={formik.handleBlur('phone')}
              value={formik.values.phone}
            />
            <div className='error text-red-900'>
              {formik.touched.phone && formik.errors.phone}
            </div>
          </div>

          {/* street */}
          <div>
            <TextField
              id='field'
              label='Đường'
              fullWidth
              variant='outlined'
              onChange={formik.handleChange('street')}
              onBlur={formik.handleBlur('street')}
              value={formik.values.street}
            />
            <div className='error text-red-900'>
              {formik.touched.street && formik.errors.street}
            </div>
          </div>

          {/* district */}
          <div>
            <TextField
              id='field'
              label='Huyện/Quận'
              fullWidth
              variant='outlined'
              onChange={formik.handleChange('district')}
              onBlur={formik.handleBlur('district')}
              value={formik.values.district}
            />
            <div className='error text-red-900'>
              {formik.touched.district && formik.errors.district}
            </div>
          </div>

          {/* city */}
          <div>
            <TextField
              id='field'
              label='Tỉnh/Thành phố'
              fullWidth
              variant='outlined'
              onChange={formik.handleChange('city')}
              onBlur={formik.handleBlur('city')}
              value={formik.values.city}
            />
            <div className='error text-red-900'>
              {formik.touched.city && formik.errors.city}
            </div>
          </div>
          <div className='flex gap-2 justify-end'>
            <button className='btnCancel' onClick={() => setOpen(false)}>
              Hủy
            </button>
            <button className='btn' onClick={() => setOpen(false)}>
              Hoàn tất
            </button>
          </div>
        </div>
      </Dialog>
    </div>
  );
};

export default DialogEditProfile;
