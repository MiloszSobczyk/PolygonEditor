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
            algorithmComboBox = new ComboBox();
            verticalRadioButton = new RadioButton();
            horizontalRadioButton = new RadioButton();
            noneRadioButton = new RadioButton();
            edgeConstraintLabel = new Label();
            LineLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            mainGroupBox.SuspendLayout();
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
            mainGroupBox.Controls.Add(algorithmComboBox);
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
            // algorithmComboBox
            // 
            algorithmComboBox.FormattingEnabled = true;
            algorithmComboBox.Items.AddRange(new object[] { "Library algorithm", "Bresenham's algorithm" });
            algorithmComboBox.Location = new Point(6, 46);
            algorithmComboBox.Name = "algorithmComboBox";
            algorithmComboBox.Size = new Size(221, 28);
            algorithmComboBox.TabIndex = 9;
            algorithmComboBox.SelectedIndexChanged += algorithmComboBox_SelectedIndexChanged;
            // 
            // verticalRadioButton
            // 
            verticalRadioButton.AutoSize = true;
            verticalRadioButton.Location = new Point(6, 170);
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
            horizontalRadioButton.Location = new Point(6, 140);
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
            noneRadioButton.Location = new Point(6, 110);
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
            edgeConstraintLabel.Location = new Point(0, 87);
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
        private ComboBox algorithmComboBox;
    }
}