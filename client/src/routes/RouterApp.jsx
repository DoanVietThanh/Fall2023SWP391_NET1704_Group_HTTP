import React from 'react';
import 'swiper/css';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import HomePage from '../pages/HomePage/HomePage';
import ErrorPage from './../pages/ErrorPage/ErrorPage';
import Loading from '../components/Loading';
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
import BlogPage from '../pages/BlogPage/BlogPage';
import BlogDetail from '../pages/BlogPage/BlogDetail';
import DocumentPage from '../pages/DocumentPage/DocumentPage';
import SideBar from '../components/SideBar';
import Profile from '../pages/PrivatePage/Profile';
import WeekSchedule from '../pages/PrivatePage/WeekSchedule';
import HistoryTest from '../pages/PrivatePage/HistoryTest';
import ManageQuestion from '../pages/PrivatePage/ManageQuestion';
import ManageBankTest from '../pages/PrivatePage/ManageBankTest';
import IntructorDetail from '../pages/IntructorPage/IntructorDetail';
import AboutUsPage from '../pages/AboutUsPage/AboutUsPage';
import InstructorSchedule from '../pages/InstructorSchedule/InstructorSchedule';
import WeekScheduleMentor from '../pages/PrivatePage/WeekScheduleMentor';
import Payment from '../pages/CoursePage/Payment';
import ManageUser from '../pages/PrivatePage/ManageUser';
import CauHoiDiemLietPage from '../pages/DocumentPage/CauHoiDiemLietPage';
import ToanBoCauHoiPage from '../pages/DocumentPage/ToanBoCauHoiPage';

const RouterApp = () => {
  const router = createBrowserRouter([
    { path: '/', element: <HomePage /> },
    { path: '/home', element: <HomePage /> },
    { path: '/login', element: <LoginPage /> },
    { path: '/register', element: <RegisterPage /> },
    { path: '/forgot-password', element: <ForgotPassword /> },
    { path: '/authentication/reset-password', element: <ShowForgetPass /> },
    { path: '/instructor', element: <IntructorPage /> },
    { path: '/instructor/detail/:idInstructor', element: <IntructorDetail /> },
    {
      path: '/instructor/teaching-schedule/:idInstructor/:idCourse',
      element: <InstructorSchedule />,
    },
    // { path: '/private-information', element: <PrivatePage /> },
    { path: '/profile', element: <Profile /> },
    { path: '/week-schedule', element: <WeekSchedule /> },
    { path: '/history-test', element: <HistoryTest /> },
    { path: '/history-test', element: <HistoryTest /> },
    { path: '/week-schedule-mentor', element: <WeekScheduleMentor /> },
    { path: '/manage-question', element: <ManageQuestion /> },
    { path: '/manage-banktest', element: <ManageBankTest /> },
    { path: '/manage-user', element: <ManageUser /> },

    { path: '/course', element: <CoursePage /> },
    { path: '/course/detail/:idCourse', element: <DetailCourse /> },
    { path: '/api/payment/notification', element: <Payment /> },
    // { path: '/test2', element: <Payment /> },

    { path: '/document', element: <DocumentPage /> },
    { path: '/document/cauhoidiemliet', element: <CauHoiDiemLietPage />},
    { path: '/document/toanbocauhoi', element: <ToanBoCauHoiPage />},

    { path: '/theory', element: <TheoryPage /> },
    { path: '/theory/test/:theoryExamId', element: <TestTheory /> },
    { path: '/theory/result/:mockTestId', element: <ResultTheory /> },

    { path: '/blog', element: <BlogPage /> },
    { path: '/blog/detail', element: <BlogDetail /> },
    { path: '/contact', element: <ContactPage /> },

    { path: '/aboutus', element: <AboutUsPage /> },
    { path: '/manager/dashboard', element: <DashboardPage /> },
    { path: '/test', element: <SideBar /> },

    { path: '*', element: <ErrorPage /> },
  ]);
  return <RouterProvider router={router} fallbackElement={<Loading />} />;
};

export default RouterApp;
