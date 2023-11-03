import React from "react";
import Header from "./../../components/Header";
import Footer from "./../../components/Footer";
import BackgroundSlider from "../../components/BackgroundSlider";
import theme from "./../../theme/index";
import { BiBookBookmark, BiSearch } from "react-icons/bi";
import {
  AiOutlineCalendar,
  AiOutlineClockCircle,
  AiOutlineArrowRight,
} from "react-icons/ai";
import { BsPerson } from "react-icons/bs";
import { Link } from "react-router-dom";
const BlogPage = () => {
  const url =
    "https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png";
  const breadcrumbs = "Blogs";
  const listBlog = [
    {
      id: "1",
      img: "https://i.pinimg.com/564x/31/c0/45/31c0457faf9a23763b9c8e67ea6c02e8.jpg",
      poster: "Thu Bui",
      time: "August 26, 2023",
      type: "Bằng lái",
      title: "Tips pass bằng lái B1 dễ dàng",
      content:
        "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Enim, nisi, molestiae corrupti deserunt dolorum repellendus hic illo eveniet dolores assumenda esse velit placeat nihil. Esse cum adipisci voluptates similique quam",
      link: "/blog/detail",
    },

    {
      id: "2",
      img: "https://i.pinimg.com/474x/ae/b3/46/aeb3462d44f97dc71b9d30af919f4f0b.jpg",
      poster: "Thanh Doan",
      time: "November 10, 2023",
      type: "Lý thuyết",
      title: "Đề thi lý thuyết của loại bằng lái B1",
      content:
        "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Enim, nisi, molestiae corrupti deserunt dolorum repellendus hic illo eveniet dolores assumenda esse velit placeat nihil. Esse cum adipisci voluptates similique quam",
      link: "/blog/detail",
    },

    {
      id: "3",
      img: "https://i.pinimg.com/474x/f0/3b/d3/f03bd36eeef9820661dbd0ad3efb2f31.jpg",
      poster: "Phuoc Le",
      time: "July 1, 2023",
      type: "Bằng lái",
      title: "Những thay đổi mới về hạng bằng lái A2",
      content:
        "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Enim, nisi, molestiae corrupti deserunt dolorum repellendus hic illo eveniet dolores assumenda esse velit placeat nihil. Esse cum adipisci voluptates similique quam",
      link: "/blog/detail",
    },
  ];
  return (
    <div>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />

      <div className="flex justify-center m-20">
        <div className="flex flex-col gap-10 w-[70%]">
          {listBlog.map((blog, index) => (
            <div className="border drop-shadow-md rounded-lg p-20 ">
              <div className="center pb-10">
                <img
                  src={blog.img}
                  alt="pic4"
                  className="rounded-lg w-[800px] h-[380px] "
                />
              </div>
              <div
                className={`flex text-lg pb-8 text-[${theme.color.mainColor}] `}
              >
                <div className="flex items-center gap-3 pr-4">
                  <BsPerson size={24} /> by {blog.poster}
                </div>
                <div className="flex items-center gap-3 border-l-[2px] px-4">
                  <AiOutlineClockCircle size={24} /> {blog.time}
                </div>
                <div className="flex items-center gap-3 border-l-[2px] px-4">
                  <BiBookBookmark size={24} /> {blog.type}
                </div>
              </div>
              <div className="text-4xl font-bold pb-8">{blog.title}</div>
              <div className="font-light text-lg pb-8">{blog.content}</div>
              <Link to={blog.link}>
                <button className="btn flex items-center gap-2">
                  READ MORE <AiOutlineArrowRight />
                </button>
              </Link>
            </div>
          ))}
        </div>

        <div className="w-[30%] ml-10">
          <div className="border drop-shadow-md rounded-lg p-10 mb-20">
            <div className="border-b-[4px] text-2xl font-bold pb-2 ">
              Tìm kiếm
            </div>
            <form className="flex mt-5">
              <input
                placeholder=" Nhập..."
                className="rounded-l-lg bg-slate-100 outline-none pl-5"
              />
              <button
                className={`center rounded-r-lg bg-[${theme.color.mainColor}] text-white font-bold w-[25%] p-3 hover:bg-blue-900`}
              >
                <BiSearch size={24} />
              </button>
            </form>
          </div>

          <div className="border drop-shadow-md rounded-lg p-10 mb-20">
            <div className="border-b-[4px] text-2xl font-bold pb-2 ">
              Bài đăng gần đây
            </div>
            {listBlog.map((lilBlog, index) => (
              <div className="flex pt-10 gap-3">
                <div className="w-[30%]">
                  <img
                    src={lilBlog.img}
                    alt="Pic1"
                    className="rounded-lg w-[80px] h-[80px] object-cover"
                  />
                </div>
                <div className="flex flex-col w-[70%] gap-2">
                  <Link to={lilBlog.link}>
                    <div className="text-lg font-bold hover:text-blue-500 overflow-hidden whitespace-nowrap overflow-ellipsis max-w-xs">
                      {lilBlog.title}
                    </div>
                  </Link>
                  <div className="flex items-center text-md font-light gap-2 text-blue-500 hover:text-blue-900 ">
                    <AiOutlineCalendar />
                    {lilBlog.time}
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>

      <Footer />
    </div>
  );
};

export default BlogPage;
