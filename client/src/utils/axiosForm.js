import axios from 'axios';

const axiosForm = axios.create({
  baseURL: process.env.REACT_APP_SERVER_API,
  headers: {
    'Content-Type': 'multipart/form-data',
  },
});

// Add a request interceptor
axiosForm.interceptors.request.use(
  function (config) {
    // Do something before request is sent
    const accessToken = localStorage.getItem('accessToken');
    if (accessToken) {
      config.headers.Authorization = `Bearer ${accessToken}`;
    }
    console.log('🚀 ~ file: axiosForm.js:18 ~ config:', config);
    return config;
  },
  function (error) {
    // Do something with request error
    return Promise.reject(error);
  }
);

// Add a response interceptor
axiosForm.interceptors.response.use(
  function (response) {
    // Any status code that lie within the range of 2xx cause this function to trigger
    // Do something with response data
    return response;
  },
  function (error) {
    // Any status codes that falls outside the range of 2xx cause this function to trigger
    // Do something with response error
    return Promise.reject(error);
  }
);

export default axiosForm;
