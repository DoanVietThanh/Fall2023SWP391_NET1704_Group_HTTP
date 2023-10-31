import { Box } from "@mui/material";
import { DataGrid } from "@mui/x-data-grid";
import React, { useState } from "react";
import { Bar, Line, Pie } from "react-chartjs-2";
import { AiOutlineMoneyCollect, AiOutlineUsergroupAdd } from "react-icons/ai";
import {
  BsArrowRightShort,
  BsFileEarmarkPost,
  BsPeopleFill,
  BsPersonWorkspace,
} from "react-icons/bs";
import { MdOutlinePerson4 } from "react-icons/md";
import SideBar from "../../components/SideBar";
import { Link } from "react-router-dom";
const DashBoard = () => {
  const userSta = [
    {
      id: 1,
      title: "Members",
      quantity: "20",
      icon: <BsPeopleFill />,
    },
    {
      id: 2,
      title: "Mentor",
      quantity: "12",
      icon: <BsPersonWorkspace />,
    },
    {
      id: 3,
      title: "Staff",
      quantity: "58",
      icon: <MdOutlinePerson4 />,
    },
  ];

  const registerSta = [
    {
      id: 1,
      title: "Số lượng đăng kí",
      quantity: "20",
      icon: <AiOutlineUsergroupAdd />,
    },
    {
      id: 2,
      title: "Doanh thu",
      quantity: "12",
      icon: <AiOutlineMoneyCollect />,
    },
  ];

  // Dữ liệu cho biểu đồ (ví dụ: điểm số theo thời gian)
  const dataLine = {
    labels: [
      "Thứ hai",
      "Thứ ba",
      "Thứ tư",
      "Thứ năm",
      "Thứ sáu",
      "Thứ bảy",
      "Chủ nhật",
    ],
    datasets: [
      {
        label: "Tuần nay",
        data: [75, 150, 85, 70, 120, 90, 78],
        borderColor: "#1976D2",
        fill: true,
      },
      {
        label: "Tuần trước",
        data: [150, 69, 79, 120, 98, 78, 130],
        borderColor: "#113152",
        fill: false,
      },
    ],
  };

  const dataPie = {
    labels: ["A1", "A2", "B1", "B2"],
    datasets: [
      {
        label: "Tỉ lệ học lý thuyết theo loại bằng lái",
        data: [40, 20, 70, 100],
        backgroundColor: ["#00BED8", "#8DEE86", "#009DE6", "#00D9B1"],
      },
    ],
  };
  const optionsPie = {
    // Tùy chỉnh hiển thị phần trăm trên mỗi phần (slice) của biểu đồ
    cutout: "40%",
    title: {
      display: true,
      text: "Predicted world population (millions) in 2050",
    },
    plugins: {
      legend: {
        position: "right", // Vị trí hiển thị legent (các nhãn)
      },
      tooltip: {
        callbacks: {
          label: function (context) {
            const label = context.label || "";
            const value = context.parsed || 0;
            const total = context.dataset.data.reduce(
              (acc, currentValue) => acc + currentValue
            );
            const percentage = ((value / total) * 100).toFixed(2) + "%";
            return label + ": " + percentage;
          },
        },
      },
    },
  };
  const dataBar = {
    labels: [
      "Tháng 1",
      "Tháng 2",
      "Tháng 3",
      "Tháng 4",
      "Tháng 5",
      "Tháng 6",
      "Tháng 7",
      "Tháng 8",
      "Tháng 9",
      "Tháng 10",
      "Tháng 11",
      "Tháng 12",
    ],
    datasets: [
      {
        label: "Số lượng đăng kí",
        data: [100, 200, 150, 300, 250, 100, 200, 150, 300, 250, 150, 200],
        type: "bar",
        backgroundColor: "#44B9FF", // Màu nền cột
        barThickness: 50,
      },
      {
        label: "Doanh thu",
        data: [400, 600, 360, 580, 420, 500, 900, 700, 390, 280, 800, 300],
        type: "line",
        borderColor: "#8A85D4",
        backgroundColor: "rgba(138, 133, 212, 0.3)",
        borderDash: [8, 8],
        pointRadius: 5,
        pointBackgroundColor: "#8A85D4",
        fill: true,
        lineTension: 0.3,
      },
    ],
  };

  const optionsBar = {
    scales: {
      y: {
        beginAtZero: true,
      },
    },
  };

  const columns = [
    { field: "id", headerName: "ID", width: 100 },
    {
      field: "img",
      headerName: "Ảnh",
      width: 140,
      renderCell: ({ row: { avatarImage } }) => (
        <div>
          <img
            src="/img/avtThanh.jpg"
            alt="avt"
            className="w-[100px] h-[60px] object-cover"
          />
        </div>
      ),
    },
    { field: "title", headerName: "Tiêu đề", width: 280 },
    { field: "comment", headerName: "Bình luận", width: 100 },
  ];
  const listBlog = [
    {
      id: 1,
      img: 1,
      title:
        "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Minima corporis quas iure veritatis animi odit ratione maxime pariatur, obcaecati quaerat!",
      comment: 10,
    },
    {
      id: 2,
      img: 2,
      title:
        "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Minima corporis quas iure veritatis animi odit ratione maxime pariatur, obcaecati quaerat!",
      comment: 10,
    },
    {
      id: 3,
      img: 3,
      title:
        "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Minima corporis quas iure veritatis animi odit ratione maxime pariatur, obcaecati quaerat!",
      comment: 10,
    },
    {
      id: 4,
      img: 1,
      title:
        "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Minima corporis quas iure veritatis animi odit ratione maxime pariatur, obcaecati quaerat!",
      comment: 10,
    },
    {
      id: 5,
      img: 2,
      title:
        "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Minima corporis quas iure veritatis animi odit ratione maxime pariatur, obcaecati quaerat!",
      comment: 10,
    },
    {
      id: 6,
      img: 3,
      title:
        "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Minima corporis quas iure veritatis animi odit ratione maxime pariatur, obcaecati quaerat!",
      comment: 10,
    },
    {
      id: 7,
      img: 2,
      title:
        "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Minima corporis quas iure veritatis animi odit ratione maxime pariatur, obcaecati quaerat!",
      comment: 10,
    },
    {
      id: 8,
      img: 3,
      title:
        "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Minima corporis quas iure veritatis animi odit ratione maxime pariatur, obcaecati quaerat!",
      comment: 10,
    },
    {
      id: 9,
      img: 2,
      title:
        "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Minima corporis quas iure veritatis animi odit ratione maxime pariatur, obcaecati quaerat!",
      comment: 10,
    },
    {
      id: 10,
      img: 3,
      title:
        "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Minima corporis quas iure veritatis animi odit ratione maxime pariatur, obcaecati quaerat!",
      comment: 10,
    },
  ];
  const listActivities = [
    {
      id: 1,
      date: "10 th10",
      title:
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis libero iure officia necessitatibus nostrum ullam porro cupiditate vero adipisci aspernatur iste rem a, corporis aliquid dicta eos ut nihil doloribus.",
    },
    {
      id: 2,
      date: "8 th10",
      title:
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis libero iure officia necessitatibus nostrum ullam porro cupiditate vero adipisci aspernatur iste rem a, corporis aliquid dicta eos ut nihil doloribus.",
    },
    {
      id: 3,
      date: "3 th10",
      title:
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis libero iure officia necessitatibus nostrum ullam porro cupiditate vero adipisci aspernatur iste rem a, corporis aliquid dicta eos ut nihil doloribus.",
    },
    {
      id: 4,
      date: "10 th10",
      title:
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis libero iure officia necessitatibus nostrum ullam porro cupiditate vero adipisci aspernatur iste rem a, corporis aliquid dicta eos ut nihil doloribus.",
    },
    {
      id: 5,
      date: "8 th10",
      title:
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis libero iure officia necessitatibus nostrum ullam porro cupiditate vero adipisci aspernatur iste rem a, corporis aliquid dicta eos ut nihil doloribus.",
    },
    {
      id: 6,
      date: "3 th10",
      title:
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis libero iure officia necessitatibus nostrum ullam porro cupiditate vero adipisci aspernatur iste rem a, corporis aliquid dicta eos ut nihil doloribus.",
    },
    {
      id: 5,
      date: "8 th10",
      title:
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis libero iure officia necessitatibus nostrum ullam porro cupiditate vero adipisci aspernatur iste rem a, corporis aliquid dicta eos ut nihil doloribus.",
    },
    {
      id: 6,
      date: "3 th10",
      title:
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis libero iure officia necessitatibus nostrum ullam porro cupiditate vero adipisci aspernatur iste rem a, corporis aliquid dicta eos ut nihil doloribus.",
    },
    {
      id: 5,
      date: "8 th10",
      title:
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis libero iure officia necessitatibus nostrum ullam porro cupiditate vero adipisci aspernatur iste rem a, corporis aliquid dicta eos ut nihil doloribus.",
    },
    {
      id: 6,
      date: "3 th10",
      title:
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis libero iure officia necessitatibus nostrum ullam porro cupiditate vero adipisci aspernatur iste rem a, corporis aliquid dicta eos ut nihil doloribus.",
    },
    {
      id: 5,
      date: "8 th10",
      title:
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis libero iure officia necessitatibus nostrum ullam porro cupiditate vero adipisci aspernatur iste rem a, corporis aliquid dicta eos ut nihil doloribus.",
    },
    {
      id: 6,
      date: "3 th10",
      title:
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis libero iure officia necessitatibus nostrum ullam porro cupiditate vero adipisci aspernatur iste rem a, corporis aliquid dicta eos ut nihil doloribus.",
    },
  ];

  const maxWords = 10;

  const [scrollable, setScrollable] = useState(listActivities.length > 5);
  return (
    <div className="flex">
      <SideBar />
      <Box component="main" sx={{ width: "100%" }}>
        <div className="flex flex-col py-24 px-10 gap-16 bg-gray-100 justify-center">
          <div className="flex gap-20">
            {userSta.map((item, index) => (
              <div
                className="w-[100%] flex gap-20 p-5 items-center justify-center bg-white shadow-md rounded-md"
                key={index}
              >
                <div className="text-gray-700 bg-blue-100 text-3xl p-5 rounded-full">
                  {item.icon}
                </div>
                <div className="flex flex-col gap-2">
                  <div className="text-xl uppercase font-semibold">
                    {item.title}
                  </div>
                  <div className={`text-4xl font-semibold text-blue-500`}>
                    {item.quantity}
                  </div>
                </div>
              </div>
            ))}
          </div>

          <div className="flex">
            {/* Line */}
            <div className="p-6 w-[60%] bg-white border-gray-500 rounded-md shadow-md mr-10">
              <h1 className="font-semibold text-lg">
                Số lượt làm bài thi lý thuyết
              </h1>
              <Line data={dataLine} options={optionsBar} />
            </div>
            {/* Pie */}
            <div className="p-6 w-[40%] bg-white border-gray-500 rounded-md shadow-md">
              <h1 className="font-semibold text-lg">
                Tỉ lệ học lý thuyết theo loại bằng lái
              </h1>
              <div className="pl-16">
                <Pie data={dataPie} options={optionsPie} />
              </div>
            </div>
          </div>

          <div className="flex flex-col shadow-md rounded-md bg-white p-5">
            <h1 className="font-semibold text-2xl p-5">
              Tổng số lượng đăng kí và doanh thu
            </h1>
            <div className="flex p-5">
              {registerSta.map((item, index) => (
                <div
                  className="w-full flex gap-20 p-5 items-center justify-center flex-item"
                  key={index}
                >
                  <div className="text-gray-700 bg-blue-100 text-4xl p-5 rounded-full">
                    {item.icon}
                  </div>
                  <div className="flex flex-col gap-2">
                    <div className="text-xl uppercase font-semibold">
                      {item.title}
                    </div>
                    <div className={`text-5xl font-semibold text-blue-500`}>
                      {item.quantity}
                    </div>
                  </div>
                </div>
              ))}
            </div>
            {/* bar */}
            <div className="p-6 bg-white border-gray-500 rounded-md mt-10">
              <h1 className="font-semibold text-lg mb-5">
                Số lượng đăng kí và doanh thu theo tháng
              </h1>
              <Bar data={dataBar} options={optionsBar} />
            </div>
          </div>
          <div className="flex gap-10 ">
            <div className="w-[35%] p-5 bg-white rounded-md shadow-md">
              <div className="text-xl font-semibold mb-8">Hoạt động</div>
              <div className="pl-5 user-list">
                <ul className={scrollable ? "scrollable" : ""}>
                  {listActivities.map((acti, index) => (
                    <li className="flex mb-4">
                      <div className="w-[80px] text-blue-700 text-lg">
                        {" "}
                        <BsFileEarmarkPost />{" "}
                      </div>
                      <div className="w-[150px] whitespace-nowrap text-md font-semibold">
                        {acti.date}
                      </div>
                      <div className="w-[100px] text-blue-500 text-2xl">
                        <BsArrowRightShort />
                      </div>
                      <div className="w-full">
                        {acti.title.split(" ").slice(0, maxWords).join(" ")}
                        {acti.title.split(" ").length > maxWords && "..."}
                        <Link
                          to={"/blog/detail"}
                          className="text-blue-400 text-lg"
                        >
                          View
                        </Link>
                      </div>
                    </li>
                  ))}
                </ul>
              </div>
            </div>

            <div className="w-[65%] p-5 bg-white rounded-md shadow-md ">
              <div className="text-xl font-semibold mb-8">Bài đăng</div>
              <div>
                <DataGrid
                  rows={listBlog}
                  columns={columns}
                  rowHeight={80}
                  initialState={{
                    pagination: {
                      paginationModel: { page: 0, pageSize: 5 },
                    },
                  }}
                  // pageSizeOptions={[5,10]}
                />
              </div>
            </div>
          </div>
        </div>
      </Box>
    </div>
  );
};

export default DashBoard;
