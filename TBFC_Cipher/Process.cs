using System;
using System.Text;



public class Process {
    public static void Main(String[] args)
    {

        TFBC_CoreCipher cipher = new TFBC_CoreCipher(541.764, 141242452412.6536);
	byte[] p = Encoding.ASCII.GetBytes("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        byte[] e = cipher.encrypt(p);
        byte[] d = cipher.decrypt(e);
		Console.WriteLine(Encoding.ASCII.GetString(d));



    }
}

