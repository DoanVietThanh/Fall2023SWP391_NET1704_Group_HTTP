import React, { useState } from 'react';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import BackgroundSlider from '../../components/BackgroundSlider';
import { BsPerson } from 'react-icons/bs';
import theme from '../../theme';
import {
  AiOutlineCalendar,
  AiOutlineClockCircle,
  AiOutlineEye,
} from 'react-icons/ai';
import { BiBookBookmark, BiSearch } from 'react-icons/bi';
import { Link } from 'react-router-dom';

const BlogDetail = () => {
  const url =
    'https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png';
  const breadcrumbs = 'Blog Detail';
  const initListComment = [
    {
      img: 'https://scontent.fsgn2-5.fna.fbcdn.net/v/t1.6435-9/125833027_2756357554611638_425435607356265941_n.jpg?_nc_cat=104&ccb=1-7&_nc_sid=174925&_nc_ohc=EgnGZJgC_VIAX9xjxuY&_nc_ht=scontent.fsgn2-5.fna&oh=00_AfBgkSMHImlU2EIBmF51WT6IKRlRcAk42Q5Z00T4g_1imw&oe=6543959D',
      name: 'Viet Thanh',
      time: '15 tháng 10, 2023 8:30am',
      content:
        'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
      listReply: [
        {
          img: 'https://scontent.fsgn2-9.fna.fbcdn.net/v/t39.30808-6/327973555_1552256688580238_5431785565292483664_n.jpg?stp=cp6_dst-jpg&_nc_cat=105&ccb=1-7&_nc_sid=5f2048&_nc_ohc=VRcWczgUkOoAX_lHpDp&_nc_ht=scontent.fsgn2-9.fna&oh=00_AfCyBkpzQRpsxmLSmJ86P3XWQZY3Em3rM8Fvz9_Udd36Lw&oe=653108D6',
          name: 'Thu Bui',
          time: '15 tháng 10, 2023 11:30am',
          content:
            'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
        },

        {
          img: 'https://scontent.fsgn2-9.fna.fbcdn.net/v/t39.30808-6/327973555_1552256688580238_5431785565292483664_n.jpg?stp=cp6_dst-jpg&_nc_cat=105&ccb=1-7&_nc_sid=5f2048&_nc_ohc=VRcWczgUkOoAX_lHpDp&_nc_ht=scontent.fsgn2-9.fna&oh=00_AfCyBkpzQRpsxmLSmJ86P3XWQZY3Em3rM8Fvz9_Udd36Lw&oe=653108D6',
          name: 'Thu Bui',
          time: '15 tháng 10, 2023 12:30am',
          content:
            'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
        },
      ],
    },

    {
      img: 'https://i.pinimg.com/474x/88/3d/74/883d74b5eca63acc4e07773f3a645ba6.jpg',
      name: 'Xuan Phuoc',
      time: '3 tháng 10, 2023 11:30am',
      content:
        'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
      listReply: [
        {
          img: 'https://scontent.fsgn2-9.fna.fbcdn.net/v/t39.30808-6/327973555_1552256688580238_5431785565292483664_n.jpg?stp=cp6_dst-jpg&_nc_cat=105&ccb=1-7&_nc_sid=5f2048&_nc_ohc=VRcWczgUkOoAX_lHpDp&_nc_ht=scontent.fsgn2-9.fna&oh=00_AfCyBkpzQRpsxmLSmJ86P3XWQZY3Em3rM8Fvz9_Udd36Lw&oe=653108D6',
          name: 'Thu Bui',
          time: '3 tháng 10, 2023 2:30pm',
          content:
            'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
        },

        {
          img: 'https://scontent.fsgn2-9.fna.fbcdn.net/v/t39.30808-6/327973555_1552256688580238_5431785565292483664_n.jpg?stp=cp6_dst-jpg&_nc_cat=105&ccb=1-7&_nc_sid=5f2048&_nc_ohc=VRcWczgUkOoAX_lHpDp&_nc_ht=scontent.fsgn2-9.fna&oh=00_AfCyBkpzQRpsxmLSmJ86P3XWQZY3Em3rM8Fvz9_Udd36Lw&oe=653108D6',
          name: 'Thu Bui',
          time: '3 tháng 10, 2023 5:30pm',
          content:
            'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
        },
      ],
    },

    {
      img: 'https://scontent.fsgn5-6.fna.fbcdn.net/v/t1.6435-9/123116045_772985256617271_6289105269278104549_n.jpg?_nc_cat=108&ccb=1-7&_nc_sid=ad2b24&_nc_ohc=laebF5sIFUUAX8VFESQ&_nc_ht=scontent.fsgn5-6.fna&oh=00_AfBOKBYLyQ6D7SQRwsFXimrfuUuKrPYLyfKst_XNlTvmIw&oe=65353413',
      name: 'Quang Huy',
      time: '20 tháng 9, 2023 5:30pm',
      content:
        'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
      listReply: [
        {
          img: 'https://scontent.fsgn2-9.fna.fbcdn.net/v/t39.30808-6/327973555_1552256688580238_5431785565292483664_n.jpg?stp=cp6_dst-jpg&_nc_cat=105&ccb=1-7&_nc_sid=5f2048&_nc_ohc=VRcWczgUkOoAX_lHpDp&_nc_ht=scontent.fsgn2-9.fna&oh=00_AfCyBkpzQRpsxmLSmJ86P3XWQZY3Em3rM8Fvz9_Udd36Lw&oe=653108D6',
          name: 'Thu Bui',
          time: '20 tháng 9, 2023 6:00pm',
          content:
            'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
        },

        {
          img: 'https://scontent.fsgn2-9.fna.fbcdn.net/v/t39.30808-6/327973555_1552256688580238_5431785565292483664_n.jpg?stp=cp6_dst-jpg&_nc_cat=105&ccb=1-7&_nc_sid=5f2048&_nc_ohc=VRcWczgUkOoAX_lHpDp&_nc_ht=scontent.fsgn2-9.fna&oh=00_AfCyBkpzQRpsxmLSmJ86P3XWQZY3Em3rM8Fvz9_Udd36Lw&oe=653108D6',
          name: 'Thu Bui',
          time: '20 tháng 9, 2023 8:00pm',
          content:
            'Lorem ipsum dolor, sit amet consectetur adipisicing elit. Possimus consequuntur laboriosam perspiciatis! Eligendi ea qui repellat quod reprehenderit dolorum? Voluptas.',
        },
      ],
    },
  ];

  const [listComment, setListComment] = useState(initListComment);
  const [newComment, setNewComment] = useState({
    img: 'https://i.pinimg.com/474x/64/34/69/64346960cc033ce02cdf41479d2d4237.jpg',
    name: 'Nguoi comment moi',
    time: '15 tháng 10, 2023 11:00pm',
    content: '',
    listReply: [],
  });
  // ham xu li khi nut dang bl dc nhan
  const handleNewComment = () => {
    if (newComment.content.trim() !== "") {
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

  const listTitleBlog = [
    {
      id: 1,
      title: "Tips pass bằng lái B1 dễ dàng",
      date: "26 Aug 2023",
      img: "https://i.pinimg.com/564x/d4/97/43/d497438580268c267151a4dedc2cc55d.jpg",
      link: "/blog/detail",
    },
    {
      id: 2,
      title: "Tips pass bằng lái B1 dễ dàng",
      date: "26 Aug 2023",
      img: "https://i.pinimg.com/564x/d4/97/43/d497438580268c267151a4dedc2cc55d.jpg",
      link: "/blog/detail",
    },
    {
      id: 3,
      title: "Tips pass bằng lái B1 dễ dàng",
      date: "26 Aug 2023",
      img: "https://i.pinimg.com/564x/d4/97/43/d497438580268c267151a4dedc2cc55d.jpg",
      link: "/blog/detail",
    },
    {
      id: 4,
      title: "Tips pass bằng lái B1 dễ dàng",
      date: "26 Aug 2023",
      img: "https://i.pinimg.com/474x/f5/1d/f8/f51df8c6a4f7f8657acabcf8ba5275fe.jpg",
      link: "/blog/detail",
    },
  ];
  return (
    <div>
      <Header />
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      <div className='flex justify-center m-20'>
        <div className='flex flex-col w-[70%] border drop-shadow-md rounded-lg p-20 gap-10'>
          <div className='text-5xl font-bold'>
            Tips Pass Bằng Lái B1 Dễ Dàng
          </div>
          <div className='center'>
            <img
              src='https://i.pinimg.com/564x/31/c0/45/31c0457faf9a23763b9c8e67ea6c02e8.jpg'
              alt='pic4'
              className='rounded-lg w-[100%] h-[380px] '
            />
          </div>
          <div className='flex justify-between'>
            <div className={`flex text-lg text-[${theme.color.mainColor}] `}>
              <div className='flex items-center gap-3 pr-4'>
                <BsPerson size={24} /> by Thu Bui
              </div>
              <div className='flex items-center gap-3 border-l-[2px] px-4'>
                <AiOutlineClockCircle size={24} /> August 26, 2023
              </div>
              <div className='flex items-center gap-3 border-l-[2px] px-4'>
                <BiBookBookmark size={24} /> License
              </div>
            </div>
            <div className='flex text-lg gap-2 items-center text-neutral-500 font-semibold '>
              <AiOutlineEye size={24} /> View: 99
            </div>
          </div>

          <div className='font-light text-lg'>
            Lorem ipsum dolor sit amet consectetur adipisicing elit. A pariatur
            repudiandae molestiae! Ad consequatur repellendus dolor fugit,
            delectus nulla necessitatibus possimus facilis quas explicabo
            recusandae velit molestiae quae soluta a voluptatum. Veniam hic
            debitis numquam earum libero tenetur impedit ratione fugit
            aspernatur veritatis similique corporis dolore cumque ducimus
            aperiam, culpa illo repellendus voluptatibus. Voluptatibus quaerat
            omnis accusamus natus molestiae. Ab molestias nihil molestiae libero
            quibusdam, voluptate, a error itaque, aliquam praesentium aut
            magnam. Quasi repellendus laboriosam beatae a ab. Sunt facilis
            animi, earum nulla ipsa, esse optio aliquid suscipit porro natus
            magni incidunt in quaerat tempora ratione. Magnam, totam expedita?
          </div>

          <div className='border-l-8 border-blue-200 pl-10 py-8 font-semiblod text-2xl text-gray-500'>
            <div>
              Lorem ipsum dolor sit amet consectetur, adipisicing elit.
              Reiciendis ducimus quis deleniti repellendus non itaque, eligendi
              optio repellat quidem iusto laboriosam ipsa voluptatem.
              Laudantium, at?
            </div>
            <div className='mt-5 ml-10 text-xl'>Thu Thanh Bui</div>
          </div>

          <div className='font-light text-lg'>
            Lorem ipsum dolor, sit amet consectetur adipisicing elit. Officia
            quisquam unde expedita, eum neque excepturi, qui possimus nostrum,
            incidunt ipsum mollitia odit asperiores facilis! Autem cupiditate
            ducimus dolorem corrupti accusamus?
            <ul className='list-disc font-light text-lg mt-5 ml-5'>
              <li>
                Lorem ipsum dolor sit amet consectetur adipisicing elit.
                Architecto quasi.
              </li>
              <li>
                Lorem ipsum dolor sit amet consectetur adipisicing elit.
                Suscipit.
              </li>
              <li>
                Lorem ipsum dolor sit amet consectetur adipisicing elit.
                Quaerat!
              </li>
            </ul>
          </div>

          <div className='flex justify-center gap-10 mt-5 pb-16 border-b-8 border-gray-200'>
            <img
              src='https://i.pinimg.com/474x/a8/5c/c3/a85cc306342a0cb1550ded1cb4938033.jpg'
              alt='bdpic1'
              className='rounded-md w-[50%] h-[380px] '
            />
            <div className='font-light text-lg'>
              Lorem ipsum, dolor sit amet consectetur adipisicing elit.
              Architecto ullam facilis officiis fugit maiores excepturi
              voluptatem repellendus id obcaecati saepe explicabo, consequatur
              officia temporibus blanditiis porro quod tempore labore deleniti
              quae animi consequuntur sequi expedita voluptates. Sit odit cum
              quos aliquid nostrum laudantium omnis sapiente doloribus
              repudiandae voluptatibus quisquam quo amet mollitia quibusdam
              pariatur veritatis quis nulla, natus maxime aperiam?
            </div>
          </div>

          <div className='flex flex-col gap-5'>
            <div className='text-4xl font-semibold'>Bình luận</div>
            <form>
              <input
                placeholder='Bình luận'
                className='border-2 pl-2 outline-none w-[100%] h-10'
                type='text'
                value={newComment.content}
                onChange={(e) =>
                  setNewComment({ ...newComment, content: e.target.value })
                }
                onChange={(e) =>
                  setNewComment({ ...newComment, content: e.target.value })
                }
              />
              <div className='flex gap-10 mt-5'>
                <input
                  placeholder='Họ tên'
                  className='border-2 pl-2 outline-none w-[50%] h-10'
                />
                <input
                  placeholder='Email'
                  className='border-2 pl-2 outline-none w-[50%] h-10'
                />
              </div>
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
                        src={comment.img}
                        alt='cmpic1'
                        className='rounded-full w-[70px] h-[70px]'
                      />
                    </div>
                  </div>
                  <div className='flex flex-col'>
                    <div className='text-xl font-semibold text-gray-700'>
                      {comment.name}
                    </div>
                    <div className='text-lg font-light text-gray-500'>
                      {comment.time}
                    </div>
                  </div>
                </div>
                <div>
                  <button className='center px-3 py-1 rounded-md bg-blue-400 text-white text-md font-semibold hover:bg-blue-700'>
                    Phản hồi
                  </button>
                </div>
              </div>

              <div className='ml-[120px] text-lg font-light '>
                {comment.content}
              </div>

              <div className='ml-[120px]'>
                {comment.listReply.map((reply, index) => (
                  <div>
                    <div className='flex items-center gap-5 mt-10'>
                      <div className='center bg-blue-100 rounded-full w-[100px] h-[100px] '>
                        <div className='center bg-blue-200 rounded-full w-[85px] h-[85px]  '>
                          <img
                            src={reply.img}
                            alt='rppic1'
                            className='rounded-full w-[70px] h-[70px]'
                          />
                        </div>
                      </div>
                      <div className='flex flex-col'>
                        <div className='text-xl font-semibold text-gray-700'>
                          {reply.name}
                        </div>
                        <div className='text-lg font-light text-gray-500'>
                          {reply.time}
                        </div>
                      </div>
                    </div>
                    <div className='ml-[120px] text-lg font-light '>
                      {reply.content}
                    </div>
                  </div>
                ))}
              </div>
            </div>
          ))}
          {/* comment */}

          {/* comment1 */}

          {/* comment2 
          <div className="border-b-2 border-gray-200 pb-10">
            <div className="flex items-center justify-between mt-10">
              <div className="flex items-center gap-5 ">
                <div className="center bg-blue-100 rounded-full w-[100px] h-[100px] ">
                  <div className="center bg-blue-200 rounded-full w-[85px] h-[85px]  ">
                    <img
                      src="https://i.pinimg.com/474x/88/3d/74/883d74b5eca63acc4e07773f3a645ba6.jpg"
                      alt="cmpic2"
                      className="rounded-full w-[70px] h-[70px]"
                    />
                  </div>
                </div>
                <div className="flex flex-col">
                  <div className="text-xl font-semibold text-gray-700">
                    Xuan Phuoc
                  </div>
                  <div className="text-lg font-light text-gray-500">
                    1 tháng 10, 2023 7:30pm
                  </div>
                </div>
              </div>
              <div>
                <button className="center px-3 py-1 rounded-md bg-blue-400 text-white text-md font-semibold hover:bg-blue-700">
                  Phản hồi
                </button>
              </div>
            </div>

            <div className="ml-[120px] text-lg font-light ">
              Lorem ipsum dolor sit, amet consectetur adipisicing elit. Dolorem
              omnis veniam eveniet asperiores culpa, sit eligendi aliquam iste
              amet consequatur autem id natus explicabo aperiam quis expedita
              reprehenderit vero libero beatae, optio in, voluptas consequuntur?
              Debitis minima eum quaerat eaque.
            </div>

            <div className="ml-[120px]">
              <div>
                <div className="flex items-center gap-5 mt-10">
                  <div className="center bg-blue-100 rounded-full w-[100px] h-[100px] ">
                    <div className="center bg-blue-200 rounded-full w-[85px] h-[85px]  ">
                      <img
                        src="https://scontent.fsgn2-9.fna.fbcdn.net/v/t39.30808-6/327973555_1552256688580238_5431785565292483664_n.jpg?stp=cp6_dst-jpg&_nc_cat=105&ccb=1-7&_nc_sid=a2f6c7&_nc_ohc=yoKzLXaVmv8AX82S6cH&_nc_ht=scontent.fsgn2-9.fna&oh=00_AfD6obC62S-8DWmiSN1aUqfNO1jf5yswIVU0WVPsdpqJUA&oe=652136D6"
                        alt="rppic1"
                        className="rounded-full w-[70px] h-[70px]"
                      />
                    </div>
                  </div>
                  <div className="flex flex-col">
                    <div className="text-xl font-semibold text-gray-700">
                      Thu Bui
                    </div>
                    <div className="text-lg font-light text-gray-500">
                      1 tháng 10, 2023 7:39pm
                    </div>
                  </div>
                </div>
                <div className="ml-[120px] text-lg font-light ">
                  Thắc mắc ít thôi
                </div>
              </div>
            </div>
          </div>
{/* comment3 *
          <div className="border-b-2 border-gray-200 pb-10">
            <div className="flex items-center justify-between mt-10">
              <div className="flex items-center gap-5 ">
                <div className="center bg-blue-100 rounded-full w-[100px] h-[100px] ">
                  <div className="center bg-blue-200 rounded-full w-[85px] h-[85px]  ">
                    <img
                      src="https://scontent.fsgn5-6.fna.fbcdn.net/v/t1.6435-9/123116045_772985256617271_6289105269278104549_n.jpg?_nc_cat=108&ccb=1-7&_nc_sid=ad2b24&_nc_ohc=laebF5sIFUUAX8VFESQ&_nc_ht=scontent.fsgn5-6.fna&oh=00_AfBOKBYLyQ6D7SQRwsFXimrfuUuKrPYLyfKst_XNlTvmIw&oe=65353413"
                      alt="cmpic3"
                      className="rounded-full w-[70px] h-[70px]"
                    />
                  </div>
                </div>
                <div className="flex flex-col">
                  <div className="text-xl font-semibold text-gray-700">
                    Quang Huy
                  </div>
                  <div className="text-lg font-light text-gray-500">
                    15 tháng 9, 2023 5:30pm
                  </div>
                </div>
              </div>
              <div>
                <button className="center px-3 py-1 rounded-md bg-blue-400 text-white text-md font-semibold hover:bg-blue-700">
                  Phản hồi
                </button>
              </div>
            </div>

            <div className="ml-[120px] text-lg font-light ">
              Lorem ipsum dolor sit amet consectetur adipisicing elit.
              Voluptatum officiis illo sunt dolore cum explicabo nobis et
              reprehenderit ratione. Debitis consequuntur facere dicta.
              Repellat, quo?
            </div>

            <div className="ml-[120px]">tạo phản hồi mới</div> 
          </div> */}
        </div>

        <div className='w-[30%] ml-10'>
          <div className='border drop-shadow-md rounded-lg p-10 mb-20'>
            <div className='border-b-[4px] text-2xl font-bold pb-2 '>
              Tìm kiếm
            </div>
            <form className='flex mt-5'>
              <input
                placeholder=' Nhập...'
                className='rounded-l-lg bg-slate-100 outline-none pl-4'
              />
              <button
                className={`center rounded-r-lg bg-[${theme.color.mainColor}] text-white font-bold w-[25%] p-3 hover:bg-blue-900`}
              >
                <BiSearch size={24} />
              </button>
            </form>
          </div>

          <div className='border drop-shadow-md rounded-lg p-10 mb-20'>
            <div className='border-b-[4px] text-2xl font-bold pb-2 '>
              Bài đăng gần đây
            </div>
            <div className="flex flex-col gap-10 mt-10">
              {listTitleBlog.map((item, index) => (
                <div className="flex gap-3">
                  <div>
                    <img
                      src={item.img}
                      alt="Pic1"
                      className="rounded-lg w-[80px] h-[100px] object-cover"
                    />
                  </div>
                  <div className="flex flex-col gap-2">
                    <Link to={item.link}>
                      <div className="text-lg font-bold hover:text-blue-500">
                        {item.title}
                      </div>
                    </Link>
                    <div className="flex items-center text-md font-light gap-2 text-blue-500 hover:text-blue-900 ">
                      <AiOutlineCalendar />
                      {item.date}
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>

      <Footer />
    </div>
  );
};

export default BlogDetail;
