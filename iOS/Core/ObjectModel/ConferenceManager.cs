using System;
using System.Json;
using System.IO;
using System.Collections.Generic;

using System.Net;
using System.Text;

namespace MonkeySpace.Core
{
	public class ConferenceManager
	{
		public Action<string> OnDownloadSucceeded;
		public Action<string> OnDownloadFailed;
		bool isDownloading = false;

		public static ConferenceInfo ConferenceInfo;

		public static string LocalJsonDataFilename = "sessions.json";

		public static string SessionDataUrl = "http://codecampsdq.com/data/2012/Sessions.txt";

		public static Dictionary<int, Session> Sessions = new Dictionary<int, Session>();

		public static Dictionary<int, Speaker> Speakers = new Dictionary<int, Speaker>();

		public static DateTime LastUpdated { get; private set; }

		public static String LastUpdatedDisplay {get{return LastUpdated.ToString("yyyy-MM-dd HH:mm:ss");}}

		public ConferenceManager ()
		{
			//TODO: Load from File/WebService/Other repository	
			ConferenceInfo = new MonkeySpace.Core.ConferenceInfo { 
					DisplayName ="CodeCampSDQ", 
					StartDate=new DateTime(2012,12,1,8,30,0),
				EndDate = new DateTime(2012,12,1,18,0,0), 
				SessionsDataUrl = "http://codecampsdq.com/data/2012/",  // Low Tech Web Services
				ImagesUrl = "http://codecampsdq.com/data/2012/images/speakers/"
			};
		}

		public static bool LoadFromString (string jsonString)
		{
			Dictionary<int, Session> localSessions = null;
			Dictionary<int, Speaker> localSpeakers = null;

			// if parsing succeeds, use the data
			if (ParseJson (jsonString, ref localSessions, ref localSpeakers)) {
				Sessions = localSessions;
				Speakers = localSpeakers;
				Console.WriteLine ("sessions.json has been loaded");
				return true;
			}
			Console.WriteLine ("Parsing sessions.json failed");
			return false;
		}

		/// <summary>
		/// Parses the json passed in and ONLY if the parsing succeeded does it populate the ref params
		/// </summary>
		static bool ParseJson (string jsonString, ref Dictionary<int, Session> sessions, ref Dictionary<int, Speaker> speakers)
		{
			Dictionary<int, Session> localSessions = new Dictionary<int, Session>();
			Dictionary<int, Speaker> localSpeakers = new Dictionary<int, Speaker>();

			try {
				var jsonObject = JsonValue.Parse (jsonString);
			
				if (jsonObject != null) {
					for (var j = 0; j < jsonObject.Count; j++) {
						var jsonSession = jsonObject [j];// as JsonValue;
						var session = new Session (jsonSession);
					
						localSessions.Add (session.Id, session);
						Console.WriteLine ("Session: " + session.Title);
					
						try
						{
							var jsonSpeakers = jsonSession ["speakers"];// as JsonValue;
						
							for (var k = 0; k < jsonSpeakers.Count; k++) {
								var jsonSpeaker = jsonSpeakers [k]; // as JsonValue;
								var speaker = new Speaker (jsonSpeaker);
							
								if (!localSpeakers.ContainsKey (speaker.Id)) {
									localSpeakers.Add (speaker.Id, speaker);
								} else {
									speaker = localSpeakers [speaker.Id];
								}
								speaker.Sessions.Add (session);
								session.Speakers.Add (speaker);
							
								Console.WriteLine ("Speaker: " + speaker.Name);
							}
						}
						catch (Exception)
						{
							Console.WriteLine("Missing Speaker Node");
						}

					}
				}
			} catch (Exception ex) {
				// something in the parsing failed
				Console.WriteLine ("Parsing failed " + ex);
				return false;
			}
			speakers = localSpeakers;
			sessions = localSessions;
			return true;
		}

		[Obsolete("System.IO dependency breaks WP7")]
		public static void LoadFromFile ()
		{
			string xmlPath = LocalJsonDataFilename;
			var basedir = Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);

			if (File.Exists (Path.Combine (basedir, LocalJsonDataFilename))) {	// load a downloaded copy
				xmlPath = Path.Combine (basedir, LocalJsonDataFilename);
			}
		
			var jsonString = File.ReadAllText(xmlPath);

			LoadFromString(jsonString);
		}

		public void DownloadFromServer ()
		{
			Console.WriteLine ("DownloadFromServer");
			if (!isDownloading) {	//TODO: lock?
				Console.WriteLine ("start downloading");
				isDownloading = true;
				var client = new WebClient ();
			
				client.DownloadDataCompleted += DownloadCompleted;

				client.DownloadDataAsync (new Uri (SessionDataUrl));
			}
			else 
				Console.WriteLine ("Already downloading");
		}
		private void DownloadCompleted (object sender, DownloadDataCompletedEventArgs e)
		{
			if (e.Error != null || e.Result == null) {
				Console.WriteLine("Nothing downloaded from " + SessionDataUrl); 
				if (e.Error != null)
					Console.WriteLine("Error was: " + e.Error.Message); 
				if (OnDownloadFailed != null)
					OnDownloadFailed("Download error.");
			} else {
				string jsonString = Encoding.UTF8.GetString(e.Result);;

				if (LoadFromString(jsonString)) {
					LastUpdated = DateTime.Now;

					if (OnDownloadSucceeded != null)
						OnDownloadSucceeded(jsonString);
				} else {
					if (OnDownloadFailed != null)
						OnDownloadFailed("Parsing error.");
				}
			}
			isDownloading = false;
		}
	}
}

