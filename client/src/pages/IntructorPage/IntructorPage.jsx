import React from 'react';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import { AiFillStar } from 'react-icons/ai';
import BackgroundSlider from '../../components/BackgroundSlider';

const intructorList = [
  {
    id: 'intructor1',
    src: 'https://scontent.fsgn2-3.fna.fbcdn.net/v/t39.30808-1/326718942_3475973552726762_6277150844361274430_n.jpg?stp=c0.50.240.240a_dst-jpg_p240x240&_nc_cat=107&ccb=1-7&_nc_sid=fe8171&_nc_ohc=OAw9KYbwIYQAX8Q351_&_nc_ht=scontent.fsgn2-3.fna&oh=00_AfA0m_qQu-449kqs4UaWC1JuzrMSnoFuCiVU2ykWEBhhMg&oe=6512BAEA',
    fullname: 'Đoàn Viết Thanh',
    currentLisence: 'B1',
    ratingStar: 5,
  },
  {
    id: 'intructor2',
    src: 'https://scontent.fsgn2-3.fna.fbcdn.net/v/t39.30808-1/326718942_3475973552726762_6277150844361274430_n.jpg?stp=c0.50.240.240a_dst-jpg_p240x240&_nc_cat=107&ccb=1-7&_nc_sid=fe8171&_nc_ohc=OAw9KYbwIYQAX8Q351_&_nc_ht=scontent.fsgn2-3.fna&oh=00_AfA0m_qQu-449kqs4UaWC1JuzrMSnoFuCiVU2ykWEBhhMg&oe=6512BAEA',
    fullname: 'Nguyễn Vũ Quang Huy',
    currentLisence: 'B2',
    ratingStar: 4,
  },
  {
    id: 'intructor3',
    src: 'https://scontent.fsgn2-3.fna.fbcdn.net/v/t39.30808-1/326718942_3475973552726762_6277150844361274430_n.jpg?stp=c0.50.240.240a_dst-jpg_p240x240&_nc_cat=107&ccb=1-7&_nc_sid=fe8171&_nc_ohc=OAw9KYbwIYQAX8Q351_&_nc_ht=scontent.fsgn2-3.fna&oh=00_AfA0m_qQu-449kqs4UaWC1JuzrMSnoFuCiVU2ykWEBhhMg&oe=6512BAEA',
    fullname: 'Bùi Trần Thanh Thư',
    currentLisence: 'C',
    ratingStar: 5,
  },
  {
    id: 'intructor3',
    src: 'https://scontent.fsgn2-3.fna.fbcdn.net/v/t39.30808-1/326718942_3475973552726762_6277150844361274430_n.jpg?stp=c0.50.240.240a_dst-jpg_p240x240&_nc_cat=107&ccb=1-7&_nc_sid=fe8171&_nc_ohc=OAw9KYbwIYQAX8Q351_&_nc_ht=scontent.fsgn2-3.fna&oh=00_AfA0m_qQu-449kqs4UaWC1JuzrMSnoFuCiVU2ykWEBhhMg&oe=6512BAEA',
    fullname: 'Lê Xuân Phước',
    currentLisence: 'A2',
    ratingStar: 3,
  },
];

const IntructorPage = () => {
  const url =
    'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png';
  const breadcrumbs = 'Intructors';
  return (
    <>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      <h1 className='text-center font-bold text-[26px] pt-8'>
        List Intructors
      </h1>
      <div className='m-8 flex justify-center flex-col items-center gap-8'>
        {intructorList.map((course, index) => (
          <div
            key={course.id}
            className='flex border-2 h-auto w-[80%] px-[30px]'
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
                Họ và tên :{' '}
                <span className='font-medium'>{course.fullname}</span>
              </p>
              <p>
                Bằng lái hiện có :{' '}
                <span className='font-medium'>{course.currentLisence}</span>
              </p>
              <p className='flex'>
                Đánh giá :
                <span className='pl-4 font-medium flex items-center gap-2'>
                  {` ${course.ratingStar} `}
                  <AiFillStar />
                </span>
              </p>
            </div>
          </div>
        ))}
      </div>
      <Footer />
    </>
  );
};

export default IntructorPage;
