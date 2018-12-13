using Appick3._0._1.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.Linq;
using System.Web.Mvc;


namespace Appick3._0._1.Controllers
{

	public class HomeController : Controller
	{
		private AppickDatabaseEntities1 db = new AppickDatabaseEntities1();
            public ActionResult Index()
            {

                var apps = (from x in db.Apps
                            select x).ToList();



                List<App> appse = new List<App>();
                for (int i = 0; i < appse.Count(); i++)
                {

                    appse.Add(apps[i]);

                }
                List<App> fetapp = new List<App>();
                for (int i = 0; i < 5; i++)
                {

                    fetapp.Add(apps[i]);

                }
                //ye query kuch b nai return karti? in do me difference?aik mai featured apss aikmai saari apps aik mai top apps... kiuna different displays tha maina 3 lists bna lein
                var apps2 = (from x in db.Apps
                             orderby x.Downloads descending
                             select x).ToList();
                List<App> topapp = new List<App>();
                for (int i = 0; i < 5; i++)
                {

                     topapp.Add(apps2[i]);

                }
            
                //ajeeb hai karti hai magar pta nae sorted kiun nae 0 1 walay tou anay hi nae chahyein...  sai nai hai? nae ye dekho
                var ap_list = new App();
                ap_list.all_apps = appse;
                ap_list.featured_apps = fetapp;

                ap_list.top_apps = topapp;

                return View(ap_list);



            }

            public ViewResult AllApps()
            {
                var apps = (from x in db.Apps
                            select x).ToList();
                List<App> appse = new List<App>();
                for (int i = 0; i < apps.Count(); i++)
                {

                    appse.Add(apps[i]);

                }
                return View(appse);
            }

		public ViewResult Search()
		{

			string a= Request["key"];
			var apps = (from x in db.Apps
						select x).ToList();
			List<App> appse = new List<App>();
				
			
			for (int i = 0; i < apps.Count(); i++)
			{
				if (apps[i].Name.Contains(a))
				{
					appse.Add(apps[i]);

					
				}

			}
			if (appse.Count == 0)
			{
				appse.Add(apps[0]);

				appse[0].Name = "NotFound";
			}
			return View(appse);
		}



        public ActionResult Contact()
		{
			//ViewBag.Message = "Your application description page.";

			return View();
		}

		public ViewResult Policies()
		{
			return View();
		}

		public ViewResult car()
		{
			return View();
		}

		public ViewResult mobile()
		{
			return View();
		}

		public ViewResult Category()
		{

			return View(Request["cat"]);
		}


		public ActionResult preview()
		{
			//ViewBag.Message = "Your contact page.";

			return View();
		}
		public ActionResult Login()
		{
			//ViewBag.Message = "Your contact page.";

			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]

		public ActionResult Login(User reg)
		{
			if (ModelState.IsValid)
			{
				var user_detail = (from User in db.Users
								   where User.Username == reg.Username && User.Password == reg.Password
								   select User).ToList();
				if (user_detail.Count != 0)
				{
					Session["UserId"] = user_detail[0].UID;
					Session["UserName"] = user_detail[0].Username;


					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", "Invalid credentials User not registered");
				}
				return View(reg);

			}
			return View();
		}
		public ActionResult Register()
		{

			return View();

		}
		[HttpPost]
		[ValidateAntiForgeryToken]

		public ActionResult Register(User reg)
		{
			if (ModelState.IsValid)
			{

				if (db.Users.Any(X => X.Username == reg.Username) || (db.Users.Any(X => X.Email == reg.Email)) || (reg.confirm_Password != reg.Password)) // check if user with same username already exist in db
				{
					ModelState.AddModelError("", "Invalid username or email or password");
				}
                else if(reg.Email==null || reg.Username==null ||reg.Password==null || reg.confirm_Password==null )
                {
                    ModelState.AddModelError("", "dont leave blank field");
                }
                else
				{
					db.Users.Add(reg);
					db.SaveChanges();
					Session["UserId"] = reg.UID;
					Session["UserName"] = reg.Username;
					return RedirectToAction("Login", "Home");
				}

			}
			return View();
		}
		public ViewResult app_description(int id) // actionresult is to return to html page
		{



			var q = from x in db.Apps
					where x.AppID.Equals(id)
					select x;



			return View(q);
		}


		public ActionResult Wishlist()
		{
			string a = Session["UserName"].ToString();

			var user = from x in db.Users
					   where x.Username.Equals(a)
					   select x;

			var wisshes = user.FirstOrDefault().Wishlists.ToList();


			var Allapps = (from x in db.Apps
						   select x).ToList();




			var Userwishes = (from x in db.Wishlists
							  where x.UID == user.FirstOrDefault().UID
							  select x).ToList();



			List<App> ToReturn = new List<App>();

			for (int i = 0; i < Userwishes.Count; i++)
			{
				bool found = false;
				for (int j = 0; j < Allapps.Count && !found; j++)
				{
					if (Allapps[j].AppID == Userwishes[i].AppID)
					{
						ToReturn.Add(Allapps[j]);
						found = true;

					}
				}
			}


			return View(ToReturn);
			//String a = (string)Session["docName"];


		}

		public ActionResult Add_to_wishlist(int id)
		{
			string a = Session["UserName"].ToString();
			var user = from x in db.Users
					   where x.Username.Equals(a)
					   select x;

			if (db.Wishlists.Any(X => X.UID == user.FirstOrDefault().UID) && (db.Wishlists.Any(X => X.AppID == id)))
			{
				ModelState.AddModelError("", "Already in wishlist");

			}
			else
			{
				var w_list = new Wishlist
				{
					AppID = id,
					UID = user.FirstOrDefault().UID,
				};
				db.Wishlists.Add(w_list);
				db.SaveChanges();
				return RedirectToAction("Wishlist", "Home");
			}

			return RedirectToAction("Index", "Home");

		}



		public ViewResult Likes()
		{
			string a = Session["UserName"].ToString();

			var user = from x in db.Users
					   where x.Username.Equals(a)
					   select x;

			//var wisshes = user.FirstOrDefault().Wishlists.ToList();


			var Allapps = (from x in db.Apps select x).ToList();




			var Userlikes = (from x in db.Likes
							 where x.UID == user.FirstOrDefault().UID
							 select x).ToList();



			List<App> ToReturn = new List<App>();

			for (int i = 0; i < Userlikes.Count; i++)
			{
				bool found = false;
				for (int j = 0; j < Allapps.Count && !found; j++)
				{
					if (Allapps[j].AppID == Userlikes[i].AppID)
					{
						ToReturn.Add(Allapps[j]);
						found = true;

					}
				}
			}


			return View(ToReturn);
			//String a = (string)Session["docName"];
		}

		public ActionResult like_count(int id)
		{
			string a = Session["UserName"].ToString();
			var user = from x in db.Users
					   where x.Username.Equals(a)
					   select x;
			var Allapps = (from x in db.Apps
						   select x).ToList();




			if (db.Likes.Any(X => X.UID == user.FirstOrDefault().UID) && (db.Likes.Any(X => X.AppID == id)))
			{
				ModelState.AddModelError("", "Already in installed");

			}
			else
			{
				var i_list = new Like
				{
					AppID = id,
					UID = user.FirstOrDefault().UID,
				};
				db.Likes.Add(i_list);
				db.SaveChanges();


				List<Like> ToReturn = new List<Like>();
				var like = (from x in db.Likes
							where x.AppID == id
							select x).ToList();

				for (int i = 0; i < like.Count; i++)
				{

					{
						if (like[i].AppID == id)
						{
							ToReturn.Add(like[i]);


						}
					}
				}

				var result = db.Apps.SingleOrDefault(b => b.AppID == id);
				if (result != null)
				{
					result.Likes1 = like.Count().ToString();
					db.SaveChanges();
				}

				return RedirectToAction("Index", "Home");
			}

			return RedirectToAction("Index", "Home");
		}

		public ViewResult Dislikes()
		{
			string a = Session["UserName"].ToString();

			var user = from x in db.Users
					   where x.Username.Equals(a)
					   select x;

			//var wisshes = user.FirstOrDefault().Wishlists.ToList();


			var Allapps = (from x in db.Apps select x).ToList();




			var UserDislikes = (from x in db.Dislikes
								where x.UID == user.FirstOrDefault().UID
								select x).ToList();



			List<App> ToReturn = new List<App>();

			for (int i = 0; i < UserDislikes.Count; i++)
			{
				bool found = false;
				for (int j = 0; j < Allapps.Count && !found; j++)
				{
					if (Allapps[j].AppID == UserDislikes[i].AppID)
					{
						ToReturn.Add(Allapps[j]);
						found = true;

					}
				}
			}


			return View(ToReturn);


		}

		public ActionResult Dislike_count(int id)
		{
			string a = Session["UserName"].ToString();
			var user = from x in db.Users
					   where x.Username.Equals(a)
					   select x;
			var Allapps = (from x in db.Apps
						   select x).ToList();




			if (db.Dislikes.Any(X => X.UID == user.FirstOrDefault().UID) && (db.Dislikes.Any(X => X.AppID == id)))
			{
				ModelState.AddModelError("", "Already in installed");

			}
			else
			{
				var i_list = new Dislike
				{
					AppID = id,
					UID = user.FirstOrDefault().UID,
				};
				db.Dislikes.Add(i_list);
				db.SaveChanges();


				List<Dislike> ToReturn = new List<Dislike>();
				var dislike = (from x in db.Dislikes
							   where x.AppID == id
							   select x).ToList();

				for (int i = 0; i < dislike.Count; i++)
				{

					{
						if (dislike[i].AppID == id)
						{
							ToReturn.Add(dislike[i]);


						}
					}
				}

				var result = db.Apps.SingleOrDefault(b => b.AppID == id);
				if (result != null)
				{
					result.Dislikes1 = dislike.Count().ToString();
					db.SaveChanges();
				}

				return RedirectToAction("Index", "Home");
			}

			return RedirectToAction("Index", "Home");
		}




		public ViewResult Installed()
		{
			string a = Session["UserName"].ToString();

			var user = from x in db.Users
					   where x.Username.Equals(a)
					   select x;

			//var wisshes = user.FirstOrDefault().Wishlists.ToList();


			var Allapps = (from x in db.Apps
						   select x).ToList();




			var UserInstalled = (from x in db.Installeds
								 where x.UID == user.FirstOrDefault().UID
								 select x).ToList();



			List<App> ToReturn = new List<App>();

			for (int i = 0; i < UserInstalled.Count; i++)
			{
				bool found = false;
				for (int j = 0; j < Allapps.Count && !found; j++)
				{
					if (Allapps[j].AppID == UserInstalled[i].AppID)
					{
						ToReturn.Add(Allapps[j]);
						found = true;

					}
				}
			}


			return View(ToReturn);


		}

		public ActionResult Download_count(int id)
		{
			string a = Session["UserName"].ToString();
			var user = from x in db.Users
					   where x.Username.Equals(a)
					   select x;
			var Allapps = (from x in db.Apps
						   select x).ToList();




			if (db.Installeds.Any(X => X.UID == user.FirstOrDefault().UID) && (db.Installeds.Any(X => X.AppID == id)))
			{
				ModelState.AddModelError("", "Already in installed");

			}
			else
			{
				var i_list = new Installed
				{
					AppID = id,
					UID = user.FirstOrDefault().UID,
				};
				db.Installeds.Add(i_list);
				db.SaveChanges();


				List<Installed> ToReturn = new List<Installed>();
				var install = (from x in db.Installeds
							   where x.AppID == id
							   select x).ToList();

				for (int i = 0; i < install.Count; i++)
				{

					{
						if (install[i].AppID == id)
						{
							ToReturn.Add(install[i]);


						}
					}
				}

				var result = db.Apps.SingleOrDefault(b => b.AppID == id);
				if (result != null)
				{
					result.Downloads = install.Count().ToString();
					db.SaveChanges();
				}

				return RedirectToAction("Index", "Home");
			}

			return RedirectToAction("Index", "Home");
		}

		public ActionResult Report_count(int id)
		{
			string a = Session["UserName"].ToString();
			var user = from x in db.Users
					   where x.Username.Equals(a)
					   select x;
			var Allapps = (from x in db.Apps
						   select x).ToList();




			if (db.Reports.Any(X => X.UID == user.FirstOrDefault().UID) && (db.Reports.Any(X => X.AppID == id)))
			{
				ModelState.AddModelError("", "Already in installed");

			}
			else
			{
				var i_list = new Report
				{
					AppID = id,
					UID = user.FirstOrDefault().UID,
				};
				db.Reports.Add(i_list);
				db.SaveChanges();


				List<Report> ToReturn = new List<Report>();
				var reports = (from x in db.Reports
							   where x.AppID == id
							   select x).ToList();

				for (int i = 0; i < reports.Count; i++)
				{

					{
						if (reports[i].AppID == id)
						{
							ToReturn.Add(reports[i]);


						}
					}
				}

				var result = db.Apps.SingleOrDefault(b => b.AppID == id);
				if (result != null)
				{
					result.ReportCount = reports.Count().ToString();
					db.SaveChanges();
				}

				return RedirectToAction("Index", "Home");
			}

			return RedirectToAction("Index", "Home");
		}


		public ActionResult logout()
		{
			Session.Contents.RemoveAll();


			return RedirectToAction("Index", "Home");
		}

		public ActionResult Rating(int Value)
		{
			return RedirectToAction("Index", "Home");
		}


	}

}
																		   