import axios from 'axios';
import { toastError, toastSuccess } from './../../components/Toastify';

const url_server = process.env.REACT_APP_SERVER_API;

const login = async (dataForm) => {
  const response = await axios.post(
    `${url_server}/authentication/login`,
    dataForm
  );
  console.log('response.data: ', response.data);
  if (response?.data.statusCode === 200) {
    toastSuccess(response.data.message);
    localStorage.setItem('user', JSON.stringify(response.data.data));
  } else {
    toastError('Login thất bại');
  }
  return response.data;
};

const authService = {
  login,
};

export default authService;
