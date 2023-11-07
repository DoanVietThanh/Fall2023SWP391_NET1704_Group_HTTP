import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import React, { useCallback, useEffect, useState } from 'react';
import { AiOutlineArrowLeft } from 'react-icons/ai';
import { Link, useNavigate, useParams } from 'react-router-dom';
import CountdownTimer from './CountdownTimer';
import axiosClient from '../../utils/axiosClient';
import { useSelector } from 'react-redux';
import * as dayjs from 'dayjs';
import { toastError, toastSuccess } from '../../components/Toastify';
import axios from 'axios';

// console.log(Array.from({ length: 10 }, (_, index) => 0));
// console.log([...Array(10)].map(() => 0));
// console.log(new Array(10).fill(0));

const TestTheory = () => {
  const navigate = useNavigate();
  const { theoryExamId } = useParams();
  // const { email } = useSelector((state) => state?.auth?.user?.accountInfo);

  const email =
    useSelector((state) => state?.auth?.user?.accountInfo?.email) ||
    'ehioqwehqwoehioqweho@gmail.com';

  const currentDate = new Date();
  const [startedDate, setStartedDate] = useState('');
  const [open, setOpen] = useState(false);
  const [answerList, setAnswerList] = useState([]);
  const [questionList, setQuestionList] = useState([]);
  const [licenseDesc, setLicenseDesc] = useState();
  const charOption = ['A', 'B', 'C', 'D', 'E'];

  useEffect(() => {
    async function fetchData() {
      await axios
        .get(`/theory-exam/${theoryExamId}`)
        .then((response) => {
          setLicenseDesc(
            response?.data?.data?.questions[0]?.licenseType?.licenseTypeDesc
          );
          console.log('response: ', response);
          setQuestionList(response?.data?.data);
          setAnswerList(
            response?.data?.data?.questions?.map((item, index) => {
              return {
                questionId: item?.questionId,
                selectAnswer: '',
              };
            })
          );
          localStorage.setItem(
            'startedDate',
            dayjs(new Date())
              .format('YYYY-MM-DD')
              .concat(' '.concat(dayjs(new Date()).format('HH:mm:ss')))
          );
          setStartedDate(dayjs(new Date()).format('YYYY-MM-DD HH:mm:ss'));
        })
        .catch((error) => toastError(error?.response?.data?.message));
    }
    fetchData();
  }, []);

  const selectAnswer = (indexQuestion, indexOption) => {
    const newAnswerList = [...answerList];
    const indexAnswer = newAnswerList.findIndex(
      (itemAnswer) => itemAnswer.questionId === indexQuestion
    );
    newAnswerList[indexAnswer] = {
      questionId: indexQuestion,
      selectAnswer: indexOption,
    };
    setAnswerList(newAnswerList);
  };

  const handleSubmit = useCallback(async (id) => {
    console.log(
      JSON.stringify({
        email,
        theoryExamId,
        totalTime: questionList?.totalTime,
        startedDate: startedDate,
        selectedAnswers: answerList,
      })
    );
    await axios
      .post(`/theory/submit`, {
        email,
        theoryExamId,
        totalTime: questionList?.totalTime,
        startedDate: startedDate,
        selectedAnswers: answerList,
      })
      .then((res) => {
        console.log('res: ', res);
        toastSuccess(res?.data?.message);
        navigate(`/theory/result/${email}/${id}/${startedDate}`);
      })
      .catch((error) => {
        console.log('error: ', error);
        toastError(error?.response?.data?.message);
      });
  });

  console.log('answerList: ', answerList);
  console.log('questionList: ', questionList);
  console.log('licenseDesc: ', licenseDesc);

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
            <h2 className='text-center font-semibold uppercase text-[20px]'>
              Thời gian
            </h2>
            {questionList?.totalTime && (
              <CountdownTimer minutes={questionList?.totalTime} />
            )}
          </div>
          <div className=''>
            <h2 className='text-center font-medium text-[20px] my-2'>
              Câu hỏi
            </h2>
            <div className='grid grid-cols-5 gap-2'>
              {answerList?.map((item, index) => (
                <div
                  key={item?.questionId}
                  className={`
                  ${item?.selectAnswer === '' ? '' : 'selected-color '}
                   text-center cursor-pointer hover:text-white hover:bg-[#0D5EF4] border p-2`}
                >
                  <a href={`#${item?.questionId}`}>{index + 1}</a>
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
          {questionList &&
            questionList?.questions?.map((itemQuestion, indexQuestion) => (
              <div
                id={indexQuestion + 1}
                className='min-h-[360px] bg-white p-4 rounded-lg'
              >
                <div>
                  <span className='font-semibold text-[20px] bg-orange-300 p-2 rounded-lg'>
                    Câu Hỏi {indexQuestion + 1} :
                  </span>
                </div>
                {/* Content Question  */}
                <div className='flex gap-2 h-full mt-2 flex-1'>
                  <div className='flex-1 h-full'>
                    {itemQuestion.questionAnswerDesc}
                    {itemQuestion.image && (
                      <div className='flex justify-center'>
                        <img
                          src={itemQuestion.image}
                          alt='img'
                          className='w-[80%] p-4'
                        />
                      </div>
                    )}
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
                              item?.answer
                                ? 'selected-color '
                                : ''
                            } 
                            border p-2 cursor-pointer hover:opacity-80 rounded`}
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
