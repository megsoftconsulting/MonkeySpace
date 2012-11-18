using System;

namespace MonkeySpace.Core
{
	[System.Diagnostics.DebuggerDisplay("MapLocation {Title} {Location}")]
   public class MapLocation 
   {
      
      public string Title {get;set;}
      public string Subtitle {get;set;}
      public Point Location { get; set; }
   }
	public class Point
		   {
		      public Point () {}
		      public Point (double x, double y) 
		      {
		         X = x;
		         Y = y;
		      }
			///<summary>Latitude</summary>
			public double X { get; set; }
			
			///<summary>Longitude</summary>
			public double Y { get; set; }
		   }
}

