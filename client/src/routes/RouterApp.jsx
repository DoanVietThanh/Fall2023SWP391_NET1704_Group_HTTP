import React from 'react';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import HomePage from '../pages/HomePage/HomePage';
import ErrorPage from './../pages/ErrorPage/ErrorPage';
import Loading from '../components/Loading';
import 'swiper/css';
import LoginPage from '../pages/LoginPage/LoginPage';
import RegisterPage from '../pages/LoginPage/RegisterPage';
import ProfilePage from '../pages/ProfilePage/ProfilePage';

const RouterApp = () => {
  const router = createBrowserRouter([
    { path: '/', element: <HomePage /> },
    { path: '/home', element: <HomePage /> },
    { path: '/login', element: <LoginPage /> },
    { path: '/register', element: <RegisterPage /> },
    { path: '/profile', element: <ProfilePage /> },
    { path: '*', element: <ErrorPage /> },
  ]);
  return <RouterProvider router={router} fallbackElement={<Loading />} />;
};

export default RouterApp;
