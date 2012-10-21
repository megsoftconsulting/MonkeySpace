using System;
using System.Collections.Generic;

namespace MonkeySpace
{

	public class ConferenceParameters
    {    
		public String ConferenceName {get;set;}
		public int DurationInDays {get;set;}
		public DateTime StartDate {get;set;}
		public IList<String> Tracks {get;set;}

    } 

}
