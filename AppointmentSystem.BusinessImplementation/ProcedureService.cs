using System.Collections.Generic;
using AppointmentSystem.BusinessContracts;
using AppointmentSystem.Domain;
using FluentValidation.Results;
using System.Data.SqlClient;
using System.IO;
using AppointmentSystem.Utils;

namespace AppointmentSystem.BusinessImplementation
{
    public class ProcedureService : IProcedureService
    {
		public SqlConnection Connection { get; set; }

	    public ProcedureService()
	    {
		    Connection = new SqlConnection(IniParse.GetConnectionString());
		}

	    public string CreateProcedure(Procedure procedure)
	    {
		    ProcedureValidator validator = new ProcedureValidator();
		    ValidationResult results = validator.Validate(procedure);

		    bool validationSucceded = results.IsValid; 

		    if (validationSucceded)
		    {
			    try
			    {
				    Connection.Open();
			    }
			    catch (SqlException e)
			    {
				    throw new BusinessException("Connection to database failed!", e);
			    }

				var writeCommand = new SqlCommand("INSERT INTO procedures (name, duration) " +
					$"Values ('{procedure.Name}', '{procedure.Duration.TotalMinutes}')", Connection);
			    writeCommand.ExecuteNonQuery();
			    try
			    {
				    var readCommand = new SqlCommand("SELECT * FROM procedures " +
						$"WHERE name='{procedure.Name}' " +
						$"AND duration='{procedure.Duration.TotalMinutes}'",
					    Connection);

				    var reader = readCommand.ExecuteReader();
				    reader.Read();
				    return reader["ID"].ToString();
			    }
			    catch (SqlException e)
			    {
				    throw new BusinessException("Cannot save procedure!", e);
			    }
		    }
		    else
		    {
				throw new BusinessException("Cannot create procedure!", results.Errors);
			}
		}

	    public bool UpdateProcedure(Procedure procedure)
	    {
		    try
		    {
			    Connection.Open();
		    }
		    catch (SqlException e)
		    {
			    throw new BusinessException("Connection to database failed!", e);
		    }

			var updateCommand = new SqlCommand("UPDATE procedures " + 
				$"name = '{procedure.Name}, " + 
				$"duration='{procedure.Duration.TotalMinutes}' " + 
				$"WHERE ProcedureID='{procedure.ProcedureID}'",
				Connection);
		    return updateCommand.ExecuteNonQuery() == 1;
	    }

	    public bool DeleteProcedure(string id)
	    {
		    try
		    {
			    Connection.Open();
		    }
		    catch (SqlException e)
		    {
			    throw new BusinessException("Connection to database failed!", e);
		    }

			var deleteCommand = new SqlCommand($"DELETE FROM procedures WHERE ProcedureID='{id}'", Connection);
		    deleteCommand.ExecuteNonQuery();
		    try
		    {
				var readCommand = new SqlCommand($"SELECT * FROM procedures WHERE ProcedureID={id}",
					Connection);

				var reader = readCommand.ExecuteReader();
			    return !reader.Read();
		    }
		    catch (SqlException e)
		    {
				throw new BusinessException("Unable to delete procedure!", e);
		    }
	    }

	    public List<Procedure> GetAllProcedures()
	    {
		    try
		    {
			    Connection.Open();
		    }
		    catch (SqlException e)
		    {
			    throw new BusinessException("Connection to database failed!", e);
		    }

			try
		    {
			    var readCommand = new SqlCommand("SELECT * FROM procedures",
				    Connection);
			    var reader = readCommand.ExecuteReader();
			    var procedures = new List<Procedure>();
				while (reader.Read())
				{
					procedures.Add(Procedure.Build(
						reader["Name"].ToString(), 
						reader["Duration"].ToString(), 
						reader["ProcedureID"].ToString()
					));
				}

			    return procedures;
		    }
		    catch (SqlException e)
		    {
			    throw new BusinessException("Cannot get procedures!", e);
		    }
	    }

	    public Procedure GetProcedure(string id)
	    {
		    try
		    {
			    Connection.Open();
		    }
		    catch (SqlException e)
		    {
			    throw new BusinessException("Connection to database failed!", e);
		    }

			try
		    {
			    var readCommand = new SqlCommand($"SELECT * FROM procedures WHERE ProcedureID={id}",
				    Connection);
			    var reader = readCommand.ExecuteReader();
			    reader.Read();

			    return Procedure.Build(
					reader["Name"].ToString(), 
					reader["Duration"].ToString(), 
					reader["ProcedureID"].ToString()
				);
		    }
		    catch (SqlException e)
		    {
			    throw new BusinessException("Cannot get procedure!", e);
		    }
	    }
    }
}