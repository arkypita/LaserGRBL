//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using LaserGRBL.CSV;

namespace LaserGRBL
{
	public static class CSVD
	{
		public static CsvDictionary Settings = new CSV.CsvDictionary("LaserGRBL.CSV.setting_codes.v1.1.csv", 3);
		public static CsvDictionary Alarms = new CSV.CsvDictionary("LaserGRBL.CSV.alarm_codes.csv", 2);
		public static CsvDictionary Errors = new CSV.CsvDictionary("LaserGRBL.CSV.error_codes.csv", 2);

		public static void LoadAppropriateSettings(GrblVersionInfo value)
		{
			try
			{
				string ResourceName;
				if (value.IsOrtur)
					ResourceName = String.Format("LaserGRBL.CSV.setting_codes.ortur.v1.0.csv");
				else
					ResourceName = String.Format("LaserGRBL.CSV.setting_codes.v{0}.{1}.csv", value.Major, value.Minor);


				Settings = new CsvDictionary(ResourceName, 3);
			}
			catch { }
		}
	}
}
