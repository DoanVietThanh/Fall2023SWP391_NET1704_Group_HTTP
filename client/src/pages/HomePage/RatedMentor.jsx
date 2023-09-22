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
    src: 'https://scontent.fsgn5-6.fna.fbcdn.net/v/t1.6435-9/123116045_772985256617271_6289105269278104549_n.jpg?_nc_cat=108&ccb=1-7&_nc_sid=ad2b24&_nc_ohc=laebF5sIFUUAX8VFESQ&_nc_ht=scontent.fsgn5-6.fna&oh=00_AfBOKBYLyQ6D7SQRwsFXimrfuUuKrPYLyfKst_XNlTvmIw&oe=65353413',
    author: 'Quang Huy',
  },
  {
    id: 2,
    src: 'https://scontent.fsgn5-10.fna.fbcdn.net/v/t39.30808-6/326718942_3475973552726762_6277150844361274430_n.jpg?_nc_cat=107&ccb=1-7&_nc_sid=a2f6c7&_nc_ohc=OAw9KYbwIYQAX-wTCe1&_nc_ht=scontent.fsgn5-10.fna&oh=00_AfBz_yOzAZPnvN40lZQJsv-CiVAZbWax2rEMyl9l_I0WGg&oe=6512B32C',
    author: 'Thanh Doan',
  },
  {
    id: 3,
    src: 'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/team_1_4.png',
    author: 'Xuân Phước',
  },
  {
    id: 4,
    src: 'https://scontent.fsgn5-9.fna.fbcdn.net/v/t39.30808-6/350126583_273968988360444_515949382367821556_n.jpg?stp=cp6_dst-jpg&_nc_cat=105&ccb=1-7&_nc_sid=1b51e3&_nc_ohc=9tJ635VccQsAX-K3PyP&_nc_ht=scontent.fsgn5-9.fna&oh=00_AfBY4MUUcxxlwuuPUqxF5o-5HhRMRNkNOmUmk-JQrBIetw&oe=6512C7DA',
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
