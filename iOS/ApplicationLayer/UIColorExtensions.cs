using System;
using MonoTouch.CoreLocation;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MonoTouch.UIKit
{
	public static class UIColorExtensions
	{
		/// <summary>
		/// Created a UIColor instance by using the hex equivalent.
		/// </summary>
		public static UIColor FromHex ( this UIColor color, int hexValue )
		{
			return UIColor.FromRGB (
				( ( ( float ) ( ( hexValue & 0xFF0000 ) >> 16 ) ) / 255.0f ),
				( ( ( float ) ( ( hexValue & 0xFF00 ) >> 8 ) ) / 255.0f ),
				( ( ( float ) ( hexValue & 0xFF ) ) / 255.0f )
			);
		}
	}
	
}
