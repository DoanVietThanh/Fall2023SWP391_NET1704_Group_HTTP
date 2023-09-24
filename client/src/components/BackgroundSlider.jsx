import React from 'react';

const BackgroundSlider = ({ url, breadcrumbs }) => {
  return (
    <div className='slider-img mb-8'>
      <div>
        <img src={url} alt='slider' />
      </div>
      <h1 className='text-white font-medium text-[50px] absolute top-[50%] left-[50%] translate-y-[-50%] translate-x-[-50%]'>
        {breadcrumbs}
      </h1>
    </div>
  );
};

export default BackgroundSlider;
