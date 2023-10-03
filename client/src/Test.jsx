import React, { useState } from 'react';
import { AiOutlinePlus, AiOutlineMinusCircle } from 'react-icons/ai';

const Test = () => {
  const [value, setValue] = useState(0);
  const [value1, setValue1] = useState(0);

  return (
    <div className='p-4'>
      <div className='center flex gap-4'>
        <div onClick={() => setValue(value + 1)} className='cursor-pointer'>
          <AiOutlinePlus />
        </div>
        <div className='text-[20px] text-blue-400'>{value}</div>
        <div onClick={() => setValue(value - 1)} className='cursor-pointer'>
          <AiOutlineMinusCircle />
        </div>
        <div>Giá 600000</div>
      </div>

      <div className='center flex gap-4'>
        <div onClick={() => setValue1(value1 + 1)} className='cursor-pointer'>
          <AiOutlinePlus />
        </div>
        <div className='text-[20px] text-blue-400'>{value1}</div>
        <div onClick={() => setValue1(value1 - 1)} className='cursor-pointer'>
          <AiOutlineMinusCircle />
        </div>
        <div>Giá 400000</div>
      </div>

      <h1>Total Price: {value * 60000 + value1 * 40000}</h1>
    </div>
  );
};

export default Test;
