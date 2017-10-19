using System.IO;
using AppointmentSystem.Utils;
using IniParser;
using IniParser.Model;
using IniParser.Model.Configuration;
using IniParser.Parser;

namespace AppointmentSystem.Utils
{
    public static class IniParse
    {
	    public static FileIniDataParser Parser = new FileIniDataParser();
	    public static IniData DbIniData;

		static IniParse()
	    {
		    if (File.Exists("Database.ini"))
		    {
			    DbIniData = Parser.ReadFile("Database.ini");
		    }
		    else
		    {
			    throw new BusinessException("Database.ini not found!", string.Empty);
		    }
		}

	    public static string GetConnectionString()
	    {
		    string connection = "";
		    connection += $"User Id={DbIniData["Sql"]["userId"]};";
			connection += $"Password={DbIniData["Sql"]["password"]};";
			connection += $"Server={DbIniData["Sql"]["server"]};";
			//connection += $"Trusted_Connection={DbIniData["Sql"]["trustedConnection"]};";
		    connection += $"Database={DbIniData["Sql"]["database"]};";
		    connection += $"Connection Timeout={DbIniData["Sql"]["connectionTimeout"]};";
		    return connection;
	    }
	}
}