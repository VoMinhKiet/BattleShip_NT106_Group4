namespace NT106_BattleshipClient.Models
{
    public class InvitePayload
    {
        public int roomId { get; set; }
        public int fromUserId { get; set; }
        public string fromUsername { get; set; }
    }
}
