﻿using System;
using System.Collections.Generic;
using DataAccessLib;
using Microsoft.Data.Sqlite;

namespace MCServerAdmin.datos
{
	class Jugador : IComparable<Jugador>
    {
		public string Nombre { get; set; }
		public string Skin { get; set; }
		public Rango Rango { get; set; }
		public int DineroVirtual { get; set; }
		public DateTime FechaIngreso { get; set; }

		public Jugador()
		{
			this.Nombre = "Jugador Autogenerado";
			Skin = "Assets/PorDefecto.Skin.png";
			Rango = Rango.USUARIO;
			DineroVirtual = 0;
			FechaIngreso = DateTime.Today;
		}

		public Jugador(string nombre, Rango rango, int dinero)
		{
			this.Nombre = nombre;
			Skin = "Assets/PorDefecto.Skin.png";
			Rango = rango;
			DineroVirtual = dinero;
			FechaIngreso = DateTime.Today;
		}

		public Jugador(string Nombre, string Skin, Rango rango, int dinero)
		{
			this.Nombre = Nombre;
			this.Skin = Skin;
			Rango = rango;
			DineroVirtual = dinero;
			FechaIngreso = DateTime.Today;
		}

		public Jugador(string Nombre, string Skin, Rango rango, int dinero, int dia, int mes, int year)
		{
			this.Nombre = Nombre;
			this.Skin = Skin;
			Rango = rango;
			DineroVirtual = dinero;
			FechaIngreso = new DateTime(year, mes, dia);
		}

		public override string ToString()
		{
			return "[" + Rango + "] "+Nombre;
		}

        public int CompareTo(Jugador other)
        {
            int sol = (AdminJugs.GetIdRango(other.Rango).CompareTo(AdminJugs.GetIdRango(Rango)));
            if (sol == 0)
            {
                sol = Nombre.CompareTo(other.Nombre);
            }
            return sol;
        }
    }

	enum Rango
	{
		USUARIO, VIP, YOUTUBER, ADMIN, OWNER		
	}

	class AdminJugs
	{
		public static Rango GetRango(byte rango)
		{
			Rango sol = Rango.USUARIO;
			switch (rango)
			{
				case 1:
					sol = Rango.VIP;
					break;
				case 2:
					sol = Rango.YOUTUBER;
					break;
				case 3:
					sol = Rango.ADMIN;
					break;
				case 4:
					sol = Rango.OWNER;
					break;
			}
			return sol;
		}

		public static byte GetIdRango(Rango rango)
		{
			byte sol = 0;
			switch (rango)
			{
				case Rango.VIP:
					sol = 1;
					break;
				case Rango.YOUTUBER:
					sol = 2;
					break;
				case Rango.ADMIN:
					sol = 3;
					break;
				case Rango.OWNER:
					sol = 4;
					break;
			}
			return sol;
		}

		private static List<Jugador> Lista;

		public static List<Jugador> GetJugadores()
		{
			if (Lista is null)
			{
				Lista = new List<Jugador>();
				using (SqliteConnection db = new SqliteConnection("Filename=" + MainPage.RUTA_DB))
				{
					db.Open();

					string tableCommand = "select * from Jugadores";

					SqliteCommand selectCommand = new SqliteCommand(tableCommand, db);

					SqliteDataReader query = selectCommand.ExecuteReader();

					while (query.Read())
					{
						Lista.Add(new Jugador(
							query.GetString(1), query.GetString(2), GetRango(query.GetByte(3)),
							query.GetInt32(4), query.GetInt32(5), query.GetInt32(6), query.GetInt32(7)
							));
					}
					db.Close();
				}

                Lista.Sort();
			}
			
			return Lista;
		}

		public static void GuardarJugadores()
		{
			using (SqliteConnection db = new SqliteConnection("Filename=" + MainPage.RUTA_DB))
			{
				db.Open();

				string limpiar = "delete from Jugadores";

				SqliteCommand limpiarExe = new SqliteCommand(limpiar, db);

				limpiarExe.ExecuteReader();

				if (Lista.Count > 0)
				{
					string reescribir = "";
					int i = 0;
					foreach (Jugador jug in Lista)
					{
						reescribir += "insert into Jugadores values(" + i++ + ", '" + jug.Nombre + "', '" + jug.Skin + "', '" + GetIdRango(jug.Rango) + "', " +
							jug.DineroVirtual + ", " + jug.FechaIngreso.Day + ", " + jug.FechaIngreso.Month + ", " + jug.FechaIngreso.Year + ");";
					}

					SqliteCommand reescribirExe = new SqliteCommand(reescribir, db);

					reescribirExe.ExecuteReader();
				}

				db.Close();
			}
		}
	}
}