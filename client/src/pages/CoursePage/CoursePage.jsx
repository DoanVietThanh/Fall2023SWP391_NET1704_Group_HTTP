import React from 'react';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import { Link } from 'react-router-dom';
import BackgroundSlider from '../../components/BackgroundSlider';

const courseList = [
  {
    id: 'courseItem1',
    src: '/img/course1.png',
    licenseType: 'B1',
    duration: 8,
    registerMember: 100,
  },
  {
    id: 'courseItem2',
    src: '/img/course2.png',
    licenseType: 'B1',
    duration: 90,
    registerMember: 100,
  },
  {
    id: 'courseItem3',
    src: '/img/course3.png',
    licenseType: 'B2',
    duration: 80,
    registerMember: 25,
  },
];

const CoursePage = () => {
  const url =
    'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png';
  const breadcrumbs = 'Course';
  return (
    <>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      <h1 className='text-center font-bold text-[26px] pt-8'>List Courses</h1>
      <div className='p-8 flex justify-center flex-col items-center gap-8'>
        {courseList.map((course, index) => (
          <div
            key={course.id}
            className='flex border-2 h-auto w-auto px-[30px]'
          >
            <div className='px-6'>
              <img
                src={course.src}
                alt='course'
                className='w-[200px] h-[200px] object-contain'
              />
            </div>
            <div className='flex-1 p-6 flex flex-col gap-4 justify-between items-center'>
              <p>
                License Type:{' '}
                <span className='font-medium'>{course.licenseType}</span>
              </p>
              <p>
                Duration:{' '}
                <span className='font-medium'>{course.duration} slots</span>
              </p>
              <p>
                Register:
                <span className='font-medium'>
                  {' '}
                  {course.registerMember} register
                </span>
              </p>
              <Link to={`detail`}>
                <button className='btn'>Read More</button>
              </Link>
            </div>
          </div>
        ))}
      </div>
      <Footer />
    </>
  );
};

export default CoursePage;
