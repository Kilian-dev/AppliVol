using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using AppliVol.Models;

namespace AppliVol.Controllers
{
    public class VolController : Controller
    {


        //Interface de manipulation de la BDD FireBase
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "iHweF8EdSGQ6kvPC2IEuZr5eJvqYvg2ex3RJRsj8",
            BasePath = "https://applivol-default-rtdb.firebaseio.com"


        };
        IFirebaseClient? client;


        // GET: VolController
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Vols");
            dynamic? data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Vol>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Vol>(((JProperty)item).Value.ToString()));
                }
            }
            return View(list);
        }

        // GET: VolController/Details/5
        public ActionResult Details(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Vols/" + id);
            Vol? aut = JsonConvert.DeserializeObject<Vol>(response.Body);
            return View(aut);
        }

        // GET: VolController/Create
        public ActionResult Create()
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
            ViewBag.Villes = list;

            return View();
        }

        // POST: VolController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vol vol)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                PushResponse response = client.Push("Vols/", vol);
                vol.IdVol = response.Result.name;
                SetResponse setResponse = client.Set("Vols/" + vol.IdVol, vol);

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

        // GET: VolController/Edit/5
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Vols/" + id);
            Vol aut = JsonConvert.DeserializeObject<Vol>(response.Body);
            return View(aut);
        }

        // POST: VolController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vol vol)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("Vols/" + vol.IdVol, vol);
            return RedirectToAction("Index");
        }

        // GET: VolController/Delete/5
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Vols/" + id);
            Vol aut = JsonConvert.DeserializeObject<Vol>(response.Body);
            return View(aut);
        }

        // POST: VolController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, Vol vol)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("Vols/" + id);
            return RedirectToAction("Index");
        }
    }
}
