namespace scalarm_client_csharp_monitor
{
    partial class MainForm
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loginButton = new System.Windows.Forms.Button();
            this.newFormBox = new System.Windows.Forms.Button();
            this.fetchButton = new System.Windows.Forms.Button();
            this.credentialsButton = new System.Windows.Forms.Button();
            this.fetchedExperimentsListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(12, 16);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Register";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // newFormBox
            // 
            this.newFormBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.newFormBox.Location = new System.Drawing.Point(0, 247);
            this.newFormBox.Name = "newFormBox";
            this.newFormBox.Size = new System.Drawing.Size(557, 23);
            this.newFormBox.TabIndex = 4;
            this.newFormBox.Text = "newFormBox";
            this.newFormBox.UseVisualStyleBackColor = true;
            this.newFormBox.Click += new System.EventHandler(this.newFormBox_Click);
            // 
            // fetchButton
            // 
            this.fetchButton.Location = new System.Drawing.Point(12, 55);
            this.fetchButton.Name = "fetchButton";
            this.fetchButton.Size = new System.Drawing.Size(75, 23);
            this.fetchButton.TabIndex = 5;
            this.fetchButton.Text = "Fetch";
            this.fetchButton.UseVisualStyleBackColor = true;
            this.fetchButton.Click += new System.EventHandler(this.fetchButton_Click);
            // 
            // credentialsButton
            // 
            this.credentialsButton.Location = new System.Drawing.Point(102, 16);
            this.credentialsButton.Name = "credentialsButton";
            this.credentialsButton.Size = new System.Drawing.Size(75, 23);
            this.credentialsButton.TabIndex = 6;
            this.credentialsButton.Text = "Credentials";
            this.credentialsButton.UseVisualStyleBackColor = true;
            this.credentialsButton.Click += new System.EventHandler(this.credentialsButton_Click);
            // 
            // fetchedExperimentsListBox
            // 
            this.fetchedExperimentsListBox.FormattingEnabled = true;
            this.fetchedExperimentsListBox.Location = new System.Drawing.Point(12, 105);
            this.fetchedExperimentsListBox.Name = "fetchedExperimentsListBox";
            this.fetchedExperimentsListBox.Size = new System.Drawing.Size(313, 108);
            this.fetchedExperimentsListBox.TabIndex = 7;
            this.fetchedExperimentsListBox.SelectedIndexChanged += new System.EventHandler(this.fetchedExperimentsListBox_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 270);
            this.Controls.Add(this.fetchedExperimentsListBox);
            this.Controls.Add(this.credentialsButton);
            this.Controls.Add(this.fetchButton);
            this.Controls.Add(this.newFormBox);
            this.Controls.Add(this.loginButton);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Button newFormBox;
        private System.Windows.Forms.Button fetchButton;
        private System.Windows.Forms.Button credentialsButton;
        private System.Windows.Forms.ListBox fetchedExperimentsListBox;

    }
}

