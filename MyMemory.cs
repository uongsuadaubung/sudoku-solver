using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Sudoku_solver
{
    internal class MyMemory
    {

        private IntPtr _processHandle;
        private Process? _process;
        private const int ProcessAllAccess = 0x1F0FFF;

        private readonly string _processName;

        private IntPtr _baseAddress;
        
        //const int PROCESS_WM_READ = 0x0010;
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);




        private byte[] ReadBytes(IntPtr handle, long address, int size)
        {
            if (!IsOk())
            {
                return Array.Empty<byte>();
            }

            int bytesRead = 0;
            byte[] buffer = new byte[size];
            ReadProcessMemory(handle, (IntPtr) address, buffer, size, ref bytesRead);
            return buffer;
        }
        /// <summary>
        /// Lấy Base Address của chương trình
        /// </summary>
        /// <returns>Base address</returns>
        public IntPtr GetBaseAddress()
        {
            return _baseAddress;
        }

        
        private static void WriteBytes(IntPtr handle, long address, dynamic value, int size)
        {
            int bytesWrite = 0;
            byte[] buffer = BitConverter.GetBytes(value);
            WriteProcessMemory(handle, (IntPtr) address, buffer, size, ref bytesWrite);
        }

        private void GetProcess()
        {
            Process[] processes = Process.GetProcessesByName(_processName);

            if (processes.Length > 0)
            {
                _process = Process.GetProcessesByName(_processName)[0];

                _processHandle = OpenProcess(ProcessAllAccess, false, _process.Id);
                if (_process.MainModule == null) return;
                _baseAddress = _process.MainModule.BaseAddress;
                foreach (ProcessModule module in _process.Modules)
                {
                    if (module.ModuleName != this._processName + ".dll") continue;
                    _baseAddress = module.BaseAddress;
                    break;
                }
            }
            else
            {
                _process = null;
            }
        }

        

        public MyMemory(string processName)
        {
            this._processName = processName; 

            GetProcess();
        
        }

        private long ReadLong(long address)
        {
            return BitConverter.ToInt64(ReadBytes(_processHandle, address, 8), 0);
        }
        public int ReadInt(long address)
        {
            return BitConverter.ToInt32(ReadBytes(_processHandle, address, 4), 0);
        }
        

        public void WriteNumber(long address, dynamic value, int length = 4)
        {
            if (!IsOk())
            {
                return;
            }
            WriteBytes(_processHandle, address, value, length);
        }

        public long GetAddressFromPointer(long[] offsets)
        {

            long value = 0;

            for (int i = 0; i < offsets.Length - 1; i++)
            {
                value = ReadLong(offsets[i] + value);
            }
            long addr = value + offsets[^1];
            return addr;
        }

        public bool IsOk()
        {
            bool result = _process is { HasExited: false };
            if (!result)
            {
                GetProcess();
            }
            return result;
        }

        
    }
}
