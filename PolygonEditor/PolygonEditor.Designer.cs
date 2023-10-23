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
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
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
            canvas.MouseMove += canvas_MouseMove;
            // 
            // mainGroupBox
            // 
            mainGroupBox.Location = new Point(703, 0);
            mainGroupBox.Name = "mainGroupBox";
            mainGroupBox.RightToLeft = RightToLeft.No;
            mainGroupBox.Size = new Size(235, 593);
            mainGroupBox.TabIndex = 1;
            mainGroupBox.TabStop = false;
            mainGroupBox.Text = "Editor";
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
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox canvas;
        private GroupBox mainGroupBox;
    }
}