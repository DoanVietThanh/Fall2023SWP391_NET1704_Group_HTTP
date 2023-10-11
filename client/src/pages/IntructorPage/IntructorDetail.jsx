import React from "react";
import Header from "../../components/Header";
import Footer from "../../components/Footer";
import BackgroundSlider from "../../components/BackgroundSlider";
import { AiFillFacebook, AiFillInstagram, AiFillStar, AiFillTwitterSquare } from "react-icons/ai";

const IntructorDetail = () => {
  const url =
    "https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png";
  const breadcrumbs = "Instructor Detail";
  return (
    <div>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      <div className="flex m-20 gap-24">
        <div className="w-[40%]">
          <img
            src="https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/team_1_4.png"
            alt="phuoc"
            className="zoom p-5 w-full h-[700px] object-cover"
          ></img>
        </div>
        <div className="w-[60%] flex flex-col mt-10 gap-8">
          <div className="flex flex-col gap-2 mb-5">
            <div className="font-semibold text-3xl">Lê Xuân Phước</div>
            <div className="font-light text-2xl text-gray-600">Người hướng dẫn</div>
          </div>

          <div className="flex flex-col gap-2">
            <div className="font-semibold text-2xl">Giới thiệu</div>
            <div className="font-light text-xl text-gray-600">
              Lorem ipsum dolor sit amet, consectetur adipisicing elit.
              Doloribus labore, eaque soluta ipsa rem inventore officia adipisci
              mollitia nihil necessitatibus.
            </div>
          </div>

          <div className="flex flex-col gap-2">
            <div className="font-semibold text-2xl">Chuyên môn</div>
            <div className="font-light text-xl text-gray-600">
              Lorem ipsum dolor sit amet, consectetur adipisicing elit.
              Doloribus labore, eaque soluta ipsa rem inventore officia adipisci
              mollitia nihil necessitatibus.
            </div>
          </div>

          <div className="flex flex-col gap-2">
              <div className="font-semibold text-2xl">Đánh giá</div>
              <div className="flex gap-1 items-center">
                <AiFillStar size={26} color="#F6F669"></AiFillStar>
                <AiFillStar size={26} color="#F6F669"></AiFillStar>
                <AiFillStar size={26} color="#F6F669"></AiFillStar>
                <AiFillStar size={26} color="#F6F669"></AiFillStar>
                <AiFillStar size={26} color="#F6F669"></AiFillStar>
            </div>
          </div>

          <div className="flex flex-col gap-2">
            <div className="font-semibold text-2xl">Liên lạc</div>
            <div className="flex gap-5 items-center">
              
              <AiFillFacebook size={34} color="#1976D2"/>
              <AiFillInstagram size={34} color="#1976D2"/>
              <AiFillTwitterSquare size={34} color="#1976D2"/>
            </div>
          </div>

        </div>
      </div>
      <Footer />
    </div>
  );
};

export default IntructorDetail;
