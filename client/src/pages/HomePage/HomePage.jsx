import React from 'react';
import Header from '../../components/Header';
import Footer from './../../components/Footer';
import LastestBlog from './LastestBlog';
import RatedMentor from './RatedMentor';
import Slider from './Slider';
import Welcome from './Welcome';

const HomePage = () => {
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
