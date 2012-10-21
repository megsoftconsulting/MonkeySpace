using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SQLite;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using MonkeySpace;

namespace Monospace11
{
	public class TabBarController : UITabBarController
	{
		/// <summary>
		/// One ViewController for each tab
		/// </summary>
		MonoTouch.UIKit.UINavigationController 
				  navSessionController
				, navSpeakerController
				, navScheduleController
				, navFavoritesController;

		/// <summary>
		/// Create the ViewControllers that we are going to use for the tabs:
		/// Sessions, Speakers
		/// </summary>
		/// <remarks>
		/// Some icons from glyphish.com -- CCA so DON'T FORGET attribution on the website!!
		/// </remarks>
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var dvc = new HomeViewController ();

			navScheduleController = new MonoTouch.UIKit.UINavigationController();
			navScheduleController.PushViewController(dvc, false);
			navScheduleController.NavigationBar.BarStyle = UIBarStyle.Black;
			navScheduleController.TopViewController.Title ="What's on".GetText();
			navScheduleController.TabBarItem = new UITabBarItem("What's on".GetText(), UIImage.FromFile("Images/83-calendar.png"), 0);
			
			var svc = new SpeakersViewController();
			navSpeakerController = new MonoTouch.UIKit.UINavigationController();
			navSpeakerController.PushViewController(svc, false);
			navSpeakerController.TopViewController.View.BackgroundColor = new UIColor(65.0f,169.0f,198.0f,255.0f);
			navSpeakerController.NavigationBar.BarStyle = UIBarStyle.Black;
			navSpeakerController.TopViewController.Title ="Speakers".GetText();
			navSpeakerController.TabBarItem = new UITabBarItem("Speakers".GetText(), UIImage.FromFile("Images/tabspeaker.png"), 1);
			
			var ssvc = new TagsViewController(); 
			navSessionController = new MonoTouch.UIKit.UINavigationController();
			navSessionController.PushViewController(ssvc, false);
			navSessionController.NavigationBar.BarStyle = UIBarStyle.Black;
			navSessionController.TopViewController.Title ="Sessions".GetText();
			navSessionController.TabBarItem = new UITabBarItem("Sessions".GetText(), UIImage.FromFile("Images/124-bullhorn.png"), 2);
			
			var mvc = new MapFlipViewController();
			mvc.Title = "Map".GetText();
			mvc.TabBarItem = new UITabBarItem("Map".GetText(), UIImage.FromFile("Images/103-map.png"), 5);
			
			var fvc = new FavoritesViewController();
			navFavoritesController = new MonoTouch.UIKit.UINavigationController();
			navFavoritesController.PushViewController(fvc, false);
			navFavoritesController.NavigationBar.BarStyle = UIBarStyle.Black;
			navFavoritesController.TopViewController.Title ="My Schedule".GetText();
			navFavoritesController.TabBarItem = new UITabBarItem("My Schedule".GetText(), UIImage.FromFile("Images/28-star.png"), 7);
			
			var u = new UIViewController[]
			{
				  navScheduleController
				, navSpeakerController
				, navSessionController
				, mvc
				, navFavoritesController
			};
			
			SelectedIndex = 0;
			ViewControllers = u;
			MoreNavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
			
			var backgroundColor = UIColor.FromPatternImage(UIImage.FromFile("Background.png"));
			MoreNavigationController.View.BackgroundColor = backgroundColor;
			
			CustomizableViewControllers = new UIViewController[]{};
		}
	}
}