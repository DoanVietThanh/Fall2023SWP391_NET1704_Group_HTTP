import React from "react";
import { AiOutlineArrowRight } from "react-icons/ai";
import "swiper/css";
import "swiper/css/navigation";
import "swiper/css/pagination";
import "swiper/css/scrollbar";
import {
  A11y,
  Autoplay,
  Navigation,
  Pagination,
  Scrollbar,
} from "swiper/modules";
import { Swiper, SwiperSlide } from "swiper/react";
import theme from "../../theme";

const Slider = () => {
  return (
    <Swiper
      modules={[Navigation, Pagination, Scrollbar, A11y, Autoplay]}
      slidesPerView={1}
      spaceBetween={50}
      navigation
      autoplay={{
        delay: 4000,
        disableOnInteraction: false,
      }}
      loop
      pagination={{ clickable: true }}
    >
      <SwiperSlide>
        <div className="w-full relative">
          <div className="slider-img">
            <img
              className="slider-bg object-cover"
              src={`/img/bgSlider1.png`}
              alt="slider"
            />
            <img
              className="slider-hero"
              src={`/img/heroSlider1.png`}
              alt="slider"
            />
            <img
              className="slider-decor"
              src={`/img/decorSlider.png`}
              alt="slider"
            />
          </div>
          <div className="slider-content flex flex-col gap-8 justify-center">
            <h1 className="text-center font-extrabold text-[74px] tracking-wide leading-tight text-gray-800">
              Hỗ trợ học thi
              <span className={`text-[${theme.color.mainColor}]`}>
                {" "}
                bằng lái xa
              </span>
            </h1>
            <p className="font-semibold text-sky-900 text-[24px] pr-60 pt-5">
              Lot E2a-7, Street D1, D. D1, Long Thanh My, Thu Duc City, Ho Chi
              Minh City 700000
            </p>
            <button className="btn w-[200px] flex items-center justify-center gap-4">
              Get Started <AiOutlineArrowRight size={20} />
            </button>
          </div>
        </div>
      </SwiperSlide>
    </Swiper>
  );
};

export default Slider;
