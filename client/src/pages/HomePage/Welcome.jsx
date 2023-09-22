import React from 'react';
import { AiOutlineArrowRight, AiFillCheckCircle } from 'react-icons/ai';
import theme from '../../theme';
import { BsBookmarkStarFill } from 'react-icons/bs';
const Welcome = () => {
  return (
    <div className='my-[120px] mx-8 flex gap-4'>
      <div className='flex-1 border border-1 overflow-hidden'>
        <div className='relative w-full h-[460px]'>
          <div className='absolute rounded-lg top-0 w-full'>
            <img
              src='/img/about2.png'
              alt='About'
              className='w-full object-cover'
            />
          </div>

          <div className='absolute right-0'>
            <img src='/img/about4.png' alt='About' />
          </div>
        </div>
      </div>

      <div className='flex-1'>
        <h3
          className={`text-[16px] text-[${theme.color.mainColor}] flex items-center gap-2 font-bold`}
        >
          <BsBookmarkStarFill />
          About Our Application
        </h3>
        <div className='text-[36px] font-medium capitalize'>About us</div>
        <p className='leading-7 mt-2'>
          Collaboratively simplify user friendly networks after principle
          centered coordinate effective methods of empowerment distributed niche
          markets pursue market positioning web-readiness after resource sucking
          applications.
        </p>
        <p className='leading-7 mt-2'>
          Online education, also known as e-learning, is a method of learning
          that takes place over the internet. It offers individuals the
          opportunity to acquire knowledge, skills.
        </p>
        <div className='flex gap-4 my-4'>
          <img src='/img/about1.png' alt='class' />
          <ul>
            <li className='flex items-center gap-2'>
              <AiFillCheckCircle className='text-blue-400' />
              <span className='font-medium '>
                Get access to 4,000+ of our top courses
              </span>
            </li>
            <li className='flex items-center gap-2'>
              <AiFillCheckCircle className='text-blue-400' />
              <span className='font-medium '>Popular topics to learn now</span>
            </li>
            <li className='flex items-center gap-2'>
              <AiFillCheckCircle className='text-blue-400' />
              <span className='font-medium '>
                Find the right instructor for you
              </span>
            </li>
          </ul>
        </div>
        <button className='btn flex items-center justify-center gap-2'>
          Read More <AiOutlineArrowRight size={20} />
        </button>
      </div>
    </div>
  );
};

export default Welcome;
