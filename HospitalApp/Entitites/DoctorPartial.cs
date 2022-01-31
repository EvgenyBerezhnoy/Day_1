using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalApp.Entitites;

namespace HospitalApp.Entitites
{
   public partial class Doctor
    {
        public string FullName
        {
            get
            {
                var fullName = $" {LastName}  {FirstName}";

                if (string.IsNullOrWhiteSpace(Patronymic) == false)
                {
                    fullName += $" {Patronymic} ";
                }
                return fullName;
            }
        }

       
    }
}
