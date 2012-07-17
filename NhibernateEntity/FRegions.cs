using System;
using System.Collections;

namespace NhibernateEntity
{
	#region FRegion

	/// <summary>
	/// FRegion object for NHibernate mapped table 'FRegions'.
	/// </summary>
	public class FRegion
	{
		#region Member Variables
		
		protected string _id;
		protected string _names;
		protected string _fartherID;
		protected int _depth;
		protected string _path;
		protected int _isLeaf;
		protected int _islock;
		protected int _sort;

		#endregion

		#region Constructors

		public FRegion() { }

		public FRegion( string names, string fartherID, int depth, string path, int isLeaf, int islock, int sort )
		{
			this._names = names;
			this._fartherID = fartherID;
			this._depth = depth;
			this._path = path;
			this._isLeaf = isLeaf;
			this._islock = islock;
			this._sort = sort;
		}

		#endregion

		#region Public Properties

		public string Id
		{
			get {return _id;}
			set
			{
				if ( value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for Id", value, value.ToString());
				_id = value;
			}
		}

		public string Names
		{
			get { return _names; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Names", value, value.ToString());
				_names = value;
			}
		}

		public string FartherID
		{
			get { return _fartherID; }
			set
			{
				if ( value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for FartherID", value, value.ToString());
				_fartherID = value;
			}
		}

		public int Depth
		{
			get { return _depth; }
			set { _depth = value; }
		}

		public string Path
		{
			get { return _path; }
			set
			{
				if ( value != null && value.Length > 150)
					throw new ArgumentOutOfRangeException("Invalid value for Path", value, value.ToString());
				_path = value;
			}
		}

		public int IsLeaf
		{
			get { return _isLeaf; }
			set { _isLeaf = value; }
		}

		public int Islock
		{
			get { return _islock; }
			set { _islock = value; }
		}

		public int Sort
		{
			get { return _sort; }
			set { _sort = value; }
		}

		

		#endregion
	}
	#endregion
}