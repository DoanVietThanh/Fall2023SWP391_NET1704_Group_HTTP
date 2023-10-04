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
import { login } from "./../../features/auth/authSlice";
const BlogPage = () => {
  const url =
    "https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png";
  const breadcrumbs = "Blogs";
  return (
    <div>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />

      <div className="flex justify-center m-20">
        <div className="flex flex-col gap-10 w-[70%]">
          <div className="border drop-shadow-md rounded-lg p-20 ">
            <div className="center pb-10">
              <img
                src="https://i.pinimg.com/564x/31/c0/45/31c0457faf9a23763b9c8e67ea6c02e8.jpg"
                alt="pic4"
                className="rounded-lg w-[800px] h-[380px] "
              />
            </div>
            <div
              className={`flex text-lg pb-8 text-[${theme.color.mainColor}] `}
            >
              <div className="flex items-center gap-3 pr-4">
                <BsPerson size={24} /> by Thu Bui
              </div>
              <div className="flex items-center gap-3 border-l-[2px] px-4">
                <AiOutlineClockCircle size={24} /> August 26, 2023
              </div>
              <div className="flex items-center gap-3 border-l-[2px] px-4">
                <BiBookBookmark size={24} /> License
              </div>
            </div>
            <div className="text-4xl font-bold pb-8">
              Tips pass bằng lái B1 dễ dàng
            </div>
            <div className="font-light text-lg pb-8">
              Lorem ipsum dolor, sit amet consectetur adipisicing elit. Minima
              repellendus impedit tenetur molestiae quia modi pariatur, suscipit
              ex optio eius, eos quis. Ipsam odio qui vel nam dolore culpa ullam
              nobis nemo rem, exercitationem doloremque delectus similique nisi
              quasi aliquam atque voluptate. Nulla, eos itaque. Quidem
              voluptatem beatae maxime necessitatibus nulla cupiditate porro
              ullam temporibus quaerat enim, consequatur explicabo cumque
              obcaecati vitae accusantium et earum. Praesentium, eveniet? Eaque
              eligendi distinctio dolores sint! Maxime similique quam delectus
              sed beatae, itaque aperiam.
            </div>
            <Link to={"/blog/detail"}>
              <div
                className={`flex items-center gap-2 font-semibold text-md text-[${theme.color.mainColor}] hover:underline`}
              >
                READ MORE <AiOutlineArrowRight />
              </div>
            </Link>
          </div>

          <div className="border drop-shadow-md rounded-lg p-20 ">
            <div className="center pb-10">
              <img
                src="https://i.pinimg.com/474x/ae/b3/46/aeb3462d44f97dc71b9d30af919f4f0b.jpg"
                alt="pic5"
                className="rounded-lg w-[800px] h-[380px] "
              />
            </div>
            <div
              className={`flex text-lg pb-8 text-[${theme.color.mainColor}] `}
            >
              <div className="flex items-center gap-3 pr-4">
                <BsPerson size={24} /> by Thanh Doan
              </div>
              <div className="flex items-center gap-3 border-l-[2px] px-4">
                <AiOutlineClockCircle size={24} /> November 10, 2023
              </div>
              <div className="flex items-center gap-3 border-l-[2px] px-4">
                <BiBookBookmark size={24} /> Theory
              </div>
            </div>
            <div className="text-4xl font-bold pb-8">
              Đề thi lý thuyết của loại bằng lái
            </div>
            <div className="font-light text-lg pb-8 ">
              Lorem ipsum, dolor sit amet consectetur adipisicing elit. Illum ut
              est voluptatum suscipit impedit eius quia nisi odio magni tenetur
              laudantium, velit officia ullam delectus, ab vero ipsa
              dignissimos, rem voluptatem. Eligendi tempora facere sequi
              sapiente. Recusandae veritatis ex dolorem, quidem distinctio
              alias? Repellendus dignissimos totam suscipit, nisi dicta at, ipsa
              laborum atque possimus minus ipsum! Quos cumque voluptatum eum,
              explicabo rem praesentium enim earum culpa eos! Soluta dicta
              commodi cum harum temporibus, voluptate aspernatur ratione
              dignissimos quidem architecto nemo?
            </div>
            <Link to={"/blog/detail"}>
              <div
                className={`flex items-center gap-2 font-semibold text-md text-[${theme.color.mainColor}] hover:underline`}
              >
                READ MORE <AiOutlineArrowRight />
              </div>
            </Link>
          </div>

          <div className="border drop-shadow-md rounded-lg p-20 mb-10 ">
            <div className="center pb-10">
              <img
                src="https://i.pinimg.com/474x/f0/3b/d3/f03bd36eeef9820661dbd0ad3efb2f31.jpg"
                alt="pic6"
                className="rounded-lg w-[800px] h-[380px] "
              />
            </div>
            <div
              className={`flex text-lg pb-8 text-[${theme.color.mainColor}] `}
            >
              <div className="flex items-center gap-3 pr-4">
                <BsPerson size={24} /> by Phuoc Le
              </div>
              <div className="flex items-center gap-3 border-l-[2px] px-4">
                <AiOutlineClockCircle size={24} /> July 1, 2023
              </div>
              <div className="flex items-center gap-3 border-l-[2px] px-4">
                <BiBookBookmark size={24} /> License
              </div>
            </div>
            <div className="text-4xl font-bold pb-8">
              Những thay đổi mới về bằng lái
            </div>
            <div className="font-light text-lg pb-8">
              Lorem ipsum dolor sit amet consectetur adipisicing elit. Itaque
              quae dolores similique cumque reprehenderit ipsa qui suscipit
              fuga? Tempora, saepe. Omnis cupiditate dolores sint nesciunt. Sed
              assumenda vitae deleniti numquam perferendis voluptatum minus
              minima facilis, nostrum officia consectetur accusamus repellat
              eligendi, perspiciatis temporibus quasi! Magni ut dolorem
              blanditiis atque molestias. Corporis quae facilis sint, excepturi
              distinctio illum ab vero placeat dolorem, ullam maiores sit quam
              quisquam dolor magni atque nulla. Sequi excepturi doloribus nobis
              commodi ea cum sint quia qui?
            </div>
            <Link to={"/blog/detail"}>
              <div
                className={`flex items-center gap-2 font-semibold text-md text-[${theme.color.mainColor}] hover:underline`}
              >
                READ MORE <AiOutlineArrowRight />
              </div>
            </Link>
          </div>
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
              Categories
            </div>

            <div className="flex justify-between text-lg text-blue-500 hover:text-blue-900 pt-5">
              <div>
                <div className="flex center gap-2 cursor-pointer">
                  <BiBookBookmark /> Theory
                </div>
              </div>
              <div>3</div>
            </div>

            <div className="flex justify-between text-lg text-blue-500 hover:text-blue-900 pt-5">
              <div>
                <div className="flex center gap-2 cursor-pointer">
                  <BiBookBookmark /> GuildLine
                </div>
              </div>
              <div>10</div>
            </div>

            <div className="flex justify-between text-lg text-blue-500 hover:text-blue-900 pt-5">
              <div>
                <div className="flex center gap-2 cursor-pointer">
                  <BiBookBookmark /> License
                </div>
              </div>
              <div>7</div>
            </div>

            <div className="flex justify-between text-lg text-blue-500 hover:text-blue-900 pt-5">
              <div>
                <div className="flex center gap-2 cursor-pointer">
                  <BiBookBookmark /> Register
                </div>
              </div>
              <div>21</div>
            </div>

            <div className="flex justify-between text-lg text-blue-500 hover:text-blue-900 pt-5">
              <div>
                <div className="flex center gap-2 cursor-pointer">
                  <BiBookBookmark /> New Changes
                </div>
              </div>
              <div>8</div>
            </div>
          </div>

          <div className="border drop-shadow-md rounded-lg p-10 mb-20">
            <div className="border-b-[4px] text-2xl font-bold pb-2 ">
              Bài đăng gần đây
            </div>
            <div className="flex pt-10 gap-3">
              <div>
                <img
                  src="https://i.pinimg.com/474x/f5/1d/f8/f51df8c6a4f7f8657acabcf8ba5275fe.jpg"
                  alt="Pic1"
                  className="rounded-lg w-[80px] h-[80px]"
                />
              </div>
              <div className="flex flex-col gap-2">
                <Link to={"/blog/detail"}>
                  <div className="text-lg font-bold hover:text-blue-500">
                    Tips pass bằng lái B1 dễ dàng
                  </div>
                </Link>
                <div className="flex items-center text-md font-light gap-2 text-blue-500 hover:text-blue-900 ">
                  <AiOutlineCalendar />
                  26 Aug 2023
                </div>
              </div>
            </div>

            <div className="flex pt-10 gap-3">
              <div>
                <img
                  src="https://i.pinimg.com/474x/fd/2c/85/fd2c8579b7863641b21ec563f097694b.jpg"
                  alt="Pic2"
                  className="rounded-lg w-[80px] h-[80px]"
                />
              </div>
              <div className="flex flex-col gap-2">
                <Link to={"/blog/detail"}>
                  <div className="text-lg font-bold hover:text-blue-500">
                    Đề thi lý thuyết của loại bằng lái
                  </div>
                </Link>
                <div className="flex items-center text-md font-light gap-2 text-blue-500 hover:text-blue-900 ">
                  <AiOutlineCalendar />
                  10 Nov 2023
                </div>
              </div>
            </div>

            <div className="flex pt-10 gap-3">
              <div>
                <img
                  src="https://i.pinimg.com/474x/a1/31/f6/a131f61280b7a899fd2e24541d1b0e80.jpg"
                  alt="Pic3"
                  className="rounded-lg w-[80px] h-[80px]"
                />
              </div>
              <div className="flex flex-col gap-2">
                <Link to={"/blog/detail"}>
                  <div className="text-lg font-bold hover:text-blue-500">
                    Những thay đổi mới về bằng lái
                  </div>
                </Link>
                <div className="flex items-center text-md font-light gap-2 text-blue-500 hover:text-blue-900 ">
                  <AiOutlineCalendar />
                  01 Jul 2023
                </div>
              </div>
            </div>
          </div>
        </div>

        <div></div>
        <div></div>
        <div></div>
      </div>

      <Footer />
    </div>
  );
};

export default BlogPage;
