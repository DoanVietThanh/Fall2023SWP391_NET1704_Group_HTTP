import React from 'react';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import BackgroundSlider from '../../components/BackgroundSlider';
import theme from '../../theme';
import { Link } from 'react-router-dom';

const TheoryPage = () => {
  const url =
    'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png';
  const breadcrumbs = 'Theory';
  return (
    <div>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      <div className='flex flex-col justify-center border p-6 m-6 gap-8'>
        <div className='center gap-8'>
          <div
            className={`active font-medium text-[20px] border-1 px-4 py-2 rounded-lg hover:bg-[${theme.color.mainColor}] hover:text-white cursor-pointer`}
          >
            Bằng lái A1
          </div>
          <div
            className={`font-medium text-[20px] border-1 px-4 py-2 rounded-lg hover:bg-[${theme.color.mainColor}] hover:text-white cursor-pointer`}
          >
            Bằng lái A2
          </div>
          <div
            className={`font-medium text-[20px] border-1 px-4 py-2 rounded-lg hover:bg-[${theme.color.mainColor}] hover:text-white cursor-pointer`}
          >
            Bằng lái B1
          </div>
          <div
            className={`font-medium text-[20px] border-1 px-4 py-2 rounded-lg hover:bg-[${theme.color.mainColor}] hover:text-white cursor-pointer`}
          >
            Bằng lái B2
          </div>
        </div>

        <div>
          <h1 className='font-bold text-[30px] text-center pb-8 text-[#0d5ef4] uppercase'>
            BỘ ĐỀ THI THỬ BẰNG LÁI XE B1
          </h1>

          <div>
            {/* Param mark */}
            <div>
              <p className='font-medium text-[20px]'>
                Mỗi đề gồm 35 câu hỏi và chỉ có 1 đáp án đúng duy nhất ở từng
                câu. Dựa theo cấu trúc đề thi lý thuyết B1 chính thức thì mỗi đề
                thi sát hạch lý thuyết B1 sẽ bao gồm:
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
                    10 câu hỏi sa hình kèm theo 1 câu hỏi điểm liệt (tình huống
                    gây mất an toàn giao thông nghiêm trọng).
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
                Bộ đề thi bằng lái xe B2 này được xây dựng theo tài liệu 600 câu
                hỏi thi ô tô Tổng Cục Đường Bộ VN ban hành. Nếu học thuộc hết 18
                đề thi thử bằng lái xe B2 này đồng nghĩa với việc bạn sẽ nắm
                chắc việc thi đậu lý thuyết 100% mà không cần phải lo lắng. Chọn
                đề thi:
              </p>
            </div>
          </div>

          {/* Select type  */}
          <div className='mt-8'>
            <h1 className='font-bold text-[30px] text-center pb-8 text-[#0d5ef4] uppercase'>
              Chọn đề thi
            </h1>
            <div className='flex justify-center gap-4'>
              {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10].map((item, index) => (
                <Link
                  key={index}
                  to={`/theory/test/${item}`}
                  className='text-[20px] font-medium p-4 bg-gray-200 cursor-pointer rounded-lg hover:text-[#0d5ef4]'
                >
                  Đề {item}
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
