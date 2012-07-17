using System;
using System.Collections;

namespace NhibernateEntity
{
	#region UserInfo

	/// <summary>
	/// UserInfo object for NHibernate mapped table 'UserInfo'.
	/// </summary>
	public class UserInfo
	{
		#region Member Variables
		
		protected int _id;
		protected string _userName;
		protected int _age;
		protected string _address;

		#endregion

		#region Constructors

		public UserInfo() { }

		public UserInfo( string userName, int age, string address )
		{
			this._userName = userName;
			this._age = age;
			this._address = address;
		}

		#endregion

		#region Public Properties

		public int Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string UserName
		{
			get { return _userName; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for UserName", value, value.ToString());
				_userName = value;
			}
		}

		public int Age
		{
			get { return _age; }
			set { _age = value; }
		}

		public string Address
		{
			get { return _address; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Address", value, value.ToString());
				_address = value;
			}
		}

		

		#endregion
	}
	#endregion
}