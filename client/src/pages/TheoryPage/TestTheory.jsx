import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import React, { useState } from 'react';
import { AiOutlineArrowLeft } from 'react-icons/ai';
import { Link, useNavigate } from 'react-router-dom';
import CountdownTimer from './CountdownTimer';

// console.log(Array.from({ length: 10 }, (_, index) => 0));
// console.log([...Array(10)].map(() => 0));
// console.log(new Array(10).fill(0));

const TestTheory = () => {
  const navigate = useNavigate();
  const currentDate = new Date();
  const [open, setOpen] = useState(false);
  const [answerList, setAnswerList] = useState(
    new Array(35).fill({ questionId: '', selectedAnswerId: '' })
  );

  const selectAnswer = (indexQuestion, char) => {
    // answerList: [{ questionId: '', selectedAnswerId: '' }]
    // indexQuestion: 1, char: "A"
    const newAnswerList = [...answerList];
    // newAnswerList[indexQuestion] = char;
    newAnswerList[indexQuestion] = {
      questionId: indexQuestion,
      selectedAnswerId: char,
    };
    setAnswerList(newAnswerList);
  };

  console.log(answerList);

  const handleSubmit = (id) => {
    navigate(`/theory/result/${id}`);
  };

  return (
    <div className='flex gap-1 bg-gray-200 h-[100vh] w-[100vw]'>
      <div className='w-[25%] p-4 border-2 bg-[#fff] flex flex-col justify-between overflow-y-auto'>
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
              {` ${currentDate.getDate()}/${
                currentDate.getMonth() + 1
              }/${currentDate.getFullYear()}`}
            </h1>
          </div>
          <div className='p-4 border-y-2 flex flex-col justify-center items-center'>
            <h2 className='text-center font-medium text-[20px]'>Thời gian</h2>
            <CountdownTimer minutes={60} />
          </div>
          <div className=''>
            <h2 className='text-center font-medium text-[20px] my-2'>
              Câu hỏi
            </h2>
            <div className='grid grid-cols-5 gap-2'>
              {Array.from({ length: 35 }, (_, index) => (
                <div
                  className={`${
                    answerList[index]?.questionId === '' ? '' : 'selected-color'
                  } text-center cursor-pointer hover:text-white hover:bg-[#0D5EF4] border p-2`}
                >
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
          {Array.from({ length: 35 }, (_, indexQuestion) => (
            <div
              id={indexQuestion + 1}
              className='min-h-[360px] bg-white p-4 rounded-lg'
            >
              <h1 className='font-medium text-[20px]'>
                Câu hỏi {indexQuestion + 1} :
              </h1>
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
                    {['A', 'B', 'C', 'D'].map((charOption, indexCharOption) => (
                      <div
                        key={indexCharOption}
                        onClick={() =>
                          selectAnswer(indexQuestion, indexCharOption)
                        }
                        className={`${
                          answerList[indexQuestion].selectedAnswerId ===
                          indexCharOption
                            ? 'selected-color'
                            : ''
                        } border p-2 cursor-pointer hover:opacity-80 rounded`}
                      >
                        <span className='font-bold'>{charOption}</span>. Lorem
                        ipsum dolor sit amet consectetur adipisicing elit. Cum
                        tempore aperiam architecto enim fugiat ea?
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
          <button className='btn' onClick={() => handleSubmit(1)}>
            Đồng ý
          </button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default TestTheory;
