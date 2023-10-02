import { DataGrid } from '@mui/x-data-grid';
import React from 'react';

import { TbEdit } from 'react-icons/tb';
import { RiDeleteBin6Line } from 'react-icons/ri';

const columns = [
  { field: 'id', headerName: 'ID', width: 50 },
  {
    field: 'title',
    headerName: 'Câu hỏi',
    flex: 1,
    width: 500,
    renderCell: ({ row: { title } }) => (
      <p className='whitespace-normal overflow-y-auto my-4'>{title}</p>
    ),
  },
  {
    field: 'listAnswer',
    headerName: 'Đáp án',
    // flex: 1,
    width: 300,
    renderCell: ({ row: { listAnswer } }) => (
      <select className='w-full p-2 rounded-lg border-0 outline-none'>
        {listAnswer.map((item, index) => (
          <option key={index} className='p-2'>
            {item}
          </option>
        ))}
      </select>
    ),
  },
  {
    field: 'control',
    headerName: 'Điều khiển',
    width: 100,
    sortable: false,
    renderCell: ({ row: { control } }) => (
      <div className='flex-x gap-2'>
        {control.map((item, index) => (
          <div className='cursor-pointer p-2'>{item}</div>
        ))}
      </div>
    ),
  },
];

const rows = [
  {
    id: 1,
    title:
      'Ở phần đường dành cho người đi bộ qua đường, trên cầu, đầu cầu, đường cao tốc, đường hẹp, đường dốc, tại nơi đường bộ giao nhau cùng mức với đường sắt có được quay đầu xe hay không?',
    listAnswer: ['Được phép', 'Không được phép', 'Tùy từng trường hợp'],
    control: [
      <TbEdit size={20} className='text-blue-600' />,
      <RiDeleteBin6Line size={20} className='text-red-400' />,
    ],
  },
  {
    id: 2,
    title:
      'Người đủ bao nhiêu tuổi trở lên thì được điều khiển xe mô tô hai bánh, xe mô tô ba bánh có dung tích xi lanh từ 50 cm3 trở lên và các loại xe có kết cấu tương tự; xe ô tô tải, máy kéo có trọng tải dưới 3,5 tấn; xe ô tô chở người đến 9 chỗ ngồi?',
    listAnswer: ['1-16 tuổi', '2-18 tuổi', '3-17 tuổi'],
    control: [
      <TbEdit size={20} className='text-blue-600' />,
      <RiDeleteBin6Line size={20} className='text-red-400' />,
    ],
  },
  {
    id: 3,
    title:
      'Biển báo hiệu có dạng tam giác đều, viền đỏ, viền màu vàng, trên có hình vẽ màu đen là loại biển gì dưới đây?',
    listAnswer: [
      'Biển báo nguy hiểm',
      'Biển báo cấm',
      'Biển báo hiệu lệnh',
      'Biển báo chỉ dẫn',
    ],
    control: [
      <TbEdit size={20} className='text-blue-600' />,
      <RiDeleteBin6Line size={20} className='text-red-400' />,
    ],
  },
  {
    id: 4,
    title:
      'Người lái xe phải làm gì khi quay đầu xe trên cầu, đường ngầm hay khu vực đường bộ giao nhau cùng mức với đường sắt?',
    listAnswer: [
      'Không được quay đầu xe',
      'Lợi dụng chỗ rộng và phải có người làm tín hiệu sau xe để bảo đảm an toàn.',
      'Lợi dụng chỗ rộng có thể quay đầu được để quay đầu xe cho an toàn.',
    ],
    control: [
      <TbEdit size={20} className='text-blue-600' />,
      <RiDeleteBin6Line size={20} className='text-red-400' />,
    ],
  },
  {
    id: 5,
    title:
      'Người đủ bao nhiêu tuổi trở lên thì được điều khiển xe mô tô hai bánh, xe mô tô ba bánh có dung tích xi lanh từ 50 cm3 trở lên và các loại xe có kết cấu tương tự; xe ô tô tải, máy kéo có trọng tải dưới 3,5 tấn; xe ô tô chở người đến 9 chỗ ngồi?',
    listAnswer: ['1-16 tuổi', '2-18 tuổi', '3-17 tuổi'],
    control: [
      <TbEdit size={20} className='text-blue-600' />,
      <RiDeleteBin6Line size={20} className='text-red-400' />,
    ],
  },
  {
    id: 6,
    title:
      'Người đủ bao nhiêu tuổi trở lên thì được điều khiển xe mô tô hai bánh, xe mô tô ba bánh có dung tích xi lanh từ 50 cm3 trở lên và các loại xe có kết cấu tương tự; xe ô tô tải, máy kéo có trọng tải dưới 3,5 tấn; xe ô tô chở người đến 9 chỗ ngồi?',
    listAnswer: ['1-16 tuổi', '2-18 tuổi', '3-17 tuổi'],
    control: [
      <TbEdit size={20} className='text-blue-600' />,
      <RiDeleteBin6Line size={20} className='text-red-400' />,
    ],
  },
];

const ManageQuestion = () => {
  return (
    <div>
      <div className='flex justify-end gap-8 mb-2'>
        <button className='btn'>Tạo đề thi</button>
        <button className='btn'>Tạo câu hỏi</button>
      </div>
      <div className='h-[64vh]'>
        <DataGrid
          className='h-[200px]'
          getRowHeight={() => 'auto'}
          rows={rows}
          columns={columns}
          initialState={{
            pagination: {
              paginationModel: { page: 0, pageSize: 5 },
            },
          }}
          pageSizeOptions={[2, 5, 10, 20, 50, 100]}
          checkboxSelection
        />
      </div>
    </div>
  );
};

export default ManageQuestion;
