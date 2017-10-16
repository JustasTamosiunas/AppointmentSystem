using System;
using System.Collections.Generic;
using System.Data;
using AppointmentSystem.BusinessContracts;
using AppointmentSystem.Domain;
using FluentValidation.Results;
using System.Data.SqlClient;
using AppointmentSystem.Utils;
using IniParser;

namespace AppointmentSystem.BusinessImplementation
{
    public class ProcedureService : IProcedureService
    {
		public SqlConnection Connection { get; set; }

	    public ProcedureService()
	    {
		    Connection = new SqlConnection("user id=username;" +
				"Password=password;" + 
				"Server=serverurl;" +
				"Trusted_Connection=yes;" +
				"Database=database; " +
				"Connection Timeout=30");
		    try
		    {
			    Connection.Open();
		    }
		    catch (SqlException e)
		    {
			    throw new BusinessException("Connection to database failed!", e);
		    }
		}

	    public string CreateProcedure(Procedure procedure)
	    {
		    ProcedureValidator validator = new ProcedureValidator();
		    ValidationResult results = validator.Validate(procedure);

		    bool validationSucceded = results.IsValid; 

		    if (validationSucceded)
		    {
			    var writeCommand = new SqlCommand("INSERT INTO procedure (name, duration) " +
					$"Values ('{procedure.Name}', '{Procedure.GetDurationInMinutes(procedure.Duration)})", Connection);
			    writeCommand.ExecuteNonQuery();
			    try
			    {
				    var readCommand = new SqlCommand("SELECT * FROM procedure " +
						$"WHERE name='{procedure.Name}' " +
						$"AND duration='{procedure.Duration.Hour * 60 + procedure.Duration.Minute}'",
					    Connection);

				    var reader = readCommand.ExecuteReader();
				    reader.Read();
				    return reader["ProcedureID"].ToString();
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
		    var updateCommand = new SqlCommand("UPDATE procedure " + 
				$"name = '{procedure.Name}, " + 
				$"duration='{Procedure.GetDurationInMinutes(procedure.Duration)}' " + 
				$"WHERE ProcedureID='{procedure.ProcedureID}'",
				Connection);
		    return updateCommand.ExecuteNonQuery() == 1;
	    }

	    public bool DeleteProcedure(string id)
	    {
			var deleteCommand = new SqlCommand($"DELETE FROM procedure WHERE ProcedureID='{id}'", Connection);
		    deleteCommand.ExecuteNonQuery();
		    try
		    {
				var readCommand = new SqlCommand($"SELECT * FROM procedure WHERE ProcedureID={id}",
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
			    var readCommand = new SqlCommand("SELECT * FROM procedure",
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
			    var readCommand = new SqlCommand($"SELECT * FROM procedure WHERE ProcedureID={id}",
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