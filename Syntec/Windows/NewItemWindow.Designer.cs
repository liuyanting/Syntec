namespace Syntec.Windows
{
	partial class NewItemWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.newItemSelectionPanel1 = new Syntec.Windows.NewItemSelectionPanel();
			this.SuspendLayout();
			// 
			// newItemSelectionPanel1
			// 
			this.newItemSelectionPanel1.Location = new System.Drawing.Point( 12, 12 );
			this.newItemSelectionPanel1.Name = "newItemSelectionPanel1";
			this.newItemSelectionPanel1.Size = new System.Drawing.Size( 640, 320 );
			this.newItemSelectionPanel1.TabIndex = 0;
			// 
			// NewItemWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 664, 424 );
			this.Controls.Add( this.newItemSelectionPanel1 );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "NewItemWindow";
			this.Text = "NewItemWindow";
			this.ResumeLayout( false );

		}

		#endregion

		private NewItemSelectionPanel newItemSelectionPanel1;

	}
}