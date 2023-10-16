import React, { useState } from "react";
import Header from "../../components/Header";
import Footer from "../../components/Footer";
import BackgroundSlider from "../../components/BackgroundSlider";
import { Link, useLocation } from "react-router-dom";

const CauHoiDiemLietPage = () => {
  const listQuestion = [
    {
      id: "1",
      question:
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Quae soluta sed unde libero harum qui quo",
      options: [
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
      ],
      correctAnswer:
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
    },

    {
      id: "2",
      question:
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Quae soluta sed unde libero harum qui quo",
      options: [
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
      ],
      correctAnswer:
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
    },

    {
      id: "3",
      question:
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Quae soluta sed unde libero harum qui quo",
      options: [
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
      ],
      correctAnswer:
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
    },

    {
      id: "4",
      question:
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Quae soluta sed unde libero harum qui quo",
      options: [
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
      ],
      correctAnswer:
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
    },

    {
      id: "5",
      question:
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Quae soluta sed unde libero harum qui quo",
      options: [
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
      ],
      correctAnswer:
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
    },

    {
      id: "6",
      question:
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Quae soluta sed unde libero harum qui quo",
      options: [
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
      ],
      correctAnswer:
        "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
    },
  ];
  const listTypeQuestion = [
    {
      link: "/document/toanbocauhoi",
      type: "Toàn bộ câu hỏi",
      num: "200",
    },

    {
      link: "/document/cauhoidiemliet",
      type: "Câu điểm liệt",
      num: "20",
    },
  ];
  const location = useLocation().pathname;
  const url =
    "https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png";
  const breadcrumbs = "Document";
  const [hienDapAn, setHienDapAn] = useState({});
  const handleHienDapAn = ([id]) =>
    setHienDapAn((prev) => ({
      ...prev,
      [id]: !prev[id],
    }));
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
          <div className="pl-5 pt-5 font-semibold text-xl text-blue-300">
            Lý thuyết lái xe máy A1
          </div>
          <div className="flex flex-col gap-5 ">
            {listTypeQuestion.map((item, index) =>
              item.link === location ? (
                <Link to={item.link}>
                  <div className="ml-10 pb-3 border-b-2 border-gray-200 mr-10 flex flex-col documentNav">
                    <div className="font-semibold text-md documentType">{item.type}</div>
                    <div className="text-md text-gray-500">{item.num} câu</div>
                  </div>
                </Link>
              ) : (
                <Link to={item.link}>
                  <div className="ml-10 pb-3 border-b-2 border-gray-200 mr-10 flex flex-col ">
                    <div className="font-semibold text-md ">{item.type}</div>
                    <div className="text-md text-gray-500">{item.num} câu</div>
                  </div>
                </Link>
              )
            )}
          </div>

          <div></div>
        </div>
        <div className="w-[80%] bg-gray-100 py-20">
          <h2 className="font-semibold text-3xl pl-32 pb-10">
            Câu hỏi điểm liệt
          </h2>
          <div className="flex flex-col gap-10">
            {listQuestion.map((question, index) => (
              <div className="mx-32 p-8 rounded-md border border-gray-200 bg-white ">
                <div className="pb-3 text-xl">
                  <span className="font-semibold text-blue-500">Câu {question.id}</span>: {question.question}
                </div>
                <div className="pl-14 flex flex-col gap-2">
                  <ul style={{ listStyleType: "upper-alpha" }}>
                    {question.options.map((option, index) => (
                      <li
                        key={index}
                        style={{ marginBottom: "10px", fontSize: "18px" }}
                      >
                        {`${option}`}{" "}
                      </li>
                    ))}
                  </ul>
                </div>
                <button
                  onClick={() => handleHienDapAn(question.id)}
                  className="mt-5 p-1 rounded-lg shadow-md text-mdmd hover:text-green-500 "
                >
                  Xem đáp án
                </button>
                {hienDapAn[question.id] && (
                  <div className="pt-5 pl-5">
                    <span className="font-semibold">Đáp án đúng: </span>{" "}
                    {question.correctAnswer}
                  </div>
                )}
              </div>
            ))}
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default CauHoiDiemLietPage;
