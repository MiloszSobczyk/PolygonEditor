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
            horizontalCheckbox = new CheckBox();
            verticalCheckbox = new CheckBox();
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
            mainGroupBox.Controls.Add(verticalCheckbox);
            mainGroupBox.Controls.Add(horizontalCheckbox);
            mainGroupBox.Location = new Point(703, 0);
            mainGroupBox.Name = "mainGroupBox";
            mainGroupBox.RightToLeft = RightToLeft.No;
            mainGroupBox.Size = new Size(235, 593);
            mainGroupBox.TabIndex = 1;
            mainGroupBox.TabStop = false;
            mainGroupBox.Text = "Editor";
            // 
            // horizontalCheckbox
            // 
            horizontalCheckbox.AutoSize = true;
            horizontalCheckbox.Location = new Point(6, 26);
            horizontalCheckbox.Name = "horizontalCheckbox";
            horizontalCheckbox.Size = new Size(101, 24);
            horizontalCheckbox.TabIndex = 0;
            horizontalCheckbox.Text = "Horizontal";
            horizontalCheckbox.UseVisualStyleBackColor = true;
            // 
            // verticalCheckbox
            // 
            verticalCheckbox.AutoSize = true;
            verticalCheckbox.Location = new Point(6, 56);
            verticalCheckbox.Name = "verticalCheckbox";
            verticalCheckbox.Size = new Size(80, 24);
            verticalCheckbox.TabIndex = 1;
            verticalCheckbox.Text = "Vertical";
            verticalCheckbox.UseVisualStyleBackColor = true;
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
        private CheckBox verticalCheckbox;
        private CheckBox horizontalCheckbox;
    }
}