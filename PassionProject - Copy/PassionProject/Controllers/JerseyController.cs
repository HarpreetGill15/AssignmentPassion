﻿using PassionProject.Data;
using PassionProject.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PassionProject.Controllers
{
    public class JerseyController : Controller
    {
        private ShopContext db = new ShopContext();
        //Jersey/List
        public ActionResult List()
        {
            List<Jersey> jerseys = db.Jerseys.SqlQuery("Select * from Jerseys").ToList();
            return View(jerseys);
        }
        //Add search for jersey later

        //Jersey/Show/int
        public ActionResult Show(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Jersey jersey = db.Jerseys.SqlQuery("select * from Jerseys where jerseyId = @JerseyId", new SqlParameter("@JerseyId",id)).FirstOrDefault();
            if (jersey == null)
            {
                return HttpNotFound();
            }
            return View(jersey);
        }
        //Customer/Add
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(string jerseyName,string jerseySize,string jerseyDescription, double jerseyPrice, int jerseyStock)
        {
            string query = "insert into jerseys (jerseyName, jerseySize, jerseyDescription,jerseyPrice,jerseyStock) values (@name,@size,@description,@price,@stock)";
            SqlParameter[] sqlparms = new SqlParameter[5];

            sqlparms[0] = new SqlParameter("@name", jerseyName);
            sqlparms[1] = new SqlParameter("@size", jerseySize);
            sqlparms[2] = new SqlParameter("@description", jerseyDescription);
            sqlparms[3] = new SqlParameter("@price", jerseyPrice);
            sqlparms[4] = new SqlParameter("@stock", jerseyStock);

            db.Database.ExecuteSqlCommand(query, sqlparms);

            return RedirectToAction("List");

        }
        //Customer/Update/int
        public ActionResult Update(int id)
        {
            //show the jerseys information in the text boxes
            Jersey jersey = db.Jerseys.SqlQuery("select * from Jerseys where jerseyId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            return View(jersey);
        }
        [HttpPost]
        public ActionResult Update(int id,string jerseyName,string jerseySize,string jerseyDescription, double jerseyPrice, int jerseyStock)
        {

            Debug.WriteLine("I am trying to edit a jerseys's name to "+jerseyName+
                " the size to "+jerseySize+" the description to "+jerseyDescription+" price to "+jerseyPrice+
                " the stock to "+jerseyStock+" for the jersey with the id of "+id);

            string query = "update Jerseys set jerseyName=@name, jerseySize=@size, jerseyDescription=@description, jerseyPrice=@price, jerseyStock=@stock where jerseyId=@id";
            SqlParameter[] sqlparams = new SqlParameter[6];
            sqlparams[0] = new SqlParameter("@name", jerseyName);
            sqlparams[1] = new SqlParameter("@size", jerseySize);
            sqlparams[2] = new SqlParameter("@description", jerseyDescription);
            sqlparams[3] = new SqlParameter("@price", jerseyPrice);
            sqlparams[4] = new SqlParameter("@stock", jerseyStock);
            sqlparams[5] = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);
            
            return RedirectToAction("List");
        }
        //Jersey/DeleteJersey/int
        public ActionResult DeleteJersey(int id)
        {
            Jersey jersey = db.Jerseys.SqlQuery("select * from Jerseys where jerseyId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            return View(jersey);
        }
        //Delete jersey after form submit
        public ActionResult Delete(int id)
        {
            Debug.WriteLine("I am trying to delete jersey with the id of "+id);
            string query = "delete from Jerseys where jerseyId= @id";
            SqlParameter parameter = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, parameter);

            return RedirectToAction("List");
        }

    }
}