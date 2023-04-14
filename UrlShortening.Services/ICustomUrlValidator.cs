using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortening.Shared.Models;

namespace UrlShortening.Abstractions
{
	public interface ICustomUrlValidator
	{
		bool IsCustomUrlValid(UrlMap urlMap);
	}
}
