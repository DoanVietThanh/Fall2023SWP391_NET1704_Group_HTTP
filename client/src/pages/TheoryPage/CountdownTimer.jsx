import React, { useState, useEffect } from 'react';
import * as dayjs from 'dayjs';

const CountdownTimer = ({ minutes = 0, seconds = 0 }) => {
  const [timeRemaining, setTimeRemaining] = useState({
    minutes,
    seconds,
  });

  useEffect(() => {
    const intervalId = setInterval(() => {
      if (timeRemaining.minutes === 0 && timeRemaining.seconds === 0) {
        clearInterval(intervalId);
        alert('Hết giờ');
        return;
      }

      let newMinutes = timeRemaining.minutes;
      let newSeconds = timeRemaining.seconds - 1;

      if (newSeconds < 0) {
        newMinutes -= 1;
        newSeconds = 59;
      }

      setTimeRemaining({
        minutes: newMinutes,
        seconds: newSeconds,
      });
    }, 1000);

    return () => clearInterval(intervalId);
  }, [timeRemaining]);

  // Sử dụng padStart để thêm số 0 nếu phút là số đơn
  const formattedMinutes = timeRemaining.minutes.toString().padStart(2, '0');
  const formattedSeconds = timeRemaining.seconds.toString().padStart(2, '0');

  return (
    <div>
      <div className='border p-4 rounded-full'>
        <p className='text-[20px] font-medium text-center'>{`${formattedMinutes} : ${formattedSeconds}`}</p>
        {/* <div>{dayjs(new Date()).format('YYYY-MM-DD HH:mm:ss')}</div> */}
        <div>Thời gian đã làm: {minutes - timeRemaining.minutes} phút</div>
      </div>
    </div>
  );
};

export default CountdownTimer;
