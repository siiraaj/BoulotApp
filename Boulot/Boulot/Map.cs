using Android.App;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using System;
using Android.Locations;
using Android.Runtime;
using Android.Content;
using Plugin.Geolocator;
using System.Collections.Generic;
using System.Data;
using Android.Views;

namespace Boulot
{
    [Activity(Label = "Map")]
    public class Map : Activity, IOnMapReadyCallback,Android.Gms.Maps.GoogleMap.IInfoWindowAdapter
    {
        private GoogleMap mMap;
        
        private List<string> listAdresse = new List<string>();
        private List<string> listArtisan = new List<string>();
        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;
            
            getAllAdresse();
       
            googleMap.UiSettings.ZoomControlsEnabled = true;
            googleMap.UiSettings.CompassEnabled = true;
            googleMap.MoveCamera(CameraUpdateFactory.ZoomIn());

           
        }
       
        void getAllAdresse()
        {
            webService.h21WebService1 ws = new webService.h21WebService1();
            ws.getAllAdresseCompleted += Ws_getAllAdresseCompleted;
            ws.getAllAdresseAsync();

        }
        private void Ws_getAllAdresseCompleted(object sender, webService.getAllAdresseCompletedEventArgs e)
        {

            foreach (DataRow dt in e.Result.Rows)
            {
               
                listAdresse.Add(dt["adresse"].ToString());
                listArtisan.Add(dt["NOM_EMP"].ToString());
            }
            
            afficher_adresse(mMap);

            void afficher_adresse(GoogleMap googleMap)
            {
                int i = 0;
               
                foreach (string t in listAdresse)
                 {
                      
                     GetLocationFromAddress(t,i, googleMap);
                    i++;
                 }
               
            }
        }

       /* public  async void GetPosition(GoogleMap googleMap)
        {
            try
            {
                var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;
                if (locator.IsGeolocationEnabled)
                {
                    var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(1000));
                    LatLng coordonne = new LatLng(position.Latitude, position.Longitude);
                    MarkerOptions markerOptions = new MarkerOptions();
                    markerOptions.SetPosition(new LatLng(position.Latitude, position.Longitude));
                    markerOptions.SetTitle("Ma Position");
                    googleMap.AddMarker(markerOptions);
                    googleMap.MoveCamera(CameraUpdateFactory.NewLatLng(coordonne));
                }
                else
                {
                    Console.WriteLine("La location ne trouve pas, geolocator est éteint.");
                }

                
            }
            catch (Exception le)
            {
                
                Console.WriteLine(le+"La location ne trouve pas.");
            }
        }*/

        public void GetLocationFromAddress(string strAddress,int id, GoogleMap googleMap)
        {
            Geocoder coder = new Geocoder(this);
          
            IList<Address> address = null;
            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetSnippet(strAddress);
            address = coder.GetFromLocationName(strAddress, 5);
            if (address == null)
            {
                return;
            }
            else
            {
                
                for (int i = 0; i < address.Count; i++)
                {
                    Address ad = address[i];
                    
                    markerOptions.SetPosition(new LatLng(ad.Latitude, ad.Longitude));
                    markerOptions.SetTitle(listArtisan[id]);
                    
                    googleMap.AddMarker(markerOptions);
                   
                }
                googleMap.SetInfoWindowAdapter(this);


            }
            
        }

        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.map);
            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);
            


        }

        public View GetInfoContents(Marker marker)
        {
            return null;
        }

        public View GetInfoWindow(Marker marker)
        {
            View view = LayoutInflater.Inflate(Resource.Layout.info_window, null, false);
            view.FindViewById<EditText>(Resource.Id.txtNom).Text=marker.Title;
            view.FindViewById<EditText>(Resource.Id.etAdresse).Text=marker.Snippet;
           
            return view;
        }
    }
}

