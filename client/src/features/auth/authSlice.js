import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import authService from './authService';
import { toastError } from '../../components/Toastify';

const initialState = {
  user: {},
  isError: false,
  isLoading: false,
  isSuccess: false,
  message: '',
};

export const login = createAsyncThunk(
  'auth/login',
  async (dataForm, thunkAPI) => {
    try {
      return await authService.login(dataForm);
    } catch (error) {
      toastError('Sai tài khoản hoặc mật khẩu');
      return thunkAPI.rejectWithValue(error);
    }
  }
);

export const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(login.pending, (state, action) => {
        state.isLoading = true;
      })
      .addCase(login.rejected, (state, action) => {
        state.isLoading = false;
        state.isError = true;
        state.isSuccess = false;
        state.message = 'fail';
      })
      .addCase(login.fulfilled, (state, action) => {
        state.isLoading = false;
        state.isError = false;
        state.isSuccess = true;
        state.message = 'success';
        state.user = action.payload?.data;
      });
  },
});

const authReducer = authSlice.reducer;

export default authReducer;
