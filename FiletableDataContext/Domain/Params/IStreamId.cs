using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiletableDataContext.Domain.Params
{
    public interface IStreamId
    {
        Guid Stream_id { get; set; }
    }
}
