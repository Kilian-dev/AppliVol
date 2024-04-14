using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using AppliVol.Models;

namespace AppliVol.Controllers
{
    public class VilleController : Controller
    {

        //Interface de manipulation de la BDD FireBase
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "iHweF8EdSGQ6kvPC2IEuZr5eJvqYvg2ex3RJRsj8",
            BasePath = "https://applivol-default-rtdb.firebaseio.com"


        };
        IFirebaseClient? client;


        // GET: HomeController1
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Villes");
            dynamic? data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Ville>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Ville>(((JProperty)item).Value.ToString()));
                }
            }
            return View(list);
        }

        // GET: HomeController1/Details/5
        public ActionResult Details(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Villes/" + id);
            Ville? ville = JsonConvert.DeserializeObject<Ville>(response.Body);

            var list = new List<Ville>();
            FirebaseResponse res = client.Get("Villes");
            dynamic? data1 = JsonConvert.DeserializeObject<dynamic>(res.Body);

            if (data1 != null)
            {
                foreach (var item in data1)
                {

                    list.Add(JsonConvert.DeserializeObject<Ville>(((JProperty)item).Value.ToString()));
                }
            }
            list = list.Where(v => v.IdVille == ville.IdVille).ToList();

            //ville.Livres = list;

            return View(ville);
        }

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ville ville)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                PushResponse response = client.Push("Auteurs/", ville);
               ville.IdVille = response.Result.name;
                SetResponse setResponse = client.Set("Auteurs/" + ville.IdVille, ville);

                if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    ModelState.AddModelError(string.Empty, "OK");
                else
                    ModelState.AddModelError(string.Empty, "KO!!");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction("Index");
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Villes/" + id);
            Ville ville = JsonConvert.DeserializeObject<Ville>(response.Body);
            return View(ville);
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ville ville)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("Villes/" + ville.IdVille, ville);
            return RedirectToAction("Index");
        }

        // GET: HomeController1/Delete/5
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Villes/" + id);
            Ville vil = JsonConvert.DeserializeObject<Ville>(response.Body);
            return View(vil);
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, Ville ville)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("Auteurs/" + id);
            return RedirectToAction("Index");
        }
    }
}
