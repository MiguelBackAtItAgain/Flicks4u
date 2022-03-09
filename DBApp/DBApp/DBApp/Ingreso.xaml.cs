using DBApp.DTO;
using DBApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DBApp
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class Ingreso : ContentPage
    {
        private bool esAct;
        int recPeliculaID;
        public Ingreso()
        {
            InitializeComponent();
        }

        private async Task llamarDtos()
        {
            List<peliculaTransformada> Lista = new List<peliculaTransformada>();
            var httpEventHandler = new HttpClientHandler();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://10.0.2.2:60424/api/Peliculas");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient(httpEventHandler);
            HttpResponseMessage response = await client.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<List<PeliculaDto>>(content);
            foreach (PeliculaDto pelicula in resultado)
            {
                ImageSource imagentransformada = ImageSource.FromStream(() => new MemoryStream(pelicula.Imagen));
                peliculaTransformada peliculaVista = new peliculaTransformada(pelicula.peliculaID, pelicula.Nombre, pelicula.Sinopsis, pelicula.Lanzamiento, pelicula.Escritores, pelicula.Descripcion, imagentransformada, pelicula.GeneroNombre, pelicula.CantVotos);
                Lista.Add(peliculaVista);
            }
            ItemsPeliculas.ItemsSource = Lista;
        }

        protected override async void OnAppearing()
        {
            await llamarDtos();
            CorreoUsuario.Text = MainPage.nomusser;
            esAct = MainPage.esActivo;
        }

        private async void botonVotar_ClickAsync(object sender, EventArgs e)
        {
            var padre = (sender as Button).Parent;
            var contexto = padre.BindingContext as peliculaTransformada;
            recPeliculaID = contexto.peliculaID;
            if(esAct)
            {
                esAct = false;
                var VotoDto = new VotoDto
                {
                    UsuarioID = MainPage.nomusser,
                    PeliculaID = contexto.peliculaID,
                    Delete = false,
                    EsActivo = esAct
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(VotoDto),Encoding.UTF8,"application/json");
                string url = "http://10.0.2.2:60424/api/VotoPeliculas";
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var respuesta = await httpClient.PostAsync(url, content);
                if (!respuesta.IsSuccessStatusCode)
                {
                    //TODO: levantar excepcion;
                    return;
                }
                disponibildad.Text = "";
                eliminarVoto.IsVisible = false;
                await llamarDtos();
            }
            else
            {
                disponibildad.Text = "Lo sentimos no puedes votar mas de una vez";
                eliminarVoto.IsVisible = true;
            }
        }

        private async void eliminarVoto_Click(object sender, EventArgs e)
        {
            if (!esAct)
            {
                esAct = true;
                var usuariID = MainPage.nomusser;
                var IdPelicula = recPeliculaID;
                var votoDto = new VotoDto
                {
                    UsuarioID = usuariID,
                    PeliculaID = IdPelicula,
                    Delete = true,
                    EsActivo = esAct
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(votoDto),Encoding.UTF8,"application/json");
                string url = "http://10.0.2.2:60424/api/VotoPeliculas";
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var respuesta = await httpClient.PostAsync(url, content);
                if (!respuesta.IsSuccessStatusCode)
                {
                    //TODO: levantar excepcion;
                    return;
                }
                disponibildad.Text = "";
                eliminarVoto.IsVisible = false;
            }
            await llamarDtos();
        }

        private async void Salir_ClickAsync(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MainPage());
        }
    }
}