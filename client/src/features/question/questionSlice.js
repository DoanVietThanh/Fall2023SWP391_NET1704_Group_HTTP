import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { toastError } from '../../components/Toastify';
import questionService from './questionService';

const initialState = {
  listQuestions: [],
  listLisenceType: [],
  isError: false,
  isLoading: false,
  isSuccess: false,
  message: '',
  createdFormData: {},
};

export const getQuestions = createAsyncThunk(
  'question/getQuestions',
  async (_, thunkAPI) => {
    try {
      return await questionService.getQuestions();
    } catch (error) {
      // toastError('Không lấy được danh sách câu hỏi');
      return thunkAPI.rejectWithValue(error);
    }
  }
);

export const getLisenceType = createAsyncThunk(
  'question/getLisenceType',
  async (_, thunkAPI) => {
    try {
      return await questionService.getLisenceType();
    } catch (error) {
      toastError('Không thể lấy danh sách loại bằng lái');
      return thunkAPI.rejectWithValue(error);
    }
  }
);

export const createQuestion = createAsyncThunk(
  'question/createQuestion',
  async (dataForm, thunkAPI) => {
    try {
      console.log('dataForm: ', dataForm);
      return await questionService.createQuestion(dataForm);
    } catch (error) {
      return thunkAPI.rejectWithValue(error);
    }
  }
);

export const questionSlice = createSlice({
  name: 'question',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(getQuestions.pending, (state, action) => {
        state.isLoading = true;
      })
      .addCase(getQuestions.rejected, (state, action) => {
        state.isLoading = false;
        state.isError = true;
        state.isSuccess = false;
        state.message = 'fail';
      })
      .addCase(getQuestions.fulfilled, (state, action) => {
        state.isLoading = false;
        state.isError = false;
        state.isSuccess = true;
        state.message = 'success';
        state.listQuestions = [...action?.payload];
      })
      .addCase(createQuestion.pending, (state, action) => {
        state.isLoading = true;
      })
      .addCase(createQuestion.rejected, (state, action) => {
        state.isLoading = false;
        state.isError = true;
        state.isSuccess = false;
        state.message = 'fail';
      })
      .addCase(createQuestion.fulfilled, (state, action) => {
        state.isLoading = false;
        state.isError = false;
        state.isSuccess = true;
        state.message = 'success';
        state.listQuestions.push(action.payload);
      })
      .addCase(getLisenceType.pending, (state, action) => {
        state.isLoading = true;
      })
      .addCase(getLisenceType.rejected, (state, action) => {
        state.isLoading = false;
        state.isError = true;
        state.isSuccess = false;
        state.message = 'fail';
      })
      .addCase(getLisenceType.fulfilled, (state, action) => {
        state.isLoading = false;
        state.isError = false;
        state.isSuccess = true;
        state.message = 'Lấy LisenceType thành công';
        state.listLisenceType = action.payload;
      });
  },
});

const questionReducer = questionSlice.reducer;

export default questionReducer;
