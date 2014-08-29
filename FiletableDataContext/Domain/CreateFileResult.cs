using System;

namespace FiletableDataContext.Domain
{
    public class CreateFileResult
    {
        public Guid Stream_id { get; set; }
        
        public string Path_locator { get; set; }
    }
}
