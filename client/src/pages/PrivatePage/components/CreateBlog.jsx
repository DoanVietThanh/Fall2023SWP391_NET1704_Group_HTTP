import { EditorState, convertToRaw } from "draft-js";
import draftToHtml from "draftjs-to-html";
import React, { useState } from "react";
import { Editor } from "react-draft-wysiwyg";
import "react-draft-wysiwyg/dist/react-draft-wysiwyg.css";

const CreateBlog = () => {
  const handleTagChange = (e) => {
    const tagValue = e.target.value;
    if (selectedTags.includes(tagValue)) {
      // Nếu tag đã được chọn, loại bỏ nó
      setSelectedTags(selectedTags.filter((tag) => tag !== tagValue));
    } else {
      // Nếu tag chưa được chọn, thêm nó vào danh sách
      setSelectedTags([...selectedTags, tagValue]);
    }
  };
  const [selectedTags, setSelectedTags] = useState([]);
  const tags = [
    "BangLaiA1",
    "ThucHanh",
    "LyThuyet",
    "GiangVien",
    "WebXin",
    "HocVien",
  ];
  const listTag = [];
  const [title, setTitle] = useState("");
  const handleTitleChange = (e) => {
    setTitle(e.target.value);
  };
  const [editorState, setEditorState] = useState(EditorState.createEmpty());

  const onEditorStateChange = (newEditorState) => {
    setEditorState(newEditorState);
  };

  return (
    <div className="flex flex-col gap-4 m-4 text-lg">
      <div className="flex flex-col gap-2">
        <label className="text-xl text-gray-900">Tiêu đề bài đăng:</label>
        <textarea
          value={title}
          rows="1"
          className="border p-2"
          onChange={handleTitleChange}
        />
      </div>

      <div className="flex flex-col gap-2">
        <label className="text-xl text-gray-900">Chọn tag cho bài đăng:</label>
        <div className="flex flex-wrap gap-4 w-[50%] ">
          {tags.map((tag) => (
            <span key={tag}>
              <div className="items-center">
                <input
                  type="checkbox"
                  value={tag}
                  checked={selectedTags.includes(tag)}
                  onChange={handleTagChange}
                  className="w-[20px] h-[20px]"
                />
                
                {tag}
              </div>
            </span>
          ))}
        </div>
      </div>

      <div className="flex flex-col gap-2">
        <label className="text-xl text-gray-900">Nội dung bài đăng:</label>
        <div className="border p-2">
          <Editor
            editorState={editorState}
            onEditorStateChange={onEditorStateChange}
            wrapperClassName=""
            editorClassName=""
            toolbar={{
              inlineStyles: true,
              blockType: {
                inDropdown: true,
                options: ["Normal", "H1", "H2", "H3", "H4", "H5", "H6"],
              },
              fontSize: {
                options: [10, 12, 14, 16, 18, 24, 30, 36],
              },
              fontFamily: {
                options: [
                  "Arial",
                  "Georgia",
                  "Impact",
                  "Tahoma",
                  "Times New Roman",
                  "Verdana",
                ],
              },
              inline: { inDropdown: true },
              list: { inDropdown: true },
              textAlign: { inDropdown: true },
              link: { inDropdown: true },
              history: { inDropdown: true },
            }}
          />
          <textarea
            disable
            value={draftToHtml(convertToRaw(editorState.getCurrentContent()))}
            className="w-full "
            rows={5}
          />
        </div>
      </div>
    </div>
  );
};

export default CreateBlog;
