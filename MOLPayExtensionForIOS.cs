using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using System.IO;

using MPayXDKExample.iOS;
using MPayXDKExample;

[assembly: Xamarin.Forms.Dependency(typeof(MPExtension))]

namespace MPayXDKExample.iOS
{
	public class MPExtension : MOLPayExtension
	{
		public void SetMOLPayContext(Object mpActivity)
		{
			// iOS does nothing
		}

		public String GetAssetPath()
		{
			return NSBundle.MainBundle.BundlePath;
		}

		public void saveImageToDevice(String base64ImageString, String filename)
		{
			System.Diagnostics.Debug.WriteLine("+++++++++++ saveImageToDevice, base64ImageString = {0}", base64ImageString);
			System.Diagnostics.Debug.WriteLine("+++++++++++ saveImageToDevice, filename = {0}", filename);

			byte[] imageData = System.Convert.FromBase64String(base64ImageString.ToString());
			NSData data = NSData.FromArray(imageData);
			UIImage img = UIImage.LoadFromData(data);
			img.SaveToPhotosAlbum((image, error) =>
			{
				//var o = image as UIImage;
				if (error == null || error.ToString() == "")
				{
					UIAlertView alert = new UIAlertView()
					{
						Title = "Info",
						Message = "Image saved"
					};
					alert.AddButton("OK");
					alert.Show();
				}
				else
				{
					UIAlertView alert = new UIAlertView()
					{
						Title = "Info",
						Message = "Image not saved"
					};
					alert.AddButton("OK");
					alert.Show();
				}
			});
		}

		private static String Base64Encode(String plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}

		private static String Base64Decode(String base64EncodedData)
		{
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}

	}


}
