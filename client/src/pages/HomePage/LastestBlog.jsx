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

const listBlog = [
  {
    id: 1,
    date: '15th Nov 2023',
    src: '/img/blog1.png',
    author: 'Quang Huy',
    title: 'Những điều cần biết khi thi bằng lái B1',
  },
  {
    id: 2,
    date: '16th Nov 2023',
    src: '/img/blog2.png',
    author: 'Thanh Doan',
    title: 'Những điều cần biết khi thi bằng lái B1',
  },
  {
    id: 3,
    date: '17th Nov 2023',
    src: '/img/blog3.png',
    author: 'Xuân Phước',
    title: 'Những điều cần biết khi thi bằng lái B1',
  },
  {
    id: 4,
    date: '18th Nov 2023',
    src: '/img/blog4.png',
    author: 'Thanh Thư',
    title: 'Những điều cần biết khi thi bằng lái B1',
  },
];

const LastestBlog = () => {
  return (
    <div className='my-8'>
      <div className='my-8'>
        <p className='capitalize text-[16px] font-medium'>Our Blogs</p>
        <div className='flex items-center justify-between'>
          <div className='text-[36px] font-medium'>Lastest Blogs</div>
          <button className='btn flex items-center justify-center gap-2'>
            View All Blogs <AiOutlineArrowRight size={20} />
          </button>
        </div>
      </div>

      <div className='px-8'>
        <Swiper
          modules={[Pagination, Autoplay]}
          loop={true}
          slidesPerView={3}
          spaceBetween={80}
          autoplay={{
            delay: 2000,
            disableOnInteraction: false,
          }}
          pagination={{ clickable: true }}
        >
          {listBlog.map((blogItem, index) => (
            <SwiperSlide>
              <div className='border border-1 p-8'>
                <div className='pb-4'>
                  <img
                    src={blogItem.src}
                    alt='blog'
                    className='w-full h-[300px] object-cover rounded'
                  />
                </div>
                <div>
                  <div className='flex justify-between text-blue-300 py-2'>
                    <div className='flex items-center gap-2 font-medium '>
                      <MdDateRange size={20} />
                      <span className='text-black opacity-70 italic'>
                        15th Nov 2023
                      </span>
                    </div>
                    <div className='flex items-center gap-2 font-medium'>
                      <BsPerson size={20} />
                      <span className='text-black opacity-70 italic'>
                        {blogItem.author}
                      </span>
                    </div>
                  </div>
                  <h3 className='text-[24px] font-medium py-2'>
                    Những điều cần biết khi thi bằng lái B1
                  </h3>
                  <button className='btn flex items-center justify-center gap-2'>
                    Read More <AiOutlineArrowRight size={20} />
                  </button>
                </div>
              </div>
            </SwiperSlide>
          ))}
        </Swiper>
      </div>
    </div>
  );
};

export default LastestBlog;
