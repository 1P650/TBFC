using System;

public class TFBC_CoreCipher {
    private double K1;
    private double K2;
    private const int ROUNDS = 128;
    private int m = 0;

    public TFBC_CoreCipher(double K1, double K2){
        this.K1 = K1;
        this.K2 = K2;
        m = (int) ((K1 * K2) + (K2 * K1 + K1 * K2));
    }


    public byte[] encrypt(byte[] plaintext){
        if(plaintext.Length < 4){
            byte[] plaintext_ext = new byte[4];
            Array.Copy(plaintext,0,plaintext_ext,0,plaintext.Length);
            plaintext =  plaintext_ext;
        }
        if(plaintext.Length >=4 && plaintext.Length % 4 != 0){
            uint extSize = (uint) ((plaintext.Length / 4) * 4 + 4);
            byte[] plaintext_ext = new byte[extSize];
            Array.Copy(plaintext,0,plaintext_ext,0,plaintext.Length);
            plaintext =  plaintext_ext;

        }

        byte[] ciphertext = new byte[plaintext.Length];

        uint[] plain_32 = new uint[plaintext.Length/4];
        for(int i = 0; i < plain_32.Length; i++){
            byte[] PlainChunck = new byte[4];
            Array.Copy(plaintext, i*4, PlainChunck, 0, 4);
            plain_32[i] = BitConverter.ToUInt32(PlainChunck, 0);
        }
        for (int i = 0; i < plain_32.Length; i++) {
            plain_32[i] = enc(plain_32[i]);
        }

        for (int i = 0; i < plain_32.Length; i++) {
            byte[] cipherChunck = BitConverter.GetBytes(plain_32[i]);
            Array.Copy(cipherChunck, 0, ciphertext, i*4, 4);

        }
            return ciphertext;
    }

    public byte[] decrypt(byte[] ciphertext){
        byte[] plaintext = new byte[ciphertext.Length];
        uint[] ciph_32 = new uint[ciphertext.Length/4];
        for(int i = 0; i < ciph_32.Length; i++){
            byte[] CipherChunck = new byte[4];
            Array.Copy(ciphertext, i*4, CipherChunck, 0, 4);
            ciph_32[i] = BitConverter.ToUInt32(CipherChunck, 0);
        }
        for (int i = 0; i < ciph_32.Length; i++) {
            ciph_32[i] = dec(ciph_32[i]);
        }

        for (int i = 0; i < ciph_32.Length; i++) {
            byte[] plainChunck = BitConverter.GetBytes(ciph_32[i]);
            Array.Copy(plainChunck, 0, ciphertext, i*4, 4);

        }
        for (int i = ciphertext.Length-1; i >=0 ; i--) {

        }
        return ciphertext;
    }



    private uint enc(uint plain){
        ushort R = (ushort)(plain & 65535);
        ushort L = (ushort)((plain >> 16) & 65535);

        for (int i = 0; i < ROUNDS; i++) {
            if (i != ROUNDS - 1) {
                ushort t = L;
                switch((i*m)%4){

                    case 0:
                        L = (ushort)(R ^ (f_1(L, i)));
                        break;

                    case 1:
                        L = (ushort)(R ^ (f_2(L, i)));
                        break;
                    case 2:
                        L = (ushort)(R ^ (f_4(L, i)));
                    break;

                    case 3:
                        L = (ushort)(R ^ (f_3(L, i)));
                    break;
                }
                R = t;
            }
            else {
                switch((i*m)%4){

                    case 0:
                        R ^= ((f_1(L, i)));
                        break;

                    case 1:
                        R ^= ((f_2(L, i)));
                        break;
                    case 2:
                        R ^= ((f_4(L, i)));
                        break;

                    case 3:
                        R ^= ((f_3(L, i)));
                        break;
                }
            }
        }


            uint ciph = (((uint) L) << 16) | R;
            return ciph;

    }

    private uint dec(uint ciph){
        ushort R = (ushort)(ciph & 65535);
        ushort L = (ushort)((ciph >> 16) & 65535);


        for (int i = ROUNDS - 1; i >= 0; i--) {
            if (i != 0) {
                ushort t = L;
                switch ((i * m) % 4) {

                    case 0:
                        L = (ushort)(R ^ (f_1(L, i)));
                        break;

                    case 1:
                        L = (ushort)(R ^ (f_2(L, i)));
                        break;
                    case 2:
                        L = (ushort)(R ^ (f_4(L, i)));
                        break;

                    case 3:
                        L = (ushort)(R ^ (f_3(L, i)));
                        break;
                }
                R = t;
            }
            else {
                switch ((i * m) % 4) {

                    case 0:
                        R ^= ((f_1(L, i)));
                        break;

                    case 1:
                        R ^= ((f_2(L, i)));
                        break;
                    case 2:
                        R ^= ((f_4(L, i)));
                        break;

                    case 3:
                        R ^= ((f_3(L, i)));
                        break;
                }
            }
        }

        uint plain = (((uint) L) << 16) | R;
        return plain;
    }

    private ushort f_1(ushort input, int numround){
        ushort output = (ushort) (Math.Floor(input + 256 * Math.Cos(K1 + K2 * numround)));
        return output;
    }

    private ushort f_2(ushort input, int numround){
        ushort output = (ushort) (Math.Floor(input - 256 * Math.Sin(K1 + K2 * numround)) + 1);
        return output;
    }

    private ushort f_3(ushort input, int numround){
        ushort output = (ushort) (Math.Floor(input + 256 * Math.Sin(K1 + K2 * numround)) + 1);
        return output;
    }

    private ushort f_4(ushort input, int numround){
        ushort output = (ushort) (Math.Floor(input - 256 * Math.Cos(K1 + K2 * numround)));
        return output;
    }



}
