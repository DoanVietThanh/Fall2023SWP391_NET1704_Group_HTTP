import React, { useState } from 'react';
import { Backdrop } from '@mui/material';
import { ScaleLoader } from 'react-spinners';

const Loading = () => {
  const [openLoading, setOpenLoading] = useState(true);
  return (
    <Backdrop
      sx={{ color: '#fff', zIndex: (theme) => theme.zIndex.drawer + 1 }}
      open={openLoading}
      onClick={() => setOpenLoading(false)}
    >
      <dir className='relative'>
        <img
          src='/img/loadingCarGif.gif'
          alt='gif'
          className='w-[200px] h-[200px] object-cover'
        />
        <div className='absolute bottom-[40px] left-[12%]'>
          <ScaleLoader
            color='#36d7b7'
            height={6}
            margin={1}
            radius={30}
            width={40}
            speedMultiplier={0}
          />
        </div>
      </dir>
    </Backdrop>
  );
};

export default Loading;

// const Loading = ({ loading, setLoading }) => {
//   // const [openLoading, setOpenLoading] = useState(loading);
//   return (
//     <Backdrop
//       sx={{ color: '#fff', zIndex: (theme) => theme.zIndex.drawer + 1 }}
//       open={loading}
//       onClick={() => setLoading(false)}
//     >
//       <dir className='relative'>
//         <img
//           src='/img/loadingCarGif.gif'
//           alt='gif'
//           className='w-[200px] h-[200px] object-cover'
//         />
//         <div className='absolute bottom-[40px] left-[12%]'>
//           <ScaleLoader
//             color='#36d7b7'
//             height={6}
//             margin={1}
//             radius={30}
//             width={40}
//             speedMultiplier={0}
//           />
//         </div>
//       </dir>
//     </Backdrop>
//   );
// };
