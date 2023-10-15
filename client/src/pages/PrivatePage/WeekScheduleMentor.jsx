import { Box } from '@mui/material';
import React, { useEffect, useState } from 'react';
import axiosClient from './../../utils/axiosClient';

import { useSelector } from 'react-redux';
import SideBar from '../../components/SideBar';
import Loading from './../../components/Loading';

import ScheduleMentor from './components/ScheduleMentor';
import SearchInput from '../../components/SearchInput';

const WeekScheduleMentor = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [listCourse, setListCourse] = useState([]);
  const { accountInfo } = useSelector((state) => state.auth.user);

  useEffect(() => {
    async function getDataCourse() {
      try {
        const resListCourse = await axiosClient.get(
          `/staffs/mentors/${accountInfo.staffId}/courses`
        );
        console.log('resListCourse: ', resListCourse);
        setListCourse(resListCourse?.data?.data);
        setIsLoading(false);
      } catch (error) {
        throw new Error(error);
      }
    }
    getDataCourse();
  }, [isLoading]);

  return (
    <div className='flex'>
      <SideBar />
      {isLoading ? (
        <Loading />
      ) : (
        <div className='flex flex-col w-full'>
          {listCourse && (
            <>
              <Box component='main' sx={{ flexGrow: 1, p: 3 }}>
                <div className='h-[80vh] w-full rounded overflow-y-auto mt-[64px]'>
                  <div>
                    <SearchInput placeholder={'Tìm kiếm...'} />
                    <div className=''>
                      {listCourse?.map((itemCourse, index) => (
                        <ScheduleMentor
                          itemCourse={itemCourse}
                          courseId={itemCourse.courseId}
                        />
                      ))}
                    </div>
                  </div>
                </div>
              </Box>
            </>
          )}
        </div>
      )}
    </div>
  );
};

export default WeekScheduleMentor;
