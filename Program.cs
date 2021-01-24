using System;
using System.Text;
using System.Security.Cryptography;

namespace PrintHashOfString
{
    class Program
    {
        enum hashAlgorithms : int{
                md5 = 0,
                sha1 =1,
                sha256 =2,
                sha384=3,
                sha512=4
            }

        static string CleanString =>" ".PadRight(Console.BufferWidth,' ');
        static void Main(string[] args)
        {
            hashAlgorithms hashAlg;
            Console.WriteLine("Hashalgorithms: MD5 (0), SHA1 (1), SHA256 (2), SHA384 (3), SHA512 (4)");
            var algC=Console.ReadKey(true);

            while (!Char.IsDigit(algC.KeyChar) || int.Parse(algC.KeyChar.ToString())<0 || int.Parse(algC.KeyChar.ToString())>4)
            {
                Console.WriteLine($"{algC.KeyChar} is not valid. Enter a number between 0 and 5");
                algC=Console.ReadKey(true);
            }

            hashAlg=(hashAlgorithms)int.Parse(algC.KeyChar.ToString());
            Console.WriteLine($"Using {hashAlg}");
            int rounds = 4;
            Console.Write("Enter number of rounds:");
            string rndStr=Console.ReadLine();

            while (! int.TryParse(rndStr, out rounds) || rounds<1){
                Console.Write($"{rndStr} is invalid. Enter a valid number of rounds or e to exit:");
                rndStr=Console.ReadLine();
                if (rndStr.ToLower() == "e"){
                    return;
                }
            }

            Console.WriteLine($"Performe {rounds} rounds for hashing.");
            Console.WriteLine("Enter text. Terminate program if string is empty.");
            string t =null;
            bool init=false;
            while (!string.IsNullOrEmpty((t = Console.ReadLine().Trim()))){
                if (init)
                    Console.WriteLine("Enter text. Terminate program if string is empty.");              
                byte[] orig= Encoding.UTF8.GetBytes(t);
                using (HashAlgorithm hash = GetHashAlgorithm(hashAlg)){
                    byte[] current = orig;
                    int count =rounds;
                    string base64="";
                    byte[] h=null;

                    while (count >0){
                        h = hash.ComputeHash(current);
                        //Console.WriteLine(BitConverter.ToString(h));
                        base64 =Convert.ToBase64String(h);
                        current=Encoding.ASCII.GetBytes(base64);
                        count--;
                    }

                    ClearLastLines(init? 2 : 1);
                    Console.WriteLine("");
                    base64 =base64.TrimEnd('=');
                    Console.WriteLine(BitConverter.ToString(h).Replace("-",""));
                    Console.WriteLine($"{base64.Length}: {base64}");
                    Console.ReadKey();
                    ClearLastLines(2);
                    MoveLines(-1);
                    init=true;
                }
            }
        }

        static HashAlgorithm GetHashAlgorithm(hashAlgorithms alg){
            switch(alg){
                case hashAlgorithms.md5: return MD5.Create();
                case hashAlgorithms.sha256: return SHA256.Create();
                case hashAlgorithms.sha1: return SHA1.Create();
                case hashAlgorithms.sha384: return SHA384.Create();
                case hashAlgorithms.sha512: return SHA512.Create();
            }
            return null;
        }

        static void ClearLastLines(int lines =1){
            Console.SetCursorPosition(0,Console.CursorTop-lines);
            for (int i=0;i<lines;i++)
            {
                Console.WriteLine(CleanString);
            }
            Console.SetCursorPosition(0,Console.CursorTop-lines);
        }

        static void MoveLines(int lines=1)
        {
            Console.SetCursorPosition(0,Console.CursorTop+lines);
        }
    }
}