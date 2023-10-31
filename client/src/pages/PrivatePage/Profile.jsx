import { Box } from "@mui/material";
import * as dayjs from "dayjs";
import React from "react";
import { useSelector } from "react-redux";
import SideBar from "../../components/SideBar";
import DialogEditUser from './components/DialogEditUser';
const Profile = () => {
  const { user, isLoading } = useSelector((state) => state.auth);
  const accInfo = user.accountInfo;
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
                className="absolute bottom-[-50px] left-8 w-[100px] h-[100px] object-cover rounded-full"
              />
            </div>
            <div className="grid grid-cols-2 gap-8 mt-8">
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
              <div className="flex-x gap-4">
                <span className="text-[20px] font-semibold">
                  Loại bằng lái:
                </span>
                <span className="text-[20px] font-normal">
                  {accInfo.licenseType.licenseTypeDesc}
                </span>
              </div>
              <div className="flex-x gap-4">
                <span className="text-[20px] font-semibold">Địa chỉ:</span>
                <span className="text-[20px] font-normal">{`${accInfo.address.street}, ${accInfo.address.district}, ${accInfo.address.city}`}</span>
              </div>
            </div>
            <div className="flex justify-end w-full">
              <DialogEditUser />
            </div>
          </div>
        </div>
        
      </Box>
    </div>
  );
};

export default Profile;
