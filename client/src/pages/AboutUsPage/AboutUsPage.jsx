import React from "react";
import BackgroundSlider from "./../../components/BackgroundSlider";
import Footer from "../../components/Footer";
import Header from "../../components/Header";
import { AiFillCar, AiFillCheckCircle, AiFillStar } from "react-icons/ai";
import { PiSteeringWheel } from "react-icons/pi";
import {
  BsFillPersonCheckFill,
  BsPostcard,
  BsSpeedometer2,
} from "react-icons/bs";
import { FaRegNewspaper } from "react-icons/fa6";
import { MdOutlineTraffic } from "react-icons/md";
import { BiSolidRightArrow } from "react-icons/bi";
const AboutUsPage = () => {
  const url =
    "https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png";
  const breadcrumbs = "About Us";
  return (
    <div>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      <div className="flex flex-col">
        <div className="flex flex-col mx-20">
          <div className="flex justify-between items-center ">
            <div className="flex flex-col gap-5 w-[50%]">
              <div className="text-xl font-md text-gray-400 italic ">
                Who are we
              </div>
              <div className="text-4xl font-bold w-[50%] border-b-2 border-gray-400 pb-3">
                Why Choose Us
              </div>

              <div className="leading-8 text-md font-md text-gray-500">
                Lorem ipsum dolor sit amet consectetur adipisicing elit.
                Incidunt aperiam unde beatae explicabo deleniti perferendis eum
                voluptatum a earum, dolore quaerat cumque dolor atque sint esse,
                perspiciatis alias! Magni a dolorum sapiente. Cum quos
                voluptatibus dolorem. Omnis aspernatur repellendus quo.
              </div>
              <div className="list flex">
                <div className="w-[50%]">
                  <ul>
                    <li>
                      <AiFillCheckCircle size={24} className="text-blue-500" />
                      <span className="font-medium ">Tay lái an toàn</span>
                    </li>
                    <li>
                      <AiFillCheckCircle size={24} className="text-blue-500" />
                      <span className="font-medium ">Tuân thủ quy định</span>
                    </li>
                    <li>
                      <AiFillCheckCircle size={24} className="text-blue-500" />
                      <span className="font-medium ">
                        Phương tiện tiêu chuẩn
                      </span>
                    </li>
                  </ul>
                </div>
                <div className="w-[50%]">
                  <ul>
                    <li>
                      <AiFillCheckCircle size={24} className="text-blue-500" />
                      <span className="font-medium ">Luật giao thông</span>
                    </li>
                    <li>
                      <AiFillCheckCircle size={24} className="text-blue-500" />
                      <span className="font-medium ">
                        Người hướng dẫn kinh nghiệm
                      </span>
                    </li>
                    <li>
                      <AiFillCheckCircle size={24} className="text-blue-500" />
                      <span className="font-medium ">Khóa học tốt nhất</span>
                    </li>
                  </ul>
                </div>
              </div>
            </div>

            <img src="/img/aboutus.png" className="w-[50%]" alt="About Us" />
          </div>

          <div className="flex">
            <div className="flex gap-3 w-[25%] p-8 items-center center bg-blue-600/100">
              <PiSteeringWheel size={36} color="white" />
              <div className="center uppercase text-xl text-white font-semibold  ">
                Comfort vehicle
              </div>
            </div>
            <div className="flex gap-3 w-[25%] p-8 items-center center bg-blue-600/90">
              <BsPostcard size={36} color="white" />
              <div className="center uppercase text-xl text-white font-semibold  ">
                License tranining
              </div>
            </div>
            <div className="flex gap-3 w-[25%] p-8 items-center center bg-blue-600/80">
              <BsSpeedometer2 size={36} color="white" />
              <div className="center uppercase text-xl text-white font-semibold  ">
                Flexible schedule
              </div>
            </div>
            <div className="flex gap-3 w-[25%] p-8 items-center center bg-blue-600/70">
              <FaRegNewspaper size={36} color="white" />
              <div className="center uppercase text-xl text-white font-semibold  ">
                Driving practice
              </div>
            </div>
          </div>
        </div>

        <div className="flex mt-20 ">
          <img
            src="https://i.pinimg.com/474x/51/58/3c/51583c713f0a17e6c3ffa13e151af50d.jpg"
            alt="driver"
            className="w-[30%]"
          />
          <div className="w-[40%] flex flex-col gap-10 center bg-gray-800 text-white px-10">
            <div className="flex flex-col gap-3">
              <div className="flex gap-5 items-center">
                <div>
                  <MdOutlineTraffic size={44} />
                </div>
                <div className="uppercase text-white text-lg font-semibold border-b-2 border-white pb-2 ">
                  traffic rule
                </div>
              </div>
              <div className="font-light text-md text-gray-300 pl-16">
                Lorem ipsum dolor sit amet consectetur adipisicing elit. Tenetur
                ea, quidem maxime ipsum illo autem laboriosam
              </div>
            </div>

            <div className="flex flex-col gap-3">
              <div className="flex gap-5 items-center">
                <div>
                  <BsFillPersonCheckFill size={44} />
                </div>
                <div className="uppercase text-white text-lg font-semibold border-b-2 border-white pb-2 ">
                  Right mentor
                </div>
              </div>
              <div className="font-light text-md text-gray-300 pl-16">
                Lorem ipsum dolor sit amet consectetur adipisicing elit. Tenetur
                ea, quidem maxime ipsum illo autem laboriosam
              </div>
            </div>

            <div className="flex flex-col gap-3">
              <div className="flex gap-5 items-center">
                <div>
                  <AiFillCar size={44} />
                </div>
                <div className="uppercase text-white text-lg font-semibold border-b-2 border-white pb-2 ">
                  Safe vehicle
                </div>
              </div>
              <div className="font-light text-md text-gray-300 pl-16">
                Lorem ipsum dolor sit amet consectetur adipisicing elit. Tenetur
                ea, quidem maxime ipsum illo autem laboriosam
              </div>
            </div>
          </div>
          <img
            src="https://i.pinimg.com/474x/27/76/59/2776590ea85befd13c554a24d6a5d171.jpg"
            alt="holdhand"
            className="w-[30%]"
          />
        </div>

        <div className="flex m-20 gap-28">
          <div className="w-[50%] flex flex-col gap-5">
            <div className="border-b-4 border-gray-300 pb-3 w-[60%] font-semibold text-3xl text-gray-700  ">
              Đánh giá của học viên
            </div>
            <div className="flex gap-5 mt-10 items-center">
              <img
                src="https://scontent.fsgn5-6.fna.fbcdn.net/v/t1.6435-9/123116045_772985256617271_6289105269278104549_n.jpg?_nc_cat=108&ccb=1-7&_nc_sid=ad2b24&_nc_ohc=laebF5sIFUUAX8VFESQ&_nc_ht=scontent.fsgn5-6.fna&oh=00_AfBOKBYLyQ6D7SQRwsFXimrfuUuKrPYLyfKst_XNlTvmIw&oe=65353413"
                alt="memberpPic"
                className="rounded-full w-[120px] h-[120px]"
              />
              <div className="">
                <div className="text-xl font-semibold text-gray-700">
                  Quang Huy
                </div>
                <div className="text-lg font-light text-gray-500">
                  1 tháng 10, 2023 7:39pm
                </div>
              </div>
            </div>
            <div className="flex gap-1 items-center">
              <div className="text-xl text-gray-800">Đánh giá:</div>
              <AiFillStar size={22} color="#1976D2" />
              <AiFillStar size={22} color="#1976D2" />
              <AiFillStar size={22} color="#1976D2" />
              <AiFillStar size={22} color="#1976D2" />
              <AiFillStar size={22} color="#1976D2" />
            </div>
            <div className="text-lg text-gray-800">
              Lorem ipsum dolor, sit amet consectetur adipisicing elit.
              Asperiores ad dicta error distinctio, dolorem magnam delectus
              consequatur doloribus cum natus quisquam necessitatibus in nemo
              corrupti molestiae excepturi illum ullam laborum.
            </div>
            <div></div>
          </div>
          <div className="w-[50%] ">
            <div className="border-b-4 border-gray-300 pb-3 w-[60%] font-semibold text-3xl text-gray-700">
              Co the ban chua biet
            </div>
            <div className="flex flex-col gap-5 mt-10">
            <div className="flex justify-between items-center border-2 border-gray-400 p-2">
              <div className="w-[85%]">
                Lorem ipsum dolor sit amet, consectetur adipisicing elit.
                Impedit
              </div>
              <div className="pr-5">
              <BiSolidRightArrow />
              </div>
            </div>
            
            <div className="flex justify-between items-center border-2 border-gray-400 p-2">
              <div className="w-[85%]">
                Lorem ipsum dolor sit amet, consectetur adipisicing elit.
                Impedit
              </div>
              <div className="pr-5">
              <BiSolidRightArrow />
              </div>
            </div>

            <div className="flex justify-between items-center border-2 border-gray-400 p-2">
              <div className="w-[85%]">
                Lorem ipsum dolor sit amet, consectetur adipisicing elit.
                Impedit
              </div>
              <div className="pr-5">
              <BiSolidRightArrow />
              </div>
            </div>

            <div className="flex justify-between items-center border-2 border-gray-400 p-2">
              <div className="w-[85%]">
                Lorem ipsum dolor sit amet, consectetur adipisicing elit.
                Impedit
              </div>
              <div className="pr-5">
              <BiSolidRightArrow />
              </div>
            </div>

            </div>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default AboutUsPage;
