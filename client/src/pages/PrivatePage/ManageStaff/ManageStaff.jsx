import { DataGrid } from '@mui/x-data-grid';
import * as dayjs from 'dayjs';
import React, { useEffect, useState } from 'react';
import { AiFillDelete } from 'react-icons/ai';
import { BiSolidEdit } from 'react-icons/bi';
import SideBar from '../../../components/SideBar';
import { toastError, toastSuccess } from '../../../components/Toastify';
import axiosClient from '../../../utils/axiosClient';
import DialogStaff from './components/DialogStaff';

const ManageStaff = () => {
  const [listStaff, setListStaff] = useState([]);
  const [openDialogStaff, setOpenDialogStaff] = useState(false);
  const [actionDialog, setActionDialog] = useState('');
  const [staffId, setStaffId] = useState();

  const columns = [
    { field: 'staffId', headerName: 'ID', width: 140 },
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
      field: 'addressStaff',
      headerName: 'Địa chỉ',
      width: 230,

      valueGetter: (params) =>
        `${params?.row?.address?.street}, ${params?.row?.address?.district}, ${params?.row?.address?.city}`,
    },
    {
      field: 'avatarImage',
      headerName: 'Ảnh',
      width: 130,
      renderCell: ({ row: { avatarImage } }) => (
        <div>
          <img
            src='/img/avtThanh.jpg'
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
              setOpenDialogStaff(true);
              setActionDialog('edit');
              setStaffId(params?.row?.staffId);
            }}
          />
          <AiFillDelete
            size={20}
            className='text-red-700 cursor-pointer'
            onClick={() => handleDeleteStaff(params?.row?.staffId)}
          />
        </div>
      ),
    },
  ];

  const handleDeleteStaff = async (staffId) => {
    console.log('staffId: ', staffId);
    await axiosClient
      .delete(`/staffs/${staffId}/delete`)
      .then((res) => {
        console.log(res);
        toastSuccess(res?.data?.message);
        getAllStaffs();
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };

  useEffect(() => {
    async function getAllStaffs() {
      await axiosClient
        .get(`/staffs`)
        .then((res) => setListStaff(res?.data.data))
        .catch((error) => toastError(error?.response?.data?.message));
    }
    getAllStaffs();
  }, []);

  async function getAllStaffs() {
    await axiosClient
      .get(`/staffs`)
      .then((res) => {
        setListStaff(res?.data?.data);
      })
      .catch((error) => toastError(error?.response?.data?.message));
  }

  console.log('listStaff: ', listStaff);

  return (
    <div className='flex'>
      <SideBar />
      <div className='flex flex-col mt-[64px] h-[90vh] w-full border rounded overflow-y-auto p-4'>
        <div className='border flex justify-end '>
          <button
            className='p-2 bg-blue-600 text-white rounded-lg'
            onClick={() => {
              setOpenDialogStaff(true);
              setActionDialog('create');
            }}
          >
            Tạo mới
          </button>
        </div>
        <div className='flex-1 w-[100%] h-[30vh]'>
          {listStaff && (
            <>
              <DataGrid
                rows={listStaff}
                columns={columns}
                initialState={{
                  pagination: {
                    paginationModel: { page: 0, pageSize: 10 },
                  },
                }}
                getRowId={(row) => row.staffId}
                pageSizeOptions={[10, 20, 50, 100]}
              />
            </>
          )}

          {actionDialog && (
            <DialogStaff
              open={openDialogStaff}
              setOpen={setOpenDialogStaff}
              actionDialog={actionDialog}
              getAllStaffs={getAllStaffs}
              staffId={staffId}
            />
          )}
        </div>
      </div>
    </div>
  );
};

export default ManageStaff;
