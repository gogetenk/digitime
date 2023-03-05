using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Digitime.Server.Application.Abstractions;

public interface IObtainPublicHolidays
{
    Task<List<DateTime>> GetPublicHolidaysForSpecifiedMonthAndCountry(DateTime dateTime, string country);
}
