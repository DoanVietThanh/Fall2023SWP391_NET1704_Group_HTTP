import { useSelector } from 'react-redux';
import './App.scss';
import Toast from './components/Toastify';
import RouterApp from './routes/RouterApp';
import Loading from './components/Loading';

function App() {
  const { isLoading } = useSelector((state) => state.auth);
  return (
    <div className='App'>
      <RouterApp />
      <Toast />
      {isLoading && <Loading />}
    </div>
  );
}

export default App;
