import React, { useEffect, useState } from 'react';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import { AiFillStar } from 'react-icons/ai';
import BackgroundSlider from '../../components/BackgroundSlider';
import { Link } from 'react-router-dom';
import { BiSearch } from 'react-icons/bi';
import theme from '../../theme';
import axiosClient from './../../utils/axiosClient';
import Loading from './../../components/Loading';
import { testJson } from '../../data';

const IntructorPage = () => {
  const url =
    'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png';
  const breadcrumbs = 'Instructors';
  const url_server = process.env.REACT_APP_SERVER_API;
  const [instructorList, setInstructorList] = useState([]);
  useEffect(() => {
    async function getListMentors() {
      const res = await axiosClient.get(`${url_server}/staffs/mentors`);
      setInstructorList(res?.data?.data.mentors);
    }
    getListMentors();
  }, []);

  console.log('instructorList: ', instructorList);

  return (
    <>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />

      <form className='flex justify-center my-20'>
        <input
          placeholder='Nhập tên giảng viên ...'
          className='w-[40%] rounded-l-lg bg-slate-100 outline-none pl-5 py-5 text-lg'
        />
        <button
          className={`center rounded-r-lg bg-[${theme.color.mainColor}] w-[5%] p-3 hover:bg-blue-900`}
        >
          <BiSearch size={24} color='white' />
        </button>
      </form>

      <div className='m-24 flex flex-wrap '>
        {instructorList ? (
          instructorList.map((instructor, index) => (
            <div className='imgBackground w-[30%] flex flex-col p-14 m-5 uppercase gap-10 bg-gray-100'>
              <img
                src={instructor.avatarImage}
                alt={instructor.id}
                className='zoom w-full h-[400px] object-cover'
              ></img>

              <div className='flex flex-col gap-3'>
                <Link to={`/instructor/detail/${instructor.staffId}`}>
                  <div className='font-semibold text-xl hover:text-blue-600'>
                    {`${instructor.firstName} ${instructor.lastName}`}
                  </div>
                </Link>

                <div className='flex gap-2 text-lg text-gray-500'>
                  <div>Đánh giá:</div>
                  <div className='flex gap-2 items-center'>
                    {instructor.ratingStar}
                    <AiFillStar size={26} color='#F6F669'></AiFillStar>
                  </div>
                </div>
              </div>
            </div>
          ))
        ) : (
          <Loading />
        )}
      </div>

      <Footer />
    </>
  );
};

export default IntructorPage;
