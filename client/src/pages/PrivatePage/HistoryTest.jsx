import React from 'react';
import { DataGrid } from '@mui/x-data-grid';

const columns = [
  { field: 'id', headerName: 'ID', width: 80 },
  { field: 'title', headerName: 'Đề' },
  { field: 'date', headerName: 'Ngày làm bài', width: 160 },
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
      <p className={`${result === 'Rớt' ? 'text-red-700' : 'text-green-500'}`}>
        {result}
      </p>
    ),
  },
];

const rows = [
  {
    id: 1,
    title: 'Đề 1',
    typeLisence: 'B1',
    date: '2023-10-07',
    rightAnswer: 34,
    wrongAnswer: 1,
    totalQuestion: 35,
    result: 'Đậu',
  },
  {
    id: 2,
    title: 'Đề 2',
    typeLisence: 'B2',
    date: '2023-10-07',
    rightAnswer: 32,
    wrongAnswer: 3,
    totalQuestion: 35,
    result: 'Đậu',
  },
  {
    id: 3,
    title: 'Đề 3',
    typeLisence: 'B1',
    date: '2023-10-07',
    rightAnswer: 30,
    wrongAnswer: 5,
    totalQuestion: 35,
    result: 'Rớt',
  },
  {
    id: 4,
    title: 'Đề 4',
    typeLisence: 'B1',
    date: '2023-10-07',
    rightAnswer: 30,
    wrongAnswer: 5,
    totalQuestion: 35,
    result: 'Rớt',
  },
  {
    id: 5,
    title: 'Đề 5',
    typeLisence: 'B1',
    date: '2023-10-07',
    rightAnswer: 30,
    wrongAnswer: 5,
    totalQuestion: 35,
    result: 'Rớt',
  },
  {
    id: 6,
    title: 'Đề 6',
    typeLisence: 'B1',
    date: '2023-10-07',
    rightAnswer: 30,
    wrongAnswer: 5,
    totalQuestion: 35,
    result: 'Rớt',
  },
  {
    id: 7,
    title: 'Đề 7',
    typeLisence: 'B1',
    date: '2023-10-07',
    rightAnswer: 30,
    wrongAnswer: 5,
    totalQuestion: 35,
    result: 'Rớt',
  },
  {
    id: 8,
    title: 'Đề 8',
    typeLisence: 'B1',
    date: '2023-10-07',
    rightAnswer: 30,
    wrongAnswer: 5,
    totalQuestion: 35,
    result: 'Rớt',
  },
  {
    id: 9,
    title: 'Đề 9',
    typeLisence: 'B1',
    date: '2023-10-07',
    rightAnswer: 30,
    wrongAnswer: 5,
    totalQuestion: 35,
    result: 'Rớt',
  },
  {
    id: 10,
    title: 'Đề 10',
    typeLisence: 'B1',
    date: '2023-10-07',
    rightAnswer: 30,
    wrongAnswer: 5,
    totalQuestion: 35,
    result: 'Rớt',
  },
  {
    id: 11,
    title: 'Đề 11',
    typeLisence: 'B1',
    date: '2023-10-07',
    rightAnswer: 30,
    wrongAnswer: 5,
    totalQuestion: 35,
    result: 'Rớt',
  },
];

const HistoryTest = () => {
  return (
    <div className='w-full'>
      <DataGrid
        rows={rows}
        columns={columns}
        initialState={{
          pagination: {
            paginationModel: { page: 0, pageSize: 5 },
          },
        }}
        pageSizeOptions={[5, 10]}
        // checkboxSelection
      />
    </div>
  );
};

export default HistoryTest;
