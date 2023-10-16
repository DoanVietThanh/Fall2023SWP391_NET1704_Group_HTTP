import React, { useEffect } from 'react';
import { toastSuccess } from '../../components/Toastify';
import { Link } from 'react-router-dom';

const Payment = () => {
  useEffect(() => {
    toastSuccess('Thanh toán thành công');
  }, []);

  return (
    <div className='w-[100vw] h-[100vh] overflow-hidden flex justify-center items-center'>
      <div className='w-[50%] h-[50%] border text-center rounded-lg p-4 '>
        <div className='flex flex-col jusitfy-center items-center h-[100%]'>
          <h1 className='font-bold text-[30px]'>
            Chúc mừng bạn đã thanh toán thành công
          </h1>
          <div className='flex justify-center gap-4'>
            <Link to={`/`}>
              <button className='btn'>Quay về trang chủ</button>
            </Link>
            <Link to={`/week-schedule`}>
              <button className='btn'>Đăng ký lịch học</button>
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Payment;
