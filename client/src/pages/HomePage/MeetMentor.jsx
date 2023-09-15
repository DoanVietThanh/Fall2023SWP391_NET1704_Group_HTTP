import React, { useState } from 'react';

const MeetMentor = () => {
  const [valueInput, setValueInput] = useState();
  console.log(valueInput);
  return (
    <div>
      MeetMentor
      <input
        type='date'
        name=''
        id=''
        value={valueInput}
        onChange={(e) => setValueInput(e.target.value)}
      />
    </div>
  );
};

export default MeetMentor;
