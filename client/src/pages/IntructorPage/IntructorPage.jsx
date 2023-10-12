import React from 'react';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import { AiFillStar } from 'react-icons/ai';
import BackgroundSlider from '../../components/BackgroundSlider';
import { Link } from 'react-router-dom';
import { BiSearch } from 'react-icons/bi';
import theme from '../../theme';

const instructorList = [
  {
    id: 'intructor1',
    src: 'https://scontent.fsgn2-3.fna.fbcdn.net/v/t39.30808-6/326718942_3475973552726762_6277150844361274430_n.jpg?_nc_cat=107&ccb=1-7&_nc_sid=a2f6c7&_nc_ohc=Qwrn79_gzkQAX-vfpDx&_nc_ht=scontent.fsgn2-3.fna&oh=00_AfDJ2wJIJEh7ouiidTlGa17nqLA4GeHeyyJzTum3K_gN7g&oe=6522852C',
    fullname: 'Đoàn Viết Thanh',
    ratingStar: 5,
  },
  {
    id: 'intructor2',
    src: 'https://scontent.fsgn5-6.fna.fbcdn.net/v/t1.6435-9/123116045_772985256617271_6289105269278104549_n.jpg?_nc_cat=108&ccb=1-7&_nc_sid=ad2b24&_nc_ohc=laebF5sIFUUAX8VFESQ&_nc_ht=scontent.fsgn5-6.fna&oh=00_AfBOKBYLyQ6D7SQRwsFXimrfuUuKrPYLyfKst_XNlTvmIw&oe=65353413',
    fullname: 'Nguyễn Vũ Quang Huy',
    ratingStar: 4,
  },
  {
    id: 'intructor3',
    src: 'https://scontent.fsgn2-9.fna.fbcdn.net/v/t39.30808-6/350126583_273968988360444_515949382367821556_n.jpg?stp=cp6_dst-jpg_s851x315&_nc_cat=105&ccb=1-7&_nc_sid=0df3a7&_nc_ohc=7b8g2aPSWp8AX-_0Ys2&_nc_ht=scontent.fsgn2-9.fna&oh=00_AfAq-AOW5jIXwBu713uTKup1eR7zfgrA8QazfUNX9SXUaA&oe=652299DA',
    fullname: 'Bùi Trần Thanh Thư',
    ratingStar: 5,
  },
  {
    id: 'intructor4',
    src: 'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/team_1_4.png',
    fullname: 'Lê Xuân Phước',
    ratingStar: 3,
  },
  {
    id: 'intructor4',
    src: 'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/team_1_4.png',
    fullname: 'Lê Xuân Phước',
    ratingStar: 3,
  },
  {
    id: 'intructor5',
    src: 'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/team_1_4.png',
    fullname: 'Lê Xuân Phước',
    ratingStar: 3,
  },
  {
    id: 'intructor6',
    src: 'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/team_1_4.png',
    fullname: 'Lê Xuân Phước',
    ratingStar: 3,
  },
];

const IntructorPage = () => {
  const url =
    'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png';
  const breadcrumbs = 'Instructors';
  return (
    <>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />

      <form className='flex justify-center my-20'>
        <input
          placeholder='Nhập tên giảng viên ...'
          className='w-[40%] rounded-l-lg bg-slate-100 outline-none pl-5 py-5 text-lg'
        />
        <button
          className={`center rounded-r-lg bg-[${theme.color.mainColor}] w-[5%] p-3 hover:bg-blue-900`}
        >
          <BiSearch size={24} color='white' />
        </button>
      </form>

      <div className='m-24 flex flex-wrap '>
        {instructorList.map((instructor, index) => (
          <div className='imgBackground w-[30%] flex flex-col p-14 m-5 uppercase gap-10 bg-gray-100'>
            <img
              src={instructor.src}
              alt={instructor.id}
              className='zoom w-full h-[400px] object-cover'
            ></img>

            <div className='flex flex-col gap-3'>
              <Link to={'/instructor/detail'}>
                <div className='font-semibold text-xl hover:text-blue-600'>
                  {instructor.fullname}
                </div>
              </Link>

              <div className='flex gap-2 text-lg text-gray-500'>
                <div>Đánh giá:</div>
                <div className='flex gap-2 items-center'>
                  {instructor.ratingStar}
                  <AiFillStar size={26} color='#F6F669'></AiFillStar>
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>

      <Footer />
    </>
  );
};

export default IntructorPage;
