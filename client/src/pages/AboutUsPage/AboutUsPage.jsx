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
    "/img/backgroundSlide.png";
  const breadcrumbs = "About Us";
  return (
    <div>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      <div className="flex flex-col">
        <div className="flex flex-col mx-20">
          <div className="flex justify-between items-end">
            <div className="flex flex-col gap-5 w-[50%]">
              <div className="text-4xl font-bold w-[70%] border-b-2 border-gray-400 pb-4 pt-10">
                Vì sao nên chọn chúng tôi ?
              </div>
              <div className="flex flex-col gap-4 pb-6 text-lg text-gray-600">
                <div className="leading-8">
                  Trang web của chúng tôi được tạo ra với mục tiêu làm cho hành
                  trình này trở nên dễ dàng và thú vị hơn bao giờ hết. Chúng tôi
                  tự hào giới thiệu một nguồn tài liệu đa dạng và phong phú về
                  luật giao thông, quy tắc an toàn khi lái xe, và kiến thức cần
                  thiết cho kỳ thi bằng lái xe. Dù bạn là người mới học lái xe
                  hoặc bạn cần cập nhật kiến thức sau một thời gian dài, trang
                  web của chúng tôi có những tài liệu phù hợp cho bạn.
                </div>
                <div className="leading-8">
                  Chúng tôi không chỉ cung cấp các kiến thức cơ bản về luật giao
                  thông và quy tắc lái xe, mà còn giúp bạn hiểu rõ về cách áp
                  dụng chúng trong thực tế khi lái xe. Chúng tôi cung cấp bài
                  kiểm tra mẫu để bạn tự đánh giá trình độ và sẵn sàng cho kỳ
                  thi.
                </div>
                <div className="leading-8">
                  Chúng tôi cam kết cung cấp cho bạn thông tin chính xác và cập
                  nhật, đồng thời hỗ trợ bạn vượt qua mọi thách thức trong việc
                  đạt được mục tiêu sở hữu bằng lái xe một cách an toàn và thành
                  công.
                </div>
                <div className="leading-8 ">
                  Hãy bắt đầu hành trình của bạn với trang web của chúng tôi
                  ngay hôm nay. Chúng tôi luôn sẵn sàng hỗ trợ bạn trong việc
                  biến giấc mơ sở hữu bằng lái xe thành hiện thực.
                </div>
                <div>
                  <div>Chân thành,</div>
                  <div className="text-gray-900 font-medium">HTTP GROUP</div>
                </div>
              </div>
            </div>

            <div className="w-[50%]">
              <img
                src="/img/aboutus.png"
                className="w-full object-cover"
                alt="About Us"
              />
            </div>
          </div>

          <div className="flex">
            <div className="flex gap-3 w-[25%] p-8 items-center center bg-blue-600/100">
              <PiSteeringWheel size={36} color="white" />
              <div className="center uppercase text-xl text-white font-semibold  ">
                Phương tiện hiện đại
              </div>
            </div>
            <div className="flex gap-3 w-[25%] p-8 items-center center bg-blue-600/90">
              <BsPostcard size={36} color="white" />
              <div className="center uppercase text-xl text-white font-semibold  ">
                Cam kết đào tạo
              </div>
            </div>
            <div className="flex gap-3 w-[25%] p-8 items-center center bg-blue-600/80">
              <BsSpeedometer2 size={36} color="white" />
              <div className="center uppercase text-xl text-white font-semibold  ">
                Lịch trình linh hoạt
              </div>
            </div>
            <div className="flex gap-3 w-[25%] p-8 items-center center bg-blue-600/70">
              <FaRegNewspaper size={36} color="white" />
              <div className="center uppercase text-xl text-white font-semibold  ">
                Thực hành an toàn
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
                  luật giao thông
                </div>
              </div>
              <div className="font-light text-md text-gray-300 pl-16">
                Trang web cung cấp kiến thức về quy tắc giao thông, biển báo, và
                an toàn đường, giúp người học hiểu rõ và tuân thủ đúng luật.
              </div>
            </div>

            <div className="flex flex-col gap-3">
              <div className="flex gap-5 items-center">
                <div>
                  <BsFillPersonCheckFill size={44} />
                </div>
                <div className="uppercase text-white text-lg font-semibold border-b-2 border-white pb-2 ">
                  Giảng viên tận tâm
                </div>
              </div>
              <div className="font-light text-md text-gray-300 pl-16">
                Giảng viên chuyên nghiệp với sự tận tâm, sẵn sàng giải đáp mọi
                thắc mắc, hỗ trợ học viên trong quá trình học.
              </div>
            </div>

            <div className="flex flex-col gap-3">
              <div className="flex gap-5 items-center">
                <div>
                  <AiFillCar size={44} />
                </div>
                <div className="uppercase text-white text-lg font-semibold border-b-2 border-white pb-2 ">
                  phương tiện an toàn
                </div>
              </div>
              <div className="font-light text-md text-gray-300 pl-16">
                Phương tiện được bảo trì định kỳ, đảm bảo an toàn và hiệu quả
                cho việc học lái.
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
          <div className="w-[60%] ">
            <div className="border-b-4 border-gray-300 pb-3 w-[60%] font-semibold text-3xl text-gray-700">
              Có thể bạn chưa biết
            </div>
            <div className="flex flex-col gap-5 mt-10">
              <div className="flex justify-between items-center text-lg border-2 border-gray-400 p-2">
                <div className="w-[85%]">
                  Trang web của chúng tôi sẽ cung cấp lịch thi và thông tin về
                  việc đăng ký kỳ thi B1, giúp bạn dễ dàng quản lý thời gian và
                  lên lịch thi phù hợp.
                </div>
                <div className="pr-5">
                  <BiSolidRightArrow />
                </div>
              </div>

              <div className="flex justify-between items-center text-lg border-2 border-gray-400 p-2">
                <div className="w-[85%]">
                  Trang web của chúng tôi sẽ cung cấp thông tin về những thay
                  đổi mới nhất về quy tắc giao thông và luật pháp liên quan đến
                  B1, giúp bạn cập nhật kiến thức hiện tại.
                </div>
                <div className="pr-5">
                  <BiSolidRightArrow />
                </div>
              </div>

              <div className="flex justify-between items-center text-lg border-2 border-gray-400 p-2">
                <div className="w-[85%]">
                  Chúng tôi sẽ cung cấp tài liệu tham khảo, bao gồm các đề thi
                  mẫu và bài tập thực hành, giúp bạn chuẩn bị tốt cho kỳ thi B1.
                </div>
                <div className="pr-5">
                  <BiSolidRightArrow />
                </div>
              </div>

              <div className="flex justify-between items-center text-lg border-2 border-gray-400 p-2">
                <div className="w-[85%]">
                  Chúng tôi sẽ giúp bạn hiểu rõ các điểm kiểm tra lý thuyết cần
                  nắm về luật giao thông và biển báo, giúp bạn tự tin hơn trong
                  kỳ thi B1.
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
