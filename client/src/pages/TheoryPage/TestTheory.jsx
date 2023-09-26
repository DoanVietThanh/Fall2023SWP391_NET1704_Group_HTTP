import React, { useState } from 'react';
import CountdownTimer from './CountdownTimer';
import { Link } from 'react-router-dom';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import { AiOutlineArrowLeft } from 'react-icons/ai';

const TestTheory = () => {
  const currentDate = new Date();
  const [open, setOpen] = useState(false);

  return (
    <div className='flex gap-1 bg-gray-200 h-[100vh] w-[100vw]'>
      <div className='w-[25%] p-4 border-2 bg-[#fff] flex flex-col justify-between'>
        <div>
          <div className='mb-2 flex justify-between items-center'>
            <Link to={`/theory`}>
              <div className='font-medium text-[18px] flex items-center gap-2 border p-2 rounded-lg bg-[#878f9f] text-white'>
                <AiOutlineArrowLeft />
                Trở về
              </div>
            </Link>
            <h1 className='font-bold text-orange-400 text-[20px]'>
              Date:
              {` ${currentDate.getDate()}/${currentDate.getMonth()}/${currentDate.getFullYear()}`}
            </h1>
          </div>
          <div className='p-4 border-y-2 flex flex-col justify-center items-center'>
            <h2 className='text-center font-medium text-[20px]'>Thời gian</h2>
            <CountdownTimer minutes={15} />
          </div>
          <div className=''>
            <h2 className='text-center font-medium text-[20px] my-2'>
              Câu hỏi
            </h2>
            <div className='grid grid-cols-5 gap-2'>
              {Array.from({ length: 35 }, (_, index) => (
                <div className='text-center cursor-pointer hover:text-white hover:bg-[#0D5EF4] border p-2'>
                  <a href={`#${index + 1}`} className='font-medium'>
                    {index + 1}
                  </a>
                </div>
              ))}
            </div>
          </div>
        </div>

        <div>
          <button className='btn w-full' onClick={() => setOpen(true)}>
            Nộp bài
          </button>
          <h3 className='text-center my-2 font-medium italic'>
            Vui lòng kiểm tra bài làm trước khi nộp
          </h3>
        </div>
      </div>

      <div className='flex-1 p-2 pr-4 border-2 overflow-y-auto py-8'>
        <div className='flex flex-col gap-8 scroll-smooth'>
          {Array.from({ length: 35 }, (_, index) => (
            <div
              id={index + 1}
              className='min-h-[360px] bg-white p-4 rounded-lg'
            >
              <h1 className='font-medium text-[20px]'>Câu hỏi {index + 1} :</h1>
              {/* Content Question  */}
              <div className='flex gap-2 h-full mt-2 flex-1'>
                <div className='flex-1 h-full'>
                  Lorem ipsum dolor, sit amet consectetur adipisicing elit.
                  Architecto quis aspernatur iusto quam qui, rerum ab sapiente
                  inventore dolores nobis deserunt modi nemo! Cumque labore
                  temporibus perspiciatis odio doloremque, voluptatum repellat
                  illo in perferendis dolores sit maiores eos ab laudantium.
                </div>

                <div className='flex-1 h-full px-2'>
                  <h1 className='btn text-center cursor-text'>Chọn Đáp án</h1>
                  <div className='flex flex-col gap-4 p-2 mt-2 border-l-2'>
                    {['A', 'B', 'C', 'D'].map((item, index) => (
                      <div className='border p-2 cursor-pointer hover:opacity-80'>
                        <span className='font-bold'>{item}</span>. Lorem ipsum
                        dolor sit amet consectetur adipisicing elit. Cum tempore
                        aperiam architecto enim fugiat ea?
                      </div>
                    ))}
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>

      <Dialog
        open={open}
        onClose={() => setOpen(false)}
        aria-labelledby='alert-dialog-title'
        aria-describedby='alert-dialog-description'
      >
        <DialogTitle id='alert-dialog-title'>{'Xác nhận nộp bài'}</DialogTitle>
        <DialogContent>
          <h1>Chưa hết thời gian, bạn có chắc xác nhận nộp bài</h1>
        </DialogContent>
        <DialogActions>
          <button className='btn-cancel' onClick={() => setOpen(false)}>
            Hủy
          </button>
          <button className='btn'>Đồng ý</button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default TestTheory;
