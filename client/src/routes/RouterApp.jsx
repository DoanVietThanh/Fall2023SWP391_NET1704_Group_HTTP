import React from 'react';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import HomePage from '../pages/HomePage/HomePage';
import ErrorPage from './../pages/ErrorPage/ErrorPage';
import Loading from '../components/Loading';
import 'swiper/css';

const RouterApp = () => {
  const router = createBrowserRouter([
    { path: '/', element: <HomePage /> },
    { path: '*', element: <ErrorPage /> },
  ]);
  return <RouterProvider router={router} fallbackElement={<Loading />} />;
};

export default RouterApp;
