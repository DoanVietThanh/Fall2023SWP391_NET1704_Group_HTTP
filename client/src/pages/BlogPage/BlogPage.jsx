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
            {listBlog.map((blog, index) => (
              <div className='flex pt-10 gap-3'>
                <div className='flex flex-col gap-2'>
                  <Link to={blog.link}>
                    <div className='text-lg font-bold hover:text-blue-500 overflow-hidden whitespace-nowrap overflow-ellipsis max-w-xs'>
                      {blog?.title}
                    </div>
                  </Link>
                  <div className='flex items-center text-md font-light gap-2 text-blue-500 hover:text-blue-900 '>
                    <AiOutlineCalendar />
                    {blog?.createDate}
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
