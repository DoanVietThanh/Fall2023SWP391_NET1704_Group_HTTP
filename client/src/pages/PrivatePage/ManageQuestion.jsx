import { DataGrid, GridToolbar } from '@mui/x-data-grid';
import React, { useEffect, useState } from 'react';

import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import { Box, DialogActions, Menu, MenuItem, TextField } from '@mui/material';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import { AiOutlineMinusCircle, AiOutlinePlusCircle } from 'react-icons/ai';
import { RiDeleteBin6Line } from 'react-icons/ri';
import { TbEdit } from 'react-icons/tb';
import SideBar from '../../components/SideBar';
import { toastError, toastSuccess } from '../../components/Toastify';
import axiosClient from '../../utils/axiosClient';
import axiosForm from '../../utils/axiosForm';
import axiosUrlencoded from '../../utils/axiosUrlencoded';
import Loading from './../../components/Loading';

const ManageQuestion = () => {
  const columns = [
    { field: 'id', headerName: 'ID', width: 50 },
    {
      field: 'title',
      headerName: 'Câu hỏi',
      width: 500,
      // flex: 1,
      renderCell: ({ row: { title } }) => (
        <sp className='whitespace-normal overflow-y-auto my-4'>{title}</sp>
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
      width: 240,
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

  const [selection, setSelection] = useState([]);
  const [listLisenceType, setListLisenceType] = useState([]);
  const [openFormQuestion, setOpenFormQuestion] = useState(false);
  const [openConfirmDelete, setOpenConfirmDelete] = useState(false);
  const [listAnswer, setListAnswer] = useState(1);
  const [previewImg, setPreviewImg] = useState(null);
  const [loading, setLoading] = useState(true);
  const [rows, setRows] = useState([]);
  const [str, setStr] = useState('');
  const [listFormAnswer, setListFormAnswer] = useState([]);
  const [createRule, setCreateRule] = useState();
  const [selectedTypeLicense, setSelectedTypeLicense] = useState();
  const [deleteQuestionID, setDeleteQuestionID] = useState();
  const initialFormData = {
    imageLink: '',
    questionAnswerDesc: '',
    isParalysis: false,
    answers: [],
    LicenseTypeId: 1,
    rightAnswer: '',
  };
  const [formData, setFormData] = useState(initialFormData);
  const [anchorEl, setAnchorEl] = React.useState(null);
  const open = Boolean(anchorEl);
  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };

  useEffect(() => {
    return () => {
      previewImg && URL.revokeObjectURL(previewImg.preview);
    };
  }, [previewImg]);

  useEffect(() => {
    async function getListQuestion() {
      await axiosClient
        .get(`/theory`)
        .then((res) => {
          console.log(res);
          setRows(
            res?.data?.data.map((item, index) => {
              return {
                id: item.question.questionId,
                title: item.question.questionAnswerDesc,
                type: item.question.licenseType.licenseTypeDesc,
                listAnswer: item.answers.map(
                  (itemAnswer, indexAnswer) => itemAnswer.answer
                ),
                control: [
                  <TbEdit size={20} className='text-blue-600' />,
                  <RiDeleteBin6Line
                    size={20}
                    className='text-red-400'
                    onClick={() => {
                      setDeleteQuestionID(item.question.questionId);
                      setOpenConfirmDelete(true);
                    }}
                  />,
                ],
              };
            })
          );
        })
        .catch((error) => toastError(error?.response?.data?.message));

      await axiosClient
        .get(`/theory/add-question`)
        .then((res) => {
          console.log(res);
          setListLisenceType(res?.data?.data);
        })
        .catch((error) => toastError(error?.response?.data?.message));
    }
    getListQuestion();
  }, []);

  const getListQuestionsByLicenseID = async (licenseId) => {
    await axiosClient
      .get(`/theory/question-bank/${licenseId}`)
      .then((res) => {
        console.log('res: ', res);
        setCreateRule(res?.data?.data?.createRule);
        setSelectedTypeLicense(licenseId);
        setRows(
          res?.data?.data?.questionWithAnswer.map((item, index) => {
            return {
              id: item?.question?.questionId,
              title: item?.question?.questionAnswerDesc,
              type: item?.question?.licenseType?.licenseTypeDesc,
              listAnswer: item?.answers?.map(
                (itemAnswer, indexAnswer) => itemAnswer.answer
              ),
              control: [
                <TbEdit size={20} className='text-blue-600' />,
                <RiDeleteBin6Line size={20} className='text-red-400' />,
              ],
            };
          })
        );
      })
      .catch((error) => {
        toastError(error?.response?.data?.message);
      });
  };

  const getListQuestion = async () => {
    await axiosClient
      .get(`/theory`)
      .then((res) => {
        setSelection([]);
        setCreateRule(null);
        setRows(
          res?.data?.data.map((item, index) => {
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
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };

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
    await axiosForm
      .post(`/theory/add-question`, { ...formData, answers: listFormAnswer })
      .then((res) => {
        toastSuccess(res?.data?.message);
        setOpenFormQuestion(false);
        getListQuestion();
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };

  const handleOnchangeTypeLicense = (item) => {
    if (item?.licenseTypeId != selectedTypeLicense) {
      setSelection([]);
      getListQuestionsByLicenseID(item?.licenseTypeId);
      setAnchorEl(null);
    }
  };

  const handleSubmitCreateTest = async () => {
    console.log({
      questionIds: selection,
      TotalQuestion: createRule?.totalQuestion,
      TotalTime: createRule?.totalTime,
      TotalAnswerRequired: createRule?.totalAnswerRequired,
    });

    console.log(
      JSON.stringify({
        questionIds: selection,
        TotalQuestion: createRule?.totalQuestion,
        TotalTime: createRule?.totalTime,
        TotalAnswerRequired: createRule?.totalAnswerRequired,
      })
    );

    await axiosUrlencoded
      .post(`/theory-exam/add-question`, {
        questionIds: selection,
        TotalQuestion: createRule?.totalQuestion,
        TotalTime: createRule?.totalTime,
        TotalAnswerRequired: createRule?.totalAnswerRequired,
      })
      .then((res) => {
        console.log('Res: ', res);
        toastSuccess(res?.data?.message);
        setSelection([]);
        getListQuestion();
        // setSelectedTypeLicense(null)
        // window.location.reload();
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };

  const handleDeleteQuestion = async () => {
    console.log('questionID: ', deleteQuestionID);
    await axiosClient
      .delete(`/theory/${deleteQuestionID}/delete-question`)
      .then((res) => {
        console.log(res);
        toastSuccess(res?.data?.message);
        setOpenConfirmDelete(false);
        getListQuestion();
      })
      .catch((error) => toastError(error?.response?.data?.message));
  };

  // console.log('selection: ', selection);

  return (
    <div className='flex'>
      <SideBar />
      <Box component='main' sx={{ flexGrow: 1, p: 3 }}>
        {rows && listLisenceType ? (
          <div className='h-[80vh] w-full rounded overflow-y-auto mt-[64px]'>
            <div className=''>
              <div className='flex justify-between mb-2'>
                <div>
                  <button
                    id='demo-customized-button'
                    aria-controls={open ? 'demo-customized-menu' : undefined}
                    aria-haspopup='true'
                    aria-expanded={open ? 'true' : undefined}
                    variant='contained'
                    disableElevation
                    onClick={handleClick}
                    endIcon={<KeyboardArrowDownIcon />}
                    className='btn'
                  >
                    Chọn đề thi
                  </button>
                  <Menu
                    id='demo-customized-menu'
                    MenuListProps={{
                      'aria-labelledby': 'demo-customized-button',
                    }}
                    anchorEl={anchorEl}
                    open={open}
                    onClose={() => setAnchorEl(null)}
                  >
                    {listLisenceType &&
                      listLisenceType?.map((item, index) => (
                        <MenuItem
                          onClick={() => handleOnchangeTypeLicense(item)}
                          disableRipple
                        >
                          Đề thi {item?.licenseTypeDesc}
                        </MenuItem>
                      ))}
                  </Menu>
                </div>
                <div className='flex gap-8'>
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

                  <button
                    className='btn'
                    onClick={() => {
                      getListQuestion();
                      setSelectedTypeLicense(null);
                    }}
                  >
                    Tất cả câu hỏi
                  </button>

                  <button
                    className='btn'
                    onClick={() => handleSubmitCreateTest()}
                  >
                    Tạo đề thi
                  </button>
                </div>
              </div>

              <div className={`${createRule ? 'h-[58vh]' : 'h-[70vh]'} `}>
                <DataGrid
                  className='h-[200px]'
                  getRowHeight={() => 'auto'}
                  autoWidth
                  rows={rows}
                  columns={columns}
                  initialState={{
                    pagination: {
                      paginationModel: { page: 0, pageSize: 20 },
                    },
                  }}
                  slots={{ toolbar: GridToolbar }}
                  pageSizeOptions={[10, 20, 50, 100]}
                  checkboxSelection={selectedTypeLicense ? true : false}
                  getRowId={(row) => row.id}
                  onCellClick={(params) => handleSelectId(params)}
                  // {...rows}
                />
              </div>
              {createRule && (
                <div className='mt-4'>
                  <div>
                    <span className='font-semibold text-[18px] mr-4'>
                      Loại bằng lái:
                    </span>{' '}
                    {createRule?.licenseTypeDesc}
                  </div>
                  <div>
                    <span className='font-semibold text-[18px] mr-4'>
                      Tổng câu hỏi:
                    </span>
                    {createRule?.totalQuestion}
                  </div>
                  <div>
                    <span className='font-semibold text-[18px] mr-4'>
                      Tổng thời gian:
                    </span>
                    {createRule?.totalTime}
                  </div>
                </div>
              )}

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

                    {listLisenceType && (
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
                          <option
                            value={item?.licenseTypeId}
                            className='px-4 py-4'
                          >
                            {item?.licenseTypeDesc}
                          </option>
                        ))}
                      </select>
                    )}

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

              <Dialog
                open={openConfirmDelete}
                onClose={() => setOpenConfirmDelete(false)}
                aria-labelledby='alert-dialog-title'
                aria-describedby='alert-dialog-description'
              >
                <div className='p-8'>
                  <h1 className='text-center font-medium capitalize text-[24px]'>
                    Xác nhận xóa câu hỏi
                  </h1>
                  <DialogActions>
                    <Button onClick={() => setOpenConfirmDelete(false)}>
                      Hủy
                    </Button>
                    <Button onClick={() => handleDeleteQuestion()} autoFocus>
                      Đồng ý
                    </Button>
                  </DialogActions>
                </div>
              </Dialog>
            </div>
          </div>
        ) : (
          <Loading />
        )}
      </Box>
    </div>
  );
};

export default ManageQuestion;

// console.log(Array.from({ length: 10 }, (_, index) => 0));
// console.log([...Array(10)].map(() => 0));
// console.log(new Array(10).fill(0));
