import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import React, { useEffect, useState } from 'react';
import { AiOutlineArrowLeft } from 'react-icons/ai';
import { Link, useNavigate, useParams } from 'react-router-dom';
import CountdownTimer from './CountdownTimer';
import axiosClient from '../../utils/axiosClient';
import { useSelector } from 'react-redux';
import * as dayjs from 'dayjs';

// console.log(Array.from({ length: 10 }, (_, index) => 0));
// console.log([...Array(10)].map(() => 0));
// console.log(new Array(10).fill(0));

const TestTheory = () => {
  const navigate = useNavigate();
  const { theoryExamId } = useParams();
  const { memberId, email } = useSelector(
    (state) => state.auth.user.accountInfo
  );
  const url_Service = process.env.REACT_APP_SERVER_API;
  const currentDate = new Date();
  const [startedDate, setStartedDate] = useState('');
  const [open, setOpen] = useState(false);
  const [answerList, setAnswerList] = useState([]);
  const [questionList, setQuestionList] = useState([]);
  const charOption = ['A', 'B', 'C', 'D', 'E'];

  useEffect(() => {
    async function fetchData() {
      const response = await axiosClient.get(`/theory-exam/${theoryExamId}`);
      setQuestionList(response?.data.data);
      setAnswerList(
        new Array(response?.data.data.totalQuestion).fill({
          questionId: '',
          selectAnswer: '',
        })
      );
      localStorage.setItem(
        'startedDate',
        dayjs(new Date())
          .format('YYYY-MM-DD')
          .concat('T'.concat(dayjs(new Date()).format('HH:mm:ss')))
      );
      setStartedDate(dayjs(new Date()).format('YYYY-MM-DD HH:mm:ss'));
    }
    fetchData();
  }, []);

  const selectAnswer = (indexQuestion, indexOption) => {
    // answerList: [{ questionId: '', selectAnswer: '' }]
    // indexQuestion: 1, indexOption: "2 - Câu C"
    const newAnswerList = [...answerList];
    newAnswerList[indexQuestion - 1] = {
      questionId: indexQuestion,
      selectAnswer: indexOption,
    };
    setAnswerList(newAnswerList);
  };

  const handleSubmit = async (id) => {
    const response = await axiosClient.post(`${url_Service}/theory/submit`, {
      email,
      memberId,
      theoryExamId,
      startedDate: startedDate,
      selectedAnswers: answerList,
    });
    console.log('submit: ', response);
    navigate(`/theory/result/${id}`);
  };

  console.log('answerList: ', answerList);
  console.log('questionList: ', questionList);

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
              {Array.from(
                { length: questionList.totalQuestion },
                (_, index) => (
                  <div
                    key={index}
                    className={`${
                      answerList[index]?.questionId === ''
                        ? ''
                        : 'selected-color'
                    } text-center cursor-pointer hover:text-white hover:bg-[#0D5EF4] border p-2`}
                  >
                    <a href={`#${index + 1}`} className='font-medium'>
                      {index + 1}
                    </a>
                  </div>
                )
              )}
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
          {questionList &&
            questionList.questions?.map((itemQuestion, indexQuestion) => (
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
                    {itemQuestion.questionAnswerDesc}
                  </div>

                  <div className='flex-1 h-full px-2'>
                    <h1 className='btn text-center cursor-text'>Chọn Đáp án</h1>
                    <div className='flex flex-col gap-4 p-2 mt-2 border-l-2'>
                      {itemQuestion.questionAnswers?.map(
                        (item, indexOption) => (
                          <div
                            key={indexOption}
                            onClick={() => {
                              console.log({
                                id: itemQuestion.questionId,
                                'dap an': item.answer,
                              });
                              selectAnswer(
                                itemQuestion.questionId,
                                item.answer
                              );
                            }}
                            className={`${
                              answerList[indexQuestion].selectAnswer ===
                              item.answer
                                ? 'selected-color '
                                : ''
                            } border p-2 cursor-pointer hover:opacity-80 rounded`}
                          >
                            <span className='font-bold'>
                              {charOption[indexOption]}
                            </span>
                            . {item.answer}
                          </div>
                        )
                      )}
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
          <button className='btn' onClick={() => handleSubmit(theoryExamId)}>
            Đồng ý
          </button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default TestTheory;
