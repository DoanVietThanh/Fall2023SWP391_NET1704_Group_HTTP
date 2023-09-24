import React from 'react';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import Box from '@mui/material/Box';
import Tab from '@mui/material/Tab';
import TabContext from '@mui/lab/TabContext';
import TabList from '@mui/lab/TabList';
import TabPanel from '@mui/lab/TabPanel';
import { BsCheckCircleFill, BsPersonCircle, BsFacebook } from 'react-icons/bs';
import { MdOutlinePeopleAlt } from 'react-icons/md';
import {
  AiFillEye,
  AiFillStar,
  AiOutlineStar,
  AiFillTwitterCircle,
} from 'react-icons/ai';
import Avatar from '@mui/material/Avatar';

const DetailCourse = () => {
  const [value, setValue] = React.useState('1');

  const handleChange = (event, newValue) => {
    setValue(newValue);
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
                    <h1 className='font-bold text-[20px]'>About The Course</h1>
                    <p>
                      <span className='font-medium'>
                        Are you new to PHP or need a refresher?{' '}
                      </span>
                      Then this course will help you get all the fundamentals of
                      Procedural PHP, Object Oriented PHP, MYSQLi and ending the
                      course by building a CMS system similar to WordPress,
                      Joomla or Drupal.
                    </p>
                  </div>
                </TabPanel>

                <TabPanel value='3'>
                  <div className='flex w-full gap-8'>
                    <div className='w-[25%]'>
                      <img
                        alt='avt'
                        className='w-[160px] h-[160px] object-cover rounded-full'
                        src='https://scontent.fsgn5-10.fna.fbcdn.net/v/t39.30808-6/326718942_3475973552726762_6277150844361274430_n.jpg?_nc_cat=107&ccb=1-7&_nc_sid=a2f6c7&_nc_ohc=F9AAbXAwDU8AX-6HyPn&_nc_ht=scontent.fsgn5-10.fna&oh=00_AfA5o3lKSL4jdT7prHoH6xLbUkhlpdXxlassURqgMA6viw&oe=6514AD6C'
                      />
                    </div>
                    <div className='flex-1 flex flex-col justify-between border text-[20px]'>
                      <div>
                        <span className='font-bold'> Intructor:</span> Đoàn Viết
                        Thanh
                      </div>
                      <div>ahihi</div>
                      <div>
                        <span className='font-bold'> Contact:</span>{' '}
                      </div>
                    </div>
                  </div>
                </TabPanel>

                <TabPanel value='4'>
                  <div>
                    <h1 className='font-bold text-[20px]'>About The Course</h1>
                    <p>
                      <span className='font-medium'>
                        Are you new to PHP or need a refresher?{' '}
                      </span>
                      Then this course will help you get all the fundamentals of
                      Procedural PHP, Object Oriented PHP, MYSQLi and ending the
                      course by building a CMS system similar to WordPress,
                      Joomla or Drupal.
                    </p>
                  </div>
                </TabPanel>
              </TabContext>
            </Box>
          </div>
        </div>

        <div className='w-[25%] p-4 border'>content</div>
      </div>

      <Footer />
    </>
  );
};

export default DetailCourse;
