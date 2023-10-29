namespace PolygonEditor
{
    partial class PolygonEditor
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            canvas = new PictureBox();
            mainGroupBox = new GroupBox();
            offsetLabel = new Label();
            offsetTrackBar = new TrackBar();
            drawOffsetCheckbox = new CheckBox();
            offsetSettingsLabel = new Label();
            bresenhamCheckbox = new CheckBox();
            verticalRadioButton = new RadioButton();
            horizontalRadioButton = new RadioButton();
            noneRadioButton = new RadioButton();
            edgeConstraintLabel = new Label();
            LineLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            mainGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)offsetTrackBar).BeginInit();
            SuspendLayout();
            // 
            // canvas
            // 
            canvas.BackColor = SystemColors.ButtonHighlight;
            canvas.BorderStyle = BorderStyle.FixedSingle;
            canvas.Dock = DockStyle.Left;
            canvas.Location = new Point(0, 0);
            canvas.Name = "canvas";
            canvas.Size = new Size(697, 593);
            canvas.TabIndex = 0;
            canvas.TabStop = false;
            canvas.Paint += canvas_Paint;
            canvas.MouseClick += canvas_MouseClick;
            canvas.MouseDoubleClick += canvas_MouseDoubleClick;
            canvas.MouseDown += canvas_MouseDown;
            canvas.MouseMove += canvas_MouseMove;
            canvas.MouseUp += canvas_MouseUp;
            // 
            // mainGroupBox
            // 
            mainGroupBox.Controls.Add(offsetLabel);
            mainGroupBox.Controls.Add(offsetTrackBar);
            mainGroupBox.Controls.Add(drawOffsetCheckbox);
            mainGroupBox.Controls.Add(offsetSettingsLabel);
            mainGroupBox.Controls.Add(bresenhamCheckbox);
            mainGroupBox.Controls.Add(verticalRadioButton);
            mainGroupBox.Controls.Add(horizontalRadioButton);
            mainGroupBox.Controls.Add(noneRadioButton);
            mainGroupBox.Controls.Add(edgeConstraintLabel);
            mainGroupBox.Controls.Add(LineLabel);
            mainGroupBox.Location = new Point(703, 0);
            mainGroupBox.Name = "mainGroupBox";
            mainGroupBox.RightToLeft = RightToLeft.No;
            mainGroupBox.Size = new Size(235, 593);
            mainGroupBox.TabIndex = 1;
            mainGroupBox.TabStop = false;
            // 
            // offsetLabel
            // 
            offsetLabel.AutoSize = true;
            offsetLabel.Location = new Point(6, 174);
            offsetLabel.Name = "offsetLabel";
            offsetLabel.Size = new Size(114, 20);
            offsetLabel.TabIndex = 13;
            offsetLabel.Text = "Current offset: 5";
            // 
            // offsetTrackBar
            // 
            offsetTrackBar.LargeChange = 10;
            offsetTrackBar.Location = new Point(0, 138);
            offsetTrackBar.Maximum = 50;
            offsetTrackBar.Minimum = 1;
            offsetTrackBar.Name = "offsetTrackBar";
            offsetTrackBar.Size = new Size(216, 56);
            offsetTrackBar.TabIndex = 12;
            offsetTrackBar.Value = 5;
            offsetTrackBar.Scroll += offsetTrackBar_Scroll;
            offsetTrackBar.ValueChanged += offsetTrackBar_ValueChanged;
            // 
            // drawOffsetCheckbox
            // 
            drawOffsetCheckbox.AutoSize = true;
            drawOffsetCheckbox.Location = new Point(6, 108);
            drawOffsetCheckbox.Name = "drawOffsetCheckbox";
            drawOffsetCheckbox.Size = new Size(108, 24);
            drawOffsetCheckbox.TabIndex = 11;
            drawOffsetCheckbox.Text = "Draw offset";
            drawOffsetCheckbox.UseVisualStyleBackColor = true;
            drawOffsetCheckbox.CheckedChanged += drawOffsetCheckbox_CheckedChanged;
            // 
            // offsetSettingsLabel
            // 
            offsetSettingsLabel.AutoSize = true;
            offsetSettingsLabel.Location = new Point(0, 85);
            offsetSettingsLabel.Name = "offsetSettingsLabel";
            offsetSettingsLabel.Size = new Size(104, 20);
            offsetSettingsLabel.TabIndex = 10;
            offsetSettingsLabel.Text = "Offset settings";
            // 
            // bresenhamCheckbox
            // 
            bresenhamCheckbox.AutoSize = true;
            bresenhamCheckbox.Location = new Point(6, 46);
            bresenhamCheckbox.Name = "bresenhamCheckbox";
            bresenhamCheckbox.Size = new Size(210, 24);
            bresenhamCheckbox.TabIndex = 9;
            bresenhamCheckbox.Text = "Use Bresenham's algorithm";
            bresenhamCheckbox.UseVisualStyleBackColor = true;
            bresenhamCheckbox.CheckedChanged += bresenhamCheckbox_CheckedChanged;
            // 
            // verticalRadioButton
            // 
            verticalRadioButton.AutoSize = true;
            verticalRadioButton.Location = new Point(6, 291);
            verticalRadioButton.Name = "verticalRadioButton";
            verticalRadioButton.Size = new Size(79, 24);
            verticalRadioButton.TabIndex = 8;
            verticalRadioButton.TabStop = true;
            verticalRadioButton.Text = "Vertical";
            verticalRadioButton.UseVisualStyleBackColor = true;
            verticalRadioButton.Visible = false;
            verticalRadioButton.CheckedChanged += verticalRadioButton_CheckedChanged;
            // 
            // horizontalRadioButton
            // 
            horizontalRadioButton.AutoSize = true;
            horizontalRadioButton.Location = new Point(6, 261);
            horizontalRadioButton.Name = "horizontalRadioButton";
            horizontalRadioButton.Size = new Size(100, 24);
            horizontalRadioButton.TabIndex = 7;
            horizontalRadioButton.TabStop = true;
            horizontalRadioButton.Text = "Horizontal";
            horizontalRadioButton.UseVisualStyleBackColor = true;
            horizontalRadioButton.Visible = false;
            horizontalRadioButton.CheckedChanged += horizontalRadioButton_CheckedChanged;
            // 
            // noneRadioButton
            // 
            noneRadioButton.AutoSize = true;
            noneRadioButton.Location = new Point(6, 231);
            noneRadioButton.Name = "noneRadioButton";
            noneRadioButton.Size = new Size(66, 24);
            noneRadioButton.TabIndex = 6;
            noneRadioButton.TabStop = true;
            noneRadioButton.Text = "None";
            noneRadioButton.UseVisualStyleBackColor = true;
            noneRadioButton.Visible = false;
            noneRadioButton.CheckedChanged += noneRadioButton_CheckedChanged;
            // 
            // edgeConstraintLabel
            // 
            edgeConstraintLabel.AutoSize = true;
            edgeConstraintLabel.Location = new Point(0, 208);
            edgeConstraintLabel.Name = "edgeConstraintLabel";
            edgeConstraintLabel.Size = new Size(112, 20);
            edgeConstraintLabel.TabIndex = 5;
            edgeConstraintLabel.Text = "Edge constraint";
            edgeConstraintLabel.Visible = false;
            // 
            // LineLabel
            // 
            LineLabel.AutoSize = true;
            LineLabel.Location = new Point(0, 23);
            LineLabel.Name = "LineLabel";
            LineLabel.Size = new Size(163, 20);
            LineLabel.TabIndex = 2;
            LineLabel.Text = "Line drawing algorithm";
            // 
            // PolygonEditor
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(942, 593);
            Controls.Add(mainGroupBox);
            Controls.Add(canvas);
            MaximumSize = new Size(960, 640);
            MinimumSize = new Size(960, 640);
            Name = "PolygonEditor";
            Text = "Polygon Editor";
            KeyDown += PolygonEditor_KeyDown;
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            mainGroupBox.ResumeLayout(false);
            mainGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)offsetTrackBar).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox canvas;
        private GroupBox mainGroupBox;
        private Label LineLabel;
        private Label edgeConstraintLabel;
        private RadioButton verticalRadioButton;
        private RadioButton horizontalRadioButton;
        private RadioButton noneRadioButton;
        private CheckBox bresenhamCheckbox;
        private Label offsetSettingsLabel;
        private TrackBar offsetTrackBar;
        private CheckBox drawOffsetCheckbox;
        private Label offsetLabel;
    }
}