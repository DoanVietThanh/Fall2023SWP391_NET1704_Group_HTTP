import React, { useEffect } from 'react';
import { AiFillCloseCircle } from 'react-icons/ai';
import { Link } from 'react-router-dom';
import { toastSuccess } from '../../components/Toastify';
const PaymentFail = () => {
  useEffect(() => {
    toastSuccess('Thanh toán thành công');
  }, []);

  return (
    <div className='w-[100vw] h-[100vh] overflow-hidden flex justify-center items-center bg-[#EEF5FF]'>
      <div className='w-[50vw] h-[40vh] border text-center rounded-lg p-8 bg-white'>
        <div className='flex flex-col justify-evenly items-center h-full'>
          <div className='flex justify-center items-center'>
            <AiFillCloseCircle className='text-red-400 text-[16vh]' />
          </div>
          <h1 className='font-bold text-[30px] my-4'>
            Bạn đã thanh toán thất bại
          </h1>
          <div className='flex justify-center gap-4'>
            <Link to={`/`}>
              <button className='btn'>Quay về trang chủ</button>
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export default PaymentFail;
