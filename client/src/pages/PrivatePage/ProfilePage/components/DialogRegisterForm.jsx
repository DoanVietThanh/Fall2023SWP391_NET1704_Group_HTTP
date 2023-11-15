import React, { useState } from 'react';
import * as yup from 'yup';
import * as dayjs from 'dayjs';
import { useFormik } from 'formik';
import {
  Button,
  Checkbox,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  MenuItem,
  Select,
  TextField,
} from '@mui/material';
import axiosClient from '../../../../utils/axiosClient';
import { toastError, toastSuccess } from '../../../../components/Toastify';
import axiosForm from '../../../../utils/axiosForm';

const schema = yup.object().shape({
  image: yup.string().required('Vui lòng chọn hình ảnh'),
  identityCardImage: yup
    .string()
    .required('Vui lòng chọn Ảnh căn cước công dân'),
  healthCertificationImage: yup
    .string()
    .required('Vui lòng tải Ảnh khám sức khỏe'),
  licenseTypeId: yup.string().required('Vui lòng Chọn licenseTypeId'),
  permanentAddress: yup.string().required('Vui lòng Nhập địa chỉ thường trú'),
  identityNumber: yup
    .string()
    .min('9', 'Độ dài số CMND ít nhất 9 kí tự')
    .required('Vui lòng Nhập số CMND'),
  gender: yup.string().required('Vui lòng chọn giới tính'),
  identityCardIssuedDate: yup.string().required('Vui lòng nhập ngày cấp thẻ'),
  identityCardIssuedBy: yup.string().required('Vui lòng nhập nơi cấp thẻ'),
  licenseTypeIssuedDate: yup.string().required('Vui lòng nhập ngày cấp thẻ'),
  availableLicenseType: yup
    .string()
    .required('Vui lòng nhập loại bằng lái sẵn có'),
});

const DialogRegisterForm = ({ open, setOpen, accInfo, licenseForm }) => {
  const [isChecked, setChecked] = useState(false);
  const [existedLicenseType, setExistedLicenseType] = useState(true);

  const [previewImg, setPreviewImg] = useState(null);
  const [previewIdentityImage, setPreviewIdentityImage] = useState(null);
  const [previewHealthImage, setPreviewHealthImage] = useState(null);

  const handleCheckboxChange = () => {
    setChecked(!isChecked);
  };

  const handlePreviewImg = (e) => {
    const file = e.target.files[0];
    file.preview = URL.createObjectURL(file);
    setPreviewImg(file);
  };

  const handlePreviewIdentityImage = (e) => {
    const file = e.target.files[0];
    file.preview = URL.createObjectURL(file);
    setPreviewIdentityImage(file);
  };

  const handlePreviewHealthImage = (e) => {
    const file = e.target.files[0];
    file.preview = URL.createObjectURL(file);
    setPreviewHealthImage(file);
  };

  const formik = useFormik({
    initialValues: {
      memberId: accInfo.memberId,
      image: '',
      identityCardImage: '',
      healthCertificationImage: '',
      licenseTypeId: '',
      permanentAddress: '',
      identityNumber: '',
      gender: 'Nam',
      identityCardIssuedDate: '',
      identityCardIssuedBy: '',
      licenseTypeIssuedDate: '',
      availableLicenseType: '',
    },
    validationSchema: schema,
    onSubmit: async (values) => {
      console.log('values: ', values);
      await axiosForm
        .post(`/members/license-form`, values)
        .then((res) => {
          console.log(res);
          toastSuccess('Tạo hồ sơ thi thành công');
          setOpen(false);
          handleClearImgages();
        })
        .catch((error) => toastError(error?.response?.data?.message));
    },
  });

  console.log('formik.values: ', formik.values);

  const handleClearImgages = () => {
    setPreviewImg(null);
    setPreviewHealthImage(null);
    setPreviewIdentityImage(null);
    formik.setFieldValue('image', '');
    formik.setFieldValue('identityCardImage', '');
    formik.setFieldValue('healthCertificationImage', '');
  };

  return (
    <div>
      <Dialog
        open={open}
        onClose={() => setOpen(false)}
        fullWidth
        maxWidth='lg'
      >
        {/* <div className="flex justify-end">
          <button className="btn">Xuất pdf</button>
        </div> */}
        <DialogTitle id='alert-dialog-title'>
          <div className='bg-gradient-to-r from-blue-500 to-fuchsia-500 text-center text-white font-bold p-2 rounded capitalize'>
            Hồ sơ đăng kí thi bằng lái
          </div>
        </DialogTitle>
        <DialogContent>
          <div className='m-4'>
            <div className='my-4 mx-16'>
              <div className=' flex flex-col gap-4'>
                <div className='flex flex-col gap-2'>
                  <div className='uppercase text-xl font-semibold text-center'>
                    Cộng hòa xã hội chủ nghĩa Việt Nam
                  </div>

                  <div className='text-lg font-semibold text-center underline underline-offset-8'>
                    Độc lập - Tự do - Hạnh phúc
                  </div>
                </div>

                {/* Ảnh 3x4 */}

                <div className='flex gap-32 items-center'>
                  <div className='w-[150px] h-[200px] border flex justify-center items-center'>
                    <div className='flex justify-center items-center'>
                      {previewImg ? (
                        <img
                          src={previewImg.preview}
                          alt='img'
                          className='w-[150px] h-[200px] overflow-hidden object-cover'
                        />
                      ) : (
                        <label
                          for='file-upload'
                          class='custom-file-upload h-full cursor-pointer py-8 px-2'
                        >
                          Ảnh màu 3cm x 4cm chụp không quá 06 tháng
                        </label>
                      )}
                    </div>
                  </div>
                  <input
                    id='file-upload'
                    type='file'
                    className='hidden'
                    onChange={(e) => {
                      handlePreviewImg(e);
                      formik.setFieldValue('image', e.target.files[0]);
                    }}
                  />

                  <div className='flex flex-col gap-6'>
                    <div className='uppercase text-lg font-semibold text-center'>
                      Đơn đề nghị học, sát hạch để cấp giấy phép lái xe
                    </div>
                    <div className='text-lg font-semibold text-center'>
                      Kính gửi: SỞ GIAO THÔNG VẬN TẢI HỒ CHÍ MINH
                    </div>
                  </div>
                </div>

                <div className='px-8 flex flex-col gap-2 text-lg'>
                  {/* ten, quoc tich */}
                  <div className='flex gap-4 items-center justify-between'>
                    <div className='flex-1'>
                      <div className='flex gap-2 justify-between items-center '>
                        <p className='whitespace-nowrap'>Tôi là</p>
                        <TextField
                          id='field'
                          label='Họ và tên'
                          type='text'
                          fullWidth
                          variant='outlined'
                          value={`${accInfo?.firstName} ${accInfo?.lastName}`}
                          InputProps={{
                            readOnly: true,
                          }}
                        />
                      </div>
                    </div>
                  </div>
                  {/* ngay sinh , gioi tinh */}
                  <div className='flex gap-4 items-center'>
                    <div className='flex-1'>
                      <div className='flex gap-2 justify-between items-center '>
                        <p className='whitespace-nowrap'>Sinh ngày</p>
                        <TextField
                          id='field'
                          type='date'
                          fullWidth
                          variant='outlined'
                          value={dayjs(accInfo?.dateBirth).format('YYYY-MM-DD')}
                          InputProps={{
                            readOnly: true,
                          }}
                        />
                      </div>
                    </div>
                    <div className='w-[30%]'>
                      <div className='flex gap-2 items-center '>
                        <p>Chọn giới tính</p>
                        <FormControl variant='outlined' className='flex-1'>
                          <Select
                            value={formik.values.gender}
                            onChange={(e) =>
                              formik.setFieldValue('gender', e.target.value)
                            }
                          >
                            <MenuItem value='Nam'>Nam</MenuItem>
                            <MenuItem value='Nữ'>Nữ</MenuItem>
                          </Select>
                        </FormControl>
                      </div>
                      <div className='error text-red-900'>
                        {formik.touched.gender && formik.errors.gender}
                      </div>
                    </div>
                  </div>

                  {/* Noi dki ho khau thuong tru */}
                  <div>
                    <div className='flex gap-2 justify-between items-center '>
                      <p className='whitespace-nowrap'>
                        Nơi đăng ký hộ khẩu thường trú
                      </p>
                      <TextField
                        id='field'
                        type='text'
                        fullWidth
                        variant='outlined'
                        onChange={formik.handleChange('permanentAddress')}
                        onBlur={formik.handleBlur('permanentAddress')}
                        value={formik.values.permanentAddress}
                      />
                    </div>
                    <div className='error text-red-900'>
                      {formik.touched.permanentAddress &&
                        formik.errors.permanentAddress}
                    </div>
                  </div>

                  {/* cmnd/cccd */}
                  <div>
                    <div className='flex gap-2 justify-between items-center '>
                      <p className='whitespace-nowrap'>
                        Số chứng minh nhân dân hoặc thẻ căn cước công dân {'('}
                        hoặc hộ chiếu{')'}
                      </p>
                      <TextField
                        id='field'
                        fullWidth
                        variant='outlined'
                        onChange={formik.handleChange('identityNumber')}
                        onBlur={formik.handleBlur('identityNumber')}
                        value={formik.values.identityNumber}
                      />
                    </div>
                    <div className='error text-red-900'>
                      {formik.touched.identityNumber &&
                        formik.errors.identityNumber}
                    </div>
                  </div>

                  {/* ngay cap, noi cap */}
                  <div className='flex gap-4 items-center justify-between'>
                    <div className='w-[30%]'>
                      <div className='flex gap-2 justify-between items-center '>
                        <p className='whitespace-nowrap'>Ngày cấp</p>
                        <TextField
                          id='field'
                          type='date'
                          fullWidth
                          variant='outlined'
                          onChange={formik.handleChange(
                            'identityCardIssuedDate'
                          )}
                          onBlur={formik.handleBlur('identityCardIssuedDate')}
                          value={formik.values.identityCardIssuedDate}
                        />
                      </div>
                      <div className='error text-red-900'>
                        {formik.touched.identityCardIssuedDate &&
                          formik.errors.identityCardIssuedDate}
                      </div>
                    </div>

                    <div className='flex-1'>
                      <div className='flex gap-2 justify-between items-center '>
                        <p className='whitespace-nowrap'>Nơi cấp</p>
                        <TextField
                          id='field'
                          type='text'
                          fullWidth
                          variant='outlined'
                          onChange={formik.handleChange('identityCardIssuedBy')}
                          onBlur={formik.handleBlur('identityCardIssuedBy')}
                          value={formik.values.identityCardIssuedBy}
                        />
                      </div>
                      <div className='error text-red-900'>
                        {formik.touched.identityCardIssuedBy &&
                          formik.errors.identityCardIssuedBy}
                      </div>
                    </div>
                  </div>

                  {/* Đã co giay phep, hang */}
                  <div className='flex gap-4 items-center justify-between'>
                    <div className='flex-1'>
                      <div className='flex gap-2 justify-start items-center '>
                        <p className='whitespace-nowrap'>
                          Đã có giấy phép lái xe
                        </p>
                        <Checkbox
                          checked={existedLicenseType}
                          onChange={(e) =>
                            setExistedLicenseType(e.target.checked)
                          }
                        />
                      </div>
                    </div>

                    {existedLicenseType && (
                      <div className='flex gap-8'>
                        <div>
                          <div className='flex gap-2 justify-between items-center '>
                            <p className='whitespace-nowrap'>Hạng</p>
                            <TextField
                              id='field'
                              type='text'
                              fullWidth
                              label='Nếu không có nhập 0'
                              variant='outlined'
                              onChange={formik.handleChange(
                                'availableLicenseType'
                              )}
                              onBlur={formik.handleBlur('availableLicenseType')}
                              value={formik.values.availableLicenseType}
                            />
                          </div>
                          <div className='error text-red-900'>
                            {formik.touched.availableLicenseType &&
                              formik.errors.availableLicenseType}
                          </div>
                        </div>
                        <div>
                          <div className='flex gap-2 justify-between items-center '>
                            <p className='whitespace-nowrap'>Cấp ngày</p>
                            <TextField
                              id='field'
                              type='date'
                              fullWidth
                              variant='outlined'
                              onChange={formik.handleChange(
                                'licenseTypeIssuedDate'
                              )}
                              onBlur={formik.handleBlur(
                                'licenseTypeIssuedDate'
                              )}
                              value={formik.values.licenseTypeIssuedDate}
                            />
                          </div>
                          <div className='error text-red-900'>
                            {formik.touched.licenseTypeIssuedDate &&
                              formik.errors.licenseTypeIssuedDate}
                          </div>
                        </div>
                      </div>
                    )}
                  </div>

                  {/* de nghi */}
                  <div>
                    <div className='flex gap-2 justify-between items-center '>
                      <p className='whitespace-nowrap'>
                        Đề nghị cho tôi được học, dự sát hạch để cấp giấy phép
                        lái xe hạng
                      </p>

                      <FormControl variant='outlined' className='flex-1'>
                        <Select
                          value={formik.values.licenseTypeId}
                          onChange={(e) =>
                            formik.setFieldValue(
                              'licenseTypeId',
                              e.target.value
                            )
                          }
                        >
                          {licenseForm?.map((item, index) => (
                            <MenuItem key={index} value={item?.licenseTypeId}>
                              {item?.licenseTypeDesc}
                            </MenuItem>
                          ))}
                        </Select>
                      </FormControl>
                    </div>
                  </div>

                  {/* dinh kem */}
                  <div className='flex flex-col gap-4'>
                    <div>Xin gửi kèm theo:</div>
                    <div>- 01 giấy chứng nhận đủ sức khỏe</div>
                    <div>- 02 ảnh màu cỡ 3cmx4cm, chụp không quá 06 tháng</div>
                    <div>
                      - Bản sao Giấy chứng minh nhân dân hoặc thẻ căn cước công
                      dân hoặc hộ chiếu còn thời hạn có ghi số giấy chứng minh
                      nhân dân hoặc thẻ căn cước công dân (đối với người Việt
                      Nam) hoặc hộ chiếu (đối với người nước ngoài)
                    </div>
                  </div>

                  {/* tai lieu khac */}
                  <div>
                    <div>
                      <div className='flex gap-2 justify-between items-center '>
                        <p className='whitespace-nowrap'>
                          - Các tài liệu khác có liên quan gồm:{' '}
                          <span>Ảnh 3x4, ảnh CCCD, ảnh Giấy khám sức khỏe</span>
                        </p>
                      </div>
                    </div>

                    <div className='font-semibold mb-4'>
                      Tôi xin cam đoan những điều ghi trên đây là đúng sự thật
                      nếu sai tôi xin hoàn toàn chịu trách nhiệm.
                    </div>
                  </div>
                </div>
              </div>

              <div className='flex gap-8 justify-evenly h-[26vh] overflow-hidden'>
                <div className='flex-1 flex justify-center items-center border-4'>
                  {previewIdentityImage ? (
                    <img
                      src={previewIdentityImage.preview}
                      alt='img'
                      className='overflow-hidden object-cover w-full h-full object-cover'
                    />
                  ) : (
                    <label
                      for='file-upload2'
                      class='flex justify-center items-center px-8 custom-file-upload h-full cursor-pointer py-8 px-2'
                    >
                      Ảnh chứng minh nhân dân
                    </label>
                  )}
                </div>

                <div className='flex-1 flex justify-center items-center border-4'>
                  {previewHealthImage ? (
                    <img
                      src={previewHealthImage.preview}
                      alt='img'
                      className='overflow-hidden object-cover w-full h-full object-cover'
                    />
                  ) : (
                    <label
                      for='file-upload3'
                      class='flex justify-center items-center px-8 custom-file-upload h-full cursor-pointer py-8 px-2'
                    >
                      Ảnh giấy khám sức khỏe
                    </label>
                  )}
                </div>
              </div>

              <div>
                <input
                  id='file-upload2'
                  type='file'
                  className='hidden'
                  onChange={(e) => {
                    handlePreviewIdentityImage(e);
                    formik.setFieldValue(
                      'identityCardImage',
                      e.target.files[0]
                    );
                  }}
                />
                <input
                  id='file-upload3'
                  type='file'
                  className='hidden'
                  onChange={(e) => {
                    handlePreviewHealthImage(e);
                    formik.setFieldValue(
                      'healthCertificationImage',
                      e.target.files[0]
                    );
                  }}
                />
              </div>

              <div className='flex mt-8'>
                <div className=' flex flex-col w-[40%] ml-auto items-center'>
                  <div>
                    Hồ Chí Minh, ngày{' '}
                    <span>{dayjs(new Date()).format('DD/MM/YYYY')}</span>
                  </div>
                  <div className='uppercase font-semibold'>Người làm đơn</div>
                  <div>
                    {'('}Ký và ghi rõ họ, tên{')'}
                  </div>
                  <p className='font-bold text-xl'>{`${accInfo.lastName}`}</p>
                  <p className='font-bold text-2xl'>{`${accInfo.firstName} ${accInfo.lastName}`}</p>
                </div>
              </div>
            </div>
          </div>
        </DialogContent>
        <DialogActions>
          <Button className='btn' onClick={handleClearImgages}>
            Hủy ảnh
          </Button>
          <Button onClick={formik.handleSubmit} autoFocus>
            Xác nhận
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default DialogRegisterForm;
