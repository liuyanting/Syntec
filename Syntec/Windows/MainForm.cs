using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;

using WeifenLuo.WinFormsUI.Docking;

using Syntec;
using Syntec.Module;

namespace Syntec.Windows
{
	public partial class MainForm : Form
	{
		// General windows
		private FindResultsForm FindResults = new FindResultsForm();
		private WorkspaceExplorerForm WorkspaceExplorer = new WorkspaceExplorerForm();
		private PropertiesWindowForm PropertiesWindow = new PropertiesWindowForm();
		private ObjectBrowserForm ObjectBrowser = new ObjectBrowserForm();
		private StringManagerForm StringManager = new StringManagerForm();
		//private FindResultsViewerForm FindResultsViewer = new FindResultsViewerForm();

		// Most recently used items variables
		private const string INI_FILE_NAME = "Syntec.ini";
		private const string INI_RECENT_WORKSPACES_SECTION = "RecentWorkspaces";
		private const string INI_RECENT_FILES_SECTION = "RecentFiles";
		private const int DEFAULT_MAX_ENTRIES = 4;
		private MruStripMenu RecentWorkspacesMenu;
		private MruStripMenu RecentFilesMenu;

		public MainForm()
		{
			InitializeComponent();

			// Bind events
			WorkspaceExplorer.OnOpenFile += new WorkspaceExplorerForm.OpenFileEventHanler( OpenFile );

			// Load all the windows and set them to default locations
			PropertiesWindow.Show( Main_DockPanel, DockState.DockRight );
			ObjectBrowser.Show( PropertiesWindow.Pane, DockAlignment.Top, 0.6 );
			StringManager.Show( ObjectBrowser.Pane, ObjectBrowser );
			WorkspaceExplorer.Show( StringManager.Pane, StringManager );

			PopulateRecentlyUsedMenu();
		}

		#region Form related

		private void PopulateRecentlyUsedMenu()
		{
			RecentWorkspacesMenu = new MruStripMenu(
				File_Recent_Workspaces_ToolStripMenuItem,
				new MruStripMenu.ClickedHandler( RecentWorkspaces_OnClick ),
				INI_FILE_NAME, INI_RECENT_WORKSPACES_SECTION, DEFAULT_MAX_ENTRIES );
			RecentWorkspacesMenu.LoadFromINIFile();

			RecentFilesMenu = new MruStripMenu(
				File_Recent_Files_ToolStripMenuItem,
				new MruStripMenu.ClickedHandler( RecentFiles_OnClick ),
				INI_FILE_NAME, INI_RECENT_FILES_SECTION, DEFAULT_MAX_ENTRIES );
			RecentFilesMenu.LoadFromINIFile();
		}

		private void MainForm_Load( object sender, EventArgs e )
		{
			// Initiate module manager
			ModuleManager.Refresh();
		}

		private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			RecentWorkspacesMenu.SaveToINIFile();
			RecentFilesMenu.SaveToINIFile();
		}

		#endregion

		#region File

		private void Open_Workspace_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			OpenDialog( "Open Workspace", true );
		}

		private void Open_File_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			OpenDialog( "Open File", false, "XML Files (*.xml)|*.xml" );
		}

		private void File_Close_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			DocumentsForm DF = this.ActiveMdiChild as DocumentsForm;
			if( DF != null ) {
				DF.Close();
			}
		}

		private void File_Save_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			DocumentsForm DF = this.ActiveMdiChild as DocumentsForm;
			if( DF != null ) {
				DF.Save();
			}
		}

		private void File_SaveAs_ToolStripMenuItem_Click( object sender, EventArgs e )
		{

		}

		private void File_SaveAll_ToolStripMenuItem_Click( object sender, EventArgs e )
		{

		}

		#region Recent Workspaces/Files

		private void RecentWorkspaces_OnClick( int index, string filename )
		{
			if( !Directory.Exists( filename ) ) {
				MessageBox.Show( "The directory is no longer existent.",
									"Directory not found",
									MessageBoxButtons.OK,
									MessageBoxIcon.Exclamation );
				return;
			}

			WorkspaceExplorer.ShowWorkspace( filename );


			StringManager.ShowString( filename );

			// Switch workspace explorer tab
			WorkspaceExplorer.Show();
		}

		private void RecentFiles_OnClick( int index, string filename )
		{
			if( !File.Exists( filename ) ) {
				MessageBox.Show( "The file is no longer existent.",
									"File not found",
									MessageBoxButtons.OK,
									MessageBoxIcon.Exclamation );
				return;
			}

			OpenFile( filename );

			// Notify no workspace shown
			MessageBox.Show( "Open a file independently won't open the corresponding workspace.",
								"Notice",
								MessageBoxButtons.OK,
								MessageBoxIcon.Information );

			// Switch to object browser tab
			ObjectBrowser.Show();
		}

		#endregion

		private void File_Exit_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Close();
		}

		#endregion

		#region Edit

		private void Edit_Cut_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			DocumentsForm DF = this.ActiveMdiChild as DocumentsForm;
			if( DF != null ) {
				DF.Cut();
			}
		}

		private void Edit_Copy_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			DocumentsForm DF = this.ActiveMdiChild as DocumentsForm;
			if( DF != null ) {
				DF.Copy();
			}
		}

		private void Edit_Paste_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			DocumentsForm DF = this.ActiveMdiChild as DocumentsForm;
			if( DF != null ) {
				DF.Paste();
			}
		}

		private void Edit_Delete_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			DocumentsForm DF = this.ActiveMdiChild as DocumentsForm;
			if( DF != null ) {
				DF.Delete();
			}
		}

		private void Edit_Search_ToolStripMenuItem_Click( object sender, EventArgs e )
		{

			string key = "�t��";

			// TODO: Show a dialog as searching interface

			string[] results = this.FindIDsByContent( key );

			this.FindResults.ClearItem();

			// search in all opened documents

			foreach( Form form in this.MdiChildren ) {
				if( typeof( DocumentsForm ) == form.GetType() ) {
					( form as DocumentsForm ).SearchForIDUsers( results );
				}
			}

			if( FindResults.IsHidden ) {
				FindResults.Show( Main_DockPanel, DockState.DockBottom );
				Main_DockPanel.UpdateDockWindowZOrder( DockStyle.Bottom, false );
			}
			else {
				FindResults.Show();
			}
			// or search in this.MdiChildren

			//DocumentsForm DF = this.ActiveMdiChild as DocumentsForm;
			//if( DF != null ) {
			//    // Do find ID user
			//}

		}

		#endregion

		#region View

		private void View_WorkspaceExplorer_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( WorkspaceExplorer.IsHidden ) {
				WorkspaceExplorer.Show();
			}
		}

		private void View_PropertiesWindow_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( PropertiesWindow.IsHidden ) {
				PropertiesWindow.Show();
			}
		}

		private void View_ObjectBrowser_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( ObjectBrowser.IsHidden ) {
				ObjectBrowser.Show();
			}
		}

		#endregion

		#region Tools

		private void Tools_ModuleManager_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			using( ModuleManagerForm PMF = new ModuleManagerForm() ) {
				PMF.ShowDialog();
			}
		}

		#endregion

		#region Helper methods

		private void OpenDialog( string title, bool dirMode )
		{
			OpenDialog( title, dirMode, "All Files|*.*" );
		}

		private void OpenDialog( string title, bool dirMode, string filter )
		{
			// Determine which type of dialog it is
			if( dirMode ) {
				FolderBrowserDialog dialog = new FolderBrowserDialog();

				dialog.Description = @"Workspace should located under ""Res"" folder.";
				dialog.ShowNewFolderButton = false;

				if( dialog.ShowDialog() != DialogResult.OK )
					return;

				RecentWorkspacesMenu.AddFile( dialog.SelectedPath );

				WorkspaceExplorer.ShowWorkspace( dialog.SelectedPath );

				StringManager.ShowString( dialog.SelectedPath );

				// Switch to workspace explorer tab
				WorkspaceExplorer.Show();
			}
			else {
				OpenFileDialog dialog = new OpenFileDialog();

				dialog.InitialDirectory = @"C:\";
				dialog.Filter = filter;
				dialog.RestoreDirectory = true;

				if( dialog.ShowDialog() != DialogResult.OK )
					return;

				RecentFilesMenu.AddFiles( dialog.FileNames );

				// Open all the selected files
				foreach( string fileName in dialog.FileNames )
					OpenFile( fileName );

				// Notify no workspace shown
				MessageBox.Show( "Open a file independently won't open the corresponding workspace.",
									"Notice",
									MessageBoxButtons.OK,
									MessageBoxIcon.Information );

				// Switch to object browser tab
				ObjectBrowser.Show();
			}
		}

		private void OpenFile( string filePath )
		{
			// Document opened, switch tab
			IDockContent target = null;

			// Scan for opened documents
			foreach( IDockContent content in Main_DockPanel.Contents ) {
				if( content.DockHandler.ToolTipText == filePath )
					target = content;
			}

			if( target != null ) {
				target.DockHandler.Show();
				return;
			}

			DocumentsForm openFromFile = new DocumentsForm( filePath,
																new DocumentsForm.ShowPropertiesEventHandler( ShowProperties ),
																new DocumentsForm.SetPropertyGridEventHandler( SetPropertyGrid ),
																new DocumentsForm.ShowObjectsEventHandler( ShowObjects ),
																new DocumentsForm.ShowStatusInfoEventHandler( ShowStatusInfo ),
																new DocumentsForm.GetResourceEventHandler( GetResource ),
																new DocumentsForm.FindResultsEventHandler( OnFindResults ) );

			if( openFromFile.IsDisposed )
				return;

			openFromFile.Show( Main_DockPanel, DockState.Document );
			openFromFile.TabText = Path.GetFileNameWithoutExtension( filePath );
			openFromFile.ToolTipText = filePath;
		}

		private void ShowProperties( object control )
		{
			PropertiesWindow.SetSelectedObject( control );
		}

		private void SetPropertyGrid( AttributeCollection hidden, string[] browsable )
		{
			PropertiesWindow.SetHiddenAttributes( hidden );
			PropertiesWindow.SetBrowsableProperties( browsable );
		}

		private void ShowObjects( Control treeView )
		{
			ObjectBrowser.SetContents( treeView );

			// When tree view is null, it means the file will be closed
			// Discard the object browser and properties window and switch to workspace explorer tabb
			if( treeView == null )
				WorkspaceExplorer.Show();
			else
				ObjectBrowser.Show();
		}

		private void ShowStatusInfo( string text, int progress, bool marquee )
		{
			if( progress > 100 )
				throw new ArgumentOutOfRangeException( "Progress bar value shouldn't exceed 100." );

			// Using null to maintain view, string.Empty to clear the text
			if( text != null )
				this.StatusText.Text = text;

			// Using -1 to hide the progress bar
			if( progress < 0 )
				this.ProgressBar.Visible = false;
			else {
				this.ProgressBar.Visible = true;

				// Marquee mode or not
				if( marquee )
					this.ProgressBar.Style = ProgressBarStyle.Marquee;
				else {
					this.ProgressBar.Style = ProgressBarStyle.Blocks;
					this.ProgressBar.Value = progress;
				}
			}
		}

		private string GetResource( string Path, string ID, string Language )
		{
			return this.StringManager.FindString( Path, ID, Language );
		}

		private void OnFindResults( DocumentsForm host, ModuleInterface.SearchResult[] results )
		{
			foreach( ModuleInterface.SearchResult result in results){
				this.FindResults.AddItem( host, result );
			}
		}

		private string[] FindIDsByContent( string content )
		{
			return this.StringManager.FindIDsByContent( content );
		}

		#endregion

		#region Test codes

		private void Test_Button_Click( object sender, EventArgs e )
		{
			string test = this.StringManager.FindString( @"D:\Res\_Arm\_Mill\_11A\Common\CncFenu.xml", "Str::Fenu::Main::MenuProgram", "CHT" );
			MessageBox.Show( test );
		}

		#endregion

	}
}