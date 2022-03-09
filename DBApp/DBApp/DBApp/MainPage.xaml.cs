using DBApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DBApp
{
    public partial class MainPage : ContentPage
    {
        public static string nomusser;
        public static bool esActivo;
        public static int iduser;
        public MainPage()
        {
            InitializeComponent();
        }
        async void ingresoclk(object sender, EventArgs args) 
        {
            List<Usuarios> usuarios = new List<Usuarios>();
            string user = Txt_Usuariolog.Text;
            string pass = Txt_Password.Text;
            nomusser = user;
            if ((user == null || pass == null) || (user == "" && pass == ""))
            {
                mext.Text = "Existen campos vacios";
            }
            else
            {
                var HttpHeadrer = new HttpClientHandler();
                var solicitud = new HttpRequestMessage();
                solicitud.RequestUri = new Uri("http://10.0.2.2:60424/api/Usuarios?email=" + user);
                solicitud.Method = HttpMethod.Get;
                solicitud.Headers.Add("Accept", "application/json");
                var Client = new HttpClient(HttpHeadrer);
                HttpResponseMessage respuesta = await Client.SendAsync(solicitud);
                string contenido = await respuesta.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<Usuarios>>(contenido);
                usuarios = data;
                if (contenido == "[]")
                {
                    mext.Text = "Ingreso de usuario incorrecto o no existente";
                }
                foreach (Usuarios aux1 in usuarios)
                {
                    if (aux1.CorreoElectronico == user && aux1.Contrasenia == pass)
                    {
                        if (aux1.tipoSuscripcion == 2)
                        {
                            mext.Text = "No se permite el ingreso de administradores";
                            break;
                        }
                        esActivo = aux1.EsActivo; //Recupera si el usuario es activo o no.
                        await Navigation.PushModalAsync(new Ingreso());
                        break;
                    }

                    else
                    {
                        
                        
                            mext.Text = "Ingreso contrasenia incorrecta";
                        
                       
                    }
                }
            } 
            
        }
    }
}
