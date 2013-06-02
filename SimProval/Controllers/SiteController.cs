using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimProval.Models;

namespace SimProval.Controllers
{
    using Helpers;
    using ViewModels;
    using AutoMapper;

    public class SiteController : Controller
    {
        private SimPROVALEntities db = new SimPROVALEntities();
       

        //
        // GET: /Site/

        public ActionResult Index()
        {
            return View(db.Sites.OrderByDescending(s => s.Id).ToList());
        }

        //
        // GET: /Site/Details/5

        public ActionResult Details(long id = 0)
        {
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        //
        // GET: /Site/Create

        public ActionResult Create()
        {
            var model = new SiteViewModel
                            {
                                // Address
                                // ********* insert GEO location result here (Device wrapper)  **********//
                                Height = 2.700
                                //StructureList = new SelectList(LookupHelper.GetStructureTypeList(), "Value", "Text")
                            };
          
            return View(model);
        }

        //
        // POST: /Site/Create

        [HttpPost]
        public ActionResult Create(SiteViewModel model)
        {
            if (ModelState.IsValid)
            {

                SiteCoord sc = Maps.GetLocForAddress(model.Address);
                string region = Maps.GetRegion(sc);
                
                //Mapper.Map<Site>(model);
                Site s = new Site()
                {
                    Address = model.Address,
                    Height = model.Height
                };
                try
                {
                    DesignWindCalculation calc = new DesignWindCalculation();

                    double designSpeed = calc.For(s);

                    SiteSurvey survey = new SiteSurvey();
                    survey = calc.Result(ref s);
                } catch(Exception e)
                {
                }
                db.Sites.Add(s);

                db.SaveChanges();
                return RedirectToAction("Edit", new { id = s.Id } );
            }

            return View(model);
        }

        //
        // GET: /Site/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        //
        // POST: /Site/Edit/5

        [HttpPost]
        public ActionResult Edit(Site site)
        {
            if (ModelState.IsValid)
            {
                db.Entry(site).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(site);
        }

        //
        // GET: /Site/Delete/5

        public ActionResult Delete(long id = 0)
        {
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        //
        // POST: /Site/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            Site site = db.Sites.Find(id);
            db.Sites.Remove(site);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}