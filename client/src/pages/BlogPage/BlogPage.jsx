import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { AiOutlineCalendar } from 'react-icons/ai';
import { BiSearch } from 'react-icons/bi';
import { Link } from 'react-router-dom';
import BackgroundSlider from '../../components/BackgroundSlider';
import Footer from './../../components/Footer';
import Header from './../../components/Header';
import BlogList from './component/BlogList';
import { toastError } from '../../components/Toastify';
import theme from '../../theme';
import * as dayjs from 'dayjs';

const BlogPage = () => {
  const url = '/img/backgroundSlide.png';
  const breadcrumbs = 'Bài đăng';
  const [searchInput, setSearchInput] = useState('');
  const [listBlog, setListBlog] = useState([]);

  const urlService = process.env.REACT_APP_SERVER_API;
  const handleSearchBlog = async (e) => {
    e.preventDefault();
    await axios
      .get(`${urlService}/blog/search/${searchInput}`)
      .then((res) => {
        setListBlog(res?.data?.data);
        setSearchInput('');
        console.log('res?.data?.data: ', res?.data?.data);
      })
      .catch((error) => {
        console.log('error: ', error);
        toastError(error?.response?.data?.message);
      });
  };
  useEffect(() => {
    async function getListBlog() {
      await axios
        .get(`${urlService}/blog`)
        .then((res) => {
          console.log('res: ', res);
          setListBlog(res.data?.data);
        })
        .catch((error) => {
          console.log('error: ', error);
          toastError(error?.response?.data?.message);
        });
    }
    getListBlog();
  }, []);
  return (
    <div>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      <div className='flex justify-center m-8'>
        <div className='flex-1'>
          <BlogList listBlog={listBlog} />
        </div>

        <div className='w-[36%] ml-10'>
          <div className='border drop-shadow-md rounded-lg p-10 mb-20'>
            <div className='border-b-[4px] text-2xl font-bold pb-2 '>
              Tìm kiếm
            </div>
            <div>
              <form className='flex mt-5' onSubmit={(e) => handleSearchBlog(e)}>
                <input
                  placeholder='Nhập tiêu đề hoặc tên người đăng'
                  className='flex-1 rounded-l-lg bg-slate-100 outline-none pl-5'
                  value={searchInput}
                  onChange={(e) => setSearchInput(e.target.value)}
                />
                <button
                  type='submit'
                  className={`flex justify-center rounded-r-lg bg-[${theme.color.mainColor}] text-white font-bold w-[20%] p-3 hover:bg-blue-900`}
                >
                  <BiSearch size={24} />
                </button>
              </form>
            </div>
          </div>

          <div className='border drop-shadow-md rounded-lg p-10 mb-20'>
            <div className='border-b-[4px] text-2xl font-bold pb-2 '>
              Bài đăng gần đây
            </div>
            {listBlog.slice(0, 5).map((blog, index) => (
              <div className='flex pt-10 gap-3 items-center'>
                <img
                  src={blog.image}
                  alt='blogPic'
                  className='w-[25%] object-cover'
                />
                <div className='flex-1 flex-col gap-2'>
                  <Link to={blog.link}>
                    <div className='text-xl font-bold hover:text-blue-500 overflow-hidden whitespace-nowrap overflow-ellipsis max-w-xs'>
                      {/* {blog?.title.split(" ").slice(0, 5).join(" ")}
                      {blog?.title.split(" ").length > 5 && "..."} */}
                      {blog?.title.length > 25
                        ? `${blog?.title.slice(0, 25)}...`
                        : blog?.title}
                    </div>
                  </Link>
                  <div className='flex items-center text-lg font-light gap-2 text-blue-500 hover:text-blue-900 '>
                    <AiOutlineCalendar size={24} />{' '}
                    {dayjs(blog?.createDate).format('DD/MM/YYYY')}
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default BlogPage;
