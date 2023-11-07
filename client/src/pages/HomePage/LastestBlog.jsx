import React from "react";
import { AiOutlineArrowRight } from "react-icons/ai";
import { BsPerson } from "react-icons/bs";
import { MdDateRange } from "react-icons/md";
import { Link } from "react-router-dom";
import "swiper/css";
import "swiper/css/navigation";
import "swiper/css/pagination";
import "swiper/css/scrollbar";
import { Autoplay, Pagination } from "swiper/modules";
import { Swiper, SwiperSlide } from "swiper/react";

const listBlog = [
  {
    id: 1,
    date: "10 tháng 9, 2023",
    src: "/img/blog1.png",
    author: "Quang Huy",
    title: "Những điều cần biết khi thi bằng lái B1",
  },
  {
    id: 2,
    date: "21 tháng 9, 2023",
    src: "/img/blog2.png",
    author: "Thanh Doan",
    title: "Đề thi lý thuyết của loại bằng lái nào khó nhất",
  },
  {
    id: 3,
    date: "25 tháng 9, 2023",
    src: "/img/blog3.png",
    author: "Xuân Phước",
    title: "Những điều cần biết khi thi bằng lái B1",
  },
  {
    id: 4,
    date: "1 tháng 10, 2023",
    src: "/img/blog4.png",
    author: "Thanh Thư",
    title: "Tips Pass Bằng Lái B1 Dễ Dàng",
  },
];

const LastestBlog = () => {
  return (
    <div className="m-20">
      <div className="my-8">
        <div className="flex items-center justify-between">
          <div className="text-4xl font-bold">BÀI ĐĂNG MỚI NHẤT</div>
          <Link to={"/blog"}>
            <button className="btn flex items-center justify-center gap-2">
              Tất cả bài đăng <AiOutlineArrowRight size={20} />
            </button>
          </Link>
        </div>
      </div>

      <div className="px-8">
        <Swiper
          modules={[Pagination, Autoplay]}
          loop={true}
          slidesPerView={3}
          spaceBetween={40}
          autoplay={{
            delay: 2000,
            disableOnInteraction: false,
          }}
          pagination={{ clickable: true }}
        >
          {listBlog.map((blogItem, index) => (
            <SwiperSlide key={blogItem.author}>
              <div className="border border-2 rounded-md p-8 hover:shadow-2xl">
                <div className="pb-4">
                  <img
                    src={blogItem.src}
                    alt="blog"
                    className="w-full h-[300px] object-cover rounded"
                  />
                </div>
                <div>
                  <div className="flex justify-between text-blue-300 py-2">
                    <div className="flex items-center gap-2 font-medium ">
                      <MdDateRange size={20} />
                      <span className="text-black opacity-70">
                        {blogItem.date}
                      </span>
                    </div>
                    <div className="flex items-center gap-2 font-medium">
                      <BsPerson size={20} />
                      <span className="text-black opacity-70">
                        {blogItem.author}
                      </span>
                    </div>
                  </div>
                  <h3 className="text-[28px] font-bold py-2">
                    {blogItem.title}
                  </h3>
                  <Link to={"/blog/detail"}>
                    <button className="btn flex items-center justify-center gap-2">
                      CHI TIẾT <AiOutlineArrowRight size={20} />
                    </button>
                  </Link>
                </div>
              </div>
            </SwiperSlide>
          ))}
        </Swiper>
      </div>
    </div>
  );
};

export default LastestBlog;
