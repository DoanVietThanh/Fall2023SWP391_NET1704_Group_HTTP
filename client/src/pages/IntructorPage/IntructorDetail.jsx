import React, { useEffect, useState } from 'react';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import BackgroundSlider from '../../components/BackgroundSlider';
import {
  AiFillFacebook,
  AiFillInstagram,
  AiFillStar,
  AiFillTwitterSquare,
} from 'react-icons/ai';
import { BsFillArrowRightCircleFill } from 'react-icons/bs';
import axios from 'axios';
import { Link, useParams } from 'react-router-dom';
import axiosClient from '../../utils/axiosClient';
import Loading from '../../components/Loading';

const IntructorDetail = () => {
  const url_server = process.env.REACT_APP_SERVER_API;
  const url =
    'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png';
  const breadcrumbs = 'Instructor Detail';

  const { idInstructor } = useParams();
  const [instructor, setInstructor] = useState();
  useEffect(() => {
    async function getInstructor() {
      const res = await axiosClient.get(`${url_server}/staffs/${idInstructor}`);
      setInstructor(res?.data?.data);
      console.log(
        '🚀 ~ file: IntructorDetail.jsx:28 ~ getInstructor ~ res:',
        res?.data
      );
    }
    getInstructor();
  }, []);

  console.log('instructor: ', instructor);

  return (
    <div>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      {instructor ? (
        <div className='flex m-20 gap-24'>
          <div className='w-[40%]'>
            <img
              src='/img/avtThanh.jpg'
              alt={`${instructor.firstName} ${instructor.lastName}`}
              className='zoom p-5 w-full h-[700px] object-cover'
            ></img>
          </div>
          <div className='w-[60%] flex flex-col mt-10 gap-8'>
            <div className='flex flex-col gap-2 mb-5'>
              <div className='font-semibold text-3xl'>{`${instructor.firstName} ${instructor.lastName}`}</div>
              <div className='font-light text-2xl text-gray-600'>
                {instructor?.jobTitle.jobTitleDesc}
              </div>
            </div>

            <div className='flex flex-col gap-2'>
              <div className='font-semibold text-2xl mb-4'>Giới thiệu</div>
              <div className='font-light text-xl text-gray-600 max-h-[40vh] overflow-y-auto'>
                {instructor?.selfDescription}
              </div>
            </div>

            <div className='flex flex-col gap-2'>
              <div className='font-semibold text-2xl'>
                Các khóa học đang đứng lớp
              </div>
              <div className='font-light text-xl text-gray-600'>
                <ul className='flex flex-col gap-6 mt-4'>
                  {instructor?.courses?.map((course, indexCourse) => (
                    <Link
                      key={indexCourse}
                      to={`/instructor/teaching-schedule/${instructor?.staffId}/${course?.courseId}`}
                    >
                      <li className='hover:text-blue-700 flex items-center gap-4'>
                        <BsFillArrowRightCircleFill
                          size={20}
                          className='text-blue-500'
                        />{' '}
                        <span className='text-blue-500'>
                          {course.courseTitle}
                        </span>
                      </li>
                    </Link>
                  ))}
                </ul>
              </div>
            </div>

            <div className='flex flex-col gap-2'>
              <div className='font-semibold text-2xl'>Đánh giá</div>
              <div className='flex gap-1 items-center'>
                <AiFillStar size={26} color='#F6F669'></AiFillStar>
                <AiFillStar size={26} color='#F6F669'></AiFillStar>
                <AiFillStar size={26} color='#F6F669'></AiFillStar>
                <AiFillStar size={26} color='#F6F669'></AiFillStar>
                <AiFillStar size={26} color='#F6F669'></AiFillStar>
              </div>
            </div>

            <div className='flex flex-col gap-2'>
              <div className='font-semibold text-2xl'>Liên lạc</div>
              <div className='flex gap-5 items-center'>
                <AiFillFacebook size={34} color='#1976D2' />
                <AiFillInstagram size={34} color='#1976D2' />
                <AiFillTwitterSquare size={34} color='#1976D2' />
              </div>
            </div>
          </div>
        </div>
      ) : (
        <Loading />
      )}

      <Footer />
    </div>
  );
};

export default IntructorDetail;
