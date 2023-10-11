import React from "react";
import { AiFillStar } from "react-icons/ai";
import { BiBold, BiCircle } from "react-icons/bi";
import { Link } from "react-router-dom";
import "swiper/css";
import "swiper/css/navigation";
import "swiper/css/pagination";
import "swiper/css/scrollbar";
import { Autoplay, Pagination } from "swiper/modules";
import { Swiper, SwiperSlide } from "swiper/react";

const listMentor = [
  {
    id: 1,
    src: "https://scontent.fsgn5-6.fna.fbcdn.net/v/t1.6435-9/123116045_772985256617271_6289105269278104549_n.jpg?_nc_cat=108&ccb=1-7&_nc_sid=ad2b24&_nc_ohc=laebF5sIFUUAX8VFESQ&_nc_ht=scontent.fsgn5-6.fna&oh=00_AfBOKBYLyQ6D7SQRwsFXimrfuUuKrPYLyfKst_XNlTvmIw&oe=65353413",
    author: "Quang Huy",
    license: "Hạng bằng lái: B1",
    rate: "5",
  },
  {
    id: 2,
    src: "https://scontent.fsgn2-3.fna.fbcdn.net/v/t39.30808-6/326718942_3475973552726762_6277150844361274430_n.jpg?_nc_cat=107&ccb=1-7&_nc_sid=a2f6c7&_nc_ohc=Qwrn79_gzkQAX-vfpDx&_nc_ht=scontent.fsgn2-3.fna&oh=00_AfDJ2wJIJEh7ouiidTlGa17nqLA4GeHeyyJzTum3K_gN7g&oe=6522852C",
    author: "Thanh Doan",
    license: "Hạng bằng lái: B2",
    rate: "4",
  },
  {
    id: 3,
    src: "https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/team_1_4.png",
    author: "Xuân Phước",
    license: "Hạng bằng lái: C",
    rate: "4",
  },
  {
    id: 4,
    src: "https://scontent.fsgn2-9.fna.fbcdn.net/v/t39.30808-6/350126583_273968988360444_515949382367821556_n.jpg?stp=cp6_dst-jpg_s851x315&_nc_cat=105&ccb=1-7&_nc_sid=0df3a7&_nc_ohc=7b8g2aPSWp8AX-_0Ys2&_nc_ht=scontent.fsgn2-9.fna&oh=00_AfAq-AOW5jIXwBu713uTKup1eR7zfgrA8QazfUNX9SXUaA&oe=652299DA",
    author: "Thanh Thư",
    license: "Hạng bằng lái: C",
    rate: "5",
  },
];

const RatedMentor = () => {
  return (
    <div className="bg-gray-800 py-20">
      <div className="pl-20 ">
        <div className="text-4xl font-bold text-white">
          NGƯỜI HƯỚNG DẪN
        </div>
      </div>

      <div className="px-32 py-10">
        <Swiper
          modules={[Pagination, Autoplay]}
          loop={true}
          slidesPerView={4}
          spaceBetween={50}
          autoplay={{
            delay: 2000,
            disableOnInteraction: false,
          }}
          pagination={{ clickable: true }}
        >
          {listMentor.map((mentorItem, index) => (
            <SwiperSlide key={mentorItem.author}>
              <div className="border-t-4 border-blue-400 bg-white">
                <div className="mentor flex relative">
                  <img
                    src={mentorItem.src}
                    alt="blog"
                    className="imgMentor w-full h-[300px] object-cover"
                  />
                  <Link to={'/intructor/detail'}>
                    <div className="moreDetail flex absolute top-[50%] left-[35%] gap-1 text-blue-300 font-semibold text-3xl hover:text-blue-500">
                      o o o
                    </div>
                  </Link>
                </div>
                <div className="flex flex-col center py-5">
                  <div className="font-bold text-xl">{mentorItem.author}</div>
                  <div className="flex gap-1 items-center ">
                    <div className="text-lg text-gray-600">Đánh giá: {mentorItem.rate} </div>
                    
                  <AiFillStar size={22} color="#ffe101"/>
                  </div>
                  {/* <div className="font-regular text-md text-gray-500">
                    {mentorItem.license}
                  </div> */}
                </div>
              </div>
            </SwiperSlide>
          ))}
        </Swiper>
      </div>
    </div>
  );
};

export default RatedMentor;
