using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

using Fenubars;
using Fenubars.XML;
using Fenubars.Display;

namespace FormTest
{
	public partial class MainForm : Form
	{
		Fenubars.Handler loader;

		public MainForm( ) {
			InitializeComponent();

			try
			{
				loader = new Handler();
				loader.Initialize( "output.xml" );
				loader.Canvas = this.splitContainer1.Panel1.Controls;
				loader.PropertyViewer = propertyGrid1;
				loader.Load( "AutoTool" );
				loader.Load( "About" );
			}
			catch( FileLoadException )
			{
				MessageBox.Show( "Cannot load file." );
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			loader.Save();
		}
	}
}