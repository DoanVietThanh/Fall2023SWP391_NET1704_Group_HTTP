import React from 'react';
import { AiOutlineArrowRight } from 'react-icons/ai';
import { BsPerson } from 'react-icons/bs';
import { MdDateRange } from 'react-icons/md';
import 'swiper/css';
import 'swiper/css/navigation';
import 'swiper/css/pagination';
import 'swiper/css/scrollbar';
import {
  Autoplay,
  Navigation,
  Pagination,
  Scrollbar,
  A11y,
} from 'swiper/modules';
import { Swiper, SwiperSlide } from 'swiper/react';

const listMentor = [
  {
    id: 1,
    src: 'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/team_1_1.png',
    author: 'Quang Huy',
  },
  {
    id: 2,
    src: 'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/team_1_2.png',
    author: 'Thanh Doan',
  },
  {
    id: 3,
    src: 'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/team_1_3.png',
    author: 'Xuân Phước',
  },
  {
    id: 4,
    src: 'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/team_1_4.png',
    author: 'Thanh Thư',
  },
];

const RatedMentor = () => {
  return (
    <div className='my-8'>
      <div className='m-8'>
        <div className='flex items-center justify-between'>
          <div className='text-[36px] font-medium'>Most Rated Mentor</div>
        </div>
      </div>

      <div className='px-8'>
        <Swiper
          modules={[Pagination, Autoplay]}
          loop={true}
          slidesPerView={4}
          spaceBetween={40}
          autoplay={{
            delay: 2000,
            disableOnInteraction: false,
          }}
          pagination={{ clickable: true }}
        >
          {listMentor.map((mentorItem, index) => (
            <SwiperSlide key={mentorItem.author}>
              <div className='border border-1 py-8'>
                <div className='pb-4'>
                  <img
                    src={mentorItem.src}
                    alt='blog'
                    className='w-full h-[300px] object-cover rounded'
                  />
                </div>
                <h2 className='font-bold text-center mb-2'>
                  {mentorItem.author}
                </h2>
                <button className='btn flex items-center justify-center gap-2 w-full'>
                  Read More <AiOutlineArrowRight size={20} />
                </button>
              </div>
            </SwiperSlide>
          ))}
        </Swiper>
      </div>
    </div>
  );
};

export default RatedMentor;
