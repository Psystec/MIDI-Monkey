namespace MIDI_Monkey
{
    public class VisualBoard
    {
        private Dictionary<int, Label> midiKeys = new Dictionary<int, Label>();
        private Panel visualPanel;
        private bool keysCreated = false;
        private int totalKeys = 128;
        private int keysPerRow = 16;
        private int gap = 5;
        private int padding = 5;

        public VisualBoard(Panel panel)
        {
            visualPanel = panel;
            visualPanel.SizeChanged += VisualPanel_SizeChanged;

            CreateVisualKeys();
        }

        private void CreateVisualKeys()
        {
            if (keysCreated)
                return;

            visualPanel.SuspendLayout();

            midiKeys.Clear();
            visualPanel.Controls.Clear();

            for (int i = 0; i < totalKeys; i++)
            {
                Label keyLabel = new Label
                {
                    BackColor = Color.LightGray,
                    FlatStyle = FlatStyle.Flat,
                    Enabled = false,
                    Tag = i,
                    BorderStyle = BorderStyle.FixedSingle,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Text = i.ToString()
                };

                midiKeys[i] = keyLabel;
                visualPanel.Controls.Add(keyLabel);
            }

            RepositionKeys();

            visualPanel.ResumeLayout(true);
            keysCreated = true;
        }

        private void RepositionKeys()
        {
            visualPanel.SuspendLayout();

            int panelWidth = visualPanel.Width - gap;
            int panelHeight = visualPanel.Height - gap;

            int keyWidth = (panelWidth - (keysPerRow - 1) * gap - padding * 2) / keysPerRow;
            int keyHeight = (panelHeight - ((totalKeys / keysPerRow) - 1) * gap) / (totalKeys / keysPerRow);

            Font keyFont = new Font("Arial", Math.Max(6, Math.Min(keyWidth, keyHeight) / 3), FontStyle.Regular);

            foreach (var kvp in midiKeys)
            {
                int i = kvp.Key;
                Label keyLabel = kvp.Value;

                int row = i / keysPerRow;
                int col = i % keysPerRow;

                int left = padding + col * (keyWidth + gap);
                int top = row * (keyHeight + gap);

                keyLabel.Left = left;
                keyLabel.Top = top;
                keyLabel.Width = keyWidth;
                keyLabel.Height = keyHeight;
                keyLabel.Font = keyFont;
            }

            visualPanel.ResumeLayout(true);
        }

        private void VisualPanel_SizeChanged(object sender, EventArgs e)
        {
            if (keysCreated)
            {
                RepositionKeys();
            }
            else
            {
                CreateVisualKeys();
            }
        }

        public async Task HighlightKey(int keyNumber, int delay = 200)
        {
            if (!midiKeys.ContainsKey(keyNumber))
                return;

            Label keyLabel = midiKeys[keyNumber];

            await UpdateKeyColorSafe(keyLabel, Color.Yellow);
            await Task.Delay(delay);
            await UpdateKeyColorSafe(keyLabel, Color.LightGray);
        }

        private Task UpdateKeyColorSafe(Label keyLabel, Color color)
        {
            if (keyLabel.InvokeRequired)
            {
                return Task.Run(() => {
                    try
                    {
                        keyLabel.Invoke(new Action(() => {
                            if (!keyLabel.IsDisposed)
                                keyLabel.BackColor = color;
                        }));
                    }
                    catch (ObjectDisposedException)
                    {
                        throw new Exception("ERROR: Key label disposed");
                    }
                    catch (InvalidOperationException)
                    {
                        throw new Exception("ERROR: Key label invalid operation");
                    }
                });
            }
            else
            {
                if (!keyLabel.IsDisposed)
                    keyLabel.BackColor = color;
                return Task.CompletedTask;
            }
        }

        public void UnhighlightKey(int keyNumber)
        {
            if (midiKeys.ContainsKey(keyNumber))
            {
                Label keyLabel = midiKeys[keyNumber];

                if (keyLabel.InvokeRequired)
                {
                    keyLabel.BeginInvoke(new Action(() => {
                        if (!keyLabel.IsDisposed)
                            keyLabel.BackColor = Color.LightGray;
                    }));
                }
                else if (!keyLabel.IsDisposed)
                {
                    keyLabel.BackColor = Color.LightGray;
                }
            }
        }

        public async Task PrettyLights()
        {
            for (int i = 0; i < totalKeys; i++)
            {
                if (midiKeys.ContainsKey(i))
                {
                    await HighlightKey(i, 50);
                }
            }
        }

        public void Dispose()
        {
            if (visualPanel != null)
            {
                visualPanel.SizeChanged -= VisualPanel_SizeChanged;
            }

            midiKeys.Clear();
        }
    }
}