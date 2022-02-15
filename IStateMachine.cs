using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleFox.AI
{
    public interface IStateMachine<TEnum>
    {
        TEnum State { get; set; }

        void Update();

        bool Enabled { get;  set; }
    }
}
