using System;

namespace MonkeySpace
{

	public class ConfererenceParametersManager: IConferenceParametersManager	
	{
	 public ConferenceParameters Load()
		{
			var config = new ConferenceParameters() {
				ConferenceName="CodeCampSDQ", 
				DurationInDays=1, 
				StartDate=new DateTime(2012,12,1)
			};

			return config;
		}

		public bool Save(ConferenceParameters newConfig)
		{
			return true;
		}

	}
}
