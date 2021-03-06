﻿using MCServerAdmin.datos;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace MCServerAdmin
{
	/// <summary>
	/// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
	/// </summary>
	public sealed partial class RegistrarNuevo : Page
	{
		ObservableCollection<String> cbItems = new ObservableCollection<String>();
		ObservableCollection<Rango> cbRangos = new ObservableCollection<Rango>();
        
		Jugador editando;
		public RegistrarNuevo()
		{
			this.InitializeComponent();
			InicializarComboBox();

			cbRangos.Add(Rango.USUARIO);
			cbRangos.Add(Rango.VIP);
			cbRangos.Add(Rango.YOUTUBER);
			cbRangos.Add(Rango.ADMIN);
			cbRangos.Add(Rango.OWNER);
            
        }

		private async void InicializarComboBox()
		{
			StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
			StorageFolder assets = await appInstalledFolder.GetFolderAsync("Assets");
			IReadOnlyCollection<StorageFile> files = await assets.GetFilesAsync();
			foreach (StorageFile archivo in files)
			{
				string nombre = archivo.Name;
				if (nombre.EndsWith(".Skin.png") || nombre.EndsWith(".Skin.jpg"))
				{
					cbItems.Add(nombre.Substring(0, nombre.Length-9));
				}
			}

            if (editando!=null)
            {
                try
                {
                    bool elegida = false;
                    int i;
                    for (i = 0; i < cbItems.Count && !elegida; i++)
                    {
                        if (editando.Skin.Contains(cbItems.ElementAt(i)))
                        {
                            CBSkin.SelectedIndex = i;
                            elegida = true;
                        }
                    }
                    if (!elegida)
                    {
                        CBSkin.PlaceholderText = "No se pudo autoseleccionar la skin";
                    }
                }
                catch
                {
                }
            }
        }

		private void Button_Click_Registrar(object sender, RoutedEventArgs e)
		{
            if (TxtNombre.Text.Trim().Equals(""))
            {
                TxtNombre.PlaceholderText = "Escribe un nombre";
                TxtNombre.Text = "";
            }
			else if (CBSkin.SelectedIndex<0)
			{
				CBSkin.PlaceholderText = "Elige una Skin";
			}
			else if (CBRango.SelectedIndex<0)
			{
				CBRango.PlaceholderText = "Elige el Rango del usuario";
			}
			else
			{
				DateTimeOffset fecha = DPFecha.Date;
                try
                {
                    int dinero = 0;
                    try
                    {
                        dinero = Int32.Parse(TxtDinero.Text);
                    }
                    catch {

                    }
                    
                    Jugador Nuevo = new Jugador(TxtNombre.Text, "Assets/" + CBSkin.SelectedItem.ToString() + ".Skin.png", (Rango)(CBRango.SelectedItem),
                    dinero, fecha.Day, fecha.Month, fecha.Year);
                    if (editando==null)
                    {
                        AdminJugs.GetJugadores().Add(Nuevo);
                    }
                    else
                    {
                        int index = AdminJugs.GetJugadores().IndexOf(editando);
                        AdminJugs.GetJugadores().Insert(index, Nuevo);
                        AdminJugs.GetJugadores().Remove(editando);
                    }
                    AdminJugs.GuardarJugadores();
                    Frame.Navigate(typeof(NuevoJugRegistrado), (editando == null ? 0 : 1) + Nuevo.Nombre);
                    Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
                }
                catch(Exception error)
                {
                    BtnAgregar.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255,255,0,0));
                    BtnAgregar.Content = error.Message;
                }
			}
		}

		private void Button_Click_Cancelar(object sender, RoutedEventArgs e)
		{
			if (Frame.CanGoBack)
			{
				Frame.GoBack();
			}
			else
			{
				Frame.Navigate(typeof(VerJugadores));
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                editando = (Jugador) e.Parameter;

                //Editando jugador
                TxtNombre.Text = editando.Nombre;

                try
                {
                    CBRango.SelectedItem = editando.Rango;
                }
                catch
                {
                }

                try
                {
                    DPFecha.Date = editando.FechaIngreso;
                }
                catch
                {
                }

                TxtDinero.Text = editando.DineroVirtual + "";
                BtnAgregar.Content = "Aplicar";
                TxtTitulo.Text = "Editando a "+editando.Nombre;
            }
            catch
            {
                //Nuevo jugador!
            }


		}

        private void CBSkin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ImgSkin.Source = new BitmapImage(new Uri("ms-appx:///Assets/" + CBSkin.SelectedItem.ToString()+".Skin.png")); ;
        }
    }
}
