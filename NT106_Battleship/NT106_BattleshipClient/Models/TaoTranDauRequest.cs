using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT106_BattleshipClient.Models
{
    public class TaoTranDauRequest
    {
        public int IdPlayer1 { get; set; }
        public int IdPlayer2 { get; set; }
        public int IdNhanVat1 { get; set; }
        public int IdNhanVat2 { get; set; }
        public int KichThuoc { get; set; }
        public int IdPhongCho { get; set; }
    }
}
