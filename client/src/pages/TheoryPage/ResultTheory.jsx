import dayjs from 'dayjs';
import React, { useEffect, useState } from 'react';
import { AiOutlineArrowLeft, AiOutlineArrowRight } from 'react-icons/ai';
import { useSelector } from 'react-redux';
import { Link, useNavigate, useParams } from 'react-router-dom';
import theme from '../../theme';
import axiosClient from '../../utils/axiosClient';
import { toastError } from './../../components/Toastify';
import RightAnswer from './components/RightAnswer';
// console.log(Array.from({ length: 10 }, (_, index) => 0));
// console.log([...Array(10)].map(() => 0));
// console.log(new Array(10).fill(0));

const ResultTheory = () => {
  const navigate = useNavigate();
  const url_Service = process.env.REACT_APP_SERVER_API;
  const { memberId, email } = useSelector(
    (state) => state.auth.user.accountInfo
  );
  const { mockTestId } = useParams();
  const currentDate = new Date();
  const [open, setOpen] = useState(false);
  const [examResult, setExamResult] = useState([]);
  const [answerList, setAnswerList] = useState(
    new Array(examResult?.history?.totalQuestion).fill(0)
  );
  const charOption = ['A', 'B', 'C', 'D', 'E'];

  useEffect(() => {
    async function getReview() {
      try {
        const response = await axiosClient
          .post(`/theory/review`, {
            email,
            mockTestId,
            joinDate: localStorage.getItem('startedDate'),
          })
          .catch((error) => toastError(error?.response?.data?.message));
        if (response.data.statusCode === 200) {
          setExamResult(response.data.data);
        }
        console.log('getReview: ', response);
      } catch (error) {
        // toastError('Review thất bại');
      }
    }
    getReview();
  }, []);

  const selectAnswer = (indexQuestion, char) => {
    const newAnswerList = [...answerList];
    newAnswerList[indexQuestion] = char;
    setAnswerList(newAnswerList);
  };

  const handleSubmit = (id) => {
    navigate(`/theory/result/${id}`);
  };

  console.log('examResult?.examResult: ', examResult?.examResult);

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
              {`Date: ${dayjs(new Date()).format('DD/MM/YYYY')}`}
            </h1>
          </div>
          <div className='p-4 border-y-2 flex flex-col justify-center items-center'>
            <h2
              className={`text-center w-full text-white font-medium text-[20px] bg-[${theme.color.mainColor}] p-4 rounded-lg`}
            >
              Kết Quả Bài Làm
            </h2>
            <div className='font-medium text-center'>
              <p>
                Mã Đề:{' '}
                <span className={`text-[20px] text-[${theme.color.mainColor}]`}>
                  {examResult?.history?.theoryExamId}
                </span>
              </p>
              <p>
                Số câu đúng:{' '}
                <span className='text-[20px] text-green-400'>
                  {examResult?.history?.totalRightAnswer}
                </span>
              </p>
              <p>
                Số câu sai:{' '}
                <span className='text-[20px] text-red-400'>
                  {examResult?.history?.totalQuestion -
                    examResult?.history?.totalRightAnswer}
                </span>
              </p>
              <p>
                Kết quả:{' '}
                <span className='text-[20px] text-green-400'>
                  {examResult?.history?.isPassed ? (
                    <span className='text-[20px] text-green-400'>
                      Đậu{' '}
                      {`${examResult?.history?.totalRightAnswer} / ${examResult?.history?.totalQuestion}`}
                    </span>
                  ) : (
                    <span className='text-[20px] text-red-400'>Rớt</span>
                  )}
                </span>
              </p>
              <p>
                Thời gian bắt đầu:{' '}
                <span className='font-bold'>
                  {dayjs(examResult?.history?.date).format(
                    'HH:mm:ss (DD/MM/YYYY) '
                  )}
                </span>
              </p>
            </div>
          </div>
          <div className=''>
            <h2 className='text-center font-medium text-[20px] my-2'>
              Câu hỏi
            </h2>
            <div className='grid grid-cols-5 gap-2'>
              {examResult?.examResult?.map((numQuestion, index) => (
                <div
                  className={`${
                    numQuestion.point === 0 ? 'bg-[red]' : 'bg-[green]'
                  } text-center text-white border p-2`}
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
          {examResult?.examResult?.map((itemQuestion, indexQuestion) => (
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
                  {itemQuestion?.question?.questionAnswerDesc}
                  {/* <img
                    alt='img'
                    src='https://hocthilaixeoto.com/upload/images/cau-hoi-ly-thuyet-lai-xe-b1.png'
                  /> */}
                </div>

                <div className='flex-1 h-full px-2'>
                  <h1 className='btn text-center cursor-text'>
                    Đáp án đã chọn
                  </h1>
                  <div className='flex flex-col gap-4 p-2 mt-2 border-l-2'>
                    {itemQuestion?.question?.questionAnswers.map(
                      (itemChoice, index) => (
                        <div
                          key={itemChoice?.questionAnswerId}
                          className={`${
                            itemQuestion?.selectedAnswerId ===
                            itemChoice.questionAnswerId
                              ? 'bg-[#dadada] opacity-80'
                              : ''
                          } border p-2 hover:opacity-80 rounded`}
                        >
                          <span className='font-bold'>{charOption[index]}</span>
                          . {itemChoice?.answer}
                        </div>
                      )
                    )}
                    {itemQuestion?.selectedAnswerId < 0 && (
                      <h1 className='text-red-400 text-center'>
                        Câu này chưa được chọn
                      </h1>
                    )}
                    <div className='font-medium p-2 bg-yellow-400 flex-x gap-2'>
                      <AiOutlineArrowRight />
                      Đáp án đúng là:{' '}
                      <RightAnswer
                        listAnswer={itemQuestion?.question.questionAnswers}
                      />
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
