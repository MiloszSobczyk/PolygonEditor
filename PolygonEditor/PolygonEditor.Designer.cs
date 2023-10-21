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
            bitmap = new PictureBox();
            editorGroupBox = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)bitmap).BeginInit();
            SuspendLayout();
            // 
            // bitmap
            // 
            bitmap.Dock = DockStyle.Left;
            bitmap.Location = new Point(0, 0);
            bitmap.Name = "bitmap";
            bitmap.Size = new Size(697, 593);
            bitmap.TabIndex = 0;
            bitmap.TabStop = false;
            // 
            // editorGroupBox
            // 
            editorGroupBox.Location = new Point(703, 0);
            editorGroupBox.Name = "editorGroupBox";
            editorGroupBox.RightToLeft = RightToLeft.No;
            editorGroupBox.Size = new Size(235, 593);
            editorGroupBox.TabIndex = 1;
            editorGroupBox.TabStop = false;
            editorGroupBox.Text = "Editor";
            // 
            // PolygonEditor
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(942, 593);
            Controls.Add(editorGroupBox);
            Controls.Add(bitmap);
            MaximumSize = new Size(960, 640);
            MinimumSize = new Size(960, 640);
            Name = "PolygonEditor";
            Text = "Polygon Editor";
            ((System.ComponentModel.ISupportInitialize)bitmap).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox bitmap;
        private GroupBox editorGroupBox;
    }
}