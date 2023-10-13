import { Button, Menu, MenuItem } from '@mui/material';
import React, { useState } from 'react';
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
import { useDispatch, useSelector } from 'react-redux';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import images from '../assets/img';
import { logout } from '../features/auth/authSlice';
import theme from '../theme';

const Header = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const currentLocation = useLocation().pathname;

  const [anchorEl, setAnchorEl] = useState(null);
  let { user } = useSelector((state) => state.auth);
  const open = Boolean(anchorEl);
  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = () => {
    dispatch(logout());
    navigate('/login');
  };

  const listNavigate = [
    {
      id: 1,
      title: 'Trang chủ',
      link: '/',
    },
    {
      id: 2,
      title: 'khóa học',
      link: '/course',
    },
    {
      id: 3,
      title: 'Giảng viên',
      link: '/instructor',
    },
    {
      id: 4,
      title: 'Tài liệu',
      link: '/document',
    },
    {
      id: 5,
      title: 'Lý thuyết',
      link: '/theory',
    },
    {
      id: 6,
      title: 'Bài đăng',
      link: '/blog',
    },
  ];

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
            Thứ 2 - Thứ 7: 8:00 - 20:00
          </div>
        </div>
        <div className='flex justify-center items-center gap-4 capitalize'>
          Theo dõi
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
                      src='/img/avtThanh.jpg'
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
                  <Link to={`/profile`}>Cá nhân</Link>
                </MenuItem>
                <MenuItem onClick={handleClose}>Tài khoản</MenuItem>
                <MenuItem onClick={handleLogout}>Đăng xuất</MenuItem>
              </Menu>
            </div>
          ) : (
            <Link to='/login'>
              <button className='btn-login ml-2 hover:opacity-80'>
                Đăng nhập / Đăng xuất
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

        <div className='flex gap-16 text-[20px] font-medium uppercase'>
          {listNavigate.map((item, index) =>
            //kiem tra duong dan
            item.link === currentLocation ? (
              <Link to={item.link} className='curNav'>
                {item.title}
              </Link>
            ) : (
              <Link to={item.link} className='headerBar'>
                {item.title}
              </Link>
            )
          )}
        </div>
        <div>
          <Link to='/contact'>
            <button className='btn flex gap-2 items-center capitalize'>
              Liên lạc
              <AiOutlineArrowRight size={20} />
            </button>
          </Link>
        </div>
      </div>
    </div>
  );
};

export default Header;
