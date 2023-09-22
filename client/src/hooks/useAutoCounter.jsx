import React from 'react';
import { InView, useInView } from 'react-intersection-observer';
import { animated, useSpring } from 'react-spring';

export function ComponentToDetect({ data }) {
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

const useAutoCounter = ({ number }) => {
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
    <InView onChange={handleInViewChange}>
      {({ inView, ref }) => (
        <div ref={ref}>
          {inView ? <ComponentToDetect data={number} /> : <>bug</>}
        </div>
      )}
    </InView>
  );
};

export default useAutoCounter;
