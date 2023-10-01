import React from 'react';
import * as dayjs from 'dayjs';

const Profile = ({ accountInfo }) => {
  console.log('🚀 ~ file: Profile.jsx:4 ~ Profile ~ accountInfo:', accountInfo);
  return (
    <div className='flex flex-col justify-between h-full'>
      <div className='relative mb-8'>
        <img
          src='/img/bg-profile2.jpg'
          alt=''
          className='h-[200px] object-cover w-full rounded-lg'
        />
        <img
          src={`/img/avtThanh.jpg`}
          alt='avt'
          className='absolute bottom-[-50px] left-8 w-[100px] h-[100px] object-cover rounded-full'
        />
      </div>
      <div className='grid grid-cols-2 gap-8 mt-8'>
        <div className='flex-x gap-4 w-full'>
          <span className='text-[20px] font-semibold'>Họ và tên:</span>
          <span className='text-[20px] font-normal'>
            {`${accountInfo.firstName} ${accountInfo.lastName}`}
          </span>
        </div>

        <div className='flex-x gap-4'>
          <span className='text-[20px] font-semibold'>Ngày sinh:</span>
          <span className='text-[20px] font-normal'>
            {dayjs(accountInfo.dateBirth).format('DD/MM/YYYY')}
          </span>
        </div>

        <div className='flex-x gap-4'>
          <span className='text-[20px] font-semibold'>Email:</span>
          <span className='text-[20px] font-normal'>{accountInfo.email}</span>
        </div>

        <div className='flex-x gap-4'>
          <span className='text-[20px] font-semibold'>Số điện thoại:</span>
          <span className='text-[20px] font-normal'>{accountInfo.phone}</span>
        </div>
        <div className='flex-x gap-4'>
          <span className='text-[20px] font-semibold'>Loại bằng lái:</span>
          <span className='text-[20px] font-normal'>
            {accountInfo.licenseType.licenseTypeDesc}
          </span>
        </div>
        <div className='flex-x gap-4'>
          <span className='text-[20px] font-semibold'>Địa chỉ:</span>
          <span className='text-[20px] font-normal'>{`${accountInfo.address.street}, ${accountInfo.address.district}, ${accountInfo.address.city}`}</span>
        </div>
      </div>
      <div className='flex justify-end w-full'>
        <button className='btn mt-4'>Chỉnh sửa</button>
      </div>
    </div>
  );
};

export default Profile;
