using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIDI_Monkey.UI
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

        // Per key: TickCount64 until which the key stays lit. Written from any thread.
        private readonly long[] highlightUntil = new long[128];
        // Per key: colour currently shown. Only touched on the UI thread.
        private readonly bool[] isLit = new bool[128];
        private readonly System.Windows.Forms.Timer refreshTimer;

        public VisualBoard(Panel panel)
        {
            visualPanel = panel;
            visualPanel.SizeChanged += VisualPanel_SizeChanged;

            refreshTimer = new System.Windows.Forms.Timer { Interval = 30 };
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Start();

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

        private void VisualPanel_SizeChanged(object? sender, EventArgs e)
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

        /// <summary>
        /// Light a key for <paramref name="durationMs"/>. Safe to call from any thread and never blocks:
        /// it only records the request, and a UI timer applies the colours (at most ~33 times a second).
        /// Previously every note started a background task that waited on the UI thread.
        /// </summary>
        public void HighlightKey(int keyNumber, int durationMs = 200)
        {
            if (keyNumber < 0 || keyNumber >= totalKeys)
                return;

            long until = Environment.TickCount64 + Math.Max(1, durationMs);
            Interlocked.Exchange(ref highlightUntil[keyNumber], until);
        }

        public void UnhighlightKey(int keyNumber)
        {
            if (keyNumber < 0 || keyNumber >= totalKeys)
                return;

            Interlocked.Exchange(ref highlightUntil[keyNumber], 0);
        }

        private void RefreshTimer_Tick(object? sender, EventArgs e)
        {
            long now = Environment.TickCount64;

            for (int i = 0; i < totalKeys; i++)
            {
                bool lit = Interlocked.Read(ref highlightUntil[i]) > now;
                if (lit == isLit[i])
                    continue;

                isLit[i] = lit;
                if (midiKeys.TryGetValue(i, out var keyLabel) && !keyLabel.IsDisposed)
                {
                    keyLabel.BackColor = lit ? Color.Yellow : Color.LightGray;
                }
            }
        }

        public async Task PrettyLights()
        {
            for (int i = 0; i < totalKeys; i++)
            {
                if (midiKeys.ContainsKey(i))
                {
                    HighlightKey(i, 50);
                    await Task.Delay(50);
                }
            }
        }

        public void Dispose()
        {
            refreshTimer.Stop();
            refreshTimer.Tick -= RefreshTimer_Tick;
            refreshTimer.Dispose();

            if (visualPanel != null)
            {
                visualPanel.SizeChanged -= VisualPanel_SizeChanged;
            }

            midiKeys.Clear();
        }
    }
}
