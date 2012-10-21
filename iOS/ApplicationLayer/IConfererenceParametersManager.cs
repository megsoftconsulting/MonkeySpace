using System;
using System.Collections.Generic;

namespace MonkeySpace
{
	public interface IConferenceParametersManager
	{
		ConferenceParameters Load();
		bool Save(ConferenceParameters configuration);
	}


}

