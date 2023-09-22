import React from 'react';
import { AiOutlineArrowRight } from 'react-icons/ai';
import 'swiper/css';
import 'swiper/css/navigation';
import 'swiper/css/pagination';
import 'swiper/css/scrollbar';
import {
  A11y,
  Autoplay,
  Navigation,
  Pagination,
  Scrollbar,
} from 'swiper/modules';
import { Swiper, SwiperSlide } from 'swiper/react';
import theme from '../../theme';

const Slider = () => {
  return (
    <Swiper
      modules={[Navigation, Pagination, Scrollbar, A11y, Autoplay]}
      slidesPerView={1}
      spaceBetween={50}
      navigation
      autoplay={{
        delay: 4000,
        disableOnInteraction: false,
      }}
      loop
      pagination={{ clickable: true }}
    >
      <SwiperSlide>
        <div className='w-full relative'>
          <div className='slider-img'>
            <img
              className='slider-bg '
              src={`/img/bgSlider1.png`}
              alt='slider'
            />
            <img
              className='slider-hero'
              src={`/img/heroSlider1.png`}
              alt='slider'
            />
            <img
              className='slider-decor'
              src={`/img/decorSlider.png`}
              alt='slider'
            />
          </div>
          <div className='slider-content flex flex-col justify-center'>
            <div className='flex flex-end items-center gap-4'>
              <button className='btn'>30% OFF</button>
            </div>
            <h1 className='font-bold text-[60px] leading-2 mt-8'>
              Education Is Create Better
              <span className={`text-[${theme.color.mainColor}]`}> Future</span>
            </h1>
            <p className='font-medium text-sky-950 text-[20px]'>
              Education can be thought of as the transmission of the values and
              accumulated knowledge of a society. In this sense, it is
              equivalent.
            </p>
            <button className='btn w-[200px] flex items-center justify-center gap-4 mt-4'>
              Get Started <AiOutlineArrowRight size={20} />
            </button>
          </div>
        </div>
      </SwiperSlide>
    </Swiper>
  );
};

export default Slider;
