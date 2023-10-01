import React from 'react';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import HomePage from '../pages/HomePage/HomePage';
import ErrorPage from './../pages/ErrorPage/ErrorPage';
import Loading from '../components/Loading';
import 'swiper/css';
import LoginPage from '../pages/LoginPage/LoginPage';
import RegisterPage from '../pages/LoginPage/RegisterPage';
import ForgotPassword from '../pages/LoginPage/ForgotPassword';
import ShowForgetPass from '../pages/LoginPage/ShowForgetPass';
import CoursePage from '../pages/CoursePage/CoursePage';
import IntructorPage from '../pages/IntructorPage/IntructorPage';
import DetailCourse from '../pages/CoursePage/DetailCourse';
import ContactPage from '../pages/ContactPage/ContactPage';
import TheoryPage from '../pages/TheoryPage/TheoryPage';
import TestTheory from '../pages/TheoryPage/TestTheory';
import ResultTheory from '../pages/TheoryPage/ResultTheory';
import DashboardPage from '../pages/Manager/pages/DashboardPage/DashboardPage';
import PrivatePage from '../pages/PrivatePage/PrivatePage';

const RouterApp = () => {
  const router = createBrowserRouter([
    { path: '/', element: <HomePage /> },
    { path: '/home', element: <HomePage /> },
    { path: '/login', element: <LoginPage /> },
    { path: '/register', element: <RegisterPage /> },
    { path: '/forgot-password', element: <ForgotPassword /> },
    { path: '/authentication/reset-password', element: <ShowForgetPass /> },
    { path: '/intructor', element: <IntructorPage /> },
    { path: '/private-information', element: <PrivatePage /> },

    { path: '/course', element: <CoursePage /> },
    { path: '/course/detail', element: <DetailCourse /> },

    { path: '/theory', element: <TheoryPage /> },
    { path: '/theory/test/:id', element: <TestTheory /> },
    { path: '/theory/result/:id', element: <ResultTheory /> },

    { path: '/contact', element: <ContactPage /> },

    { path: '/manager/dashboard', element: <DashboardPage /> },

    { path: '*', element: <ErrorPage /> },
  ]);
  return <RouterProvider router={router} fallbackElement={<Loading />} />;
};

export default RouterApp;
