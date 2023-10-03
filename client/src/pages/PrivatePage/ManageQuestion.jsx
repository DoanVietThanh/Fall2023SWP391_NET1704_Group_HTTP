import { DataGrid, GridToolbar } from '@mui/x-data-grid';
import React, { useCallback, useEffect, useState } from 'react';
import { toastSuccess } from './../../components/Toastify';

import { TextField } from '@mui/material';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import { AiOutlineMinusCircle, AiOutlinePlusCircle } from 'react-icons/ai';
import { RiDeleteBin6Line } from 'react-icons/ri';
import { TbEdit } from 'react-icons/tb';

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
    field: 'type',
    headerName: 'Loại',
    width: 100,
    renderCell: ({ row: { type } }) => (
      <p className='center'>
        <span className='text-red-500 font-bold text-[24px]'>*</span> {type}
      </p>
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
          <div className='cursor-pointer p-2' key={index}>
            {item}
          </div>
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
    type: 'B1',
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
    type: 'A2',

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
    type: 'A1',

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
    type: 'B1',

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
    type: 'B1',
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
    type: 'B2',
    listAnswer: ['1-16 tuổi', '2-18 tuổi', '3-17 tuổi'],
    control: [
      <TbEdit size={20} className='text-blue-600' />,
      <RiDeleteBin6Line size={20} className='text-red-400' />,
    ],
  },
];

const ManageQuestion = () => {
  const [selection, setSelection] = useState([]);
  const [openFormQuestion, setOpenFormQuestion] = useState(false);
  const [listAnswer, setListAnswer] = useState(1);
  const [previewImg, setPreviewImg] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    return () => {
      previewImg && URL.revokeObjectURL(previewImg.preview);
    };
  }, [previewImg]);

  const handleSelectId = (params) => {
    const newArray = [...selection];
    if (newArray.includes(params.id)) {
      const index = newArray.indexOf(params.id);
      newArray.splice(index, 1);
    } else {
      newArray.push(params?.id);
    }
    setSelection(newArray);
  };

  const handlePreviewImg = (e) => {
    const file = e.target.files[0];
    file.preview = URL.createObjectURL(file);
    setPreviewImg(file);
  };

  const handleSubmit = () => {
    toastSuccess('Tạo câu hỏi thành công');
  };

  const handleCreateQuestion = () => {
    setOpenFormQuestion(false);
    toastSuccess('Tạo câu hỏi thành công');
  };

  const handleCreateTest = useCallback(() => {
    setLoading((loading) => !loading);
    // console.log(selection);
    toastSuccess('Tạo đề thành công');
  }, [selection]);

  console.log('loading: ', loading);

  return (
    <div>
      <div className='flex justify-end gap-8 mb-2'>
        <button
          className='btn'
          onClick={() => {
            setOpenFormQuestion(true);
            setPreviewImg(null);
          }}
        >
          Tạo câu hỏi
        </button>
        <button className='btn' onClick={handleCreateTest}>
          Tạo đề thi
        </button>
      </div>

      <div className='h-[70vh]'>
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
          slots={{ toolbar: GridToolbar }}
          pageSizeOptions={[2, 5, 10, 20, 50, 100]}
          checkboxSelection
          getRowId={(row) => row.id}
          onCellClick={(params) => handleSelectId(params)}
          {...rows}
        />
      </div>

      <Dialog
        open={openFormQuestion}
        onClose={() => setOpenFormQuestion(false)}
        aria-labelledby='alert-dialog-title'
        aria-describedby='alert-dialog-description'
      >
        <form className='mx-8 py-4' onSubmit={handleSubmit}>
          <div>
            <h1 className='font-bold capitalize py-4 text-blue-400'>
              Câu hỏi{' '}
            </h1>
            <TextField
              id='outlined-basic'
              label='Nhập câu hỏi'
              variant='outlined'
              fullWidth
            />
            <input
              type='file'
              name=''
              id=''
              className='my-4 w-full'
              onChange={handlePreviewImg}
            />
            {previewImg && (
              <div className='flex justify-center'>
                <img
                  src={previewImg.preview}
                  alt='img test'
                  className='h-[160px] '
                />
              </div>
            )}
          </div>
          <div>
            <h1 className='font-bold capitalize py-4 text-blue-400'>Đáp án</h1>
            <div className='flex flex-col gap-4'>
              {[...Array(listAnswer)].map((item, index) => (
                <div className='center'>
                  <TextField
                    id='outlined-basic'
                    label='Nhập đáp án'
                    variant='outlined'
                    fullWidth
                    className='flex-1'
                  />
                  <div className='flex-y p-2 gap-2'>
                    <label
                      className='font-medium cursor-pointer'
                      htmlFor={`rightAnswer-${index}`}
                    >
                      Đáp án đúng
                    </label>
                    <input
                      type='radio'
                      name='rightAnswer'
                      id={`rightAnswer-${index}`}
                      className='w-[20px] h-[20px]'
                      required
                    />
                  </div>
                </div>
              ))}
            </div>
          </div>

          <div className='flex justify-between items-center mt-4'>
            <div className='flex-x gap-4 '>
              <label htmlFor='isParalyzed' className='font-bold cursor-pointer'>
                Câu điểm liệt
              </label>
              <input
                type='checkbox'
                name='cursor-pointer'
                id='isParalyzed'
                className='w-[20px] h-[20px]'
              />
            </div>
            <div className='flex gap-8 justify-end items-center h-full'>
              <div
                className='cursor-pointer'
                onClick={() => setListAnswer(listAnswer + 1)}
              >
                <AiOutlinePlusCircle className='text-blue-500 ' size={30} />
              </div>
              <div
                className='cursor-pointer'
                onClick={() => listAnswer > 1 && setListAnswer(listAnswer - 1)}
              >
                <AiOutlineMinusCircle className='text-blue-500' size={30} />
              </div>
            </div>
          </div>
        </form>
        <div className='mr-8 mb-4 flex justify-end'>
          <Button
            onClick={() => setOpenFormQuestion(false)}
            className='font-bold'
          >
            Hủy
          </Button>
          <button className='btn' onClick={handleCreateQuestion}>
            Tạo câu hỏi
          </button>
        </div>
      </Dialog>
    </div>
  );
};

export default ManageQuestion;

// console.log(Array.from({ length: 10 }, (_, index) => 0));
// console.log([...Array(10)].map(() => 0));
// console.log(new Array(10).fill(0));
