import { DataGrid } from '@mui/x-data-grid';
import * as dayjs from 'dayjs';
import React, { useEffect, useState } from 'react';
import { AiFillDelete } from 'react-icons/ai';
import { BiSolidEdit } from 'react-icons/bi';
import SideBar from '../../components/SideBar';
import axiosClient from '../../utils/axiosClient';
import DialogEditUser from './components/DialogEditUser';
import { toastError } from '../../components/Toastify';
import DialogCreateUser from './components/DialogCreateUser';

const ManageUser = () => {
  const [listMembers, setListMember] = useState([]);
  const [openCreateUser, setOpenCreateUser] = useState(false);
  const [openEditUser, setOpenEditUser] = useState(false);
  const [userId, setUserId] = useState();
  const [loading, setLoading] = useState(false);

  const columns = [
    { field: 'memberId', headerName: 'ID', width: 140 },
    { field: 'firstName', headerName: 'Họ', width: 130, editable: true },
    { field: 'lastName', headerName: 'Tên', width: 130 },
    {
      field: 'fullName',
      headerName: 'Họ và tên',
      description: 'This column has a value getter and is not sortable.',
      sortable: false,
      width: 160,
      valueGetter: (params) =>
        `${params.row.firstName || ''} ${params.row.lastName || ''}`,
    },
    {
      field: 'avatarImage',
      headerName: 'Ảnh',
      width: 130,
      renderCell: ({ row: { avatarImage } }) => (
        <div>
          <img
            src={avatarImage}
            alt='avt'
            className='w-[40px] h-[40px] rounded-full object-cover'
          />
        </div>
      ),
    },
    {
      field: 'dateBirth',
      headerName: 'Ngày sinh',
      width: 130,
      renderCell: ({ row: { dateBirth } }) => (
        <p>{dayjs(dateBirth).format('DD/MM/YYYY')}</p>
      ),
    },
    { field: 'phone', headerName: 'Số điện thoại', width: 130 },
    { field: 'email', headerName: 'Email', width: 130 },
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
              setUserId(params.row.memberId);
              setOpenEditUser(true);
            }}
          />
          <AiFillDelete size={20} className='text-red-700 cursor-pointer' />
        </div>
      ),
    },
  ];

  useEffect(() => {
    async function getAllUsers() {
      await axiosClient
        .get(`/members`)
        .then((res) => setListMember(res?.data.data.members))
        .catch((error) => toastError(error?.response?.data?.message));
    }
    getAllUsers();
  }, []);

  async function getAllUsers() {
    await axiosClient
      .get(`/members`)
      .then((res) => setListMember(res?.data.data.members))
      .catch((error) => toastError(error?.response?.data?.message));
  }

  console.log('listMembers: ', listMembers);
  return (
    <div className='flex'>
      <SideBar />
      <div className='flex flex-col mt-[64px] h-[90vh] w-full border rounded overflow-y-auto p-4'>
        <div className='border flex justify-end '>
          <button
            className='p-2 bg-blue-600 text-white rounded-lg'
            onClick={() => setOpenCreateUser(true)}
          >
            Tạo mới
          </button>
        </div>
        <div className='flex-1 w-[100%]'>
          {listMembers && (
            <>
              <DataGrid
                rows={listMembers}
                columns={columns}
                initialState={{
                  pagination: {
                    paginationModel: { page: 0, pageSize: 5 },
                  },
                }}
                getRowId={(row) => row.memberId}
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
        </div>
      </div>
    </div>
  );
};

export default ManageUser;
