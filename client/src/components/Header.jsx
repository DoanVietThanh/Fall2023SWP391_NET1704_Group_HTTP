import React, { useState } from 'react';
import theme from '../theme';
import images from '../assets/img';
import { Link } from 'react-router-dom';
import {
  AiOutlineArrowRight,
  AiOutlineClockCircle,
  AiOutlineMail,
} from 'react-icons/ai';
import {
  BsFacebook,
  BsLinkedin,
  BsTelephone,
  BsTwitter,
  BsYoutube,
} from 'react-icons/bs';
import { useSelector } from 'react-redux';

const Header = () => {
  const { user } = useSelector((state) => state.auth);
  return (
    <div className={`bg-[${theme.color.mainColor}] border-b-2`}>
      <div
        className={`flex justify-between p-4 text-[16px] text-white font-medium w-full`}
      >
        <div className='flex gap-4 divide-x '>
          <div className='flex items-center gap-2 pl-2 '>
            <BsTelephone /> +36 4745 067
          </div>
          <div className='flex items-center gap-2 pl-2 '>
            <AiOutlineMail /> http@fpt.edu.vn
          </div>
          <div className='flex items-center gap-2 pl-2'>
            <AiOutlineClockCircle />
            Mon - Sat: 8:00 - 20:00
          </div>
        </div>
        <div className='flex justify-center items-center gap-4'>
          Follow us
          <Link to='https://www.facebook.com/fullstack2k3/'>
            <BsFacebook />
          </Link>
          <BsTwitter />
          <BsLinkedin />
          <BsYoutube />
          {user?.accountInfo ? (
            <Link to={`/profile`} className='hover:text-rose-400'>
              {`${user.accountInfo?.firstName} ${user.accountInfo?.lastName}`}
            </Link>
          ) : (
            <Link to='/login'>
              <button className='btn-login ml-2 hover:opacity-80'>
                Login / Register
              </button>
            </Link>
          )}
        </div>
      </div>

      <div className='flex justify-between items-center p-8 bg-white rounded-tl-[50px]'>
        <div>
          <img className='h-[80px]' src={images.logo} alt='logo' />
        </div>
        <div className='flex gap-16 text-[16px] font-medium uppercase'>
          <Link to='/'>
            <div>Home</div>
          </Link>
          <div>Course</div>
          <div>Teachers</div>
          <div>Pages</div>
          <div>Blogs</div>
          <div>Contact</div>
        </div>
        <div>
          <button className='btn flex gap-2 items-center'>
            Contact us
            <AiOutlineArrowRight size={20} />
          </button>
        </div>
      </div>
    </div>
  );
};

export default Header;
