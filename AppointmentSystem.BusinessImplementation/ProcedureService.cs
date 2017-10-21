using System;
using System.Collections.Generic;
using AppointmentSystem.BusinessContracts;
using AppointmentSystem.Domain;
using FluentValidation.Results;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using AppointmentSystem.Utils;
using MySql.Data.MySqlClient;

namespace AppointmentSystem.BusinessImplementation
{
    public class ProcedureService : IProcedureService
    {
		public MySqlConnection Connection { get; set; }

	    public ProcedureService()
	    {
		    Connection = new MySqlConnection(IniParse.GetConnectionString());
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
			    catch (MySqlException ex)
			    {
				    switch (ex.Number)
				    {
					    case 0:
						    throw new BusinessException("Cannot connect to server.  Contact administrator", ex);
					    case 1045:
						    throw new BusinessException("Invalid username/password, please try again", ex);
				    }
			    }

				var writeCommand = new MySqlCommand("INSERT INTO procedures (name, duration) " +
					$"Values ('{procedure.Name}', '{procedure.Duration.TotalMinutes}')", Connection);
				writeCommand.ExecuteNonQuery();
			    try
			    {
				    var readCommand = new MySqlCommand("SELECT * FROM procedures " +
						$"WHERE name='{procedure.Name}' " +
						$"AND duration='{procedure.Duration.TotalMinutes}'",
					    Connection);

					var reader = readCommand.ExecuteReader();
				    if (reader.Read())
				    {
					    return reader["ID"].ToString();
				    }
				    else
				    {
						throw new BusinessException("Cannot save procedure!", new Exception());
					}
			    }
			    catch (SqlException e)
			    {
				    throw new BusinessException("Cannot save procedure!", e);
			    }
			    finally
			    {
				    Connection.Close();
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

			var updateCommand = new MySqlCommand("UPDATE procedures SET " + 
				$"name='{procedure.Name}', " + 
				$"duration='{procedure.Duration.TotalMinutes}' " + 
				$"WHERE ID='{procedure.ProcedureID}'",
				Connection);
			bool update = updateCommand.ExecuteNonQuery() == 1;
			Connection.Close();
		    return update;
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

			var deleteCommand = new MySqlCommand($"DELETE FROM procedures WHERE ID='{id}'", Connection);

		    deleteCommand.ExecuteNonQuery();
		    try
		    {
				var readCommand = new MySqlCommand($"SELECT * FROM procedures WHERE ID='{id}'", Connection);
				var reader = readCommand.ExecuteReader();
			    return !reader.Read();
		    }
		    catch (SqlException e)
		    {
				throw new BusinessException("Unable to delete procedure!", e);
		    }
		    finally
		    {
			    Connection.Close();
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
			    var readCommand = new MySqlCommand("SELECT * FROM procedures",
				    Connection);
				var reader = readCommand.ExecuteReader();
			    var procedures = new List<Procedure>();
				while (reader.Read())
				{
					procedures.Add(Procedure.Build( 
						reader["Name"].ToString(), 
						reader["Duration"].ToString(),
						reader["ID"].ToString()
					));
				}

			    return procedures;
		    }
		    catch (SqlException e)
		    {
			    throw new BusinessException("Cannot get procedures!", e);
		    }
			finally
			{
				Connection.Close();
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
			    var readCommand = new MySqlCommand($"SELECT * FROM procedures WHERE ID='{id}'",
				    Connection);
				var reader = readCommand.ExecuteReader();
			    if (reader.Read())
			    {
				    return Procedure.Build(
					    reader["Name"].ToString(),
					    reader["Duration"].ToString(),
					    reader["ID"].ToString()
				    );
			    }
			    else
			    {
				    throw new BusinessException("Cannot get procedure!", new Exception());
				}
		    }
		    catch (SqlException e)
		    {
			    throw new BusinessException("Cannot get procedure!", e);
		    }
			finally
			{
				Connection.Close();
			}
		}
    }
}