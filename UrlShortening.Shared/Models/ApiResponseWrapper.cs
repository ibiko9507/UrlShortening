using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortening.Shared.Models
{
	public class ApiResponseWrapper
	{
		public object? Data { get; set; }
		public bool HasError { get; set; }
		public string? Error { get; set; }
	}
}
