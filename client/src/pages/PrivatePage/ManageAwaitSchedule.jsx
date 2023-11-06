import { DataGrid } from '@mui/x-data-grid';
import * as dayjs from 'dayjs';
import React, { useEffect, useState } from 'react';
import { AiFillDelete } from 'react-icons/ai';
import SideBar from '../../components/SideBar';
import axiosClient from '../../utils/axiosClient';

import { GrFormView } from 'react-icons/gr';
import { useNavigate } from 'react-router-dom';
import { toastError } from '../../components/Toastify';
const ManageAwaitSchedule = () => {
  const navigate = useNavigate();
  const [userId, setUserId] = useState();

  const [listAwait, setListAwait] = useState();

  const columns = [
    { field: 'staffId', headerName: 'Mentor ID', width: 170 },
    { field: 'firstName', headerName: 'Họ', width: 130, editable: true },
    { field: 'lastName', headerName: 'Tên', width: 130 },
    {
      field: 'dateBirth',
      headerName: 'Ngày sinh',
      width: 130,
      renderCell: (params) => (
        <div>{dayjs(params.row.dateBirth).format('DD/MM/YYYY')}</div>
      ),
    },
    {
      field: 'isActive',
      headerName: 'Hoạt động',
      with: 130,
      renderCell: (params) => <div>{JSON.stringify(params.row.isActive)}</div>,
    },
    { field: 'phone', headerName: 'Số điện thoại', width: 130 },
    { field: 'email', headerName: 'Email', width: 130 },
    {
      field: 'view',
      headerName: 'Chi tiết',
      width: 130,
      renderCell: (params) => (
        <div className='flex gap-4 items-center'>
          <GrFormView
            size={24}
            className='text-green-700 cursor-pointer'
            onClick={() => {
              setUserId(params.row.staffId);
              navigate(`/manage-await-schedule/${params.row.staffId}`);
            }}
          />
        </div>
      ),
    },
    {
      field: 'delete',
      headerName: 'Xóa',
      renderCell: (params) => (
        <div>
          <AiFillDelete size={20} className='text-red-700 cursor-pointer' />
        </div>
      ),
    },
  ];

  useEffect(() => {
    async function getAllUsers() {
      await axiosClient
        .get(`/teaching-schedules/await`)
        .then((res) => setListAwait(res?.data?.data))
        .catch((error) => toastError(error?.response?.data?.message));
    }
    getAllUsers();
  }, []);

  console.log(userId);

  return (
    <div className='flex'>
      <SideBar />
      <div className='flex flex-col mt-[64px] h-[90vh] w-full border rounded overflow-y-auto p-4'>
        <div className='flex-1 w-[100%]'>
          {listAwait && (
            <>
              <DataGrid
                rows={listAwait}
                columns={columns}
                initialState={{
                  pagination: {
                    paginationModel: { page: 0, pageSize: 5 },
                  },
                }}
                getRowId={(row) => row.staffId}
                pageSizeOptions={[5, 10]}
              />
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default ManageAwaitSchedule;
