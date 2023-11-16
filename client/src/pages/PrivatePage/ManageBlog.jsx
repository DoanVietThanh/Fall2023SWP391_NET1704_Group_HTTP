import dayjs from "dayjs";
import React, { useState } from "react";
import SideBar from "../../components/SideBar";
import { DataGrid } from "@mui/x-data-grid";
import CreateBlog from "./components/CreateBlog";
import { useEffect } from "react";
import axios from "axios";
import { useSelector } from "react-redux";
import { BiSolidEdit } from "react-icons/bi";
import { AiFillDelete } from "react-icons/ai";
import DialogEditBlog from "./components/DialogEditBlog";

const ManageBlog = () => {
  const { user, isLoading } = useSelector((state) => state.auth);
  const userRoleId = user.accountInfo.emailNavigation.role.roleId;
  const userId = user.accountInfo.staffId;
  const urlService = process.env.REACT_APP_SERVER_API;

  const [openEditBlog, setOpenEditBlog] = useState(false);
  const [showCreateBlog, setShowCreateBlog] = useState(false);
  const [showManageBlog, setShowManageBlog] = useState(true);

  const handleShowCreateBlog = () => {
    setShowCreateBlog(true);
    setShowManageBlog(false);
  };
  const handleShowManageBlog = () => {
    setShowManageBlog(true);
    setShowCreateBlog(false);
  };
  const [blogList, setBlogList] = useState([]);
  useEffect(() => {
    async function getUserBlogList() {
      await axios
        .get(`${urlService}/blog`)
        .then((res) => {
          console.log("res: ", res);
          setBlogList(res.data?.data);
        })
        .catch((error) => {
          console.log("error: ", error);
          setBlogList(error?.response?.data?.message);
        });
    }
    getUserBlogList();
  }, []);
    const filteredBlogList = userRoleId !== 2 ? blogList.filter((blog) => blog.staffId === userId) : blogList;
  // const listBlog = [
  //   {
  //     blogId: 1,
  //     title: "Bài viết số 1",
  //     blogImg: "/img/avtThanh.jpg",
  //     datePost: "2023-10-25",
  //   },
  //   {
  //     blogId: 2,
  //     title: "Bài viết số 2",
  //     blogImg: "/img/avtThanh.jpg",
  //     datePost: "2023-10-26",
  //   },
  //   {
  //     blogId: 3,
  //     title: "Bài viết số 3",
  //     blogImg: "/img/avtThanh.jpg",
  //     datePost: "2023-10-27",
  //   },
  //   {
  //     blogId: 4,
  //     title: "Bài viết số 4",
  //     blogImg: "/img/avtThanh.jpg",
  //     datePost: "2023-10-28",
  //   },
  //   {
  //     blogId: 5,
  //     title: "Bài viết số 5",
  //     blogImg: "/img/avtThanh.jpg",
  //     datePost: "2023-10-29",
  //   },
  // ];
  const columns = [
    { field: "blogId", headerName: "ID", width: 100 },
    { field: "title", headerName: "Tiêu đề", width: 200, editable: true },
    {
      field: "image",
      headerName: "Ảnh",
      width: 130,
      renderCell: ({ row }) => (
        <div>
          <img
            src={row.blogImg}
            alt="Ảnh"
            className="w-[40px] h-[40px] rounded-full object-cover"
          />
        </div>
      ),
    },
    {
      field: "comments",
      headerName: "Số bình luận",
      width: 200,
      renderCell: ({ row }) => (
       <p>{row.comments.length}</p>
      ),
    },
    {
      field: "createDate",
      headerName: "Ngày đăng",
      width: 130,
      renderCell: ({ row }) => (
        <p>{dayjs(row.createDate).format("DD/MM/YYYY")}</p>
      ),
    },
    {
      field: "lastModifiedDate",
      headerName: "Ngày chỉnh sửa gần nhất",
      width: 200,
      renderCell: ({ row }) => (
        <p>{dayjs(row.lastModifiedDate).format("DD/MM/YYYY")}</p>
      ),
    },

    {
      field: "edit",
      headerName: "Chỉnh sửa",
      width: 130,
      renderCell: (params) => (
        <div className="flex gap-4 items-center">
          <BiSolidEdit
            size={20}
            className="text-yellow-700 cursor-pointer"
            onClick={() => {
              // setUserId(params.row.memberId);
               setOpenEditBlog(true);
            }}
          />
          <AiFillDelete size={20} className="text-red-700 cursor-pointer" />
        </div>
      ),
    },
  ];
  return (
    <div className="flex">
      <SideBar />
      <div className="flex flex-col mt-[64px] h-[90vh] w-full overflow-y-auto mx-6">
        <div className="flex gap-8 m-8">
          <button className="btn" onClick={handleShowManageBlog}>
            Bài đăng
          </button>
          <button className="btn" onClick={handleShowCreateBlog}>
            Tạo bài đăng
          </button>
        </div>
        {showManageBlog && (
          <div style={{ height: 400, width: "100%" }}>
            <DataGrid
              rows={filteredBlogList}
              columns={columns}
              getRowId={(row) => row.blogId}
            />
          </div>
        )}
        {showCreateBlog && <CreateBlog />}
        <DialogEditBlog open={openEditBlog} setOpen={setOpenEditBlog}/>
      </div>
    </div>
  );
};

export default ManageBlog;
