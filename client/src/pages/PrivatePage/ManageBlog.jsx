import dayjs from 'dayjs';
import React, { useState } from 'react';
import SideBar from '../../components/SideBar';
import { DataGrid } from '@mui/x-data-grid';
import CreateBlog from './components/CreateBlog';
import { useEffect } from 'react';
import axios from 'axios';
import { useSelector } from 'react-redux';
import { BiSolidEdit } from 'react-icons/bi';
import { AiFillDelete } from 'react-icons/ai';
import DialogEditBlog from './components/DialogEditBlog';
import axiosClient from '../../utils/axiosClient';
import { toastError, toastSuccess } from '../../components/Toastify';

const ManageBlog = () => {
  const { user, isLoading } = useSelector((state) => state.auth);
  const userRoleId = user.accountInfo.emailNavigation.role.roleId;
  const userId = user.accountInfo.staffId;
  const urlService = process.env.REACT_APP_SERVER_API;

  const [openEditBlog, setOpenEditBlog] = useState(false);
  const [showCreateBlog, setShowCreateBlog] = useState(false);
  const [showManageBlog, setShowManageBlog] = useState(true);
  const [selectedBlog, setSelectedBlog] = useState();

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
          console.log('res: ', res);
          setBlogList(res.data?.data);
        })
        .catch((error) => {
          console.log('error: ', error);
          setBlogList(error?.response?.data?.message);
        });
    }
    getUserBlogList();
  }, []);
  const filteredBlogList =
    userRoleId !== 2
      ? blogList.filter((blog) => blog.staffId === userId)
      : blogList;

  const columns = [
    { field: 'blogId', headerName: 'ID', width: 100 },
    { field: 'title', headerName: 'Tiêu đề', width: 200, editable: true },
    // {
    //   field: 'image',
    //   headerName: 'Ảnh',
    //   width: 130,
    //   renderCell: ({ row }) => (
    //     <div>
    //       <img
    //         src={row.blogImg}
    //         alt='Ảnh'
    //         className='w-[40px] h-[40px] rounded-full object-cover'
    //       />
    //     </div>
    //   ),
    // },

    {
      field: 'createDate',
      headerName: 'Ngày đăng',
      width: 130,
      renderCell: ({ row }) => (
        <p>{dayjs(row.createDate).format('DD/MM/YYYY')}</p>
      ),
    },
    {
      field: 'lastModifiedDate',
      headerName: 'Ngày chỉnh sửa gần nhất',
      width: 200,
      renderCell: ({ row }) => (
        <p>{dayjs(row.lastModifiedDate).format('DD/MM/YYYY')}</p>
      ),
    },

    {
      field: 'edit',
      headerName: 'Chỉnh sửa',
      width: 130,
      renderCell: (params) => (
        <div className='flex gap-4 items-center'>
          <BiSolidEdit
            size={20}
            className='text-yellow-700 cursor-pointer'
            onClick={() => {
              setSelectedBlog(params?.row?.blogId);
              setOpenEditBlog(true);
            }}
          />
          <AiFillDelete
            size={20}
            className='text-red-700 cursor-pointer'
            onClick={() => handleDeleteBlog(params.row.blogId)}
          />
        </div>
      ),
    },
  ];
  const handleDeleteBlog = async (blogId) => {
    await axiosClient
      .delete(`/blog/${blogId}`)
      .then((res) => {
        console.log(res);
        toastSuccess(res?.data?.message);
        getBlogList();
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };
  const getBlogList = async () => {
    await axiosClient
      .get(`/blog`)
      .then((res) => {
        console.log(res);
        setBlogList(res?.data?.data);
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };
  return (
    <div className='flex'>
      <SideBar />
      <div className='flex flex-col mt-[64px] h-[90vh] w-full overflow-y-auto mx-6'>
        <div className='flex gap-8 m-8'>
          <button className='btn' onClick={handleShowManageBlog}>
            Bài đăng
          </button>
          <button className='btn' onClick={handleShowCreateBlog}>
            Tạo bài đăng
          </button>
        </div>
        {showManageBlog && (
          <div style={{ height: 400, width: '100%' }}>
            <DataGrid
              rows={filteredBlogList}
              columns={columns}
              getRowId={(row) => row.blogId}
            />
          </div>
        )}
        {showCreateBlog && <CreateBlog />}

        {selectedBlog && (
          <DialogEditBlog
            open={openEditBlog}
            setOpen={setOpenEditBlog}
            selectedBlog={selectedBlog}
          />
        )}
      </div>
    </div>
  );
};

export default ManageBlog;
