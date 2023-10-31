import React from "react";
import BackgroundSlider from "../../components/BackgroundSlider";
import Footer from "../../components/Footer";
import Header from "../../components/Header";
import { Chart, registerables } from "chart.js";
import { BiSolidRightArrow } from "react-icons/bi";
import { AiOutlineArrowLeft, AiOutlineArrowRight } from "react-icons/ai";
Chart.register(...registerables);
const SimulationSituation = () => {
  const url =
    "https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png";
  const breadcrumbs = "Thi thử mô phỏng";
  const listChap = [
    {
      id: 1,
      title: "Chương I",
      content:
        "29 tình huống xoay quanh các tình huống thực tế khi tham gia giao thông trong khu đô thị, khu dân cư đông đúc.",
    },
    {
      id: 2,
      title: "Chương II",
      content:
        "13 tình huống từ 30 - 43 xoay quanh các tình huống thực tế khi tham gia giao thông ở các đường gấp khúc vào buổi tối.",
    },
    {
      id: 3,
      title: "Chương III",
      content:
        "19 tình huống từ 44 - 63 xoay quanh các tình huống thực tế khi tham gia giao thông ở trên đường cao tốc.",
    },
    {
      id: 4,
      title: "Chương IV",
      content:
        "9 tình huống từ 64 - 73 xoay quanh các tình huống thực tế khi tham gia giao thông ở trên đường cao tốc.",
    },
    {
      id: 5,
      title: "Chương V",
      content:
        "16 tình huống từ 64 - 90 xoay quanh các tình huống thực tế khi tham gia giao thông ở khu vực ngoại thành.",
    },
    {
      id: 6,
      title: "Chương VI",
      content:
        " 29 tình huống từ 91 - 120 xoay quanh các tình huống thực tế khi tham gia giao thông các tình huống hỗn hợp.",
    },
  ];
  // const listButton = [
  //   {
  //     id: 1,
  //     title:(
  //       <>
  //         <AiOutlineArrowLeft /> Câu trước đó
  //       </>
  //     ),
  //     link: "",
  //   },
  //   {
  //     id: 2,
  //     title:(
  //       <>
  //         <BiSolidRightArrow /> Làm lại
  //       </>
  //     ),
  //     link: "",
  //   },
  //   {
  //     id: 3,
  //     title:(
  //       <>
  //         Câu tiếp theo <AiOutlineArrowRight />
  //       </>
  //     ),
  //     link: "",
  //   },
  // ];

  return (
    <div>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      <div className="p-10 flex gap-4 bg-gray-100">
        <div className="w-[25%]">
          <div className="flex flex-col gap-4">
            <div className="text-2xl font-medium text-blue-900 border-b-2">
              Tình huống mô phỏng
            </div>
            <div className=" bg-gray-200 py-6 px-10">
              <div>
                <div className="grid grid-cols-4 gap-4 ">
                  {Array.from({ length: 60 }, (_, index) => (
                    <div
                      key={index + 1}
                      className="text-center text-xl cursor-pointer bg-gray-300 rounded-md p-2 hover:text-white hover:bg-[#0D5EF4]"
                    >
                      {index + 1}
                    </div>
                  ))}
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="w-[75%] flex flex-col gap-10">
          <div className="flex flex-col gap-4">
            <div>
              <div className="text-xl font-medium text-blue-900">
                Tình huống số ??
              </div>
              <img
                src="https://i.pinimg.com/474x/32/72/9e/32729e0351bd190331900b82643be254.jpg"
                alt="pic"
                className="w-full h-[600px]"
              />
            </div>
            <div className="w-[100%] flex justify-between gap-2">
              <button className="buttonSimulate ">
                {" "}
                <AiOutlineArrowLeft /> Câu trước đó
              </button>
              <button className="buttonSimulate ">
                {" "}
                <BiSolidRightArrow /> Làm lại
              </button>
              <button className="buttonSimulate ">
                {" "}
                Câu tiếp theo <AiOutlineArrowRight />
              </button>
            </div>

            <div className="flex bg-gray-300 p-2 text-xl">
              <div className="w-[50%] ">Thời điểm gán cờ:</div>
              <div className="w-[50%] ">Điểm đạt được: </div>
            </div>

            <div className="text-blue-400 text-2xl">
              Nhấn phím CÁCH trên bàn phím để thực hiện gán cờ khi phát hiện
              tình huống mất an toàn
            </div>

            {/* ket qua */}
            <div className="flex flex-col gap-4 text-lg">
              <div className="flex mx-36 bg-gray-300 items-center">
                <img 
                src="/img/police.png"
                alt="police"
                className="w-[180px] h-[210px] pl-8"
                />
              <div className="flex flex-col gap-4 p-8">
                <div>Bạn gán cờ ở giây thứ 3 và được số điểm là 0 điểm</div>
                <div>
                  <div>Bạn cần cố gắng hơn</div>
                  <div>Bài thi của bạn khá tốt</div>
                  <div>Bài thi của bạn khá tốt</div>
                </div>
              </div>
              </div>
              
              <div>
                <div> Bạn không đạt yêu cầu</div>
                <div> Bạn đã đạt yêu cầu</div>
              </div>
              <div>Bạn có thể xem gợi ý như sau: </div>
              <div>Chèn ảnh</div>
            </div>
          </div>

          <div className="flex flex-col gap-6">
            <div className="text-4xl font-bold mt-6">
              Ôn tập 120 tình huống mô phỏng giao thông trong học lái xe ô tô
            </div>
            <div className="text-xl">
              Trong 120 tình huống mô phỏng giao thông trong học lái xe
              B1,B2,C,D, E do daylaixehanoi.vn biên soạn và phát triển dựa vào
              các tình huống thực tế giao thông của Việt Nam. Dựa theo thực tế
              thì cấu trúc nội dung của phần mềm mô phỏng này bao gồm :
            </div>
            <ul className="list-disc pl-6">
              {listChap.map((item, index) => (
                <li className="mb-4">
                  <span className="text-2xl font-semibold">
                    {item.title} {": "}
                  </span>
                  <span className="text-xl mb-4">{item.content}</span>
                </li>
              ))}
            </ul>
            <div className="italic">
              <span className="text-2xl font-semibold">Lưu ý: </span>
              <span className="text-xl mb-4">
                Bộ 120 câu hỏi mô phỏng tình huống giao thông được phát triển
                dựa trên tình hình giao thông thực tế do Tổng Cục Đường Bộ Việt
                Nam ban hành. Các tình huống đều rất sát sao và đặc biệt nghiêm
                trọng khi tham gia giao thông rất dễ xảy ra. Nếu học thuộc hết
                120 tình huống này đồng nghĩa bạn đã nắm chắc việc thi đậu mô
                phỏng 100% không cần lo lắng ngoài ra còn có thêm kỹ năng thực
                tế khi tham gia giao thông.
              </span>
            </div>
          </div>

          <div className="flex flex-col gap-5">
            <div className="text-4xl font-bold mt-5">
              Phương pháp chấm điểm :
            </div>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
            <ul className="list-disc pl-5">
              <li>
                <div className="text-xl mb-4">
                  Trong mỗi tình huống có 02 mốc thời điểm là{" "}
                  <span className="font-semibold">5đ và 0đ</span>.
                </div>
              </li>
              <li>
                <div className="text-xl mb-4">
                  <span className="font-semibold">5 điểm:</span>Thời điểm bắt
                  đầu có dấu hiệu phát hiện ra tình huống nguy hiểm, lái xe cần
                  phải xử lý.
                </div>
              </li>
              <li>
                <div className="text-xl mb-4">
                  <span className="font-semibold">0 điểm:</span>Mốc thời điểm mà
                  xử lý từ thời điểm này vẫn xảy ra tai nạn.
                </div>
              </li>
              <li>
                <div className="text-xl mb-4">
                  Học viên lựa chọn được giữa hai mốc này sẽ đạt mức điểm tương
                  ứng từ 5-4-3-2-1 điểm. (tối đa điểm cho mỗi tình huống là 5đ)
                </div>
              </li>
            </ul>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default SimulationSituation;
