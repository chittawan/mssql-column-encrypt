using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace mssqlEncryption.Entites
{
    public class ProfileImages
    {
        [Key]
        public int Id{ get; set; }
        public byte[] Image { get; set; }
        public byte[] ImageEnc { get; set; }
    }
}
