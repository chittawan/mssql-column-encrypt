using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mssqlEncryption.Entites
{
    public class MemberEntity
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } // FirstName (length: 150)
        public string LastName { get; set; } // LastName (length: 150)
        public string CardId { get; set; } // CardId (length: 20)
        public string FirstNameEnc { get; set; } // FirstNameEnc
        public string LastNameEnc { get; set; } // LastNameEnc
        public string CardIdEnc { get; set; } // CardIdEnc (length: 128)
        public bool DelFlag { get; set; } // DelFlag
        public string CreatedBy { get; set; } // CreatedBy (length: 50)
        public DateTime? CreatedDate { get; set; } // CreatedDate
        public string UpdatedBy { get; set; } // UpdatedBy (length: 50)
        public DateTime? UpdatedDated { get; set; } // UpdatedDated
    }
}
