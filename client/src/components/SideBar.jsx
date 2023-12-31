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
  BsBarChartLine,
  BsFilePost,
  BsFillFilePostFill,
} from 'react-icons/bs';
import { useLocation, useNavigate, Link } from 'react-router-dom';
import { logout } from '../features/auth/authSlice';
import { Button, Menu, MenuItem } from '@mui/material';
import { MdFreeCancellation, MdOutlineManageHistory } from 'react-icons/md';
import { FaBook } from 'react-icons/fa';
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

const listMentorNavbar = [
  {
    id: 1,
    title: 'Thông tin cá nhân',
    icon: <BsPerson size={20} />,
    navigate: '/profile',
  },
  {
    id: 2,
    title: 'Lịch dạy theo tuần',
    icon: <BsCalendarEvent size={24} />,
    navigate: '/week-schedule-mentor',
  },
];

const listNavbarManage = [
  {
    id: 1,
    title: 'Thông tin cá nhân',
    icon: <BsPerson size={24} />,
    navigate: '/profile',
  },
  {
    id: 2,
    title: 'Quản lí người dùng',
    icon: <BsPeople size={24} />,
    navigate: '/manage-user',
  },
  {
    id: 5,
    title: 'Quản lí câu hỏi',
    icon: <AiOutlineQuestionCircle size={24} />,
    navigate: '/manage-question',
  },
  // {
  //   id: 6,
  //   title: 'Quản lí đề thi',
  //   icon: <BsEnvelopePaper size={24} />,
  //   navigate: '/manage-banktest',
  // },
  {
    id: 7,
    title: 'Quản lí bài đăng',
    icon: <BsFillFilePostFill size={24} />,
    navigate: '/manage-blog',
  },
  {
    id: 8,
    title: 'Quản lí lịch chờ',
    icon: <MdOutlineManageHistory size={24} />,
    navigate: '/manage-await-schedule',
  },

  {
    id: 9,
    title: 'Quản lí lịch hủy',
    icon: <MdFreeCancellation size={24} />,
    navigate: '/manage-deny-schedule',
  },
  {
    id: 13,
    title: 'Quản lí khóa học',
    icon: <FaBook size={20} />,
    navigate: '/manage-course',
  },
];

const listNavbarAdmin = [
  {
    id: 1,
    title: 'Thông tin cá nhân',
    icon: <BsPerson size={24} />,
    navigate: '/profile',
  },
  {
    id: 2,
    title: 'Thống kê',
    icon: <BsBarChartLine size={24} />,
    navigate: '/dashboard',
  },
  {
    id: 4,
    title: 'Quản lí nhân viên',
    icon: <BsEnvelopePaper size={20} />,
    navigate: '/manage-staff',
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
  const [anchorEl, setAnchorEl] = useState(null);
  const openControl = Boolean(anchorEl);

  const { user, isLoading } = useSelector((state) => state.auth);
  const location = useLocation();

  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleLogout = () => {
    dispatch(logout());
    navigate('/login');
  };

  console.log(
    'accountInfo?.emailNavigation.role.roleId: ',
    accountInfo?.emailNavigation.role.roleId
  );

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
              {accountInfo?.emailNavigation.role.roleId == 1 &&
                listNavbarAdmin.map((itemNav, index) =>
                  location.pathname === itemNav.navigate ? itemNav.title : ''
                )}

              {accountInfo?.emailNavigation.role.roleId == 2 &&
                listNavbarManage.map((itemNav, index) =>
                  location.pathname === itemNav.navigate ? itemNav.title : ''
                )}

              {accountInfo?.emailNavigation.role.roleId == 3 &&
                listMentorNavbar.map((itemNav, index) =>
                  location.pathname === itemNav.navigate ? itemNav.title : ''
                )}

              {accountInfo?.emailNavigation.role.roleId == 4 &&
                listNavbar.map((itemNav, index) =>
                  location.pathname === itemNav.navigate ? itemNav.title : ''
                )}
            </div>
            <div className='flex-x gap-2'>
              <div>
                <Button
                  id='basic-button'
                  aria-controls={openControl ? 'basic-menu' : undefined}
                  aria-haspopup='true'
                  aria-expanded={openControl ? 'true' : undefined}
                  onClick={handleClick}
                >
                  <div className='flex justify-center items-center'>
                    <div className='flex justify-center items-center'>
                      <img
                        src={user.accountInfo?.avatarImage}
                        alt='Avatar'
                        className='rounded-full w-[36px] h-[36px] object-cover'
                      />
                    </div>
                    <h2 className='text-white text-[16px] pl-2'>
                      {`${user.accountInfo?.firstName} ${user.accountInfo?.lastName}`}
                    </h2>
                  </div>
                </Button>
                <Menu
                  id='basic-menu'
                  anchorEl={anchorEl}
                  open={openControl}
                  onClose={() => setAnchorEl(null)}
                  MenuListProps={{
                    'aria-labelledby': 'basic-button',
                  }}
                >
                  <MenuItem onClick={() => setAnchorEl(null)}>
                    <Link to={`/profile`}>Cá nhân</Link>
                  </MenuItem>
                  <MenuItem onClick={() => setAnchorEl(null)}>
                    Tài khoản
                  </MenuItem>
                  <MenuItem onClick={handleLogout}>Đăng xuất</MenuItem>
                </Menu>
              </div>
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
            {accountInfo?.emailNavigation.role.roleId == 1 &&
              listNavbarAdmin.map((item, index) => (
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

            {accountInfo?.emailNavigation.role.roleId == 2 &&
              listNavbarManage.map((item, index) => (
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

            {accountInfo?.emailNavigation.role.roleId == 3 &&
              listMentorNavbar.map((item, index) => (
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

            {accountInfo?.emailNavigation.role.roleId == 4 &&
              listNavbar.map((item, index) => (
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
