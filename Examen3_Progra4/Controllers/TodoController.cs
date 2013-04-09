using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Examen3_Progra4.Models;

namespace Examen3_Progra4.Controllers
{
    public class TodoController : Controller
    {
        private MyDBEntities db = new MyDBEntities();

        public ActionResult MyToDo(string DateS="",string DateF="")
        {
            DateTime start,finish;
            if(!DateS.Equals("") && !DateS.Equals("")){
                string[] arrayS=DateS.Split('/');
                string[] arrayF = DateF.Split('/');
                try
                {
                    start = new DateTime(int.Parse(arrayS[2]),int.Parse(arrayS[1]),int.Parse(arrayS[0]));
                    finish = new DateTime(int.Parse(arrayF[2]), int.Parse(arrayF[1]), int.Parse(arrayF[0]));
                }
                catch
                {
                    ViewBag.error = "The Filter's Date aren't valid";
                    return View(this.getTodos());
                }

                return View(this.getTodos().Where(x => ((x.dateF.CompareTo(finish) <= 0) && (x.dateF.CompareTo(start) > 0))));
                
            }
            ViewBag.error = "";
            return View(this.getTodos());
        }

        public ActionResult Create()
        {
            List<string> statusList = new List<string>();
            statusList.Add("Outgoing");
            statusList.Add("Cancel");
            statusList.Add("Done");

            ViewBag.statusList = new SelectList(statusList);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Examen3_Progra4.Models.todoEstructure model,string statusList)
        {
                string valStatus = (statusList.Equals("Outgoing") ? "O" : statusList.Equals("Cancel") ? "C" : "D");
                
                todo myTodo = new todo
                {
                    nombre=model.nameTodo,
                    descripcion=model.descriptionTodo,
                    fechaI=model.dateI,
                    fechaF=model.dateF,
                    status=valStatus,
                    userOwner=this.User.Identity.Name,
                };
                try
                {
                    db.todos.AddObject(myTodo);
                    db.SaveChanges();
                }
                catch
                {
                    ModelState.AddModelError("", "It can't create the new TO-DO");
                    return View(model);
                }

                return RedirectToAction("MyToDo", "Todo");
            

            // If we got this far, something failed, redisplay form
        }

        public ActionResult Edit(int id = 0)
        {
            todo myTodo = db.todos.Single(u => u.idTodo == id);
            todoEstructure temp = new todoEstructure
            {
                 IdTodo=myTodo.idTodo,
                 nameTodo=myTodo.nombre,
                 descriptionTodo=myTodo.descripcion,
                 dateI=myTodo.fechaI,
                 dateF=myTodo.fechaF,
                 status=myTodo.status,
                 userOwner=myTodo.userOwner
            };

            string valStatus = (myTodo.status.Equals("O") ? "Outgoing" : myTodo.status.Equals("C") ? "Cancel" : "Done");
            List<string> statusList=new List<string>();
            statusList.Add(valStatus);

            if(!statusList.Contains("Outgoing"))
                statusList.Add("Outgoing");
            if(!statusList.Contains("Cancel"))
                statusList.Add("Cancel");
            if(!statusList.Contains("Done"))
                statusList.Add("Done");

            ViewBag.statusList= new SelectList(statusList);
            
            return View(temp);
        }

        //
        // POST: /Movies/Edit/5

        [HttpPost]
        public ActionResult Edit(todoEstructure todo,string statusList)
        {
            System.Diagnostics.Debug.WriteLine("Estado: " + statusList);
            string valStatus = (statusList.Equals("Outgoing") ? "O" : statusList.Equals("Cancel") ? "C" : "D");
            System.Diagnostics.Debug.WriteLine("Estado2: " + valStatus);
            
                System.Diagnostics.Debug.WriteLine("Entro");
                todo myTodo = db.todos.Single(u => u.idTodo == todo.IdTodo);
                myTodo.nombre = todo.nameTodo;
                myTodo.descripcion = todo.descriptionTodo;
                myTodo.fechaI = todo.dateI;
                myTodo.fechaF = todo.dateF;
                myTodo.status = valStatus;
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    ModelState.AddModelError("", "It can't edit this TO-DO");
                    return View(todo);
                }                
                
                return RedirectToAction("MyToDo");
            
        }

        public ActionResult Delete(int id = 0)
        {
            todo myTodo = db.todos.Single(u => u.idTodo == id);
            
            todoEstructure temp = new todoEstructure
            {
                IdTodo = myTodo.idTodo,
                nameTodo = myTodo.nombre,
                descriptionTodo = myTodo.descripcion,
                dateI = myTodo.fechaI,
                dateF = myTodo.fechaF,
                status = myTodo.status,
                userOwner = myTodo.userOwner
            };
            return View(temp);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(todoEstructure model)
        {

            todo myTodo = db.todos.Single(u => u.idTodo == model.IdTodo);
            db.todos.DeleteObject(myTodo);
            try
            {
                db.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("", "It can't delete this TO-DO");
                todoEstructure temp = new todoEstructure
                {
                    IdTodo = myTodo.idTodo,
                    nameTodo = myTodo.nombre,
                    descriptionTodo = myTodo.descripcion,
                    dateI = myTodo.fechaI,
                    dateF = myTodo.fechaF,
                    status = myTodo.status,
                    userOwner = myTodo.userOwner
                };
                return View(temp);
            }
            
            return RedirectToAction("MyToDo");
        }
        public ActionResult Details(int id=0)
        {
            todo myTodo = db.todos.Single(u => u.idTodo == id);

            todoEstructure temp = new todoEstructure
            {
                IdTodo = myTodo.idTodo,
                nameTodo = myTodo.nombre,
                descriptionTodo = myTodo.descripcion,
                dateI = myTodo.fechaI,
                dateF = myTodo.fechaF,
                status = myTodo.status,
                userOwner = myTodo.userOwner
            };
            return View(temp);
        }

        private IEnumerable<Examen3_Progra4.Models.todoEstructure> getTodos()
        {
            var values = from u in db.todos
                         where u.userOwner.Equals(this.User.Identity.Name)
                         orderby u.idTodo
                         select u;

            List<Examen3_Progra4.Models.todoEstructure> results = new List<Examen3_Progra4.Models.todoEstructure>();
            
            foreach(todo val in values){
                Examen3_Progra4.Models.todoEstructure temp = new Examen3_Progra4.Models.todoEstructure
                {
                    IdTodo=val.idTodo,
                    nameTodo=val.nombre,
                    descriptionTodo=val.descripcion,
                    dateI=val.fechaI,
                    dateF=val.fechaF,
                    status=val.status,
                    userOwner=val.userOwner
                };
                results.Add(temp);                
            }

            return results;
                       
        }

    }
}
