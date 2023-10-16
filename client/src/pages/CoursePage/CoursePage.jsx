import React, { useEffect, useState } from 'react';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import { Link } from 'react-router-dom';
import BackgroundSlider from '../../components/BackgroundSlider';
import { BsFillArrowRightSquareFill } from 'react-icons/bs';
import theme from '../../theme';
import { BiSearch } from 'react-icons/bi';
import axiosClient from './../../utils/axiosClient';
import * as dayjs from 'dayjs';

const CoursePage = () => {
  const url =
    'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png';
  const breadcrumbs = 'Course';
  const url_server = process.env.REACT_APP_SERVER_API;
  const [courseList, setCourseList] = useState();

  useEffect(() => {
    async function fetchCourseList() {
      const response = await axiosClient.get(`${url_server}/courses`);
      console.log('response: ', response);
      setCourseList(response.data.data);
    }

    fetchCourseList();
  }, []);

  return (
    <>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />

      <form className='flex justify-center my-20'>
        <input
          placeholder='Nhập'
          className='w-[40%] rounded-l-lg bg-slate-100 outline-none pl-5 py-5 text-lg'
        />
        <button
          className={`center rounded-r-lg bg-[${theme.color.mainColor}] w-[5%] p-3 hover:bg-blue-900`}
        >
          <BiSearch size={24} color='white' />
        </button>
      </form>

      <div className='m-20'>
        <div className='flex flex-col gap-28'>
          {courseList &&
            courseList?.map((course, index) => (
              <div className='zoom'>
                <div className='w-[60%]'>
                  <div className='px-6 relative'>
                    <img
                      src='/img/course3.jpg'
                      alt='course'
                      className='imgClipPath w-full h-[500px] object-contain'
                    />
                    <div className='flex flex-col gap-5  absolute top-0 right-0 translate-x-2/3 pl-20 pr-32 '>
                      <div className='font-semibold text-5xl leading-snug'>
                        {course?.courseTitle}
                      </div>
                      <Link to={`/course/detail/${course.courseId}`}>
                        <button className='btn w-[40%]'>
                          Chi tiết khóa học
                        </button>
                      </Link>
                      <div className='pl-56 pt-3 flex flex-col gap-10'>
                        <div className='flex gap-5 text-2xl items-center'>
                          <BsFillArrowRightSquareFill
                            size={38}
                            color='#0d5ef4'
                            className='iconHover'
                          />
                          <div className='flex flex-col gap 5'>
                            <div>Kéo dài: {course.totalMonth} tháng</div>
                          </div>
                        </div>

                        <div className='flex gap-5 text-2xl items-center'>
                          <BsFillArrowRightSquareFill
                            size={34}
                            color='#0d5ef4'
                            className='iconHover'
                          />
                          <div className='flex flex-col gap 5'>
                            <div>Tổng tiết học: {course.totalSession}</div>
                          </div>
                        </div>

                        <div className='flex gap-5 text-2xl items-center'>
                          <BsFillArrowRightSquareFill
                            size={38}
                            color='#0d5ef4'
                            className='iconHover'
                          />
                          <div className='flex flex-col gap 5'>
                            <div>
                              Ngày bắt đầu:{' '}
                              {dayjs(course.startDate).format('DD-MM-YYYY')}
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            ))}
        </div>
      </div>
      <Footer />
    </>
  );
};

export default CoursePage;
