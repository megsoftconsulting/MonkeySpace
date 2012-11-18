using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Diagnostics;
using MonkeySpace;

namespace Monospace11
{
    [Register]
    public class FavoritesViewController : DialogViewController
    {
		public FavoritesViewController () : base (null)
		{
		}
		
		public override void ViewWillAppear (bool animated)
		{
			Root = GenerateRoot ();
			
			Debug.WriteLine ("Summary " + Root.Summary() );
		}
		
		RootElement GenerateRoot ()
		{
			var favs = AppDelegate.UserData.GetFavoriteCodes();
			var root = 	new RootElement ("Favorites".GetText()) {
				from s in MonkeySpace.Core.ConferenceManager.Sessions.Values.ToList () //AppDelegate.ConferenceData.Sessions
							where favs.Contains(s.Code )
							group s by s.Start.Ticks into g
							orderby g.Key
							select new Section (HomeViewController.MakeCaption ("", new DateTime (g.Key))) {
							from hs in g
							   select (Element) new SessionElement (hs)
			}};	
			
			if(favs.Count == 0)
			{

				/**
				 * var logoView = new UIImageView(UIImage.FromFile("100x100_icon.png"));
				logoView.Alpha = .5f;
				logoView.Frame = new RectangleF(0,42,320,100);
				NavigationController.NavigationBar.Layer.AddSublayer(logoView.Layer);
		**/

				var section = new Section("Whoops, Star a few sessions first!".GetText());
				root.Add(section);




			}
			return root;
        }
    }
}
