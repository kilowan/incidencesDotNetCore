using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiPrimeraApp.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraApp.Controllers
{
    [Route("api/Credentials")]
    [ApiController]
    public class CredentialsController : ControllerBase
    {
        private CredentialsBz cred;
        public CredentialsController()
        {
            this.cred = new();
        }
        // GET: CredentialsController
        /*public ActionResult Index()
        {
            return View();
        }*/

        // GET: CredentialsController/Details/5
        //[HttpGet("username={username} password={password}")]
        public bool Details(string username, string password)
        {
            return this.cred.CheckCredentialsFn(username, password);
        }

        // GET: CredentialsController/Create
        /*public ActionResult Create()
        {
            return View();
        }*/

        // POST: CredentialsController/Create
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/

        // GET: CredentialsController/Edit/5
        /*public ActionResult Edit(int id)
        {
            return View();
        }*/

        // POST: CredentialsController/Edit/5
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/

        // GET: CredentialsController/Delete/5
        /*public ActionResult Delete(int id)
        {
            return View();
        }*/

        // POST: CredentialsController/Delete/5
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
