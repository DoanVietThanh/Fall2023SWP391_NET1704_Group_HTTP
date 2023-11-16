import React from 'react';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import BackgroundSlider from '../../components/BackgroundSlider';
import { Link } from 'react-router-dom';
import { useEffect } from 'react';
import axios from 'axios';
import { useState } from 'react';
import { toastError } from '../../components/Toastify';

const DocumentPage = () => {
  // const listDocDes = [
  //   {
  //     id: 1,
  //     title: "",
  //     content:
  //       "<div>Lý thuyết thi bằng lái xe A1 được thiết kế với 200 câu hỏi trắc nghiệm dành cho người điều khiển xe máy có dung tích xi lanh dưới 175cm3.</div> <div></div>Bộ câu hỏi 200 câu hỏi trắc nghiệm lý thuyết thi bằng lái A1 gồm những câu hỏi về kiến thức luật giao thông đường bộ, câu hỏi trắc nghiệm biển báo giao thông và các câu hỏi về sa hình. Bộ câu hỏi này nhằm mục đích giúp người dự thi lấy bằng lái A1 nắm chắc được luật giao thông, hiểu được ý nghĩa các biển báo, và tham gia giao thông đúng luật cũng như bảo vệ sự an toàn cho bản thân và những người cùng tham gia giao thông trên đường.</div><div>Thời gian làm bài thi lý thuyết là 19 phút. Làm đúng 21/25 câu là đạt với điều kiện không được sai câu điểm liệt</div><div>Chúc các bạn học tốt và sớm có bằng!</div>",
  //   },
  //   {
  //     id: 2,
  //     title: "",
  //     content:
  //       "<div>Lý thuyết thi bằng lái xe A2 được thiết kế với 200 câu hỏi trắc nghiệm dành cho người điều khiển xe máy có dung tích xi lanh dưới 175cm3.</div> <div></div>Bộ câu hỏi 200 câu hỏi trắc nghiệm lý thuyết thi bằng lái A1 gồm những câu hỏi về kiến thức luật giao thông đường bộ, câu hỏi trắc nghiệm biển báo giao thông và các câu hỏi về sa hình. Bộ câu hỏi này nhằm mục đích giúp người dự thi lấy bằng lái A1 nắm chắc được luật giao thông, hiểu được ý nghĩa các biển báo, và tham gia giao thông đúng luật cũng như bảo vệ sự an toàn cho bản thân và những người cùng tham gia giao thông trên đường.</div><div>Thời gian làm bài thi lý thuyết là 19 phút. Làm đúng 21/25 câu là đạt với điều kiện không được sai câu điểm liệt</div><div>Chúc các bạn học tốt và sớm có bằng!</div>",
  //   },
  //   {
  //     id: 3,
  //     title: "",
  //     content:
  //       "<div>Lý thuyết thi bằng lái xe B1 được thiết kế với 200 câu hỏi trắc nghiệm dành cho người điều khiển xe máy có dung tích xi lanh dưới 175cm3.</div> <div></div>Bộ câu hỏi 200 câu hỏi trắc nghiệm lý thuyết thi bằng lái A1 gồm những câu hỏi về kiến thức luật giao thông đường bộ, câu hỏi trắc nghiệm biển báo giao thông và các câu hỏi về sa hình. Bộ câu hỏi này nhằm mục đích giúp người dự thi lấy bằng lái A1 nắm chắc được luật giao thông, hiểu được ý nghĩa các biển báo, và tham gia giao thông đúng luật cũng như bảo vệ sự an toàn cho bản thân và những người cùng tham gia giao thông trên đường.</div><div>Thời gian làm bài thi lý thuyết là 19 phút. Làm đúng 21/25 câu là đạt với điều kiện không được sai câu điểm liệt</div><div>Chúc các bạn học tốt và sớm có bằng!</div>",
  //   },
  //   {
  //     id: 4,
  //     title: "",
  //     content:
  //       "<div>Lý thuyết thi bằng lái xe B1.1 được thiết kế với 200 câu hỏi trắc nghiệm dành cho người điều khiển xe máy có dung tích xi lanh dưới 175cm3.</div> <div></div>Bộ câu hỏi 200 câu hỏi trắc nghiệm lý thuyết thi bằng lái A1 gồm những câu hỏi về kiến thức luật giao thông đường bộ, câu hỏi trắc nghiệm biển báo giao thông và các câu hỏi về sa hình. Bộ câu hỏi này nhằm mục đích giúp người dự thi lấy bằng lái A1 nắm chắc được luật giao thông, hiểu được ý nghĩa các biển báo, và tham gia giao thông đúng luật cũng như bảo vệ sự an toàn cho bản thân và những người cùng tham gia giao thông trên đường.</div><div>Thời gian làm bài thi lý thuyết là 19 phút. Làm đúng 21/25 câu là đạt với điều kiện không được sai câu điểm liệt</div><div>Chúc các bạn học tốt và sớm có bằng!</div>",
  //   },
  //   {
  //     id: 5,
  //     title: "",
  //     content:
  //       "<div>Lý thuyết thi bằng lái xe B2 được thiết kế với 200 câu hỏi trắc nghiệm dành cho người điều khiển xe máy có dung tích xi lanh dưới 175cm3.</div> <div></div>Bộ câu hỏi 200 câu hỏi trắc nghiệm lý thuyết thi bằng lái A1 gồm những câu hỏi về kiến thức luật giao thông đường bộ, câu hỏi trắc nghiệm biển báo giao thông và các câu hỏi về sa hình. Bộ câu hỏi này nhằm mục đích giúp người dự thi lấy bằng lái A1 nắm chắc được luật giao thông, hiểu được ý nghĩa các biển báo, và tham gia giao thông đúng luật cũng như bảo vệ sự an toàn cho bản thân và những người cùng tham gia giao thông trên đường.</div><div>Thời gian làm bài thi lý thuyết là 19 phút. Làm đúng 21/25 câu là đạt với điều kiện không được sai câu điểm liệt</div><div>Chúc các bạn học tốt và sớm có bằng!</div>",
  //   },
  // ];
  const url = '/img/backgroundSlide.png';
  const breadcrumbs = 'Tài liệu lý thuyết';
  // const [showDocDes, setShowDocDes] = useState(true);
  // const handleShowDocDes = () => {
  //   setShowDocDes(true);
  //   setShowAllQues(false);
  //   setShowImpQues(false);
  // };
  const [showAllQues, setShowAllQues] = useState(true);
  const handleShowAllQues = () => {
    // setShowDocDes(false);
    setShowAllQues(true);
    setShowImpQues(false);
  };
  const [showImpQues, setShowImpQues] = useState(false);
  const handleShowImpQues = () => {
    // setShowDocDes(false);
    setShowAllQues(false);
    setShowImpQues(true);
  };
  const [listLicense, setListLicense] = useState([]);
  const urlService = process.env.REACT_APP_SERVER_API;
  //list tat ca
  const [listAllQuestion, setListAllQuestion] = useState([]);
  //list cau diem liet
  const [listImportantQuestion, setListImportantQuestion] = useState([]);
  //biến lưu ID loại bằng lái được chọn
  const [choosenLicense, setChoosenLicense] = useState(1);
  const handleChoosenLicenseChange = (license) => {
    setChoosenLicense(license);
  };

  useEffect(() => {
    async function getListAllQuestion() {
      //lấy tất cả câu hỏi thuộc loại bằng lái được chọn
      await axios
        .get(`${urlService}/theory/question-bank/${choosenLicense}`)
        .then((res) => {
          console.log('res: ', res);
          setListAllQuestion(res.data?.data.questionWithAnswer);
        })
        .catch((error) => {
          console.log('error: ', error);
          toastError(error?.response?.data?.message);
        });
      //lấy câu hỏi diem liet thuộc loại bằng lái được chọn
      await axios
        .get(`${urlService}/theory/license-type/${choosenLicense}/isParalysis`)
        .then((res) => {
          console.log('res: ', res);
          setListImportantQuestion(res.data?.data);
        })
        .catch((error) => {
          console.log('error: ', error);
          toastError(error?.response?.data?.message);
        });
      //lấy các loại bằng lái
      await axios
        .get(`${urlService}/theory/add-question`)
        .then((res) => {
          console.log('res: ', res);
          setListLicense(res.data?.data);
        })
        .catch((error) => {
          console.log('error: ', error);
          toastError(error?.response?.data?.message);
        });
    }
    getListAllQuestion();
  }, [choosenLicense]);

  const [hienDapAn, setHienDapAn] = useState({});
  const handleHienDapAn = ([id]) =>
    setHienDapAn((prev) => ({
      ...prev,
      [id]: !prev[id],
    }));
  console.log('listLicense', listLicense);
  console.log('licenseID', choosenLicense);
  console.log('all', listAllQuestion);
  console.log('diemliet', listImportantQuestion);
  return (
    <div>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      {/* button cac loai bang lai */}
      <div className='flex justify-center gap-10 my-12'>
        {listLicense.map((license, index) => (
          <button
            key={license.licenseTypeId}
            className='btn'
            onClick={() =>
              handleChoosenLicenseChange(license.licenseTypeId) &&
              handleShowAllQues
            }
          >
            Bằng lái {license.licenseTypeDesc}
          </button>
        ))}
      </div>
      {/* sidebar */}
      <div className='flex'>
        <div className='w-[20%] border-r-2 border-gray-200 flex flex-col gap-10 '>
          <div className='pl-5 pt-5 uppercase font-semibold text-lg text-blue-300'>
            Lý thuyết bằng lái{' '}
            {listLicense.map((license, index) =>
              license.licenseTypeId === choosenLicense ? (
                <span>{license.licenseTypeDesc}</span>
              ) : null
            )}
          </div>

          {/* nav Toan bo cau hoi */}
          <div
            className='ml-10 pb-3 border-b-2 border-gray-200 mr-10 flex flex-col hover:cursor-pointer'
            onClick={handleShowAllQues}
          >
            <div className='font-semibold text-md'>Toàn bộ câu hỏi</div>
            <div className='text-md text-gray-500'>
              {' '}
              {listAllQuestion.length} câu{' '}
            </div>
          </div>
          {/* nav Cau hoi diem liet */}
          <div
            className='ml-10 pb-3 border-b-2 border-gray-200 mr-10 flex flex-col hover:cursor-pointer'
            onClick={handleShowImpQues}
          >
            <div className='font-semibold text-md'>Câu hỏi điểm liệt</div>
            <div className='text-md text-gray-500'>
              {' '}
              {listImportantQuestion.length} câu
            </div>
          </div>
        </div>
        <div className='w-[80%] bg-gray-100 py-20'>
          {/* mô tả lý thuyết bằng lái */}
          {/* {showDocDes && (
            <div className="m-40 bg-white p-10 shadow-md text-lg">
              {listLicense.map((license, index) =>
                license.licenseTypeId === choosenLicense ? (
                  <div className="flex flex-col gap-4">
                    <h2 className="font-semibold text-2xl">
                      Tài liệu lý thuyết bằng lái{" "}
                      <span>{license.licenseTypeDesc}</span>
                    </h2>
                    {listDocDes.map((licenseDesc, index) =>
                      license.licenseTypeId === licenseDesc.id ? (
                        <div
                          className="flex flex-col gap-2"
                          dangerouslySetInnerHTML={{
                            __html: `${licenseDesc.content}`,
                          }}
                        ></div>
                      ) : null
                    )}
                  </div>
                ) : null
              )}
            </div>
          )} */}
          {/* list toàn bộ câu hỏi */}
          {showAllQues && (
            <div>
              <h2 className='font-semibold text-3xl pl-32 pb-10'>
                Toàn bộ câu hỏi
              </h2>
              <div className='flex flex-col gap-10'>
                {listAllQuestion.map((qa, index) => (
                  <div className='mx-32 p-8 rounded-md border border-gray-200 bg-white '>
                    <div className='pb-3 text-xl'>
                      <span className='font-semibold text-blue-500'>
                        Câu {index + 1}
                      </span>
                      : {qa.question.questionAnswerDesc}
                    </div>
                    <div className='pl-14 flex flex-col gap-2'>
                      <ul style={{ listStyleType: 'upper-alpha' }}>
                        {qa.answers.map((option, x) => (
                          <li
                            key={x}
                            style={{ marginBottom: '10px', fontSize: '18px' }}
                          >
                            {`${option.answer}`}{' '}
                          </li>
                        ))}
                      </ul>
                    </div>
                    <button
                      onClick={() => handleHienDapAn([index + 1])}
                      className='mt-5 p-1 rounded-lg shadow-md text-mdmd hover:text-green-500'
                    >
                      Xem đáp án
                    </button>
                    {hienDapAn[index + 1] && (
                      <div className='pt-5 pl-5'>
                        <span className='font-semibold'>Đáp án đúng: </span>{' '}
                        {qa.answers.map((option, i) => (
                          <div key={i}>
                            {`${option.isTrue}` === 'true' ? (
                              <li
                                key={i}
                                style={{
                                  marginBottom: '10px',
                                  fontSize: '18px',
                                }}
                              >
                                {`${option.answer}`}{' '}
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
          )}
          {/* list câu hỏi điểm liệt */}

          {showImpQues && (
            <div>
              <h2 className='font-semibold text-3xl pl-32 pb-10'>
                Toàn bộ câu hỏi
              </h2>
              <div className='flex flex-col gap-10'>
                {listImportantQuestion.map((qa, index) => (
                  <div className='mx-32 p-8 rounded-md border border-gray-200 bg-white '>
                    <div className='pb-3 text-xl'>
                      <span className='font-semibold text-blue-500'>
                        Câu {index + 1}
                      </span>
                      : {qa.questionAnswerDesc}
                    </div>
                    <div className='pl-14 flex flex-col gap-2'>
                      <ul style={{ listStyleType: 'upper-alpha' }}>
                        {qa.questionAnswers.map((option, x) => (
                          <li
                            key={x}
                            style={{ marginBottom: '10px', fontSize: '18px' }}
                          >
                            {`${option.answer}`}{' '}
                          </li>
                        ))}
                      </ul>
                    </div>
                    <button
                      onClick={() => handleHienDapAn([index + 1])}
                      className='mt-5 p-1 rounded-lg shadow-md text-mdmd hover:text-green-500'
                    >
                      Xem đáp án
                    </button>
                    {hienDapAn[index + 1] && (
                      <div className='pt-5 pl-5'>
                        <span className='font-semibold'>Đáp án đúng: </span>{' '}
                        {qa.questionAnswers.map((option, i) => (
                          <div key={i}>
                            {`${option.isTrue}` === 'true' ? (
                              <li
                                key={i}
                                style={{
                                  marginBottom: '10px',
                                  fontSize: '18px',
                                }}
                              >
                                {`${option.answer}`}{' '}
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
          )}
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default DocumentPage;
