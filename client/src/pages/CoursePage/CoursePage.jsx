import React from "react";
import Header from "../../components/Header";
import Footer from "../../components/Footer";
import { Link } from "react-router-dom";
import BackgroundSlider from "../../components/BackgroundSlider";
import { BsFillArrowRightSquareFill } from "react-icons/bs";
import theme from "../../theme";
import { BiSearch } from "react-icons/bi";

const courseList = [
  {
    id: "courseItem1",
    src: "/img/course1.jpg",
    licenseType: "B1",
    duration: 8,
    registerMember: 100,
  },
  {
    id: "courseItem2",
    src: "/img/course2.jpg",
    licenseType: "B1",
    duration: 12,
    registerMember: 100,
  },
  {
    id: "courseItem3",
    src: "/img/course3.jpg",
    licenseType: "B2",
    duration: 16,
    registerMember: 25,
  },
];

const CoursePage = () => {
  const url =
    "https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png";
  const breadcrumbs = "Course";
  return (
    <>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />

      <form className="flex justify-center my-20">
        <input
          placeholder="Nhập"
          className="w-[40%] rounded-l-lg bg-slate-100 outline-none pl-5 py-5 text-lg"
        />
        <button
          className={`center rounded-r-lg bg-[${theme.color.mainColor}] w-[5%] p-3 hover:bg-blue-900`}
        >
          <BiSearch size={24} color="white" />
        </button>
      </form>

      <div className="m-20">
        <div className="flex flex-col gap-28">
          {courseList.map((course, index) => (
            <div className="shadow-lg hover:shadow-none">
              <div className="zoom w-[50%]">
                <div className="relative">
                  <img
                    src={course.src}
                    alt="course"
                    className="imgClipPath w-full h-[400px] object-cover"
                  />
                  <div className="flex flex-col gap-3 absolute top-0 right-0 translate-x-2/3 pl-20 ">
                    <div className="font-semibold text-4xl leading-snug pt-5">
                      Khóa học thực hành bằng lái {course.licenseType}
                    </div>
                    <Link to={"/course/detail"}>
                      <button className="btn">Chi tiết khóa học</button>
                    </Link>
                    <div className="pl-56 pt-3 flex flex-col gap-10">
                      <div className="flex gap-5 text-xl items-center">
                        <BsFillArrowRightSquareFill
                          size={34}
                          color="#0d5ef4"
                          className="iconHover"
                        />
                        <div className="flex flex-col gap 5">
                          <div>Duration: {course.duration} slots</div>
                        </div>
                      </div>

                      <div className="flex gap-5 text-xl items-center">
                        <BsFillArrowRightSquareFill
                          size={34}
                          color="#0d5ef4"
                          className="iconHover"
                        />
                        <div className="flex flex-col gap 5">
                          <div>Number of register: {course.registerMember}</div>
                        </div>
                      </div>

                      <div className="flex gap-5 text-xl items-center">
                        <BsFillArrowRightSquareFill
                          size={34}
                          color="#0d5ef4"
                          className="iconHover"
                        />
                        <div className="flex flex-col gap 5">
                          <div>Number of register: {course.registerMember}</div>
                        </div>
                      </div>
                    </div>
                  </div>
              </div>
            </div>
            </div>
          ))}
        </div>
      </div>
      <Footer />
    </>
  );
};

export default CoursePage;
