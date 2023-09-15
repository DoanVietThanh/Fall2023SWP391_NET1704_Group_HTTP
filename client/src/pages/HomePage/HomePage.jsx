import React from 'react';
import { toastError, toastSuccess } from '../../components/Toastify';
import Header from '../../components/Header';
import LastestBlog from './LastestBlog';
import UpComingEvent from './UpComingEvent';
import MeetMentor from './MeetMentor';
import PopularCourses from './PopularCourses';
import Welcome from './Welcome';
import Slider from './Slider';
import TopCategories from './TopCategories';
import Footer from './../../components/Footer';

const HomePage = () => {
  const notify = (message) => {
    toastError(message);
  };
  return (
    <>
      <Header />
      <Slider />
      <div className='px-4'>
        <TopCategories />
        <Welcome />
        <PopularCourses />
        <MeetMentor />
        <UpComingEvent />
        <LastestBlog />
      </div>
      <Footer />
    </>
  );
};

export default HomePage;
