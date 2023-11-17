import React, { useEffect, useState } from 'react';
import * as dayjs from 'dayjs';
import { Box } from '@mui/material';
import { useSelector } from 'react-redux';
import SideBar from '../../../components/SideBar';
import DialogEditProfile from './components/DialogEditProfile';
import DialogRegisterForm from './components/DialogRegisterForm';
import axiosClient from '../../../utils/axiosClient';
import { toastError } from '../../../components/Toastify';
import DialogCreatedRegisterForm from './components/DialogCreatedRegisterForm';

const Profile = () => {
  const { user } = useSelector((state) => state.auth);
  const accInfo = user.accountInfo;
  const [openEditProfile, setOpenEditProfile] = useState(false);
  const [openRegisterForm, setOpenRegisterForm] = useState(false);
  const [openCreatedRegisterForm, setOpenCreatedRegisterForm] = useState(false);
  const [licenseForm, setLicenseForm] = useState();

  useEffect(() => {
    async function getLicenseType() {
      await axiosClient
        .get(`/members/license-form`)
        .then((res) => {
          console.log(res);
          setLicenseForm(res?.data?.data);
        })
        .catch((error) => toastError(error?.response?.data?.message));
    }
    getLicenseType();
  }, []);

  console.log('accInfo.licenseForm: ', accInfo.licenseForm);

  return (
    <div className='flex'>
      <SideBar />
      <Box component='main' sx={{ flexGrow: 1, p: 3 }}>
        <div className='h-[80vh] w-full rounded overflow-y-auto mt-[64px]'>
          <div className='flex flex-col justify-between h-full'>
            <div className='relative mb-8'>
              <img
                src='/img/bg-profile2.jpg'
                alt=''
                className='h-[40vh] object-cover w-full rounded-lg'
              />
              <img
                src={user.accountInfo?.avatarImage}
                alt='avt'
                className='absolute bottom-[-90px] left-16 w-[180px] h-[180px] object-cover rounded-full border-8 border-white'
              />
            </div>
            <div className='grid grid-cols-2 gap-8 mt-20 ml-16'>
              <div className='flex-x gap-4 w-full'>
                <span className='text-[20px] font-semibold'>Họ và tên:</span>
                <span className='text-[20px] font-normal'>
                  {`${accInfo.firstName} ${accInfo.lastName}`}
                </span>
              </div>

              <div className='flex-x gap-4'>
                <span className='text-[20px] font-semibold'>Ngày sinh:</span>
                <span className='text-[20px] font-normal'>
                  {dayjs(accInfo.dateBirth).format('DD/MM/YYYY')}
                </span>
              </div>

              <div className='flex-x gap-4'>
                <span className='text-[20px] font-semibold'>Email:</span>
                <span className='text-[20px] font-normal'>{accInfo.email}</span>
              </div>

              <div className='flex-x gap-4'>
                <span className='text-[20px] font-semibold'>
                  Số điện thoại:
                </span>
                <span className='text-[20px] font-normal'>{accInfo.phone}</span>
              </div>
              {/* <div className='flex-x gap-4'>
                <span className='text-[20px] font-semibold'>
                  Loại bằng lái:
                </span>
                <span className='text-[20px] font-normal'>
                  {accInfo.licenseType.licenseTypeDesc}
                </span>
              </div> */}
              <div className='flex-x gap-4'>
                <span className='text-[20px] font-semibold'>Địa chỉ:</span>
                <span className='text-[20px] font-normal'>{`${accInfo.address.street}, ${accInfo.address.district}, ${accInfo.address.city}`}</span>
              </div>

              {accInfo?.licenseForm && (
                <div className='flex-x gap-4'>
                  <span
                    className='text-[20px] font-semibold cursor-pointer text-red-400 underline'
                    onClick={() => setOpenCreatedRegisterForm(true)}
                  >
                    Hồ sơ dự thi (
                    {`${accInfo.licenseForm?.registerFormStatus.registerFormStatusDesc}`}
                    )
                  </span>
                </div>
              )}
            </div>
            <div className='flex justify-end w-full gap-4 pr-4'>
              <button
                className='btn mt-4'
                onClick={() => setOpenEditProfile(true)}
              >
                Chỉnh sửa thông tin cá nhân
              </button>
              {accInfo?.licenseForm ? (
                <button className='btn-cancel mt-4 bg-[#dadada] cursor-text'>
                  Đang chờ duyệt hồ sơ
                </button>
              ) : (
                <button
                  className={`btn mt-4 ${
                    accInfo?.emailNavigation.role.roleId != 4 ? 'hidden' : ''
                  }`}
                  onClick={() => setOpenRegisterForm(true)}
                >
                  Tạo hồ sơ thi
                </button>
              )}
            </div>
          </div>

          <DialogEditProfile
            open={openEditProfile}
            setOpen={setOpenEditProfile}
          />

          {licenseForm && (
            <DialogRegisterForm
              open={openRegisterForm}
              setOpen={setOpenRegisterForm}
              accInfo={accInfo}
              licenseForm={licenseForm}
            />
          )}

          {accInfo?.licenseForm && (
            <DialogCreatedRegisterForm
              open={openCreatedRegisterForm}
              setOpen={setOpenCreatedRegisterForm}
              accInfo={accInfo}
            />
          )}
        </div>
      </Box>
    </div>
  );
};

export default Profile;
