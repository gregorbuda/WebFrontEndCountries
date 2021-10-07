using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFrontEndCountries.Models
{
	public class SubDivision
	{
		public Int16 SubDivisionId { get; set; }
		public string CodeSubDivision { get; set; }
		public string NameSubDivision { get; set; }
		public Int16 CountryId { get; set; }
		public Countries countries { get; set; }
	}


	public class Sub
	{
		public Int16 SubDivisionId { get; set; }
		public string CodeSubDivision { get; set; }
		public string NameSubDivision { get; set; }
		public Int16 CountryId { get; set; }

	}

	public class Countries
	{

		public Int16 CountryId { get; set; }
		public string NameCountry { get; set; }
		public string AlphaOneCountry { get; set; }
		public string AlphaTwoCountry { get; set; }
		public string NumericCode { get; set; }
		public bool Independent { get; set; }
		public ICollection<SubDivision> subDivision { get; set; }
	}
}
