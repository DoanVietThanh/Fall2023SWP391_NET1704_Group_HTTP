import { DataGrid, GridToolbar } from '@mui/x-data-grid';
import React, { useCallback, useEffect, useState } from 'react';
import { toastSuccess } from './../../components/Toastify';

import { Box, TextField } from '@mui/material';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import { AiOutlineMinusCircle, AiOutlinePlusCircle } from 'react-icons/ai';
import { RiDeleteBin6Line } from 'react-icons/ri';
import { TbEdit } from 'react-icons/tb';
import { useDispatch, useSelector } from 'react-redux';
import {
  createQuestion,
  getLisenceType,
  getQuestions,
} from '../../features/question/questionSlice';
import SideBar from '../../components/SideBar';

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

const ManageQuestion = () => {
  const dispatch = useDispatch();
  const { listQuestions, listLisenceType, isError, isLoading, message } =
    useSelector((state) => state.question);
  const [selection, setSelection] = useState([]);
  const [openFormQuestion, setOpenFormQuestion] = useState(false);
  const [listAnswer, setListAnswer] = useState(1);
  const [previewImg, setPreviewImg] = useState(null);
  const [loading, setLoading] = useState(true);
  const [rows, setRows] = useState([]);
  const [str, setStr] = useState('');

  const [listFormAnswer, setListFormAnswer] = useState([]);

  const initialFormData = {
    imageLink: '',
    questionAnswerDesc: '',
    isParalysis: false,
    answers: [],
    LicenseTypeId: 1,
    rightAnswer: '',
  };
  const [formData, setFormData] = useState(initialFormData);

  useEffect(() => {
    return () => {
      previewImg && URL.revokeObjectURL(previewImg.preview);
    };
  }, [previewImg]);

  useEffect(() => {
    dispatch(getQuestions());
    dispatch(getLisenceType());
    const newListQuestion = [...listQuestions];
    setRows(
      newListQuestion.map((item, index) => {
        return {
          id: item.question.questionId,
          title: item.question.questionAnswerDesc,
          type: item.question.licenseType.licenseTypeDesc,
          listAnswer: item.answers.map(
            (itemAnswer, indexAnswer) => itemAnswer.answer
          ),
          control: [
            <TbEdit size={20} className='text-blue-600' />,
            <RiDeleteBin6Line size={20} className='text-red-400' />,
          ],
        };
      })
    );
  }, []);

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

  const handleCreateQuestion = async (e) => {
    e.preventDefault();
    console.log({ ...formData, answers: listFormAnswer });
    await dispatch(createQuestion({ ...formData, answers: listFormAnswer }));
    dispatch(getQuestions());
    setOpenFormQuestion(false);
  };

  // const handleCreateTest = useCallback(() => {
  //   setLoading((loading) => !loading);
  //   console.log(selection);
  //   toastSuccess('Tạo đề thành công');
  // }, [selection]);
  console.log('listFormAnswer: ', listFormAnswer);
  return (
    <div className='flex'>
      <SideBar />
      <Box component='main' sx={{ flexGrow: 1, p: 3 }}>
        <div className='h-[80vh] w-full rounded overflow-y-auto mt-[64px]'>
          <div>
            <div className='flex justify-end gap-8 mb-2'>
              <button
                className='btn'
                onClick={() => {
                  setOpenFormQuestion(true);
                  setPreviewImg(null);
                  setFormData(initialFormData);
                  setListAnswer(1);
                }}
              >
                Tạo câu hỏi
              </button>
              <button className='btn' onClick={() => {}}>
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
              <form
                className='mx-8 py-4'
                onSubmit={(e) => handleCreateQuestion(e)}
              >
                <div>
                  <h1 className='font-bold capitalize py-4 text-blue-400'>
                    Câu hỏi
                  </h1>
                  <TextField
                    id='outlined-basic'
                    label='Nhập câu hỏi'
                    variant='outlined'
                    fullWidth
                    value={formData.questionAnswerDesc}
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        questionAnswerDesc: e.target.value,
                      })
                    }
                  />
                  <select
                    className='my-4'
                    value={formData.LicenseTypeId}
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        LicenseTypeId: e.target.value,
                      })
                    }
                  >
                    {listLisenceType.map((item, index) => (
                      <option value={item?.licenseTypeId} className='px-4 py-4'>
                        {item?.licenseTypeDesc}
                      </option>
                    ))}
                  </select>
                  <input
                    type='file'
                    name='imageLink'
                    className='my-4 w-full'
                    onChange={(e) => {
                      console.log(e);
                      handlePreviewImg(e);
                      setFormData({
                        ...formData,
                        imageLink: e.target.files[0],
                      });
                    }}
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
                  <h1 className='font-bold capitalize py-4 text-blue-400'>
                    Đáp án
                  </h1>
                  <div className='flex flex-col gap-4'>
                    {[...Array(listAnswer)].map((item, index) => (
                      <div className='center'>
                        <TextField
                          id='outlined-basic'
                          label='Nhập đáp án'
                          variant='outlined'
                          fullWidth
                          className='flex-1'
                          value={formData.answers[index]}
                          required={true}
                          onBlur={(e) => {
                            const temp = [...listFormAnswer];
                            temp[index] = e.target.value;
                            setListFormAnswer(temp);
                          }}
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
                            value={listFormAnswer[index]}
                            onChange={(e) => {
                              const { answers, ...obj } = formData;
                              setFormData({
                                ...formData,
                                rightAnswer: e.target.value,
                              });
                            }}
                          />
                        </div>
                      </div>
                    ))}
                  </div>
                </div>

                <div className='flex justify-between items-center mt-4'>
                  <div className='flex-x gap-4 '>
                    <label
                      htmlFor='isParalyzed'
                      className='font-bold cursor-pointer'
                    >
                      Câu điểm liệt
                    </label>
                    <input
                      type='checkbox'
                      name='cursor-pointer'
                      id='isParalyzed'
                      className='w-[20px] h-[20px]'
                      onChange={(e) =>
                        setFormData({
                          ...formData,
                          isParalysis: e.target.checked,
                        })
                      }
                    />
                  </div>
                  <div className='flex gap-8 justify-end items-center h-full'>
                    <div
                      className='cursor-pointer'
                      onClick={() => setListAnswer(listAnswer + 1)}
                    >
                      <AiOutlinePlusCircle
                        className='text-blue-500 '
                        size={30}
                      />
                    </div>
                    <div
                      className='cursor-pointer'
                      onClick={() =>
                        listAnswer > 1 && setListAnswer(listAnswer - 1)
                      }
                    >
                      <AiOutlineMinusCircle
                        className='text-blue-500'
                        size={30}
                      />
                    </div>
                  </div>
                </div>
                <div className='mr-8 my-4 flex justify-end'>
                  <Button
                    onClick={() => setOpenFormQuestion(false)}
                    className='font-bold'
                  >
                    Hủy
                  </Button>
                  <button className='btn' type='submit'>
                    Tạo câu hỏi
                  </button>
                </div>
              </form>
            </Dialog>
          </div>
        </div>
      </Box>
    </div>
  );
};

export default ManageQuestion;

// console.log(Array.from({ length: 10 }, (_, index) => 0));
// console.log([...Array(10)].map(() => 0));
// console.log(new Array(10).fill(0));
