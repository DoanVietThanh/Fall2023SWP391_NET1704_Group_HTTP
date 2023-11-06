import { TextField } from '@mui/material';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';

export default function DialogCurriculum({ open, setOpen, selectedIdCourse }) {
  return (
    <Dialog
      open={open}
      onClose={() => setOpen(false)}
      aria-labelledby='alert-dialog-title'
      aria-describedby='alert-dialog-description'
      fullWidth
      maxWidth='md'
    >
      <DialogTitle id='alert-dialog-title'>
        <h1 className='text-center capitalize font-medium'>
          Khung chương trình
        </h1>
      </DialogTitle>
      <DialogContent>
        <div className='my-4'>
          <TextField id='outlined-basic' label='Outlined' variant='outlined' />
        </div>
      </DialogContent>
      <DialogActions>
        <Button onClick={() => setOpen(false)}>Hủy</Button>
        <Button onClick={() => setOpen(false)} autoFocus>
          Tạo mới
        </Button>
      </DialogActions>
    </Dialog>
  );
}
