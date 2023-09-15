import './App.scss';
import Toast from './components/Toastify';
import RouterApp from './routes/RouterApp';

function App() {
  return (
    <div className='App'>
      <RouterApp />
      <Toast />
    </div>
  );
}

export default App;
