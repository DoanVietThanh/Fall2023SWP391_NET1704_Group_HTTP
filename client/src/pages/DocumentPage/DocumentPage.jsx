import React from "react";
import Header from "../../components/Header";
import Footer from "../../components/Footer";
import BackgroundSlider from "../../components/BackgroundSlider";
import { Link } from "react-router-dom";

const DocumentPage = () => {
  const url =
    "/img/backgroundSlide.png";
  const breadcrumbs = "Tài liệu lý thuyết";
  return (
    <div>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      <div className="flex justify-center gap-10 my-12">
        <button className="btn">Bằng lái A1</button>

        <button className="btn">Bằng lái A2</button>

        <button className="btn">Bằng lái B1</button>

        <button className="btn">Bằng lái B2</button>
      </div>
      <div className="flex">
        <div className="w-[20%] border-r-2 border-gray-200 flex flex-col gap-10 ">
          <div className="pl-5 pt-5 uppercase font-semibold text-lg text-blue-300">
            Lý thuyết bằng lái A1
          </div>
          <div className="flex flex-col gap-5 ">
            <Link to={"/document/all-question"}>
              <div className="ml-10 pb-3 border-b-2 border-gray-200 mr-10 flex flex-col ">
                <div className="font-semibold text-md">Toàn bộ câu hỏi</div>
                <div className="text-md text-gray-500">200 câu</div>
              </div>
            </Link>

            <Link to={"/document/important-question"}>
              <div className="ml-10 pb-3 border-b-2 border-gray-200 mr-10 flex flex-col ">
                <div className="font-semibold text-md">Câu hỏi điểm liệt</div>
                <div className="text-md text-gray-500">20 câu</div>
              </div>
            </Link>
          </div>

          <div></div>
        </div>
        <div className="w-[80%] pt-30 bg-gray-100">
          <div className="m-40 bg-white p-10 flex flex-col gap-5 shadow-md text-lg">
            <h2 className="font-semibold text-2xl">Lý thuyết bằng lái xe A1</h2>
            <div>
              Lý thuyết thi bằng lái xe A1 được thiết kế với 200 câu hỏi trắc
              nghiệm dành cho người điều khiển xe máy có dung tích xi lanh dưới
              175cm3.
            </div>
            <div>
              Bộ câu hỏi 200 câu hỏi trắc nghiệm lý thuyết thi bằng lái A1 gồm
              những câu hỏi về kiến thức luật giao thông đường bộ, câu hỏi trắc
              nghiệm biển báo giao thông và các câu hỏi về sa hình. Bộ câu hỏi
              này nhằm mục đích giúp người dự thi lấy bằng lái A1 nắm chắc được
              luật giao thông, hiểu được ý nghĩa các biển báo, và tham gia giao
              thông đúng luật cũng như bảo vệ sự an toàn cho bản thân và những
              người cùng tham gia giao thông trên đường.{" "}
            </div>
            <div>
              Thời gian làm bài thi lý thuyết là 19 phút. Làm đúng 21/25 câu là
              đạt với điều kiện không được sai câu điểm liệt
            </div>
            <div>Chúc các bạn học tốt và sớm có bằng!</div>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default DocumentPage;
