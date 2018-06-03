using System;
using Cryptography.ECDSA;
using System.Security.Cryptography;

namespace Security.Cryptography {
	public class RandomSha {
		public static string serverSeed = "55200d8d7bd2bec7debd261536dba0b0f4ed341ef79116bbd03300afb397e731501c4037b5f8c7125d992a7949897dc3e4774901be55d839d6febaee566eaec8";
		public static string clientSeed = "d3fe899f7ff7cce35d1d7389cee0e4a163f59ebb";

		public static uint lucky;
		public static float luckyNumber;
		public static string seed;
		public static int nonce;

		public static byte[] SHA1(byte[] bytes)
		{
			using (SHA1Managed sha1 = new SHA1Managed())
			{
				byte[] hash = sha1.ComputeHash(bytes);
				return hash;
			}
		}

		public static byte[] SHA512(byte[] bytes)
		{
			using (SHA512Managed sha512 = new SHA512Managed())
			{
				byte[] hash = sha512.ComputeHash(bytes);
				return hash;
			}
		}

		public static string SHA1(string str)
		{
			return System.BitConverter.ToString(SHA1(System.Text.Encoding.UTF8.GetBytes(str))).Replace("-", "").ToLower();
		}

		public static string SHA512(string str)
		{
			return System.BitConverter.ToString(SHA512(System.Text.Encoding.UTF8.GetBytes(str))).Replace("-", "").ToLower();
		}

		public static void NewServerSeed() 
		{
			nonce = 1;

			serverSeed = SHA512(serverSeed + UnityEngine.Random.value.ToString());
			clientSeed = SHA512(clientSeed + UnityEngine.Random.value.ToString());
		}

		public static float GetRoll() {
			seed = serverSeed + '-' + clientSeed + '-' + nonce;
			do {
				seed = SHA1(seed);

				lucky = (uint)int.Parse(seed.Substring(0, 8), System.Globalization.NumberStyles.HexNumber);
			} while (lucky > 4294960000);

			luckyNumber = (lucky % 10000);

			if (luckyNumber < 0) {
				luckyNumber = -luckyNumber;
			}

			nonce++;

			return luckyNumber;
		}

        public static byte[] GenerateRandomKey()
        {
            var rand = new System.Random(DateTime.Now.Millisecond);

            for (int i=0; i < rand.Next(3, 17); i++)
            {
                NewServerSeed();
            }

            var key = Hex.HexToBytes(SHA1(GetRoll().ToString()));

            return key;
        }
    }
}