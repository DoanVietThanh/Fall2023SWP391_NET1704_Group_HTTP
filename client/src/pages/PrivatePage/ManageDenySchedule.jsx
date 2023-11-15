import React, { useEffect, useState } from 'react';
import SideBar from '../../components/SideBar';
import axiosClient from '../../utils/axiosClient';
import { toastError, toastSuccess } from '../../components/Toastify';
import { DataGrid } from '@mui/x-data-grid';
import { AiFillCheckCircle, AiFillDelete } from 'react-icons/ai';
import * as dayjs from 'dayjs';
import { useNavigate } from 'react-router-dom';

const ManageDenySchedule = () => {
  const navigate = useNavigate();
  const [rows, setRows] = useState(null);
  const columns = [
    {
      field: 'rollCallBookId',
      headerName: 'ID RollCallBook',
      width: 130,
      renderCell: (params) => <div>{params?.row?.rollCallBookId}</div>,
    },
    {
      field: 'coursePackageDesc',
      headerName: 'Gói khóa học',
      width: 230,
      renderCell: (params) => (
        <div>
          {params?.row?.teachingSchedule?.coursePackage?.coursePackageDesc}
        </div>
      ),
    },
    {
      field: 'staffFullname',
      headerName: 'Tên Giảng viên',
      width: 130,
      valueGetter: (params) =>
        `${params?.row?.teachingSchedule?.staff?.firstName} ${params?.row?.teachingSchedule?.staff?.lastName}`,
    },
    {
      field: 'memberFullname',
      headerName: 'Tên học viên',
      width: 200,
      // renderCell: (params) => <div>{params?.row?.member?.firstName}</div>,
      valueGetter: (params) =>
        `${params?.row?.member?.firstName} ${params?.row?.member?.lastName}`,
    },

    {
      field: 'email',
      headerName: 'Email học viên',
      width: 200,
      renderCell: (params) => <div>{params?.row?.member?.email}</div>,
    },
    {
      field: 'phone',
      headerName: 'Sđt học viên',
      width: 130,
      renderCell: (params) => <div>{params?.row?.member?.phone}</div>,
    },
    {
      field: 'slotId',
      headerName: 'Slot',
      width: 50,
      valueGetter: (params) => `${params?.row?.teachingSchedule?.slotId}`,
    },
    {
      field: 'teachingDate',
      headerName: 'Ngày học',
      width: 100,
      renderCell: (params) => (
        <div>
          {dayjs(params?.row?.teachingSchedule?.teachingDate).format(
            'DD/MM/YYYY'
          )}
        </div>
      ),
    },
    {
      field: 'cancelMessage',
      headerName: 'Lí do từ chối',
      width: 260,
      valueGetter: (params) => `${params?.row?.cancelMessage}`,
    },

    {
      field: 'action',
      headerName: 'Duyệt',
      width: 260,
      renderCell: (params) => (
        <div className='flex justify-center gap-4'>
          <div
            onClick={() =>
              handleDeleteApproveCancel(params?.row?.rollCallBookId)
            }
            className='cursor-pointer '
          >
            <AiFillCheckCircle size={24} className='text-green-400' />
          </div>
          <div
            onClick={() => handleDeny(params?.row?.rollCallBookId)}
            className='cursor-pointer '
          >
            <AiFillDelete size={24} className='text-red-400' />
          </div>
        </div>
      ),
    },
  ];

  useEffect(() => {
    async function getCancelRcb() {
      await axiosClient
        .get(`/rollcallbooks/cancel`)
        .then((res) => setRows(res?.data?.data))
        .catch((error) => {
          toastError(error?.response?.data?.message);
        });
    }
    getCancelRcb();
  }, []);

  console.log('rows: ', rows);

  const handleDeny = async (rollCallBookId) => {
    console.log('rollCallBookId: ', rollCallBookId);
    const response = await axiosClient
      .get(`/rollcallbooks/${rollCallBookId}/deny-cancel`)
      .then((res) => {
        console.log('res: ', res);
        toastSuccess(res?.data?.message);
        // getData();
        window.location.reload();
      })
      .catch((error) => {
        console.log(error);
        toastError(error?.response?.data?.message);
      });
    console.log(response);
  };

  const handleDeleteApproveCancel = async (rollCallBookId) => {
    console.log('rollCallBookId: ', rollCallBookId);
    const response = await axiosClient
      .delete(`/rollcallbooks/${rollCallBookId}/approve-cancel`)
      .then((res) => {
        console.log('res: ', res);
        toastSuccess(res?.data?.message);
        // getData();
        window.location.reload();
      })
      .catch((error) => {
        toastError(error?.response?.data?.message);
      });
    console.log(response);
  };

  async function getData() {
    await axiosClient
      .get(`/rollcallbooks/cancel`)
      .then((res) => {
        console.log('res: ', res);
        if (res?.data?.data == null) {
          setRows([]);
        } else {
          setRows(res?.data?.data);
        }
      })
      .catch((error) => toastError(error?.response?.data?.message));
  }

  return (
    <div className='flex'>
      <SideBar />
      <div className='flex flex-col mt-[64px] h-[90vh] w-full border rounded overflow-y-auto p-4'>
        {rows && (
          <DataGrid
            rows={rows}
            columns={columns}
            initialState={{
              pagination: {
                paginationModel: { page: 0, pageSize: 5 },
              },
            }}
            pageSizeOptions={[5, 10, 20, 50, 100]}
            getRowId={(row) => row.rollCallBookId}
            {...rows}
          />
        )}
      </div>
    </div>
  );
};

export default ManageDenySchedule;
