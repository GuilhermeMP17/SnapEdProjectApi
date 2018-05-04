using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapEd.Infra.Models
{
    #region TB_POSTING
    [Table("TB_POSTING")]
    public class Posting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPosting { get; set; }

        public int IdUser { get; set; }

        public int IdClassRoom { get; set; }

        [MaxLength(120)]
        public string Title { get; set; }

        [MaxLength(580)]
        public string Description { get; set; }

        [MaxLength(240)]
        public string Image { get; set; }

        [MaxLength(240)]
        public string ImagePath { get; set; }
    }
    #endregion
}
