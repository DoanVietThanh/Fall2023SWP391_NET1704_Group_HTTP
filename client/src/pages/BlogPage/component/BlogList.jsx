import axios from "axios";
import React, { useEffect, useState } from "react";
import { AiOutlineArrowRight, AiOutlineClockCircle } from "react-icons/ai";
import dayjs from "dayjs";
import { BiBookBookmark } from "react-icons/bi";
import { Link } from "react-router-dom";
import { toastError } from "../../../components/Toastify";
import theme from "../../../theme";
import { BsPerson, BsTags } from "react-icons/bs";
import axiosClient from "./../../../utils/axiosClient";

const BlogList = () => {
  const [listBlog, setListBlog] = useState([]);
  const [listStaff, setListStaff] = useState([]);
  const urlService = process.env.REACT_APP_SERVER_API;
  useEffect(() => {
    async function getListBlog() {
      await axios
        .get(`${urlService}/blog`)
        .then((res) => {
          console.log("res: ", res);
          setListBlog(res.data?.data);
        })
        .catch((error) => {
          console.log("error: ", error);
          toastError(error?.response?.data?.message);
        });
      // await axiosClient
      // .get(`${urlService}/staff/${staffId}`)
      // .then((res) => {
      //   console.log("res: ", res);
      //   setStaffName(res.data?.firstName);
      // })
      // .catch((error) => {
      //   console.log("error: ", error);
      //   toastError(error?.response?.data?.message);
      // });
    }
    getListBlog();
  }, []);
  return (
    <div className="flex flex-col gap-10">
      {listBlog?.map((blog, index) => (
        <div className="border drop-shadow-md rounded-lg p-20 ">
          <div className="center pb-10">
            <img
              src={blog?.img}
              alt="blogPic"
              className="rounded-lg w-[800px] h-[380px] "
            />
          </div>
          <div className={`flex text-lg pb-8 text-[${theme.color.mainColor}] `}>
            <div className="flex items-center gap-3 pr-4">
              <BsPerson size={24} /> by {}
            </div>
            <div className="flex items-center gap-3 border-l-[2px] px-4">
              <AiOutlineClockCircle size={24} />{" "}
              {dayjs(blog?.createDate).format("DD/MM/YYYY")}
            </div>
            <div className="flex items-center gap-3 border-l-[2px] px-4">
              <BsTags size={24} />{" "}
              {blog?.tags?.map((tag, index) => (
                <span key={tag.tagId}>{tag.tagName}</span>
              ))}
            </div>
          </div>
          <div className="text-4xl font-bold pb-8">{blog?.title}</div>
          <div
            className="font-light text-lg pb-8"
            dangerouslySetInnerHTML={{ __html: `${blog?.content}` }}
          ></div>
          <Link to={`/blog/${blog?.blogId}`}>
            <button className="btn flex items-center gap-2">
              READ MORE <AiOutlineArrowRight />
            </button>
          </Link>
        </div>
      ))}
    </div>
  );
};

export default BlogList;
