using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace AppointmentSystem.Utils
{
	[Serializable]
	public class BusinessException : Exception
	{
		public string Key { get; set; }

		public IList<ValidationFailure> Errors { get; set; } = new List<ValidationFailure>();

		public BusinessException(Exception originalException, string messageFormatString, params object[] args)
			: base(string.Format(messageFormatString, args), originalException.GetBaseException())
		{
		}

		public BusinessException()
		{
		}

		public BusinessException(string message, IList<ValidationFailure> errors)
			: base(message)
		{
			Errors = errors;
		}

		public BusinessException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public BusinessException(string message, string key)
			: base(message)
		{
			Key = key;
		}

		public BusinessException(string message, Exception innerException, string key)
			: base(message, innerException)
		{
			Key = key;
		}
	}
}