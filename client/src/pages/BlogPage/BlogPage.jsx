import axios from "axios";
import React, { useEffect, useState } from "react";
import {
  AiOutlineCalendar
} from "react-icons/ai";
import { BiSearch } from "react-icons/bi";
import { Link } from "react-router-dom";
import BackgroundSlider from "../../components/BackgroundSlider";
import Footer from "./../../components/Footer";
import Header from "./../../components/Header";
import BlogList from './component/BlogList';
import { toastError } from "../../components/Toastify";
import theme from "../../theme";

const BlogPage = () => {
  const url = "/img/backgroundSlide.png";
  const breadcrumbs = "Bài đăng";
  const urlService = process.env.REACT_APP_SERVER_API;
  const [searchInput, setSearchInput] = useState('');
  // const listBlog = [
  //   {
  //     id: "1",
  //     img: "https://i.pinimg.com/564x/31/c0/45/31c0457faf9a23763b9c8e67ea6c02e8.jpg",
  //     poster: "Thu Bui",
  //     time: "August 26, 2023",
  //     type: "Bằng lái",
  //     title: "Tips pass bằng lái B1 dễ dàng",
  //     content:
  //       "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Enim, nisi, molestiae corrupti deserunt dolorum repellendus hic illo eveniet dolores assumenda esse velit placeat nihil. Esse cum adipisci voluptates similique quam",
  //     link: "/blog/detail",
  //   },
  // ];
  const [listBlog, setListBlog] = useState([]);

  useEffect(() => {
    async function getListBlog() {
      await axios
        .get(`${urlService}/blog`)
        .then((res) => {
          console.log("res: ", res);
          setListBlog(res.data?.data);
        })
        .catch((error) => {
          console.log("error: ", error);
          toastError(error?.response?.data?.message);
        });
    }
    getListBlog();
  }, []);

  console.log("listBlog: ", listBlog);

  return (
    <div>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      <div className="flex justify-center m-20">
        <div className="w-[70%]">
          <BlogList/>
          {/* {listBlog?.map((blog, index) => (
            <div className="border drop-shadow-md rounded-lg p-20 ">
              <div className="center pb-10">
                <img
                  src={blog?.img}
                  alt="pic4"
                  className="rounded-lg w-[800px] h-[380px] "
                />
              </div>
              <div
                className={`flex text-lg pb-8 text-[${theme.color.mainColor}] `}
              >
                <div className="flex items-center gap-3 pr-4">
                  <BsPerson size={24} /> by {blog?.poster} 
                </div>
                <div className="flex items-center gap-3 border-l-[2px] px-4">
                  <AiOutlineClockCircle size={24} /> {dayjs(blog?.createDate).format("DD/MM/YYYY")}
                </div>
                <div className="flex items-center gap-3 border-l-[2px] px-4">
                  <BiBookBookmark size={24} /> {blog?.tags?.map((tag, index) => (
                    <span key={tag.tagId}>{tag.tagName}</span>
                  ))}
                </div>
              </div>
              <div className="text-4xl font-bold pb-8">{blog?.title}</div>
              <div className="font-light text-lg pb-8">{blog?.content}</div>
              <Link to={`/blog/${blog?.blogId}`}>
                <button className="btn flex items-center gap-2">
                  READ MORE <AiOutlineArrowRight />
                </button>
              </Link>
            </div>
          ))}  */}
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
                value={searchInput}
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
