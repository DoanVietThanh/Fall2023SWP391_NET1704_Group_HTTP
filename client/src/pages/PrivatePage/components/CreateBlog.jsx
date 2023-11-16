import axios from 'axios';
import { EditorState, convertToRaw } from 'draft-js';
import draftToHtml from 'draftjs-to-html';
import React, { useEffect, useState } from 'react';
import { Editor } from 'react-draft-wysiwyg';
import 'react-draft-wysiwyg/dist/react-draft-wysiwyg.css';
import { toastError, toastSuccess } from '../../../components/Toastify';
import { useRef } from 'react';
import { useSelector } from 'react-redux';
import axiosClient from '../../../utils/axiosClient';
import axiosForm from '../../../utils/axiosForm';

const CreateBlog = () => {
  const urlService = process.env.REACT_APP_SERVER_API;
  const [tagList, setTagList] = useState([]);
  const [fileImg, setFileImg] = useState();
  const { user } = useSelector((state) => state.auth);
  const accInfo = user.accountInfo;
  console.log('user: ', user);
  useEffect(() => {
    async function getTagList() {
      await axios
        .get(`${urlService}/blog/tags`)
        .then((res) => {
          console.log('res: ', res);
          setTagList(res.data?.data);
        })
        .catch((error) => {
          console.log('error: ', error);
          toastError(error?.response?.data?.message);
        });
    }
    getTagList();
  }, []);

  const [selectedTags, setSelectedTags] = useState([]);

  const handleTagChange = (tagId) => {
    if (selectedTags.includes(tagId)) {
      // Nếu tag đã được chọn, loại bỏ nó
      setSelectedTags(selectedTags.filter((tag) => tag !== tagId));
    } else {
      // Nếu tag chưa được chọn, thêm nó vào danh sách
      setSelectedTags([...selectedTags, tagId]);
    }
  };

  const [title, setTitle] = useState('');
  const handleTitleChange = (e) => {
    setTitle(e.target.value);
  };
  const [editorState, setEditorState] = useState(EditorState.createEmpty());

  const onEditorStateChange = (newEditorState) => {
    setEditorState(newEditorState);
  };
  const fileInputRef = useRef();
  const [imageData, setImageData] = useState(null);

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

  console.log({
    StaffId: user?.accountInfo?.staffId,
    Title: title,
    Content: draftToHtml(convertToRaw(editorState.getCurrentContent())),
    Image: fileImg || null,
    TagIds: selectedTags,
  });

  const submitCreateBlog = async () => {
    console.log({
      StaffId: user?.accountInfo?.staffId,
      Title: title,
      Content: draftToHtml(convertToRaw(editorState.getCurrentContent())),
      Image: fileImg || null,
      TagIds: selectedTags,
    });
    await axiosForm
      .post(`/blog`, {
        StaffId: user?.accountInfo?.staffId,
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

  return (
    <div className='flex flex-col gap-6 mx-8 text-lg'>
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
        <label className='text-xl text-gray-900'>Chọn tag cho bài đăng:</label>
        <div className='flex flex-wrap gap-6 '>
          {tagList.map((tag, index) => (
            <span key={index}>
              <div className='flex items-center gap-2'>
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
      </div>

      <div className='flex flex-col gap-2'>
        <label className='text-xl text-gray-900'>Chọn ảnh cho bài đăng:</label>
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
              style={{ maxWidth: '100%', maxHeight: '300px', margin: '10px 0' }}
            />
          </div>
        )}
      </div>
      <div className='flex flex-col gap-2'>
        <label className='text-xl text-gray-900'>Nội dung bài đăng:</label>
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
          <div className='hidden'>
            {draftToHtml(convertToRaw(editorState.getCurrentContent()))}
          </div>
        </div>
      </div>
      <div className='flex justify-end'>
        <button className='btn' onClick={() => submitCreateBlog()}>
          Đăng bài
        </button>
      </div>
    </div>
  );
};

export default CreateBlog;
