import React from 'react';
import { AiOutlineArrowRight } from 'react-icons/ai';
import {
  A11y,
  Autoplay,
  Navigation,
  Pagination,
  Scrollbar,
} from 'swiper/modules';
import { Swiper, SwiperSlide } from 'swiper/react';

const TopCategories = () => {
  return (
    <div className='mt-[120px] flex gap-4'>
      <div className=''>
        <div className='text-[36px] font-medium'>Explore Top Categories</div>
        <div>
          <button className='btn flex items-center justify-center mt-8'>
            View All Category <AiOutlineArrowRight size={20} />
          </button>
        </div>
      </div>

      <div className='flex-1 gap-4'>
        <Swiper
          modules={[Navigation, Pagination, Scrollbar, A11y, Autoplay]}
          slidesPerView={3}
          spaceBetween={20}
          navigation
          autoplay={{
            delay: 4000,
            disableOnInteraction: false,
          }}
          loop
          pagination={{ clickable: true }}
        >
          <SwiperSlide>
            <div className='p-8 border h-[240px] flex flex-col items-center justify-center '>
              <div>Driving Course</div>
              <div>40+ Courses</div>
              <button className='btn flex items-center justify-center mt-8 w-full'>
                Learn More <AiOutlineArrowRight />
              </button>
            </div>
          </SwiperSlide>
          Welcome
        </Swiper>
      </div>
    </div>
  );
};

export default TopCategories;
