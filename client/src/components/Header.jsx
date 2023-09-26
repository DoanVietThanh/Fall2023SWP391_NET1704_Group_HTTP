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
import { Button, Menu, MenuItem } from '@mui/material';

const Header = () => {
  const { user } = useSelector((state) => state.auth);
  const [anchorEl, setAnchorEl] = React.useState(null);
  const open = Boolean(anchorEl);
  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };
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
            <div>
              <Button
                id='basic-button'
                aria-controls={open ? 'basic-menu' : undefined}
                aria-haspopup='true'
                aria-expanded={open ? 'true' : undefined}
                onClick={handleClick}
              >
                <div className='flex justify-center items-center'>
                  <div className='flex justify-center items-center'>
                    <img
                      src='https://scontent.fsgn5-10.fna.fbcdn.net/v/t39.30808-6/326718942_3475973552726762_6277150844361274430_n.jpg?_nc_cat=107&ccb=1-7&_nc_sid=a2f6c7&_nc_ohc=OAw9KYbwIYQAX9p4Tu9&_nc_ht=scontent.fsgn5-10.fna&oh=00_AfA86LZGE5THISarUceKKdD_G35FxPWHNR0dsFrdfrlnAQ&oe=6512B32C'
                      alt='Avatar'
                      className='rounded-full w-[40px] h-[40px] object-cover'
                    />
                  </div>
                  <h2 className='text-white text-[20px] pl-2'>
                    {`${user.accountInfo?.firstName} ${user.accountInfo?.lastName}`}
                  </h2>
                </div>
              </Button>
              <Menu
                id='basic-menu'
                anchorEl={anchorEl}
                open={open}
                onClose={handleClose}
                MenuListProps={{
                  'aria-labelledby': 'basic-button',
                }}
              >
                <MenuItem onClick={handleClose}>
                  <Link to={`/profile`}>Profile</Link>
                </MenuItem>
                <MenuItem onClick={handleClose}>My account</MenuItem>
                <MenuItem onClick={handleClose}>Logout</MenuItem>
              </Menu>
            </div>
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
          <img
            className='h-[80px] object-contain'
            src={images.logo}
            alt='logo'
          />
        </div>
        <div className='flex gap-16 text-[16px] font-medium uppercase'>
          <Link to='/'>
            <div>Home</div>
          </Link>
          <Link to='/course'>
            <div>Course</div>
          </Link>
          <Link to='/intructor'>
            <div>Intructors</div>
          </Link>
          <Link to='/document'>
            <div>Documents</div>
          </Link>
          <Link to='/theory'>
            <div>Theory</div>
          </Link>
          <Link to='/blogs'>
            <div>Blog</div>
          </Link>
          <Link to='/contact'>
            <div>Contact</div>
          </Link>
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
