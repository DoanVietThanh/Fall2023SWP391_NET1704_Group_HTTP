import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import axiosClient from '../../../utils/axiosClient';
import { toastError, toastSuccess } from '../../../components/Toastify';
import axios from 'axios';

const DialogReservation = ({
  open,
  setOpen,
  course = null,
  selectedCoursePackage = null,
}) => {
  const { memberId } =
    useSelector((state) => state?.auth?.user?.accountInfo) || '';
  const [paymentList, setPaymentList] = useState([]);
  const [typePayment, setTypePayment] = useState('3');

  const [selectedMentor, setSelectedMentor] = useState(
    course?.mentors[0]?.staffId
  );

  useEffect(() => {
    async function getListTypePayment() {
      await axios
        .get('/courses/packages/reservation')
        .then((res) => setPaymentList(res?.data?.data));
    }
    getListTypePayment();
  }, []);

  console.log(
    JSON.stringify({
      memberId,
      mentorId: selectedMentor,
      coursePackageId: selectedCoursePackage?.coursePackageId,
      paymentTypeId: typePayment,
    })
  );

  const submitForm = (e) => {
    async function submitPayment() {
      await axiosClient
        .post(`/courses/packages/reservation`, {
          memberId,
          mentorId: selectedMentor,
          coursePackageId: selectedCoursePackage?.coursePackageId,
          paymentTypeId: typePayment,
        })
        .then(async (res) => {
          console.log('resssss: ', res);
          if (res?.data?.statusCode === 201) {
            console.log('typePayment: ', typePayment);
            if (typePayment === '1') {
              toastSuccess(res?.data?.message);
            }
            handleTypePayment(res);
          } else {
            toastError(res?.data?.message);
          }
        })
        .catch((e) => {
          console.log(e);
          toastError(e?.response?.data?.message);
        });
    }
    submitPayment();
  };

  const handleTypePayment = async (res) => {
    console.log('ahihi ');
    if (typePayment === '3') {
      console.log(123123123);
      const resPayment = await axiosClient.post(
        `/api/payment`,
        res?.data?.data
      );
      console.log('resPayment: ', resPayment);
      window.location.href = resPayment?.data?.data?.paymentUrl;
    }
  };

  console.log({
    memberId,
    mentorId: selectedMentor,
    coursePackageId: selectedCoursePackage?.coursePackageId,
    typePayment,
  });

  return (
    <div>
      <Dialog
        open={open}
        onClose={() => setOpen(false)}
        aria-labelledby='alert-dialog-title'
        aria-describedby='alert-dialog-description'
      >
        <div className='w-[26vw] rounded-lg overflow-hidden'>
          <div>
            <div className='p-4 border flex-1'>
              <h1 className='text-center font-bold text-[25px]'>
                {selectedCoursePackage?.coursePackageDesc}
              </h1>
              <ul className='my-4 list-disc pl-[20%] text-[22px]'>
                {selectedCoursePackage?.totalSession && (
                  <li>
                    Số buổi học:{' '}
                    <span className='text-green-800'>
                      {selectedCoursePackage?.totalSession}
                    </span>
                  </li>
                )}

                {selectedCoursePackage?.sessionHour && (
                  <li>
                    Thời gian mỗi buổi học:{' '}
                    <span className='text-green-800'>
                      {selectedCoursePackage?.sessionHour}
                    </span>
                  </li>
                )}

                {selectedCoursePackage?.cost && (
                  <li>
                    Giá:{' '}
                    <span className='text-green-800'>
                      {selectedCoursePackage?.cost}
                    </span>
                  </li>
                )}
              </ul>
              {selectedCoursePackage?.ageRequired && (
                <p>
                  Yêu cầu tuổi phải trên{' '}
                  <span className='text-red-700'>
                    {selectedCoursePackage?.ageRequired}
                  </span>
                </p>
              )}
            </div>
          </div>

          <form action='' className='p-4'>
            <div className='flex gap-4 justify-between'>
              <h1>Chọn giảng viên: </h1>
              <select
                className='outline outline-1 rounded-lg px-2'
                onChange={(e) => setSelectedMentor(e.target.value)}
                required
              >
                {course?.mentors?.map((mentor, index) => (
                  <option
                    key={mentor?.staffId}
                    value={mentor?.staffId}
                  >{`${mentor?.firstName} ${mentor?.lastName}`}</option>
                ))}
              </select>
            </div>
            <div className='flex gap-4 justify-between my-4'>
              <h1>Hình thức thanh toán: </h1>
              <select
                className='outline outline-1 rounded-lg px-2'
                onChange={(e) => setTypePayment(e.target.value)}
                required
              >
                {paymentList?.map((payment, index) => (
                  <option key={index} value={payment?.paymentTypeId}>
                    {payment.paymentTypeDesc}
                  </option>
                ))}
              </select>
            </div>
            <DialogActions>
              <Button onClick={() => setOpen(false)}>Hủy</Button>
              <Button onClick={(e) => submitForm(e)} autoFocus>
                Thanh toán
              </Button>
            </DialogActions>
          </form>
        </div>
      </Dialog>
    </div>
  );
};

export default DialogReservation;
