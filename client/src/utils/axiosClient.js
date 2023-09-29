import axios from 'axios';

const axiosClient = axios.create({
  baseURL: process.env.REACT_APP_SERVER_API,
});

// Add a request interceptor
axiosClient.interceptors.request.use(
  function (config) {
    console.log('🚀 ~ file: axiosClient.js:10 ~ config:', config);
    // Do something before request is sent
    return config;
  },
  function (error) {
    // Do something with request error
    return Promise.reject(error);
  }
);

// Add a response interceptor
axiosClient.interceptors.response.use(
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

export default axiosClient;
