using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapEd.Infra.Models
{
    [Table("TB_CLASSROOM")]
    public class ClassRoom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdClassRom { get; set; }

        public int IdUserCreated { get; set; }

        [MaxLength(120)]
        public string NameClass { get; set; }

        [MaxLength(240)]
        public string DescriptionClass { get; set; }

        [MaxLength(240)]
        public string ImageClass { get; set; }

        [MaxLength(240)]
        public string ImagePathClass { get; set; }
    }
}
