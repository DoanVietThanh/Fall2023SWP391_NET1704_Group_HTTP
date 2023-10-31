import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import BackgroundSlider from '../../components/BackgroundSlider';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import theme from '../../theme';
const TheoryPage = () => {
  const url =
    'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png';
  const breadcrumbs = 'Theory';

  const [listTypeTest, setListTypeTest] = useState();
  const [infoTypeTest, setInfoTypeTest] = useState();

  useEffect(() => {
    async function fetchData() {
      const fetchListTypeTest = await axios
        .get('/theory/add-question')
        // .then((res) => console.log(res))
        //.catch((error) => toastError(error?.response?.data?.message));
        .catch((error) => console.log(error));
      console.log('fetchListTypeTest: ', fetchListTypeTest);

      const fetchInfoTypeTest = await axios
        .get('/theory/license-type/1')
        //.catch((error) => toastError(error?.response?.data?.message));
        .catch((error) => console.log(error));
      setListTypeTest(fetchListTypeTest?.data?.data);
      setInfoTypeTest(fetchInfoTypeTest?.data?.data);
    }
    fetchData();
  }, []);

  async function selectInfoTypeLicense(id) {
    const fetchInfoTypeTest = await axios.get(`/theory/license-type/${id}`);
    console.log('fetchInfoTypeTest: ', fetchInfoTypeTest);
    setInfoTypeTest(fetchInfoTypeTest?.data?.data);
  }

  console.log('infoTypeTest: ', infoTypeTest);
  console.log('listTypeTest: ', listTypeTest);
  // licenseTypeDesc
  return (
    <div>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      <div className='flex flex-col justify-center border p-6 m-6 gap-8'>
        <div className='center gap-8'>
          {listTypeTest &&
            listTypeTest?.map((item, index) => (
              <div
                onClick={() => selectInfoTypeLicense(item.licenseTypeId)}
                key={item.licenseTypeDesc}
                className={`active font-medium text-[20px] border-1 px-4 py-2 rounded-lg hover:bg-[${theme.color.mainColor}] hover:text-white cursor-pointer`}
              >
                Bằng lái {item.licenseTypeDesc}
              </div>
            ))}
        </div>

        <div>
          <div>
            <h1 className='font-bold text-[30px] text-center pb-8 text-[#0d5ef4] uppercase'>
              BỘ ĐỀ THI THỬ BẰNG LÁI XE B1
            </h1>

            <div>
              {/* Param mark */}
              <div>
                <p className='font-medium text-[20px]'>
                  Mỗi đề gồm 35 câu hỏi và chỉ có 1 đáp án đúng duy nhất ở từng
                  câu. Dựa theo cấu trúc đề thi lý thuyết B1 chính thức thì mỗi
                  đề thi sát hạch lý thuyết B1 sẽ bao gồm:
                </p>
                <div className='flex flow-col justify-center '>
                  <ul className='list-disc text-[18px] flex-y flex-col gap-2 my-2'>
                    <li>1 câu hỏi phần khái niệm;</li>
                    <li>7 câu hỏi về quy tắc giao thông,</li>
                    <li>1 câu hỏi nghiệp vụ vận tải;</li>
                    <li>1 câu về tốc độ khoảng cách;</li>
                    <li>1 câu hỏi về văn hóa & đạo đức người lái xe;</li>
                    <li>2 câu hỏi về kỹ thuật lái xe;</li>
                    <li>1 câu hỏi về cấu tạo sữa chữa;</li>
                    <li>10 câu hỏi biển báo;</li>
                    <li>
                      10 câu hỏi sa hình kèm theo 1 câu hỏi điểm liệt (tình
                      huống gây mất an toàn giao thông nghiêm trọng).
                    </li>
                  </ul>
                </div>
              </div>

              {/* Requirement  */}
              <div>
                <p className='font-medium text-[20px]'>
                  Học viên ôn tập cần đáp án ứng yêu cầu sau:
                </p>
                <div className='flex flow-col justify-center '>
                  <ul className='list-disc text-[18px] flex-y flex-col gap-4 my-2'>
                    <li>Thời gian làm đề thi: 22 phút.</li>
                    <li>Số câu hỏi phải đúng: 32/35 câu trở lên là đậu.</li>
                    <li className='text-red-700'>
                      Yêu cầu đặc biệt: KHÔNG LÀM SAI CÂU ĐIỂM LIỆT (câu hỏi *)
                    </li>
                  </ul>
                </div>
              </div>

              {/* Note  */}
              <div>
                <button className='text-[20px] font-bold text-white p-2 m-2 bg-[#0d5ef4] rounded-lg cursor-text'>
                  Lưu ý:{' '}
                </button>
                <p className='italic font-medium text-[18px]'>
                  Bộ đề thi bằng lái xe B2 này được xây dựng theo tài liệu 600
                  câu hỏi thi ô tô Tổng Cục Đường Bộ VN ban hành. Nếu học thuộc
                  hết 18 đề thi thử bằng lái xe B2 này đồng nghĩa với việc bạn
                  sẽ nắm chắc việc thi đậu lý thuyết 100% mà không cần phải lo
                  lắng. Chọn đề thi:
                </p>
              </div>
            </div>
          </div>

          {/* Select type  */}
          <div className='mt-8'>
            <h1 className='font-bold text-[30px] text-center pb-8 text-[#0d5ef4] uppercase'>
              Chọn đề thi
            </h1>
            <div className='flex justify-center gap-4'>
              {infoTypeTest &&
                infoTypeTest.map((item, index) => (
                  <Link
                    key={index}
                    to={`/theory/test/${item.theoryExamId}`}
                    className='text-[20px] font-medium p-4 bg-gray-200 cursor-pointer rounded-lg hover:text-[#0d5ef4]'
                  >
                    Đề {item.theoryExamId}
                  </Link>
                ))}
            </div>
          </div>
        </div>
      </div>

      <Footer />
    </div>
  );
};

export default TheoryPage;

// {
//   "statusCode": 200,
//   "message": null,
//   "data": {
//       "theoryExamId": 6,
//       "totalQuestion": 25,
//       "totalTime": 15,
//       "totalAnswerRequired": 21,
//       "licenseTypeId": 2,
//       "questions": [
//           {
//               "questionId": 1,
//               "questionAnswerDesc": "Khái niệm “phương tiện giao thông thô sơ đường bộ” được hiểu như thế nào là đúng?",
//               "isParalysis": true,
//               "image": null,
//               "isActive": null,
//               "licenseTypeId": 0,
//               "licenseType": {
//                   "licenseTypeId": 2,
//                   "licenseTypeDesc": "A1"
//               },
//               "questionAnswers": [
//                   {
//                       "questionAnswerId": 0,
//                       "answer": "G?m xe d?p (k? c? xe d?p máy, xe d?p di?n), xe xích lô, xe lan dùng cho ngu?i khuy?t t?t, xe súc v?t kéo và các lo?i xe tuong t?.",
//                       "isTrue": true,
//                       "questionId": 0
//                   },
//                   {
//                       "questionAnswerId": 1,
//                       "answer": "G?m xe d?p (k? c? xe d?p máy, xe d?p di?n), xe g?n máy, xe co gi?i dùng cho ngu?i khuy?t t?t và xe máy chuyên dùng.",
//                       "isTrue": false,
//                       "questionId": 0
//                   },
//                   {
//                       "questionAnswerId": 2,
//                       "answer": "G?m xe ô tô, máy kéo, ro moóc ho?c so mi ro moóc du?c kéo b?i xe ô tô, máy kéo.",
//                       "isTrue": false,
//                       "questionId": 0
//                   }
//               ]
//           },
//       ]
//     }
//   }

// console.log(Array.from({ length: 10 }, (_, index) => 0));
// console.log([...Array(10)].map(() => 0));
// console.log(new Array(10).fill(0));
