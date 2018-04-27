using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapEd.Infra.Models
{
    public enum Permission
    {
        Aluno = 1,
        Pais = 2,
        Supervisor = 3,

        Professor = 9,
        Administrator = 10,
    }

    [Table("TB_USER")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUser { get; set; }

        [MaxLength(120)]
        public string FirstName { get; set; }

        [MaxLength(120)]
        public string LastName { get; set; }

        [MaxLength(120)]
        public string Login { get; set; }

        [MaxLength(120)]
        public string Password { get; set; }

        public bool Active { get; set; }

        Permission PermS;
        public Permission perm
        {
            get
            {
                return PermS;
            }
            set
            {
                PermS = value;
            }
        }
    }
}
