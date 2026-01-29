using System.Drawing;
using System.Windows.Forms;

namespace MIDI_Monkey.Helpers
{
    public class ResizableFormHelper
    {
        private bool resizing = false;
        private Point resizeCursorPoint;
        private Size resizeFormSize;
        private Form form;
        private Control resizeHandle;

        public ResizableFormHelper(Form form, Control resizeHandle)
        {
            this.form = form;
            this.resizeHandle = resizeHandle;
            this.resizeHandle.MouseDown += new MouseEventHandler(ResizeHandle_MouseDown);
            this.resizeHandle.MouseMove += new MouseEventHandler(ResizeHandle_MouseMove);
            this.resizeHandle.MouseUp += new MouseEventHandler(ResizeHandle_MouseUp);
        }

        private void ResizeHandle_MouseDown(object sender, MouseEventArgs e)
        {
            resizing = true;
            resizeCursorPoint = Cursor.Position;
            resizeFormSize = form.Size;
        }

        private void ResizeHandle_MouseMove(object sender, MouseEventArgs e)
        {
            if (resizing)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(resizeCursorPoint));
                form.Size = new Size(resizeFormSize.Width + diff.X, resizeFormSize.Height + diff.Y);
            }
        }

        private void ResizeHandle_MouseUp(object sender, MouseEventArgs e)
        {
            resizing = false;
        }
    }
}
