import React, { useState } from 'react';
import { AiOutlineArrowLeft, AiOutlineArrowRight } from 'react-icons/ai';
import { Link, useNavigate } from 'react-router-dom';
import theme from '../../theme';
// console.log(Array.from({ length: 10 }, (_, index) => 0));
// console.log([...Array(10)].map(() => 0));
// console.log(new Array(10).fill(0));

const ResultTheory = () => {
  const navigate = useNavigate();
  const currentDate = new Date();
  const [open, setOpen] = useState(false);
  const [answerList, setAnswerList] = useState(new Array(35).fill(0));

  const selectAnswer = (indexQuestion, char) => {
    const newAnswerList = [...answerList];
    newAnswerList[indexQuestion] = char;
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
                Trở về trang chủ
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
            <h2
              className={`text-center w-full text-white font-medium text-[20px] bg-[${theme.color.mainColor}] p-4 rounded-lg`}
            >
              Kết Quả Bài Làm
            </h2>
            <div className='font-medium'>
              <p>
                Đề số:{' '}
                <span className={`text-[20px] text-[${theme.color.mainColor}]`}>
                  01 - Đề thi B1
                </span>
              </p>
              <p>
                Số câu đúng:{' '}
                <span className='text-[20px] text-green-400'>30</span>
              </p>
              <p>
                Số câu sai: <span className='text-[20px] text-red-400'>5</span>
              </p>
              <p>
                Kết quả:{' '}
                <span className='text-[20px] text-green-400'>Đậu (30/35)</span>
              </p>
            </div>
          </div>
          <div className=''>
            <h2 className='text-center font-medium text-[20px] my-2'>
              Câu hỏi
            </h2>
            <div className='grid grid-cols-5 gap-2'>
              {Array.from({ length: 35 }, (_, index) => (
                <div
                  className={`${
                    answerList[index] ? 'selected-color ' : ''
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
                  <h1 className='btn text-center cursor-text'>
                    Đáp án đã chọn
                  </h1>
                  <div className='flex flex-col gap-4 p-2 mt-2 border-l-2'>
                    {['A', 'B', 'C', 'D'].map((charOption, indexCharOption) => (
                      <div
                        key={indexCharOption}
                        onClick={() => selectAnswer(indexQuestion, charOption)}
                        className={`${
                          answerList[indexQuestion] === charOption
                            ? 'selected-color'
                            : ''
                        } border p-2 cursor-pointer hover:opacity-80 rounded`}
                      >
                        <span className='font-bold'>{charOption}</span>. Lorem
                        ipsum dolor sit amet consectetur adipisicing elit. Cum
                        tempore aperiam architecto enim fugiat ea?
                      </div>
                    ))}
                    <div className='font-medium p-2 bg-yellow-400 flex-x gap-2'>
                      <AiOutlineArrowRight />
                      Đáp án đúng là: A
                    </div>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default ResultTheory;
