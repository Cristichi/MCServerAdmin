using MCServerAdmin.datos;
using Microsoft.Data.Sqlite;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace MCServerAdmin
{

	public sealed partial class VerJugadores : Page
	{
		List<Jugador> ListaJugadores;

		public VerJugadores()
		{
			this.InitializeComponent();
			ListaJugadores = AdminJugs.GetJugadores();
		}

		private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count>0)
			{
				Jugador Jug = (Jugador) e.AddedItems.First();
				SPHerramientasJug.Visibility = Visibility.Visible;
				TxtNombre.Text = "Jugador: "+Jug.Nombre;
				TxtRango.Text = "Rango: "+Jug.Rango.ToString();
				ImgImagen.Source = new BitmapImage(new Uri("ms-appx:///"+Jug.Skin));
			}
			else
			{
				SPHerramientasJug.Visibility = Visibility.Collapsed;
			}
		}

		private void BtnBorrarJug_Click(object sender, RoutedEventArgs e)
		{
			AdminJugs.GetJugadores().RemoveAt(GVVerJugs.SelectedIndex);
			AdminJugs.GuardarJugadores();
			GVVerJugs.ItemsSource = AdminJugs.GetJugadores();
			Frame.Navigate(typeof(VerJugadores));
			Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
		}

		//AutosuggestBox
		private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
		{
			// Only get results when it was a user typing, 
			// otherwise assume the value got filled in by TextMemberPath 
			// or the handler for SuggestionChosen.
			if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
			{
				//Set the ItemsSource to be your filtered dataset
				List<Jugador> Busca = new List<Jugador>();
				foreach (Jugador Candy in AdminJugs.GetJugadores())
				{
					if (Candy.Nombre.Contains(ASBBuscar.Text))
					{
						Busca.Add(Candy);
					}
				}
				sender.ItemsSource = Busca;
			}
		}

		private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
		{
			// Set sender.Text. You can use args.SelectedItem to build your text string.
			sender.Text = args.SelectedItem.ToString();
		}

		private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
		{
			if (args.ChosenSuggestion != null)
			{
				// User selected an item from the suggestion list, take an action on it here.
			}
			else
			{
				// Use args.QueryText to determine what to do.
			}
		}
	}
}
