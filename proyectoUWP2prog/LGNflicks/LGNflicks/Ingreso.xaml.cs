﻿using LGNflicks.DTOS;
using LGNflicks.Models;
using LGNflicks.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace LGNflicks
{
    
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Ingreso : Page
    {
        private bool esActivo;
        int recPeliculaID;
        public Ingreso()
        {
            this.InitializeComponent();
            llamarDtos();
            usuario();
        }
        public void usuario() 
        {
            string nomusser = MainPage.nomusser;
            esActivo = MainPage.esActivo;
            int usid = MainPage.idusser;
            nomus.Text = nomusser;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
        private async void llamarDtos()
        {
            List<peliculaTransformada> Lista = new List<peliculaTransformada>();
            var httpEventHandler = new HttpClientHandler();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://localhost:44379/api/Peliculas");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient(httpEventHandler);
            HttpResponseMessage response = await client.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<List<PeliculaDto>>(content);
            foreach (PeliculaDto pelicula in resultado)
            {
                    BitmapImage imagentransformada = await Herramientas.ArrayToBmI(pelicula.Imagen);
                    peliculaTransformada peliculaVista = new peliculaTransformada(pelicula.peliculaID, pelicula.Nombre, pelicula.Sinopsis.ToString(), pelicula.Lanzamiento, pelicula.Escritores, pelicula.Descripcion, imagentransformada, pelicula.GeneroNombre, pelicula.CantVotos);
                    Lista.Add(peliculaVista);
            }
            GridPeliculas.ItemsSource = Lista;
        }
        private async void botonVotar_Click(object sender, RoutedEventArgs e)
        {
            Grid gridPadre = (Grid)(sender as Button).Parent;
            var contexto = gridPadre.DataContext as peliculaTransformada;
            var IdPelicula = contexto.peliculaID;
            if (esActivo == true) {
                esActivo = false;
                var usuariID = MainPage.nomusser;
                recPeliculaID = IdPelicula;
                var votoDto = new VotoDto
                {
                    UsuarioID = usuariID,
                    PeliculaID = IdPelicula,
                    Delete = false,
                    EsActivo = esActivo
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(votoDto), Encoding.UTF8, "application/json");
                string url = "https://localhost:44379/api/VotoPeliculas";
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var respuesta = await httpClient.PostAsync(url, content);
                if (!respuesta.IsSuccessStatusCode)
                {
                    //TODO: levantar excepcion;
                    return;
                }
                //TODO OK
            }
            else
            {
                disponibildadbtn.Text = "Lo sentimos no puedes votar mas de una vez";
                eliminarVoto.Visibility = Visibility.Visible;
                recPeliculaID = IdPelicula;
            }

            llamarDtos();
        }

        private async void eliminarVoto_Click(object sender, RoutedEventArgs e)
        {
            if (esActivo == false)
            {
                esActivo = true;
                var usuariID = MainPage.nomusser;
                var IdPelicula = recPeliculaID;
                var votoDto = new VotoDto
                {
                    UsuarioID = usuariID,
                    PeliculaID = IdPelicula,
                    Delete = true,
                    EsActivo = esActivo
                };
                StringContent content = new StringContent(
                JsonConvert.SerializeObject(votoDto),
                Encoding.UTF8,
                "application/json");
                string url = "https://localhost:44379/api/VotoPeliculas";
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var respuesta = await httpClient.PostAsync(url, content);
                if (!respuesta.IsSuccessStatusCode)
                {
                    //TODO: levantar excepcion;
                    return;
                }
                disponibildadbtn.Text = "";
                eliminarVoto.Visibility = Visibility.Collapsed;
            }
            llamarDtos();
        }

        private void borrarVoto_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
