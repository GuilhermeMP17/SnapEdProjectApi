using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapEd.Infra.Models
{
    #region TB_STUDENT_CLASS
    [Table("TB_STUDENT_CLASS")]
    public class StudentClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMatriculation { get; set; }

        public int IdUser { get; set; }

        public int IdClassRoom { get; set; }
    }
    #endregion
}
