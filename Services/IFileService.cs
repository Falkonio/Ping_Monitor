using System.Collections.Generic;

namespace Ping_Monitor
{
    public interface IFileService
    {

        string Open_File_Error { get; }
        string Save_File_Error { get; }

        List<Pinger> OpenLST(string filename);
        Settings OpenCFG(string filename);
        void SaveLST(string filename, List<Pinger> pingersList);
        void SaveCFG(string filename, Settings settings);
    }
}
