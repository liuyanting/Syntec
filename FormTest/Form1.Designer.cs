namespace FormTest
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
		protected override void Dispose(bool disposing) {
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( ) {
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.label1 = new System.Windows.Forms.Label();
			this.propertyGrid1 = new Azuria.Common.Controls.FilteredPropertyGrid();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add( this.propertyGrid1 );
			this.splitContainer1.Size = new System.Drawing.Size( 1091, 325 );
			this.splitContainer1.SplitterDistance = 816;
			this.splitContainer1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 981, 351 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 35, 13 );
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.BrowsableProperties = null;
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid1.HiddenAttributes = null;
			this.propertyGrid1.HiddenProperties = null;
			this.propertyGrid1.Location = new System.Drawing.Point( 0, 0 );
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size( 271, 325 );
			this.propertyGrid1.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 1091, 390 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.splitContainer1 );
			this.Name = "MainForm";
			this.Text = "Form Test";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.MainForm_FormClosing );
			this.splitContainer1.Panel2.ResumeLayout( false );
			this.splitContainer1.ResumeLayout( false );
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label label1;
		private Azuria.Common.Controls.FilteredPropertyGrid propertyGrid1;
	}
}

