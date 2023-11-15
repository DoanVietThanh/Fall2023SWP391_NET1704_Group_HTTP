import React from "react";
import { AiOutlineArrowRight, AiFillCheckCircle } from "react-icons/ai";
import theme from "../../theme";
import { BsBookmarkStarFill } from "react-icons/bs";
import { Link } from "react-router-dom";
const Welcome = () => {
  return (
    <div className="m-20 flex">
      <div className="flex-1 overflow-hidden rounded-md">
        <div className="relative w-[90%] h-[400px]">
          <div className="absolute rounded-lg w-full ">
            <img
              src="/img/about2.png"
              alt="About"
              className="w-full object-cover rounded-md"
            />
          </div>

          <div className="absolute right-0">
            <img src="/img/about4.png" alt="About" className="w-[150px] h-[200px] rounded-md" />
          </div>
        </div>
      </div>

      <div className="flex-1">
        <h3
          className={`text-[16px] text-[${theme.color.mainColor}] flex items-center gap-2 font-bold`}
        >
          <BsBookmarkStarFill />
          Thông tin ứng dụng
        </h3>
        <div className="text-2xl font-semibold capitalize mt-4">Về chúng tôi</div>
        <p className="leading-6 mt-2 text-lg">
          Tự hào giới thiệu trang web của chúng tôi, nơi bạn có thể dễ dàng học
          và thi bằng lái xe. Chúng tôi cung cấp tài liệu đa dạng về luật giao
          thông, quy tắc an toàn khi lái xe, và kiến thức cho kỳ thi. Dành cho
          cả người mới học lái và người muốn cập nhật kiến thức.
        </p>
        <p className="leading-6 mt-2 text-lg">
          Khám phá ngay và sở hữu bằng lái xe một cách tự tin.
        </p>
        <div className="flex items-center gap-4 my-4 text-lg">
          <img
            src="/img/about1.png"
            alt="class"
            className="w-[180] h-[120px] object-cover rounded-md"
          />
          <ul>
            <li className="flex items-center gap-2">
              <AiFillCheckCircle className="text-blue-400" />
              <span className="font-medium ">
                Kết nối khóa học của chúng tôi
              </span>
            </li>
            <li className="flex items-center gap-2">
              <AiFillCheckCircle className="text-blue-400" />
              <span className="font-medium ">Làm bài kiểm tra lí thuyết miễn phí</span>
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
