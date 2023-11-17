import {
  Checkbox,
  FormControl,
  MenuItem,
  Select,
  TextField,
} from '@mui/material';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import * as dayjs from 'dayjs';

export default function DialogCreatedRegisterForm({ open, setOpen, accInfo }) {
  console.log('accInfo: ', accInfo);
  return (
    <Dialog
      open={open}
      onClose={() => setOpen(false)}
      aria-labelledby='alert-dialog-title'
      aria-describedby='alert-dialog-description'
      fullWidth
      maxWidth='md'
    >
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
                    <img
                      src={accInfo?.licenseForm?.image}
                      alt='img'
                      className='w-[150px] h-[200px] overflow-hidden object-cover'
                    />
                  </div>
                </div>
                <input id='file-upload' type='file' className='hidden' />

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
                    <div className='flex gap-2  items-center '>
                      <p className='whitespace-nowrap'>Tôi là</p>
                      <span className='font-semibold'>{`${accInfo?.firstName} ${accInfo?.lastName}`}</span>
                    </div>
                  </div>
                </div>
                {/* ngay sinh , gioi tinh */}
                <div className='flex gap-4 items-center'>
                  <div className='flex-1'>
                    <div className='flex gap-2 items-center '>
                      <p className='whitespace-nowrap'>Sinh ngày</p>
                      <span className='font-semibold'>
                        {dayjs(accInfo?.dateBirth).format('YYYY-MM-DD')}
                      </span>
                    </div>
                  </div>
                  <div className='w-[30%]'>
                    <div className='flex gap-2 items-center '>
                      <p>Chọn giới tính</p>
                      <span className='font-semibold'>
                        {accInfo?.licenseForm?.gender}
                      </span>
                    </div>
                  </div>
                </div>

                {/* Noi dki ho khau thuong tru */}
                <div>
                  <div className='flex gap-2 items-center '>
                    <p className='whitespace-nowrap'>
                      Nơi đăng ký hộ khẩu thường trú
                    </p>
                    <span className='font-semibold'>
                      {accInfo?.licenseForm?.permanentAddress}
                    </span>
                  </div>
                </div>

                {/* cmnd/cccd */}
                <div>
                  <div className='flex gap-2 items-center '>
                    <p className='whitespace-nowrap'>
                      Số chứng minh nhân dân hoặc thẻ căn cước công dân {'('}
                      hoặc hộ chiếu{'): '}
                    </p>
                    <span className='font-semibold'>
                      {accInfo?.licenseForm?.identityNumber}
                    </span>
                  </div>
                </div>

                {/* ngay cap, noi cap */}
                <div className='flex gap-8 items-center '>
                  <div className='w-[36%]'>
                    <div className='flex gap-2 justify-between items-center '>
                      <p className='whitespace-nowrap'>Ngày cấp</p>
                      <span className='font-semibold'>
                        {dayjs(
                          accInfo?.licenseForm?.identityCardIssuedDate
                        ).format('DD/MM/YYYY')}
                      </span>
                    </div>
                  </div>

                  <div className='flex-1'>
                    <div className='flex gap-2  items-center '>
                      <p className='whitespace-nowrap'>Nơi cấp</p>
                      <span className='font-semibold'>
                        {accInfo?.licenseForm?.identityCardIssuedBy}
                      </span>
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
                      {/* <Checkbox
                        checked={existedLicenseType}
                      /> */}
                    </div>
                  </div>

                  <div className='flex gap-8 items-center'>
                    <div>
                      <div className='flex gap-2 justify-between items-center '>
                        <p className='whitespace-nowrap'>Hạng</p>
                        <span className='font-semibold'>
                          {accInfo?.licenseForm?.availableLicenseType}
                        </span>
                      </div>
                    </div>
                    <div>
                      <div className='flex gap-2 justify-between items-center '>
                        <p className='whitespace-nowrap'>Cấp ngày</p>
                        <span className='font-semibold'>
                          {dayjs(
                            accInfo?.licenseForm?.licenseTypeIssuedDate
                          ).format('DD/MM/YYYY')}
                        </span>
                      </div>
                    </div>
                  </div>
                </div>

                {/* de nghi */}
                <div>
                  <div className='flex gap-2 justify-between items-center '>
                    <p className='whitespace-nowrap'>
                      Đề nghị cho tôi được học, dự sát hạch để cấp giấy phép lái
                      xe hạng
                    </p>

                    {/* <FormControl variant='outlined' className='flex-1'>
                      <Select value={accInfo?.licenseForm?.licenseTypeId}>
                        {licenseForm?.map((item, index) => (
                          <MenuItem key={index} value={item?.licenseTypeId}>
                            {item?.licenseTypeDesc}
                          </MenuItem>
                        ))}
                      </Select>
                    </FormControl> */}
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
                    nhân dân hoặc thẻ căn cước công dân (đối với người Việt Nam)
                    hoặc hộ chiếu (đối với người nước ngoài)
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
                    Tôi xin cam đoan những điều ghi trên đây là đúng sự thật nếu
                    sai tôi xin hoàn toàn chịu trách nhiệm.
                  </div>
                </div>
              </div>
            </div>

            <div className='flex gap-8 justify-evenly h-[26vh] overflow-hidden'>
              <div className='flex-1 flex justify-center items-center border-4'>
                <img
                  src={accInfo?.licenseForm?.identityCardImage}
                  alt='img'
                  className='overflow-hidden object-cover w-full h-full object-cover'
                />
              </div>

              <div className='flex-1 flex justify-center items-center border-4'>
                <img
                  src={accInfo?.licenseForm?.healthCertificationImage}
                  alt='img'
                  className='overflow-hidden object-cover w-full h-full object-cover'
                />
              </div>
            </div>

            <div className='flex mt-8'>
              <div className=' flex flex-col w-[40%] ml-auto items-center'>
                <div>
                  Hồ Chí Minh, ngày{' '}
                  <span>
                    {dayjs(accInfo?.licenseForm?.createDate).format(
                      'DD/MM/YYYY'
                    )}
                  </span>
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
        <Button onClick={() => setOpen(false)}>Thoát</Button>
      </DialogActions>
    </Dialog>
  );
}
