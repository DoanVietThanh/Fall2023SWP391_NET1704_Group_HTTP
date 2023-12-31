import * as dayjs from 'dayjs';
import React, { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import theme from '../../theme';

import TabContext from '@mui/lab/TabContext';
import TabList from '@mui/lab/TabList';
import TabPanel from '@mui/lab/TabPanel';
import Box from '@mui/material/Box';
import Tab from '@mui/material/Tab';
import {
  AiFillFacebook,
  AiFillStar,
  AiFillTwitterCircle,
  AiFillYoutube,
  AiOutlineStar,
} from 'react-icons/ai';
import { BsCheckCircleFill, BsPersonCircle } from 'react-icons/bs';
import { GrDocumentText } from 'react-icons/gr';
import { MdOutlinePeopleAlt } from 'react-icons/md';
import { PiStudentBold } from 'react-icons/pi';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import Accordion from '@mui/material/Accordion';
import AccordionDetails from '@mui/material/AccordionDetails';
import AccordionSummary from '@mui/material/AccordionSummary';

import { Textarea } from '@mui/joy';
import { BsFillArrowRightCircleFill } from 'react-icons/bs';
import { useSelector } from 'react-redux';
import Loading from '../../components/Loading';
import axiosClient from '../../utils/axiosClient';
import { toastError } from './../../components/Toastify';
import DialogReservation from './components/DialogReservation';
import axios from 'axios';

const DetailCourse = () => {
  const navigate = useNavigate();
  const { memberId } =
    useSelector((state) => state?.auth?.user?.accountInfo) || '';
  const { idCourse } = useParams();
  const [course, setCourse] = useState();

  const [value, setValue] = useState('1');
  const [totalMember, setTotalMember] = useState(0);
  const [selectedMentor, setSelectedMentor] = useState('');
  const [open, setOpen] = useState(false);
  const [paymentList, setPaymentList] = useState([]);
  const [typePayment, setTypePayment] = useState(1);
  const [coursePackage, setCoursePackage] = useState([]);
  const [selectedCoursePackage, setSelectedCoursePackage] = useState();

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  const handleSelectMentor = (event) => {
    setSelectedMentor(event.target.value);
  };

  const handleClickOpen = (e) => {
    e.preventDefault();
    setOpen(true);
  };

  const handleClose = () => setOpen(false);

  useEffect(() => {
    async function courseDetail() {
      const res = await axios.get(`/courses/${idCourse}`).catch((error) => {
        toastError(error?.response?.data?.message);
        navigate('/home');
      });
      setCoursePackage(res?.data?.data.course.coursePackages);
      const payment = await axios
        .get(`/courses/packages/reservation`)
        .catch((error) => {
          toastError(error?.response?.data?.message);
          navigate('/home');
        });
      if (res?.data.statusCode === 200 && payment?.data.statusCode === 200) {
        setTotalMember(res?.data?.data.totalMember);
        setCourse(res?.data?.data?.course);
        setPaymentList(payment?.data?.data);
      }
    }
    courseDetail();
  }, []);

  console.log('course: ', course);
  console.log('paymentList: ', paymentList);

  // const submitPayment = async () => {
  //   try {
  //     const resReservation = await axiosClient
  //       .post(`/courses/packages/reservation`, {
  //         memberId,
  //         mentorId: selectedMentor,
  //         paymentTypeId: typePayment,
  //         paymentAmount: course?.cost,
  //         courseId: course?.courseId,
  //       })
  //       .catch((error) => toastError(error?.response?.data?.message));
  //     console.log('resReservation: ', resReservation);
  //     const resPayment = await axiosClient.post(
  //       `/api/payment`,
  //       resReservation?.data?.data
  //     );
  //     console.log('resPayment:', resPayment);
  //     window.location.href = resPayment?.data?.data.paymentUrl;
  //   } catch (error) {
  //     // toastError(error);
  //   }
  // };

  console.log(coursePackage);

  const scrollToSection = () => {
    //phan tu muon cuon den
    const section = document.getElementById('targetSection');

    if (section) {
      //cuon den
      section.scrollIntoView({ behavior: 'smooth' });
    }
  };
  return (
    <>
      <Header />

      {course ? (
        <div className='p-8'>
          <div className='p-4 border'>
            <div className='min-h-[300px]'>
              <div className='bannerCourse'>
                <img
                  className='w-full h-full object-cover'
                  src='/img/courseB1Pic.png'
                  alt='hinhdemo'
                />
                <button className='btnRegister' onClick={scrollToSection}>
                  Đăng ký ngay
                </button>
              </div>
              <div className='py-6 flex gap-8'>
                <div className='flex-x gap-2 border-r-2 pr-8'>
                  <MdOutlinePeopleAlt className='text-blue-700' size={20} />{' '}
                  Tổng số học sinh : {totalMember}
                </div>
              </div>

              <h1 className='font-bold text-[30px]'>{course.courseTitle}</h1>

              <div className='pt-4 flex gap-8'>
                <div className='flex-x gap-2 border-r-2 pr-8'>
                  <div>
                    <p className='font-medium'>Loại bằng:</p>
                    <p className='text-center'>B1</p>
                  </div>
                </div>
                <div className='flex-x gap-2 border-r-2 pr-8'>
                  <div>
                    <p className='font-medium'>Ngày khai giảng:</p>
                    <p>{dayjs(course?.startDate).format('DD / MM / YYYY')}</p>
                  </div>
                </div>
                {/* <div className='hidden flex-x gap-2'>
                  <BsPersonCircle className='text-blue-700' size={30} />
                  <div>
                    <p className='font-medium'>Rating:</p>
                    <p className='flex-x'>
                      <AiFillStar className='text-yellow-400' />
                      <AiFillStar className='text-yellow-400' />
                      <AiFillStar className='text-yellow-400' />
                      <AiFillStar className='text-yellow-400' />
                      <AiOutlineStar />
                    </p>
                  </div>
                </div> */}
              </div>
            </div>

            <div className='p-4 border mt-4'>
              <Box sx={{ width: '100%', typography: 'body1' }}>
                <TabContext value={value}>
                  <Box
                    sx={{ borderBottom: 1, borderColor: 'divider' }}
                    className='w-full flex justify-between'
                  >
                    <TabList
                      onChange={handleChange}
                      aria-label='lab API tabs example'
                      className='w-full flex justify-between'
                    >
                      <Tab label='Giới thiệu' value='1' className='uppercase' />
                      <Tab
                        label='Khung chương trình'
                        value='2'
                        className='uppercase'
                      />
                      <Tab label='Giảng viên' value='3' className='uppercase' />
                      {/* <Tab label='Phản hồi' value='4' className='uppercase' /> */}
                    </TabList>
                  </Box>
                  <TabPanel value='1'>
                    <div className='flex flex-col gap-8'>
                      <div>
                        <h1 className='font-bold text-[20px]'>Về Khóa học</h1>
                        <p className='pt-2'>
                          <span className='font-medium text-blue-700'>
                            Are you new to PHP or need a refresher?{' '}
                          </span>
                          <div
                            dangerouslySetInnerHTML={{
                              __html: course.courseDesc,
                            }}
                          />
                        </p>

                        <p className='pt-2'>
                          <span className='font-medium text-blue-700'>
                            Why?{' '}
                          </span>
                          Because Millions of websites and applications (the
                          majority) use PHP. You can find a job anywhere or even
                          work on your own, online and in places like freelancer
                          or Odesk. You can definitely make a substantial income
                          once you learn it.
                        </p>

                        <p className='pt-2'>
                          <span className='font-medium text-blue-700'>
                            Are you new to PHP or need a refresher?{' '}
                          </span>
                          Then this course will help you get all the
                          fundamentals of Procedural PHP, Object Oriented PHP,
                          MYSQLi and ending the course by building a CMS system
                          similar to WordPress, Joomla or Drupal.
                        </p>
                      </div>

                      <div>
                        <h1 className='font-bold text-[20px]'>My Approach</h1>
                        <p>
                          Practice, practice and more practice. Every section
                          inside this course has a practice lecture at the end,
                          reinforcing everything with went over in the lectures.
                          I also created a small application the you will be
                          able to download to help you practice PHP. To top it
                          off, we will build and awesome CMS like WordPress,
                          Joomla or Drupal.
                        </p>
                      </div>

                      <div className='flex justify-around'>
                        <div className='flex flex-col gap-4'>
                          <div className='flex-x gap-2'>
                            <BsCheckCircleFill className='text-blue-600' />{' '}
                            Learn Figma Basic to Advanced Design
                          </div>
                          <div className='flex-x gap-2'>
                            <BsCheckCircleFill className='text-blue-600' />{' '}
                            Justo non mauris pretium at tempor justo.
                          </div>{' '}
                          <div className='flex-x gap-2'>
                            <BsCheckCircleFill className='text-blue-600' />{' '}
                            Phasellus enim magna, varius et comm.
                          </div>
                        </div>

                        <div className='flex flex-col gap-4'>
                          <div className='flex-x gap-2'>
                            <BsCheckCircleFill className='text-blue-600' />{' '}
                            Learn Ut nullar Tellus, leafed eulimid pellet
                          </div>{' '}
                          <div className='flex-x gap-2'>
                            <BsCheckCircleFill className='text-blue-600' />{' '}
                            Phaseolus denim magna various.
                          </div>{' '}
                          <div className='flex-x gap-2'>
                            <BsCheckCircleFill className='text-blue-600' /> Sed
                            consequent just non mauri's.
                          </div>
                        </div>
                      </div>
                    </div>
                  </TabPanel>

                  <TabPanel value='2'>
                    <div>
                      <h1 className='font-bold text-[20px] mb-4'>
                        Danh sách bài học
                      </h1>

                      {course?.curricula.map((item, index) => (
                        <Accordion className='my-4 border' key={item.id}>
                          <AccordionSummary
                            expandIcon={<ExpandMoreIcon />}
                            aria-controls='panel1a-content'
                            id='panel1a-header'
                          >
                            <h1
                              className={`font-bold text-[20px] text-[${theme.color.mainColor}]`}
                            >
                              Bài {index + 1} {item.curriculumDesc}
                            </h1>
                          </AccordionSummary>
                          <AccordionDetails>
                            <div className='flex justify-between'>
                              <div className='flex-x gap-4'>
                                <GrDocumentText />
                                {item.curriculumDetail}
                              </div>
                              <span>02:30</span>
                            </div>
                          </AccordionDetails>
                        </Accordion>
                      ))}
                    </div>
                  </TabPanel>

                  <TabPanel value='3'>
                    <div className='flex flex-col gap-16'>
                      {course?.mentors?.map((item, index) => (
                        <div className='flex w-full gap-8'>
                          <div className='w-[30%] flex justify-center items-center'>
                            <img
                              alt='avt'
                              className='w-[160px] h-[160px] object-cover rounded-full'
                              src={item?.avatarImage}
                            />
                          </div>
                          <div className='flex-1 flex flex-col justify-between pl-4 border-l-2 text-[20px]'>
                            <div>
                              <span className='font-bold'> Giảng viên:</span>{' '}
                              {`${item?.firstName} ${item?.lastName}`}
                            </div>

                            <div className='flex-x gap-4'>
                              <span className='font-bold'> Tương tác:</span>{' '}
                              <Link to={`#`}>
                                <AiFillFacebook
                                  className={`text-[${theme.color.mainColor}]`}
                                  size={24}
                                />
                              </Link>
                              <Link to={`#`}>
                                <AiFillTwitterCircle
                                  className={`text-[${theme.color.mainColor}]`}
                                  size={24}
                                />
                              </Link>
                              <Link to={`#`}>
                                <AiFillYoutube
                                  className={`text-red-500`}
                                  size={24}
                                />
                              </Link>
                            </div>

                            <Link to={`/instructor/detail/${item?.staffId}`}>
                              <div className='flex items-center gap-4'>
                                <BsFillArrowRightCircleFill className='text-yellow-700' />
                                <span className='capitalize text-yellow-700'>
                                  Xem chi tiết ...
                                </span>
                              </div>
                            </Link>
                          </div>
                        </div>
                      ))}
                    </div>
                  </TabPanel>

                  <TabPanel value='4'>
                    <div className='hidden'>
                      <div className=' flex-x gap-4'>
                        <div className='flex flex-col gap-4 items-center border-r-2 px-8'>
                          <div className='text-[50px]'>4.0</div>
                          <div className='flex-x gap-2 text-blue-500'>
                            <AiFillStar size={20} />
                            <AiFillStar size={20} />
                            <AiFillStar size={20} />
                            <AiFillStar size={20} />
                            <AiOutlineStar size={20} />
                          </div>
                          <div>Tổng: {course?.feedBacks.length} đánh giá</div>
                        </div>
                        <div className='flex-1 flex flex-col gap-2'>
                          {[...Array(5)].map((item, index) => (
                            <div className='flex-x gap-4 ' key={index}>
                              <div className='flex-x gap-2'>
                                <AiOutlineStar className='text-blue-500' />{' '}
                                {index + 1}
                              </div>
                              <div className='flex-1 rounded-lg bg-gray-200'>
                                <div className='w-[70%] border rounded-lg border h-[10px] bg-[#0D5EF4]'></div>
                              </div>
                              <span>4 đánh giá </span>
                            </div>
                          ))}
                        </div>
                      </div>
                      {course?.feedBacks.map((itemFeedback, index) => (
                        <div className='flex-x gap-4 p-4' key={index}>
                          <div>
                            <img
                              src='/img/avtThanh.jpg'
                              alt='avt'
                              className='w-[140px] h-[140px] object-cover rou nded-lg'
                            />
                          </div>
                          <div className='flex-1 border flex flex-col gap-2 p-4'>
                            <div className='flex-x justify-between w-full'>
                              <h1 className='font-bold text-[18px] '>{`${itemFeedback.member.firstName} ${itemFeedback.member.lastName}`}</h1>
                              <div className='flex-x gap-2 text-blue-500'>
                                {[
                                  ...Array.from(
                                    { length: itemFeedback?.ratingStar },
                                    (_, index) => <AiFillStar size={20} />
                                  ),
                                ]}
                                {[
                                  ...Array.from(
                                    { length: 5 - itemFeedback?.ratingStar },
                                    (_, index) => <AiOutlineStar size={20} />
                                  ),
                                ]}
                              </div>
                            </div>
                            <div className='italic'>
                              {dayjs(itemFeedback?.createDate).format(
                                'DD-MM-YYYY'
                              )}
                            </div>
                            <div>{itemFeedback?.content}</div>
                          </div>
                        </div>
                      ))}

                      <div className='my-4'>
                        <h1 className='font-bold text-[24px] capitalize'>
                          Bình luận
                        </h1>
                        <form action=''>
                          <Textarea
                            minRows={3}
                            size='lg'
                            placeholder='Nhập bình luận...'
                          />
                          <div className='flex justify-end mt-4'>
                            <button className='btn' type='submit'>
                              Submit
                            </button>
                          </div>
                        </form>
                      </div>
                    </div>
                  </TabPanel>
                </TabContext>
              </Box>
            </div>
          </div>

          {coursePackage && (
            <div className='my-4'>
              <h1
                id='targetSection'
                className='font-bold text-[30px] text-yellow-700 text-center cappitalize '
              >
                Chọn gói liên quan
              </h1>
              <div className='flex gap-4'>
                {coursePackage?.map((item, index) => (
                  <div className='p-8 border flex-1 shadow-md'>
                    <h1 className='text-center font-bold text-[26px]'>
                      {item?.coursePackageDesc}
                    </h1>
                    <ul className='my-4 list-disc pl-[20%] font-bold'>
                      {item?.cost && (
                        <li>
                          {`Ưu đãi: Từ `}
                          <span className='text-red-700 text-[20px]'>
                            {item?.cost}{' '}
                          </span>
                          VNĐ khi đăng kí Online
                        </li>
                      )}
                      <li>
                        {`Học phí `}
                        <span className='text-yellow-500'>trọn gói 100%</span>
                      </li>
                      {item?.totalSession && (
                        <li>
                          Số buổi học:{' '}
                          <span className='text-green-900 font-bold '>
                            {item?.totalSession}
                          </span>
                        </li>
                      )}

                      {item?.sessionHour && (
                        <li>
                          Thời lượng mỗi buổi học:{' '}
                          <span className='text-green-900 font-bold'>
                            {item?.sessionHour}
                          </span>
                        </li>
                      )}
                      <li>Học gần nhà, thời gian linh hoạt</li>
                      <li>Học 1 thầy 1 trò</li>
                      <li className='font-semibold'>
                        Cam kết KHÔNG phát sinh thêm chi phí
                      </li>
                      {item?.ageRequired && (
                        <li>
                          Yêu cầu tuổi phải trên{' '}
                          <span className='text-red-700'>
                            {item?.ageRequired}
                          </span>
                        </li>
                      )}
                    </ul>
                    <div className='flex justify-center mt-4'>
                      <button
                        className='btn'
                        onClick={() => {
                          setOpen(true);
                          setSelectedCoursePackage(item);
                        }}
                      >
                        Đăng ký ngay
                      </button>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          )}

          <DialogReservation
            open={open}
            setOpen={setOpen}
            selectedCoursePackage={selectedCoursePackage}
            course={course}
          />
        </div>
      ) : (
        <Loading />
      )}

      <Footer />
    </>
  );
};

export default DetailCourse;
