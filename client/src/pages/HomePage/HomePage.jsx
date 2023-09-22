import React from 'react';
import Header from '../../components/Header';
import { toastError } from '../../components/Toastify';
import Footer from './../../components/Footer';
import LastestBlog from './LastestBlog';
import Slider from './Slider';
import Welcome from './Welcome';
import RatedMentor from './RatedMentor';

const HomePage = () => {
  const notify = (message) => {
    toastError(message);
  };
  return (
    <>
      <Header />
      <Slider />
      <div className='px-4'>
        <Welcome />
        <RatedMentor />
        <LastestBlog />
      </div>
      <Footer />
    </>
  );
};

export default HomePage;
