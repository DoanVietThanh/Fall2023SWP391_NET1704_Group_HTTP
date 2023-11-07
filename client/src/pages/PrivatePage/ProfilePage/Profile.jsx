import React, { useState } from "react";
import * as dayjs from "dayjs";
import { Box } from "@mui/material";
import { useSelector } from "react-redux";
import SideBar from "../../../components/SideBar";
import DialogEditProfile from "./components/DialogEditProfile";
import DialogRegisterForm from "./components/DialogRegisterForm";

const Profile = () => {
  const { user, isLoading } = useSelector((state) => state.auth);
  const accInfo = user.accountInfo;
  const [openEditProfile, setOpenEditProfile] = useState(false);
  const [openRegisterForm, setOpenRegisterForm] = useState(false);
  return (
    <div className="flex">
      <SideBar />
      <Box component="main" sx={{ flexGrow: 1, p: 3 }}>
        <div className="h-[80vh] w-full rounded overflow-y-auto mt-[64px]">
          <div className="flex flex-col justify-between h-full">
            <div className="relative mb-8">
              <img
                src="/img/bg-profile2.jpg"
                alt=""
                className="h-[40vh] object-cover w-full rounded-lg"
              />
              <img
                src={`/img/avtThanh.jpg`}
                alt="avt"
                className="absolute bottom-[-90px] left-16 w-[180px] h-[180px] object-cover rounded-full border-8 border-white"
              />
            </div>
            <div className="grid grid-cols-2 gap-8 mt-20 ml-16">
              <div className="flex-x gap-4 w-full">
                <span className="text-[20px] font-semibold">Họ và tên:</span>
                <span className="text-[20px] font-normal">
                  {`${accInfo.firstName} ${accInfo.lastName}`}
                </span>
              </div>

              <div className="flex-x gap-4">
                <span className="text-[20px] font-semibold">Ngày sinh:</span>
                <span className="text-[20px] font-normal">
                  {dayjs(accInfo.dateBirth).format("DD/MM/YYYY")}
                </span>
              </div>

              <div className="flex-x gap-4">
                <span className="text-[20px] font-semibold">Email:</span>
                <span className="text-[20px] font-normal">{accInfo.email}</span>
              </div>

              <div className="flex-x gap-4">
                <span className="text-[20px] font-semibold">
                  Số điện thoại:
                </span>
                <span className="text-[20px] font-normal">{accInfo.phone}</span>
              </div>
              {/* <div className='flex-x gap-4'>
                <span className='text-[20px] font-semibold'>
                  Loại bằng lái:
                </span>
                <span className='text-[20px] font-normal'>
                  {accInfo.licenseType.licenseTypeDesc}
                </span>
              </div> */}
              <div className="flex-x gap-4">
                <span className="text-[20px] font-semibold">Địa chỉ:</span>
                <span className="text-[20px] font-normal">{`${accInfo.address.street}, ${accInfo.address.district}, ${accInfo.address.city}`}</span>
              </div>
            </div>
            <div className="flex justify-end w-full gap-4 pr-4">
              <button className="btn mt-4" onClick={() => setOpenEditProfile(true)}>Chỉnh sửa</button>
              <button className="btn mt-4" onClick={() => setOpenRegisterForm(true)}>Tạo hồ sơ thi</button>
            </div>
          </div>
              
          <DialogEditProfile open={openEditProfile} setOpen={setOpenEditProfile}/>
          <DialogRegisterForm open={openRegisterForm} setOpen={setOpenRegisterForm}/>
        
        </div>
      </Box>
    </div>
  );
};

export default Profile;
