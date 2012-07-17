using System;
using System.Collections;

namespace NhibernateEntity
{
	#region Log

	/// <summary>
	/// Log object for NHibernate mapped table 'Log'.
	/// </summary>
	public class Log
	{
		#region Member Variables
		
		protected int _id;
		protected DateTime _date;
		protected string _thread;
		protected string _level;
		protected string _logger;
		protected string _message;
		protected string _exception;

		#endregion

		#region Constructors

		public Log() { }

		public Log( DateTime date, string thread, string level, string logger, string message, string exception )
		{
			this._date = date;
			this._thread = thread;
			this._level = level;
			this._logger = logger;
			this._message = message;
			this._exception = exception;
		}

		#endregion

		#region Public Properties

		public int Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public DateTime Date
		{
			get { return _date; }
			set { _date = value; }
		}

		public string Thread
		{
			get { return _thread; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Thread", value, value.ToString());
				_thread = value;
			}
		}

		public string Level
		{
			get { return _level; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Level", value, value.ToString());
				_level = value;
			}
		}

		public string Logger
		{
			get { return _logger; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Logger", value, value.ToString());
				_logger = value;
			}
		}

		public string Message
		{
			get { return _message; }
			set
			{
				if ( value != null && value.Length > 4000)
					throw new ArgumentOutOfRangeException("Invalid value for Message", value, value.ToString());
				_message = value;
			}
		}

		public string Exception
		{
			get { return _exception; }
			set
			{
				if ( value != null && value.Length > 2000)
					throw new ArgumentOutOfRangeException("Invalid value for Exception", value, value.ToString());
				_exception = value;
			}
		}

		

		#endregion
	}
	#endregion
}