using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Syntec.Windows
{
	public partial class NewWorkspaceDialog : Form
	{
		#region Properties

		private string _SelectedBaseRes = string.Empty;
		public string SelectedBaseRes
		{
			get
			{
				return this._SelectedBaseRes;
			}
		}

		#endregion

		#region Constructor

		public NewWorkspaceDialog()
		{
			InitializeComponent();
			this.SelectionPanel.PopulateCategory( Application.StartupPath + Global.DefinitonFolderPath + "Workspace.xml" );
		}

		#endregion

		#region Button Click

		private void OK_Button_Click( object sender, EventArgs e )
		{
			if( this.SelectionPanel.SelectedCategory == null ) {
				MessageBox.Show( "Please select a product" );
				return;
			}
			if( !Directory.Exists( this.InputPanel.SelectedPath ) ) {
				MessageBox.Show( "Please select a path" );
				return;
			}

			// extract seleted product path
			string ProductPath = this.SelectionPanel.SelectedCategory.Substring( this.SelectionPanel.SelectedCategory.IndexOf( Path.DirectorySeparatorChar ) + 1 );
			if( this.SelectionPanel.SelectedTemplate != string.Empty ) {
				ProductPath = ProductPath + Path.DirectorySeparatorChar + this.SelectionPanel.SelectedTemplate;
			}
			ProductPath = ProductPath.Replace( Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar + "_" );

			switch( this.InputPanel.SelectedSolution ) {
				// Create new Res folder
				case NewItemInputPanel.NewWorkspaceSolutionType.CreateNewResFolder:
					this._SelectedBaseRes = this.InputPanel.SelectedPath + Path.DirectorySeparatorChar + "Res";
					MessageBox.Show( _SelectedBaseRes );
					Directory.CreateDirectory( this._SelectedBaseRes + Path.DirectorySeparatorChar + ProductPath );
					break;

				// Add to existing Res folder
				case NewItemInputPanel.NewWorkspaceSolutionType.AddToExistingResFolder:
					int index = this.InputPanel.SelectedPath.ToUpper().LastIndexOf( "RES" );
					if( index < 0 ) {
						// This section should never occur
						MessageBox.Show( "Designated path isn't located in Res.",
											"Wrong File Path",
											MessageBoxButtons.OK,
											MessageBoxIcon.Error );
						return;
					}
					this._SelectedBaseRes = this.InputPanel.SelectedPath.Substring( 0, index ) + "Res";
					Directory.CreateDirectory( this._SelectedBaseRes + Path.DirectorySeparatorChar + ProductPath );
					break;
			}

			this.DialogResult = DialogResult.OK;
		}

		private void Cancel_Button_Click( object sender, EventArgs e )
		{
			this.DialogResult = DialogResult.Cancel;
		}

		#endregion
	}
}