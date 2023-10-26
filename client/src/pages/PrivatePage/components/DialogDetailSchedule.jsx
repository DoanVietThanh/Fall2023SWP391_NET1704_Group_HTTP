import { Button, Dialog, DialogActions } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { AiOutlineCheckCircle, AiOutlineCloseCircle } from 'react-icons/ai';
import * as dayjs from 'dayjs';

const DialogDetailSchedule = ({ open, setOpen, dialogDetail = null }) => {
  const [vehicle, setVehicle] = useState(dialogDetail && dialogDetail?.vehicle);
  console.log('dialogDetail: ', dialogDetail);

  useEffect(() => {
    setVehicle(dialogDetail?.vehicle);
  }, [dialogDetail]);

  return (
    <div>
      {dialogDetail && (
        <Dialog
          open={open}
          onClose={() => setOpen(false)}
          aria-labelledby='alert-dialog-title'
          aria-describedby='alert-dialog-description'
        >
          <div className='flex gap-10 p-6'>
            <div>
              <h2 className='font-bold my-4 text-center font-bold text-[24px]'>
                Chi tiết Lịch học
              </h2>
              <div className='flex flex-col gap-4'>
                <div className='flex gap-4'>
                  <div>Date:</div>
                  <div>
                    {/* {dayjs(dataWeek?.data?.weekdays?.monday).format(
              'DD/MM/YYYY'
            )} */}
                  </div>
                </div>
                <div className='flex gap-4'>
                  <div>Tiết học:</div>
                  {/* <div> {slotName} </div> */}
                </div>
                <div className='flex gap-4'>
                  <div>Giảng viên:</div>
                  <div>Thanh Đoàn</div>
                </div>
                <div className='flex gap-4'>
                  <div>Khóa học:</div>
                  <div>Bằng lái B2 </div>
                </div>
                <div className='flex gap-4'>
                  <div>Tổng buổi đã đăng kí:</div>
                  <div>12</div>
                </div>
                <div>
                  <div>Tổng thời gian đã đi</div>
                </div>
                <div>
                  <div>Tổng số km đã đi</div>
                </div>
                <div className='flex gap-4'>
                  <div>Điểm danh:</div>
                  <div className='text-green-700 flex justify-center items-center gap-8'>
                    <span>
                      <AiOutlineCheckCircle size={20} />
                    </span>
                    <span className='text-red-800'>
                      <AiOutlineCloseCircle size={20} />
                    </span>
                  </div>
                </div>
              </div>
            </div>

            {vehicle && (
              <div>
                <h2 className='font-bold my-4 text-center font-bold text-[24px]'>
                  Phương tiện
                </h2>
                <div className='flex flex-col gap-4'>
                  {vehicle?.vehicleName && (
                    <h3>
                      <span className='font-semibold'>Tên xe:</span>
                      <span> {vehicle?.vehicleName}</span>
                    </h3>
                  )}
                  {vehicle?.vehicleLicensePlate && (
                    <h3>
                      <span className='font-semibold'>Biển số:</span>
                      <span> {vehicle?.vehicleLicensePlate}</span>
                    </h3>
                  )}

                  {vehicle?.registerDate && (
                    <h3>
                      <span className='font-semibold'>Ngày đăng kí:</span>
                      <span>
                        {' '}
                        {dayjs(vehicle?.registerDate).format('DD/MM/YYYY')}
                      </span>
                    </h3>
                  )}
                  <div className='w-full'>
                    <img
                      src='https://upload.wikimedia.org/wikipedia/commons/thumb/1/18/Land_Rover_Range_Rover_Autobiography_2016.jpg/1200px-Land_Rover_Range_Rover_Autobiography_2016.jpg'
                      alt='transport'
                      className='w-[300px]'
                    />
                  </div>
                </div>
              </div>
            )}
          </div>
          <DialogActions>
            <Button onClick={() => setOpen(false)}>Thoát</Button>
          </DialogActions>
        </Dialog>
      )}
    </div>
  );
};

export default DialogDetailSchedule;
