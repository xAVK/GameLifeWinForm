using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLifeWinForm
{
    [Serializable]
    public class cCell
    {
        public bool IsAlive { get; set; }        
        public cCell(bool IsAlive)
        {            
            this.IsAlive = IsAlive;
        }
    }
}
