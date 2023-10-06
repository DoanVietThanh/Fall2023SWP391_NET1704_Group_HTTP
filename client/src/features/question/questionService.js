import axios from 'axios';
import axiosClient from '../../utils/axiosClient';
import { toastError, toastSuccess } from './../../components/Toastify';
import axiosForm from '../../utils/axiosForm';

const url_server = process.env.REACT_APP_SERVER_API;

const getQuestions = async () => {
  const response = await axiosClient.get(`${url_server}/theory`);
  if (response?.data.statusCode === 200) {
    toastSuccess('Lấy danh sách câu hỏi thành công');
  } else {
    toastError('Lấy danh sách câu hỏi thất bại');
  }
  return response.data.data;
};

const getLisenceType = async () => {
  const response = await axiosClient.get(`${url_server}/theory/add-question`);
  if (response?.data.statusCode === 200) {
    console.log('Lấy Loại bằng lái thành công');
  } else {
    toastError('Lấy Loại bằng lái thất bại');
  }
  return response.data.data;
};

const createQuestion = async (dataForm) => {
  console.log('createQuestion (dataForm): ', dataForm);
  const response = await axiosForm.post(
    `${url_server}/theory/add-question`,
    dataForm
  );
  return response?.data.data;
};

const questionService = {
  getQuestions,
  createQuestion,
  getLisenceType,
};

export default questionService;
