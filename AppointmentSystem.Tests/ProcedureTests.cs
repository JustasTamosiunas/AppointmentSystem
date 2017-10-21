using System;
using System.Collections.Generic;
using AppointmentSystem.BusinessContracts;
using AppointmentSystem.BusinessImplementation;
using AppointmentSystem.Domain;
using AppointmentSystem.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace AppointmentSystem.Tests
{
	[TestFixture]
	public class ProcedureTests
	{
		private IProcedureService procedureService;

		[SetUp]
		public void SetUp()
		{
			procedureService = new ProcedureService();
		}

		[Test]
		[Category("Test")]
		[Category("IntegrationTests.Procedure")]
		public void Can_insert_procedure_to_database()
		{
			var duration = new TimeSpan(0, 30, 0);
			var procedure = new Procedure
			{
				Name = "Test",
				Duration = duration
			};

			string id = procedureService.CreateProcedure(procedure);
			id.ShouldNotBeNull();
		}

		[Test]
		[Category("Test")]
		[Category("IntegrationTests.Procedure")]
		public void Cannot_insert_procedure_to_database_when_duration_not_provided()
		{
			var procedure = new Procedure
			{
				Name = "Test"
			};

			typeof(BusinessException).ShouldBeThrownBy(() => procedureService.CreateProcedure(procedure));
		}

		[Test]
		[Category("Test")]
		[Category("IntegrationTests.Procedure")]
		public void Cannot_insert_procedure_to_database_when_name_not_provided()
		{
			var duration = new TimeSpan(0, 30, 0);
			var procedure = new Procedure
			{
				Duration = duration
			};

			typeof(BusinessException).ShouldBeThrownBy(() => procedureService.CreateProcedure(procedure));
		}

		[Test]
		[Category("Test")]
		[Category("IntegrationTests.Procedure")]
		public void Can_delete_procedure_from_database()
		{
			var duration = new TimeSpan(0, 30, 0);
			var procedure = new Procedure
			{
				Name = "DELETE",
				Duration = duration
			};

			string id = procedureService.CreateProcedure(procedure);
			id.ShouldNotBeNull();

			procedureService.DeleteProcedure(id);
			typeof(BusinessException).ShouldBeThrownBy(() => procedureService.GetProcedure(id));
		}

		[Test]
		[Category("Test")]
		[Category("IntegrationTests.Procedure")]
		public void Can_get_procedure_by_id()
		{
			var duration = new TimeSpan(0, 60, 0);
			var procedure = new Procedure
			{
				Name = "GET",
				Duration = duration
			};

			string id = procedureService.CreateProcedure(procedure);
			id.ShouldNotBeNull();

			var procedureFromDb = procedureService.GetProcedure(id);
			procedureFromDb.ShouldNotBeNull();
		}

		[Test]
		[Category("Test")]
		[Category("IntegrationTests.Procedure")]
		public void Can_get_all_procedures()
		{
			var duration1 = new TimeSpan(0, 60, 0);
			var procedure1 = new Procedure
			{
				Name = "GET1",
				Duration = duration1
			};

			var duration2 = new TimeSpan(0, 60, 0);
			var procedure2 = new Procedure
			{
				Name = "GET2",
				Duration = duration2
			};

			string id1 = procedureService.CreateProcedure(procedure1);
			id1.ShouldNotBeNull();

			string id2 = procedureService.CreateProcedure(procedure2);
			id2.ShouldNotBeNull();

			var procedures = procedureService.GetAllProcedures();

			procedures.ShouldBe<List<Procedure>>();
			(procedures.Count > 0).ShouldBeTrue();
		}

		[Test]
		[Category("Test")]
		[Category("IntegrationTests.Procedure")]
		public void Can_update_language_to_database()
		{
			var duration = new TimeSpan(0, 60, 0);
			var procedure = new Procedure
			{
				Name = "UPDATE",
				Duration = duration
			};
			procedure.ProcedureID = procedureService.CreateProcedure(procedure);

			procedure.Name = "UPDATED";
			procedureService.UpdateProcedure(procedure);

			var procedureFromDb = procedureService.GetProcedure(procedure.ProcedureID);
			procedureFromDb.ShouldNotBeNull();
			procedureFromDb.Name.ShouldEqual("UPDATED");
		}


		[TearDown]
		public void Dispose()
		{
			foreach (var procedure in procedureService.GetAllProcedures())
			{
				procedureService.DeleteProcedure(procedure.ProcedureID);
			}
		}
	}
}