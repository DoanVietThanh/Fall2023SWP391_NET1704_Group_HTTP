import React, { useEffect } from 'react';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

const DashboardPage = () => {
  //   const navigate = useNavigate();
  //   const { user, isSuccess, isError, isLoading } = useSelector(
  //     (state) => state.auth
  //   );
  //   const role = user?.accountInfo.emailNavigation.role.roleId;
  //   useEffect(() => {
  //     if (isSuccess && role === 1) {
  //       navigate('/manager/dashboard');
  //     } else {
  //       navigate('/home');
  //     }
  //   }, [user, isSuccess, isError, isLoading]);
  return (
    <div className='flex w-[100vw] h-[100vh]'>
      <div className='w-[20%] border'></div>
      <div className='flex-1 border'></div>
    </div>
  );
};

export default DashboardPage;
