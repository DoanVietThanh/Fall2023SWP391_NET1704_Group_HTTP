import dayjs from "dayjs";
import React from "react";
import SideBar from "../../components/SideBar";
import { DataGrid } from "@mui/x-data-grid";

const ManageBlog = () => {
  const listBlog = [
    {
      blogId: 1,
      title: "Bài viết số 1",
      blogImg: "/img/avtThanh.jpg",
      datePost: "2023-10-25",
    },
    {
      blogId: 2,
      title: "Bài viết số 2",
      blogImg: "/img/avtThanh.jpg",
      datePost: "2023-10-26",
    },
    {
      blogId: 3,
      title: "Bài viết số 3",
      blogImg: "/img/avtThanh.jpg",
      datePost: "2023-10-27",
    },
    {
      blogId: 4,
      title: "Bài viết số 4",
      blogImg: "/img/avtThanh.jpg",
      datePost: "2023-10-28",
    },
    {
      blogId: 5,
      title: "Bài viết số 5",
      blogImg: "/img/avtThanh.jpg",
      datePost: "2023-10-29",
    },
  ];
  const columns = [
    { field: "blogId", headerName: "ID", width: 140 },
    { field: "title", headerName: "Tiêu đề", width: 130, editable: true },
    {
      field: "blogImg",
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
      field: "datePost",
      headerName: "Ngày đăng",
      width: 130,
      renderCell: ({ row }) => (
        <p>{dayjs(row.datePost).format("DD/MM/YYYY")}</p>
      ),
    },

    {
      field: "edit",
      headerName: "Chỉnh sửa",
      width: 130,
      //   renderCell: (params) => (
      //     <div className='flex gap-4 items-center'>
      //       <BiSolidEdit
      //         size={20}
      //         className='text-yellow-700 cursor-pointer'
      //         onClick={() => {
      //           setUserId(params.row.memberId);
      //           setOpenEditUser(true);
      //         }}
      //       />
      //       <AiFillDelete size={20} className='text-red-700 cursor-pointer' />
      //     </div>
      //   ),
    },
  ];
  return (
    <div className="flex">
      <SideBar />
      <div className="flex flex-col mt-[64px] h-[90vh] w-full border rounded overflow-y-auto p-4">
        <div className="border flex justify-end ">
          <button
            className="p-2 bg-blue-600 text-white rounded-lg"
            // onClick={() => setOpenCreateUser(true)}
          >
            Tạo mới
          </button>
        </div>
        <div style={{ height: 400, width: "100%" }}>
          <DataGrid
            rows={listBlog}
            columns={columns}
            getRowId={(row) => row.blogId}
          />
        </div>
        {/* <div className='flex-1 w-[100%]'>
          {listBlog && (
            <>
              <DataGrid
                rows={listBlog}
                columns={columns}
                initialState={{
                  pagination: {
                    paginationModel: { page: 0, pageSize: 5 },
                  },
                }}
                getRowId={(row) => row.blogId}
                pageSizeOptions={[5, 10]}
              />
              {userId && (
                <DialogEditUser
                  open={openEditUser}
                  setOpen={setOpenEditUser}
                  userId={userId}
                  setLoading={setLoading}
                  loading={loading}
                  getAllUsers={getAllUsers}
                />
              )}
            </>
          )}

          {openCreateUser && (
            <DialogCreateUser
              open={openCreateUser}
              setOpen={setOpenCreateUser}
              getAllUsers={getAllUsers}
            />
          )}
        </div> */}
      </div>
    </div>
  );
};

export default ManageBlog;
