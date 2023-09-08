namespace Sudoku_solver
{
    partial class Form
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
            pnButton = new Panel();
            btnSolve = new Button();
            btnClear = new Button();
            btnFill = new Button();
            SuspendLayout();
            // 
            // pnButton
            // 
            pnButton.BackColor = SystemColors.ControlDark;
            pnButton.Location = new Point(12, 12);
            pnButton.Name = "pnButton";
            pnButton.Size = new Size(850, 850);
            pnButton.TabIndex = 0;
            // 
            // btnSolve
            // 
            btnSolve.Location = new Point(902, 144);
            btnSolve.Name = "btnSolve";
            btnSolve.Size = new Size(214, 73);
            btnSolve.TabIndex = 1;
            btnSolve.Text = "Solve";
            btnSolve.UseVisualStyleBackColor = true;
            btnSolve.Click += btnSolve_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(902, 763);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(214, 73);
            btnClear.TabIndex = 2;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // btnFill
            // 
            btnFill.Location = new Point(902, 244);
            btnFill.Name = "btnFill";
            btnFill.Size = new Size(214, 73);
            btnFill.TabIndex = 3;
            btnFill.Text = "Solve && Fill";
            btnFill.UseVisualStyleBackColor = true;
            btnFill.Click += btnFill_Click;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1141, 873);
            Controls.Add(btnFill);
            Controls.Add(btnClear);
            Controls.Add(btnSolve);
            Controls.Add(pnButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sudoku solver";
            TopMost = true;
            ResumeLayout(false);
        }

        #endregion

        private Panel pnButton;
        private Button btnSolve;
        private Button btnClear;
        private Button btnFill;
    }
}