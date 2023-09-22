import React from 'react';
import Avatar from '@mui/material/Avatar';

const ProfilePage = () => {
  return (
    <div className='flex'>
      <div className='w-[25%]'>
        <div className='flex justify-center items-center'>
          <img
            src='https://scontent.fsgn5-10.fna.fbcdn.net/v/t39.30808-6/326718942_3475973552726762_6277150844361274430_n.jpg?_nc_cat=107&ccb=1-7&_nc_sid=a2f6c7&_nc_ohc=OAw9KYbwIYQAX9p4Tu9&_nc_ht=scontent.fsgn5-10.fna&oh=00_AfA86LZGE5THISarUceKKdD_G35FxPWHNR0dsFrdfrlnAQ&oe=6512B32C'
            alt='Avatar'
            className='rounded-full w-[100px] h-[100px] object-cover'
          />
        </div>
        <h1 className='text-center'>Đoàn Viết Thanh</h1>
      </div>
      <div className='flex-1'>4</div>
    </div>
  );
};

export default ProfilePage;
