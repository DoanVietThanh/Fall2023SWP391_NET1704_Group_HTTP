import * as React from 'react';
import { useFormik } from 'formik';
import { useEffect, useState } from 'react';
import * as yup from 'yup';
import axiosClient from '../../../utils/axiosClient';
import {
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  TextField,
} from '@mui/material';
import * as dayjs from 'dayjs';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import { toastSuccess } from '../../../components/Toastify';

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

const DialogEditUser = ({ open, setOpen, userId, loading, setLoading }) => {
  const [user, setUser] = useState();
  const [listType, setListType] = useState([]);
  const [selectType, setSelectType] = useState();
  const enableReinitialize = true;
  // const [formik, setFormik] = useState()
  const [availableValue, setAvailableValue] = useState();
  const [labelSelected, setLabelSelected] = useState([]);

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
      licenseTypeId: selectType,
    },
    enableReinitialize,
    validationSchema: schema,
    // onSubmit: async (values) => {
    //   console.log(123);
    //   console.log('values: ', values);
    //   await axiosClient.put(`/members/${userId}/update`, values);
    // },
  });
  useEffect(() => {
    async function getUser() {
      const res = await axiosClient.get(`/members/${userId}`);
      console.log(res?.data?.data.dateBirth);
      setUser(res?.data?.data);
      setAvailableValue({
        username: res?.data?.data.email,
        firstName: res?.data?.data.firstName,
        lastName: res?.data?.data.lastName,
        dateBirth: dayjs(res?.data?.data.dateBirth).format('YYYY-MM-DD'),
        phone: res?.data?.data.phone,
        street: res?.data?.data.address.street,
        district: res?.data?.data.address.district,
        city: res?.data?.data.address.city,
        licenseTypeId: res?.data?.data.licenseTypeId,
      });
      await axiosClient.get(`/members/add`).then((response) => {
        setListType(response?.data?.data?.licenseTypes);

        setLabelSelected(
          response?.data?.data?.licenseTypes.filter(
            (item) => item.licenseTypeId === res?.data?.data.licenseTypeId
          )
        );
      });
    }
    getUser();
  }, [userId]);

  console.log(formik.values);
  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const submitUpdateUser = async (e) => {
    console.log('formik.values: ', formik.values);
    e.preventDefault();
    const response = await axiosClient.put(
      `/members/${userId}/update`,
      formik.values
    );
    toastSuccess(response?.data?.data);
    setLoading(!loading);
    console.log('response: ', response);
  };
  return (
    <div>
      {availableValue && user && (
        <Dialog
          open={open}
          onClose={handleClose}
          aria-labelledby='alert-dialog-title'
          aria-describedby='alert-dialog-description'
        >
          <DialogTitle id='alert-dialog-title'>
            {'Sửa thông tin người dùng'}
          </DialogTitle>
          <DialogContent>
            <form
              action=''
              // onSubmit={formik.handleSubmit}
              onSubmit={(e) => submitUpdateUser(e)}
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

              {availableValue.licenseTypeId && (
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
                    {listType &&
                      listType?.map((item) =>
                        item.licenseTypeId === availableValue?.licenseTypeId ? (
                          <MenuItem
                            value={item.licenseTypeId}
                            key={item.lisenceTypeId}
                            selected
                          >
                            {item.licenseTypeDesc}
                          </MenuItem>
                        ) : (
                          <MenuItem
                            value={item.licenseTypeId}
                            key={item.lisenceTypeId}
                          >
                            {item.licenseTypeDesc}
                          </MenuItem>
                        )
                      )}
                  </Select>
                </FormControl>
              )}

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

              <button className='btn' type='submit'>
                Cập nhật
              </button>
            </form>
          </DialogContent>
        </Dialog>
      )}
    </div>
  );
};

export default DialogEditUser;
