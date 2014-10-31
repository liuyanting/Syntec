using System;
using System.Collections.Generic;
using Fenubars.XML;
using System.Windows.Forms;
using System.Drawing;

using Fenubars.Editor;

namespace Fenubars.Display
{
	public partial class ObjectTree : TreeView
	{
		public delegate void IndefiniteFenuOperationEventHandler();
		public event IndefiniteFenuOperationEventHandler NewFenu;
		public event IndefiniteFenuOperationEventHandler PasteFenu;
		public delegate void DefiniteFenuOperationEventHandler( string name );
		public event DefiniteFenuOperationEventHandler CutFenu;
		public event DefiniteFenuOperationEventHandler CopyFenu;
		public event DefiniteFenuOperationEventHandler DeleteFenu;

		private readonly string CUSTOM_FENU_HEADER = "CUSTOMFENU_";

		private List<FenuLink> links = new List<FenuLink>();

		public ObjectTree( string filePath, List<FenuState> fenus )
		{
			InitializeComponent();

			// Save file name
			this.Name = filePath;

			// Set context menu
			SetContextMenu();

			// RebuildForest
			RebuildForest( fenus );
		}

		private void SetContextMenu()
		{
			ToolStripMenuItem NewFenu_ToolStripMenuItem = new ToolStripMenuItem();
			NewFenu_ToolStripMenuItem.Text = "New Fenu";
			NewFenu_ToolStripMenuItem.Click += new EventHandler( NewFenu_ToolStripMenuItem_Click );

			ToolStripMenuItem PasteFenu_ToolStripMenuItem = new ToolStripMenuItem();
			PasteFenu_ToolStripMenuItem.Text = "Paste Fenu";
			PasteFenu_ToolStripMenuItem.Click += new EventHandler( PasteFenu_ToolStripMenuItem_Click );

			ToolStripSeparator ObjectFenu_ToolStripSeparator = new ToolStripSeparator();

			ToolStripMenuItem CutFenu_ToolStripMenuItem = new ToolStripMenuItem();
			CutFenu_ToolStripMenuItem.Text = "Cut Fenu";
			CutFenu_ToolStripMenuItem.Click += new EventHandler( CutFenu_ToolStripMenuItem_Click );

			ToolStripMenuItem CopyFenu_ToolStripMenuItem = new ToolStripMenuItem();
			CopyFenu_ToolStripMenuItem.Text = "Copy Fenu";
			CopyFenu_ToolStripMenuItem.Click += new EventHandler( CopyFenu_ToolStripMenuItem_Click );

			ToolStripMenuItem DeleteFenu_ToolStripMenuItem = new ToolStripMenuItem();
			DeleteFenu_ToolStripMenuItem.Text = "Delete Fenu";
			DeleteFenu_ToolStripMenuItem.Click += new EventHandler(DeleteFenu_ToolStripMenuItem_Click);

			this.ObjectTree_ContextMenuStrip.Items.Add( NewFenu_ToolStripMenuItem );
			this.ObjectTree_ContextMenuStrip.Items.Add( PasteFenu_ToolStripMenuItem );
			this.ObjectTree_ContextMenuStrip.Items.Add( ObjectFenu_ToolStripSeparator );
			this.ObjectTree_ContextMenuStrip.Items.Add( CutFenu_ToolStripMenuItem );
			this.ObjectTree_ContextMenuStrip.Items.Add( CopyFenu_ToolStripMenuItem );
			this.ObjectTree_ContextMenuStrip.Items.Add( DeleteFenu_ToolStripMenuItem );
		}

		private void RebuildForest( List<FenuState> fenus )
		{
			CompileLinksInfo( fenus );
			// First time execution, fully reconstruct the tree
			ConstructForest();
		}

		#region Context Menu Event

		private void ObjectTree_ContextMenuStrip_Opening( object sender, System.ComponentModel.CancelEventArgs e )
		{
			if( ClipBoardManager<FenuState>.Available() ) {
				this.ObjectTree_ContextMenuStrip.Items[ 1 ].Enabled = true;
			}
			else {
				this.ObjectTree_ContextMenuStrip.Items[ 1 ].Enabled = false;
			}

			if( this.SelectedNode == null ) {
				this.ObjectTree_ContextMenuStrip.Items[ 3 ].Enabled = false;
				this.ObjectTree_ContextMenuStrip.Items[ 4 ].Enabled = false;
				this.ObjectTree_ContextMenuStrip.Items[ 5 ].Enabled = false;
			}
			else {
				this.ObjectTree_ContextMenuStrip.Items[ 3 ].Enabled = true;
				this.ObjectTree_ContextMenuStrip.Items[ 4 ].Enabled = true;
				this.ObjectTree_ContextMenuStrip.Items[ 5 ].Enabled = true;
			}
		}

		private void NewFenu_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( this.NewFenu != null ) {
				this.NewFenu.Invoke();
			}
		}

		private void PasteFenu_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( this.PasteFenu != null ) {
				this.PasteFenu.Invoke();
			}
		}

		private void CutFenu_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( this.CutFenu != null ) {
				this.CutFenu.Invoke( this.SelectedNode.Text );
			}
		}

		private void CopyFenu_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( this.CopyFenu != null ) {

				this.CopyFenu.Invoke( this.SelectedNode.Text );
			}
		}

		private void DeleteFenu_ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( this.DeleteFenu != null ) {
				this.DeleteFenu.Invoke( this.SelectedNode.Text );
			}
		}



		#endregion

		#region Build Links

		private void CompileLinksInfo( List<FenuState> fenus )
		{
			links.Clear();
			foreach( FenuState fenu in fenus ) {
				FenuLink newLink = new FenuLink( fenu.Name );
				CompileChildLinks( newLink, fenu );
				links.Add( newLink );
			}
		}

		private void CompileChildLinks( FenuLink parent, FenuState fenu )
		{
			List<string> links = new List<string>();

			//// Add links from Escape Button
			//links.AddRange( ParseLinksFromButton( fenu.EscapeButton ) );
			// ^ Causing stack overflow

			// Add links from Normal Buttons
			foreach( FenuButtonState parsedNormalButton in fenu.NormalButtonList )
				links.AddRange( ParseLinksFromButton( parsedNormalButton ) );

			// Add links from Next Button
			if( fenu.NextButton != null )
				links.AddRange( ParseLinksFromButton( fenu.NextButton ) );

			// Add to fenu link object
			parent.Links = links;
		}

		private string[] ParseLinksFromButton( FenuButtonState button )
		{
			List<string> acquiredLinks = new List<string>();

			// Read from <link>
			if( button.Link != null )
				acquiredLinks.Add( button.Link );

			// Read from <actions>
			foreach( string action in button.Actions ) {
				if( action == null )
					continue;
				if( action != null && action.IndexOf( CUSTOM_FENU_HEADER ) == 0 )
					acquiredLinks.Add( action.Substring( CUSTOM_FENU_HEADER.Length ) );
			}

			return acquiredLinks.ToArray();
		}

		#endregion

		#region Build Forest

		private void ConstructForest()
		{
			this.Nodes.Clear();
			foreach( FenuLink Leaf in links ) {
				if( !IsInForest( Leaf.Name ) ) {
					this.Nodes.Add( Leaf.Name, Leaf.Name, 0, 0 );
					TreeNode Tree = this.Nodes[ Leaf.Name ];
					ConstructTree( Tree, Leaf );
				}
			}
		}

		private void ConstructTree( TreeNode Tree, FenuLink Parent )
		{
			if( Parent == null ) {
				return;
			}
			foreach( string ChildName in Parent.Links ) {
				if( !IsInForest( ChildName ) && !IsInTree( Tree, ChildName ) ) {
					Tree.Nodes.Add( ChildName, ChildName, 0, 0 );
					TreeNode Subtree = Tree.Nodes[ ChildName ];
					FenuLink Child = FindFenuLinkByName( ChildName );
					ConstructTree( Subtree, Child );
				}
			}
		}

		private bool IsInForest( string LeafName )
		{
			if( this.Nodes.ContainsKey( LeafName ) ) {
				return true;
			}

			foreach( TreeNode Tree in this.Nodes ) {
				if( IsInTree( Tree, LeafName ) ) {
					return true;
				}
			}

			return false;
		}

		private bool IsInTree( TreeNode Tree, string LeafName )
		{
			if( Tree.Nodes.ContainsKey( LeafName ) ) {
				return true;
			}
			else {
				foreach( TreeNode Subtree in Tree.Nodes ) {
					if( IsInTree( Subtree, LeafName ) ) {
						return true;
					}
				}
				return false;
			}
		}

		private FenuLink FindFenuLinkByName( string name )
		{
			return 
				links.Find( delegate( FenuLink selectedFenuLink )
				{
					return selectedFenuLink.Name == name;
				} );
		}

		#endregion

	}

	internal class FenuLink
	{
		public FenuLink( string name )
		{
			this._Name = name;
		}

		private string _Name = string.Empty;
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name = value;
			}
		}

		private List<string> _Links = new List<string>();
		public List<string> Links
		{
			get
			{
				return _Links;
			}
			set
			{
				_Links = value;
			}
		}
	}
}
