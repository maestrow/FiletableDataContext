using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiletableDataContext.Domain
{
    public interface IFiletableEntity
    {
        [Key]
        Guid Stream_id { get; set; }
      
        Byte[] File_stream { get; set; }
      
        string Name { get; set; }
        
        string Path { get; set; }
        
        string File_type { get; }
        
        Int64 Cached_file_size { get; }
        
        DateTimeOffset Creation_time { get; set; }
        
        DateTimeOffset Last_write_time { get; set; }
        
        DateTimeOffset Last_access_time { get; set; }
        
        bool Is_directory { get; set; }
        
        bool Is_offline { get; set; }
        
        bool Is_hidden { get; set; }
        
        bool Is_readonly { get; set; }
        
        bool Is_archive { get; set; }
        
        bool Is_system { get; set; }
        
        bool Is_temporary { get; set; }
    }
}
