using System;
using MonoTouch.Foundation;

namespace MonkeySpace
{
	public static class LocalizationExtensions
	{
		/// <summary>
		/// Gets the localized text for the specified string.
		/// </summary>
		public static string GetText (this string text)
		{
			if (String.IsNullOrEmpty (text))
				return text;
			return NSBundle.MainBundle.LocalizedString (text, String.Empty, String.Empty);
		}
	}
}

