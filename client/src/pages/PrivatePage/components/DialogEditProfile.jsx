import { FormControl, InputLabel, MenuItem, Select } from "@mui/material";
import Dialog from "@mui/material/Dialog";
import TextField from "@mui/material/TextField";
import axios from "axios";
import { useFormik } from "formik";
import React, { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import * as yup from "yup";
import { toastError, toastSuccess } from "../../components/Toastify";

const EditProfileForm = () => {
  const baseServer = process.env.REACT_APP_SERVER_API;
  const [lisenceType, setLisenceType] = useState([]);
  const [selectType, setSelectType] = useState("1");

  useEffect(() => {
    async function fetchType() {
      const response = await axios.get(`${baseServer}/authentication`);
      setLisenceType(response.data.data);
    }
    fetchType();
  }, []);
  const schema = yup.object().shape({
    firstName: yup
      .string()
      .max(10, "Tên có nhiều nhất 10 kí tự")
      .matches("^[a-zA-Z ]+$", "Không chứa số hay kí tự đặc biệt")
      .required("Nhập tên"),
    lastName: yup
      .string()
      .max(10, "Họ có nhiều nhất 10 kí tự")
      .matches("^[a-zA-Z ]+$", "Không chứa số hay kí tự đặc biệt")
      .required("Nhập họ"),
    dateBirth: yup.string().required("Nhập ngày sinh"),
    phone: yup
      .string()
      .matches("^0[0-9]{9,11}$", "Số điện thoại có độ dài 10-12")
      .required("Nhập số điện thoại"),
    street: yup
      .string()
      .matches("^[a-zA-Z0-9 ]+$", "Không chứa số hay kí tự đặc biệt")
      .required("Nhập đường"),
    district: yup
      .string()
      .matches("^[a-zA-Z ]+$", "Không chứa số hay kí tự đặc biệt")
      .required("Nhập huyện/quận"),
    city: yup
      .string()
      .matches("^[a-zA-Z ]+$", "Không chứa số hay kí tự đặc biệt")
      .required("Nhập tỉnh/thành phố"),
  });

  const formik = useFormik({
    initialValues: {
      firstName: "",
      lastName: "",
      dateBirth: "",
      phone: "",
      street: "",
      district: "",
      city: "",
      licenseTypeId: selectType,
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
  const [open, setOpen] = React.useState(false);

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  return (
    <div>
      {/* <button className="btn" onClick={handleClickOpen}>
        Chỉnh sửa
      </button> */}
      <Dialog open={open} onClose={handleClose}>
        <div className="py-4 px-8 flex flex-col gap-4">
          <div className="dialogTit">Chỉnh sửa thông tin cá nhân</div>
          <div className="flex gap-4">
            {/* lastName */}
            <div>
              <TextField
                id="field"
                label="Họ"
                fullWidth
                variant="outlined"
                onChange={formik.handleChange("lastName")}
                onBlur={formik.handleBlur("lastName")}
                value={formik.values.lastName}
              />

              <div className="error text-red-900">
                {formik.touched.lastName && formik.errors.lastName}
              </div>
            </div>
            {/* firstName */}
            <div>
              <TextField
                id="field"
                label="Tên"
                fullWidth
                variant="outlined"
                onChange={formik.handleChange("firstName")}
                onBlur={formik.handleBlur("firstName")}
                value={formik.values.firstName}
              />
              <div className="error text-red-900">
                {formik.touched.firstName && formik.errors.firstName}
              </div>
            </div>
          </div>

          {/* dateBirth */}
          <div>
            <div className="flex gap-4 justify-between items-center ">
              <p className="whitespace-nowrap text-lg">Ngày sinh</p>
              <TextField
                id="field"
                type="date"
                fullWidth
                variant="outlined"
                onChange={formik.handleChange("dateBirth")}
                onBlur={formik.handleBlur("dateBirth")}
                value={formik.values.dateBirth}
              />
            </div>
            <div className="error text-red-900">
              {formik.touched.dateBirth && formik.errors.dateBirth}
            </div>
          </div>

          {/* phone */}
          <div>
            <TextField
              id="field"
              label="Số điện thoại"
              fullWidth
              variant="outlined"
              onChange={formik.handleChange("phone")}
              onBlur={formik.handleBlur("phone")}
              value={formik.values.phone}
            />
            <div className="error text-red-900">
              {formik.touched.phone && formik.errors.phone}
            </div>
          </div>

          {/* Select Type  */}
          <div>
            <FormControl fullWidth>
              <InputLabel id="demo-simple-select-label">
                Lisence Type
              </InputLabel>
              <Select
                labelId="demo-simple-select-label"
                id="demo-simple-select"
                value={formik.values.licenseTypeId}
                label="License Type"
                onChange={(event) => setSelectType(event.target.value)}
              >
                {lisenceType?.map((item) => (
                  <MenuItem value={item.licenseTypeId} key={item.lisenceTypeId}>
                    {item.licenseTypeDesc}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </div>

          {/* street */}
          <div>
            <TextField
              id="field"
              label="Đường"
              fullWidth
              variant="outlined"
              onChange={formik.handleChange("street")}
              onBlur={formik.handleBlur("street")}
              value={formik.values.street}
            />
            <div className="error text-red-900">
              {formik.touched.street && formik.errors.street}
            </div>
          </div>

          {/* district */}
          <div>
            <TextField
              id="field"
              label="Huyện/Quận"
              fullWidth
              variant="outlined"
              onChange={formik.handleChange("district")}
              onBlur={formik.handleBlur("district")}
              value={formik.values.district}
            />
            <div className="error text-red-900">
              {formik.touched.district && formik.errors.district}
            </div>
          </div>

          {/* city */}
          <div>
            <TextField
              id="field"
              label="Tỉnh/Thành phố"
              fullWidth
              variant="outlined"
              onChange={formik.handleChange("city")}
              onBlur={formik.handleBlur("city")}
              value={formik.values.city}
            />
            <div className="error text-red-900">
              {formik.touched.city && formik.errors.city}
            </div>
          </div>
          <div className="flex gap-2 justify-end">
            <button className="btnCancel" onClick={handleClose}>
              Hủy
            </button>
            <button className="btn" onClick={handleClose}>
              Hoàn tất
            </button>
          </div>
        </div>
      </Dialog>
    </div>
  );
};

export default EditProfileForm;
