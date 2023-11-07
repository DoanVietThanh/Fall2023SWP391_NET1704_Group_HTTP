import {
  Dialog,
  FormControl,
  MenuItem,
  Select,
  TextField,
} from "@mui/material";
import axios from "axios";
import { useFormik } from "formik";
import React, { useState } from "react";
import { Navigate } from "react-router-dom";
import * as yup from "yup";
import { toastError, toastSuccess } from "../../../../components/Toastify";

const DialogRegisterForm = ({ open, setOpen }) => {
  const baseServer = process.env.REACT_APP_SERVER_API;

  const currentDate = new Date();
  const day = currentDate.getDate();
  const month = currentDate.getMonth() + 1;
  const year = currentDate.getFullYear();
  const [isChecked, setChecked] = useState(false);

  const [selectedGender, setSelectedGender] = useState("nam");

  const handleGenderChange = (event) => {
    setSelectedGender(event.target.value);
  };

  const handleCheckboxChange = () => {
    setChecked(!isChecked);
  };

  const schema = yup.object().shape({
    fullname: yup
      .string()
      .matches("^[a-zA-Z ]+$", "Không chứa số hay kí tự đặc biệt")
      .required("Nhập họ tên "),
    nation: yup
      .string()
      .matches("^[a-zA-Z ]+$", "Không chứa số hay kí tự đặc biệt")
      .required("Nhập quốc tịch"),
    dob: yup.string().required("Nhập ngày sinh"),
    registerPlace: yup.string().required("Nhập nơi đăng ký thường trú"),
    livePlace: yup.string().required("Nhập nơi cư trú"),
    identifyID: yup
      .string()
      .matches(
        "^[0-9]{9,12}$",
        "Số CMND hoặc CCCD hoặc hộ chiếu có độ dài 9-12"
      )
      .required("Nhập số CMND hoặc CCCD hoặc hộ chiếu"),
    dateOfIdenID: yup.string().required("Nhập ngày cấp"),
    placeOfIdenID: yup
      .string()
      .matches("^[a-zA-Z ]+$", "Không chứa số hay kí tự đặc biệt")
      .required("Nhập nơi cấp"),
    existNumOfDriLicense: yup
      .string()
      .matches("^[0-9]{9,12}$", "Số của giấy phép lái xe có độ dài 12")
      .required("Nhập số của giấy phép lái xe đã có nếu không có nhập 0 "),
    typeOfDriLicense: yup.string().required("Nhập hạng nếu không có nhập 0"),
    provideBy: yup
      .string()
      .matches("^[a-zA-Z ]+$", "Không chứa số hay kí tự đặc biệt")
      .required("Nhập nơi cấp"),
    providedDate: yup.string().required("Nhập ngày cấp"),
    typeLicenseRegister: yup
      .string()
      .required("Nhập hạng bằng lái muốn đăng ký"),
    otherInfo: yup.string().required("Nhập tài liệu liên quan"),
  });

  const formik = useFormik({
    initialValues: {
      fullname: "",
      nation: "",
      dob: "",
      sex: "",
      registerPlace: "",
      livePlace: "",
      identifyID: "",
      dateOfIdenID: "",
      placeOfIdenID: "",
      existNumOfDriLicense: "",
      typeOfDriLicense: "",
      provideBy: "",
      providedDate: "",
      typeLicenseRegister: "",
      otherInfo: "",
    },
    validationSchema: schema,
    onSubmit: async (values) => {
      const result = await axios.post(`${baseServer}/authentication`, values);
      if (result.data.statusCode === 200) {
        toastSuccess(result.data.message);
        Navigate("/login");
      } else {
        toastError("Something went wrong");
      }
      console.log("result: ", result);
    },
  });
  return (
    <div>
      <Dialog
        open={open}
        onClose={() => setOpen(false)}
        fullWidth
        maxWidth="lg"
      >
        {/* <div className="flex justify-end">
          <button className="btn">Xuất pdf</button>
        </div> */}
        <div className="m-4">
          <div className="my-4 mx-16">
            <div className=" flex flex-col gap-4">
              <div className="flex flex-col gap-2">
                <div className="uppercase text-xl font-semibold text-center">
                  Cộng hòa xã hội chủ nghĩa Việt Nam
                </div>

                <div className="text-lg font-semibold text-center underline underline-offset-8">
                  Độc lập - Tự do - Hạnh phúc
                </div>
              </div>

              <div className="flex gap-32 items-center">
                <div className="w-[120px] h-[160px] border p-4 text-md">
                  Ảnh màu 3cm x 4cm chụp không quá 06 tháng
                </div>

                <div className="flex flex-col gap-6">
                  <div className="uppercase text-lg font-semibold text-center">
                    Đơn đề nghị học, sát hạch để cấp giấy phép lái xe
                  </div>
                  <div className="text-lg font-semibold text-center">
                    Kính gửi: SỞ GIAO THÔNG VẬN TẢI HỒ CHÍ MINH
                  </div>
                </div>
              </div>

              <div className="px-8 flex flex-col gap-2 text-lg">
                {/* ten, quoc tich */}
                <div className="flex gap-4 items-center justify-between">
                  <div className="flex-1">
                    <div className="flex gap-2 justify-between items-center ">
                      <p className="whitespace-nowrap">Tôi là</p>
                      <TextField
                        id="field"
                        label="Họ và tên"
                        type="text"
                        fullWidth
                        variant="outlined"
                        onChange={formik.handleChange("fullname")}
                        onBlur={formik.handleBlur("fullname")}
                        value={formik.values.fullname}
                      />
                    </div>
                    <div className="error text-red-900">
                      {formik.touched.fullname && formik.errors.fullname}
                    </div>
                  </div>
                  <div className="">
                    <div className="flex gap-2 justify-between items-center ">
                      <p className="whitespace-nowrap">Quốc tịch</p>
                      <TextField
                        id="field"
                        type="text"
                        fullWidth
                        variant="outlined"
                        onChange={formik.handleChange("nation")}
                        onBlur={formik.handleBlur("nation")}
                        value={formik.values.nation}
                      />
                    </div>
                    <div className="error text-red-900">
                      {formik.touched.nation && formik.errors.nation}
                    </div>
                  </div>
                </div>
                {/* ngay sinh , gioi tinh */}
                <div className="flex gap-4 items-center">
                  <div className="flex-1">
                    <div className="flex gap-2 justify-between items-center ">
                      <p className="whitespace-nowrap">Sinh ngày</p>
                      <TextField
                        id="field"
                        type="date"
                        fullWidth
                        variant="outlined"
                        onChange={formik.handleChange("dob")}
                        onBlur={formik.handleBlur("dob")}
                        value={formik.values.dob}
                      />
                    </div>
                    <div className="error text-red-900">
                      {formik.touched.dob && formik.errors.dob}
                    </div>
                  </div>
                  <div className="w-[30%]">
                    <div className="flex gap-2 items-center ">
                      <p>Chọn giới tính</p>
                      <FormControl variant="outlined" className="flex-1">
                        <Select
                          value={selectedGender}
                          onChange={handleGenderChange}
                        >
                          <MenuItem value="nam">Nam</MenuItem>
                          <MenuItem value="nữ">Nữ</MenuItem>
                        </Select>
                      </FormControl>
                    </div>
                    <div className="error text-red-900">
                      {formik.touched.sex && formik.errors.sex}
                    </div>
                  </div>
                </div>
                {/* noi dki ho khau thuong tru */}
                <div>
                  <div className="flex gap-2 justify-between items-center ">
                    <p className="whitespace-nowrap">
                      Nơi đăng ký hộ khẩu thường trú
                    </p>
                    <TextField
                      id="field"
                      type="text"
                      fullWidth
                      variant="outlined"
                      onChange={formik.handleChange("registerPlace")}
                      onBlur={formik.handleBlur("registerPlace")}
                      value={formik.values.registerPlace}
                    />
                  </div>
                  <div className="error text-red-900">
                    {formik.touched.registerPlace &&
                      formik.errors.registerPlace}
                  </div>
                </div>
                {/* noi cu tru */}
                <div>
                  <div className="flex gap-4 justify-between items-center ">
                    <p className="whitespace-nowrap">Nơi cư trú</p>
                    <TextField
                      id="field"
                      type="text"
                      fullWidth
                      variant="outlined"
                      onChange={formik.handleChange("livePlace")}
                      onBlur={formik.handleBlur("livePlace")}
                      value={formik.values.livePlace}
                    />
                  </div>
                  <div className="error text-red-900">
                    {formik.touched.livePlace && formik.errors.livePlace}
                  </div>
                </div>
                {/* cmnd/cccd */}
                <div>
                  <div className="flex gap-2 justify-between items-center ">
                    <p className="whitespace-nowrap">
                      Số chứng minh nhân dân hoặc thẻ căn cước công dân {"("}
                      hoặc hộ chiếu{")"}
                    </p>
                    <TextField
                      id="field"
                      fullWidth
                      variant="outlined"
                      onChange={formik.handleChange("identifyID")}
                      onBlur={formik.handleBlur("identifyID")}
                      value={formik.values.identifyID}
                    />
                  </div>
                  <div className="error text-red-900">
                    {formik.touched.identifyID && formik.errors.identifyID}
                  </div>
                </div>
                {/* ngay cap, noi cap */}
                <div className="flex gap-4 items-center justify-between">
                  <div className="w-[30%]">
                    <div className="flex gap-2 justify-between items-center ">
                      <p className="whitespace-nowrap">Ngày cấp</p>
                      <TextField
                        id="field"
                        type="date"
                        fullWidth
                        variant="outlined"
                        onChange={formik.handleChange("dateOfIdenID")}
                        onBlur={formik.handleBlur("dateOfIdenID")}
                        value={formik.values.dateOfIdenID}
                      />
                    </div>
                    <div className="error text-red-900">
                      {formik.touched.dateOfIdenID &&
                        formik.errors.dateOfIdenID}
                    </div>
                  </div>
                  <div className="flex-1">
                    <div className="flex gap-2 justify-between items-center ">
                      <p className="whitespace-nowrap">Nơi cấp</p>
                      <TextField
                        id="field"
                        type="text"
                        fullWidth
                        variant="outlined"
                        onChange={formik.handleChange("placeOfIdenID")}
                        onBlur={formik.handleBlur("placeOfIdenID")}
                        value={formik.values.placeOfIdenID}
                      />
                    </div>
                    <div className="error text-red-900">
                      {formik.touched.placeOfIdenID &&
                        formik.errors.placeOfIdenID}
                    </div>
                  </div>
                </div>
                {/* da co giay phep, hang */}
                <div className="flex gap-4 items-center justify-between">
                  <div className="flex-1">
                    <div className="flex gap-2 justify-between items-center ">
                      <p className="whitespace-nowrap">
                        Đã có giấy phép lái xe số
                      </p>
                      <TextField
                        id="field"
                        fullWidth
                        variant="outlined"
                        label="Nếu không có nhập 0"
                        onChange={formik.handleChange("existNumOfDriLicense")}
                        onBlur={formik.handleBlur("existNumOfDriLicense")}
                        value={formik.values.existNumOfDriLicense}
                      />
                    </div>
                    <div className="error text-red-900">
                      {formik.touched.existNumOfDriLicense &&
                        formik.errors.existNumOfDriLicense}
                    </div>
                  </div>
                  <div>
                    <div className="flex gap-2 justify-between items-center ">
                      <p className="whitespace-nowrap">Hạng</p>
                      <TextField
                        id="field"
                        type="text"
                        fullWidth
                        label="Nếu không có nhập 0"
                        variant="outlined"
                        onChange={formik.handleChange("typeOfDriLicense")}
                        onBlur={formik.handleBlur("typeOfDriLicense")}
                        value={formik.values.typeOfDriLicense}
                      />
                    </div>
                    <div className="error text-red-900">
                      {formik.touched.typeOfDriLicense &&
                        formik.errors.typeOfDriLicense}
                    </div>
                  </div>
                </div>
                {/* do, cap ngay */}
                <div className="flex gap-4 items-center justify-between">
                  <div className="flex-1">
                    <div className="flex gap-2 justify-between items-center ">
                      <p className="whitespace-nowrap">do</p>
                      <TextField
                        id="field"
                        fullWidth
                        variant="outlined"
                        label="Nếu không có nhập 0"
                        onChange={formik.handleChange("provideBy")}
                        onBlur={formik.handleBlur("provideBy")}
                        value={formik.values.provideBy}
                      />
                    </div>
                    <div className="error text-red-900">
                      {formik.touched.provideBy && formik.errors.provideBy}
                    </div>
                  </div>
                  <div>
                    <div className="flex gap-2 justify-between items-center ">
                      <p className="whitespace-nowrap">Cấp ngày</p>
                      <TextField
                        id="field"
                        type="date"
                        fullWidth
                        variant="outlined"
                        onChange={formik.handleChange("providedDate")}
                        onBlur={formik.handleBlur("providedDate")}
                        value={formik.values.providedDate}
                      />
                    </div>
                    <div className="error text-red-900">
                      {formik.touched.providedDate &&
                        formik.errors.providedDate}
                    </div>
                  </div>
                </div>
                {/* de nghi */}
                <div>
                  <div className="flex gap-2 justify-between items-center ">
                    <p className="whitespace-nowrap">
                      Đề nghị cho tôi được học, dự sát hạch để cấp giấy phép lái
                      xe hạng
                    </p>
                    <TextField
                      id="field"
                      type="text"
                      fullWidth
                      variant="outlined"
                      onChange={formik.handleChange("typeLicenseRegister")}
                      onBlur={formik.handleBlur("typeLicenseRegister")}
                      value={formik.values.typeLicenseRegister}
                    />
                  </div>
                  <div className="error text-red-900">
                    {formik.touched.typeLicenseRegister &&
                      formik.errors.typeLicenseRegister}
                  </div>
                </div>
                {/* dang ky tich hop */}
                <div>
                  <div className="flex gap-4 items-center ">
                    <p className="whitespace-nowrap">
                      Đăng ký tích hợp giấy phép lái xe
                    </p>
                    <input
                      type="checkbox"
                      checked={isChecked}
                      onChange={handleCheckboxChange}
                      className="w-[30px] h-[30px]"
                    />
                  </div>
                </div>

                {/* dinh kem */}
                <div>Xin gửi kèm theo:</div>
                <div>- 01 giấy chứng nhận đủ sức khỏe</div>
                <div>- 02 ảnh màu cỡ 3cmx4cm, chụp không quá 06 tháng</div>
                <div>
                  - Bản sao Giấy chứng minh nhân dân hoặc thẻ căn cước công dân
                  hoặc hộ chiếu còn thời hạn có ghi số giấy chứng minh nhân dân
                  hoặc thẻ căn cước công dân (đối với người Việt Nam) hoặc hộ
                  chiếu (đối với người nước ngoài)
                </div>

                {/* tai lieu khac */}
                <div>
                  <div className="flex gap-2 justify-between items-center ">
                    <p className="whitespace-nowrap">
                      - Các tài liệu khác có liên quan gồm:
                    </p>
                    <TextField
                      id="field"
                      type="text"
                      label="Nếu không có nhập 0"
                      fullWidth
                      variant="outlined"
                      onChange={formik.handleChange("otherInfo")}
                      onBlur={formik.handleBlur("otherInfo")}
                      value={formik.values.otherInfo}
                    />
                  </div>
                  <div className="error text-red-900">
                    {formik.touched.otherInfo && formik.errors.otherInfo}
                  </div>
                </div>

                <div>
                  Tôi xin cam đoan những điều ghi trên đây là đúng sự thật nếu
                  sai tôi xin hoàn toàn chịu trách nhiệm.
                </div>
                <div className="flex">
                  <div className=" flex flex-col w-[40%] ml-auto items-center">
                    <div>
                      Hồ Chí Minh, ngày <span>{day}</span> tháng{" "}
                      <span>{month}</span> năm <span>{year}</span>
                    </div>
                    <div className="uppercase font-semibold">Người làm đơn</div>
                    <div>
                      {"("}Ký và ghi rõ họ, tên{")"}
                    </div>
                    <div>Thư</div>
                    <div>Bùi Trần Thanh Thư</div>
                  </div>
                </div>
              </div>
            </div>
            <form action="/upload" method="post" enctype="multipart/form-data">
              <label for="fileToUpload" className="flex gap-2">
                <span>Chọn tệp cần tải lên</span>
                <input
                  type="file"
                  name="fileToUpload"
                  id="fileToUpload"
                  accept=".pdf, .doc, .docx"
                />
              </label>
              <input type="submit" value="Tải lên" name="submit" />
            </form>
            {/* <form action="/upload" method="post" enctype="multipart/form-data">
              <input
                type="file"
                name="fileToUpload"
                id="fileToUpload"
              />
              <input type="submit" value="Tải lên" name="submit" />
            </form> */}
          </div>
          <div className="flex gap-2 justify-end mt-4">
            <button className="btnCancel" onClick={() => setOpen(false)}>
              Hủy
            </button>
            <button className="btn" onClick={() => setOpen(false)}>
              Hoàn tất
            </button>
          </div>
        </div>
      </Dialog>
    </div>
  );
};

export default DialogRegisterForm;
