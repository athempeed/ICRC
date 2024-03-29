﻿using IC_RC.Models;
using ICRC.Model;
using ICRC.Model.ViewModel;
using ICRCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IC_RC.Controllers
{
    [Authorize]
    public class ReciprocitiesController : Controller
    {
        public readonly ICertifiedPersonService CertifiedPersonService;
        public readonly IReciprocitiesService reciprocityService;
        public readonly ICertificateService certificateService;
        public readonly IBoardService BoardService;
        public readonly IPaymentTypeService paymentService;
        string returnUrl = "";
        Users user;

        public ReciprocitiesController(ICertifiedPersonService CertifiedPersonService, IReciprocitiesService reciprocityService, IBoardService BoardService, ICertificateService certificateService, IPaymentTypeService paymentService)
        {
            this.CertifiedPersonService = CertifiedPersonService;
            this.reciprocityService = reciprocityService;
            this.BoardService = BoardService;
            this.certificateService = certificateService;
            this.paymentService = paymentService;           
        }


        // GET: Reciprocity
        public ActionResult Index()
        {
            var user = ShrdMaster.Instance.GetUser(User.Identity.Name);
            if (user == null)
            {
                ViewBag.Error = "User is not active.";
                return Redirect("/Account/login");
            }
            var data = GetReciprocities();

            return View(data);
        }
        public void ExportToExcel(string person = "", string oboard = "", string rboard = "", string certificate = "", string number = "", string notes = "")
        {
            var data = GetReciprocities(person, oboard, rboard, certificate, number, notes).AsEnumerable();
            ShrdMaster.Instance.ExportListFromTsv(data, "Reciprocities");
        }
        public List<ReciprocitiesForIndex> GetReciprocities(string person = "", string oboard = "", string rboard = "", string certificate = "", string number = "", string notes = "")
        {
            var user = ShrdMaster.Instance.GetUser(User.Identity.Name);

            List<ReciprocitiesForIndex> reciprocities;
            if (ShrdMaster.Instance.IsAdmin(user.Username))
            {
                reciprocities = reciprocityService.GetReciprocitiesForIndex(person, oboard, rboard, certificate, number, notes).ToList();
            }
            else
            {
                reciprocities = reciprocityService.GetReciprocitiesByBoardID(user.BoardID, person, oboard, rboard, certificate, number, notes).ToList();
            }

            return reciprocities;
        }

        public ActionResult GetData(string person="",string oboard="",string rboard="",string certificate="",string number="",string notes="")
        {
            var reciprocities = GetReciprocities(person,oboard,rboard,certificate,number,notes);
            return PartialView("_Reciprocities",reciprocities);
        }


        // GET: Reciprocity/Details/5
        public ActionResult Details(int ?id)
        {
            SetReturnUrl();
            if(id==null)
            {
                return HttpNotFound();
            }
            var data = reciprocityService.GetReciprocitiesByID(id.Value);
            return View(data);
        }

        // GET: Reciprocity/Create
        public ActionResult Create()
        {
            SetReturnUrl();
            ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            ViewBag.Certificates = new SelectList(certificateService.GetCertificates(), "ID", "Name");
            ViewBag.PaymentTypes = paymentService.GetPaymentTypes();
           // ViewBag.Persons= new SelectList(CertifiedPersonService.GetCertifiedPersons(), "ID", "FullName");
            return View();
        }

        // POST: Reciprocity/Create
        [HttpPost]
        public ActionResult Create(Reciprocities model)
        {
            SetReturnUrl();
            if(ModelState.IsValid)
            {
                
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = SessionContext<int>.Instance.GetSession("UserID"); 
                reciprocityService.CreateReciprocity(model);
                reciprocityService.Save();
                return Redirect(returnUrl);
            }

            ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            ViewBag.Certificates = new SelectList(certificateService.GetCertificates(), "ID", "Name");
            ViewBag.PaymentTypes = paymentService.GetPaymentTypes();
          //  ViewBag.Persons = new SelectList(CertifiedPersonService.GetCertifiedPersons(), "ID", "FullName");
            return View(model);
        }

        // GET: Reciprocity/Edit/5
        public ActionResult Edit(int ?id)
        {

            SetReturnUrl();
            if(id==null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }
            var data = reciprocityService.GetReciprocitiesByID(id.Value);
            ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            ViewBag.Certificates = new SelectList(certificateService.GetCertificates(), "ID", "Name");
            ViewBag.PaymentTypes = paymentService.GetPaymentTypes();
            var person = CertifiedPersonService.GetCertifiedPersonByID(data.PersonID ?? data.PersonID.Value);
            string name = "";
            if (person != null)
            {
                if (!string.IsNullOrEmpty(person.FirstName))
                {
                    name += person.FirstName + " ";
                }
                if (!string.IsNullOrEmpty(person.MiddleName))
                {
                    name += person.MiddleName + " ";
                }
                if (!string.IsNullOrEmpty(person.LastName))
                {
                    name += person.LastName + " ";
                }

                //name = string.Format($"{person.FirstName != null ? person.FirstName + " ":\"\"}");
                //name = person.FirstName != null ? person.FirstName + " " + person.MiddleName != null ? person.MiddleName + " " + person.LastName != null ? person.LastName:"":"":"";
            }
            ViewBag.Person = name;
          

            return View(data);
        }

        // POST: Reciprocity/Edit/5
        [HttpPost]
        public ActionResult Edit(Reciprocities model)
        {
            SetReturnUrl();
            model.Status = true;
           if(ModelState.IsValid)
            {
                model.ModifiedAt = DateTime.Now;
                model.ModifiedBy = SessionContext<int>.Instance.GetSession("UserID");
                reciprocityService.UpdateReciprocity(model);
                reciprocityService.Save();
                return Redirect(returnUrl);
            }

            ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            ViewBag.Certificates = new SelectList(certificateService.GetCertificates(), "ID", "Name");
            ViewBag.PaymentTypes = paymentService.GetPaymentTypes();
            var person = CertifiedPersonService.GetCertifiedPersonByID(model.PersonID ?? model.PersonID.Value);
            string name = "";
            if (person != null)
            {
                if (!string.IsNullOrEmpty(person.FirstName))
                {
                    name += person.FirstName + " ";
                }
                if (!string.IsNullOrEmpty(person.MiddleName))
                {
                    name += person.MiddleName + " ";
                }
                if (!string.IsNullOrEmpty(person.LastName))
                {
                    name += person.LastName + " ";
                }

                //name = string.Format($"{person.FirstName != null ? person.FirstName + " ":\"\"}");
                //name = person.FirstName != null ? person.FirstName + " " + person.MiddleName != null ? person.MiddleName + " " + person.LastName != null ? person.LastName:"":"":"";
            }
            ViewBag.Person = name;
            return View(model);
        }

        // GET: Reciprocity/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Reciprocity/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                reciprocityService.Delete(id);
                reciprocityService.Save();
                return Json(true, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public void SetReturnUrl()
        {
            returnUrl = ShrdMaster.Instance.GetReturnUrl("/Reciprocities/Index");
            ViewBag.ReturnURL = returnUrl;

            ////to go to previous page
            //if (Request.QueryString["returnUrl"] != null)
            //{
            //    returnUrl = Request.QueryString["returnUrl"];
            //}

            //if (string.IsNullOrEmpty(returnUrl))
            //{
            //    returnUrl = "/Reciprocities/Index";
            //    ViewBag.ReturnURL = returnUrl;

            //}
            //else
            //{
            //    ViewBag.ReturnURL = returnUrl;
            //}
            // return returnUrl;
        }
    }
}
