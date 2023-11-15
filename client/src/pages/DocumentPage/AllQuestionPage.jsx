import React, { useState } from "react";
import Header from "../../components/Header";
import Footer from "../../components/Footer";
import BackgroundSlider from "../../components/BackgroundSlider";
import { Link, useLocation } from "react-router-dom";
import { useEffect } from "react";
import axios from "axios";
import { toastError } from "../../components/Toastify";

const AllQuestionPage = () => {
 
  const listTypeQuestion = [
    {
      link: "/document/all-question",
      type: "Toàn bộ câu hỏi",
      num: "200",
    },

    {
      link: "/document/important-question",
      type: "Câu điểm liệt",
      num: "20",
    },
  ];
  const urlService = process.env.REACT_APP_SERVER_API;
  const [listLicense, setListLicense] = useState([]);
  const [listAllQuestion, setListAllQuestion] = useState([]);
  //lấy các loại bằng lái
  useEffect(() => {
    async function getListLicense() {
      await axios
        .get(`${urlService}/theory/add-question`)
        .then((res) => {
          console.log("res: ", res);
          setListLicense(res.data?.data);
        })
        .catch((error) => {
          console.log("error: ", error);
          toastError(error?.response?.data?.message);
        });
    }
    getListLicense();
  }, []);
  //lấy tất cả câu hỏi thuộc loại bằng lái được chọn
  useEffect(() => {
    async function getListAllQuestion() {
      await axios
        .get(`${urlService}/theory/question-bank/1`)
        .then((res) => {
          console.log("res: ", res);
          setListAllQuestion(res.data?.data.questionWithAnswer);
        })
        .catch((error) => {
          console.log("error: ", error);
          toastError(error?.response?.data?.message);
        });
    }
    getListAllQuestion();
  }, []);
  console.log("allQuestion", listAllQuestion);
  const location = useLocation().pathname;
  const url = "/img/backgroundSlide.png";
  const breadcrumbs = "Tài liệu lý thuyết";
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
        {listLicense.map((license, y) => (
          <button key={license.licenseTypeId} className="btn">
            Bằng lái {license.licenseTypeDesc}
          </button>
        ))}
      </div>
      <div className="flex">
        <div className="w-[20%] border-r-2 border-gray-200 flex flex-col gap-10 ">
          <div className="pl-5 pt-5 font-semibold text-xl text-blue-300">
            Lý thuyết lái xe máy A1
          </div>
          <div className="flex flex-col gap-5 ">
            {listTypeQuestion.map((item, i) =>
              item.link === location ? (
                <Link to={item.link}>
                  <div className="ml-10 pb-3 border-b-2 border-gray-200 mr-10 flex flex-col documentNav">
                    <div className="font-semibold text-md documentType">
                      {item.type}
                    </div>
                    <div className="text-md text-gray-500">{item.num} câu</div>
                  </div>
                </Link>
              ) : (
                <Link to={item.link} >
                  <div className="ml-10 pb-3 border-b-2 border-gray-200 mr-10 flex flex-col ">
                    <div className="font-semibold text-md ">{item.type}</div>
                    <div className="text-md text-gray-500">{item.num} câu</div>
                  </div>
                </Link>
              )
            )}
          </div>
        </div>
        <div className="w-[80%] bg-gray-100 py-20">
          <h2 className="font-semibold text-3xl pl-32 pb-10">
            Toàn bộ câu hỏi
          </h2>
          <div className="flex flex-col gap-10">
            {listAllQuestion.map((qa, index) => (
              <div className="mx-32 p-8 rounded-md border border-gray-200 bg-white ">
                <div className="pb-3 text-xl">
                  <span className="font-semibold text-blue-500">
                    Câu {index + 1}
                  </span>
                  : {qa.question.questionAnswerDesc}
                </div>
                <div className="pl-14 flex flex-col gap-2">
                  <ul style={{ listStyleType: "upper-alpha" }}>
                    {qa.answers.map((option, x) => (
                      <li
                        key={x}
                        style={{ marginBottom: "10px", fontSize: "18px" }}
                      >
                        {`${option.answer}`}{" "}
                      </li>
                    ))}
                  </ul>
                </div>
                <button
                  onClick={() => handleHienDapAn([index+1])}
                  className="mt-5 p-1 rounded-lg shadow-md text-mdmd hover:text-green-500"
                >
                  Xem đáp án
                </button>
                {hienDapAn[index+1] && (
                  <div className="pt-5 pl-5">
                    <span className="font-semibold">Đáp án đúng: </span>{" "}
                    {qa.answers.map((option, i) => (
                      <div key={i}>
                        {`${option.isTrue}` === "true" ? (
                          <li
                            key={i}
                            style={{ marginBottom: "10px", fontSize: "18px" }}
                          >
                            {`${option.answer}`}{" "}
                          </li>
                        ) : null}
                      </div>
                    ))}
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

export default AllQuestionPage;
