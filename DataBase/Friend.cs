namespace DataBase
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

        public int idPlayerOne { get; set; }

        public DateTime date { get; set; }

        public int Player_idPlayer { get; set; }

        public virtual Player Player { get; set; }
    }
}
