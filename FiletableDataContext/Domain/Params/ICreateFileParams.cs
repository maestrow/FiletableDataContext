using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiletableDataContext.Domain.Params
{
    public interface ICreateFileParams: ICreateDirParams
    {
        Byte[] File_stream { get; set; }
        
        bool Is_hidden { get; set; }

        bool Is_readonly { get; set; }

        bool Is_archive { get; set; }

        bool Is_system { get; set; }

        bool Is_temporary { get; set; }
    }
}
