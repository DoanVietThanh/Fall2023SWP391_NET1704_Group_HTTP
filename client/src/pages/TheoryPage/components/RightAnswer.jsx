import React from 'react';

const RightAnswer = ({ listAnswer }) => {
  const charOption = ['A', 'B', 'C', 'D', 'E'];
  const rightAnswer = listAnswer.filter((item) => item.isTrue);
  return (
    <div className='font-bold'>
      {`${charOption[rightAnswer[0]?.questionAnswerId]}) ${
        rightAnswer[0]?.answer
      }`}
    </div>
  );
};

export default RightAnswer;
