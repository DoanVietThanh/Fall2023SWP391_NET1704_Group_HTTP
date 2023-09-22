import React from 'react';
import ListSubheader from '@mui/material/ListSubheader';
import List from '@mui/material/List';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import Collapse from '@mui/material/Collapse';
import InboxIcon from '@mui/icons-material/MoveToInbox';
import DraftsIcon from '@mui/icons-material/Drafts';
import SendIcon from '@mui/icons-material/Send';
import ExpandLess from '@mui/icons-material/ExpandLess';
import ExpandMore from '@mui/icons-material/ExpandMore';
import StarBorder from '@mui/icons-material/StarBorder';
import Header from './../../components/Header';
import Footer from '../../components/Footer';
import { useSelector } from 'react-redux';
const ProfilePage = () => {
  const { user } = useSelector((state) => state.auth);
  console.log('🚀 ~ file: ProfilePage.jsx:19 ~ ProfilePage ~ user:', user);
  const [open, setOpen] = React.useState(true);

  const handleClick = () => {
    setOpen(!open);
  };
  return (
    <div>
      <Header />
      <div className='flex my-8'>
        <div className='w-[25%] border-r-2'>
          <div className='flex justify-center items-center'>
            <img
              src='https://scontent.fsgn5-10.fna.fbcdn.net/v/t39.30808-6/326718942_3475973552726762_6277150844361274430_n.jpg?_nc_cat=107&ccb=1-7&_nc_sid=a2f6c7&_nc_ohc=OAw9KYbwIYQAX9p4Tu9&_nc_ht=scontent.fsgn5-10.fna&oh=00_AfA86LZGE5THISarUceKKdD_G35FxPWHNR0dsFrdfrlnAQ&oe=6512B32C'
              alt='Avatar'
              className='rounded-full w-[100px] h-[100px] object-cover'
            />
          </div>
          <h1 className='text-center my-4 font-medium text-[20px]'>
            {`${user.accountInfo?.firstName} ${user.accountInfo?.lastName}`}
          </h1>
        </div>
        <div className='flex-1 px-8'>4</div>
        {/* <List
          sx={{ width: '100%', maxWidth: 360, bgcolor: 'background.paper' }}
          component='nav'
          aria-labelledby='nested-list-subheader'
          subheader={
            <ListSubheader component='div' id='nested-list-subheader'>
              Nested List Items
            </ListSubheader>
          }
        >
          <ListItemButton onClick={handleClick}>
            <ListItemIcon>
              <InboxIcon />
            </ListItemIcon>
            <ListItemText primary='Inbox' />
            {open ? <ExpandLess /> : <ExpandMore />}
          </ListItemButton>
          <Collapse in={open} timeout='auto' unmountOnExit>
            <List component='div' disablePadding>
              <ListItemButton sx={{ pl: 4 }}>
                <ListItemIcon>
                  <StarBorder />
                </ListItemIcon>
                <ListItemText primary='Starred' />
              </ListItemButton>
            </List>
          </Collapse>
        </List> */}
      </div>
      <Footer />
    </div>
  );
};

export default ProfilePage;
