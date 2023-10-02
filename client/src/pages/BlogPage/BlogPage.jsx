import React from "react";
import Header from "./../../components/Header";
import Footer from "./../../components/Footer";
import BackgroundSlider from "../../components/BackgroundSlider";
import theme from "./../../theme/index";
import { BiBookBookmark } from "react-icons/bi";
import {
  AiOutlineCalendar,
  AiOutlineClockCircle,
  AiOutlineArrowRight,
} from "react-icons/ai";
import { BsPerson } from "react-icons/bs";
const BlogPage = () => {
  const url =
    "https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png";
  const breadcrumbs = "Blogs";
  return (
    <div>
      <Header></Header>
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />

      <div className="flex justify-center pt-20">
        <div className="flex flex-col gap-10 w-[45%]">
          <div className="border drop-shadow-md rounded-lg p-10 ">
            <div className="center pb-10">
              <img
                src="https://i.pinimg.com/564x/31/c0/45/31c0457faf9a23763b9c8e67ea6c02e8.jpg"
                alt="pic4"
                className="rounded-lg w-[600px] h-[380px] "
              />
            </div>
            <div
              className={`flex text-lg pb-8 text-[${theme.color.mainColor}] `}
            >
              <div className="flex items-center gap-3 pr-4">
                <BsPerson /> by Thu Bui
              </div>
              <div className="flex items-center gap-3 border-l-[2px] px-4">
                <AiOutlineClockCircle /> August 26, 2023
              </div>
              <div className="flex items-center gap-3 border-l-[2px] px-4">
                <BiBookBookmark /> License
              </div>
            </div>
            <div className="text-4xl font-bold pb-8">
              Tips pass bằng lái B1 dễ dàng
            </div>
            <div className="font-light text-lg pb-8">
              Uniquely pursue emerging experiences before liemerging content.
              Efficiently underwhelm customer directed total linkage after B2C
              synergy. Dynamically simplify superior human capital whereas
              efficient infrastructures generate business web-readiness after
              wireless outsourcing. A platform dedicated to exploring the
              transformative power of education. We believe that education is
              not{" "}
            </div>
            <div
              className={`flex items-center gap-2 font-semibold text-md text-[${theme.color.mainColor}] hover:underline-offset-4`}
            >
              READ MORE <AiOutlineArrowRight />
            </div>
          </div>

          <div className="border drop-shadow-md rounded-lg p-10 ">
            <div className="center pb-10">
              <img
                src="https://i.pinimg.com/474x/ae/b3/46/aeb3462d44f97dc71b9d30af919f4f0b.jpg"
                alt="pic5"
                className="rounded-lg w-[600px] h-[380px] "
              />
            </div>
            <div
              className={`flex text-lg pb-8 text-[${theme.color.mainColor}] `}
            >
              <div className="flex items-center gap-3 pr-4">
                <BsPerson /> by Thanh Doan
              </div>
              <div className="flex items-center gap-3 border-l-[2px] px-4">
                <AiOutlineClockCircle /> November 10, 2023
              </div>
              <div className="flex items-center gap-3 border-l-[2px] px-4">
                <BiBookBookmark /> Theory
              </div>
            </div>
            <div className="text-4xl font-bold pb-8">
            Đề thi lý thuyết của loại bằng lái
            </div>
            <div className="font-light text-lg pb-8">
              Uniquely pursue emerging experiences before liemerging content.
              Efficiently underwhelm customer directed total linkage after B2C
              synergy. Dynamically simplify superior human capital whereas
              efficient infrastructures generate business web-readiness after
              wireless outsourcing. A platform dedicated to exploring the
              transformative power of education. We believe that education is
              not{" "}
            </div>
            <div
              className={`flex items-center gap-2 font-semibold text-md text-[${theme.color.mainColor}] hover:underline-offset-4`}
            >
              READ MORE <AiOutlineArrowRight />
            </div>
          </div>

          <div className="border drop-shadow-md rounded-lg p-10 mb-10 ">
            <div className="center pb-10">
              <img
                src="https://i.pinimg.com/474x/f0/3b/d3/f03bd36eeef9820661dbd0ad3efb2f31.jpg"
                alt="pic6"
                className="rounded-lg w-[600px] h-[380px] "
              />
            </div>
            <div
              className={`flex text-lg pb-8 text-[${theme.color.mainColor}] `}
            >
              <div className="flex items-center gap-3 pr-4">
                <BsPerson /> by Phuoc Le
              </div>
              <div className="flex items-center gap-3 border-l-[2px] px-4">
                <AiOutlineClockCircle /> July 1, 2023
              </div>
              <div className="flex items-center gap-3 border-l-[2px] px-4">
                <BiBookBookmark /> License
              </div>
            </div>
            <div className="text-4xl font-bold pb-8">
            Những thay đổi mới về bằng lái
            </div>
            <div className="font-light text-lg pb-8">
              Uniquely pursue emerging experiences before liemerging content.
              Efficiently underwhelm customer directed total linkage after B2C
              synergy. Dynamically simplify superior human capital whereas
              efficient infrastructures generate business web-readiness after
              wireless outsourcing. A platform dedicated to exploring the
              transformative power of education. We believe that education is
              not{" "}
            </div>
            <div
              className={`flex items-center gap-2 font-semibold text-md text-[${theme.color.mainColor}] hover:underline-offset-4`}
            >
              READ MORE <AiOutlineArrowRight />
            </div>
          </div>
        </div>

        <div className="w-[38%] ">
          <div className="border drop-shadow-md rounded-lg p-10 ml-7 mb-14">
            <div className="border-b-[4px] text-3xl font-bold pb-2 ">
              Search
            </div>
            <div className="flex justify-end rounded-lg bg-slate-100 mt-5">
              <button
                className={`rounded-r-lg bg-[${theme.color.mainColor}] text-white font-light text-lg w-[25%] p-5 hover:bg-blue-900`}
              >
                Search
              </button>
            </div>
          </div>

          <div className="border drop-shadow-md rounded-lg p-10 ml-7 mb-14">
            <div className="border-b-[4px] text-3xl font-bold pb-2 ">
              Categories
            </div>

            <div className="flex justify-between text-lg text-blue-500 hover:text-blue-900 pt-5">
              <div>
                <div className="flex center gap-2 ">
                  <BiBookBookmark /> Theory
                </div>
              </div>
              <div>3</div>
            </div>

            <div className="flex justify-between text-lg text-blue-500 hover:text-blue-900 pt-5">
              <div>
                <div className="flex center gap-2 ">
                  <BiBookBookmark /> GuildLine
                </div>
              </div>
              <div>10</div>
            </div>

            <div className="flex justify-between text-lg text-blue-500 hover:text-blue-900 pt-5">
              <div>
                <div className="flex center gap-2 ">
                  <BiBookBookmark /> License
                </div>
              </div>
              <div>7</div>
            </div>

            <div className="flex justify-between text-lg text-blue-500 hover:text-blue-900 pt-5">
              <div>
                <div className="flex center gap-2 ">
                  <BiBookBookmark /> Register
                </div>
              </div>
              <div>21</div>
            </div>

            <div className="flex justify-between text-lg text-blue-500 hover:text-blue-900 pt-5">
              <div>
                <div className="flex center gap-2 ">
                  <BiBookBookmark /> New Changes
                </div>
              </div>
              <div>8</div>
            </div>
          </div>

          <div className="border drop-shadow-md rounded-lg p-10 ml-7 mb-14">
            <div className="border-b-[4px] text-3xl font-bold pb-2 ">
              Recent Posts
            </div>
            <div className="flex pt-10 gap-5">
              <div>
                <img
                  src="https://i.pinimg.com/474x/f5/1d/f8/f51df8c6a4f7f8657acabcf8ba5275fe.jpg"
                  alt="Pic1"
                  className="rounded-lg w-[100px] h-[100px]"
                />
              </div>
              <div className="flex flex-col gap-2">
                <div className="text-2xl font-bold">
                  Tips pass bằng lái B1 dễ dàng
                </div>
                <div className="flex items-center text-lg font-light gap-2 text-blue-500 hover:text-blue-900 ">
                  <AiOutlineCalendar />
                  26 Aug 2023
                </div>
              </div>
            </div>

            <div className="flex pt-10 gap-5">
              <div>
                <img
                  src="https://i.pinimg.com/474x/fd/2c/85/fd2c8579b7863641b21ec563f097694b.jpg"
                  alt="Pic2"
                  className="rounded-lg w-[100px] h-[100px]"
                />
              </div>
              <div className="flex flex-col gap-2">
                <div className="text-2xl font-bold">
                  Đề thi lý thuyết của loại bằng lái
                </div>
                <div className="flex items-center text-lg font-light gap-2 text-blue-500 hover:text-blue-900 ">
                  <AiOutlineCalendar />
                  10 Nov 2023
                </div>
              </div>
            </div>

            <div className="flex pt-10 gap-5">
              <div>
                <img
                  src="https://i.pinimg.com/474x/a1/31/f6/a131f61280b7a899fd2e24541d1b0e80.jpg"
                  alt="Pic3"
                  className="rounded-lg w-[100px] h-[100px]"
                />
              </div>
              <div className="flex flex-col gap-2">
                <div className="text-2xl font-bold">
                  Những thay đổi mới về bằng lái
                </div>
                <div className="flex items-center text-lg font-light gap-2 text-blue-500 hover:text-blue-900 ">
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

      <Footer></Footer>
    </div>
  );
};

export default BlogPage;
