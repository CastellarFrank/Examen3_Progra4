using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Examen3_Progra4.Controllers
{
    public class meetingController : Controller
    {

        private MyDBEntities db = new MyDBEntities();

        public ActionResult MyMeetings()
        {
            return View(this.getTodos());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Examen3_Progra4.Models.meetingStructure model, FormCollection form)
        {

            meeting myMeet = new meeting
            {
                nombre = model.meetName,
                descripcion = model.meetDescription,
                fecha= model.meetDate,
                longitud=model.meetLength,
                latitud=model.meetLatitude,
                userOwner = this.User.Identity.Name,
            };
            try
            {
                db.meetings.AddObject(myMeet);
                db.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("", "It can't create the new Meeting");
                return View(model);
            }

            return RedirectToAction("MyMeetings");


        }

        public ActionResult Edit(int id = 0)
        {
            meeting myMeet = db.meetings.Single(u => u.idMeet == id);
            Examen3_Progra4.Models.meetingStructure temp = new Examen3_Progra4.Models.meetingStructure
            {
                meetId = myMeet.idMeet,
                meetName = myMeet.nombre,
                meetDescription = myMeet.descripcion,
                meetDate = myMeet.fecha,
                meetLatitude=myMeet.latitud,
                meetLength=myMeet.longitud,
                userOwner = myMeet.userOwner
            };
            return View(temp);
        }

        //
        // POST: /Movies/Edit/5

        [HttpPost]
        public ActionResult Edit(Examen3_Progra4.Models.meetingStructure meet)
        {
            
            meeting myMeet = db.meetings.Single(u => u.idMeet == meet.meetId);
            myMeet.nombre = meet.meetName;
            myMeet.descripcion = meet.meetDescription;
            myMeet.fecha = meet.meetDate;
            myMeet.longitud = meet.meetLength;
            myMeet.latitud = meet.meetLatitude;
            try
            {
                db.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("", "It can't edit this Meeting");
                return View(meet);
            }

            return RedirectToAction("MyMeetings");

        }
        public ActionResult Delete(int id = 0)
        {
            System.Diagnostics.Debug.WriteLine("ID: "+id);
            meeting myMeet = db.meetings.Single(u => u.idMeet == id);
            db.meetings.DeleteObject(myMeet);
            try
            {
                db.SaveChanges();
            }
            catch
            {
                /*Laalal*/
            }

            return RedirectToAction("MyMeetings");
        }

        public ActionResult Details(int id = 0)
        {
            meeting myMeet = db.meetings.Single(u => u.idMeet == id);

            Examen3_Progra4.Models.meetingStructure temp = new Examen3_Progra4.Models.meetingStructure
            {
                meetId = myMeet.idMeet,
                meetName = myMeet.nombre,
                meetDescription = myMeet.descripcion,
                meetDate = myMeet.fecha,
                meetLatitude=myMeet.latitud,
                meetLength=myMeet.longitud,
                userOwner = myMeet.userOwner
            };
            return View(temp);
        }
        private IEnumerable<Examen3_Progra4.Models.meetingStructure> getTodos()
        {
            var values = from u in db.meetings
                         where u.userOwner.Equals(this.User.Identity.Name)
                         orderby u.idMeet
                         select u;

            List<Examen3_Progra4.Models.meetingStructure> results = new List<Examen3_Progra4.Models.meetingStructure>();

            foreach (meeting val in values)
            {
                Examen3_Progra4.Models.meetingStructure temp = new Examen3_Progra4.Models.meetingStructure
                {
                    meetId= val.idMeet,
                    meetName = val.nombre,
                    meetDescription = val.descripcion,
                    meetDate = val.fecha,
                    meetLatitude=val.latitud,
                    meetLength = val.longitud,
                    userOwner = val.userOwner
                };
                results.Add(temp);
            }

            return results;

        }

    }
}
