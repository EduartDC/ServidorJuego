namespace DataAcces
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Friend
    {
        [Key]
        public int idFriend { get; set; }

        public int gameFriend { get; set; }

        public DateTime creationDate { get; set; }

        public int ownerPlayer { get; set; }

        public virtual Player Player { get; set; }
    }
}
