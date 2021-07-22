using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace AppCrypto
{

	public class AppCrypto
	{

		// Clase base
		public abstract class CryptoMain
		{
			public abstract void New(System.String[] args);

			public abstract MensajeOut EncriptarMsg(MensajeIn MensajeIn , Proceso Proceso);
		}

		private static bool XMLValido(string Cadena)
		{

			bool Retorno;
			XmlDocument xmlTest;

			Retorno = false;
			xmlTest = new XmlDocument();

			try
			{
				xmlTest.LoadXml(Cadena);
				Retorno = true;
			}
			catch (Exception)
			{
				Retorno = false;
			}

			return Retorno;

		}

		public class MensajeIn
		{
			public string CadenaMsg { get; set; }
			public bool esXML { get; set; }
		}

		public class MensajeOut
		{
			public string CadenaMsg { get; set; }
			public bool esXML { get; set; }
			public bool esError { get; set; }
		}

		public enum Proceso
		{
			Encriptar = 0x1 ,
			Desencriptar = 0x2 ,
		}


		// Implementación de las clases

		// AES
		public class CryptoAES : CryptoMain
		{

			private byte[] _key;
			private byte[] _iv;

			// key = Llave principal, iv = Llave secundaria
			public override void New(System.String[] args)
			{
				_key = Encoding.ASCII.GetBytes(args[0]);
				_iv = Encoding.ASCII.GetBytes(args[0]);
			}

			public override MensajeOut EncriptarMsg(MensajeIn MensajeIn, Proceso Proceso)
			{

				MensajeOut Retorno;

				Retorno = new MensajeOut();

				try
				{
					if (Proceso == Proceso.Encriptar)
						Retorno = Encriptar(MensajeIn);
					else
						Retorno = Desencriptar(MensajeIn);
				}
				catch (System.Exception)
				{
					Retorno.esError = true;
				}

				return Retorno;

			}

			private MensajeOut Encriptar(MensajeIn msg)
			{

				MensajeOut Retorno;

				Retorno = new MensajeOut();
				Retorno.esError = false;

				try 
				{	        
					Retorno.CadenaMsg = Crypt(msg.CadenaMsg);
					Retorno.esError = false;
				}
				catch (System.Exception)
				{
					Retorno.esError=true;
				}

				if (msg.esXML == true)
					if (XMLValido(msg.CadenaMsg) == false)
						Retorno.esError = true;

				return Retorno;

			}

			private MensajeOut Desencriptar(MensajeIn msg)
			{

				MensajeOut Retorno;

				Retorno = new MensajeOut();
				Retorno.esError = false;

				try
				{
					Retorno.CadenaMsg = Decrypt(msg.CadenaMsg);
					Retorno.esError = false;
				}
				catch (System.Exception)
				{
					Retorno.esError = true;
				}

				if (msg.esXML == true)
					if (XMLValido(msg.CadenaMsg) == false)
						Retorno.esError = true;

				return Retorno;

			}

			private string Crypt(string Cadena)
			{

				string Retorno;
				byte[] bCadena;
				byte[] sEncripted;
				RijndaelManaged Cripto;

				Retorno = String.Empty;
				Cripto = new RijndaelManaged();
				bCadena = Encoding.ASCII.GetBytes(Cadena);

				using (MemoryStream mStream = new MemoryStream(bCadena.Length))
				{
					using (CryptoStream objCryptoStream = new CryptoStream(mStream, Cripto.CreateEncryptor(_key, _iv), CryptoStreamMode.Write))
					{
						objCryptoStream.Write(bCadena, 0, bCadena.Length);
						objCryptoStream.FlushFinalBlock();
						objCryptoStream.Close();
					}

					sEncripted = mStream.ToArray();

				}

				Retorno = Convert.ToBase64String(sEncripted);

				return Retorno;

			}

			private string Decrypt(string Cadena)
			{

				string Retorno;
				byte[] bCadena;
				string sCadena;
				RijndaelManaged Cripto;

				Retorno = String.Empty;
				sCadena = string.Empty;
				Cripto = new RijndaelManaged();
				bCadena = Convert.FromBase64String(Cadena);

				using (MemoryStream mStream = new MemoryStream(bCadena))
				{
					using (CryptoStream objCryptoStream = new CryptoStream(mStream , Cripto.CreateDecryptor(_key , _iv) , CryptoStreamMode.Read))
					{
						using (StreamReader mStreamReader = new StreamReader(objCryptoStream, true))
						{
							sCadena = mStreamReader.ReadToEnd();
						}
					}

				}

				Retorno = sCadena;

				return Retorno;

			}

		}

	}
}
