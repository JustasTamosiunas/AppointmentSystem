using System;
using AppointmentSystem.BusinessContracts;
using AppointmentSystem.BusinessImplementation;
using AppointmentSystem.Domain;
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
	}
}