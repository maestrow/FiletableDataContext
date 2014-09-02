using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiletableDataContext.Domain.Params
{
    public interface IRenameParams: IStreamId
    {
        string Name { get; set; }
    }
}
