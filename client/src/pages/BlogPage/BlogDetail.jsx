import axios from 'axios';
import dayjs from 'dayjs';
import React, { useEffect, useState } from 'react';
import { AiOutlineClockCircle } from 'react-icons/ai';
import { BsPerson, BsTags } from 'react-icons/bs';
import { useParams } from 'react-router-dom';
import BackgroundSlider from '../../components/BackgroundSlider';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import { toastError } from '../../components/Toastify';
import theme from '../../theme';

const BlogDetail = () => {
  const urlService = process.env.REACT_APP_SERVER_API;
  const { blogId } = useParams();
  const [blog, setBlog] = useState();
  console.log(blogId);

  const url = '/img/backgroundSlide.png';
  const breadcrumbs = 'Bài đăng chi tiết';
  // const initListComment = [
  //   {
  //     img: 'https://scontent.fsgn2-5.fna.fbcdn.net/v/t1.6435-9/125833027_2756357554611638_425435607356265941_n.jpg?_nc_cat=104&ccb=1-7&_nc_sid=174925&_nc_ohc=EgnGZJgC_VIAX9xjxuY&_nc_ht=scontent.fsgn2-5.fna&oh=00_AfBgkSMHImlU2EIBmF51WT6IKRlRcAk42Q5Z00T4g_1imw&oe=6543959D',
  //     name: 'Viet Thanh',
  //     time: '15 tháng 10, 2023 8:30am',
  //     content:
  //       'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
  //     listReply: [
  //       {
  //         img: 'https://scontent.fsgn2-9.fna.fbcdn.net/v/t39.30808-6/327973555_1552256688580238_5431785565292483664_n.jpg?stp=cp6_dst-jpg&_nc_cat=105&ccb=1-7&_nc_sid=5f2048&_nc_ohc=VRcWczgUkOoAX_lHpDp&_nc_ht=scontent.fsgn2-9.fna&oh=00_AfCyBkpzQRpsxmLSmJ86P3XWQZY3Em3rM8Fvz9_Udd36Lw&oe=653108D6',
  //         name: 'Thu Bui',
  //         time: '15 tháng 10, 2023 11:30am',
  //         content:
  //           'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
  //       },

  //       {
  //         img: 'https://scontent.fsgn2-9.fna.fbcdn.net/v/t39.30808-6/327973555_1552256688580238_5431785565292483664_n.jpg?stp=cp6_dst-jpg&_nc_cat=105&ccb=1-7&_nc_sid=5f2048&_nc_ohc=VRcWczgUkOoAX_lHpDp&_nc_ht=scontent.fsgn2-9.fna&oh=00_AfCyBkpzQRpsxmLSmJ86P3XWQZY3Em3rM8Fvz9_Udd36Lw&oe=653108D6',
  //         name: 'Thu Bui',
  //         time: '15 tháng 10, 2023 12:30am',
  //         content:
  //           'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
  //       },
  //     ],
  //   },

  //   {
  //     img: 'https://i.pinimg.com/474x/88/3d/74/883d74b5eca63acc4e07773f3a645ba6.jpg',
  //     name: 'Xuan Phuoc',
  //     time: '3 tháng 10, 2023 11:30am',
  //     content:
  //       'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
  //     listReply: [
  //       {
  //         img: 'https://scontent.fsgn2-9.fna.fbcdn.net/v/t39.30808-6/327973555_1552256688580238_5431785565292483664_n.jpg?stp=cp6_dst-jpg&_nc_cat=105&ccb=1-7&_nc_sid=5f2048&_nc_ohc=VRcWczgUkOoAX_lHpDp&_nc_ht=scontent.fsgn2-9.fna&oh=00_AfCyBkpzQRpsxmLSmJ86P3XWQZY3Em3rM8Fvz9_Udd36Lw&oe=653108D6',
  //         name: 'Thu Bui',
  //         time: '3 tháng 10, 2023 2:30pm',
  //         content:
  //           'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
  //       },

  //       {
  //         img: 'https://scontent.fsgn2-9.fna.fbcdn.net/v/t39.30808-6/327973555_1552256688580238_5431785565292483664_n.jpg?stp=cp6_dst-jpg&_nc_cat=105&ccb=1-7&_nc_sid=5f2048&_nc_ohc=VRcWczgUkOoAX_lHpDp&_nc_ht=scontent.fsgn2-9.fna&oh=00_AfCyBkpzQRpsxmLSmJ86P3XWQZY3Em3rM8Fvz9_Udd36Lw&oe=653108D6',
  //         name: 'Thu Bui',
  //         time: '3 tháng 10, 2023 5:30pm',
  //         content:
  //           'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
  //       },
  //     ],
  //   },

  //   {
  //     img: 'https://scontent.fsgn5-6.fna.fbcdn.net/v/t1.6435-9/123116045_772985256617271_6289105269278104549_n.jpg?_nc_cat=108&ccb=1-7&_nc_sid=ad2b24&_nc_ohc=laebF5sIFUUAX8VFESQ&_nc_ht=scontent.fsgn5-6.fna&oh=00_AfBOKBYLyQ6D7SQRwsFXimrfuUuKrPYLyfKst_XNlTvmIw&oe=65353413',
  //     name: 'Quang Huy',
  //     time: '20 tháng 9, 2023 5:30pm',
  //     content:
  //       'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
  //     listReply: [
  //       {
  //         img: 'https://scontent.fsgn2-9.fna.fbcdn.net/v/t39.30808-6/327973555_1552256688580238_5431785565292483664_n.jpg?stp=cp6_dst-jpg&_nc_cat=105&ccb=1-7&_nc_sid=5f2048&_nc_ohc=VRcWczgUkOoAX_lHpDp&_nc_ht=scontent.fsgn2-9.fna&oh=00_AfCyBkpzQRpsxmLSmJ86P3XWQZY3Em3rM8Fvz9_Udd36Lw&oe=653108D6',
  //         name: 'Thu Bui',
  //         time: '20 tháng 9, 2023 6:00pm',
  //         content:
  //           'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
  //       },

  //       {
  //         img: 'https://scontent.fsgn2-9.fna.fbcdn.net/v/t39.30808-6/327973555_1552256688580238_5431785565292483664_n.jpg?stp=cp6_dst-jpg&_nc_cat=105&ccb=1-7&_nc_sid=5f2048&_nc_ohc=VRcWczgUkOoAX_lHpDp&_nc_ht=scontent.fsgn2-9.fna&oh=00_AfCyBkpzQRpsxmLSmJ86P3XWQZY3Em3rM8Fvz9_Udd36Lw&oe=653108D6',
  //         name: 'Thu Bui',
  //         time: '20 tháng 9, 2023 8:00pm',
  //         content:
  //           'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
  //       },
  //     ],
  //   },
  // ];

  const [listComment, setListComment] = useState([]);
  const [newComment, setNewComment] = useState({
    img: 'https://i.pinimg.com/474x/64/34/69/64346960cc033ce02cdf41479d2d4237.jpg',
    name: 'Nguoi comment moi',
    time: '15 tháng 10, 2023 11:00pm',
    content: '',
    listReply: [],
  });
  // ham xu li khi nut dang bl dc nhan
  const handleNewComment = () => {
    if (newComment.content.trim() !== '') {
      //add comment moi vao list
      setListComment([...listComment, newComment]);
      //set gtri cua comment moi ve gtri mac dinh
      setNewComment({
        img: 'https://i.pinimg.com/474x/64/34/69/64346960cc033ce02cdf41479d2d4237.jpg',
        name: 'Nguoi comment moi',
        time: '15 tháng 10, 2023 11:00pm',
        content: '',
        listReply: [],
      });
    }
  };

  useEffect(() => {
    async function getBlogAndComments() {
      await axios
        .get(`${urlService}/blog/blog_id/${blogId}`)
        .then((res) => {
          console.log('res: ', res);
          setBlog(res.data?.data[0]);
        })
        .catch((error) => {
          console.log('error: ', error);
          toastError(error?.response?.data?.message);
        });
      await axios
        .get(`${urlService}/blog/blog_id/${blogId}`)
        .then((res) => {
          console.log('res: ', res);
          setListComment(res.data?.data[0].comments);
        })
        .catch((error) => {
          console.log('error: ', error);
          toastError(error?.response?.data?.message);
        });
    }
    getBlogAndComments();
  }, []);
  console.log('list tag', blog?.tags);
  return (
    <div>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      <div className='flex justify-center m-20'>
        <div className='flex flex-col border drop-shadow-md rounded-lg p-20 gap-10'>
          <div className='flex justify-center h-[60vh]'>
            <img
              src='https://media.vov.vn/sites/default/files/styles/large/public/2022-10/4_6.jpeg.jpg'
              alt='blogPic'
              className='w-full object-cover'
            />
          </div>
          <div className='mt-8'>
            <div
              className={`flex text-xl pb-8 text-[${theme.color.mainColor}] `}
            >
              <div className='flex items-center gap-3 pr-4'>
                <BsPerson size={24} /> by{' '}
                {`${blog?.staff?.firstName} ${blog?.staff?.lastName}`}
              </div>
              <div className='flex items-center gap-3 border-l-[2px] px-4'>
                <AiOutlineClockCircle size={24} />{' '}
                {dayjs(blog?.createDate).format('DD/MM/YYYY')}
              </div>
              <div className='flex items-center gap-3 border-l-[2px] px-4'>
                <BsTags size={24} />
                Tags :
                {blog?.tags?.map((tag, index) => (
                  <span className='border-l-2' key={tag.tagId}>
                    {tag.tagName}
                  </span>
                ))}
              </div>
              {/* <div className='flex text-lg gap-2 items-center text-neutral-500 font-semibold '>
              <AiOutlineEye size={24} /> View: 99
            </div> */}
            </div>
            <div className='text-4xl font-bold pb-8'>{blog?.title}</div>
            <div
              className='font-light text-lg pb-8'
              dangerouslySetInnerHTML={{ __html: `${blog?.content}` }}
            ></div>
          </div>

          <div className='flex flex-col gap-5'>
            <div className='text-4xl font-semibold'>Bình luận</div>
            <form>
              <textarea
                placeholder='Bình luận'
                className='border-2 p-4 outline-none w-[100%]'
                rows={4}
                value={newComment.content}
                onChange={(e) =>
                  setNewComment({ ...newComment, content: e.target.value })
                }
              />
            </form>

            <button
              className={`w-[100%] h-10 center text-xl font-semibold text-white bg-[${theme.color.mainColor}] p-5 hover:bg-blue-700`}
              onClick={handleNewComment}
            >
              Đăng bình luận
            </button>
          </div>
          {listComment.map((comment, index) => (
            <div className='border-b-2 border-gray-200 pb-10'>
              <div className='flex items-center justify-between mt-10'>
                <div className='flex items-center gap-5 '>
                  <div className='center bg-blue-100 rounded-full w-[100px] h-[100px] '>
                    <div className='center bg-blue-200 rounded-full w-[85px] h-[85px]  '>
                      <img
                        // src={comment.img}
                        src=''
                        alt='cmpic1'
                        className='rounded-full w-[70px] h-[70px]'
                      />
                    </div>
                  </div>
                  <div className='flex flex-col'>
                    <div className='text-xl font-semibold text-gray-700'>
                      {comment.fullName}
                    </div>
                    <div className='text-lg font-light text-gray-500'>
                      {/* {comment.time} */}
                      thoi gian
                    </div>
                  </div>
                </div>
              </div>

              <div className='ml-[120px] text-lg font-light '>
                {comment.content}
              </div>
            </div>
          ))}
        </div>
      </div>

      <Footer />
    </div>
  );
};

export default BlogDetail;
