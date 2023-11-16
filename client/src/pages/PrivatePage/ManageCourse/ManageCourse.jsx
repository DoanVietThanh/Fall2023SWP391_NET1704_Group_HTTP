import { DataGrid } from '@mui/x-data-grid';
import React, { useCallback, useEffect, useState } from 'react';
import { AiFillDelete } from 'react-icons/ai';
import { BiHide, BiSolidEdit } from 'react-icons/bi';
import { GrFormView } from 'react-icons/gr';
import SideBar from '../../../components/SideBar';
import { toastError, toastSuccess } from '../../../components/Toastify';
import * as dayjs from 'dayjs';
import axiosClient from '../../../utils/axiosClient';
import DialogCreateCourse from './components/DialogCreateCourse';
import DialogEditCourse from './components/DialogEditCourse';

import { Menu, MenuItem } from '@mui/material';
import { BsThreeDotsVertical } from 'react-icons/bs';
import DialogCoursePackage from './components/DialogCoursePackage';
import DialogCurriculum from './components/DialogCurriculum';
import DialogAddMentor from './components/DialogAddMentor';

const ManageCourse = () => {
  const [listCourses, setListCourses] = useState([]);
  const [selectedIdCourse, setSelectedIdCourse] = useState('');
  const [anchorEl, setAnchorEl] = useState(null);
  const [openCreateCourse, setOpenCreateCourse] = useState(false);
  const [openEditCourse, setOpenEditCourse] = useState(false);
  const [openCoursePackage, setOpenCoursePackage] = useState(false);
  const [openCurriculum, setOpenCurriculum] = useState(false);
  const [openAddMentor, setOpenAddMentor] = useState(false);

  const openMenu = Boolean(anchorEl);
  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
    event.stopPropagation();
  };
  const handleClose = () => setAnchorEl(null);

  const columns = [
    { field: 'courseId', headerName: 'ID', width: 140 },
    { field: 'courseTitle', headerName: 'Tên khóa học', width: 200 },
    { field: 'courseDesc', headerName: 'Mô tả khóa học', width: 140 },
    {
      field: 'startDate',
      headerName: 'Ngày bắt đầu',
      width: 140,
      renderCell: (params) => {
        return <div>{dayjs(params.row.startDate).format('DD/MM/YYYY')}</div>;
      },
    },
    { field: 'isActive', headerName: 'Trạng thái', width: 80 },
    {
      field: 'totalHoursRequired',
      headerName: 'Tổng giờ yêu cầu',
      width: 140,
    },
    {
      field: 'totalKmRequired',
      headerName: 'Tổng km yêu cấu',
      width: 140,
    },
    { field: 'totalMonth', headerName: 'Tổng tháng', width: 140 },
    {
      field: 'licenseTypeId',
      headerName: 'Loại bằng',
      width: 130,
    },
    {
      field: 'isHiden',
      headerName: 'Ẩn',
      width: 80,
      renderCell: (params) => (
        <div>
          {params.row.isActive ? (
            <BiHide
              className='cursor-pointer text-orange-400'
              size={24}
              onClick={() => handleHidenCourse(params.row.courseId)}
            />
          ) : (
            <GrFormView
              className='cursor-pointer text-gray-400'
              size={24}
              onClick={() => handleUnHidenCourse(params.row.courseId)}
            />
          )}
        </div>
      ),
    },
    {
      field: 'edit',
      headerName: 'Chỉnh sửa',
      width: 130,
      renderCell: (params, record) => {
        return (
          <div className='flex gap-4 items-center'>
            <BiSolidEdit size={20} className='text-yellow-700 cursor-pointer' />
            <AiFillDelete size={20} className='text-red-700 cursor-pointer' />
            <div>
              <BsThreeDotsVertical
                id='basic-button'
                aria-controls={openMenu ? 'basic-menu' : undefined}
                aria-haspopup='true'
                aria-expanded={openMenu ? 'true' : undefined}
                onClick={(event) => {
                  setAnchorEl(event.currentTarget);
                  setSelectedIdCourse(params.row.courseId);
                }}
                size={20}
                className='cursor-pointer'
              />
              <Menu
                id='basic-menu'
                anchorEl={anchorEl}
                open={openMenu}
                onClose={handleClose}
                MenuListProps={{
                  'aria-labelledby': 'basic-button',
                }}
              >
                <MenuItem
                  onClick={() => {
                    setOpenCoursePackage(true);
                    handleClose();
                  }}
                >
                  Quản lí Gói khóa học
                </MenuItem>
                <MenuItem
                  onClick={() => {
                    setOpenCurriculum(true);
                    handleClose();
                  }}
                >
                  Quản lí Chương trình
                </MenuItem>
                <MenuItem
                  onClick={() => {
                    setOpenAddMentor(true);
                    handleClose();
                  }}
                >
                  Thêm giảng viên
                </MenuItem>
              </Menu>
            </div>
          </div>
        );
      },
    },
  ];

  useEffect(() => {
    async function getAllCourses() {
      await axiosClient
        .get(`/courses`)
        .then((res) => {
          console.log(res);
          setListCourses(res?.data?.data);
        })
        .catch((error) => toastError(error?.response?.data?.message));
    }
    getAllCourses();
  }, []);

  const handleHidenCourse = async (idCourse) => {
    await axiosClient
      .put(`/courses/${idCourse}/hide`)
      .then((res) => {
        console.log(res);
        toastSuccess(res?.data?.message);
        getAllCourses();
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };

  const handleUnHidenCourse = async (idCourse) => {
    await axiosClient
      .put(`/courses/${idCourse}/unhide`)
      .then((res) => {
        console.log(res);
        toastSuccess(res?.data?.message);
        getAllCourses();
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };

  const handleDeleteCourse = async (idCourse) => {
    await axiosClient
      .delete(`/courses/${idCourse}`)
      .then((res) => {
        console.log(res);
        toastSuccess(res?.data?.message);
        getAllCourses();
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };

  const getAllCourses = async () => {
    await axiosClient
      .get(`/courses`)
      .then((res) => {
        console.log(res);
        setListCourses(res?.data?.data);
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };

  // console.log('test', test);
  return (
    <div className='flex'>
      <SideBar />
      <div className='flex flex-col mt-[64px] h-[90vh] w-full border rounded overflow-y-auto p-4'>
        <div className='flex justify-end '>
          <button
            className='p-2 bg-blue-600 text-white rounded-lg'
            onClick={() => setOpenCreateCourse(true)}
          >
            Tạo mới
          </button>
        </div>
        <div className='flex-1 w-[100%]'>
          {listCourses ? (
            <DataGrid
              rows={listCourses}
              columns={columns}
              initialState={{
                pagination: {
                  paginationModel: { page: 0, pageSize: 10 },
                },
              }}
              getRowId={(row) => row.courseId}
              pageSizeOptions={[10, 15]}
            />
          ) : (
            <h1>Không tìm thấy</h1>
          )}
        </div>
      </div>

      {openCreateCourse && (
        <DialogCreateCourse
          open={openCreateCourse}
          setOpen={setOpenCreateCourse}
          getAllCourses={getAllCourses}
        />
      )}

      {openEditCourse && (
        <DialogEditCourse open={openEditCourse} setOpen={setOpenEditCourse} />
      )}

      {openCoursePackage && (
        <DialogCoursePackage
          open={openCoursePackage}
          setOpen={setOpenCoursePackage}
          selectedIdCourse={selectedIdCourse}
          getAllCourses={getAllCourses}
        />
      )}

      {openCurriculum && (
        <DialogCurriculum
          open={openCurriculum}
          setOpen={setOpenCurriculum}
          selectedIdCourse={selectedIdCourse}
        />
      )}

      {openAddMentor && (
        <DialogAddMentor
          open={openAddMentor}
          setOpen={setOpenAddMentor}
          selectedIdCourse={selectedIdCourse}
        />
      )}
    </div>
  );
};

export default ManageCourse;
