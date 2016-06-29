using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;

namespace WebApplication1.Controllers
{
    public class DictionariesController : Controller
    {
        private DatabaseEntities1 db = new DatabaseEntities1();

        // GET: Dictionaries
        public ActionResult Index()
        {
            return View(db.Dictionaries.ToList());
        }

        // GET: Dictionaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dictionaries dictionaries = db.Dictionaries.Find(id);
            if (dictionaries == null)
            {
                return HttpNotFound();
            }
            return View(dictionaries);
        }

        // GET: Dictionaries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dictionaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
                                    //[Bind(Include = "Id,DictionaryName,SourceLanguage,Path")]
        public ActionResult Create(Dictionaries dictionaries)
        {
            if (ModelState.IsValid)
            {
                dictionaries.upload(dictionaries);

                db.Dictionaries.Add(dictionaries);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dictionaries);
        }

        // GET: Dictionaries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dictionaries dictionaries = db.Dictionaries.Find(id);
            if (dictionaries == null)
            {
                return HttpNotFound();
            }
            return View(dictionaries);
        }

        // POST: Dictionaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DictionaryName,SourceLanguage,Path")] Dictionaries dictionaries)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dictionaries).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dictionaries);
        }

        // GET: Dictionaries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dictionaries dictionaries = db.Dictionaries.Find(id);
            if (dictionaries == null)
            {
                return HttpNotFound();
            }
            return View(dictionaries);
        }

        // POST: Dictionaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dictionaries dictionaries = db.Dictionaries.Find(id);
            db.Dictionaries.Remove(dictionaries);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
