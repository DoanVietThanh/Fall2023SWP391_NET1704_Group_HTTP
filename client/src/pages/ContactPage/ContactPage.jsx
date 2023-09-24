import React from 'react';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import BackgroundSlider from '../../components/BackgroundSlider';
import { TextField } from '@mui/material';
import { Textarea } from '@mui/joy';
import { BiLocationPlus } from 'react-icons/bi';
import { AiOutlinePhone, AiOutlineClockCircle } from 'react-icons/ai';

const ContactPage = () => {
  const url =
    'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png';
  const breadcrumbs = 'Contact';
  return (
    <div>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      <div className='m-4 p-4 border'>
        <div>
          <iframe
            title='fpt google map'
            src='https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3918.6099415305202!2d106.80730807587666!3d10.84113285799513!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x31752731176b07b1%3A0xb752b24b379bae5e!2zVHLGsOG7nW5nIMSQ4bqhaSBo4buNYyBGUFQgVFAuIEhDTQ!5e0!3m2!1svi!2s!4v1695544056354!5m2!1svi!2s'
            width='100%'
            height='500'
            allowfullscreen=''
            loading='lazy'
            referrerpolicy='no-referrer-when-downgrade'
          ></iframe>
        </div>
        <div className='flex border mt-4'>
          <div className='flex-1 p-4'>
            <h1 className='font-bold text-[30px]'>Have any Questions?</h1>
            <p className='py-2 text-gray-500'>
              Have a inquiry or some feedback for us? Fill out the form below to
              contact our team.
            </p>
            <div className='flex flex-col gap-4'>
              <div className='border rounded-lg p-4 flex gap-4'>
                <div className='center'>
                  <BiLocationPlus size={40} className='text-blue-500' />
                </div>
                <div>
                  <h4 className='font-medium text-[20px]'>Our Address</h4>
                  <p>
                    Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức,
                    Thành phố Hồ Chí Minh 700000
                  </p>
                </div>
              </div>
              <div className='border rounded-lg p-4 flex gap-4'>
                <div className='center'>
                  <AiOutlinePhone size={40} className='text-blue-500' />
                </div>
                <div>
                  <h4 className='font-medium text-[20px]'>Phone Number</h4>
                  <p>{`(+84) 364 . 745 . 067`}</p>
                </div>
              </div>
              <div className='border rounded-lg p-4 flex gap-4'>
                <div className='center'>
                  <AiOutlineClockCircle size={40} className='text-blue-500' />
                </div>
                <div>
                  <h4 className='font-medium text-[20px]'>
                    Hours of Operation
                  </h4>
                  <p>Monday - Friday: 09:00 - 20:00</p>
                  <p>Sunday & Saturday: 10:30 - 22:00</p>
                </div>
              </div>
            </div>
          </div>
          <div className='flex-1 p-4'>
            <h3 className='uppercase text-[16px] font-medium text-blue-700'>
              contact with us!
            </h3>
            <h1 className='font-bold text-[30px]'>Get in Touch</h1>
            <p className='text-gray-500'>
              Lorem ipsum dolor sit amet adipiscing elit, sed do eiusmod tempor
              eiusmod tempor incididunt ut labore et dolore magna aliqua.
            </p>
            <form action='' className='pt-8'>
              <div className='flex flex-col gap-8'>
                <div className='flex gap-4'>
                  <TextField
                    id='outlined-basic'
                    label='Your name'
                    variant='outlined'
                    className='w-full'
                  />{' '}
                  <TextField
                    id='outlined-basic'
                    label='Phone Number'
                    variant='outlined'
                    className='w-full'
                  />
                </div>
                <div className='flex gap-4'>
                  <TextField
                    id='outlined-basic'
                    label='Email Address'
                    variant='outlined'
                    className='w-full'
                  />
                </div>
                <Textarea placeholder='Write your message...' />
              </div>
              <div className='flex justify-end'>
                <button className='btn mt-8'>send message</button>
              </div>
            </form>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default ContactPage;
