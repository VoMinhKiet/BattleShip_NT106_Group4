using NT106_BattleshipClient;
using System.Collections.Generic;

public static class ChatSession
{
    public static ucChatBox ChatBox { get; private set; }


    public static List<string> MessageHistory { get; } = new List<string>();


    public static void Init(int idPhongCho)
    {
        if (ChatBox != null) return;

        ChatBox = new ucChatBox(idPhongCho);
    }

    public static void Clear()
    {
        MessageHistory.Clear();
        ChatBox = null;
    }
}
