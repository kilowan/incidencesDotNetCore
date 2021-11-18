using MiPrimeraApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraApp.Business
{
    public class NoteBz : BusinessBase
    {
        public bool InsertNote(int ownerId, int incidenceId, string issueDesc, string noteType)
        {
            try
            {
                
                return Insert("notes",
                    new CDictionary<string, string> 
                    {
                        { "employee", null, ownerId.ToString() },
                        { "incidence", null, incidenceId.ToString() },
                        { "noteType", null, $"'{ noteType }'" },
                        { "noteStr", null, $"'{ issueDesc }'" }
                    }
                );
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
    }
}
