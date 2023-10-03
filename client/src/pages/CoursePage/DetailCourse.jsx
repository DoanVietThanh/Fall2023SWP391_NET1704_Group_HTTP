import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import theme from '../../theme';

import TabContext from '@mui/lab/TabContext';
import TabList from '@mui/lab/TabList';
import TabPanel from '@mui/lab/TabPanel';
import Box from '@mui/material/Box';
import Tab from '@mui/material/Tab';
import {
  AiFillEye,
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

import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';

import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import { toastSuccess } from './../../components/Toastify';

const DetailCourse = () => {
  const navigate = useNavigate();
  const [value, setValue] = React.useState('1');
  const [selectedMentor, setSelectedMentor] = React.useState('');
  const [open, setOpen] = React.useState(false);

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  const handleSelectMentor = (event) => {
    setSelectedMentor(event.target.value);
  };

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  return (
    <>
      <Header />
      <div className='flex p-8 gap-4'>
        <div className='flex-1 p-4 border'>
          <div className='min-h-[300px]'>
            <div>
              <iframe
                width='100%'
                height='600px'
                src='https://www.youtube.com/embed/Do-W6ccKqCw?si=XXx0R-j2lMot57QO'
                title='YouTube video player'
                frameborder='0'
                allow='accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share'
                allowfullscreen
                className='rounded-lg'
              ></iframe>
            </div>

            <div className='py-6 flex gap-8'>
              <div className='flex-x gap-2 border-r-2 pr-8'>
                <MdOutlinePeopleAlt className='text-blue-700' size={20} />{' '}
                Student 2
              </div>
              <div className='flex-x gap-2'>
                <AiFillEye className='text-blue-700' size={20} /> Views 343
              </div>
            </div>

            <h1 className='font-bold text-[30px]'>
              Marketing 2023: Complete Guide To Social Growth
            </h1>

            <div className='pt-4 flex gap-8'>
              <div className='flex-x gap-2 border-r-2 pr-8'>
                <BsPersonCircle className='text-blue-700' size={30} />
                <div>
                  <p className='font-medium'>Intructor:</p>
                  <p>ThanhDoan</p>
                </div>
              </div>
              <div className='flex-x gap-2 border-r-2 pr-8'>
                <div>
                  <p className='font-medium'>Category:</p>
                  <p className='text-center'>B1</p>
                </div>
              </div>
              <div className='flex-x gap-2 border-r-2 pr-8'>
                <div>
                  <p className='font-medium'>Last Update:</p>
                  <p>12 / 10 / 2023</p>
                </div>
              </div>
              <div className='flex-x gap-2'>
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
              </div>
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
                    <Tab label='Over View' value='1' className='uppercase' />
                    <Tab label='Curriculum' value='2' className='uppercase' />
                    <Tab label='Instructor' value='3' className='uppercase' />
                    <Tab label='Reviews' value='4' className='uppercase' />
                  </TabList>
                </Box>
                <TabPanel value='1'>
                  <div className='flex flex-col gap-8'>
                    <div>
                      <h1 className='font-bold text-[20px]'>
                        About The Course
                      </h1>
                      <p className='pt-2'>
                        <span className='font-medium text-blue-700'>
                          Are you new to PHP or need a refresher?{' '}
                        </span>
                        Then this course will help you get all the fundamentals
                        of Procedural PHP, Object Oriented PHP, MYSQLi and
                        ending the course by building a CMS system similar to
                        WordPress, Joomla or Drupal.
                      </p>

                      <p className='pt-2'>
                        <span className='font-medium text-blue-700'>Why? </span>
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
                        Then this course will help you get all the fundamentals
                        of Procedural PHP, Object Oriented PHP, MYSQLi and
                        ending the course by building a CMS system similar to
                        WordPress, Joomla or Drupal.
                      </p>
                    </div>

                    <div>
                      <h1 className='font-bold text-[20px]'>My Approach</h1>
                      <p>
                        Practice, practice and more practice. Every section
                        inside this course has a practice lecture at the end,
                        reinforcing everything with went over in the lectures. I
                        also created a small application the you will be able to
                        download to help you practice PHP. To top it off, we
                        will build and awesome CMS like WordPress, Joomla or
                        Drupal.
                      </p>
                    </div>

                    <div className='flex justify-around'>
                      <div className='flex flex-col gap-4'>
                        <div className='flex-x gap-2'>
                          <BsCheckCircleFill className='text-blue-600' /> Learn
                          Figma Basic to Advanced Design
                        </div>
                        <div className='flex-x gap-2'>
                          <BsCheckCircleFill className='text-blue-600' /> Justo
                          non mauris pretium at tempor justo.
                        </div>{' '}
                        <div className='flex-x gap-2'>
                          <BsCheckCircleFill className='text-blue-600' />{' '}
                          Phasellus enim magna, varius et comm.
                        </div>
                      </div>

                      <div className='flex flex-col gap-4'>
                        <div className='flex-x gap-2'>
                          <BsCheckCircleFill className='text-blue-600' /> Learn
                          Ut nullar Tellus, leafed eulimid pellet
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
                      Summary Curriculam Course
                    </h1>

                    {[...Array(3)].map((item, index) => (
                      <Accordion className='my-4 border' key={index}>
                        <AccordionSummary
                          expandIcon={<ExpandMoreIcon />}
                          aria-controls='panel1a-content'
                          id='panel1a-header'
                        >
                          <h1
                            className={`font-bold text-[20px] text-[${theme.color.mainColor}]`}
                          >
                            Bài {index + 1}: Cách cầm tay lái
                          </h1>
                        </AccordionSummary>
                        <AccordionDetails>
                          <div className='flex justify-between'>
                            <div className='flex-x gap-4'>
                              <GrDocumentText />
                              What do course cover?
                            </div>
                            <span>02:30</span>
                          </div>
                        </AccordionDetails>
                      </Accordion>
                    ))}
                  </div>
                </TabPanel>

                <TabPanel value='3'>
                  <div className='flex w-full gap-8'>
                    <div className='w-[25%]'>
                      <img
                        alt='avt'
                        className='w-[160px] h-[160px] object-cover rounded-full'
                        src='/img/avtThanh.jpg'
                      />
                    </div>
                    <div className='flex-1 flex flex-col justify-between pl-4 border-l-2 text-[20px]'>
                      <div>
                        <span className='font-bold'> Intructor:</span> Đoàn Viết
                        Thanh
                      </div>
                      <div className='flex-x gap-2'>
                        <div className='flex-x gap-2'>
                          <PiStudentBold
                            className={`text-[${theme.color.mainColor}]`}
                          />
                          <span>100 students</span>
                        </div>
                        <div className='flex-x gap-2'>
                          <GrDocumentText
                            className={`text-[${theme.color.mainColor}]`}
                          />
                          <span>90 courses</span>
                        </div>
                      </div>
                      <div className='flex-x gap-4'>
                        <span className='font-bold'> Contact:</span>{' '}
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
                          <AiFillYoutube className={`text-red-500`} size={24} />
                        </Link>
                      </div>
                    </div>
                  </div>
                </TabPanel>

                <TabPanel value='4'>
                  <div className='flex-x gap-4'>
                    <div className='flex flex-col gap-4 items-center border-r-2 px-8'>
                      <div className='text-[50px]'>4.0</div>
                      <div className='flex-x gap-2 text-blue-500'>
                        <AiFillStar size={20} />
                        <AiFillStar size={20} />
                        <AiFillStar size={20} />
                        <AiFillStar size={20} />
                        <AiOutlineStar size={20} />
                      </div>
                      <div>Total 20 Ratings</div>
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
                          <span>4 rating(s)</span>
                        </div>
                      ))}
                    </div>
                  </div>

                  {[...Array(3)].map((item, index) => (
                    <div className='flex-x gap-4 p-4' key={index}>
                      <div>
                        <img
                          src='/img/avtThanh.jpg'
                          alt='avt'
                          className='w-[140px] h-[140px] object-cover rounded-lg'
                        />
                      </div>
                      <div className='flex-1 border flex flex-col gap-2 p-4'>
                        <div className='flex-x justify-between w-full'>
                          <h1 className='font-medium'>Thanh Đoàn</h1>
                          <div className='flex-x gap-2 text-blue-500'>
                            <AiFillStar size={20} />
                            <AiFillStar size={20} />
                            <AiFillStar size={20} />
                            <AiFillStar size={20} />
                            <AiOutlineStar size={20} />
                          </div>
                        </div>
                        <div className='italic'>2 days ago</div>
                        <div>
                          Lorem ipsum dolor sit amet consectetur adipisicing
                          elit. Natus iure cumque sint molestiae reiciendis,
                          soluta doloribus nisi explicabo ut repellat.
                        </div>
                      </div>
                    </div>
                  ))}
                </TabPanel>
              </TabContext>
            </Box>
          </div>
        </div>

        <div className='w-[25%] p-4 border flex flex-col gap-4'>
          <div className='px-8'>
            <img src='/img/course1.png' alt='' className='w-full' />
          </div>
          <p className='text-center font-medium text-[24px]'>
            Cost: <span className='text-yellow-300'>$99</span>
          </p>
          <h2 className='text-center font-medium text-[24px] mb-4'>
            Tổng tiết học: 88
          </h2>

          <Box sx={{ minWidth: 120 }}>
            <FormControl fullWidth>
              <InputLabel id='demo-simple-select-label'>
                Chọn người hướng dẫn
              </InputLabel>
              <Select
                labelId='demo-simple-select-label'
                id='demo-simple-select'
                value={selectedMentor}
                label='Thầy Thanh'
                onChange={handleSelectMentor}
                defaultValue={10}
                required={true}
              >
                <MenuItem value={10}>Thầy Thanh </MenuItem>
                <MenuItem value={20}>Thầy Phước </MenuItem>
                <MenuItem value={30}>Thầy Huy </MenuItem>
                <MenuItem value={40}>Cô Thư </MenuItem>
              </Select>
            </FormControl>
          </Box>

          <button className='btn' onClick={handleClickOpen}>
            Thanh Toán
          </button>

          <Dialog
            open={open}
            onClose={handleClose}
            aria-labelledby='alert-dialog-title'
            aria-describedby='alert-dialog-description'
          >
            <DialogTitle id='alert-dialog-title'>
              {'Chọn phương thức thanh toán?'}
            </DialogTitle>
            <DialogContent>
              <form action=''>
                <select name='' id=''>
                  <option value='1'>Thanh toán trực tiếp</option>
                  <option value='2'>Thanh toán trực tuyến</option>
                </select>
              </form>
            </DialogContent>
            <DialogActions>
              <Button onClick={handleClose}>Hủy</Button>
              <Button
                onClick={() => {
                  toastSuccess('Thanh toán thành công');
                  navigate('/home');
                }}
                autoFocus
              >
                Xác nhận
              </Button>
            </DialogActions>
          </Dialog>
        </div>
      </div>

      <Footer />
    </>
  );
};

export default DetailCourse;
