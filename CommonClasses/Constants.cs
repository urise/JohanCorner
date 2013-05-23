using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonClasses
{
    public enum AccessComponent
    {
        None = 0,
        Payrolls = 1,
        Transactions = 2,
        Calculations = 3,
        BankPayrolls = 4,
        Acts = 5,
        Employees = 6,
        IndividPayroll = 7,
        Settings = 9,
        Currencies = 10,
        Company = 11,
        FinanceKey = 12,
        PositionCategories = 13,
        PositionLevels = 14,
        Users = 15,
        Roles = 16,
        Periods = 17,
        Duties = 18,
        TransactionTypes = 19,
        Positions = 20
    }

    public enum AccessLevel
    {
        None = 1,
        Read = 2,
        ReadWrite = 3
    }

    public class Constants
    {
    }
}
