import React from 'react';
import { AiOutlineArrowRight } from 'react-icons/ai';

const LastestBlog = () => {
  return (
    <div>
      <div className='flex justify-center items-center'>
        <div className='text-[36px] font-medium'>Lastest News & Blogs</div>
        <button className='btn flex items-center justify-center mt-8'>
          View All Category <AiOutlineArrowRight size={20} />
        </button>
      </div>
    </div>
  );
};

export default LastestBlog;
