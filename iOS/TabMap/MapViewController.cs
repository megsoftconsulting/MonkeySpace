using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MapKit;  // required
using MonoTouch.CoreLocation;  // required
using System.Diagnostics;
using MonkeySpace;

using MonkeySpace.Core;

namespace Monospace11
{
	public class MapViewController : UIViewController
	{
		private MKMapView mapView;
		public UILabel labelDistance;
		
		public CLLocationCoordinate2D ConferenceLocation;
		private CLLocationManager locationManager;

		private MapFlipViewController _mfvc;

		public MapViewController (MapFlipViewController mfvc):base()
		{
			_mfvc = mfvc;
			var l = new MonkeySpace.Core.MapLocation() {
				Title="MonkeySpace".GetText(), 
				Location=new MonkeySpace.Core.Point{Y=18.487918,X=-69.961857}};  //18.487918,-69.961857
			ConferenceLocation = l.Location.To2D();
		}
		
		public void SetLocation (MonkeySpace.Core.MapLocation toLocation)
		{
			var targetLocation = toLocation.Location.To2D();
			if (toLocation.Location.X == 0 && toLocation.Location.Y == 0)
			{
				// use the 'location manager' current coordinate
				if (locationManager.Location == null)
				{
					return;
				}
				{	// catch a possible null reference that i saw once [CD]
					targetLocation = locationManager.Location.Coordinate;
					ConferenceAnnotation a = new ConferenceAnnotation(targetLocation, "My location".GetText(), "");
					mapView.AddAnnotationObject(a); 
				}
			}
			else if (toLocation.Title == "MonkeySpace")
			{
				// no need to drop anything
			}
			else
			{
				// drop a new pin
				ConferenceAnnotation a = new ConferenceAnnotation(toLocation.Location.To2D(), toLocation.Title,toLocation.Subtitle);
				mapView.AddAnnotationObject(a); 
			}
			mapView.CenterCoordinate = targetLocation;
		}
		
		public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			mapView = new MKMapView()
			{
				ShowsUserLocation = true
			};
			
			labelDistance = new UILabel()
			{
				Frame = new RectangleF (0, 0, 320, 49),
				Lines = 2,
				BackgroundColor = UIColor.Black,
				TextColor = UIColor.White
			};

			var segmentedControl = new UISegmentedControl();
			var topOfSegement = View.Frame.Height - 120;
			segmentedControl.Frame = new RectangleF(20, topOfSegement, 282, 30);
			segmentedControl.InsertSegment("Map".GetText(), 0, false);
			segmentedControl.InsertSegment("Satellite".GetText(), 1, false);
			segmentedControl.InsertSegment("Hybrid".GetText(), 2, false);
			segmentedControl.SelectedSegment = 0;
			segmentedControl.ControlStyle = UISegmentedControlStyle.Bar;
			segmentedControl.TintColor = UIColor.DarkGray;
			
			segmentedControl.ValueChanged += delegate {
				if (segmentedControl.SelectedSegment == 0)
					mapView.MapType = MonoTouch.MapKit.MKMapType.Standard;
				else if (segmentedControl.SelectedSegment == 1)
					mapView.MapType = MonoTouch.MapKit.MKMapType.Satellite;
				else if (segmentedControl.SelectedSegment == 2)
					mapView.MapType = MonoTouch.MapKit.MKMapType.Hybrid;
			};
			
			mapView.Delegate = new MapViewDelegate(this); 

			// Set the web view to fit the width of the app.
            mapView.SizeToFit();

            // Reposition and resize the receiver
            mapView.Frame = new RectangleF (0, 50, View.Frame.Width, View.Frame.Height - 100);

			MKCoordinateSpan span = new MKCoordinateSpan(0.01,0.01);
			MKCoordinateRegion region = new MKCoordinateRegion(ConferenceLocation,span);
			mapView.SetRegion(region, true);
			
			ConferenceAnnotation a = new ConferenceAnnotation(ConferenceLocation
                                , "CodeCampSDQ"
                                , "INTEC"
                              );
			mapView.AddAnnotationObject(a); 
			
			
			locationManager = new CLLocationManager();
			locationManager.Delegate = new LocationManagerDelegate(mapView, this);
			locationManager.StartUpdatingLocation();
			
            // Add the table view as a subview
            View.AddSubview(mapView);
			View.AddSubview(labelDistance);
			View.AddSubview(segmentedControl);
			
			// Add the 'info' button to flip
			var flipButton = UIButton.FromType(UIButtonType.InfoLight);
			flipButton.Frame = new RectangleF(290,17,20,20);
			flipButton.Title (UIControlState.Normal);
			flipButton.TouchDown += delegate {
				_mfvc.Flip();
			};
			View.AddSubview(flipButton);
		}	
		
		public class MapViewDelegate : MKMapViewDelegate
		{
			public MapViewDelegate (MapViewController controller):base()
			{
			}

			public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, NSObject annotation)
			{
				try
				{
					var ca = (ConferenceAnnotation)annotation;
					var aview = (MKPinAnnotationView)mapView.DequeueReusableAnnotation("pin");
					if (aview == null)
					{
						aview = new MKPinAnnotationView(ca, "pin");
					}
					else 
					{
						aview.Annotation = ca;
					}
					aview.AnimatesDrop = true;
					aview.PinColor = MKPinAnnotationColor.Purple;
					aview.CanShowCallout = true;

//					UIButton rightCallout = UIButton.FromType(UIButtonType.DetailDisclosure);
//					rightCallout.Frame = new RectangleF(250,8f,25f,25f);
//					rightCallout.TouchDown += delegate 
//					{
//						NSUrl url = new NSUrl("http://maps.google.com/maps?q=" + ca.Coordinate.ToLL()  );
//						UIApplication.SharedApplication.OpenUrl(url);
//					};
//					aview.RightCalloutAccessoryView = rightCallout;

					return aview;
				} catch (Exception)
				{
					return null;
				}
			}
		}
		/// <summary>
		/// MonoTouch definition seemed to work without too much trouble
		/// </summary>
		private class LocationManagerDelegate: CLLocationManagerDelegate
		{
			private MapViewController _appd;

			public LocationManagerDelegate(MKMapView mapview, MapViewController mapvc):base()
			{
				_appd = mapvc;
			}
			/// <summary>
			/// Whenever the GPS sends a new location, update text in label
			/// and increment the 'count' of updates AND reset the map to that location 
			/// </summary>
			public override void UpdatedLocation (CLLocationManager manager, CLLocation newLocation, CLLocation oldLocation)
			{
				//MKCoordinateSpan span = new MKCoordinateSpan(0.2,0.2);
				//MKCoordinateRegion region = new MKCoordinateRegion(newLocation.Coordinate,span);
				//_appd.mylocation = newLocation;
				//_mapview.SetRegion(region, true);
				double distanceToConference = MapHelper.Distance (new Coordinate(_appd.ConferenceLocation), new Coordinate(newLocation.Coordinate), UnitsOfLength.Miles); //TODO: Make this Configurable
				string distanceMessage  = "XUnitsFromConference".GetText();
				_appd.labelDistance.TextAlignment = UITextAlignment.Center;
				_appd.labelDistance.Text = String.Format(distanceMessage, Math.Round(distanceToConference,0));
				Debug.WriteLine("Distance: {0}", distanceToConference);
				
				// only use the first result
				manager.StopUpdatingLocation();
			}
			public override void Failed (CLLocationManager manager, NSError error)
			{
				Debug.WriteLine("Failed to find location");
			}
		}
	}
	
	
	/// <summary>
	/// MKAnnotation is an abstract class (in Objective C I think it's a protocol).
	/// Therefore we must create our own implementation of it. Since all the properties
	/// are read-only, we have to pass them in via a constructor.
	/// </summary>
	public class ConferenceAnnotation : MKAnnotation
	{
		private CLLocationCoordinate2D _coordinate;
		private string _title, _subtitle;
		public override CLLocationCoordinate2D Coordinate {
			get {
				return _coordinate;
			}
			set {
				_coordinate = value;
			}
		}
		public override string Title {
			get {
				return _title;
			}
		}
		public override string Subtitle {
			get {
				return _subtitle;
			}
		}
		/// <summary>
		/// custom constructor
		/// </summary>
		public ConferenceAnnotation (CLLocationCoordinate2D coord, string t, string s) : base()
		{
			_coordinate=coord;
		 	_title=t; 
			_subtitle=s;
		}
	}
}
