namespace MIDI_Monkey
{
    public class DraggablePanelHelper
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private Form form;

        public DraggablePanelHelper(Form form, Panel panel)
        {
            this.form = form;
            panel.MouseDown += new MouseEventHandler(panel_MouseDown);
            panel.MouseMove += new MouseEventHandler(panel_MouseMove);
            panel.MouseUp += new MouseEventHandler(panel_MouseUp);
        }

        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = form.Location;
        }

        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                form.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        private void panel_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
    }
}
