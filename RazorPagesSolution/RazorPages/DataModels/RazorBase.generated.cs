//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by T4Model template for T4 (https://github.com/linq2db/linq2db).
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------

#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace DataModels
{
	/// <summary>
	/// Database       : RazorBase
	/// Data Source    : PROG
	/// Server Version : 12.00.2269
	/// </summary>
	public partial class RazorBaseDB : LinqToDB.Data.DataConnection
	{
		public ITable<PhoneBook>        PhoneBooks         { get { return this.GetTable<PhoneBook>(); } }
		public ITable<PhoneBookHistory> PhoneBookHistories { get { return this.GetTable<PhoneBookHistory>(); } }

		public RazorBaseDB()
		{
			InitDataContext();
			InitMappingSchema();
		}

		public RazorBaseDB(string configuration)
			: base(configuration)
		{
			InitDataContext();
			InitMappingSchema();
		}

		partial void InitDataContext  ();
		partial void InitMappingSchema();
	}

	[Table(Schema="dbo", Name="PhoneBook")]
	public partial class PhoneBook
	{
		[Column("id_pb"),      PrimaryKey,	NotNull] public int    IdPb       { get; set; } // int
		[Column("name"),                    NotNull] public string Name       { get; set; } // nvarchar(50)
		[Column("patronymic"),				NotNull] public string Patronymic { get; set; } // nvarchar(50)
		[Column("surname"),					NotNull] public string Surname    { get; set; } // nvarchar(50)
		[Column("phone"),					NotNull] public string Phone      { get; set; } // nvarchar(50)
		[Column("sex"),                     NotNull] public string Sex        { get; set; } // char(10)

		#region Associations

		/// <summary>
		/// FK_PhoneBookHistory_PhoneBook_BackReference
		/// </summary>
		[Association(ThisKey="IdPb", OtherKey="IdPb", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<PhoneBookHistory> PhoneBookHistories { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="PhoneBookHistory")]
	public partial class PhoneBookHistory
	{
		[Column("id_pbh"),     PrimaryKey,  NotNull] public int      IdPbh      { get; set; } // int
		[Column("id_pb"),                   NotNull] public int      IdPb       { get; set; } // int
		[Column("date"),                    NotNull] public DateTime Date       { get; set; } // datetime2(7)
		[Column("name"),                    NotNull] public string   Name       { get; set; } // nvarchar(50)
		[Column("patronymic"),    Nullable         ] public string   Patronymic { get; set; } // nvarchar(50)
		[Column("surname"),       Nullable         ] public string   Surname    { get; set; } // nvarchar(50)
		[Column("phone"),         Nullable         ] public string   Phone      { get; set; } // nvarchar(50)
		[Column("sex"),                     NotNull] public string   Sex        { get; set; } // char(10)

		#region Associations

		/// <summary>
		/// FK_PhoneBookHistory_PhoneBook
		/// </summary>
		[Association(ThisKey="IdPb", OtherKey="IdPb", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="FK_PhoneBookHistory_PhoneBook", BackReferenceName="PhoneBookHistories")]
		public PhoneBook PhoneBook { get; set; }

		#endregion
	}

	public static partial class RazorBaseDBStoredProcedures
	{
		#region SpHelpdiagrams

		public static IEnumerable<SpHelpdiagramsResult> SpHelpdiagrams(this RazorBaseDB dataConnection, string @diagramname, int? @owner_id)
		{
			return dataConnection.QueryProc<SpHelpdiagramsResult>("[dbo].[sp_helpdiagrams]",
				new DataParameter("@diagramname", @diagramname, DataType.NVarChar),
				new DataParameter("@owner_id",    @owner_id,    DataType.Int32));
		}

		public partial class SpHelpdiagramsResult
		{
			public string Database { get; set; }
			public string Name     { get; set; }
			public int    ID       { get; set; }
			public string Owner    { get; set; }
			public int    OwnerID  { get; set; }
		}

		#endregion

		#region SpHelpdiagramdefinition

		public static IEnumerable<SpHelpdiagramdefinitionResult> SpHelpdiagramdefinition(this RazorBaseDB dataConnection, string @diagramname, int? @owner_id)
		{
			return dataConnection.QueryProc<SpHelpdiagramdefinitionResult>("[dbo].[sp_helpdiagramdefinition]",
				new DataParameter("@diagramname", @diagramname, DataType.NVarChar),
				new DataParameter("@owner_id",    @owner_id,    DataType.Int32));
		}

		public partial class SpHelpdiagramdefinitionResult
		{
			public int?   version    { get; set; }
			public byte[] definition { get; set; }
		}

		#endregion

		#region SpCreatediagram

		public static int SpCreatediagram(this RazorBaseDB dataConnection, string @diagramname, int? @owner_id, int? @version, byte[] @definition)
		{
			return dataConnection.ExecuteProc("[dbo].[sp_creatediagram]",
				new DataParameter("@diagramname", @diagramname, DataType.NVarChar),
				new DataParameter("@owner_id",    @owner_id,    DataType.Int32),
				new DataParameter("@version",     @version,     DataType.Int32),
				new DataParameter("@definition",  @definition,  DataType.VarBinary));
		}

		#endregion

		#region SpRenamediagram

		public static int SpRenamediagram(this RazorBaseDB dataConnection, string @diagramname, int? @owner_id, string @new_diagramname)
		{
			return dataConnection.ExecuteProc("[dbo].[sp_renamediagram]",
				new DataParameter("@diagramname",     @diagramname,     DataType.NVarChar),
				new DataParameter("@owner_id",        @owner_id,        DataType.Int32),
				new DataParameter("@new_diagramname", @new_diagramname, DataType.NVarChar));
		}

		#endregion

		#region SpAlterdiagram

		public static int SpAlterdiagram(this RazorBaseDB dataConnection, string @diagramname, int? @owner_id, int? @version, byte[] @definition)
		{
			return dataConnection.ExecuteProc("[dbo].[sp_alterdiagram]",
				new DataParameter("@diagramname", @diagramname, DataType.NVarChar),
				new DataParameter("@owner_id",    @owner_id,    DataType.Int32),
				new DataParameter("@version",     @version,     DataType.Int32),
				new DataParameter("@definition",  @definition,  DataType.VarBinary));
		}

		#endregion

		#region SpDropdiagram

		public static int SpDropdiagram(this RazorBaseDB dataConnection, string @diagramname, int? @owner_id)
		{
			return dataConnection.ExecuteProc("[dbo].[sp_dropdiagram]",
				new DataParameter("@diagramname", @diagramname, DataType.NVarChar),
				new DataParameter("@owner_id",    @owner_id,    DataType.Int32));
		}

		#endregion
	}

	public static partial class SqlFunctions
	{
		#region FnDiagramobjects

		[Sql.Function(Name="dbo.fn_diagramobjects", ServerSideOnly=true)]
		public static int? FnDiagramobjects()
		{
			throw new InvalidOperationException();
		}

		#endregion
	}

	public static partial class TableExtensions
	{
		public static PhoneBook Find(this ITable<PhoneBook> table, int IdPb)
		{
			return table.FirstOrDefault(t =>
				t.IdPb == IdPb);
		}

		public static PhoneBookHistory Find(this ITable<PhoneBookHistory> table, int IdPbh)
		{
			return table.FirstOrDefault(t =>
				t.IdPbh == IdPbh);
		}
	}
	public class ConnectionStringSettings : IConnectionStringSettings
	{
		public string ConnectionString { get; set; }
		public string Name { get; set; }
		public string ProviderName { get; set; }
		public bool IsGlobal => false;
	}

	public class MySettings : ILinqToDBSettings
	{
		public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

		public string DefaultConfiguration => "SqlServer";
		public string DefaultDataProvider => "SqlServer";

		public IEnumerable<IConnectionStringSettings> ConnectionStrings
		{
			get
			{
				yield return
					new ConnectionStringSettings
					{
						Name = "RazorBase",
						ProviderName = "SqlServer",
						ConnectionString = Environment.MachineName == "PROG" ? @"Server=PROG;Database=RazorBase;Trusted_Connection=True;Enlist=False;" :
							@"Server=DESKTOP-NRD2CRJ\SQLEXPRESS;Database=RazorBase;Trusted_Connection=True;Enlist=False;"
                    };
			}
		}
	}
	
}

#pragma warning restore 1591
