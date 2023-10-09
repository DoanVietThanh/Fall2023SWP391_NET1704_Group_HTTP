import React from 'react';
import SideBar from '../../components/SideBar';
import { Box } from '@mui/material';

const WeekSchedule = () => {
  return (
    <div className='flex'>
      <SideBar />
      <Box component='main' sx={{ flexGrow: 1, p: 3 }}>
        <div className='h-[80vh] w-full rounded overflow-y-auto mt-[64px]'>
          <div>
            <h1 className='font-bold text-[30px] uppercase'>
              Lịch học theo tuần
            </h1>
          </div>
        </div>
      </Box>
    </div>
  );
};

export default WeekSchedule;
