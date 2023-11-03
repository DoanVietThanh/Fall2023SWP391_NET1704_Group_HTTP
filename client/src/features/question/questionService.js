import axiosClient from '../../utils/axiosClient';
import axiosForm from '../../utils/axiosForm';
import { toastError } from './../../components/Toastify';

const url_server = process.env.REACT_APP_SERVER_API;

const getQuestions = async () => {
  const response = await axiosClient
    .get(`/theory`)
    .then((res) => {
      console.log(123);
      console.log('res: ', res);
    })
    .catch((error) => toastError(error?.response?.data?.message));
  // if (response?.data.statusCode === 200) {
  //   console.log('Lấy danh sách câu hỏi thành công');
  // } else {
  //   toastError('Lấy danh sách câu hỏi thất bại');
  // }
  return response?.data?.data;
};

const getLisenceType = async () => {
  const response = await axiosClient
    .get(`/theory/add-question`)
    .then((res) => console.log(res))
    .catch((error) => toastError(error?.response?.data?.message));
  // if (response?.data.statusCode === 200) {
  //   console.log('Lấy Loại bằng lái thành công');
  // } else {
  //   toastError('Lấy Loại bằng lái thất bại');
  // }
  return response?.data?.data;
};

const createQuestion = async (dataForm) => {
  console.log('createQuestion (dataForm): ', dataForm);
  const response = await axiosForm
    .post(`/theory/add-question`, dataForm)
    .then((res) => console.log(res))
    .catch((error) => toastError(error?.response?.data?.message));
  console.log(response);
  return response?.data?.data;
};

const questionService = {
  getQuestions,
  createQuestion,
  getLisenceType,
};

export default questionService;
