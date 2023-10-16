import * as React from 'react';
import { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import ChevronLeftIcon from '@mui/icons-material/ChevronLeft';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import MenuIcon from '@mui/icons-material/Menu';
import MuiAppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import CssBaseline from '@mui/material/CssBaseline';
import MuiDrawer from '@mui/material/Drawer';
import IconButton from '@mui/material/IconButton';
import Toolbar from '@mui/material/Toolbar';
import { styled, useTheme } from '@mui/material/styles';
import { AiOutlineQuestionCircle } from 'react-icons/ai';
import { BiLogOut } from 'react-icons/bi';
import {
  BsCalendarEvent,
  BsClockHistory,
  BsEnvelopePaper,
  BsPerson,
  BsPeople,
} from 'react-icons/bs';
import { useLocation, useNavigate, Link } from 'react-router-dom';
import { logout } from '../features/auth/authSlice';

const drawerWidth = 240;

const listNavbar = [
  {
    id: 1,
    title: 'Thông tin cá nhân',
    icon: <BsPerson size={20} />,
    navigate: '/profile',
  },
  {
    id: 2,
    title: 'Lịch học theo tuần',
    icon: <BsCalendarEvent size={20} />,
    navigate: '/week-schedule',
  },
  {
    id: 3,
    title: 'Lịch sử kiểm tra',
    icon: <BsClockHistory size={20} />,
    navigate: '/history-test',
  },
];

const listNavbarManage = [
  {
    id: 1,
    title: 'Thông tin cá nhân',
    icon: <BsPerson size={20} />,
    navigate: '/profile',
  },
  {
    id: 2,
    title: 'Quản lí người dùng',
    icon: <BsPeople size={20} />,
    navigate: '/manage-user',
  },
  {
    id: 3,
    title: 'Lịch dạy theo tuần',
    icon: <BsCalendarEvent size={20} />,
    navigate: '/week-schedule-mentor',
  },
  {
    id: 4,
    title: 'Lịch sử kiểm tra',
    icon: <BsClockHistory size={20} />,
    navigate: '/history-test',
  },
  {
    id: 5,
    title: 'Quản lí câu hỏi',
    icon: <AiOutlineQuestionCircle size={20} />,
    navigate: '/manage-question',
  },
  {
    id: 6,
    title: 'Quản lí đề thi',
    icon: <BsEnvelopePaper size={20} />,
    navigate: '/manage-banktest',
  },
];

const openedMixin = (theme) => ({
  width: drawerWidth,
  transition: theme.transitions.create('width', {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.enteringScreen,
  }),
  overflowX: 'hidden',
});

const closedMixin = (theme) => ({
  transition: theme.transitions.create('width', {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.leavingScreen,
  }),
  overflowX: 'hidden',
  width: `calc(${theme.spacing(7)} + 1px)`,
  [theme.breakpoints.up('sm')]: {
    width: `calc(${theme.spacing(8)} + 1px)`,
  },
});

const DrawerHeader = styled('div')(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'flex-end',
  padding: theme.spacing(0, 1),
  // necessary for content to be below app bar
  ...theme.mixins.toolbar,
}));

const AppBar = styled(MuiAppBar, {
  shouldForwardProp: (prop) => prop !== 'open',
})(({ theme, open }) => ({
  zIndex: theme.zIndex.drawer + 1,
  transition: theme.transitions.create(['width', 'margin'], {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.leavingScreen,
  }),
  ...(open && {
    marginLeft: drawerWidth,
    width: `calc(100% - ${drawerWidth}px)`,
    transition: theme.transitions.create(['width', 'margin'], {
      easing: theme.transitions.easing.sharp,
      duration: theme.transitions.duration.enteringScreen,
    }),
  }),
}));

const Drawer = styled(MuiDrawer, {
  shouldForwardProp: (prop) => prop !== 'open',
})(({ theme, open }) => ({
  width: drawerWidth,
  flexShrink: 0,
  whiteSpace: 'nowrap',
  boxSizing: 'border-box',
  ...(open && {
    ...openedMixin(theme),
    '& .MuiDrawer-paper': openedMixin(theme),
  }),
  ...(!open && {
    ...closedMixin(theme),
    '& .MuiDrawer-paper': closedMixin(theme),
  }),
}));

export default function SideBar() {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const theme = useTheme();
  const { accountInfo } = useSelector((state) => state.auth.user);
  const [open, setOpen] = useState(localStorage.getItem('showNav') || false);

  const { user, isLoading } = useSelector((state) => state.auth);
  const location = useLocation();

  const handleLogout = () => {
    dispatch(logout());
    navigate('/login');
  };

  return (
    <Box sx={{ display: 'flex' }}>
      <CssBaseline />
      <AppBar position='fixed' open={open}>
        <Toolbar>
          <IconButton
            color='inherit'
            aria-label='open drawer'
            onClick={() => {
              setOpen(true);
              localStorage.setItem('showNav', true);
            }}
            edge='start'
            sx={{
              marginRight: 5,
              ...(open && { display: 'none' }),
            }}
          >
            <MenuIcon />
          </IconButton>
          <div className='w-full flex justify-between'>
            <div className='font-medium capitalize flex-x'>
              {accountInfo?.emailNavigation.role.roleId === 4
                ? listNavbar.map((itemNav, index) =>
                    location.pathname === itemNav.navigate ? itemNav.title : ''
                  )
                : listNavbarManage.map((itemNav, index) =>
                    location.pathname === itemNav.navigate ? itemNav.title : ''
                  )}
            </div>
            <div className='flex-x gap-2'>
              <div>
                <img
                  src='/img/avtThanh.jpg'
                  alt='avt'
                  className='h-[40px] w-[40px] object-cover rounded-full'
                />
              </div>
              <p>Thanh Đoàn</p>
            </div>
          </div>
        </Toolbar>
      </AppBar>
      <Drawer variant='permanent' open={open}>
        <DrawerHeader>
          <div className='flex justify-between items-center w-full'>
            <Link to={`/`}>
              <img src='/img/logo.png' alt='' className='h-[50px]' />
            </Link>
            <p className='font-bold'>Http Team</p>
            <IconButton
              onClick={() => {
                setOpen(false);
                localStorage.setItem('showNav', false);
              }}
            >
              {theme.direction === 'rtl' ? (
                <ChevronRightIcon />
              ) : (
                <ChevronLeftIcon />
              )}
            </IconButton>
          </div>
        </DrawerHeader>
        <div className='flex flex-col justify-between h-full'>
          <div>
            {accountInfo?.emailNavigation.role.roleId === 4
              ? listNavbar.map((item, index) => (
                  <div
                    key={item.id}
                    onClick={() => navigate(item.navigate)}
                    className={`min-h-[48px] flex-x px-[20px] py-[8px] hover:opacity-80 hover:cursor-pointer `}
                  >
                    <div className='flex-x gap-[30px]'>
                      {item.icon}
                      <p className='cappitalize font-medium capitalize'>
                        {item.title}
                      </p>
                    </div>
                  </div>
                ))
              : listNavbarManage.map((item, index) => (
                  <div
                    key={item.id}
                    onClick={() => navigate(item.navigate)}
                    className={`min-h-[48px] flex-x px-[20px] py-[8px] hover:opacity-80 hover:cursor-pointer `}
                  >
                    <div className='flex-x gap-[30px]'>
                      {item.icon}
                      <p className='cappitalize font-medium capitalize'>
                        {item.title}
                      </p>
                    </div>
                  </div>
                ))}
          </div>
          <div
            className={`min-h-[48px] flex-x px-[20px] py-[8px] mb-4 hover:opacity-80 hover:cursor-pointer btn rounded-none`}
          >
            <div
              className='flex-x gap-[30px] rounded-none'
              onClick={handleLogout}
            >
              <BiLogOut size={20} />
              <p className='cappitalize font-medium uppercase'>Đăng xuất</p>
            </div>
          </div>
        </div>
      </Drawer>
    </Box>
  );
}
