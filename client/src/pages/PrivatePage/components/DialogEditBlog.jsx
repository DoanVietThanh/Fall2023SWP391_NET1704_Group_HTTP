import { Dialog, TextField } from '@mui/material';
import {
  EditorState,
  convertToRaw,
  ContentState,
  convertFromHTML,
} from 'draft-js';
import { Editor } from 'react-draft-wysiwyg';
import draftToHtml from 'draftjs-to-html';
import React, { useEffect, useRef, useState } from 'react';
import { toastError, toastSuccess } from '../../../components/Toastify';
import axios from 'axios';
import axiosForm from '../../../utils/axiosForm';

const DialogEditBlog = ({ open, setOpen, selectedBlog }) => {
  console.log('selectedBlog: ', selectedBlog);
  const urlService = process.env.REACT_APP_SERVER_API;
  const [tagList, setTagList] = useState([]);
  const [selectedTags, setSelectedTags] = useState([]);
  const [title, setTitle] = useState('');
  const [editorState, setEditorState] = useState(EditorState.createEmpty());
  const fileInputRef = useRef();
  const [imageData, setImageData] = useState(null);
  const [blog, setBlog] = useState();
  const [fileImg, setFileImg] = useState();

  // .get(`/blog/blog_id/${selectedBlog}`)

  useEffect(() => {
    async function getData() {
      await axios
        .get(`${urlService}/blog/tags`)
        .then((res) => {
          setTagList(res.data?.data);
        })
        .catch((error) => {
          toastError(error?.response?.data?.message);
        });

      await axios
        .get(`/blog/blog_id/${selectedBlog}`)
        .then((res) => {
          console.log(res);
          setBlog(res?.data?.data[0]);
          setTitle(res?.data?.data[0].title);
          setSelectedTags(res?.data?.data[0].tags);
          setImageData(res?.data?.data[0].image);

          // console.log(
          //   JSON.stringify(res?.data?.data[0].content).substring(
          //     1,
          //     JSON.stringify(res?.data?.data[0].content).length - 1
          //   )
          // );
          // setEditorState(
          //   EditorState.createWithContent(
          //     convertFromHTML(
          //       JSON.stringify(res?.data?.data[0].content).substring(
          //         1,
          //         JSON.stringify(res?.data?.data[0].content).length - 1
          //       )
          //     )
          //   )
          // );
        })
        .catch((error) => toastError(error?.response?.data?.message));
    }
    getData();
  }, []);

  const editBlog = async () => {
    console.log({
      BlogId: selectedBlog,
      Title: title,
      Content: draftToHtml(convertToRaw(editorState.getCurrentContent())),
      Image: fileImg || null,
      TagIds: selectedTags,
    });
    await axiosForm
      .put(`/blog`, {
        BlogId: selectedBlog,
        Title: title,
        Content: draftToHtml(convertToRaw(editorState.getCurrentContent())),
        Image: fileImg || null,
        TagIds: selectedTags,
      })
      .then((res) => {
        console.log(res);
        setFileImg(null);
        setTitle('');
        setSelectedTags([]);
        setEditorState(EditorState.createEmpty());
        toastSuccess(res?.data?.message);
      })
      .catch((error) => {
        console.log('error: ', error);
        toastError(error?.response?.data?.message);
      });
  };

  const handleTagChange = (tagValue) => {
    if (selectedTags.includes(tagValue)) {
      // Nếu tag đã được chọn, loại bỏ nó
      setSelectedTags(selectedTags.filter((tag) => tag !== tagValue));
    } else {
      // Nếu tag chưa được chọn, thêm nó vào danh sách
      setSelectedTags([...selectedTags, tagValue]);
    }
  };

  const handleTitleChange = (e) => {
    setTitle(e.target.value);
  };

  const onEditorStateChange = (newEditorState) => {
    setEditorState(newEditorState);
  };

  const handleFileChange = () => {
    const file = fileInputRef.current.files[0];
    console.log(
      '🚀 ~ file: CreateBlog.jsx:59 ~ handleFileChange ~ file:',
      file
    );
    if (file) {
      setFileImg(file);
      const reader = new FileReader();
      reader.onloadend = () => {
        const imageData = reader.result;
        setImageData(imageData);
      };
      reader.readAsDataURL(file);
    }
  };

  console.log('tagList: ', tagList);
  console.log('blog: ', blog);
  console.log('selectedTags: ', selectedTags);

  return (
    <div>
      <Dialog open={open} onClose={() => setOpen(false)} maxWidth='md'>
        <div className='p-8 flex flex-col gap-6'>
          <div className='dialogTit'>Chỉnh sửa bài đăng</div>
          <div className='flex flex-col gap-2'>
            <label className='text-xl text-gray-900'>Tiêu đề bài đăng:</label>
            <textarea
              value={title}
              rows='1'
              className='border p-2'
              onChange={handleTitleChange}
            />
          </div>

          <div className='flex flex-col gap-2'>
            <label className='text-xl text-gray-900'>
              Chọn tag cho bài đăng:
            </label>
            <div className='flex flex-wrap gap-6'>
              {selectedTags &&
                tagList.map((tag, index) => (
                  <span key={index}>
                    <div className='flex gap-2 items-center'>
                      <input
                        type='checkbox'
                        value={tag.tagId}
                        checked={selectedTags.includes(tag.tagId)}
                        onChange={() => handleTagChange(tag.tagId)}
                        className='w-[20px] h-[20px]'
                      />
                      {tag.tagName}
                    </div>
                  </span>
                ))}
            </div>
            {/* <div className='flex flex-wrap gap-6'>
              {tagList.map((tag, index) => {
                selectedTags?.map((selectedTag, index) => (
                  <div className='flex gap-2 items-center'>
                    <input
                      type='checkbox'
                      value={tag.tagId}
                      checked={
                        JSON.stringify(tag) == JSON.stringify(selectedTag)
                          ? true
                          : false
                      }
                      onChange={() => handleTagChange(tag.tagId)}
                      className='w-[20px] h-[20px]'
                    />
                    {tag.tagName}
                  </div>
                ));
              })}
            </div> */}
          </div>

          <div className='flex flex-col gap-2'>
            <label className='text-xl text-gray-900'>
              Chọn ảnh cho bài đăng:
            </label>
            <input
              type='file'
              id='fileToUpload'
              accept='.img, .png, .jpg'
              onChange={handleFileChange}
              ref={fileInputRef}
            />
            {imageData && (
              <div>
                <h2>Ảnh đã chọn:</h2>
                <img
                  src={imageData}
                  alt='Selected img'
                  style={{
                    maxWidth: '100%',
                    maxHeight: '300px',
                    margin: '10px 0',
                  }}
                />
              </div>
            )}
          </div>
          <div className='flex flex-col gap-2'>
            <label className='text-xl text-gray-900'>Nội dung bài đăng:</label>
            {editorState && (
              <div className='border p-2'>
                <Editor
                  editorState={editorState}
                  onEditorStateChange={onEditorStateChange}
                  wrapperClassName=''
                  editorClassName=''
                  toolbar={{
                    inlineStyles: true,
                    blockType: {
                      inDropdown: true,
                      options: ['Normal', 'H1', 'H2', 'H3', 'H4', 'H5', 'H6'],
                    },
                    fontSize: {
                      options: [10, 12, 14, 16, 18, 24, 30, 36],
                    },
                    fontFamily: {
                      options: [
                        'Arial',
                        'Georgia',
                        'Impact',
                        'Tahoma',
                        'Times New Roman',
                        'Verdana',
                      ],
                    },
                    inline: { inDropdown: true },
                    list: { inDropdown: true },
                    textAlign: { inDropdown: true },
                    link: { inDropdown: true },
                    history: { inDropdown: true },
                  }}
                />
                {/* <div className='hidden'>
                  {draftToHtml(convertToRaw(editorState.getCurrentContent()))}
                </div> */}
              </div>
            )}
          </div>
          <div className='flex gap-2 justify-end'>
            <button className='btnCancel' onClick={() => setOpen(false)}>
              Hủy
            </button>
            <button className='btn' onClick={() => editBlog()}>
              Hoàn tất
            </button>
          </div>
        </div>
      </Dialog>
    </div>
  );
};

export default DialogEditBlog;

// const array = [
//   {tagId: 1, tagName: 'Bảo dưỡng'},
//   {tagId: 3, tagName: 'Dòng xe mới nhất'},
//   {tagId: 4, tagName: 'Kỹ thuật'}
// ]
