import React from "react";
import { AiOutlineArrowRight, AiFillCheckCircle } from "react-icons/ai";
import theme from "../../theme";
import { BsBookmarkStarFill } from "react-icons/bs";
import { Link } from "react-router-dom";
const Welcome = () => {
  return (
    <div className="m-20 flex gap-6">
      <div className="flex-1 border border-1 overflow-hidden rounded-md">
        <div className="relative w-full h-[460px]">
          <div className="absolute rounded-lg w-full ">
            <img
              src="/img/about2.png"
              alt="About"
              className="w-full object-cover rounded-md"
            />
          </div>

          <div className="absolute right-0">
            <img src="/img/about4.png" alt="About" className="rounded-md"/>
          </div>
        </div>
      </div>

      <div className="flex-1">
        <h3
          className={`text-[16px] text-[${theme.color.mainColor}] flex items-center gap-2 font-bold`}
        >
          <BsBookmarkStarFill />
          About Our Application
        </h3>
        <div className="text-[36px] font-semibold capitalize">About us</div>
        <p className="leading-6 mt-2 text-lg">
          Collaboratively simplify user friendly networks after principle
          centered coordinate effective methods of empowerment distributed niche
          markets pursue market positioning web-readiness after resource sucking
          applications.
        </p>
        <p className="leading-6 mt-2 text-lg">
          Online education, also known as e-learning, is a method of learning
          that takes place over the internet. It offers individuals the
          opportunity to acquire knowledge, skills.
        </p>
        <div className="flex items-center gap-4 my-4 text-lg">
          <img src="/img/about1.png" alt="class" className="w-[180] h-[120px] object-cover rounded-md"/>
          <ul>
            <li className="flex items-center gap-2">
              <AiFillCheckCircle className="text-blue-400" />
              <span className="font-medium ">
                Kết nối với 4,000+ khóa học của chúng tôi
              </span>
            </li>
            <li className="flex items-center gap-2">
              <AiFillCheckCircle className="text-blue-400" />
              <span className="font-medium ">Chủ đề phổ biến hiện nay</span>
            </li>
            <li className="flex items-center gap-2">
              <AiFillCheckCircle className="text-blue-400" />
              <span className="font-medium ">
                Tìm người hướng dẫn phù hợp với bạn
              </span>
            </li>
          </ul>
        </div>
        <Link to={"/aboutus"}>
          <button className="btn flex items-center justify-center gap-2">
            CHI TIẾT <AiOutlineArrowRight size={20} />
          </button>
        </Link>
      </div>
    </div>
  );
};

export default Welcome;
