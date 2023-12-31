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
import DocumentPage from '../pages/DocumentPage/DocumentPage';
import HomePage from '../pages/HomePage/HomePage';
import IntructorDetail from '../pages/IntructorPage/IntructorDetail';
import IntructorPage from '../pages/IntructorPage/IntructorPage';
import ForgotPassword from '../pages/LoginPage/ForgotPassword';
import LoginPage from '../pages/LoginPage/LoginPage';
import RegisterPage from '../pages/LoginPage/RegisterPage';
import ShowForgetPass from '../pages/LoginPage/ShowForgetPass';
import DashBoard from '../pages/PrivatePage/DashBoard';
import HistoryTest from '../pages/PrivatePage/HistoryTest';
import ManageBankTest from '../pages/PrivatePage/ManageBankTest';
import ManageQuestion from '../pages/PrivatePage/ManageQuestion';
import ManageUser from '../pages/PrivatePage/ManageUser';
import WeekSchedule from '../pages/PrivatePage/WeekSchedule';
import WeekScheduleMentor from '../pages/PrivatePage/WeekScheduleMentor';
import SimulationSituation from '../pages/SimulationSituationPage/SimulationSituation';
import ResultTheory from '../pages/TheoryPage/ResultTheory';
import TestTheory from '../pages/TheoryPage/TestTheory';
import TheoryPage from '../pages/TheoryPage/TheoryPage';
import ErrorPage from './../pages/ErrorPage/ErrorPage';
import InstructorSchedule from './../pages/IntructorPage/InstructorSchedule';
import ManageAwaitSchedule from './../pages/PrivatePage/ManageAwaitSchedule';
import Profile from './../pages/PrivatePage/ProfilePage/Profile';
// import ManageAwaitSchedule from '../pages/PrivatePage/ManageAwaitSchedule';

import ApproveSchedule from '../pages/PrivatePage/ApproveSchedule';
import ManageDenySchedule from '../pages/PrivatePage/ManageDenySchedule';
import ManageCourse from '../pages/PrivatePage/ManageCourse/ManageCourse';
import ManageStaff from '../pages/PrivatePage/ManageStaff/ManageStaff';
import ManageBlog from './../pages/PrivatePage/ManageBlog';
import PaymentFail from '../pages/CoursePage/PaymentFail';

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
    { path: '/instructor/detail', element: <IntructorDetail /> },
    {
      path: '/instructor/teaching-schedule/:idInstructor/:idCourse',
      element: <InstructorSchedule />,
    },
    { path: '/profile', element: <Profile /> },
    { path: '/week-schedule', element: <WeekSchedule /> },
    { path: '/history-test', element: <HistoryTest /> },
    { path: '/week-schedule-mentor', element: <WeekScheduleMentor /> },
    { path: '/manage-question', element: <ManageQuestion /> },
    { path: '/manage-banktest', element: <ManageBankTest /> },
    { path: '/manage-user', element: <ManageUser /> },
    { path: '/manage-staff', element: <ManageStaff /> },
    { path: '/manage-blog', element: <ManageBlog /> },

    { path: '/dashboard', element: <DashBoard /> },
    { path: '/manage-await-schedule', element: <ManageAwaitSchedule /> },
    { path: '/manage-deny-schedule', element: <ManageDenySchedule /> },
    { path: '/manage-course', element: <ManageCourse /> },
    {
      path: '/manage-await-schedule/:idMentor',
      element: <ApproveSchedule />,
    },

    { path: '/course', element: <CoursePage /> },
    { path: '/course/detail/:idCourse', element: <DetailCourse /> },
    { path: '/api/payment/notification', element: <Payment /> },
    {
      path: '/api/payment/notification-fail',
      element: <PaymentFail />,
    },

    { path: '/document', element: <DocumentPage /> },

    { path: '/theory', element: <TheoryPage /> },
    { path: '/theory/test/:theoryExamId', element: <TestTheory /> },
    {
      path: '/theory/result/:email/:mockTestId/:startedDate',
      element: <ResultTheory />,
    },

    { path: '/simulationSituation', element: <SimulationSituation /> },

    { path: '/blog', element: <BlogPage /> },
    { path: '/blog/:blogId', element: <BlogDetail /> },
    { path: '/contact', element: <ContactPage /> },

    { path: '/aboutus', element: <AboutUsPage /> },
    { path: '/test', element: <SideBar /> },

    { path: '*', element: <ErrorPage /> },
  ]);
  return <RouterProvider router={router} fallbackElement={<Loading />} />;
};

export default RouterApp;
