import React from 'react';
import { Link } from 'react-router-dom';
const ErrorPage = () => {
  return (
    <div className='h-[100vh] w-[100vw] center flex-col'>
      <h1 className='text-center font-bold text-[100px] uppercase'>
        404 - page not found
      </h1>
      <p className='text-center font-medium text-[60px] my-8 capitalize'>
        we can't seem to find the page you're looking for.
      </p>
      <div className='flex-x justify-center'>
        <Link to={`/home`}>
          <button className='btn text-[40px]'>Trở về trang chủ</button>
        </Link>
      </div>
    </div>
  );
};

export default ErrorPage;
