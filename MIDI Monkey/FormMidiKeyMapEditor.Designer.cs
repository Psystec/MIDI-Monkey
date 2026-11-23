using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MIDI_Monkey
{
    partial class FormMidiKeyMapEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            dataGridViewKeyMappings = new DataGridView();
            panelButtons = new Panel();
            buttonRefresh = new Button();
            buttonLoad = new Button();
            buttonNew = new Button();
            buttonSave = new Button();
            panelTop = new Panel();
            label1 = new Label();
            textBoxApplication = new TextBox();
            ((ISupportInitialize)dataGridViewKeyMappings).BeginInit();
            panelButtons.SuspendLayout();
            panelTop.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewKeyMappings
            // 
            dataGridViewKeyMappings.AllowUserToAddRows = false;
            dataGridViewKeyMappings.AllowUserToDeleteRows = false;
            dataGridViewKeyMappings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewKeyMappings.BackgroundColor = SystemColors.Control;
            dataGridViewKeyMappings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewKeyMappings.Location = new Point(12, 51);
            dataGridViewKeyMappings.Name = "dataGridViewKeyMappings";
            dataGridViewKeyMappings.RowHeadersWidth = 40;
            dataGridViewKeyMappings.Size = new Size(560, 449);
            dataGridViewKeyMappings.TabIndex = 1;
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(buttonRefresh);
            panelButtons.Controls.Add(buttonLoad);
            panelButtons.Controls.Add(buttonNew);
            panelButtons.Controls.Add(buttonSave);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Location = new Point(0, 512);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(584, 50);
            panelButtons.TabIndex = 2;
            // 
            // buttonRefresh
            // 
            buttonRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonRefresh.Location = new Point(283, 11);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(85, 28);
            buttonRefresh.TabIndex = 3;
            buttonRefresh.Text = "Refresh";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += buttonRefresh_Click;
            // 
            // buttonLoad
            // 
            buttonLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonLoad.Location = new Point(386, 11);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(85, 28);
            buttonLoad.TabIndex = 2;
            buttonLoad.Text = "Load";
            buttonLoad.UseVisualStyleBackColor = true;
            buttonLoad.Click += buttonLoad_Click;
            // 
            // buttonNew
            // 
            buttonNew.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonNew.Location = new Point(12, 11);
            buttonNew.Name = "buttonNew";
            buttonNew.Size = new Size(85, 28);
            buttonNew.TabIndex = 0;
            buttonNew.Text = "New";
            buttonNew.UseVisualStyleBackColor = true;
            buttonNew.Click += buttonNew_Click;
            // 
            // buttonSave
            // 
            buttonSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonSave.Location = new Point(486, 11);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(85, 28);
            buttonSave.TabIndex = 1;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // panelTop
            // 
            panelTop.Controls.Add(label1);
            panelTop.Controls.Add(textBoxApplication);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(584, 45);
            panelTop.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(171, 15);
            label1.TabIndex = 0;
            label1.Text = "Application Name (Process ID):";
            // 
            // textBoxApplication
            // 
            textBoxApplication.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxApplication.Location = new Point(194, 12);
            textBoxApplication.Name = "textBoxApplication";
            textBoxApplication.Size = new Size(377, 23);
            textBoxApplication.TabIndex = 0;
            textBoxApplication.TextChanged += textBoxApplication_TextChanged;
            // 
            // FormMidiKeyMapEditor
            // 
            ClientSize = new Size(584, 562);
            Controls.Add(panelTop);
            Controls.Add(panelButtons);
            Controls.Add(dataGridViewKeyMappings);
            MinimumSize = new Size(600, 600);
            Name = "FormMidiKeyMapEditor";
            StartPosition = FormStartPosition.CenterParent;
            Text = "MIDI KeyMap Editor - New KeyMap";
            FormClosing += FormMidiKeyMapEditor_FormClosing;
            Load += FormMidiKeyMapEditor_Load;
            ((ISupportInitialize)dataGridViewKeyMappings).EndInit();
            panelButtons.ResumeLayout(false);
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dataGridViewKeyMappings;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxApplication;
        private System.Windows.Forms.Button buttonRefresh;

        #endregion
    }
}
