﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCoreMVCOefeningenreeks2.Common;
using DotNetCoreMVCOefeningenreeks2.Entities;
using DotNetCoreMVCOefeningenreeks2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotNetCoreMVCOefeningenreeks2.Controllers
{
    //Scaffold-DbContext -Connection "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MyShopLi;Integrated Security=True" -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -Context MyShopLiContext
    // Use the extra option -f to 'force' overwriting the current entities in your project
    public class ShoppingController : Controller
    {
        private MyShopLiContext db;

        public ShoppingController(MyShopLiContext context)
        {
            db = context;
        }

        #region Index
        public IActionResult Index()
        {
            return View(db.ShopItem
                        .Select(s => s)
                        .ToList());
        }
        #endregion Index

        #region Create
        [HttpGet]
        public ViewResult Create()
        {
            ViewBag.Suggestion = (Suggestion)
                                    new Random()
                                    .Next(1, (Enum.GetValues(typeof(Suggestion)).Length) + 1);
            //List<SelectListItem> selectList = new List<SelectListItem>();
             

            ViewBag.CartId = db.Cart
                        .Select(c => new SelectListItem()
                        {
                            Text = c.Name,
                            Value = c.Id.ToString()
                        }).ToList();
            ViewBag.CategoryId = db.Category
                                  .Select(c => new SelectListItem()
                                  {
                                      Text = c.Name,
                                      Value = c.Id.ToString()
                                  }).ToList();
            return View(new ShopItem());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ShopItem shopItem)
        {
            if (ModelState.IsValid)
            {
                db.ShopItem.Add(shopItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(shopItem);
            }
        }
        #endregion Create

        #region Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                ShopItem shopItemToEdit = db.ShopItem
                        .Where(s => s.Id == id)
                        .Select(s => s)
                        .SingleOrDefault();
                if (shopItemToEdit != null)
                {
                    return View(shopItemToEdit);
                }
            } 
            return View("Error", new ErrorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ShopItem shopItem)
        {
            if (ModelState.IsValid)
            {
                db.ShopItem.Update(shopItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shopItem);
        }
        #endregion Edit

        #region Delete
        public IActionResult Delete(int? id)
        {
            if (id != null) {
                ShopItem shopItemToDelete = db.ShopItem
                      .Where(s => s.Id == id)
                      .Select(s => s)
                      .SingleOrDefault();
                if (shopItemToDelete != null)
                {
                    db.ShopItem.Remove(shopItemToDelete);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
        #endregion Delete

        #region Find
        public ViewResult Find(string item, int? aantal)
        {
            return View("Index", 
                  db.ShopItem
                  .Where(s => s.Name.StartsWith(item ?? "") && s.Quantity <= (aantal ?? byte.MaxValue))
                  .Select(s => s)
                  .ToList());
        }
        #endregion Find

        #region Private Methods
       


        #endregion
    }
}