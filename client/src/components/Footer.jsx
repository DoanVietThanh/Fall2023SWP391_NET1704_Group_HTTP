import React from 'react';
import { BsTelephone } from 'react-icons/bs';
import { MdAddLocation, MdOutlineEmail } from 'react-icons/md';
import { useInView } from 'react-intersection-observer';
import { animated, useSpring } from 'react-spring';
import theme from '../theme';

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
            <span className='text-slate-500'>Gọi đến số điện thoại:</span>
            <h2>+364745067</h2>
          </div>
        </div>

        <div className='flex gap-2 border-l-r'>
          <div className='footer-icons '>
            <MdOutlineEmail size={24} className='text-white p-1' />
          </div>
          <div>
            <span className='text-slate-500'>Email 24/7:</span>
            <h2>teamhttp@gmail.com</h2>
          </div>
        </div>

        <div className='flex gap-2'>
          <div className='footer-icons '>
            <MdAddLocation size={24} className='text-white p-1' />
          </div>
          <div>
            <span className='text-slate-500'>Địa chỉ: </span>
            <h2>Thủ Đức - TP.HCM</h2>
          </div>
        </div>
      </div>

      <div className='footer-link'>
        <div className='flex gap-6'>
          <div className='flex-1 flex flex-col'>
            <div className='w-[100px] h-[100px] flex justify-center items-center'>
              <img src={`/img/logo.png`} alt='logo' className='mb-5' />
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
            <h1 className='text-[20px] font-medium'>Liên lạc</h1>
            <div className='pt-4'>
              <form action='' className='flex'>
                <input
                  required
                  type='email'
                  placeholder='Enter email...'
                  className='px-1 py-2'
                />
                <button className='btn'></button>
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
              Chính sách
            </span>
            <span className='hover:opacity-80 hover:cursor-pointer'>
              Điều kiện
            </span>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
