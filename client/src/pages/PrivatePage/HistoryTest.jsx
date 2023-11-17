import { Box } from '@mui/material';
import { DataGrid } from '@mui/x-data-grid';
import * as dayjs from 'dayjs';
import React, { useEffect, useState } from 'react';
import { GrFormView } from 'react-icons/gr';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import SideBar from '../../components/SideBar';
import { toastError } from '../../components/Toastify';
import axiosClient from '../../utils/axiosClient';
import Loading from '../../components/Loading';

const HistoryTest = () => {
  const { memberId, email } = useSelector(
    (state) => state.auth.user.accountInfo
  );
  const navigate = useNavigate();
  const [listHistoryTest, setListHistoryTest] = useState();

  const columns = [
    { field: 'id', headerName: 'ID', width: 80 },
    { field: 'title', headerName: 'Đề' },
    {
      field: 'date',
      headerName: 'Ngày làm bài',
      width: 160,
      renderCell: (params) => (
        <div>{dayjs(params?.row?.date).format('DD/MM/YYYY')}</div>
      ),
    },
    {
      field: 'startTime',
      headerName: 'Giờ làm bài',
      width: 160,
      renderCell: (params) => (
        <div>{dayjs(params?.row?.date).format('HH:mm:ss')}</div>
      ),
    },
    { field: 'typeLisence', headerName: 'Loại bằng lái', width: 160 },
    {
      field: 'rightAnswer',
      headerName: 'Số câu đúng',
      type: Number,
      width: 160,
    },
    {
      field: 'wrongAnswer',
      headerName: 'Số câu sai',
      type: Number,
      width: 160,
      renderCell: ({ row: { wrongAnswer } }) => (
        <p className={`${wrongAnswer >= 5 ? 'text-red-400' : ''}`}>
          {wrongAnswer}
        </p>
      ),
    },
    {
      field: 'totalQuestion',
      headerName: 'Tổng',
      type: Number,
      width: 160,
      valueGetter: (params) =>
        `${params.row.rightAnswer} / ${
          params.row.rightAnswer + params.row.wrongAnswer
        }`,
    },
    {
      field: 'result',
      headerName: 'Kết Quả',
      width: 160,
      class: 'text-red-400',
      renderCell: ({ row: { result } }) => (
        <p className={`${result == 'Rớt' ? 'text-green-700' : 'text-red-700'}`}>
          {result}
        </p>
      ),
    },
    {
      field: 'detail',
      headerName: 'Xem chi tiết',
      width: 160,
      class: 'text-red-400',
      renderCell: (params) => (
        <div>
          <GrFormView
            size={24}
            className='text-gray-400 cursor-pointer'
            onClick={async () => {
              const joinDate = dayjs(params.row.date)
                .format('YYYY-MM-DD')
                .concat(' '.concat(dayjs(params.row.date).format('HH:mm:ss')));
              console.log(
                JSON.stringify({
                  email,
                  mockTestId: params.row.idTheoryExam,
                  joinDate,
                })
              );
              await axiosClient
                .post(`/theory/review`, {
                  email,
                  mockTestId: params.row.idTheoryExam,
                  joinDate,
                })
                .then((res) => {
                  console.log(res);
                  navigate(
                    `/theory/result/${email}/${params.row.idTheoryExam}/${joinDate}`
                  );
                })
                .catch((error) => toastError(error?.response?.data?.message));
            }}
          />
        </div>
      ),
    },
  ];

  useEffect(() => {
    async function getHistoryTest() {
      await axiosClient
        .get(`/theory/history/${memberId}`)
        .then((res) => {
          console.log('res: ', res);
          setListHistoryTest(
            res?.data?.data?.map((item, index) => {
              return {
                id: index + 1,
                idTheoryExam: item?.theoryExam?.theoryExamId,
                date: item?.date,
                rightAnswer: item?.totalRightAnswer,
                wrongAnswer: item?.totalQuestion - item?.totalRightAnswer,
                totalQuestion: item?.totalQuestion,
                title: item?.theoryExamDesc,
                typeLisence: item?.theoryExam?.licenseType?.licenseTypeDesc,
                result: item?.isPassed ? 'Đậu' : 'Rớt',
              };
            })
          );
          // toastSuccess(res?.data?.message)
        })
        .catch((error) => toastError(error?.response?.data?.message));
    }
    getHistoryTest();
  }, []);

  console.log('listHistoryTest: ', listHistoryTest);

  return (
    <div className='flex'>
      <SideBar />
      <Box component='main' sx={{ flexGrow: 1, p: 3 }}>
        <div className='h-[80vh] w-full rounded overflow-y-auto mt-[64px]'>
          <div className='w-full'>
            {listHistoryTest && (
              <DataGrid
                rows={listHistoryTest}
                columns={columns}
                initialState={{
                  pagination: {
                    paginationModel: { page: 0, pageSize: 10 },
                  },
                }}
                pageSizeOptions={[5, 10, 20, 50]}
                // checkboxSelection
              />
            )}
          </div>
        </div>
      </Box>
    </div>
  );
};

export default HistoryTest;
