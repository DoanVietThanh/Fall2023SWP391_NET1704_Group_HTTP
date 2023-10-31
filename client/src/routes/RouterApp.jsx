import React from 'react';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import 'swiper/css';
import Loading from '../components/Loading';
import SideBar from '../components/SideBar';
import AboutUsPage from '../pages/AboutUsPage/AboutUsPage';
import BlogDetail from '../pages/BlogPage/BlogDetail';
import BlogPage from '../pages/BlogPage/BlogPage';
import ContactPage from '../pages/ContactPage/ContactPage';
import CoursePage from '../pages/CoursePage/CoursePage';
import DetailCourse from '../pages/CoursePage/DetailCourse';
import Payment from '../pages/CoursePage/Payment';
import CauHoiDiemLietPage from '../pages/DocumentPage/CauHoiDiemLietPage';
import DocumentPage from '../pages/DocumentPage/DocumentPage';
import ToanBoCauHoiPage from '../pages/DocumentPage/ToanBoCauHoiPage';
import HomePage from '../pages/HomePage/HomePage';
import InstructorSchedule from '../pages/InstructorSchedule/InstructorSchedule';
import IntructorDetail from '../pages/IntructorPage/IntructorDetail';
import IntructorPage from '../pages/IntructorPage/IntructorPage';
import ForgotPassword from '../pages/LoginPage/ForgotPassword';
import LoginPage from '../pages/LoginPage/LoginPage';
import RegisterPage from '../pages/LoginPage/RegisterPage';
import ShowForgetPass from '../pages/LoginPage/ShowForgetPass';
import DashboardPage from '../pages/Manager/pages/DashboardPage/DashboardPage';
import DashBoard from '../pages/PrivatePage/DashBoard';
import HistoryTest from '../pages/PrivatePage/HistoryTest';
import ManageBankTest from '../pages/PrivatePage/ManageBankTest';
import ManageQuestion from '../pages/PrivatePage/ManageQuestion';
import ManageUser from '../pages/PrivatePage/ManageUser';
import Profile from '../pages/PrivatePage/Profile';
import WeekSchedule from '../pages/PrivatePage/WeekSchedule';
import WeekScheduleMentor from '../pages/PrivatePage/WeekScheduleMentor';
import SimulationSituation from '../pages/SimulationSituationPage/SimulationSituation';
import ResultTheory from '../pages/TheoryPage/ResultTheory';
import TestTheory from '../pages/TheoryPage/TestTheory';
import TheoryPage from '../pages/TheoryPage/TheoryPage';
import ErrorPage from './../pages/ErrorPage/ErrorPage';

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
    { path: '/dashboard', element: <DashBoard /> },

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

    { path: '/simulationSituation', element: <SimulationSituation /> },

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
