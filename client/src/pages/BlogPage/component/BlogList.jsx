import dayjs from 'dayjs';
import React, { useState } from 'react';
import { AiOutlineArrowRight, AiOutlineClockCircle } from 'react-icons/ai';
import { BsPerson, BsTags } from 'react-icons/bs';
import { useNavigate } from 'react-router-dom';
import theme from '../../../theme';

const BlogList = ({ listBlog }) => {
  const navigate = useNavigate();

  return (
    <div className='flex flex-col gap-10'>
      {listBlog?.map((blog, index) => (
        <div className='border drop-shadow-md rounded-lg p-8 '>
          <div className='flex justify-center h-[60vh]'>
            <img
              src={blog?.image}
              alt='blogPic'
              className='w-full object-cover'
            />
          </div>
          <div className='mt-8'>
            <div
              className={`flex text-xl pb-8 text-[${theme.color.mainColor}] `}
            >
              <div className='flex items-center gap-3 pr-4'>
                <BsPerson size={24} /> by{' '}
                {`${blog?.staff?.firstName} ${blog?.staff?.lastName}`}
              </div>
              <div className='flex items-center gap-3 border-l-[2px] px-4'>
                <AiOutlineClockCircle size={24} />{' '}
                {dayjs(blog?.createDate).format('DD/MM/YYYY')}
              </div>
              <div className='flex items-center gap-3 border-l-[2px] px-4'>
                <BsTags size={24} /> Tags:
                {blog?.tags?.map((tag, index) => (
                  <span className='border-l-2' key={tag.tagId}>
                    {tag.tagName}
                  </span>
                ))}
              </div>
              {/* <div className='flex text-lg gap-2 items-center text-neutral-500 font-semibold '>
              <AiOutlineEye size={24} /> View: 99
            </div> */}
            </div>
            <div className='text-4xl font-bold pb-8'>{blog?.title}</div>
            <div
              className='font-light text-lg pb-8'
              dangerouslySetInnerHTML={{ __html: `${blog?.content}` }}
            ></div>
            <button
              className='btn flex items-center gap-2 uppercase'
              onClick={() => navigate(`/blog/${blog?.blogId}`)}
            >
              Xem thêm <AiOutlineArrowRight />
            </button>
          </div>
        </div>
      ))}
    </div>
  );
};

export default BlogList;
