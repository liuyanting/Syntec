using System.Windows.Forms;
using Fenubars.XML;
using System.Drawing;

namespace Fenubars.Buttons
{
	public partial class NormalButton : Button
	{
		public delegate void ButtonModifiedHandler();
		public event ButtonModifiedHandler Modified;

		public delegate string GetResourceEventHandler( string ID );
		public event GetResourceEventHandler OnGetResource;

		public NormalButton( int Index )
		{
			InitializeComponent();

			// Non-variable visual setup
			this.Size = new Size( 80, 60 );
			this.Location = new Point( 3 + 80 * Index, 3 );
		}

		public void SetState( FenuButtonState State )
		{
			SetState( State, false );
		}
		public void SetState( FenuButtonState State, bool isForeign )
		{
			// Wipe bindings
			this.DataBindings.Clear();
			this.ResetText();
			this.FlatStyle = FlatStyle.Popup;

			// Adjust button style to indicate configured or not
			if( State == null )
				return;
			if( !isForeign )
				this.FlatStyle = FlatStyle.Standard;

			// Create binding source for this button
			Binding bind;
			BindingSource bindingSource = new BindingSource();
			bindingSource.DataSource = State;

			// Bindings
			this.DataBindings.Add( "Name", bindingSource, "Name" );
			this.DataBindings.Add( "BackColor", bindingSource, "BackColor" );
			this.DataBindings.Add( "ForeColor", bindingSource, "ForeColor" );

			bind = new Binding( "Text", bindingSource, "Title" );
			bind.Format += new ConvertEventHandler( IdToContent );
			this.DataBindings.Add( bind );

			bind = new Binding( "Enabled", bindingSource, "State" );
			bind.Format += new ConvertEventHandler( StateConverter.StateToBool );
			bind.Parse += new ConvertEventHandler( StateConverter.BoolToState );
			this.DataBindings.Add( bind );

			// Post-filled
			//State.Name = State.Name ?? "F" + State.Position.ToString();

			bindingSource.CurrentItemChanged += new System.EventHandler( bindingSource_CurrentItemChanged );
			// Reset IsDirty
			//_IsDirty = false;
		}

		private void bindingSource_CurrentItemChanged( object sender, System.EventArgs e )
		{
			if( Modified != null ) {
				Modified();
			}
		}

		public void PaintComponent( System.Windows.Forms.Control.ControlCollection Canvas )
		{
			Canvas.Add( this );
		}

		public void IdToContent( object sender, ConvertEventArgs cevent )
		{
			if( cevent.Value != null ) {
				string id = (string)cevent.Value;
				if( id.ToUpper().StartsWith( "STR::" ) ) {
					string resource = this.OnGetResource( id.Substring( 5 ) );
					cevent.Value = ( resource == string.Empty ) ? id : resource;
				}
			}
		}
	}
}
