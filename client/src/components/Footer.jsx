import React, { useState } from 'react';
import images from './../assets/img/index';
import { BsTelephone } from 'react-icons/bs';
import { MdOutlineEmail, MdAddLocation } from 'react-icons/md';
import theme from '../theme';
import { useSpring, animated } from 'react-spring';
import { useInView, InView } from 'react-intersection-observer';

function ComponentToDetect({ data }) {
  const [ref, inView] = useInView();
  const { number } = useSpring({
    from: { number: 0 },
    number: data,
    delay: 500,
    config: { mass: 1, tension: 20, friction: 10 },
  });
  if (inView) {
  }

  return (
    <div ref={ref}>
      {inView ? (
        <animated.div>{inView && number.to((n) => n.toFixed(0))}</animated.div>
      ) : null}
    </div>
  );
}

const Footer = () => {
  const handleInViewChange = (inView) => {
    if (inView) {
      console.log('Phần tử hiện ra trong viewport');
      // Thực hiện các tác vụ khi phần tử hiện ra trong viewport
    } else {
      console.log('Phần tử ra khỏi viewport');
      // Thực hiện các tác vụ khi phần tử ra khỏi viewport
    }
  };
  return (
    <footer
      className={`footer bg-[${theme.color.bgFooterColor}] p-8 text-white`}
    >
      {/* <InView onChange={handleInViewChange}>
        {({ inView, ref }) => (
          <div ref={ref}>
            {inView ? <ComponentToDetect data={1000} /> : <>bug</>}
          </div>
        )}
      </InView> */}
      <div className='flex flex-row justify-around items-center py-4'>
        <div className='flex gap-2'>
          <div className='footer-icons '>
            <BsTelephone size={24} className='text-white p-1' />
          </div>
          <div>
            <span className='text-slate-500'>Call us any time:</span>
            <h2>+364745067</h2>
          </div>
        </div>

        <div className='flex gap-2 border-l-r'>
          <div className='footer-icons '>
            <MdOutlineEmail size={24} className='text-white p-1' />
          </div>
          <div>
            <span className='text-slate-500'>Email us 24/7:</span>
            <h2>teamhttp@gmail.com</h2>
          </div>
        </div>

        <div className='flex gap-2'>
          <div className='footer-icons '>
            <MdAddLocation size={24} className='text-white p-1' />
          </div>
          <div>
            <span className='text-slate-500'>Our location: </span>
            <h2>Thủ Đức- TP.HCM</h2>
          </div>
        </div>
      </div>

      <div className='footer-link'>
        <div className='flex gap-6'>
          <div className='flex-1 flex flex-col'>
            <div className='w-[100px] h-[100px] flex justify-center items-center'>
              <img src={`/img/logo.png`} alt='logo' className='' />
            </div>
            <div>
              <p>
                Continually optimize backward manufactured products whereas
                communities negotiate life compelling alignments
              </p>
            </div>
          </div>

          <div className='flex-1 '>
            <h1 className='text-[20px] font-medium'>Quick Links</h1>
            <ul className='footer-ul'>
              <li className='pt-4'>Development</li>
              <li>Marketing</li>
              <li>Data Science</li>
              <li>Business</li>
            </ul>
          </div>
          <div className='flex-1 '>
            <h1 className='text-[20px] font-medium'>Resources</h1>
            <ul className='footer-ul'>
              <li className='pt-4'>Community</li>
              <li>Support</li>
              <li>Video Guides</li>
              <li>Documentation</li>
            </ul>
          </div>
          <div className='flex-1 '>
            <h1 className='text-[20px] font-medium'>Get in Touch</h1>
            <div className='pt-4'>
              <form action='' className='flex'>
                <input
                  required
                  type='email'
                  placeholder='Enter email...'
                  className='px-1 py-2'
                />
                <button className='btn'>Submit</button>
              </form>
            </div>
          </div>
        </div>

        <div className='flex justify-between pt-8'>
          <span>
            Copyright © 2023 by
            <span className='text-[#0D5EF4]'> HTTP Team</span>.
          </span>
          <div className='center text-white gap-8 '>
            <span className='hover:opacity-80 hover:cursor-pointer'>
              Privacy Policy
            </span>
            <span className='hover:opacity-80 hover:cursor-pointer'>
              Terms & Condition
            </span>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
