﻿using IC_RC.Models;
using ICRC.Model;
using ICRCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IC_RC.Controllers
{
    [Authorize]        
    public class CertifiedPersonsController : Controller
    {
        public readonly ICertifiedPersonService CertifiedPersonService;
        public readonly ICertificationService CertificationService;
        public readonly IReciprocitiesService ReciprocityService;
        public readonly IEthicalViolationService violationservice;
        public readonly IBoardService BoardService;
        public readonly IScoreservice scoreService;
        string returnUrl;
        public readonly IStudentEthicalViolationService Stundentethicalviolationservice;
        

        public CertifiedPersonsController(ICertifiedPersonService CertifiedPersonService, ICertificationService CertificationService, IReciprocitiesService ReciprocityService, IEthicalViolationService violationservice, IBoardService BoardService, IScoreservice scoreService, IStudentEthicalViolationService Stundentethicalviolationservice)
        {
            this.CertifiedPersonService = CertifiedPersonService;
            this.CertificationService = CertificationService;
            this.ReciprocityService = ReciprocityService;
            this.violationservice = violationservice;
            this.BoardService = BoardService;
            this.scoreService = scoreService;
            this.Stundentethicalviolationservice = Stundentethicalviolationservice;
        }

        public ActionResult Index()
        {
            var user = ShrdMaster.Instance.GetUser(User.Identity.Name);
            if (user == null)
            {
                ViewBag.Error = "User is not active.";
                return Redirect("/Account/login");
            }
            return View();
        }

        public ActionResult GetData()
        {
            //pageIndex = ShrdMaster.Instance.GetPageIndex();
            //var data1 = CertifiedPersonService.GetCertifedPersonsForIndex(pageIndex).AsQueryable();
            //var data =new CertifiedPersonsGrid(data1,1,true);
            List<CertifiedPersons> data=new List<CertifiedPersons> ();
            if(ShrdMaster.Instance.IsAdmin(User.Identity.Name))
            {
                data= CertifiedPersonService.GetCertifiedPersons().ToList();
            }
            else
            {
                var user = ShrdMaster.Instance.GetUser(User.Identity.Name);
                //if(user==null)
                //{
                //    ViewBag.Error = "User is not active.";
                //    return RedirectToAction("Account/login");
                //}
                data = CertifiedPersonService.GetCertifiedPersonsByBoardId(user.BoardID).ToList();
            }

            
            return PartialView("_CertifiedPersons", data);
        }




        // GET: CertifiedPersons/Details/5
        public ActionResult Details(int? id)
        {
            SetReturnUrl();
            if(id==null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }

            var data = CertifiedPersonService.GetCertifiedPersonByID(id.Value);
            if(data==null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }

            if(data!=null && data.CurrentBoardID>0)
            {
                ViewBag.CurrentBoard = BoardService.GetBoardByID(data.CurrentBoardID).Acronym;
            }
            
            return View(data);
        }


        public ActionResult Scores(int? ID)
        {
            
            if (ID==null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }
            var data = scoreService.ScoresGetByPersonID(ID.Value);
            return PartialView("_Scores",data);

        }

        public ActionResult Certificates(int? ID)
        {
            if (ID == null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }

            var data = CertificationService.GetCertificationsByPersonID(ID.Value);
            return PartialView("_Certifications", data);
        }

        public ActionResult Ethicalviolations(int? ID)
        {
            if (ID == null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }
            var data = Stundentethicalviolationservice.GetVoiltaionsByPersonID(ID.Value).ToList();
            return PartialView("_violations", data);
        }


        public ActionResult Reciprocities(int? ID)
        {
            if (ID == null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }
            var data = ReciprocityService.ReciprocityGetByPersonID(ID.Value).ToList();
            return PartialView("_Reciprocities", data);
        }


        // GET: CertifiedPersons/Create
        public ActionResult Create()
        {
            SetReturnUrl();

            ViewBag.CurrentBoardID = new SelectList(BoardService.GetBoards(), "ID", "Board");
            
            return View();
        }

        // POST: CertifiedPersons/Create
        [HttpPost]
        public ActionResult Create(CertifiedPersons person)
        {
            SetReturnUrl();

            // TODO: Add insert logic here
            if (ModelState.IsValid)
                {
                person.CreatedAt = DateTime.Now;
                person.CreatedBy = SessionContext<int>.Instance.GetSession("UserID");
                CertifiedPersonService.CreateCertifiedPerson(person);
                    CertifiedPersonService.Save();
                //db.SaveChanges();
                    return Redirect(returnUrl);
                }

                ViewBag.CurrentBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
                ViewBag.OtherBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
                return View(person);
                            
        }

        // GET: CertifiedPersons/Edit/5
        public ActionResult Edit(int id)
        {
            SetReturnUrl();
            var data = CertifiedPersonService.GetCertifiedPersonByID(id);

            ViewBag.CurrentBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym",data.CurrentBoardID);
            ViewBag.OtherBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym",data.OtherBoardID);
            //Certifications


            return View(data);
        }


        [HttpPost]
        public ActionResult Edit(CertifiedPersons person)
        {
            SetReturnUrl();
            // TODO: Add insert logic here
            if (ModelState.IsValid)
            {
                person.ModifiedAt = DateTime.Now;
                person.ModifiedBy = SessionContext<int>.Instance.GetSession("UserID");
                CertifiedPersonService.UpdateCertifiedPerson(person);
                CertifiedPersonService.Save();
                //db.SaveChanges();
                return Redirect(returnUrl);
            }

            ViewBag.CurrentBoardID = new SelectList(BoardService.GetBoards(), "ID", "Name");
            ViewBag.OtherBoardID = new SelectList(BoardService.GetBoards(), "ID", "Name");
            return View(person);
        }

        //[ChildActionOnly]
        //public ActionResult Certifications(int ID)
        //{

        //    var certifications = CertificationService.GetCertificationsByPersonID(ID);

        //    return PartialView("_Certifications",certifications);
        //}

        //[ChildActionOnly]
        //public ActionResult violations(int ID)
        //{
        //    var violations = Stundentethicalviolationservice.GetVoiltaionsByPersonID(ID);

        //    return PartialView("_violations",violations);

        //}

        //[ChildActionOnly]
        //public ActionResult Reciprocity(int ID)
        //{

        //    var data = ReciprocityService.ReciprocityGetByPersonID(ID);           
        //    return PartialView("_Recprocities",data);
        //}





        //// POST: CertifiedPersons/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        ViewBag.CurrentBoardID = new SelectList(BoardService.GetBoards(), "ID", "Name");
        //        ViewBag.OtherBoardID = new SelectList(BoardService.GetBoards(), "ID", "Name");
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: CertifiedPersons/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: CertifiedPersons/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                CertifiedPersonService.Delete(id);
                CertifiedPersonService.Save();
                return Json(true,JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }

        public void SetReturnUrl()
        {
            returnUrl = ShrdMaster.Instance.GetReturnUrl("/CertifiedPersons/Index");
            ViewBag.ReturnURL = returnUrl;
            
        }
    }
}
