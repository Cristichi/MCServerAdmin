using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace MCServerAdmin
{
	/// <summary>
	/// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
	/// </summary>
	public sealed partial class NuevoJugRegistrado : Page
	{
		public NuevoJugRegistrado()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			try
			{
				string nombre = e.Parameter as string;
                bool nuevo = nombre.Substring(0,1)=="0"?true:false;
                nombre = nombre.Substring(1);
                if (nombre.Length>40)
                {
                    nombre = nombre.Substring(0, 37).Trim()+"...";
                }
				TxtNuevoJug.Text = "Se ha "+(nuevo?"registrado":"editado")+" a \"" + nombre + "\" correctamente.";
			}
			catch(NullReferenceException)
			{
				Frame.Navigate(typeof(VerJugadores));
				Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
			}

		}

		private void Button_Click_OK(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(VerJugadores));
			Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
		}
	}
}
