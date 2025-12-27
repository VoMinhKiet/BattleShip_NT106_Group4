using System;

namespace NT106_BattleshipClient.Models
{
    public class FriendDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int Stars { get; set; }
        public string RelationStatus { get; set; }
        public bool Online { get; set; }
        public DateTime? LastOnline { get; set; }
    }
}
